using Google.Apis.Auth;
using IT_DeskServer.Business.Services;

namespace IT_DeskServer.DataAccess.Services;

public class GoogleTokenVerifier : IGoogleTokenVerifier
{
    public async Task<bool> VerifyTokenAsync(string idToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            return true; // Token geçerli
        }
        catch
        {
            return false; // Token geçersiz
        }
    }
}