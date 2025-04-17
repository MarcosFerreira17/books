using Moq;
using System.Linq.Expressions;
using Application.DataTransferObjects.HandleAssunto;
using Domain.Entities;
using Infra.Database.Repositories.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore.Query;

namespace Unit.Tests.Application.Services;
public class AssuntoServiceTests
{
    private readonly Mock<IAssuntoRepository> _assuntoRepositoryMock;

    public AssuntoServiceTests()
    {
        _assuntoRepositoryMock = new Mock<IAssuntoRepository>();
    }

    [Fact]
    public void GetById_RetornaAssunto_QuandoExiste()
    {
        // Arrange
        var fakeAssunto = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakeAssuntos = new List<Assunto> { fakeAssunto };

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        // Act
        var result = service.GetById(1);

        // Assert
        Assert.True(result.IsSuccess);
        var assunto = result.Value;
        Assert.NotNull(assunto);
        Assert.Equal(1, assunto.CodAs);
        Assert.Equal("Ficção", assunto.Descricao);
    }

    [Fact]
    public void GetById_RetornaFalha_QuandoNaoExiste()
    {
        // Arrange
        var fakeAssuntos = new List<Assunto>();

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        // Act
        var result = service.GetById(1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Assunto não encontrado.", result.Errors[0]);
    }

    [Fact]
    public void GetAll_RetornaListaDeAssuntos_QuandoExistem()
    {
        // Arrange
        var fakeAssunto1 = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakeAssunto2 = new Assunto { CodAs = 2, Descricao = "Não Ficção" };
        var fakeAssuntos = new List<Assunto> { fakeAssunto1, fakeAssunto2 };

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        // Act
        var result = service.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        var assuntos = result.Value;
        Assert.Equal(2, assuntos.Count());
        Assert.Contains(assuntos, a => a.CodAs == 1 && a.Descricao == "Ficção");
        Assert.Contains(assuntos, a => a.CodAs == 2 && a.Descricao == "Não Ficção");
    }

    [Fact]
    public void GetAll_RetornaFalha_QuandoNaoExistem()
    {
        // Arrange
        var fakeAssuntos = new List<Assunto>();

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        // Act
        var result = service.GetAll();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Nenhum assunto encontrado.", result.Errors[0]);
    }

    [Fact]
    public void Create_InsereNovoAssunto_QuandoRequestValido()
    {
        // Arrange
        var fakeAssuntos = new List<Assunto>();

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        _assuntoRepositoryMock.Setup(r => r.Insert(It.IsAny<Assunto>())).Verifiable();
        _assuntoRepositoryMock.Setup(r => r.SaveChanges()).Returns(true).Verifiable();

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        var request = new AssuntoDTO { Descricao = "Novo Assunto" };

        // Act
        var result = service.Create(request);

        // Assert
        Assert.True(result.IsSuccess);
        _assuntoRepositoryMock.Verify(r => r.Insert(It.Is<Assunto>(a => a.Descricao == "Novo Assunto")), Times.Once());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Create_RetornaFalha_QuandoDescricaoVazia()
    {
        // Arrange
        var fakeAssuntos = new List<Assunto>();

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        var request = new AssuntoDTO { Descricao = "" };

        // Act
        var result = service.Create(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Descrição é obrigatória.");
        _assuntoRepositoryMock.Verify(r => r.Insert(It.IsAny<Assunto>()), Times.Never());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public void Create_RetornaFalha_QuandoAssuntoJaExiste()
    {
        // Arrange
        var fakeAssunto = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakeAssuntos = new List<Assunto> { fakeAssunto };

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        var request = new AssuntoDTO { Descricao = "Ficção" };

        // Act
        var result = service.Create(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Assunto já existe.");
        _assuntoRepositoryMock.Verify(r => r.Insert(It.IsAny<Assunto>()), Times.Never());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public void Update_AtualizaAssunto_QuandoExisteERequestValido()
    {
        // Arrange
        var fakeAssunto = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakeAssuntos = new List<Assunto> { fakeAssunto };

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        _assuntoRepositoryMock.Setup(r => r.Update(It.IsAny<Assunto>())).Verifiable();
        _assuntoRepositoryMock.Setup(r => r.SaveChanges()).Returns(true).Verifiable();

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        var request = new AssuntoDTO { Descricao = "Atualizado" };

        // Act
        var result = service.Update(1, request);

        // Assert
        Assert.True(result.IsSuccess);
        _assuntoRepositoryMock.Verify(r => r.Update(It.Is<Assunto>(a => a.CodAs == 1 && a.Descricao == "Atualizado")), Times.Once());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Update_RetornaFalha_QuandoAssuntoNaoExiste()
    {
        // Arrange
        var fakeAssuntos = new List<Assunto>();

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        var request = new AssuntoDTO { Descricao = "Atualizado" };

        // Act
        var result = service.Update(1, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Assunto não encontrado.");
        _assuntoRepositoryMock.Verify(r => r.Update(It.IsAny<Assunto>()), Times.Never());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public void Update_RetornaFalha_QuandoDescricaoVazia()
    {
        // Arrange
        var fakeAssunto = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakeAssuntos = new List<Assunto> { fakeAssunto };

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        var request = new AssuntoDTO { Descricao = "" };

        // Act
        var result = service.Update(1, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Descrição é obrigatória.");
        _assuntoRepositoryMock.Verify(r => r.Update(It.IsAny<Assunto>()), Times.Never());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public void Update_RetornaAmbosErros_QuandoAssuntoNaoExisteEDescricaoVazia()
    {
        // Arrange
        var fakeAssuntos = new List<Assunto>();

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        var request = new AssuntoDTO { Descricao = "" };

        // Act
        var result = service.Update(1, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e == "Assunto não encontrado.");
        Assert.Contains(result.Errors, e => e == "Descrição é obrigatória.");
        _assuntoRepositoryMock.Verify(r => r.Update(It.IsAny<Assunto>()), Times.Never());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Never());
    }

    [Fact]
    public void Delete_DeletaAssunto_QuandoExiste()
    {
        // Arrange
        var fakeAssunto = new Assunto { CodAs = 1, Descricao = "Ficção" };
        var fakeAssuntos = new List<Assunto> { fakeAssunto };

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        _assuntoRepositoryMock.Setup(r => r.Delete(It.IsAny<Assunto>())).Verifiable();
        _assuntoRepositoryMock.Setup(r => r.SaveChanges()).Returns(true).Verifiable();

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        // Act
        var result = service.Delete(1);

        // Assert
        Assert.True(result.IsSuccess);
        _assuntoRepositoryMock.Verify(r => r.Delete(It.Is<Assunto>(a => a.CodAs == 1)), Times.Once());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Once());
    }

    [Fact]
    public void Delete_RetornaFalha_QuandoNaoExiste()
    {
        // Arrange
        var fakeAssuntos = new List<Assunto>();

        _assuntoRepositoryMock.Setup(r => r.Query(
            It.IsAny<Expression<Func<Assunto, bool>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>>>(),
            It.IsAny<Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>>>(),
            It.IsAny<bool>()))
            .Returns((Expression<Func<Assunto, bool>> predicate, Func<IQueryable<Assunto>, IOrderedQueryable<Assunto>> orderBy, Func<IQueryable<Assunto>, IIncludableQueryable<Assunto, object>> include, bool enableTracking) =>
                fakeAssuntos.AsQueryable().Where(predicate ?? (a => true)));

        var service = new AssuntoService(_assuntoRepositoryMock.Object);

        // Act
        var result = service.Delete(1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Assunto não encontrado.", result.Errors[0]);
        _assuntoRepositoryMock.Verify(r => r.Delete(It.IsAny<Assunto>()), Times.Never());
        _assuntoRepositoryMock.Verify(r => r.SaveChanges(), Times.Never());
    }
}
