using System.ComponentModel.DataAnnotations;

namespace EasyFood.DTOs.Pedidos;

public record CriarPedidoRequest(
    [Required, MinLength(1)] List<ItemPedidoRequest> Itens
);

public record ItemPedidoRequest(
    [Required] int ProdutoId,
    [Required, Range(1, int.MaxValue)] int Quantidade
);
