using Codeuctivity.PdfjsSharp;
using System;
using Xunit;

namespace PdfJsSharpTests
{
    public class NodeVersionDetectorTest : NodeVersionDetectorTestFixture
    {
        [Fact]
        public void ShouldDetectNodeVersionAsync()
        {
            var actualInstalledVersion = NodeVersionDetector.DetectVersion(Rasterizer.NodeExecutablePath);
            Assert.True(actualInstalledVersion.Major > 6);
        }

        [Fact]
        public void ShouldDetermineInstalledVersionAsync()
        {
            var actualInstalledVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(Rasterizer.NodeExecutablePath, [8, 20, 22]);
            Assert.True(actualInstalledVersion > 6);
        }

        [Fact]
        public void ShouldThrowOnMissingNodeVersion()
        {
            var exception = Assert.Throws<NotSupportedException>(() => NodeVersionDetector.CheckRequiredNodeVersionInstalled(Rasterizer.NodeExecutablePath, [9999]));
            Assert.Contains(" Expected a supported node version 9999 to be installed.", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void ShouldDetectNodeBittnes()
        {
            var detectedBittnes = NodeVersionDetector.DetectBittness(Rasterizer.NodeExecutablePath);
            Assert.Equal("x64", detectedBittnes);
        }
    }
}