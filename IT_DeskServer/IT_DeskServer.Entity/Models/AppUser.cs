using IT_DeskServer.Entity.Abstract;
using Microsoft.AspNetCore.Identity;

namespace IT_DeskServer.Entity.Models;

public sealed class AppUser : IdentityUser<Guid>, IEntity
{
    public string Name { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public int WrongTryCount { get; set; } = 0;
    public DateTime LockOutDate { get; set; } = DateTime.Now;
}