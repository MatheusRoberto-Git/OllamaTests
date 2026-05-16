using AnaliseRedacao.Domain.Services.OCR;
using AnaliseRedacao.Infrastructure.Services.OCR;
using AnaliseRedacao.Infrastructure.Settings;
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
            var settings = configuration.GetSection("Settings:Ollama").Get<OllamaSettings>()!;

            services.AddSingleton(settings);

            services.AddHttpClient<IExtractTextFromPdfService, OllamaOCRService>(client =>
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
            });
        }
    }
}
