using Moq;
using System.Linq.Expressions;
using Application.DataTransferObjects.HandleLivro;
using Domain.Entities;
using Infra.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Application.Services;

namespace Unit.Tests.Application.Services;

public class LivroServiceTests
{
    private readonly Mock<ILivroRepository> _livroRepositoryMock;
    private readonly Mock<IAutorRepository> _autorRepositoryMock;
    private readonly Mock<IAssuntoRepository> _assuntoRepositoryMock;
    private readonly Mock<ILivroAutorRepository> _livroAutorRepositoryMock;
    private readonly Mock<ILivroPrecoRepository> _precoLivroRepositoryMock;
    private readonly Mock<ILivroAssuntoRepository> _livroAssuntoRepositoryMock;
    private readonly LivroService _service;

    public LivroServiceTests()
    {
        _livroRepositoryMock = new Mock<ILivroRepository>();
        _autorRepositoryMock = new Mock<IAutorRepository>();
        _assuntoRepositoryMock = new Mock<IAssuntoRepository>();
        _livroAutorRepositoryMock = new Mock<ILivroAutorRepository>();
        _precoLivroRepositoryMock = new Mock<ILivroPrecoRepository>();
        _livroAssuntoRepositoryMock = new Mock<ILivroAssuntoRepository>();
        _service = new LivroService(
            _livroRepositoryMock.Object,
            _autorRepositoryMock.Object,
            _assuntoRepositoryMock.Object,
            _livroAutorRepositoryMock.Object,
            _precoLivroRepositoryMock.Object,
            _livroAssuntoRepositoryMock.Object
        );
    }

