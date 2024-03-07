using IT_DeskServer.Entity.Models;
using Microsoft.AspNetCore.Identity;

namespace IT_DeskServer.WebApi.Middlewares;

public static class CreateDeveloperUserMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        
        if (userManager.Users.Any()) return;
        var user = new AppUser()
        {
            UserName = "developer",
            Email = "developer@developer.com",
            EmailConfirmed = true,
            Name = "developer",
            Lastname = "developer",
        };
        userManager.CreateAsync(user, "1234Aa").Wait();
    }
}