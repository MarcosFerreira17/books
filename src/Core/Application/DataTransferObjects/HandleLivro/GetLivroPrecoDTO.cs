namespace Application.DataTransferObjects.HandleLivro;

public class GetLivroPrecoDTO
{
    public int Codp { get; set; }
    public string TipoCompra { get; set; }
    public decimal Valor { get; set; }
    public int LivroCodl { get; set; }
}