using Jering.Javascript.NodeJS;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Codeuctivity.PdfjsSharp
{
    /// <summary>
    /// Rasterizes PDFs, depends on node
    /// </summary>
    public class Rasterizer : PdfJsWrapper, IRasterizer
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public Rasterizer()
        {
        }

        /// <summary>
        /// Ctor used with custom NodeExecuteablePath
        /// </summary>
        /// <param name="nodeExecuteablePath"></param>
        public Rasterizer(string nodeExecuteablePath) : base(nodeExecuteablePath)
        {
        }

        /// <summary>
        /// Converts a PDF to PNGs
        /// </summary>
        /// <param name="pathToPdf"></param>
        /// <param name="pathToPngOutput">Prefix of file path to PNGs created for each page of PDF. E.g. c:\temp\PdfPage will result in c:\temp\PdfPage1.png and c:\temp\PdfPage2.png for a PDF with two pages.</param>
        /// <returns>Collection of paths to PNGs for each page in the PDF</returns>
        public async Task<IReadOnlyList<string>> ConvertToPngAsync(string pathToPdf, string pathToPngOutput)
        {
            await InitPdfJsWrapper().ConfigureAwait(false);
            var pathToRasterizeJs = Path.Combine(pathToTempFolder, "Rasterize.mjs");

            var pathsToPngOfEachPage = new List<string>();
            var pageQuantity = await StaticNodeJSService.InvokeFromFileAsync<int>(pathToRasterizeJs, "convertToPng", args: new object[] { pathToPdf, pathToPngOutput }).ConfigureAwait(false);
            for (var pagenumber = 1; pagenumber <= pageQuantity; pagenumber++)
            {
                pathsToPngOfEachPage.Add($"{pathToPngOutput}{pagenumber}.png");
            }

            return pathsToPngOfEachPage.AsReadOnly();
        }
    }
}