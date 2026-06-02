namespace EasyFood.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public RoleUsuario Role { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Pedido> Pedidos { get; set; } = [];
}

public enum RoleUsuario
{
    Admin,
    Cliente
}
