using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasyFood.Data;
using EasyFood.DTOs.Auth;
using EasyFood.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EasyFood.Services;

public class AuthService(AppDbContext db, IConfiguration config) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await db.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive)
            ?? throw new UnauthorizedAccessException("Credenciais inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        var expiresInHours = config.GetValue<int>("Jwt:ExpiresInHours");
        var expiresAt = DateTime.UtcNow.AddHours(expiresInHours);
        var token = GerarToken(usuario.Id, usuario.Email, usuario.Role.ToString(), expiresAt);

        return new LoginResponse(token, expiresAt, usuario.Nome, usuario.Email, usuario.Role.ToString());
    }

    private string GerarToken(int id, string email, string role, DateTime expiresAt)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
