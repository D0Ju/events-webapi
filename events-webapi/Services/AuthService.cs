using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using events_webapi.Data;
using events_webapi.Models;

namespace events_webapi.Services;

public class AuthService
{
    private readonly AppdbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppdbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<(bool Success, string Message, User User)> RegisterAsync(string email, string password)
    {
        // Check if user exists
        if (_context.Users.Any(u => u.Email == email))
            return (false, "Email već postoji!", null);

        // Validate password
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return (false, "Lozinka mora imati najmanje 6 znakova!", null);

        // Hash password
        var passwordHash = HashPassword(password);

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, "Registracija uspješna!", user);
    }

    public async Task<(bool Success, string Token, string Message)> LoginAsync(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return (false, null, "Pogrešan email ili lozinka!");

        if (!user.IsActive)
            return (false, null, "Korisnički račun je deaktiviran!");

        var token = GenerateJwtToken(user);
        return (true, token, "Prijava uspješna!");
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("userId", user.Id.ToString()),
                new System.Security.Claims.Claim("email", user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["JwtSettings:ExpirationMinutes"] ?? "1440")),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
