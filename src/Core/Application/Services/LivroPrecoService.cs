using Application.DataTransferObjects;
using Application.DataTransferObjects.HandleLivro;
using Application.Enums;
using Application.Services.Interfaces;
using Domain.Entities;
using Infra.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class LivroPrecoService : ILivroPrecoService
{
    private readonly ILivroPrecoRepository _livroPrecoRepository;
    private readonly ILivroRepository _livroRepository;

    public LivroPrecoService(
        ILivroPrecoRepository livroPrecoRepository,
        ILivroRepository livroRepository)
    {
        _livroPrecoRepository = livroPrecoRepository;
        _livroRepository = livroRepository;
    }

    public Result Create(LivroPrecoDTO request)
    {
        var errors = ValidateLivroPreco(request);
     
        if (errors.Any())
            return Result.Failure(errors);

        var livro = _livroRepository.Query(column => column.Codl == request.LivroCodl).FirstOrDefault();

        if (livro == null)
            return Result.Failure("Livro não encontrado.");

        var preco = new LivroPreco
        {
            LivroCodl = request.LivroCodl,
            TipoCompra = request.TipoCompra,
            Valor = request.Valor
        };

        _livroPrecoRepository.Insert(preco);
        _livroPrecoRepository.SaveChanges();

        return Result.Success();
    }

    public Result Delete(int cod)
    {
        var preco = _livroPrecoRepository.Query()
            .FirstOrDefault(p => p.Codp == cod);

        if (preco == null)
            return Result.Failure("Preço não encontrado.");

        _livroPrecoRepository.Delete(preco);
        _livroPrecoRepository.SaveChanges();

        return Result.Success();
    }

    public ResultGeneric<IEnumerable<GetLivroPrecoDTO>> GetAll()
    {
        var precos = _livroPrecoRepository.Query()
            .Include(p => p.Livro)
            .Select(p => new GetLivroPrecoDTO
            {
                Codp = p.Codp,
                LivroCodl = p.LivroCodl,
                TipoCompra = p.TipoCompra,
                Valor = p.Valor
            })
            .ToList();

        return ResultGeneric<IEnumerable<GetLivroPrecoDTO>>.Success(precos);
    }

    public ResultGeneric<GetLivroPrecoDTO> GetById(int cod)
    {
        var preco = _livroPrecoRepository.Query()
            .Include(p => p.Livro)
            .Select(p => new GetLivroPrecoDTO
            {
                Codp = p.Codp,
                LivroCodl = p.LivroCodl,
                TipoCompra = p.TipoCompra,
                Valor = p.Valor
            })
            .FirstOrDefault(p => p.Codp == cod);

        if (preco == null)
            return ResultGeneric<GetLivroPrecoDTO>.Failure("Preço não encontrado.");

        return ResultGeneric<GetLivroPrecoDTO>.Success(preco);
    }

    public Result Update(int cod, LivroPrecoDTO request)
    {
        var errors = ValidateLivroPreco(request);
        var preco = _livroPrecoRepository.Query()
            .FirstOrDefault(p => p.Codp == cod);

        if (preco == null)
            errors.Add("Preço não encontrado.");

        if (errors.Any())
            return Result.Failure(errors);

        var livro = _livroRepository.Query(column => column.Codl == request.LivroCodl).FirstOrDefault();

        if (livro == null)
            return Result.Failure("Livro não encontrado.");

        preco.LivroCodl = request.LivroCodl;
        preco.TipoCompra = request.TipoCompra;
        preco.Valor = request.Valor;

        _livroPrecoRepository.Update(preco);
        _livroPrecoRepository.SaveChanges();

        return Result.Success();
    }

    private List<string> ValidateLivroPreco(LivroPrecoDTO request)
    {
        var errors = new List<string>();

        if (request.LivroCodl <= 0)
            errors.Add("Código do livro inválido.");

        if (string.IsNullOrEmpty(request.TipoCompra))
            errors.Add("Tipo de compra é obrigatório.");

        if (!Enum.IsDefined(typeof(TipoCompra), request.TipoCompra))
            errors.Add("Tipo de compra inválido.");

        if (request.Valor <= 0)
            errors.Add("Valor deve ser maior que zero.");

        return errors;
    }
}