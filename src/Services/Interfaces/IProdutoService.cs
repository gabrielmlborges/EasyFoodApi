using EasyFood.DTOs.Produtos;

namespace EasyFood.Services.Interfaces;

public interface IProdutoService
{
    Task<ProdutoResponse> CriarAsync(CriarProdutoRequest request);
    Task<ProdutoResponse> ObterPorIdAsync(int id);
    Task<List<ProdutoResponse>> ListarAsync(bool apenasAtivos = true);
    Task<ProdutoResponse> AtualizarAsync(int id, AtualizarProdutoRequest request);
}
