using Lojinha.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Persistencia
{
    public class ItemsPedidosMapConfiguration : IEntityTypeConfiguration<ItemsPedidos>
    {
        public void Configure(EntityTypeBuilder<ItemsPedidos> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Valor)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.IdPedido)
                .IsRequired();

            builder.Property(i => i.IdProduto)
                .IsRequired();

            builder.Property(i => i.Quantidade)
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}
