Sample - e.g. use it in your unit test to check SourceTest.pdf has one page that is equal to ExpectedShouldCreatePngFromPdf1.png:

```Csharp
var actualImagePath = Path.Combine(Path.GetTempPath(), "ActualShouldCreatePngFromPdf");
using var rasterizer = new Rasterizer();

var actualImages = await rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);

Assert.Equal(1, actualImages.Count);
Assert.True(File.Exists(actualImages.Single()), "Actual output file not found");
Assert.True(ImageSharpCompare.ImagesAreEqual(actualImages.Single(), @"../../../ExpectedImages/ExpectedShouldCreatePngFromPdf1.png"), "Actual and expected image do differ");
```
