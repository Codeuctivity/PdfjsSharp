Sample:

```Csharp
var outputPath = Path.Combine(Path.GetTempPath(), "OutputPathPng");
var images = await Rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", outputPath);

Console.WriteLine($"Pages converted: {images.Count}");
Console.WriteLine($"First page path: {images.Single()}");
//Pages converted: 1
//First page path: /tmp/OutputPathPng1.png
```
