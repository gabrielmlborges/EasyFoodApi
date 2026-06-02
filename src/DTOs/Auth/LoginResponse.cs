namespace EasyFood.DTOs.Auth;

public record LoginResponse(
    string Token,
    DateTime ExpiresAt,
    string Nome,
    string Email,
    string Role
);
