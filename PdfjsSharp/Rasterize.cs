using Jering.Javascript.NodeJS;
using System.IO;
using System.Threading.Tasks;

namespace PdfjsSharp
{
    /// <summary>
    /// Rasterizes PDFs
    /// </summary>
    public static class Rasterize
    {
        /// <summary>
        /// Converts a pdf to a png
        /// </summary>
        /// <param name="pathToPdf"></param>
        /// <param name="pathToPngOutput">Prefix of file path to pngs created for each page of pdf. E.g. c:\temp\PdfPage will result in c:\temp\PdfPage1.png and c:\temp\PdfPage2.png for a Pdf with two pages.</param>
        /// <returns></returns>
        public static async Task<int> ConvertToPngAsync(string pathToPdf, string pathToPngOutput)
        {
            // Invoke javascript in Node.js
            var pdfRasterizerJsPath = File.ReadAllText("Rasterize.js");
            var pageQuantiy = await StaticNodeJSService.InvokeFromStringAsync<int>(pdfRasterizerJsPath, args: new object[] { pathToPdf, pathToPngOutput }).ConfigureAwait(false);
            return pageQuantiy;
        }
    }
}