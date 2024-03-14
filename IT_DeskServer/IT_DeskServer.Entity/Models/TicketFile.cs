using IT_DeskServer.Entity.Abstract;

namespace IT_DeskServer.Entity.Models;

public sealed class TicketFile : IEntity
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public string FileUrl { get; set; } = string.Empty;
}