namespace EasyFood.DTOs.Produtos;

public record ProdutoResponse(
    int Id,
    string Nome,
    string Descricao,
    string UrlImagem,
    decimal Preco,
    bool IsActive
);
