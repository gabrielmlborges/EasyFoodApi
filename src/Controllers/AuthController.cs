using EasyFood.DTOs.Auth;
using EasyFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyFood.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        return Ok(response);
    }
}
