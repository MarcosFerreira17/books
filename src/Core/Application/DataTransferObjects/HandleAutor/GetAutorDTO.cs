
using Application.DataTransferObjects.HandleLivro;
using System.Text.Json.Serialization;

namespace Application.DataTransferObjects.HandleAutor;

public class GetAutorDTO
{
    public int CodAu { get; set; }
    public string Nome { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<LivroDTO> Livros { get; set; }
}

