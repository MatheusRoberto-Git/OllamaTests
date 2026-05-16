using AnaliseRedacao.Communication.Request;
using AnaliseRedacao.Communication.Response;
using AnaliseRedacao.Domain.Services.HelperServices;
using AnaliseRedacao.Domain.Services.OCR;
using AnaliseRedacao.Domain.ValueObjects;
using AnaliseRedacao.Exception;
using AnaliseRedacao.Exception.ExceptionsBase;
using AnaliseRedacao.Infrastructure.Settings;
using System.Net.Http.Json;

namespace AnaliseRedacao.Infrastructure.Services.OCR
{
    public class OllamaOCRService : IExtractTextFromPdfService
    {
        private readonly HttpClient _httpClient;
        private readonly OllamaSettings _settings;

        public OllamaOCRService(HttpClient httpClient, OllamaSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<string> Extract(Stream pdfStream)
        {
            var pdfBytes = await PdfPageConverter.ReadAllBytesAsync(pdfStream);
            var pageImages = PdfPageConverter.ToJpegImages(pdfBytes);
            var pages = new List<string>(pageImages.Count);

            foreach (var imageBytes in pageImages)
            {
                var base64 = Convert.ToBase64String(imageBytes);
                var text = await ExtractTextFromImage(base64);
                pages.Add(text);
            }

            return string.Join("\n\n", pages);
        }

        private async Task<string> ExtractTextFromImage(string base64Image)
        {
            var body = new OllamaRequest
            {
                Model = AnaliseRedacaoRuleConstants.OLLAMA_OCR_MODEL,
                Prompt = _settings.OcrPrompt,
                Images = new List<string> { base64Image },
                Stream = false
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/generate", body);
                var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();

                if (!string.IsNullOrWhiteSpace(result?.Error))
                    throw new ExternalServiceException($"{ResourceMessagesException.OCR_SERVICE_ERROR}: {result.Error}");

                if (!response.IsSuccessStatusCode)
                    throw new ExternalServiceException(ResourceMessagesException.OCR_SERVICE_ERROR);

                return result?.Response ?? string.Empty;
            }
            catch (ExternalServiceException)
            {
                throw;
            }
            catch (TaskCanceledException)
            {
                throw new ExternalServiceException(ResourceMessagesException.OCR_SERVICE_TIMEOUT);
            }
            catch (HttpRequestException)
            {
                throw new ExternalServiceException(ResourceMessagesException.OCR_SERVICE_ERROR);
            }
        }
    }
}
