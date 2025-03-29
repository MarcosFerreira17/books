using Application.DataTransferObjects;
using Application.DataTransferObjects.HandleAssunto;
using Application.Services.Interfaces;
using Domain.Entities;
using Infra.Database.Repositories.Interfaces;

namespace Application.Services;

public class AssuntoService : IAssuntoService
{
    private readonly IAssuntoRepository _assuntoRepository;
    public AssuntoService(IAssuntoRepository assuntoRepository)
    {
        _assuntoRepository = assuntoRepository ?? throw new ArgumentNullException(nameof(assuntoRepository));
    }

    public ResultGeneric<GetAssuntoDTO> GetById(int cod)
    {
        var entity = _assuntoRepository
            .Query(predicate: a => a.CodAs == cod)
            .FirstOrDefault();

        if (entity == null)
            return ResultGeneric<GetAssuntoDTO>.Failure("Assunto não encontrado.");

        return ResultGeneric<GetAssuntoDTO>.Success(new GetAssuntoDTO
        {
            CodAs = entity.CodAs,
            Descricao = entity.Descricao
        });
    }

    public Result Delete(int cod)
    {
        var entity = _assuntoRepository.Query(predicate: a => a.CodAs == cod).FirstOrDefault();

        if (entity == null)
            return Result.Failure("Assunto não encontrado.");

        _assuntoRepository.Delete(entity);
        _assuntoRepository.SaveChanges();
        return Result.Success();
    }

    public ResultGeneric<IEnumerable<GetAssuntoDTO>> GetAll()
    {
        var assuntos = _assuntoRepository
            .Query()
            .Select(a => new GetAssuntoDTO
            {
                CodAs = a.CodAs,
                Descricao = a.Descricao
            })
            .ToList();

        if (!assuntos.Any())
            return ResultGeneric<IEnumerable<GetAssuntoDTO>>.Failure("Nenhum assunto encontrado.");

        return ResultGeneric<IEnumerable<GetAssuntoDTO>>.Success(assuntos);
    }

    public Result Update(int cod, AssuntoDTO request)
    {
        var errors = new List<string>();

        var entity = _assuntoRepository.Query(predicate: where => where.CodAs == cod).FirstOrDefault();

        if (entity == null)
        {
            errors.Add("Assunto não encontrado.");
        }

        if (string.IsNullOrEmpty(request.Descricao))
        {
            errors.Add("Descrição é obrigatória.");
        }

        if (errors.Count != 0)
        {
            return Result.Failure(errors);
        }

        entity.Descricao = request.Descricao;

        _assuntoRepository.Update(entity);
        _assuntoRepository.SaveChanges();

        return Result.Success();
    }

    public Result Create(AssuntoDTO request)
    {
        var errors = new List<string>();

        var assuntoAlreadyExists = _assuntoRepository.Query(predicate: where => where.Descricao == request.Descricao).FirstOrDefault();

        if (assuntoAlreadyExists != null)
        {
            errors.Add("Assunto já existe.");
        }

        if (string.IsNullOrEmpty(request.Descricao))
        {
            errors.Add("Descrição é obrigatória.");
        }

        if (errors.Count != 0)
        {
            return Result.Failure(errors);
        }

        Assunto entity = new Assunto()
        {
            Descricao = request.Descricao
        };

        _assuntoRepository.Insert(entity);
        _assuntoRepository.SaveChanges();

        return Result.Success();
    }
}
