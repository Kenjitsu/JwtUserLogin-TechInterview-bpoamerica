using JwtUserLogin.client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JwtUserLogin.client.Pages;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<LoginModel> _logger;

    private const string ApiClientName = "ApiBackend";
    private const string LoginEndpoint = "/api/security/loginuser";
    private const string AuthHeader = "Authorization";
    private const string BearerPrefix = "Bearer ";
    private const string TempDataKey = "ApiResponse";
    private const string JwtClaimType = "JwtToken";

    public LoginModel(IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "The password is required.")]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var loginPayload = BuildLoginRequest();
            var (isSuccess, userResponse, jwtToken) = await ExecuteLoginAsync(loginPayload);

            if (!isSuccess || userResponse == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials. Please check your data.");
                return Page();
            }

            await SignInUserAsync(userResponse.UserName, jwtToken);
            SaveResponseToTempData(userResponse);

            return RedirectToPage("/Index");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error connecting to the authentication API.");
            ModelState.AddModelError(string.Empty, "The authentication service is currently unavailable.");
            return Page();
        }
    }

    private object BuildLoginRequest()
    {
        return new
        {
            userName = ComputeSha256(Email),
            userPassword = ComputeSha256(Password),
            clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
            userAgent = Request.Headers.UserAgent.ToString()
        };
    }

    private async Task<(bool IsSuccess, LoginResponse? User, string JwtToken)> ExecuteLoginAsync(object payload)
    {
        var client = _httpClientFactory.CreateClient(ApiClientName);
        var response = await client.PostAsJsonAsync(LoginEndpoint, payload);

        if (!response.IsSuccessStatusCode)
            return (false, null, string.Empty);

        var user = await response.Content.ReadFromJsonAsync<LoginResponse>();
        var token = GetBearerToken(response);

        return (true, user, token);
    }

    private string GetBearerToken(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues(AuthHeader, out var headers))
        {
            var token = headers.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(token) && token.StartsWith(BearerPrefix))
            {
                return token[BearerPrefix.Length..];
            }
        }
        return string.Empty;
    }

    private async Task SignInUserAsync(string userName, string jwtToken)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userName),
            new(JwtClaimType, jwtToken)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
    }

    private void SaveResponseToTempData(LoginResponse user)
    {
        TempData[TempDataKey] = JsonSerializer.Serialize(user);
    }

    private static string ComputeSha256(string rawData)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData.Trim()));
        return Convert.ToHexString(hashBytes);
    }
}
