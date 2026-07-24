using JwtUserLogin.API.DTOs;
using JwtUserLogin.API.Entitites;

namespace JwtUserLogin.API.Extensions.Mappers;

public static class AppUserMapperExtension
{
    public static LoginResponseDto ToLoginResponseDto(this AppUser user)
    {
        return new LoginResponseDto
        {
            IdUser = user.Id,
            UserName = user.UserName,
            UserProfile = user.UserProfile,
            LastLoginDate = user.LastLoginDate
        };
    }
}
