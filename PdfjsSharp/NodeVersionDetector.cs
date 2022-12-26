using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Codeuctivity.PdfjsSharp
{
    /// <summary>
    /// Reads installed node version
    /// </summary>
    public static class NodeVersionDetector
    {
        public static string? NodePath { get; }
        public static readonly ImmutableArray<int> SupportedNodeVersions = ImmutableArray.Create(new[] { 18, 16 });

        static NodeVersionDetector()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (!string.IsNullOrEmpty(NodePath))
                {
                    return;
                }

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
                        NodePath = detectedPath;
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
                                NodePath = nodePath;
                                return;
                            }
                        }
                    }
                }
            }
            NodePath = "node";
        }

        /// <summary>
        /// Reads installed node version
        /// </summary>
        /// <summary>
        /// Reads installed node version
        /// </summary>
        public static Version? DetectVersion()
        {
            using var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = NodePath;
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
        public static string DetectBittness()
        {
            using var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = NodePath;
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
        public static int CheckRequiredNodeVersionInstalled(IEnumerable<int> supportedMajorNodeVersions)
        {
            var foundMajorVersion = (DetectVersion())?.Major;

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