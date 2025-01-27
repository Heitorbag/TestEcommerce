using Microsoft.EntityFrameworkCore;

namespace Lojinha.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemsPedidos> ItemsPedidos { get; set; }
        public DbSet<Estoque> Estoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estoque>()
                .Property(e => e.Quantidade)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ItemsPedidos>()
                .Property(i => i.Valor)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Produtos>()
                .Property(p => p.Valor)
                .HasPrecision(18, 2);
        }
    }
}
