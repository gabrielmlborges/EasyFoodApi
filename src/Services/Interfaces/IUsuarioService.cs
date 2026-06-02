using EasyFood.DTOs.Usuarios;

namespace EasyFood.Services.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request);
    Task<UsuarioResponse> ObterPorIdAsync(int id);
    Task<List<UsuarioResponse>> ListarAsync();
    Task DesativarAsync(int id);
}
