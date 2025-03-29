using Application.DataTransferObjects.HandleAssunto;
using Application.DataTransferObjects.HandleAutor;

namespace Application.DataTransferObjects.HandleLivro;

public class GetLivroDTO
{
    public int Codl { get; set; }
    public string Titulo { get; set; }
    public string Editora { get; set; }
    public int Edicao { get; set; }
    public string AnoPublicacao { get; set; }
    public ICollection<AutorDTO> Autores { get; set; }
    public ICollection<AssuntoDTO> Assuntos { get; set; }
    public ICollection<LivroPrecoDTO> Precos { get; set; }
}