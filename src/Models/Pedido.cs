namespace EasyFood.Models;

public class Pedido
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Pendente;
    public decimal Total { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public ICollection<PedidoProduto> PedidoProdutos { get; set; } = [];
}

public enum StatusPedido
{
    Pendente,
    Confirmado,
    EmPreparo,
    Entregue,
    Cancelado
}
