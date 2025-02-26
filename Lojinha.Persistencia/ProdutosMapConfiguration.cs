using Lojinha.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Persistencia
{
    public class ProdutosMapConfiguration : IEntityTypeConfiguration<Produtos>
    {
        public void Configure(EntityTypeBuilder<Produtos> builder)
        {
            builder.HasKey(p => p.IdProduto);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(p => p.Nome)
                .IsUnique();

            builder.Property(p => p.Valor)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.Estoque)
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}
