using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Core.ResultPattern;
using IT_DeskServer.Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace IT_DeskServer.Business.Services;

public interface ITicketService
{
    public Task<IResult> AddAsync([FromForm]TicketAddDto request, CancellationToken cancellationToken);
    
    public Task<IDataResult<List<TicketResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
}