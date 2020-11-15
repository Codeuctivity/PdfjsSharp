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
    }
}