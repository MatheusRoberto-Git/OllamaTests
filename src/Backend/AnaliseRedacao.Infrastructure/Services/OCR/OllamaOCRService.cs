using AnaliseRedacao.Communication.Request;
using AnaliseRedacao.Communication.Response;
using AnaliseRedacao.Domain.Services.OCR;
using AnaliseRedacao.Domain.ValueObjects;
using AnaliseRedacao.Exception;
using AnaliseRedacao.Exception.ExceptionsBase;
using Docnet.Core;
using Docnet.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System.Net.Http.Json;

namespace AnaliseRedacao.Infrastructure.Services.OCR
{
    public class OllamaOCRService : IExtractTextFromPdfService
    {
        private readonly HttpClient _httpClient;

        public OllamaOCRService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Extract(Stream pdfStream)
        {
            var pdfBytes = await ReadAllBytesAsync(pdfStream);
            var pageImages = ConvertPdfToImages(pdfBytes);
            var pages = new List<string>(pageImages.Count);

            foreach (var imageBytes in pageImages)
            {
                var base64 = Convert.ToBase64String(imageBytes);
                var text = await ExtractTextFromImage(base64);

                pages.Add(text);
            }

            return string.Join("\n\n", pages);
        }        

        private static async Task<byte[]> ReadAllBytesAsync(Stream stream)
        {
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }

        private static IList<Byte[]> ConvertPdfToImages(byte[] pdfBytes)
        {
            using var library = DocLib.Instance;
            using var docReader = library.GetDocReader(pdfBytes, new PageDimensions(scalingFactor: 1.0));
            var pageCount = docReader.GetPageCount();
            var images = new List<byte[]>(pageCount);

            for (var i = 0; i < pageCount; i++)
            {
                using var pageReader = docReader.GetPageReader(i);
                var rawBytes = pageReader.GetImage();
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();

                images.Add(EncodeToJpeg(rawBytes, width, height));
            }

            return images;
        }

        private static byte[] EncodeToPng(byte[] bgraBytes, int width, int height)
        {
            using var image = Image.LoadPixelData<Bgra32>(bgraBytes, width, height);
            using var ms = new MemoryStream();

            image.SaveAsPng(ms);

            return ms.ToArray();
        }

        private static byte[] EncodeToJpeg(byte[] bgraBytes, int width, int height)
        {
            using var image = Image.LoadPixelData<Bgra32>(bgraBytes, width, height);
            using var ms = new MemoryStream();

            image.SaveAsJpeg(ms, new JpegEncoder
            {
                Quality = 80
            });

            return ms.ToArray();
        }

        private async Task<string> ExtractTextFromImage(string base64Image)
        {
            var body = new OllamaRequest
            {
                Model = AnaliseRedacaoRuleConstants.OLLAMA_OCR_MODEL,
                Prompt = "Extract all the text from this image exactly as written. Return only the extracted text.",
                Images = new string[] { base64Image },
                Stream = false
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/generate", body);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
                return result?.Response ?? string.Empty;
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
