using Microsoft.AspNetCore.Http;

namespace AnaliseRedacao.Communication.Request
{
    public class RequestExtractRedacaoFormData
    {
        public IFormFile? PDF { get; set; }
    }
}