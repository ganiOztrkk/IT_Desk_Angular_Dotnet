using System.Security.Claims;
using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Business.Services;
using IT_DeskServer.Core.ResultPattern;
using IT_DeskServer.DataAccess.Context;
using IT_DeskServer.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        return new SuccessResult("Ticket Oluşturuldu.");
    }

    public async Task<IDataResult<List<TicketResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return new ErrorDataResult<List<TicketResponseDto>>(null ,"Access Token bulunamadı.");
        var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (userId is null) return new ErrorDataResult<List<TicketResponseDto>>(null, "Kullanıcı bulunamadı.");
        
        
        var tickets = await context.Set<Ticket>()
            .Where(x => x.AppUserId == Guid.Parse(userId))
            .Select(x => new TicketResponseDto(x.Id, x.Subject, x.CreateDate, x.IsActive))
            .ToListAsync(cancellationToken);
        return new SuccessDataResult<List<TicketResponseDto>>(tickets);
    }
}