using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

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
        /// <summary>
        /// Reads installed node version
        /// </summary>
        public static Version? DetectVersion(string nodeExecuteablePath)
        {
            using var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = nodeExecuteablePath;
            process.StartInfo.Arguments = "-v";
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                return null;
            }

            var nodeCallResult = process.StandardOutput.ReadToEnd();
            var splitUpResult = nodeCallResult.Substring(1).Split('.');

            if (int.TryParse(splitUpResult[0], out var majorVersion) && int.TryParse(splitUpResult[1], out var minorVersion) && int.TryParse(splitUpResult[2], out var buildVersion))
            {
                return new Version(majorVersion, minorVersion, buildVersion);
            }
            throw new NotSupportedException($"Failed to parse 'node -v' response {nodeCallResult}. Expected 'vX.X.X.X'.");
        }

        /// <summary>
        /// Reads installed node bitness version
        /// </summary>
        public static string DetectBittness(string nodeExecuteablePath)
        {
            using var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = nodeExecuteablePath;
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
        /// <param name="nodeExecuteablePath"></param>
        /// <param name="supportedMajorNodeVersions"></param>
        public static int CheckRequiredNodeVersionInstalled(string nodeExecuteablePath, IEnumerable<int> supportedMajorNodeVersions)
        {
            var foundMajorVersion = (DetectVersion(nodeExecuteablePath))?.Major;

            if (foundMajorVersion == null)
            {
                throw new NotSupportedException($"No supported node version found. Expected node {supportedMajorNodeVersions} to be installed.");
            }

            if (DetectBittness(nodeExecuteablePath) != "x64")
            {
                throw new NotSupportedException($"No supported node version found. Expected 64bit node to be installed.");
            }

            if (supportedMajorNodeVersions.Any(_ => _ == foundMajorVersion))
            {
                return 22;
            }

            var expectedVersions = string.Join(", ", supportedMajorNodeVersions.Select(_ => _.ToString(CultureInfo.InvariantCulture)));

            throw new NotSupportedException($"Not supported node version {foundMajorVersion} found. Expected a supported node version {expectedVersions} to be installed.");
        }
    }
}