using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Business.Services;
using IT_DeskServer.Core.ResultPattern;
using IT_DeskServer.Entity.Abstract;
using IT_DeskServer.Entity.Models;
using Microsoft.AspNetCore.Identity;

namespace IT_DeskServer.DataAccess.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<IDataResult<string>> LoginAsync(LoginDto request, CancellationToken cancellationToken)
    {
        var appUser = await userManager.FindByNameAsync(request.UsernameOrEmail);
        if (appUser is null)
        {
            appUser = await userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (appUser is null) return new ErrorDataResult<string>(null,"Kullanıcı adı hatalı");
        }

        if (appUser.WrongTryCount == 3)
        {
            var timeSpan = (appUser.LockOutDate - DateTime.Now).TotalMinutes;
            if (timeSpan <= 0)
            {
                appUser.WrongTryCount = 0;
                await userManager.UpdateAsync(appUser);
            }
            else return new ErrorDataResult<string>(null,$"3 kez şifrenizi yanlış girdiniz. {Math.Ceiling(timeSpan)} dakika daha beklemelisiniz");
        }

        var checkPassword = await userManager.CheckPasswordAsync(appUser, request.Password);
        if (checkPassword)
        {
            appUser.WrongTryCount = 0;
            await userManager.UpdateAsync(appUser);
            var token = await jwtProvider.CreateTokenAsync(appUser, request.HasRememberMe);
            return new SuccessDataResult<string>(token.Data, "Giriş başarılı.");
        }

        if (appUser.WrongTryCount < 3)
        {
            appUser.WrongTryCount++;
            await userManager.UpdateAsync(appUser);
        }
        
        if (appUser.WrongTryCount == 3)
        {
            appUser.LockOutDate = DateTime.Now.AddMinutes(3);
            await userManager.UpdateAsync(appUser);
            return new ErrorDataResult<string>(null, $"3 kez şifrenizi yanlış girdiniz. 3 dakika beklemelisiniz");
        }
        return new ErrorDataResult<string>(null,$"Şifre hatalı. {3-appUser.WrongTryCount} hakkınız kaldı");
    }
}