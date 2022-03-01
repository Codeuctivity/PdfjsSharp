using Jering.Javascript.NodeJS;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Codeuctivity.PdfjsSharp
{
    /// <summary>
    /// Reads installed node version
    /// </summary>
    public static class NodeVersionDetector
    {
        /// <summary>
        /// Reads installed node version
        /// </summary>
        public static async Task<Version?> DetectVersionAsync()
        {
            var result = await StaticNodeJSService.InvokeFromStringAsync<string>("module.exports = (callback) => callback(null, process.versions.node);").ConfigureAwait(false);

            var splitUpResult = result?.Split('.');

            if (int.TryParse(splitUpResult?[0], out var majorVersion) && int.TryParse(splitUpResult?[1], out var minorVersion) && int.TryParse(splitUpResult?[2], out var buildVersion))
            {
                return new Version(majorVersion, minorVersion, buildVersion);
            }
            throw new NotSupportedException($"Failed to parse 'process.versions.node' response {result}. Expected 'vX.X.X.X'.");
        }

        /// <summary>
        /// Reads installed node bitness version
        /// </summary>
        public static string DetectBittness()
        {
            using var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "node";
            process.StartInfo.Arguments = "-p \"process.arch\"";
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                return string.Empty;
            }

            return process.StandardOutput.ReadToEnd().Trim();
        }

        /// <summary>
        /// Check that node with majorNodeVersion is installed
        /// </summary>
        /// <param name="supportedMajorNodeVersions"></param>
        public static async Task<int> CheckRequiredNodeVersionInstalledAsync(int[] supportedMajorNodeVersions)
        {
            var foundMajorVersion = (await DetectVersionAsync())?.Major;

            if (foundMajorVersion == null)
            {
                throw new NotSupportedException($"No supported node version found. Expected node {supportedMajorNodeVersions} to be installed.");
            }

            if (DetectBittness() != "x64")
            {
                throw new NotSupportedException($"No supported node version found. Expected 64bit node to be installed.");
            }

            if (supportedMajorNodeVersions.Any(_ => _ == foundMajorVersion))
            {
                return foundMajorVersion.Value;
            }

            var expectedVersions = string.Join(", ", supportedMajorNodeVersions.Select(_ => _.ToString(CultureInfo.InvariantCulture)));

            throw new NotSupportedException($"Not supported node version {foundMajorVersion} found. Expected a supported node version {expectedVersions} to be installed.");
        }
    }
}