using Application.DataTransferObjects.HandleAssunto;
using Application.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AssuntosController : BaseController
{
    private readonly IAssuntoService _assuntoService;
    public AssuntosController(IAssuntoService assuntoService) 
        => _assuntoService = assuntoService ?? throw new ArgumentNullException(nameof(assuntoService));

    [HttpGet]
    public IActionResult Get()
    {
        var result = _assuntoService.GetAll();

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _assuntoService.GetById(id);

        return HandleResultById(result);
    }

    [HttpPost]
    public IActionResult Post(AssuntoDTO request)
    {
        var result = _assuntoService.Create(request);

        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, AssuntoDTO request)
    {
        var result = _assuntoService.Update(id, request);

        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _assuntoService.Delete(id);

        return HandleResult(result);
    }
}