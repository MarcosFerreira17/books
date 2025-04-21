using Application.DataTransferObjects.HandleLivro;
using Application.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LivroPrecosController : BaseController
{

    private readonly ILivroPrecoService _livroPrecoService;
    public LivroPrecosController(ILivroPrecoService livroPrecoService)
    {
        _livroPrecoService = livroPrecoService ?? throw new ArgumentNullException(nameof(livroPrecoService));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = _livroPrecoService.GetAll();

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _livroPrecoService.GetById(id);

        return HandleResultById(result);
    }

    [HttpPost]
    public IActionResult Post(LivroPrecoDTO request)
    {
        var result = _livroPrecoService.Create(request);

        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, LivroPrecoDTO request)
    {
        var result = _livroPrecoService.Update(id, request);

        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _livroPrecoService.Delete(id);

        return HandleResult(result);
    }
}