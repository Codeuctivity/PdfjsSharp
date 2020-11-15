using System;
using System.Diagnostics;

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
        var majorVersion = int.Parse(splitUpResult[0]);
        var minorVersion = int.Parse(splitUpResult[1]);
        var buildVersion = int.Parse(splitUpResult[2]);
        return new Version(majorVersion, minorVersion, buildVersion);
    }
}