    [Fact]
    public void GetAll_RetornaTodosOsLivros_ComRelacionamentos()
    {
        // Arrange
        var fakeAutor1 = new Autor { CodAu = 1, Nome = "Autor 1" };
        var fakeAutor2 = new Autor { CodAu = 2, Nome = "Autor 2" };
        var fakeAssunto1 = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakePreco1 = new LivroPreco { Codp = 1, LivroCodl = 1, TipoCompra = "Capa Dura", Valor = 29.99m };

        var fakeLivro1 = new Livro
        {
            Codl = 1,
            Titulo = "Livro 1",
            Editora = "Editora A",
            Edicao = 1,
            AnoPublicacao = "2020",
            Autores = new List<LivroAutor>
            {
                new LivroAutor { LivroCodl = 1, AutorCodAu = 1, Autor = fakeAutor1 },
                new LivroAutor { LivroCodl = 1, AutorCodAu = 2, Autor = fakeAutor2 }
            },
            Assuntos = new List<LivroAssunto>
            {
                new LivroAssunto { LivroCodl = 1, AssuntoCodAs = 1, Assunto = fakeAssunto1 }
            },
            Precos = new List<LivroPreco> { fakePreco1 }
        };

        var fakeLivros = new List<Livro> { fakeLivro1 };

        _livroRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Livro, bool>>>(),
            It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>(),
            It.IsAny<Func<IQueryable<Livro>, IIncludableQueryable<Livro, object>>>(),
            It.IsAny<bool>()))
            .Returns(fakeLivros.AsQueryable());

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        var livros = result.Value;
        Assert.Single(livros);
        var livro = livros.First();
        Assert.Equal(1, livro.Codl);
        Assert.Equal("Livro 1", livro.Titulo);
        Assert.Equal("Editora A", livro.Editora);
        Assert.Equal(1, livro.Edicao);
        Assert.Equal("2020", livro.AnoPublicacao);
        Assert.Equal(2, livro.Autores.Count);
        Assert.Contains(livro.Autores, a => a.CodAu == 1 && a.Nome == "Autor 1");
        Assert.Contains(livro.Autores, a => a.CodAu == 2 && a.Nome == "Autor 2");
        Assert.Single(livro.Assuntos);
        Assert.Equal(1, livro.Assuntos.First().CodAs);
        Assert.Equal("Ficção", livro.Assuntos.First().Descricao);
        Assert.Single(livro.Precos);
        Assert.Equal(1, livro.Precos.First().Codp);
        Assert.Equal("Capa Dura", livro.Precos.First().TipoCompra);
        Assert.Equal(29.99m, livro.Precos.First().Valor);
    }

    [Fact]
    public void GetById_RetornaLivro_QuandoExiste()
    {
        // Arrange
        var fakeAutor1 = new Autor { CodAu = 1, Nome = "Autor 1" };
        var fakeAssunto1 = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakePreco1 = new LivroPreco { Codp = 1, LivroCodl = 1, TipoCompra = "Capa Dura", Valor = 29.99m };

        var fakeLivro1 = new Livro
        {
            Codl = 1,
            Titulo = "Livro 1",
            Editora = "Editora A",
            Edicao = 1,
            AnoPublicacao = "2020",
            Autores = new List<LivroAutor>
            {
                new LivroAutor { LivroCodl = 1, AutorCodAu = 1, Autor = fakeAutor1 }
            },
            Assuntos = new List<LivroAssunto>
            {
                new LivroAssunto { LivroCodl = 1, AssuntoCodAs = 1, Assunto = fakeAssunto1 }
            },
            Precos = new List<LivroPreco> { fakePreco1 }
        };

        var fakeLivros = new List<Livro> { fakeLivro1 };

        _livroRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Livro, bool>>>(),
            It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>(),
            It.IsAny<Func<IQueryable<Livro>, IIncludableQueryable<Livro, object>>>(),
            It.IsAny<bool>()))
            .Returns(fakeLivros.AsQueryable());

        // Act
        var result = _service.GetById(1);

        // Assert
        Assert.True(result.IsSuccess);
        var livro = result.Value;
        Assert.NotNull(livro);
        Assert.Equal(1, livro.Codl);
        Assert.Equal("Livro 1", livro.Titulo);
        Assert.Single(livro.Autores);
        Assert.Equal(1, livro.Autores.First().CodAu);
        Assert.Single(livro.Assuntos);
        Assert.Equal(1, livro.Assuntos.First().CodAs);
        Assert.Single(livro.Precos);
        Assert.Equal(29.99m, livro.Precos.First().Valor);
    }

    [Fact]
    public void GetById_RetornaFalha_QuandoLivroNaoExiste()
    {
        // Arrange
        var fakeLivros = new List<Livro>();
        _livroRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Livro, bool>>>(),
            It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>(),
            It.IsAny<Func<IQueryable<Livro>, IIncludableQueryable<Livro, object>>>(),
            It.IsAny<bool>()))
            .Returns(fakeLivros.AsQueryable());

        // Act
        var result = _service.GetById(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Livro não encontrado.", result.Errors[0]);
    }

    [Fact]
    public void Create_InsereNovoLivro_ComRelacionamentos()
    {
        // Arrange
        var request = new LivroDTO
        {
            Titulo = "Novo Livro",
            Editora = "Editora B",
            Edicao = 2,
            AnoPublicacao = "2023",
            Autores = new List<int> { 1, 2 },
            Assuntos = new List<int> { 1 }
        };

        _livroRepositoryMock.Setup(r => r.Insert(It.IsAny<Livro>()))
            .Returns((Livro l) =>
            {
                l.Codl = 3;
                return l;
            });

        _livroRepositoryMock.Setup(r => r.SaveChanges())
            .Returns(true);

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value);
        _livroRepositoryMock.Verify(r => r.Insert(It.Is<Livro>(livro =>
            livro.Titulo == "Novo Livro" &&
            livro.Editora == "Editora B" &&
            livro.Edicao == 2 &&
            livro.AnoPublicacao == "2023" &&
            livro.Autores.Count == 2 &&
            livro.Autores.Any(la => la.AutorCodAu == 1) &&
            livro.Autores.Any(la => la.AutorCodAu == 2) &&
            livro.Assuntos.Count == 1 &&
            livro.Assuntos.Any(las => las.AssuntoCodAs == 1)
        )), Times.Once());
        _livroRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Create_RetornaFalha_QuandoTituloVazio()
    {
        // Arrange
        var request = new LivroDTO
        {
            Titulo = "",
            Editora = "Editora B",
            Edicao = 2,
            AnoPublicacao = "2023",
            Autores = new List<int>(),
            Assuntos = new List<int>()
        };

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Título é obrigatório.");
    }

    [Fact]
    public void Update_AtualizaLivro_QuandoExiste()
    {
        // Arrange
        var fakeLivro = new Livro
        {
            Codl = 1,
            Titulo = "Livro Antigo",
            Editora = "Editora A",
            Edicao = 1,
            AnoPublicacao = "2020",
            Autores = new List<LivroAutor>
            {
                new LivroAutor { LivroCodl = 1, AutorCodAu = 1 }
            },
            Assuntos = new List<LivroAssunto>
            {
                new LivroAssunto { LivroCodl = 1, AssuntoCodAs = 1 }
            }
        };

        _livroRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Livro, bool>>>(),
            It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>(),
            It.IsAny<Func<IQueryable<Livro>, IIncludableQueryable<Livro, object>>>(),
            It.IsAny<bool>()))
            .Returns(new List<Livro> { fakeLivro }.AsQueryable());

        _livroRepositoryMock.Setup(r => r.Update(It.IsAny<Livro>())).Verifiable();
        _livroRepositoryMock.Setup(r => r.SaveChanges()).Returns(true);

        var request = new LivroDTO
        {
            Titulo = "Livro Atualizado",
            Editora = "Editora B",
            Edicao = 2,
            AnoPublicacao = "2023",
            Autores = new List<int> { 2 },
            Assuntos = new List<int> { 2 }
        };

        // Act
        var result = _service.Update(1, request);

        // Assert
        Assert.True(result.IsSuccess);
        _livroRepositoryMock.Verify(r => r.Update(It.Is<Livro>(livro =>
            livro.Codl == 1 &&
            livro.Titulo == "Livro Atualizado" &&
            livro.Editora == "Editora B" &&
            livro.Edicao == 2 &&
            livro.AnoPublicacao == "2023" &&
            livro.Autores.Count == 1 &&
            livro.Autores.First().AutorCodAu == 2 &&
            livro.Assuntos.Count == 1 &&
            livro.Assuntos.First().AssuntoCodAs == 2
        )), Times.Once());
        _livroRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Update_RetornaFalha_QuandoLivroNaoExiste()
    {
        // Arrange
        _livroRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Livro, bool>>>(),
            It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>(),
            It.IsAny<Func<IQueryable<Livro>, IIncludableQueryable<Livro, object>>>(),
            It.IsAny<bool>()))
            .Returns(new List<Livro>().AsQueryable());

        var request = new LivroDTO
        {
            Titulo = "Livro Atualizado",
            Editora = "Editora B",
            Edicao = 2,
            AnoPublicacao = "2023",
            Autores = new List<int>(),
            Assuntos = new List<int>()
        };

        // Act
        var result = _service.Update(999, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Livro não encontrado.");
    }

    [Fact]
    public void Delete_DeletaLivroErelacionamentos_QuandoExiste()
    {
        // Arrange
        var fakeLivro = new Livro
        {
            Codl = 1,
            Titulo = "Livro 1",
            Autores = new List<LivroAutor> { new LivroAutor { LivroCodl = 1, AutorCodAu = 1 } },
            Assuntos = new List<LivroAssunto> { new LivroAssunto { LivroCodl = 1, AssuntoCodAs = 1 } },
            Precos = new List<LivroPreco> { new LivroPreco { Codp = 1, LivroCodl = 1 } }
        };

        _livroRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Livro, bool>>>(),
            It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>(),
            It.IsAny<Func<IQueryable<Livro>, IIncludableQueryable<Livro, object>>>(),
            It.IsAny<bool>()))
            .Returns(new List<Livro> { fakeLivro }.AsQueryable());

        _livroAssuntoRepositoryMock.Setup(r => r.Delete(It.IsAny<IEnumerable<LivroAssunto>>())).Verifiable();
        _livroAutorRepositoryMock.Setup(r => r.Delete(It.IsAny<IEnumerable<LivroAutor>>())).Verifiable();
        _precoLivroRepositoryMock.Setup(r => r.Delete(It.IsAny<IEnumerable<LivroPreco>>())).Verifiable();
        _livroRepositoryMock.Setup(r => r.Delete(It.IsAny<Livro>())).Verifiable();
        _livroRepositoryMock.Setup(r => r.SaveChanges()).Returns(true).Verifiable();

        // Act
        var result = _service.Delete(1);

        // Assert
        Assert.True(result.IsSuccess);
        _livroAssuntoRepositoryMock.Verify(r => r.Delete(It.Is<IEnumerable<LivroAssunto>>(las => las.Count() == 1 && las.First().AssuntoCodAs == 1)), Times.Once());
        _livroAutorRepositoryMock.Verify(r => r.Delete(It.Is<IEnumerable<LivroAutor>>(la => la.Count() == 1 && la.First().AutorCodAu == 1)), Times.Once());
        _precoLivroRepositoryMock.Verify(r => r.Delete(It.Is<IEnumerable<LivroPreco>>(lp => lp.Count() == 1 && lp.First().Codp == 1)), Times.Once());
        _livroRepositoryMock.Verify(r => r.Delete(It.Is<Livro>(l => l.Codl == 1)), Times.Once());
        _livroRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Delete_RetornaFalha_QuandoLivroNaoExiste()
    {
        // Arrange
        _livroRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Livro, bool>>>(),
            It.IsAny<Func<IQueryable<Livro>, IOrderedQueryable<Livro>>>(),
            It.IsAny<Func<IQueryable<Livro>, IIncludableQueryable<Livro, object>>>(),
            It.IsAny<bool>()))
            .Returns(new List<Livro>().AsQueryable());

        // Act
        var result = _service.Delete(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Livro não encontrado.", result.Errors[0]);
    }
}