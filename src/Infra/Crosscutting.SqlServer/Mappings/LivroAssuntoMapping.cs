using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Mappings;

public class LivroAssuntoMapping : IEntityTypeConfiguration<LivroAssunto>
{
    public void Configure(EntityTypeBuilder<LivroAssunto> builder)
    {

        builder.HasKey(la => new { la.LivroCodl, la.AssuntoCodAs });

        builder.HasOne(la => la.Livro)
            .WithMany(l => l.Assuntos)
            .HasForeignKey(la => la.LivroCodl)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(la => la.Assunto)
            .WithMany(a => a.Livros)
            .HasForeignKey(la => la.AssuntoCodAs)
            .OnDelete(DeleteBehavior.Cascade);
    }
}