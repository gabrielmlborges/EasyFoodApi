using EasyFood.DTOs.Pedidos;

namespace EasyFood.Services.Interfaces;

public interface IPedidoService
{
    Task<PedidoResponse> CriarAsync(int usuarioId, CriarPedidoRequest request);
    Task<PedidoResponse> ObterPorIdAsync(int id, int usuarioId, bool isAdmin);
    Task<List<PedidoResponse>> ListarAsync(int usuarioId, bool isAdmin);
    Task<PedidoResponse> AtualizarStatusAsync(int id, AtualizarStatusRequest request);
}
