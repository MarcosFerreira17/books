
using Application.DataTransferObjects.HandleLivro;

namespace Application.DataTransferObjects.HandleAutor;

public class GetAutorDTO
{
    public int CodAu { get; set; }
    public string Nome { get; set; }
    public ICollection<LivroDTO> Livros { get; set; }
}

