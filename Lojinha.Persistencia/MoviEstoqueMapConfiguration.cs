using Lojinha.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Persistencia
{
    public class MoviEstoqueMapConfiguration : IEntityTypeConfiguration<MovimentacaoEstoque>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
        {
            builder.HasKey(m => m.IdMovimentacao);

            builder.Property(m => m.IdProduto)
                .IsRequired();

            builder.Property(m => m.TipoMovimentacao)
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(m => m.Quantidade)
               .IsRequired()
               .HasPrecision(18, 2);

            builder.Property(m => m.DataMovimentacao)
                .IsRequired();

            builder.Property(m => m.IdFornecedor);
        }
    }
}
