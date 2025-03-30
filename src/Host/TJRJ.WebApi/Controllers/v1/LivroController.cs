using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Application.DataTransferObjects.HandleLivro;

namespace TJRJ.WebApi.Controllers.v1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LivroController : BaseController
{
    private readonly ILivroService _livroService;
    public LivroController(ILivroService livroService)
    {
        _livroService = livroService ?? throw new ArgumentNullException(nameof(livroService));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = _livroService.GetAll();

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _livroService.GetById(id);

        return HandleResultById(result);
    }

    [HttpPost]
    public IActionResult Post(LivroDTO request)
    {
        var result = _livroService.Create(request);

        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, LivroDTO request)
    {
        var result = _livroService.Update(id, request);

        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _livroService.Delete(id);

        return HandleResult(result);
    }
}