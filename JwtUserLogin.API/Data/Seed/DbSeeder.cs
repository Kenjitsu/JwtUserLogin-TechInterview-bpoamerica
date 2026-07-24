using JwtUserLogin.API.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JwtUserLogin.API.Data.Seed;

public static class DbSeeder
{
    public static async Task Initialize(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var json = await File.ReadAllTextAsync("Data/Seed/seedData.json");

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        var usersToSeed = JsonSerializer.Deserialize<List<UserSeedDto>>(json, options);

        if(usersToSeed == null || usersToSeed.Count == 0)
        {
            Console.WriteLine("No users to seed.");
            return;
        }

        foreach (var u in usersToSeed)
        {
            await context.Users.AddAsync(new AppUser
            {
                UserNameHash = SHA256.HashData(Encoding.UTF8.GetBytes(u.UserName)),
                UserPasswordHash = SHA256.HashData(Encoding.UTF8.GetBytes(u.Password)),
                UserName = u.UserName,
                UserProfile = u.UserProfile,
                UserAgent = "Client user agent",
                LastLoginDate = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }
}

public record UserSeedDto
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string UserProfile { get; set; } = "";
}
