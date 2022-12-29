﻿using Codeuctivity.PdfjsSharp;
using System.Threading.Tasks;
using Xunit;

namespace PdfjsSharpTests
{
    public class NodeVersionDetectorTestFixture : IAsyncLifetime
    {
        public Rasterizer Rasterizer { get; }

        public NodeVersionDetectorTestFixture()
        {
            Rasterizer = new Rasterizer();
        }

        public Task InitializeAsync()
        {
            return Rasterizer.InitPdfJsWrapper();
        }

        public Task DisposeAsync()
        {
            Rasterizer.Dispose();
            return Task.CompletedTask;
        }
    }
}