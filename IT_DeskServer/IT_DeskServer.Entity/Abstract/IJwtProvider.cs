using IT_DeskServer.Core.ResultPattern;
using IT_DeskServer.Entity.Models;

namespace IT_DeskServer.Entity.Abstract;

public interface IJwtProvider
{
    Task<IDataResult<string>> CreateTokenAsync(AppUser user, bool rememberMe); // geriye string result donen bir creatre token metodu yazdık
}