namespace AnaliseRedacao.Infrastructure.Settings
{
    public class OllamaSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 300;
        public string OcrPrompt { get; set; } = string.Empty;
    }
}
