using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DemoChart.Utilities
{
    public class PdfUtility
    {
        // Create a simple Pdf document and add an image to it.
        public static MemoryStream GetSimplePdf(MemoryStream chartImage)
        {
            const int documentMargin = 10;

            var pdfStream = new MemoryStream();
            var pdfDocument = new Document(PageSize.LETTER);
            pdfDocument.SetMargins(documentMargin, documentMargin,
                                   documentMargin, documentMargin);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, pdfStream);

            Image image = Image.GetInstance(chartImage.GetBuffer());
            image.SetAbsolutePosition(documentMargin
                , pdfDocument.PageSize.Height - documentMargin - image.ScaledHeight);

            pdfDocument.Open();
            pdfDocument.Add(image);
            pdfDocument.Close();
            pdfWriter.Flush();

            return pdfStream;
        }
    }
}