using Codeuctivity.ImageSharpCompare;
using Codeuctivity.PdfjsSharp;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PdfJsSharpTests
{
    public class RasterizerTests : NodeVersionDetectorTestFixture
    {
        private readonly ITestOutputHelper Console;

        public RasterizerTests(ITestOutputHelper output)
        {
            Console = output;
        }

        [Fact]
        public async Task ShouldCreatePngFromPdf()
        {
            var outputPath = Path.Combine(Path.GetTempPath(), "OutputPathPng");
            var images = await Rasterizer.ConvertToPngAsync(Path.GetFullPath(@"../../../SourceTest.pdf"), outputPath);

            Console.WriteLine($"Pages converted: {images.Count}");
            Console.WriteLine($"First page path: {images.Single()}");
            //Pages converted: 1
            //First page path: /tmp/OutputPathPng1.png

            Assert.Single( images);
            Assert.True(File.Exists(images.Single()), "Actual output file not found");
            Assert.True(ImageSharpCompare.ImagesAreEqual(images.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"), "Actual and expected image do differ");
            File.Delete(images.Single());
        }

        [Fact]
        public async Task ShouldCreatePngFromPdfCustomNodeExecutablePath()
        {
            var actualImagePath = Path.Combine(Path.GetTempPath(), "ActualShouldCreatePngFromPdfCustomNodeExecutablePath");
            using var rastirizerSut = new Rasterizer(Rasterizer.NodeExecutablePath);

            var actualImages = await rastirizerSut.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);

            Assert.Single(actualImages);
            Assert.True(File.Exists(actualImages.Single()), "Actual output file not found");
            Assert.True(ImageSharpCompare.ImagesAreEqual(actualImages.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"), "Actual and expected image do differ");
            File.Delete(actualImages.Single());
        }
    }
}