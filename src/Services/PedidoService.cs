using EasyFood.Data;
using EasyFood.DTOs.Pedidos;
using EasyFood.Models;
using EasyFood.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyFood.Services;

public class PedidoService(AppDbContext db) : IPedidoService
{
    public async Task<PedidoResponse> CriarAsync(int usuarioId, CriarPedidoRequest request)
    {
        var produtoIds = request.Itens.Select(i => i.ProdutoId).Distinct().ToList();
        var produtos = await db.Produtos
            .Where(p => produtoIds.Contains(p.Id) && p.IsActive)
            .ToListAsync();

        if (produtos.Count != produtoIds.Count)
            throw new ArgumentException("Um ou mais produtos não foram encontrados ou estão inativos.");

        var itens = request.Itens.Select(i =>
        {
            var produto = produtos.First(p => p.Id == i.ProdutoId);
            return new PedidoProduto
            {
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = produto.Preco
            };
        }).ToList();

        var pedido = new Pedido
        {
            UsuarioId = usuarioId,
            PedidoProdutos = itens,
            Total = itens.Sum(i => i.PrecoUnitario * i.Quantidade)
        };

        db.Pedidos.Add(pedido);
        await db.SaveChangesAsync();

        return await CarregarPedidoResponse(pedido.Id);
    }

    public async Task<PedidoResponse> ObterPorIdAsync(int id, int usuarioId, bool isAdmin)
    {
        var pedido = await db.Pedidos.FindAsync(id)
            ?? throw new KeyNotFoundException($"Pedido {id} não encontrado.");

        if (!isAdmin && pedido.UsuarioId != usuarioId)
            throw new UnauthorizedAccessException("Acesso negado a este pedido.");

        return await CarregarPedidoResponse(id);
    }

    public async Task<List<PedidoResponse>> ListarAsync(int usuarioId)
    {
        var pedidos = await db.Pedidos
            .Include(p => p.PedidoProdutos)
            .ThenInclude(pp => pp.Produto)
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<List<PedidoResponse>> ListarTodosAsync()
    {
        var pedidos = await db.Pedidos
            .Include(p => p.PedidoProdutos)
            .ThenInclude(pp => pp.Produto)
            .ToListAsync();

        return pedidos.Select(MapearResponse).ToList();
    }

    public async Task<PedidoResponse> AtualizarStatusAsync(int id, AtualizarStatusRequest request)
    {
        var pedido = await db.Pedidos.FindAsync(id)
            ?? throw new KeyNotFoundException($"Pedido {id} não encontrado.");

        if (!Enum.TryParse<StatusPedido>(request.Status, ignoreCase: true, out var novoStatus))
            throw new ArgumentException($"Status '{request.Status}' inválido.");

        pedido.Status = novoStatus;
        await db.SaveChangesAsync();

        return await CarregarPedidoResponse(id);
    }

    private async Task<PedidoResponse> CarregarPedidoResponse(int pedidoId)
    {
        var pedido = await db.Pedidos
            .Include(p => p.PedidoProdutos)
            .ThenInclude(pp => pp.Produto)
            .FirstAsync(p => p.Id == pedidoId);

        return MapearResponse(pedido);
    }

    private static PedidoResponse MapearResponse(Pedido pedido) =>
        new(
            pedido.Id,
            pedido.UsuarioId,
            pedido.Status.ToString(),
            pedido.Total,
            pedido.IsActive,
            pedido.CriadoEm,
            pedido.AtualizadoEm,
            pedido.PedidoProdutos.Select(pp => new ItemPedidoResponse(
                pp.ProdutoId,
                pp.Produto.Nome,
                pp.Quantidade,
                pp.PrecoUnitario,
                pp.PrecoUnitario * pp.Quantidade
            )).ToList()
        );
}
