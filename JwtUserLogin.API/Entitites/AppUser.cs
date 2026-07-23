using System.ComponentModel.DataAnnotations;

namespace JwtUserLogin.API.Entitites;

public class AppUser
{
    [Key]
    public int Id { get; set; }

    public byte[] UserNameHash { get; set; } = [];

    public byte[] UserPasswordHash { get; set; } = [];

    public string UserName { get; set; } = "";

    public string UserProfile { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public DateTime LastLoginDate { get; set; }
}
