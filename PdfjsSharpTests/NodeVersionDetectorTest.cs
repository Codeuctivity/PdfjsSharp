using Codeuctivity.PdfjsSharp;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Codeuctivity.PdfjsSharpTests
{
    public class NodeVersionDetectorTest
    {
        [Fact]
        public async Task ShouldDetectNodeVersionAsync()
        {
            var actualInstalledVersion = await NodeVersionDetector.DetectVersionAsync();
            Assert.True(actualInstalledVersion.Major > 6);
        }

        [Fact]
        public async Task ShouldSuccescullyDetermineInstalledVersionAsync()
        {
            var actualInstalledVersion = await NodeVersionDetector.CheckRequiredNodeVersionInstalledAsync(new[] { 8, 16, 14 });
            Assert.True(actualInstalledVersion > 6);
        }

        [Fact]
        public async Task ShouldThrowOnMissingNodeVersion()
        {
            var exception = await Assert.ThrowsAsync<NotSupportedException>(() => NodeVersionDetector.CheckRequiredNodeVersionInstalledAsync(new[] { 9999 }));
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