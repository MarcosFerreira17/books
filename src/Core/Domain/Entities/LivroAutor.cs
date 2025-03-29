using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Livro_Autor")]
public class LivroAutor
{
    [Column("Livro_Codl")]
    public int LivroCodl { get; set; }
    public Livro Livro { get; set; }
    
    [Column("Autor_CodAu")]
    public int AutorCodAu { get; set; }
    public Autor Autor { get; set; }
}