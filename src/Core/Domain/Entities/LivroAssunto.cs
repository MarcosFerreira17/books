using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Livro_Assunto")]
public class LivroAssunto
{
    [Column("Livro_Codl")]
    public int LivroCodl { get; set; }
    public Livro Livro { get; set; }

    [Column("Assunto_CodAs")]
    public int AssuntoCodAs { get; set; }
    public Assunto Assunto { get; set; }
}