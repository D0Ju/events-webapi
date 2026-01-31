using Microsoft.AspNetCore.Mvc;
using events_webapi.Services;

namespace events_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AuthService _authService;

    public AccountController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request?.Password))
            return BadRequest(new { message = "Email i lozinka su obavezni!" });

        var (success, message, user) = await _authService.RegisterAsync(request.Email, request.Password);

        if (!success)
            return BadRequest(new { message });

        return Ok(new { message = "Registracija uspješna!", userId = user.Id, email = user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request?.Password))
            return BadRequest(new { message = "Email i lozinka su obavezni!" });

        var (success, token, message) = await _authService.LoginAsync(request.Email, request.Password);

        if (!success)
            return Unauthorized(new { message });

        return Ok(new { token, message = "Prijava uspješna!" });
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;  
    public string Password { get; set; } = string.Empty;  
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty; 
    public string Password { get; set; } = string.Empty;
}
