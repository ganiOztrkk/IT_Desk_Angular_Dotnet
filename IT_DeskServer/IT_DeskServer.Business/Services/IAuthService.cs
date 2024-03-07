using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Core.ResultPattern;

namespace IT_DeskServer.Business.Services;

public interface IAuthService
{
    public Task<IResult> LoginAsync(LoginDto request, CancellationToken cancellationToken);
}