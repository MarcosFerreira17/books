
using Domain.Entities;
using Infra.Database.DbContexts;

namespace Infra.Database.Helpers;
public static class SeedData
{
    public static void Seed(this ApplicationDbContext context)
    {
        if (context.Database.CanConnect())
        {
            Initialize(context);
        }
    }

    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Verifica se já existem registros
        if (!context.Autor.Any())
        {
            var autores = new[]
            {
                new Autor { Nome = "J.K. Rowling" },
                new Autor { Nome = "George Orwell" },
                new Autor { Nome = "Agatha Christie" }
            };

            context.Autor.AddRange(autores);
            context.SaveChanges();
        }

        if (!context.Assunto.Any())
        {
            var assuntos = new[]
            {
                new Assunto { Descricao = "Fantasia" },
                new Assunto { Descricao = "Ficção Científica" },
                new Assunto { Descricao = "Mistério" }
            };

            context.Assunto.AddRange(assuntos);
            context.SaveChanges();
        }

        if (!context.Livro.Any())
        {
            var livros = new[]
            {
                new Livro
                {
                    Titulo = "Harry Potter e a Pedra Filosofal",
                    Editora = "Rocco",
                    Edicao = 1,
                    AnoPublicacao = "1997",
                    Precos = new List<LivroPreco>
                    {
                        new LivroPreco { TipoCompra = "Físico", Valor = 49.90m },
                        new LivroPreco { TipoCompra = "Digital", Valor = 29.90m }
                    }
                },
                new Livro
                {
                    Titulo = "1984",
                    Editora = "Companhia das Letras",
                    Edicao = 1,
                    AnoPublicacao = "1949",
                    Precos = new List<LivroPreco>
                    {
                        new LivroPreco { TipoCompra = "Físico", Valor = 39.90m }
                    }
                }
            };

            context.Livro.AddRange(livros);
            context.SaveChanges();

            // Relacionar livros com autores e assuntos
            var livro1 = context.Livro.First(l => l.Titulo.Contains("Harry Potter"));
            var autor1 = context.Autor.First(a => a.Nome.Contains("Rowling"));
            var assunto1 = context.Assunto.First(a => a.Descricao == "Fantasia");

            context.LivroAutor.Add(new LivroAutor { Livro = livro1, Autor = autor1 });
            context.LivroAssunto.Add(new LivroAssunto { Livro = livro1, Assunto = assunto1 });

            var livro2 = context.Livro.First(l => l.Titulo == "1984");
            var autor2 = context.Autor.First(a => a.Nome.Contains("Orwell"));
            var assunto2 = context.Assunto.First(a => a.Descricao == "Ficção Científica");

            context.LivroAutor.Add(new LivroAutor { Livro = livro2, Autor = autor2 });
            context.LivroAssunto.Add(new LivroAssunto { Livro = livro2, Assunto = assunto2 });

            context.SaveChanges();
        }
    }
}