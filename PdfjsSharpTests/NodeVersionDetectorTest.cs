using Codeuctivity.PdfjsSharp;
using System;
using Xunit;

namespace PdfjsSharpTests
{
    public class NodeVersionDetectorTest
    {
        [Fact]
        public void ShouldDetectNodeVersionAsync()
        {
            var actualInstalledVersion = NodeVersionDetector.DetectVersion();
            Assert.True(actualInstalledVersion.Major > 6);
        }

        [Fact]
        public void ShouldSuccescullyDetermineInstalledVersionAsync()
        {
            var actualInstalledVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(new[] { 8, 16, 14 });
            Assert.True(actualInstalledVersion > 6);
        }

        [Fact]
        public void ShouldThrowOnMissingNodeVersion()
        {
            var exception = Assert.Throws<NotSupportedException>(() => NodeVersionDetector.CheckRequiredNodeVersionInstalled(new[] { 9999 }));
            Assert.Contains(" Expected a supported node version 9999 to be installed.", exception.Message);
        }

        [Fact]
        public void ShouldDetectNodeBittnes()
        {
            var detectedBittnes = NodeVersionDetector.DetectBittness();
            Assert.Equal("x64", detectedBittnes);
        }
    }
}