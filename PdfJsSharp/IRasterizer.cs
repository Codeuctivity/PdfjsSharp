using System.Collections.Generic;
using System.Threading.Tasks;

namespace Codeuctivity.PdfjsSharp
{
    /// <summary>
    /// Rasterizes PDFs, Depends on node
    /// </summary>
    public interface IRasterizer
    {
        /// <summary>
        /// Converts a PDF to PNGs
        /// </summary>
        /// <param name="pathToPdf"></param>
        /// <param name="pathToPngOutput">Prefix of file path to PNGs created for each page of pdf. E.g. c:\temp\PdfPage will result in c:\temp\PdfPage1.PNG and c:\temp\PdfPage2.PNG for a Pdf with two pages.</param>
        /// <returns>Collection of paths to PNGs for each page in the pdf</returns>
        Task<IReadOnlyList<string>> ConvertToPngAsync(string pathToPdf, string pathToPngOutput);
    }
}