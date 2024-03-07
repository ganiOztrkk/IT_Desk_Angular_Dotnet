using IT_DeskServer.Entity.Abstract;
using Microsoft.AspNetCore.Identity;

namespace IT_DeskServer.Entity.Models;

public sealed class AppRole : IdentityRole<Guid>, IEntity
{
    
}