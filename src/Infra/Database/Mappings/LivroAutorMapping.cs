using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Mappings;

public class LivroAutorMapping : IEntityTypeConfiguration<LivroAutor>
{
    public void Configure(EntityTypeBuilder<LivroAutor> builder)
    {
        // Chave primária composta
        builder.HasKey(la => new { la.LivroCodl, la.AutorCodAu });

        // Relacionamento com Livro
        builder.HasOne(la => la.Livro)
            .WithMany(l => l.Autores)
            .HasForeignKey(la => la.LivroCodl)
            .OnDelete(DeleteBehavior.Cascade); // Cascata ao excluir Livro

        // Relacionamento com Autor
        builder.HasOne(la => la.Autor)
            .WithMany(a => a.Livros)
            .HasForeignKey(la => la.AutorCodAu)
            .OnDelete(DeleteBehavior.NoAction); // Não permite cascata aqui
    }
}