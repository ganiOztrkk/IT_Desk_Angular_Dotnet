using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Business.Services;
using IT_DeskServer.WebApi.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace IT_DeskServer.WebApi.Controllers;

public class TicketsController(ITicketService ticketService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromForm]TicketAddDto request, CancellationToken cancellationToken)
    {
        var result = await ticketService.AddAsync(request, cancellationToken);
        return Ok(new{message = result.Message});
    }
    
    [HttpPost]
    public async Task<IActionResult> AddDetailContent(TicketDetailDto request, CancellationToken cancellationToken)
    {
        var result = await ticketService.AddDetailContentAsync(request, cancellationToken);
        return Ok(new{message = result.Message});
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await ticketService.GetAllAsync(cancellationToken);
        return Ok(result.Data);
    }
    
    [HttpGet("{ticketId}")]
    public async Task<IActionResult> GetDetails(Guid ticketId, CancellationToken cancellationToken)
    {
        var result = await ticketService.GetDetailsAsync(ticketId ,cancellationToken);
        return Ok(result.Data);
    }
    
    [HttpGet("by-id/{ticketId}")]
    public async Task<IActionResult> GetById(Guid ticketId, CancellationToken cancellationToken)
    {
        var result = await ticketService.GetByIdAsync(ticketId ,cancellationToken);
        return Ok(result.Data);
    }
    
    [HttpGet("close-ticket/{ticketId}")]
    public async Task<IActionResult> Close(Guid ticketId, CancellationToken cancellationToken)
    {
        var result = await ticketService.CloseAsync(ticketId ,cancellationToken);
        return Ok(new{message = result.Message});
    }
}