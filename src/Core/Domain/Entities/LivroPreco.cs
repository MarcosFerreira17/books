using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class LivroPreco
{
    public int Codp { get; set; }
    public string TipoCompra { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }
    
    [Column("Livro_Codl")]
    public int LivroCodl { get; set; }
    public Livro Livro { get; set; }
}