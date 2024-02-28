using System.Text;
using IT_DeskServer.WebApi.Models;

namespace IT_DeskServer.WebApi.Services;

public static class PasswordService
{
    public static void CreatePassword(string password, out byte[] hash, out byte[] salt)
    {
        var hmac = new System.Security.Cryptography.HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    
    public static bool CheckPassword(User user, string password)
    {
        var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return false;
        }
        return true;
    }
}