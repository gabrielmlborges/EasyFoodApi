using EasyFood.Data;
using EasyFood.DTOs.Usuarios;
using EasyFood.Models;
using EasyFood.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyFood.Services;

public class UsuarioService(AppDbContext db) : IUsuarioService
{
    public async Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request)
    {
        var existe = await db.Usuarios.AnyAsync(u => u.Email == request.Email);
        if (existe)
            throw new ArgumentException("E-mail já cadastrado.");

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            Role = RoleUsuario.Cliente
        };

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();

        return ToResponse(usuario);
    }

    public async Task<UsuarioResponse> ObterPorIdAsync(int id)
    {
        var usuario = await db.Usuarios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado.");

        return ToResponse(usuario);
    }

    public async Task<List<UsuarioResponse>> ListarAsync()
    {
        return await db.Usuarios
            .Select(u => ToResponse(u))
            .ToListAsync();
    }

    public async Task DesativarAsync(int id)
    {
        var usuario = await db.Usuarios.FindAsync(id)
            ?? throw new KeyNotFoundException($"Usuário {id} não encontrado.");

        usuario.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static UsuarioResponse ToResponse(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.Role.ToString(), u.IsActive);
}
