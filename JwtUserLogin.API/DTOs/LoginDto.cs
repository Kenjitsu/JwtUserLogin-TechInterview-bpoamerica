namespace JwtUserLogin.API.DTOs;

public record LoginDto(
    string UserName,
    string UserPassword,
    string ClientIp,
    string UserAgent
);
