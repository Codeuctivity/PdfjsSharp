using Codeuctivity.PdfjsSharp;
using Codeuctivity.SkiaSharpCompare;
using Jering.Javascript.NodeJS;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PdfJsSharpTests
{
    public class RasterizerTests(ITestOutputHelper output) : NodeVersionDetectorTestFixture
    {
        private readonly ITestOutputHelper Console = output;

        [Fact]
        public async Task ShouldCreatePngFromPdf()
        {
            var outputPath = Path.Combine(Path.GetTempPath(), "OutputPathPng");
            var images = await Rasterizer.ConvertToPngAsync(Path.GetFullPath(@"../../../SourceTest.pdf"), outputPath);

            Console.WriteLine($"Pages converted: {images.Count}");
            Console.WriteLine($"First page path: {images.Single()}");
            //Pages converted: 1
            //First page path: /tmp/OutputPathPng1.png

            Assert.Single(images);
            Assert.True(File.Exists(images.Single()), "Actual output file not found");
            // File.Copy(images.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png", true);
            Assert.True(Compare.ImagesAreEqual(images.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"), "Actual and expected image do differ");
            File.Delete(images.Single());
        }

        [Fact]
        public async Task ShouldCreatePngFromPdfCustomNodeExecutablePath()
        {
            var outputPath = Path.Combine(Path.GetTempPath(), "OutputPathPng");
            StaticNodeJSService.DisposeServiceProvider();
            using var rastirizerSut = new Rasterizer(Rasterizer.NodeExecutablePath);
            Assert.NotEmpty(Rasterizer.NodeExecutablePath);

            var actualImages = await rastirizerSut.ConvertToPngAsync(Path.GetFullPath(@"../../../SourceTest.pdf"), outputPath);

            Assert.Single(actualImages);
            Assert.True(File.Exists(actualImages.Single()), "Actual output file not found");
            Assert.True(Compare.ImagesAreEqual(actualImages.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"), "Actual and expected image do differ");
            File.Delete(actualImages.Single());
        }
    }
}