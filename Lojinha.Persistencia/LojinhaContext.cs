using Lojinha.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Lojinha.Persistencia
{
    public class LojinhaContext : DbContext
    {
        public LojinhaContext(DbContextOptions<LojinhaContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemsPedidos> ItemsPedidos { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<Fornecedores> Fornecedores { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacaoEstoque { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfiguration(new UsuarioMapConfiguration());

            modelBuilder.ApplyConfiguration(new ProdutosMapConfiguration());

            modelBuilder.ApplyConfiguration(new EstoqueMapConfiguration());

            modelBuilder.ApplyConfiguration(new PedidoMapConfiguration());

            modelBuilder.ApplyConfiguration(new ItemsPedidosMapConfiguration());

            modelBuilder.ApplyConfiguration(new FornecedoresMapConfiguration());

            modelBuilder.ApplyConfiguration(new MoviEstoqueMapConfiguration());
        }
    }
}
