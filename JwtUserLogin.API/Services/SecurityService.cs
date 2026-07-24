using JwtUserLogin.API.Data;
using JwtUserLogin.API.DTOs;
using JwtUserLogin.API.Entitites;
using JwtUserLogin.API.Extensions.Mappers;
using JwtUserLogin.API.Interfaces;
using JwtUserLogin.API.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtUserLogin.API.Services;

public class SecurityService : ISecurityService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly JwtSettings _jwtSettings;

    public SecurityService(AppDbContext context, IConfiguration configuration, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _configuration = configuration;
        _jwtSettings = jwtSettings.Value;
    }
    public async Task<(LoginResponseDto?, string? jwtToken)?> LoginAsync(LoginDto user)
    {
        byte[] userBytes = Convert.FromHexString(user.UserName);
        byte[] passBytes = Convert.FromHexString(user.UserPassword);      

        var validUser = await _context.Users
            .FirstOrDefaultAsync(u => u.UserNameHash.SequenceEqual(userBytes)
                                   && u.UserPasswordHash.SequenceEqual(passBytes));

        if (validUser == null) return null;

        var jwtToken = CreateJwtToken(validUser);
        await UpdateLastLoginTime(validUser);

        return (validUser.ToLoginResponseDto(), jwtToken);
        
    }

    private string CreateJwtToken(AppUser user)
    {
        var tokenKey = _jwtSettings.Key;

        if (tokenKey.Length < 64) throw new Exception("Your token key needs to be longer.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, user.UserProfile),
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private async Task UpdateLastLoginTime(AppUser user)
    {
        user.LastLoginDate = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
