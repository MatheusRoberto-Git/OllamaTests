using AnaliseRedacao.Communication.Response;
using AnaliseRedacao.Exception;
using AnaliseRedacao.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AnaliseRedacao.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AnaliseRedacaoException analiseRedacaoException)
            {
                HandleProjectException(analiseRedacaoException, context);
            }
            else
            {
                ThrowUnknowException(context);
            }
        }        

        private static void HandleProjectException(AnaliseRedacaoException analiseRedacaoException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)analiseRedacaoException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(analiseRedacaoException.GetErrorMessages()));
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOW_ERROR));
        }
    }
}
