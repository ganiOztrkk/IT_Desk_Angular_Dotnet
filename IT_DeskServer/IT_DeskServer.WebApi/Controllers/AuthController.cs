using AutoMapper;
using FluentValidation;
using IT_DeskServer.WebApi.DTOs;
using IT_DeskServer.WebApi.Models;
using IT_DeskServer.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IT_DeskServer.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterDto> _validator;

    public AuthController(IMapper mapper, IValidator<RegisterDto> validator)
    {
        _mapper = mapper;
        _validator = validator;
    }

    [HttpPost]
    public IActionResult Register(RegisterDto request)
    {
        PasswordService.CreatePassword(request.Password, out var passwordHash, out var paswordSalt);
        
        var result = _validator.Validate(request);
        if (result.IsValid)
        {
            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = paswordSalt;
            return Ok(new { user = user, message = "Kayıt başarılı" });
        }
        else
        {
            var errors = new List<string>();
            result.Errors.ForEach(x =>
            {
                errors.Add(x.ErrorMessage);
            });
            return Ok(errors);
        }
    }
}
