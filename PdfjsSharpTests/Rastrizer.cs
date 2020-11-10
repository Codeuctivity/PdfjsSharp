using Codeuctivity;
using System.IO;
using System.Linq;
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
            var actualImages = await new Rasterize().ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);

            Assert.Equal(1, actualImages.Count);
            Assert.True(ImageSharpCompare.ImageAreEqual(actualImages.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"));
        }
    }
}