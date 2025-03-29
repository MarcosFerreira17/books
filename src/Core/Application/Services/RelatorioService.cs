using Application.Services.Interfaces;
using Infra.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services;

public class RelatorioService : IRelatorioService
{
    private readonly ApplicationDbContext _context;
    public RelatorioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public byte[] GerarRelatorioPDF()
    {
        var dados = _context.RelatorioAutoresLivros
            .AsNoTracking()
            .OrderBy(r => r.Autor)
            .ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Header().Text("Relatório de Livros por Autor").Bold().FontSize(16);

                page.Content().Table(table =>
                {
                    // Cabeçalho
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(); // Autor
                        columns.RelativeColumn(); // Livro
                        columns.RelativeColumn(); // Assuntos
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Autor");
                        header.Cell().Text("Livro");
                        header.Cell().Text("Assuntos");
                    });

                    // Dados
                    foreach (var item in dados)
                    {
                        table.Cell().Text(item.Autor);
                        table.Cell().Text(item.Livro);
                        table.Cell().Text(item.Assuntos);
                    }
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Gerado em: ").FontColor(Colors.Grey.Medium);
                    t.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });
            });
        });

        return document.GeneratePdf();
    }
}