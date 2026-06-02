using System.ComponentModel.DataAnnotations;

namespace EasyFood.DTOs.Produtos;

public record AtualizarProdutoRequest(
    [MaxLength(100)] string? Nome,
    [MaxLength(500)] string? Descricao,
    [Range(0.01, double.MaxValue)] decimal? Preco,
    bool? IsActive
);
