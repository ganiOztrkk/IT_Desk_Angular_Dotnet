using System.Security.Claims;
using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Business.Services;
using IT_DeskServer.Core.ResultPattern;
using IT_DeskServer.DataAccess.Context;
using IT_DeskServer.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IResult = IT_DeskServer.Core.ResultPattern.IResult;

namespace IT_DeskServer.DataAccess.Services;

public class TicketService
    (IHttpContextAccessor httpContextAccessor, ApplicationDbContext context): ITicketService
{
    public async Task<IResult> AddAsync([FromForm]TicketAddDto request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return new ErrorResult("Access Token bulunamadı.");
        var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (userId is null) return new ErrorResult("Kullanıcı bulunamadı.");

        var ticket = new Ticket
        {
            AppUserId = Guid.Parse(userId),
            Subject = request.Subject,
            CreateDate = DateTime.Now,
            IsActive = true
        };
        if (request.Files is not null)
        {
            foreach (var item in request.Files)
            {
                var fileFormat = item.FileName.Substring(item.FileName.LastIndexOf('.'));
                var fileName = Guid.NewGuid().ToString() + fileFormat;
                await using var stream = System.IO.File.Create(@"/Users/gani/Desktop/Github/IT_Desk/IT_DeskClient/src/assets/files/" + fileName);
                await item.CopyToAsync(stream, cancellationToken);

                var ticketFile = new TicketFile
                {
                    TicketId = ticket.Id,
                    FileUrl = fileName
                };
                ticket.FileUrl.Add(ticketFile);
            }
        }

        var ticketDetail = new TicketDetail
        {
            TicketId = ticket.Id,
            AppUserId = Guid.Parse(userId),
            Content = request.Summary,
            CreateDate = ticket.CreateDate
        };

        await context.Set<TicketDetail>().AddAsync(ticketDetail, cancellationToken);
        await context.Set<Ticket>().AddAsync(ticket, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return new SuccessResult("Destek Talebi Oluşturuldu.");
    }

    public async Task<IDataResult<List<TicketResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return new ErrorDataResult<List<TicketResponseDto>>(null ,"Access Token bulunamadı.");
        var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (userId is null) return new ErrorDataResult<List<TicketResponseDto>>(null, "Kullanıcı bulunamadı.");
        
        var role = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        if (role.Contains("admin"))
        {
            var allTickets = await context.Set<Ticket>()
                .Select(x => new TicketResponseDto(x.Id, x.Subject, x.CreateDate, x.IsActive))
                .ToListAsync(cancellationToken);
            return new SuccessDataResult<List<TicketResponseDto>>(allTickets);
        }
        
        var tickets = await context.Set<Ticket>()
            .Where(x => x.AppUserId == Guid.Parse(userId))
            .Select(x => new TicketResponseDto(x.Id, x.Subject, x.CreateDate, x.IsActive))
            .ToListAsync(cancellationToken);
        return new SuccessDataResult<List<TicketResponseDto>>(tickets);
    }

    public async Task<IDataResult<List<TicketDetail>>> GetDetailsAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        var details = await context.Set<TicketDetail>()
            .Where(x => x.TicketId == ticketId)
            .Include(x => x.AppUser)
            .OrderBy(x => x.CreateDate)
            .ToListAsync(cancellationToken);

        if (details.IsNullOrEmpty()) return new ErrorDataResult<List<TicketDetail>>(null, "Destek talebi bulunamadı.");

        return new SuccessDataResult<List<TicketDetail>>(details);
    }

    public async Task<IDataResult<Ticket>> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        var ticket = await context.Set<Ticket>()
            .Include(x => x.AppUser)
            .Include(x => x.FileUrl)
            .FirstOrDefaultAsync(x => x.Id == ticketId, cancellationToken);

        if (ticket is null) return new ErrorDataResult<Ticket>(null, "Destek talebi bulunamadı.");
        
        return new SuccessDataResult<Ticket>(ticket);
    }

    public async Task<IResult> AddDetailContentAsync(TicketDetailDto request, CancellationToken cancellationToken)
    {
        var ticket = await context.Set<Ticket>().FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);
        var ticketDetail = new TicketDetail
        {
            TicketId = request.TicketId,
            AppUserId = request.AppUserId,
            Content = request.Content,
            CreateDate = DateTime.Now
        };
        if (ticket is not null) ticket.IsActive = true;
        
        await context.Set<TicketDetail>().AddAsync(ticketDetail, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new SuccessResult("Mesaj gönderildi.");
    }

    public async Task<IResult> CloseAsync(Guid ticketId, CancellationToken cancellationToken)
    {
        var ticket = await context.Set<Ticket>().FindAsync(ticketId, cancellationToken);
        if (ticket is null) return new ErrorResult("Destek bulunamadı.");

        ticket.IsActive = false;
        await context.SaveChangesAsync(cancellationToken);
        return new SuccessResult("Destek kapandı.");
    }
}