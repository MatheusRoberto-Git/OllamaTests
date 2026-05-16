using System.Text.Json.Serialization;

namespace AnaliseRedacao.Communication.Request
{
    public class OllamaRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;
        [JsonPropertyName("images")]
        public IList<string> Images { get; set; } = new List<string>();
        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }
}
