namespace JwtUserLogin.client.Models;

public record LoginRequest
{
    public string UserName { get; init; } = string.Empty;
    public string UserPassword { get; init; } = string.Empty;
    public string ClientIp { get; init; } = string.Empty;
    public string UserAgent { get; init; } = string.Empty;

}
