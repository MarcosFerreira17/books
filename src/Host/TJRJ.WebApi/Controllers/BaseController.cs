namespace TJRJ.WebApi.Controllers;

using Application.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

public class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(ResultGeneric<T> result)
    {
        if (result == null)
            return Problem("Ocorreu um erro inesperado."); // 500

        if (result.IsSuccess)
            return Ok(result.Value); // 200

        return BadRequest(result.Errors); // 400
    }

    // Para operações sem retorno (Result)
    protected IActionResult HandleResult(Result result)
    {
        if (result == null)
            return Problem("Ocorreu um erro inesperado."); // 500

        if (result.IsSuccess)
            return Ok(); // 200

        return BadRequest(result.Errors); // 400
    }

    // Validação automática do ModelState
    protected IActionResult ValidateModelState()
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(errors); // 400
        }

        return null; // Continua a execução
    }
}