using Application.DataTransferObjects.HandleAssunto;
using Application.DataTransferObjects.HandleAutor;
using Application.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AutoresController : BaseController
{
    private readonly IAutorService _autorService;
    public AutoresController(IAutorService autorService)
    {
        _autorService = autorService ?? throw new ArgumentNullException(nameof(autorService));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = _autorService.GetAll();

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _autorService.GetById(id);

        return HandleResultById(result);
    }

    [HttpPost]
    public IActionResult Post(AutorDTO request)
    {
        var result = _autorService.Create(request);

        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, AutorDTO request)
    {
        var result = _autorService.Update(id, request);

        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _autorService.Delete(id);

        return HandleResult(result);
    }
}