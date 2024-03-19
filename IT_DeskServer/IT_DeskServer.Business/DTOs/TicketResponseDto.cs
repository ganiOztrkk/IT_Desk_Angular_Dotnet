namespace IT_DeskServer.Business.DTOs;

public sealed record TicketResponseDto(
    Guid Id,
    string Subject,
    DateTime CreateDate,
    bool IsActive
    );