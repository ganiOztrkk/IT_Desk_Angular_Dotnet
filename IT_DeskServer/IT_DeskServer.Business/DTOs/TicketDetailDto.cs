namespace IT_DeskServer.Business.DTOs;

public class TicketDetailDto
{
    public Guid TicketId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid AppUserId { get; set; }
}