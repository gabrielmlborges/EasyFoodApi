using EasyFood.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyFood.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoProduto> PedidoProdutos => Set<PedidoProduto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasConversion<string>();
        });

        modelBuilder.Entity<Produto>(e =>
        {
            e.Property(p => p.Preco).HasColumnType("numeric(10,2)");
        });

        modelBuilder.Entity<Pedido>(e =>
        {
            e.Property(p => p.Status).HasConversion<string>();
            e.Property(p => p.Total).HasColumnType("numeric(10,2)");
        });

        modelBuilder.Entity<PedidoProduto>(e =>
        {
            e.Property(p => p.PrecoUnitario).HasColumnType("numeric(10,2)");
        });

        // Seed: admin inicial — hash estático gerado de "Admin@123"
        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            Nome = "Admin",
            Email = "admin@easyfood.com",
            SenhaHash = "$2a$11$873B/mukwJ9d4ca6o2.xc.rlg06Hxclu0s3NpSWG7G9oRz8nCDzSq",
            Role = RoleUsuario.Admin,
            IsActive = true
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Pedido>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CriadoEm = DateTime.UtcNow;

            if (entry.State is EntityState.Added or EntityState.Modified)
                entry.Entity.AtualizadoEm = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
