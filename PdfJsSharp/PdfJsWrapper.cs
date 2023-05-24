using Jering.Javascript.NodeJS;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
        internal const int windowsMaxPathLength = 206;
        internal static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        internal bool useCustomNodeModulePath;
        internal string pathToNodeModules = default!;
        internal string pathToTempFolder = default!;
        private bool disposed;

        /// <summary>
        /// Supported node versions
        /// </summary>
        private readonly ImmutableArray<int> SupportedNodeVersions = ImmutableArray.Create(18, 20);

        internal bool IsInitialized { get; set; }

        /// <summary>
        /// Path to node executable
        /// </summary>
        public string NodeExecutablePath { get; private set; }

        /// <summary>
        /// Ctor used with custom NodeExecutablePath
        /// </summary>
        /// <param name="nodeExecutablePath"></param>
        public PdfJsWrapper(string nodeExecutablePath)
        {
            NodeExecutablePath = nodeExecutablePath;
        }

        /// <summary>
        /// Default ctor, NodeExecutablePath gets auto detected
        /// </summary>
        public PdfJsWrapper()
        {
            NodeExecutablePath = "node";
        }

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

        /// <summary>
        /// Needs to be called before any method can be used
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PathTooLongException"></exception>
        public async Task InitPdfJsWrapper()
        {
            if (IsInitialized)
            {
                return;
            }

            await semaphore.WaitAsync().ConfigureAwait(false);

            if (IsInitialized)
            {
                return;
            }

            try
            {
                InitializeNodeExecutablePath();

                pathToTempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (pathToTempFolder.Length > windowsMaxPathLength)
                    {
                        throw new PathTooLongException(pathToTempFolder);
                    }
                    var foundVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(NodeExecutablePath, SupportedNodeVersions);

                    Directory.CreateDirectory(pathToTempFolder);

                    await ExtractBinaryFromManifest($"Codeuctivity.PdfjsSharp.node_modules.win.node{foundVersion}.zip").ConfigureAwait(false);

                    pathToNodeModules = pathToTempFolder.Replace("\\", "/") + "/node_modules/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var foundVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(NodeExecutablePath, SupportedNodeVersions);
                    Directory.CreateDirectory(pathToTempFolder);
                    await ExtractBinaryFromManifest($"Codeuctivity.PdfjsSharp.node_modules.linux.node{foundVersion}.zip").ConfigureAwait(false);

                    pathToNodeModules = pathToTempFolder + "/node_modules/";
                }
                else
                {
                    pathToNodeModules = string.Empty;
                    useCustomNodeModulePath = true;
                }

                if (!string.IsNullOrEmpty(NodeExecutablePath) && NodeExecutablePath != "node")
                {
                    StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ExecutablePath = NodeExecutablePath);
                }

                IsInitialized = true;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void InitializeNodeExecutablePath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var filePathWhich = "/usr/bin/which";

                if (File.Exists(filePathWhich))
                {
                    using var process = new Process();
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = filePathWhich;
                    process.StartInfo.Arguments = "node";
                    process.Start();
                    process.WaitForExit();

                    var detectedPath = process.StandardOutput.ReadToEnd();
                    if (!string.IsNullOrEmpty(detectedPath) && File.Exists(detectedPath))
                    {
                        NodeExecutablePath = detectedPath;
                        return;
                    }

                    // not finding node is a WSL only issue
                    // known location of node installed by nvm in wsl - https://learn.microsoft.com/en-us/windows/dev-environment/javascript/nodejs-on-wsl

                    var home = Environment.GetEnvironmentVariable("HOME");

                    var path = Path.Combine(home, ".nvm", "versions", "node");
                    if (!string.IsNullOrEmpty(home) && Directory.Exists(path))
                    {
                        var installedNodeVersions = Directory.GetDirectories(path);

                        var nodeExecutableDirectory = installedNodeVersions.FirstOrDefault(directory => SupportedNodeVersions.Any(version => Path.GetFileName(directory).StartsWith("v" + version.ToString())));

                        if (nodeExecutableDirectory != null)
                        {
                            var nodePath = Path.Combine(path, nodeExecutableDirectory, "bin", "node");
                            if (File.Exists(nodePath))
                            {
                                NodeExecutablePath = nodePath;
                                return;
                            }
                        }
                    }
                }
            }
            NodeExecutablePath = "node";
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