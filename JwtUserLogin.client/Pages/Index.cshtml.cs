using JwtUserLogin.client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JwtUserLogin.client.Pages;

[Authorize]
public class IndexModel : PageModel
{
    public string IdUser { get; set; } = "";
    public string UserName { get; set; } = "";
    public string UserProfile { get; set; } = "";
    public string LastLoginDate { get; set; } = "";

    public void OnGet()
    {
        if (TempData["ApiResponse"] is string jsonData)
        {
            var user = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(jsonData);

            if (user != null)
            {
                IdUser = user.IdUser.ToString();
                UserName = user.UserName;
                UserProfile = user.UserProfile;
                LastLoginDate = user.LastLoginDate.ToString("dd/MM/yyyy HH:mm:ss");
            }

            TempData.Keep("ApiResponse");
        }
    }
}