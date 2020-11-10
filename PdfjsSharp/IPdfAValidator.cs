using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Codeuctivity
{
    /// <summary>
    /// PdfAValidator is a VeraPdf wrapper
    /// </summary>
    public interface IPdfAValidator : IDisposable
    {
        /// <summary>
        /// Converts each page of a pdf to a png
        /// </summary>
        /// <param name="pathToPdf"></param>
        /// <param name="pathToPngOutput"></param>
        /// <returns></returns>
        Task<IReadOnlyList<string>> ConvertToPngAsync(string pathToPdf, string pathToPngOutput);
    }
}