namespace JwtUserLogin.API.DTOs;

public record LoginDto(
    byte[] UserName,
    byte[] UserPassword
);
