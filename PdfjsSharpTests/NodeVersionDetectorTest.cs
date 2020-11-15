using System;
using Codeuctivity.PdfjsSharp;
using Xunit;

namespace Codeuctivity.PdfjsSharpTests
{
    public class NodeVersionDetectorTest
    {
        [Fact]
        public void ShouldDetectNodeVersion()
        {
            var actualInstalledVersion = NodeVersionDetector.DetectVersion();
            Assert.True(actualInstalledVersion.Major > 6);
        }

        [Fact]
        public void ShouldSuccescullyDetermineInstalledVersion()
        {
            var actualInstalledVersion = NodeVersionDetector.CheckRequiredNodeVersionInstalled(new[] { 8, 10, 12 });
            Assert.True(actualInstalledVersion > 6);
        }

        [Fact]
        public void ShouldThrowOnMissingNodeVersion()
        {
            var exception = Assert.Throws<NotSupportedException>(() => NodeVersionDetector.CheckRequiredNodeVersionInstalled(new[] { 9999 }));
            Assert.Contains(" Expected a supported node version 9999 to be installed.", exception.Message);
        }
    }
}