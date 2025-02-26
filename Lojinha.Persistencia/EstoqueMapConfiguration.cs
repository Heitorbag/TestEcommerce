using Lojinha.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Lojinha.Persistencia
{ 
    public class EstoqueMapConfiguration : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(e => e.Nome)
                .IsUnique();

            builder.Property(e => e.IdProduto)
                .IsRequired();

            builder.Property(e => e.Quantidade)
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}
