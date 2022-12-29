using Codeuctivity.PdfjsSharp;
using System;
using Xunit;

namespace PdfjsSharpTests
{
    public class NodeVersionDetectorTest : NodeVersionDetectorTestFixture
    {
        [Fact]
        public void ShouldDetectNodeVersionAsync()
        {
            var actualInstalledVersion = NodeVersionDetector.DetectVersion(Rasterizer.NodeExecuteablePath);
            Assert.True(actualInstalledVersion.Major > 6);
        }

        [Fact]
        public void ShouldDetermineInstalledVersionAsync()
        {
            var actualInstalledVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(Rasterizer.NodeExecuteablePath, new[] { 8, 16, 18 });
            Assert.True(actualInstalledVersion > 6);
        }

        [Fact]
        public void ShouldThrowOnMissingNodeVersion()
        {
            var exception = Assert.Throws<NotSupportedException>(() => NodeVersionDetector.CheckRequiredNodeVersionInstalled(Rasterizer.NodeExecuteablePath, new[] { 9999 }));
            Assert.Contains(" Expected a supported node version 9999 to be installed.", exception.Message);
        }

        [Fact]
        public void ShouldDetectNodeBittnes()
        {
            var detectedBittnes = NodeVersionDetector.DetectBittness(Rasterizer.NodeExecuteablePath);
            Assert.Equal("x64", detectedBittnes);
        }
    }
}