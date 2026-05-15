using AnaliseRedacao.Communication.Request;
using AnaliseRedacao.Communication.Response;

namespace AnaliseRedacao.Application.UseCases.OCR.Extract
{
    public interface IExtractRedacaoUseCase
    {
        public Task<ResponseExtractedRedacaoJson> Execute(RequestExtractRedacaoFormData request);
    }
}
