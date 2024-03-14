namespace IT_DeskServer.Business.Services;

public interface IGoogleTokenVerifier
{
    public Task<bool> VerifyTokenAsync(string idToken);
}