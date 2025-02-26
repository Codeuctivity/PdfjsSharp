using Codeuctivity.PdfjsSharp;
using System.Threading.Tasks;
using Xunit;

namespace PdfJsSharpTests
{
    public class NodeVersionDetectorTestFixture : IAsyncLifetime
    {
        public Rasterizer Rasterizer { get; }

        public NodeVersionDetectorTestFixture()
        {
            Rasterizer = new Rasterizer();
        }

        public async Task InitializeAsync()
        {
            await Rasterizer.InitPdfJsWrapper();
        }

        public async Task DisposeAsync()
        {
            Rasterizer.Dispose();
        }
    }
}