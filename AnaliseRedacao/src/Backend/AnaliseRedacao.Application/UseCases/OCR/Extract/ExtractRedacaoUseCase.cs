using AnaliseRedacao.Communication.Request;
using AnaliseRedacao.Communication.Response;
using AnaliseRedacao.Domain.Extensions;
using AnaliseRedacao.Domain.Services.OCR;
using AnaliseRedacao.Exception.ExceptionsBase;
using FluentValidation;

namespace AnaliseRedacao.Application.UseCases.OCR.Extract
{
    public class ExtractRedacaoUseCase : IExtractRedacaoUseCase
    {
        private readonly IExtractTextFromPdfService _ocr;
        private readonly IValidator<RequestExtractRedacaoFormData> _validator;

        public ExtractRedacaoUseCase(IExtractTextFromPdfService ocr, IValidator<RequestExtractRedacaoFormData> validator)
        {
            _ocr = ocr;
            _validator = validator;
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

        private void Validate(RequestExtractRedacaoFormData request)
        {
            var result = _validator.Validate(request);

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}