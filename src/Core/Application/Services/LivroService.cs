using Application.DataTransferObjects;
using Application.DataTransferObjects.HandleAssunto;
using Application.DataTransferObjects.HandleAutor;
using Application.DataTransferObjects.HandleLivro;
using Application.Services.Interfaces;
using Domain.Entities;
using Infra.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class LivroService : ILivroService
{
    private readonly ILivroRepository _livroRepository;
    private readonly ILivroPrecoRepository _precoLivroRepository;
    private readonly IAutorRepository _autorRepository;
    private readonly ILivroAutorRepository _livroAutorRepository;
    private readonly IAssuntoRepository _assuntoRepository;
    private readonly ILivroAssuntoRepository _livroAssuntoRepository;

    public LivroService(
        ILivroRepository livroRepository,
        IAutorRepository autorRepository,
        IAssuntoRepository assuntoRepository,
        ILivroAutorRepository livroAutorRepository,
        ILivroPrecoRepository precoLivroRepository,
        ILivroAssuntoRepository livroAssuntoRepository)
    {
        _livroRepository = livroRepository ?? throw new ArgumentNullException(nameof(livroRepository));
        _autorRepository = autorRepository ?? throw new ArgumentNullException(nameof(autorRepository));
        _assuntoRepository = assuntoRepository ?? throw new ArgumentNullException(nameof(assuntoRepository));
        _livroAutorRepository = livroAutorRepository ?? throw new ArgumentNullException(nameof(livroAutorRepository));
        _livroAssuntoRepository = livroAssuntoRepository ?? throw new ArgumentNullException(nameof(livroAssuntoRepository));
        _precoLivroRepository = precoLivroRepository ?? throw new ArgumentNullException(nameof(precoLivroRepository));
    }

    public ResultGeneric<IEnumerable<GetLivroDTO>> GetAll()
    {
        var livros = _livroRepository.Query()
            .Include(l => l.Autores)
                .ThenInclude(la => la.Autor)
            .Include(l => l.Assuntos)
                .ThenInclude(las => las.Assunto)
            .Include(l => l.Precos)
            .Select(l => new GetLivroDTO
            {
                Codl = l.Codl,
                Titulo = l.Titulo,
                Editora = l.Editora,
                Edicao = l.Edicao,
                AnoPublicacao = l.AnoPublicacao,
                Autores = l.Autores.Select(la => new GetAutorDTO { CodAu = la.Autor.CodAu, Nome = la.Autor.Nome }).ToList(),
                Assuntos = l.Assuntos.Select(las => new GetAssuntoDTO { CodAs = las.Assunto.CodAs ,Descricao = las.Assunto.Descricao }).ToList(),
                Precos = l.Precos.Select(las => new GetLivroPrecoDTO { Codp = las.Codp, TipoCompra = las.TipoCompra, Valor = las.Valor }).ToList()
            })
            .ToList();

        return ResultGeneric<IEnumerable<GetLivroDTO>>.Success(livros);
    }

    public ResultGeneric<GetLivroDTO> GetById(int cod)
    {
        var livro = _livroRepository.Query()
            .Include(l => l.Autores)
                .ThenInclude(la => la.Autor)
            .Include(l => l.Assuntos)
                .ThenInclude(las => las.Assunto)
            .Include(l => l.Precos)
            .Select(l => new GetLivroDTO
            {
                Codl = l.Codl,
                Titulo = l.Titulo,
                Editora = l.Editora,
                Edicao = l.Edicao,
                AnoPublicacao = l.AnoPublicacao,
                Autores = l.Autores.Select(la => new GetAutorDTO { CodAu = la.Autor.CodAu, Nome = la.Autor.Nome }).ToList(),
                Assuntos = l.Assuntos.Select(las => new GetAssuntoDTO { CodAs = las.Assunto.CodAs , Descricao = las.Assunto.Descricao }).ToList(),
                Precos = l.Precos.Select(las => new GetLivroPrecoDTO { LivroCodl = las.LivroCodl, Codp = las.Codp,TipoCompra = las.TipoCompra, Valor = las.Valor }).ToList()
            })
            .FirstOrDefault(l => l.Codl == cod);

        if (livro == null)
            return ResultGeneric<GetLivroDTO>.Failure("Livro não encontrado.");

        return ResultGeneric<GetLivroDTO>.Success(livro);
    }

    public ResultGeneric<int> Create(LivroDTO request)
    {
        var errors = ValidateLivro(request);

        if (errors.Any())
            return ResultGeneric<int>.Failure(errors);

        var livro = new Livro
        {
            Titulo = request.Titulo,
            Editora = request.Editora,
            Edicao = request.Edicao,
            AnoPublicacao = request.AnoPublicacao
        };

        AddRelacionamentos(livro, request.Autores, request.Assuntos);

        var entity = _livroRepository.Insert(livro);
        _livroRepository.SaveChanges();

        return ResultGeneric<int>.Success(entity.Codl);
    }

    public Result Update(int cod, LivroDTO request)
    {
        var errors = ValidateLivro(request);

        var livro = _livroRepository.Query()
            .Include(l => l.Autores)
            .Include(l => l.Assuntos)
            .FirstOrDefault(l => l.Codl == cod);

        if (livro == null)
            errors.Add("Livro não encontrado.");

        if (errors.Any())
            return Result.Failure(errors);

        UpdateLivro(livro, request);
        UpdateRelacionamentos(livro, request.Autores, request.Assuntos);

        _livroRepository.Update(livro);
        _livroRepository.SaveChanges();

        return Result.Success();
    }

    public Result Delete(int cod)
    {
        var livro = _livroRepository.Query()
                                    .Include(l => l.Assuntos)
                                    .Include(l => l.Autores)  
                                    .Include(l => l.Precos)
                                    .FirstOrDefault(l => l.Codl == cod);

        if (livro == null)
            return Result.Failure("Livro não encontrado.");

        // Remove todos os relacionamentos
        _livroAssuntoRepository.Delete(livro.Assuntos);
        _livroAutorRepository.Delete(livro.Autores);
        _precoLivroRepository.Delete(livro.Precos);

        _livroRepository.Delete(livro);
        _livroRepository.SaveChanges();

        return Result.Success();
    }

    private List<string> ValidateLivro(LivroDTO request)
    {
        var errors = new List<string>();

        if (request.Autores.Count == 0)
            errors.Add("Pelo menos um autor é obrigatório.");

        if (request.Assuntos.Count == 0)
            errors.Add("Pelo menos um assunto é obrigatório.");

        if (string.IsNullOrEmpty(request.Titulo))
            errors.Add("Título é obrigatório.");

        if (string.IsNullOrEmpty(request.Editora))
            errors.Add("Editora é obrigatória.");

        if (!int.TryParse(request.AnoPublicacao, out int ano))
            errors.Add($"{ano} Ano de publicação inválido.");

        return errors;
    }

    private void AddRelacionamentos(Livro livro, List<int> autoresIds, List<int> assuntosIds)
    {
        foreach (var autorId in autoresIds)
        {
            livro.Autores.Add(new LivroAutor
            {
                LivroCodl = livro.Codl,
                AutorCodAu = autorId
            });
        }

        foreach (var assuntoId in assuntosIds)
        {
            livro.Assuntos.Add(new LivroAssunto
            {
                LivroCodl = livro.Codl,
                AssuntoCodAs = assuntoId
            });
        }
    }

    private void UpdateRelacionamentos(Livro livro, List<int> novosAutores, List<int> novosAssuntos)
    {
        // Atualizar Autores
        var autoresAtuais = livro.Autores.Select(la => la.AutorCodAu).ToList();
        var autoresParaRemover = autoresAtuais.Except(novosAutores).ToList();

        // Remover autores (usando ToList() para evitar modificação durante a iteração)
        foreach (var autorId in autoresParaRemover.ToList())
        {
            var autorParaRemover = livro.Autores.First(la => la.AutorCodAu == autorId);
            livro.Autores.Remove(autorParaRemover);
        }

        // Adicionar novos autores
        var autoresParaAdicionar = novosAutores.Except(autoresAtuais).ToList();
        foreach (var autorId in autoresParaAdicionar)
        {
            livro.Autores.Add(new LivroAutor
            {
                LivroCodl = livro.Codl,
                AutorCodAu = autorId
            });
        }

        // Atualizar Assuntos
        var assuntosAtuais = livro.Assuntos.Select(las => las.AssuntoCodAs).ToList();
        var assuntosParaRemover = assuntosAtuais.Except(novosAssuntos).ToList();

        // Remover assuntos
        foreach (var assuntoId in assuntosParaRemover.ToList())
        {
            var assuntoParaRemover = livro.Assuntos.First(las => las.AssuntoCodAs == assuntoId);
            livro.Assuntos.Remove(assuntoParaRemover);
        }

        // Adicionar novos assuntos
        var assuntosParaAdicionar = novosAssuntos.Except(assuntosAtuais).ToList();
        foreach (var assuntoId in assuntosParaAdicionar)
        {
            livro.Assuntos.Add(new LivroAssunto
            {
                LivroCodl = livro.Codl,
                AssuntoCodAs = assuntoId
            });
        }
    }

    private void UpdateLivro(Livro livro, LivroDTO request)
    {
        livro.Titulo = request.Titulo;
        livro.Editora = request.Editora;
        livro.Edicao = request.Edicao;
        livro.AnoPublicacao = request.AnoPublicacao;
    }
}