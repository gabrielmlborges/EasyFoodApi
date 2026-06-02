namespace EasyFood.DTOs.Usuarios;

public record UsuarioResponse(
    int Id,
    string Nome,
    string Email,
    string Role,
    bool IsActive
);
