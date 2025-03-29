using Application.DataTransferObjects.HandleAssunto;
using Application.DataTransferObjects.HandleAutor;
using Application.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace TJRJ.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RelatorioController : BaseController
{
    private readonly IRelatorioService _relatorioService;
    public RelatorioController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService ?? throw new ArgumentNullException(nameof(relatorioService));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var pdfBytes = _relatorioService.GerarRelatorioPDF();

        return File(pdfBytes, "application/pdf", "RelatorioLivrosPorAutor.pdf");
    }

}