using Codeuctivity.ImageSharpCompare;
using Codeuctivity.PdfjsSharp;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PdfJsSharpTests
{
    public class RasterizerTests : NodeVersionDetectorTestFixture
    {
        [Fact]
        public async Task ShouldCreatePngFromPdf()
        {
            var actualImagePath = Path.Combine(Path.GetTempPath(), "ActualShouldCreatePngFromPdf");
            var actualImages = await Rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);

            Assert.Equal(1, actualImages.Count);
            Assert.True(File.Exists(actualImages.Single()), "Actual output file not found");
            Assert.True(ImageSharpCompare.ImagesAreEqual(actualImages.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"), "Actual and expected image do differ");
            File.Delete(actualImages.Single());
        }

        [Fact]
        public async Task ShouldCreatePngFromPdfCustomNodeExecutablePath()
        {
            var actualImagePath = Path.Combine(Path.GetTempPath(), "ActualShouldCreatePngFromPdfCustomNodeExecutablePath");
            using var rastirizerSut = new Rasterizer(Rasterizer.NodeExecutablePath);

            var actualImages = await rastirizerSut.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);

            Assert.Equal(1, actualImages.Count);
            Assert.True(File.Exists(actualImages.Single()), "Actual output file not found");
            Assert.True(ImageSharpCompare.ImagesAreEqual(actualImages.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"), "Actual and expected image do differ");
            File.Delete(actualImages.Single());
        }
    }
}