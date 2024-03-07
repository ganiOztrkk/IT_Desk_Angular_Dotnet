namespace IT_DeskServer.Business.DTOs;

public sealed record LoginDto(
    string UsernameOrEmail,
    string Password
    );