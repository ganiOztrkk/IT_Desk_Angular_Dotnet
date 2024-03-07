using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Business.Services;
using IT_DeskServer.WebApi.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_DeskServer.WebApi.Controllers;

[AllowAnonymous]
public class AuthController(IAuthService authService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        var loginResult = await authService.LoginAsync(request, cancellationToken);
        if (!loginResult.Success) return BadRequest(new{message = loginResult.Message});
        return Ok(new{accessToken = loginResult.Data, message= loginResult.Message});
    }
}