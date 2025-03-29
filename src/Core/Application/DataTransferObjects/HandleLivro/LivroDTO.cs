namespace Application.DataTransferObjects.HandleLivro;

public class LivroDTO
{
    public string Titulo { get; set; }
    public string Editora { get; set; }
    public int Edicao { get; set; }
    public string AnoPublicacao { get; set; }
    public List<int> Autores { get; set; }
    public List<int> Assuntos { get; set; }
}