using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.Mappings;

// Data/Configurations/LivroConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {

        // Chave Primária
        builder.HasKey(l => l.Codl);

        // Propriedades
        builder.Property(l => l.Codl)
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Titulo)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(l => l.Editora)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(l => l.Edicao)
            .IsRequired();

        builder.Property(l => l.AnoPublicacao)
            .IsRequired()
            .HasMaxLength(4);

        // Relacionamentos
        builder.HasMany(l => l.Autores)
            .WithOne(la => la.Livro)
            .HasForeignKey(la => la.LivroCodl);

        builder.HasMany(l => l.Assuntos)
            .WithOne(la => la.Livro)
            .HasForeignKey(la => la.LivroCodl);

        builder.HasMany(l => l.Precos)
            .WithOne(p => p.Livro)
            .HasForeignKey(p => p.LivroCodl);
    }
}