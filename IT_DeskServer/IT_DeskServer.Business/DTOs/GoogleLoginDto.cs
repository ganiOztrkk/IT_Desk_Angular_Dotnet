namespace IT_DeskServer.Business.DTOs;

public sealed record GoogleLoginDto(
    string Id,
    string IdToken,
    string Email,
    string FirstName,
    string LastName,
    string Name,
    string PhotoUrl
    );