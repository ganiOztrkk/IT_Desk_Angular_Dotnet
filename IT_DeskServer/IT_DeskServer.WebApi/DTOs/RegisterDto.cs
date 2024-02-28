namespace IT_DeskServer.WebApi.DTOs;

public sealed record RegisterDto(
    string Name,
    string Lastname,
    string Email,
    string Password
    );