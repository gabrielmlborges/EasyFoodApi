using System.ComponentModel.DataAnnotations;

namespace EasyFood.DTOs.Produtos;

public record CriarProdutoRequest(
    [Required, MaxLength(100)] string Nome,
    [MaxLength(500)] string Descricao,
    [MaxLength(500)] string UrlImagem,
    [Required, Range(0.01, double.MaxValue)] decimal Preco
);
