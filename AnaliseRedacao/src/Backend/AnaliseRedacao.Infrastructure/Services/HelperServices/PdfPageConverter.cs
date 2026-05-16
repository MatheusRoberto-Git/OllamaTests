using Docnet.Core;
using Docnet.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

namespace AnaliseRedacao.Domain.Services.HelperServices
{
    internal static class PdfPageConverter
    {
        internal static async Task<byte[]> ReadAllBytesAsync(Stream stream)
        {
            using var ms = new MemoryStream();

            await stream.CopyToAsync(ms);

            return ms.ToArray();
        }

        internal static IReadOnlyList<byte[]> ToJpegImages(byte[] pdfBytes)
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

        private static byte[] EncodeToJpeg(byte[] bgraBytes, int width, int height)
        {
            using var image = Image.LoadPixelData<Bgra32>(bgraBytes, width, height);
            using var ms = new MemoryStream();

            image.SaveAsJpeg(ms, new JpegEncoder { Quality = 80 });

            return ms.ToArray();
        }
    }
}
