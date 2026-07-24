using JwtUserLogin.API.DTOs;

namespace JwtUserLogin.API.Interfaces;

public interface ISecurityService
{
    Task<(LoginResponseDto?, string? jwtToken)?> LoginAsync(LoginDto user);
}
