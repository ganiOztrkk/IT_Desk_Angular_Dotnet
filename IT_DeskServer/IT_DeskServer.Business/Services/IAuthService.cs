using IT_DeskServer.Business.DTOs;
using IT_DeskServer.Core.ResultPattern;

namespace IT_DeskServer.Business.Services;

public interface IAuthService
{
    public Task<IDataResult<string>> LoginAsync(LoginDto request, CancellationToken cancellationToken);
    public Task<IDataResult<string>> GoogleLoginAsync(GoogleLoginDto request, CancellationToken cancellationToken);
}