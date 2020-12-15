using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Codeuctivity.PdfjsSharp
{
    /// <summary>
    /// Wraps pdfjs boilerplaiting
    /// </summary>
    public class PdfJsWrapper : IDisposable
    {
        internal const int someMaxPathLength = 206;
        internal static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        internal bool useCustomNodeModulePath;
        internal string pathToNodeModules = default!;
        private bool disposed;

        internal bool IsInitialized { get; set; }

        /// <summary>
        /// Disposing packaged node_modules folder
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing packaged node_modules folder
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing && !useCustomNodeModulePath && Directory.Exists(pathToNodeModules))
            {
                try
                {
                    Directory.Delete(pathToNodeModules, true);
                }
                catch (UnauthorizedAccessException)
                {
                    //TODO do something smarter than ignoring
                }
            }
            disposed = true;
        }

        internal async Task InitNodeModules()
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (IsInitialized)
                {
                    return;
                }

                pathToNodeModules = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (pathToNodeModules.Length > someMaxPathLength)
                    {
                        throw new PathTooLongException(pathToNodeModules);
                    }
                    var foundVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(new[] { 8, 12 });

                    Directory.CreateDirectory(pathToNodeModules);

                    await ExtractBinaryFromManifest($"Codeuctivity.PdfjsSharp.node_modules.win.node{foundVersion}.zip").ConfigureAwait(false);

                    pathToNodeModules = pathToNodeModules.Replace("\\", "/") + "/node_modules/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    NodeVersionDetector.CheckRequiredNodeVersionInstalled(new[] { 10 });
                    Directory.CreateDirectory(pathToNodeModules);
                    await ExtractBinaryFromManifest("Codeuctivity.PdfjsSharp.node_modules.linux.zip").ConfigureAwait(false);

                    pathToNodeModules = pathToNodeModules + "/node_modules/";
                }
                else
                {
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