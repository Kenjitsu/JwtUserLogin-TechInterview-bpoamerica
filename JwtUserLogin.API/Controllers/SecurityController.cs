using JwtUserLogin.API.DTOs;
using JwtUserLogin.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtUserLogin.API.Controllers;

public class SecurityController : BaseApiController
{
    private readonly ISecurityService _securityService;

    public SecurityController(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    [HttpPost("LoginUser")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var result = await _securityService.LoginAsync(request);

        if (result == null) return Unauthorized(new { message = "Invalid username or password" });

        var (loginResponse, jwtToken) = result.Value;

        Response.Headers.Append("Authorization", $"Bearer {jwtToken}");

        return Ok(loginResponse);

    }
}
