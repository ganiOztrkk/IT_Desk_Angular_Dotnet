using Microsoft.AspNetCore.Http;
#nullable enable

namespace IT_DeskServer.Business.DTOs;

public sealed record TicketAddDto(
    string Subject,
    string Summary,
    List<IFormFile>? Files
    );