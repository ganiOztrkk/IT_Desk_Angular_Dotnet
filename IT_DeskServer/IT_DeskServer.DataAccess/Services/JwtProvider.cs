using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IT_DeskServer.Core.ResultPattern;
using IT_DeskServer.Entity.Abstract;
using IT_DeskServer.Entity.Models;
using IT_DeskServer.Entity.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IT_DeskServer.DataAccess.Services;

public class JwtProvider(IOptions<Jwt> jwt) : IJwtProvider //jwt sınıfını ioptions ile çağırıyoruz
{
    public Task<IDataResult<string>> CreateTokenAsync(AppUser user, List<string> roles, bool rememberMe)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName ?? string.Empty),
            new Claim("userId", user.Id.ToString()),
            new Claim("userFullName", string.Join(" ", user.Name, user.Lastname)),
            new Claim("username", user.UserName ?? string.Empty),
            new Claim("roles", string.Join(" ", roles))
        };

        var tokenExpires = rememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30);

        var securityToken = new JwtSecurityToken(
        issuer: jwt.Value.Issuer,
        audience: jwt.Value.Audience,
        claims: claims,
        notBefore: DateTime.Now,
        expires: tokenExpires,
        signingCredentials: 
        new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Value.SecretKey)), SecurityAlgorithms.HmacSha512));
        
        JwtSecurityTokenHandler handler = new();
        var token = handler.WriteToken(securityToken);

        return Task.FromResult<IDataResult<string>>(new SuccessDataResult<string>(token));
    }
}