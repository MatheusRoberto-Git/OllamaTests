using System.Net;

namespace AnaliseRedacao.Exception.ExceptionsBase
{
    public class ErrorOnValidationException : AnaliseRedacaoException
    {
        private readonly IList<string> _errors;

        public ErrorOnValidationException(IList<string> errors) : base(string.Empty) => _errors = errors;

        public override IList<string> GetErrorMessages() => _errors;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}