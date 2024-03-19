using IT_DeskServer.Entity.Abstract;

namespace IT_DeskServer.Entity.Models;

public sealed class Ticket : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public string Subject { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public bool IsActive { get; set; }
    public List<TicketFile> FileUrl { get; set; } = new();
}