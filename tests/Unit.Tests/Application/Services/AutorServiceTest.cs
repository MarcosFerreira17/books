using Moq;
using System.Linq.Expressions;
using Application.DataTransferObjects.HandleAutor;
using Domain.Entities;
using Infra.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Application.Services;

namespace Unit.Tests.Application.Services;

public class AutorServiceTests
{
    private readonly Mock<IAutorRepository> _autorRepositoryMock;
    private readonly AutorService _service;

    public AutorServiceTests()
    {
        _autorRepositoryMock = new Mock<IAutorRepository>();
        _service = new AutorService(_autorRepositoryMock.Object);
    }

    [Fact]
    public void GetById_RetornaAutor_ComLivros_QuandoExiste()
    {
        // Arrange
        var fakeLivro1 = new Livro { Codl = 1, Titulo = "Livro 1", AnoPublicacao = "2020", Edicao = 1, Editora = "Editora A" };
        var fakeLivro2 = new Livro { Codl = 2, Titulo = "Livro 2", AnoPublicacao = "2021", Edicao = 2, Editora = "Editora B" };
        var fakeAutor1 = new Autor
        {
            CodAu = 1,
            Nome = "Autor 1",
            Livros = new List<LivroAutor>
            {
                new LivroAutor { AutorCodAu = 1, LivroCodl = 1, Livro = fakeLivro1 },
                new LivroAutor { AutorCodAu = 1, LivroCodl = 2, Livro = fakeLivro2 }
            }
        };
        var fakeAutores = new List<Autor> { fakeAutor1 };

        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                fakeAutores.AsQueryable().Where(predicate));

        // Act
        var result = _service.GetById(1);

