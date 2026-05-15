using System.Net;

namespace AnaliseRedacao.Exception.ExceptionsBase
{
    public abstract class AnaliseRedacaoException : SystemException
    {
        protected AnaliseRedacaoException(string message) : base(message) { }

        public abstract IList<string> GetErrorMessages();

        public abstract HttpStatusCode GetStatusCode();
    }
}
