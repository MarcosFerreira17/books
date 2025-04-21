using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Mappings;

public class AutorMapping : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {

        builder.HasKey(a => a.CodAu);

        builder.Property(a => a.CodAu)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Nome)
            .IsRequired()
            .HasMaxLength(40);

        // Relacionamento many-to-many
        builder.HasMany(a => a.Livros)
            .WithOne(la => la.Autor)
            .HasForeignKey(la => la.LivroCodl);
    }
}