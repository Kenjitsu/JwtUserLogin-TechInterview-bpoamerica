namespace JwtUserLogin.client.Models;

public record LoginResponse(int IdUser, string UserName, string UserProfile, DateTime LastLoginDate);