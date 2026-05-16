using System.Text.Json.Serialization;

namespace AnaliseRedacao.Communication.Response
{
    public class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; } = string.Empty;
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
