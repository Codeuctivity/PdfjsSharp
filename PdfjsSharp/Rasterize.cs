using Jering.Javascript.NodeJS;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Codeuctivity
{
    /// <summary>
    /// Rasterizes PDFs, Depands on node
    /// </summary>
    public static class Rasterize
    {
        /// <summary>
        /// Converts a pdf to pngs
        /// </summary>
        /// <param name="pathToPdf"></param>
        /// <param name="pathToPngOutput">Prefix of file path to pngs created for each page of pdf. E.g. c:\temp\PdfPage will result in c:\temp\PdfPage1.png and c:\temp\PdfPage2.png for a Pdf with two pages.</param>
        /// <returns>Collection of paths to pngs for each page in the pdf</returns>
        public static async Task<IReadOnlyList<string>> ConvertToPngAsync(string pathToPdf, string pathToPngOutput)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Codeuctivity.PdfjsSharp.Rasterize.js");
            using var reader = new StreamReader(stream);
            var pdfRasterizerJsPath = reader.ReadToEnd();

            var pathsToPngOfEachPage = new List<string>();
            // Invoke javascript in Node.js
            var pageQuantiy = await StaticNodeJSService.InvokeFromStringAsync<int>(pdfRasterizerJsPath, args: new object[] { pathToPdf, pathToPngOutput }).ConfigureAwait(false);

            for (var pagenumber = 1; pagenumber <= pageQuantiy; pagenumber++)
            {
                pathsToPngOfEachPage.Add($"{pathToPngOutput}{pagenumber}.png");
            }

            return pathsToPngOfEachPage.AsReadOnly();
        }
    }
}