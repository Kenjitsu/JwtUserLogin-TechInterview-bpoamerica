namespace JwtUserLogin.API.DTOs;

public record LoginResponseDto
{
    public int IdUser { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string UserProfile { get; init; } = string.Empty;
    public DateTime LastLoginDate { get; init; }
}
