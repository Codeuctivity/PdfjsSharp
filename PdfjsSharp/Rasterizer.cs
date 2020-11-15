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
    public class Rasterizer : PdfJsWrapper, IRasterizer
    {
        /// <summary>
        /// Converts a pdf to pngs
        /// </summary>
        /// <param name="pathToPdf"></param>
        /// <param name="pathToPngOutput">Prefix of file path to pngs created for each page of pdf. E.g. c:\temp\PdfPage will result in c:\temp\PdfPage1.png and c:\temp\PdfPage2.png for a Pdf with two pages.</param>
        /// <returns>Collection of paths to pngs for each page in the pdf</returns>
        public async Task<IReadOnlyList<string>> ConvertToPngAsync(string pathToPdf, string pathToPngOutput)
        {
            await InitNodeModules().ConfigureAwait(false);
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Codeuctivity.PdfjsSharp.Rasterize.js");
            using var reader = new StreamReader(stream);
            var script = reader.ReadToEnd();

            var scriptWithAbsolutePathsToNodeModules = script.Replace("MagicPrefix", pathToNodeModules);
            var pdfRasterizerJsCodeToExecute = scriptWithAbsolutePathsToNodeModules;

            var pathsToPngOfEachPage = new List<string>();

            var pageQuantity = await StaticNodeJSService.InvokeFromStringAsync<int>(pdfRasterizerJsCodeToExecute, args: new object[] { pathToPdf, pathToPngOutput }).ConfigureAwait(false);

            for (var pagenumber = 1; pagenumber <= pageQuantity; pagenumber++)
            {
                pathsToPngOfEachPage.Add($"{pathToPngOutput}{pagenumber}.png");
            }

            return pathsToPngOfEachPage.AsReadOnly();
        }
    }
}