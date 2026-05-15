using AnaliseRedacao.Domain.Services.OCR;
using AnaliseRedacao.Infrastructure.Services.OCR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnaliseRedacao.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddOllama(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            
        }

        private static void AddRepositories(IServiceCollection services)
        {
            
        }

        private static void AddOllama(IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("Settings:Ollama:BaseUrl")!;

            services.AddHttpClient<IExtractTextFromPdfService, OllamaOCRService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });
        }
    }
}
