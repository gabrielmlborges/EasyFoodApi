namespace EasyFood.DTOs.Produtos;

public record ProdutoResponse(
    int Id,
    string Nome,
    string Descricao,
    decimal Preco,
    bool IsActive
);
