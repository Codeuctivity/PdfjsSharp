using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Codeuctivity.PdfjsSharpTests
{
    public class RasterizerTests
    {
        [Fact]
        public async Task ShouldCreatePngFromPdf()
        {
            var actualImagePath = Path.Combine(Path.GetTempPath(), "ActualShouldCreatePngFromPdf");
            using var rasterizer = new Rasterizer();
            var actualImages = await rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);

            Assert.Equal(1, actualImages.Count);
            Assert.True(ImageSharpCompare.ImageAreEqual(actualImages.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"));
            File.Delete(actualImages.Single());
        }
    }
}