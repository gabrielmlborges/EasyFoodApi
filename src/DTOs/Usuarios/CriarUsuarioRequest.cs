using System.ComponentModel.DataAnnotations;

namespace EasyFood.DTOs.Usuarios;

public record CriarUsuarioRequest(
    [Required, MaxLength(100)] string Nome,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Senha
);
