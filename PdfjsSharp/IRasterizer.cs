using System.Collections.Generic;
using System.Threading.Tasks;

namespace Codeuctivity.PdfjsSharp
{
    /// <summary>
    /// Rasterizes PDFs, Depands on node
    /// </summary>
    public interface IRasterizer
    {
        /// <summary>
        /// Converts a pdf to pngs
        /// </summary>
        /// <param name="pathToPdf"></param>
        /// <param name="pathToPngOutput">Prefix of file path to pngs created for each page of pdf. E.g. c:\temp\PdfPage will result in c:\temp\PdfPage1.png and c:\temp\PdfPage2.png for a Pdf with two pages.</param>
        /// <returns>Collection of paths to pngs for each page in the pdf</returns>
        Task<IReadOnlyList<string>> ConvertToPngAsync(string pathToPdf, string pathToPngOutput);
    }
}