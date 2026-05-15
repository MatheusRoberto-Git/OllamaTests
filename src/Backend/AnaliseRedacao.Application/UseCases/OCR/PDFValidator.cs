using AnaliseRedacao.Communication.Request;
using AnaliseRedacao.Domain.ValueObjects;
using AnaliseRedacao.Exception;
using FluentValidation;

namespace AnaliseRedacao.Application.UseCases.OCR
{
    public class PDFValidator : AbstractValidator<RequestExtractRedacaoFormData>
    {
        public PDFValidator()
        {
            RuleFor(x => x.PDF)
                .NotNull()
                .WithMessage(ResourceMessagesException.PDF_REQUIRED);

            When(x => x.PDF is not null, () =>
            {
                RuleFor(x => x.PDF!.ContentType)
                .Equal("application/pdf")
                .WithMessage(ResourceMessagesException.ONLY_PDF_ACCEPTED);

                RuleFor(x => x.PDF!.Length)
                .LessThanOrEqualTo(AnaliseRedacaoRuleConstants.MAX_PDF_SIZE_BYTES)
                .WithMessage(ResourceMessagesException.PDF_TOO_LARGE);
            });
        }
    }
}
