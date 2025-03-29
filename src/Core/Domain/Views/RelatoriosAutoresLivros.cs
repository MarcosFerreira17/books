using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Views;

public class RelatorioAutoresLivros
{
    public string Autor { get; set; }
    public string Livro { get; set; }
    public string Assuntos { get; set; }
    public int TotalLivros { get; set; }
}