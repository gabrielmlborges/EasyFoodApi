using EasyFood.Data;
using EasyFood.DTOs.Produtos;
using EasyFood.Models;
using EasyFood.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyFood.Services;

public class ProdutoService(AppDbContext db) : IProdutoService
{
    public async Task<ProdutoResponse> CriarAsync(CriarProdutoRequest request)
    {
        var produto = new Produto
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            UrlImagem = request.UrlImagem,
            Preco = request.Preco
        };

        db.Produtos.Add(produto);
        await db.SaveChangesAsync();

        return ToResponse(produto);
    }

    public async Task<ProdutoResponse> ObterPorIdAsync(int id)
    {
        var produto = await db.Produtos.FindAsync(id)
            ?? throw new KeyNotFoundException($"Produto {id} não encontrado.");

        return ToResponse(produto);
    }

    public async Task<List<ProdutoResponse>> ListarAsync(bool apenasAtivos = true)
    {
        return await db.Produtos
            .Where(p => !apenasAtivos || p.IsActive)
            .Select(p => ToResponse(p))
            .ToListAsync();
    }

    public async Task<ProdutoResponse> AtualizarAsync(int id, AtualizarProdutoRequest request)
    {
        var produto = await db.Produtos.FindAsync(id)
            ?? throw new KeyNotFoundException($"Produto {id} não encontrado.");

        if (request.Nome is not null) produto.Nome = request.Nome;
        if (request.Descricao is not null) produto.Descricao = request.Descricao;
        if (request.UrlImagem is not null) produto.UrlImagem = request.UrlImagem;
        if (request.Preco is not null) produto.Preco = request.Preco.Value;
        if (request.IsActive is not null) produto.IsActive = request.IsActive.Value;

        await db.SaveChangesAsync();

        return ToResponse(produto);
    }

    private static ProdutoResponse ToResponse(Produto p) =>
        new(p.Id, p.Nome, p.Descricao, p.UrlImagem, p.Preco, p.IsActive);
}
