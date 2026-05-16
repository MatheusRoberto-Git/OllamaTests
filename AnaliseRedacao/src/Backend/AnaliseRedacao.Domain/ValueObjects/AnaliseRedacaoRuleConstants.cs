namespace AnaliseRedacao.Domain.ValueObjects
{
    public abstract class AnaliseRedacaoRuleConstants
    {
        public const long MAX_PDF_SIZE_BYTES = 10 * 1024 * 1024;
        public const string OLLAMA_OCR_MODEL = "glm-ocr";
    }
}