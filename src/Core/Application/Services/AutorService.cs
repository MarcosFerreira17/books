using Application.DataTransferObjects;
using Application.DataTransferObjects.HandleAutor;
using Application.DataTransferObjects.HandleLivro;
using Application.Services.Interfaces;
using Domain.Entities;
using Infra.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AutorService : IAutorService
{
    private readonly IAutorRepository _autorRepository;
    public AutorService(IAutorRepository autorRepository)
        => _autorRepository = autorRepository ?? throw new ArgumentNullException(nameof(autorRepository));

    public ResultGeneric<GetAutorDTO> GetById(int cod)
    {
        var autor = _autorRepository
            .Query(predicate: a => a.CodAu == cod)
            .Include(a => a.Livros)
                .ThenInclude(la => la.Livro) // Carrega os dados do Livro
            .Select(a => new GetAutorDTO
            {
                CodAu = a.CodAu,
                Nome = a.Nome,
                Livros = a.Livros.Select(la => new LivroDTO
                {
                    Titulo = la.Livro.Titulo,
                    AnoPublicacao = la.Livro.AnoPublicacao,
                    Edicao = la.Livro.Edicao,
                    Editora = la.Livro.Editora
                }).ToList()
            })
            .FirstOrDefault();

        if (autor == null)
            return ResultGeneric<GetAutorDTO>.Failure("Autor não encontrado.");

        return ResultGeneric<GetAutorDTO>.Success(autor);
    }

    public Result Delete(int cod)
    {
        var entity = _autorRepository.Query(predicate: a => a.CodAu == cod).FirstOrDefault();

        if (entity == null)
            return Result.Failure("Autor não encontrado.");

        _autorRepository.Delete(entity);
        _autorRepository.SaveChanges();
        return Result.Success();
    }

    public ResultGeneric<IEnumerable<GetAutorDTO>> GetAll()
    {
        var autores = _autorRepository.Query()
                                      .Include(a => a.Livros)
                                      .Select(a => new GetAutorDTO
                                      {
                                          CodAu = a.CodAu,
                                          Nome = a.Nome,
                                          Livros = a.Livros.Select(la => new LivroDTO
                                          {
                                              Titulo = la.Livro.Titulo,
                                              AnoPublicacao = la.Livro.AnoPublicacao,
                                              Edicao = la.Livro.Edicao,
                                              Editora = la.Livro.Editora
                                          }).ToList()
                                      })
                                      .ToList();

        return ResultGeneric<IEnumerable<GetAutorDTO>>.Success(autores);
    }

    public Result Update(int cod, AutorDTO request)
    {
        var errors = new List<string>();

        var entity = _autorRepository.Query(predicate: where => where.CodAu == cod).FirstOrDefault();

        if (entity == null)
        {
            errors.Add("Autor não encontrado.");
        }

        if (string.IsNullOrEmpty(request.Nome))
        {
            errors.Add("Nome é obrigatório.");
        }

        if (errors.Count != 0)
        {
            return Result.Failure(errors);
        }

        entity.Nome = request.Nome;

        _autorRepository.Update(entity);
        _autorRepository.SaveChanges();

        return Result.Success();
    }

    public Result Create(AutorDTO request)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(request.Nome))
        {
            errors.Add("Nome é obrigatório.");

            return Result.Failure(errors);
        }

        Autor entity = new()
        {
            Nome = request.Nome
        };

        _autorRepository.Insert(entity);

        _autorRepository.SaveChanges();

        return Result.Success();
    }
}
