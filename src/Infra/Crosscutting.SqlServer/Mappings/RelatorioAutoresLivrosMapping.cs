using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Views;

namespace Infra.Database.Mappings;

public class RelatorioAutoresLivrosMapping : IEntityTypeConfiguration<RelatorioAutoresLivros>
{
    public void Configure(EntityTypeBuilder<RelatorioAutoresLivros> builder)
    {
       builder.ToView("RelatorioAutoresLivros").HasNoKey();
    }
}