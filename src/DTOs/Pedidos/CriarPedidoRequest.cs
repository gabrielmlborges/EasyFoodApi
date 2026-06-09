using System.ComponentModel.DataAnnotations;
using EasyFood.Models;

namespace EasyFood.DTOs.Pedidos;

public record CriarPedidoRequest(
    [Required, MinLength(1)] List<ItemPedidoRequest> Itens,
    [Required, MinLength(5)] string EnderecoEntrega,
    [Required] MetodoPagamento MetodoPagamento
);

public record ItemPedidoRequest(
    [Required] int ProdutoId,
    [Required, Range(1, int.MaxValue)] int Quantidade
);
