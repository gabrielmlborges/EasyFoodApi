namespace EasyFood.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<PedidoProduto> PedidoProdutos { get; set; } = [];
}
