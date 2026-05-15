using System.Net;

namespace AnaliseRedacao.Exception.ExceptionsBase
{
    public class ExternalServiceException : AnaliseRedacaoException
    {
        public ExternalServiceException(string message) : base(message) { }

        public override IList<string> GetErrorMessages() => new string[] { Message };
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.ServiceUnavailable;
    }
}
