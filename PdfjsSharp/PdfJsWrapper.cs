using Jering.Javascript.NodeJS;
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
    /// Wraps PDFjs boiler plaiting
    /// </summary>
    public class PdfJsWrapper : IDisposable
    {
        internal const int someMaxPathLength = 206;
        internal static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        internal bool useCustomNodeModulePath;
        internal string pathToNodeModules = default!;
        internal string pathToTempFolder = default!;
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

            if (disposing && !useCustomNodeModulePath && Directory.Exists(pathToTempFolder))
            {
                try
                {
                    Directory.Delete(pathToTempFolder, true);
                }
                catch (UnauthorizedAccessException)
                {
                    // Deleting of temp files in case of missing permissions will be ignored
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

                pathToTempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (pathToTempFolder.Length > someMaxPathLength)
                    {
                        throw new PathTooLongException(pathToTempFolder);
                    }
                    var foundVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(NodeVersionDetector.SupportedNodeVersions);

                    Directory.CreateDirectory(pathToTempFolder);

                    await ExtractBinaryFromManifest($"Codeuctivity.PdfjsSharp.node_modules.win.node{foundVersion}.zip").ConfigureAwait(false);

                    pathToNodeModules = pathToTempFolder.Replace("\\", "/") + "/node_modules/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var foundVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(NodeVersionDetector.SupportedNodeVersions);
                    Directory.CreateDirectory(pathToTempFolder);
                    await ExtractBinaryFromManifest($"Codeuctivity.PdfjsSharp.node_modules.linux.node{foundVersion}.zip").ConfigureAwait(false);

                    pathToNodeModules = pathToTempFolder + "/node_modules/";

                    var executeablePath = NodeVersionDetector.NodePath;
                    if (executeablePath != "node")
                    {
                        StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ExecutablePath = executeablePath);
                    }
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
            var pathNodeModules = Path.Combine(pathToTempFolder, "node_modules.zip");
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var fileStream = File.Create(pathNodeModules))
            {
                stream!.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream).ConfigureAwait(false);
            }

            ZipFile.ExtractToDirectory(pathNodeModules, pathToTempFolder);
        }
    }
}