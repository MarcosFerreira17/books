using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Mappings;

public class PrecoLivroMapping : IEntityTypeConfiguration<LivroPreco>
{
    public void Configure(EntityTypeBuilder<LivroPreco> builder)
    {
        builder.HasKey(p => p.Codp);

        builder.Property(p => p.Codp)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.TipoCompra)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Valor).IsRequired();

        // Relacionamento com Livro
        builder.HasOne(p => p.Livro)
            .WithMany(l => l.Precos)
            .HasForeignKey(p => p.LivroCodl)
            .OnDelete(DeleteBehavior.Cascade);
    }
}