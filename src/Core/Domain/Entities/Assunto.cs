namespace Domain.Entities;

public class Assunto
{
    public int CodAs { get; set; }
    public string Descricao { get; set; }
    public ICollection<LivroAssunto> Livros { get; set; }
}
