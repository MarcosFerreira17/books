using Domain.Entities;
using Domain.Views;
using Infra.Database.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infra.Database.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    public DbSet<RelatorioAutoresLivros> RelatorioAutoresLivros { get; set; }
    public DbSet<Livro> Livro { get; set; }
    public DbSet<Autor> Autor { get; set; }
    public DbSet<Assunto> Assunto { get; set; }
    public DbSet<LivroPreco> LivroPreco { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new LivroConfiguration());
        builder.ApplyConfiguration(new AutorMapping());
        builder.ApplyConfiguration(new AssuntoMapping());
        builder.ApplyConfiguration(new LivroAutorMapping());
        builder.ApplyConfiguration(new LivroAssuntoMapping());
        builder.ApplyConfiguration(new PrecoLivroMapping());
        builder.ApplyConfiguration(new RelatorioAutoresLivrosMapping());
    }
}