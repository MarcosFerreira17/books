using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Mappings;

public class AssuntoMapping : IEntityTypeConfiguration<Assunto>
{
    public void Configure(EntityTypeBuilder<Assunto> builder)
    {
        builder.HasKey(a => a.CodAs);

        builder.Property(a => a.CodAs)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Descricao)
            .IsRequired()
            .HasMaxLength(20);

        // Relacionamento many-to-many
        builder.HasMany(a => a.Livros)
            .WithOne(la => la.Assunto)
            .HasForeignKey(la => la.AssuntoCodAs);
    }
}