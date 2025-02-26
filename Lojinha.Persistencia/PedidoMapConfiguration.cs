using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Lojinha.Domain;

namespace Lojinha.Persistencia
{

    public class PedidoMapConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {       
            builder.HasKey(p => p.IdPedido);

            builder.Property(p => p.IdClient)
                .IsRequired();

            builder.Property(p => p.ValorTotal)
                .HasPrecision(18, 2);

            builder.Property(p => p.DataPedido)
                .IsRequired();
        }
    }
}
