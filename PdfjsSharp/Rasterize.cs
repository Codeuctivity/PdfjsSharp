﻿using Jering.Javascript.NodeJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Codeuctivity
{
    /// <summary>
    /// Rasterizes PDFs, Depands on node
    /// </summary>
    public class Rasterize : IRasterize
    {
        private const int someMaxPathLenth = 206;
        private bool disposed;
        private bool useCustomNodeModulePath;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private bool IsInitialized { get; set; }
        private string pathToNodeModules = default!;

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

            var pageQuantiy = await StaticNodeJSService.InvokeFromStringAsync<int>(pdfRasterizerJsCodeToExecute, args: new object[] { pathToPdf, pathToPngOutput }).ConfigureAwait(false);

            for (var pagenumber = 1; pagenumber <= pageQuantiy; pagenumber++)
            {
                pathsToPngOfEachPage.Add($"{pathToPngOutput}{pagenumber}.png");
            }

            return pathsToPngOfEachPage.AsReadOnly();
        }

        private async Task InitNodeModules()
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (IsInitialized)
                {
                    return;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    pathToNodeModules = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                    if (pathToNodeModules.Length > someMaxPathLenth)
                    {
                        throw new PathTooLongException(pathToNodeModules);
                    }

                    Directory.CreateDirectory(pathToNodeModules);
                    await ExtractBinaryFromManifest("Codeuctivity.PdfjsSharp.node_modules.win.zip").ConfigureAwait(false);

                    pathToNodeModules = pathToNodeModules.Replace("\\", "/") + "/node_modules/";
                }
                else
                {
                    // TODO implement packaged node_modules for other os, use globaly installed canvas in the mean time
                    pathToNodeModules = string.Empty;
                    useCustomNodeModulePath = true;
                }

                IsInitialized = true;
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Disposing packaged node_moduels folder
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing packaged node_moduels folder
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing && !useCustomNodeModulePath && Directory.Exists(pathToNodeModules))
            {
                Directory.Delete(pathToNodeModules, true);
            }
            disposed = true;
        }

        private async Task ExtractBinaryFromManifest(string resourceName)
        {
            var pathNodeModules = Path.Combine(pathToNodeModules, "node_modules.zip");
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var fileStream = File.Create(pathNodeModules))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream).ConfigureAwait(false);
            }

            ZipFile.ExtractToDirectory(pathNodeModules, pathToNodeModules);
        }
    }
}