namespace AnaliseRedacao.Domain.Services.OCR
{
    public interface IExtractTextFromPdfService
    {
        public Task<string> Extract(Stream pdfStream);
    }
}