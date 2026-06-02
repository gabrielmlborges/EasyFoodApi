using System.ComponentModel.DataAnnotations;

namespace EasyFood.DTOs.Auth;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Senha
);
