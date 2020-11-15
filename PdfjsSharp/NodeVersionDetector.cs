using System;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Reads installed node version
/// </summary>
public class NodeVersionDetector
{
    /// <summary>
    /// Reads installed node version
    /// </summary>
    public static Version? DetectVersion()
    {
        using var process = new Process();
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.FileName = "node";
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
    /// Check that node with majorNodeVersion is installed
    /// </summary>
    /// <param name="supportedMajorNodeVersions"></param>
    public static int CheckRequiredNodeVersionInstalled(int[] supportedMajorNodeVersions)
    {
        var foundMajorVersion = DetectVersion()?.Major;

        if (foundMajorVersion == null)
        {
            throw new NotSupportedException($"No supported node version found. Expected node {supportedMajorNodeVersions} to be installed.");
        }

        if (supportedMajorNodeVersions.Any(_ => _ == foundMajorVersion))
        {
            return foundMajorVersion.Value;
        }

        var expectedVersions = string.Join("", supportedMajorNodeVersions.SelectMany(_ => _.ToString()));

        throw new NotSupportedException($"Not supported node version {foundMajorVersion} found. Expected a supported node version {expectedVersions} to be installed.");
    }
}