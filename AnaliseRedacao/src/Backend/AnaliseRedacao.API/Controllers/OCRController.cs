using AnaliseRedacao.Application.UseCases.OCR.Extract;
using AnaliseRedacao.Communication.Request;
using Microsoft.AspNetCore.Mvc;

namespace AnaliseRedacao.API.Controllers;

public class OCRController : AnaliseRedacaoController
{
    [HttpPost("extract")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExtractRedacao([FromServices]IExtractRedacaoUseCase useCase, [FromForm]RequestExtractRedacaoFormData request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}