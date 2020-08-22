using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PdfjsSharpTests
{
    public class Rastrizer
    {
        [Fact]
        public async Task ShouldCreatePngFromPdf()
        {
            var actualImagePath = Path.Combine(Path.GetTempPath(), "ActualShouldCreatePngFromPdf");
            var pageQuantity = await PdfjsSharp.Rasterize.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);

            Assert.Equal(1, pageQuantity);
            Assert.True(Codeuctivity.ImageSharpCompare.ImageAreEqual($"{actualImagePath}1.png", @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"));
        }
    }
}