        // Assert
        Assert.True(result.IsSuccess);
        var autor = result.Value;
        Assert.NotNull(autor);
        Assert.Equal(1, autor.CodAu);
        Assert.Equal("Autor 1", autor.Nome);
        Assert.Equal(2, autor.Livros.Count);
        Assert.Contains(autor.Livros, l => l.Titulo == "Livro 1" && l.AnoPublicacao == "2020" && l.Edicao == 1 && l.Editora == "Editora A");
        Assert.Contains(autor.Livros, l => l.Titulo == "Livro 2" && l.AnoPublicacao == "2021" && l.Edicao == 2 && l.Editora == "Editora B");
    }

    [Fact]
    public void GetById_RetornaFalha_QuandoAutorNaoExiste()
    {
        // Arrange
        var fakeAutores = new List<Autor>();
        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                fakeAutores.AsQueryable().Where(predicate));

        // Act
        var result = _service.GetById(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Autor não encontrado.", result.Errors[0]);
    }

    [Fact]
    public void GetAll_RetornaTodosOsAutores_ComLivros()
    {
        // Arrange
        var fakeLivro1 = new Livro { Codl = 1, Titulo = "Livro 1", AnoPublicacao = "2020", Edicao = 1, Editora = "Editora A" };
        var fakeLivro2 = new Livro { Codl = 2, Titulo = "Livro 2", AnoPublicacao = "2021", Edicao = 2, Editora = "Editora B" };
        var fakeAutor1 = new Autor
        {
            CodAu = 1,
            Nome = "Autor 1",
            Livros = new List<LivroAutor> { new LivroAutor { AutorCodAu = 1, LivroCodl = 1, Livro = fakeLivro1 } }
        };
        var fakeAutor2 = new Autor
        {
            CodAu = 2,
            Nome = "Autor 2",
            Livros = new List<LivroAutor> { new LivroAutor { AutorCodAu = 2, LivroCodl = 2, Livro = fakeLivro2 } }
        };
        var fakeAutores = new List<Autor> { fakeAutor1, fakeAutor2 };

        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                fakeAutores.AsQueryable().Where(predicate ?? (a => true)));

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        var autores = result.Value;
        Assert.Equal(2, autores.Count());
        var autor1 = autores.First(a => a.CodAu == 1);
        Assert.Equal("Autor 1", autor1.Nome);
        Assert.Single(autor1.Livros);
        Assert.Equal("Livro 1", autor1.Livros.First().Titulo);
        var autor2 = autores.First(a => a.CodAu == 2);
        Assert.Equal("Autor 2", autor2.Nome);
        Assert.Single(autor2.Livros);
        Assert.Equal("Livro 2", autor2.Livros.First().Titulo);
    }

    [Fact]
    public void Create_InsereNovoAutor_QuandoRequestValido()
    {
        // Arrange
        var request = new AutorDTO { Nome = "Novo Autor" };
        _autorRepositoryMock.Setup(r => r.Insert(It.IsAny<Autor>())).Callback((Autor a) => { a.CodAu = 3; });
        _autorRepositoryMock.Setup(r => r.SaveChanges()).Returns(true);

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.True(result.IsSuccess);
        _autorRepositoryMock.Verify(r => r.Insert(It.Is<Autor>(a => a.Nome == "Novo Autor")), Times.Once());
        _autorRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Create_RetornaFalha_QuandoNomeVazio()
    {
        // Arrange
        var request = new AutorDTO { Nome = "" };

        // Act
        var result = _service.Create(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Nome é obrigatório.");
    }

    [Fact]
    public void Update_AtualizaAutor_QuandoExisteERequestValido()
    {
        // Arrange
        var fakeAutor = new Autor { CodAu = 1, Nome = "Autor Antigo" };
        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                new List<Autor> { fakeAutor }.AsQueryable().Where(predicate));
        _autorRepositoryMock.Setup(r => r.Update(It.IsAny<Autor>())).Verifiable();
        _autorRepositoryMock.Setup(r => r.SaveChanges()).Returns(true);

        var request = new AutorDTO { Nome = "Autor Atualizado" };

        // Act
        var result = _service.Update(1, request);

        // Assert
        Assert.True(result.IsSuccess);
        _autorRepositoryMock.Verify(r => r.Update(It.Is<Autor>(a => a.CodAu == 1 && a.Nome == "Autor Atualizado")), Times.Once());
        _autorRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Update_RetornaFalha_QuandoAutorNaoExiste()
    {
        // Arrange
        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                new List<Autor>().AsQueryable().Where(predicate));

        var request = new AutorDTO { Nome = "Autor Atualizado" };

        // Act
        var result = _service.Update(999, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Autor não encontrado.");
    }

    [Fact]
    public void Update_RetornaFalha_QuandoNomeVazio()
    {
        // Arrange
        var fakeAutor = new Autor { CodAu = 1, Nome = "Autor Antigo" };
        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                new List<Autor> { fakeAutor }.AsQueryable().Where(predicate));

        var request = new AutorDTO { Nome = "" };

        // Act
        var result = _service.Update(1, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Nome é obrigatório.");
    }

    [Fact]
    public void Delete_DeletaAutor_QuandoExiste()
    {
        // Arrange
        var fakeAutor = new Autor { CodAu = 1, Nome = "Autor 1" };
        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                new List<Autor> { fakeAutor }.AsQueryable().Where(predicate));
        _autorRepositoryMock.Setup(r => r.Delete(It.IsAny<Autor>())).Verifiable();
        _autorRepositoryMock.Setup(r => r.SaveChanges()).Returns(true);

        // Act
        var result = _service.Delete(1);

        // Assert
        Assert.True(result.IsSuccess);
        _autorRepositoryMock.Verify(r => r.Delete(It.Is<Autor>(a => a.CodAu == 1)), Times.Once());
        _autorRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Delete_RetornaFalha_QuandoAutorNaoExiste()
    {
        // Arrange
        _autorRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Autor, bool>>>(),
            It.IsAny<Func<IQueryable<Autor>, IOrderedQueryable<Autor>>>(),
            It.IsAny<Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Autor, bool>> predicate, Func<IQueryable<Autor>, IOrderedQueryable<Autor>> orderBy, Func<IQueryable<Autor>, IIncludableQueryable<Autor, object>> include, bool enableTracking) =>
                new List<Autor>().AsQueryable().Where(predicate));

        // Act
        var result = _service.Delete(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Autor não encontrado.", result.Errors[0]);
    }
}