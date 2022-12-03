using System.Drawing.Imaging;
using System.IO;

namespace BLL
{
    public class PdfService
    {
        public PdfService()
        {

        }
        public void UnwrapImage(Stream docStream, int page, Stream imgStream, ImageFormat format = null)
        {
            var imgFormat = format ?? ImageFormat.Jpeg;

            using PdfiumViewer.PdfDocument pdf = PdfiumViewer.PdfDocument.Load(docStream);
            pdf.Render(page - 1, 50f, 50f, true).Save(imgStream, imgFormat);
        }
    }
}
