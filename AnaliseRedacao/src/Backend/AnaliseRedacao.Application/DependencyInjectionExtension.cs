using AnaliseRedacao.Application.UseCases.OCR;
using AnaliseRedacao.Application.UseCases.OCR.Extract;
using AnaliseRedacao.Communication.Request;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnaliseRedacao.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddUseCases(services);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IValidator<RequestExtractRedacaoFormData>, PDFValidator>();
            services.AddScoped<IExtractRedacaoUseCase, ExtractRedacaoUseCase>();
        }
    }
}