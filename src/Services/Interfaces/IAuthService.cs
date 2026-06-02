using EasyFood.DTOs.Auth;

namespace EasyFood.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
