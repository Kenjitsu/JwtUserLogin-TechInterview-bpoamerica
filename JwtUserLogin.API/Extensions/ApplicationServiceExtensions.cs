using JwtUserLogin.API.Data;
using JwtUserLogin.API.Interfaces;
using JwtUserLogin.API.Services;
using JwtUserLogin.API.Settings;
using Microsoft.EntityFrameworkCore;

namespace JwtUserLogin.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<ISecurityService, SecurityService>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowClientApp", policy =>
            {
                policy.WithOrigins("https://localhost:7020")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }
}
