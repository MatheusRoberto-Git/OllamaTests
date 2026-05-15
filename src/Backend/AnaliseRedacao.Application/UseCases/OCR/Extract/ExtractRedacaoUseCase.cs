using AnaliseRedacao.Communication.Request;
using AnaliseRedacao.Communication.Response;
using AnaliseRedacao.Domain.Extensions;
using AnaliseRedacao.Domain.Services.OCR;
using AnaliseRedacao.Exception.ExceptionsBase;

namespace AnaliseRedacao.Application.UseCases.OCR.Extract
{
    public class ExtractRedacaoUseCase : IExtractRedacaoUseCase
    {
        private readonly IExtractTextFromPdfService _ocr;

        public ExtractRedacaoUseCase(IExtractTextFromPdfService ocr)
        {
            _ocr = ocr;
        }

        public async Task<ResponseExtractedRedacaoJson> Execute(RequestExtractRedacaoFormData request)
        {
            Validate(request);

            await using var stream = request.PDF!.OpenReadStream();
            var extractedText = await _ocr.Extract(stream);

            return new ResponseExtractedRedacaoJson
            {
                ExtractedText = extractedText
            };
        }

        private static void Validate(RequestExtractRedacaoFormData request)
        {
            var result = new PDFValidator().Validate(request);

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}