using Lojinha.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Persistencia
{ 
    public class FornecedoresMapConfiguration : IEntityTypeConfiguration<Fornecedores>
    {
        public void Configure(EntityTypeBuilder<Fornecedores> builder)
        {
            builder.HasKey(f => f.IdFornecedor);

            builder.Property(f => f.NomeFornecedor)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.CNPJ)
                .IsRequired()
                .HasMaxLength(14);

            builder.HasIndex(f => f.CNPJ)
                .IsUnique();

            builder.Property(f => f.Endereco)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(f => f.Telefone)
                .IsRequired()
                .HasMaxLength(15);

            builder.HasIndex(f => f.Telefone)
                .IsUnique();

            builder.Property(f => f.Email)
               .IsRequired()
               .HasMaxLength(100);

            builder.HasIndex(f => f.Email)
               .IsUnique();
        }
    }
}
