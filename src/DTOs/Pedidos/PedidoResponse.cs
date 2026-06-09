namespace EasyFood.DTOs.Pedidos;

public record PedidoResponse(
    int Id,
    int UsuarioId,
    string Status,
    decimal Total,
    string EnderecoEntrega,
    string MetodoPagamento,
    bool IsActive,
    DateTime CriadoEm,
    DateTime AtualizadoEm,
    List<ItemPedidoResponse> Itens
);

public record ItemPedidoResponse(
    int ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal
);
