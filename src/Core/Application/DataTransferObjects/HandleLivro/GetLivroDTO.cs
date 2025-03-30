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
    public ICollection<GetAutorDTO> Autores { get; set; }
    public ICollection<GetAssuntoDTO> Assuntos { get; set; }
    public ICollection<GetLivroPrecoDTO> Precos { get; set; }
}