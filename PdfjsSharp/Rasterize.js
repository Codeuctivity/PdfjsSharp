//based on https://github.com/mozilla/pdf.js/tree/master/examples/node/pdf2png

const Canvas = require("canvas");
const fs = require("fs");
const pdfjsLib = require('pdfjs-dist/es5/build/pdf.js');

function NodeCanvasFactory() { }
NodeCanvasFactory.prototype = {
    create: function NodeCanvasFactory_create(width, height) {
        assert(width > 0 && height > 0, "Invalid canvas size");
        var canvas = Canvas.createCanvas(width, height);
        var context = canvas.getContext("2d");
        return {
            canvas: canvas,
            context: context,
        };
    },

    reset: function NodeCanvasFactory_reset(canvasAndContext, width, height) {
        assert(canvasAndContext.canvas, "Canvas is not specified");
        assert(width > 0 && height > 0, "Invalid canvas size");
        canvasAndContext.canvas.width = width;
        canvasAndContext.canvas.height = height;
    },

    destroy: function NodeCanvasFactory_destroy(canvasAndContext) {
        assert(canvasAndContext.canvas, "Canvas is not specified");

        // Zeroing the width and height cause Firefox to release graphics
        // resources immediately, which can greatly reduce memory consumption.
        canvasAndContext.canvas.width = 0;
        canvasAndContext.canvas.height = 0;
        canvasAndContext.canvas = null;
        canvasAndContext.context = null;
    },
};

module.exports = async (sourceFile, targetPrefix) => {
    // Loading file from file system into typed array.
    const data = new Uint8Array(fs.readFileSync(sourceFile));

    // Some PDFs need external cmaps.
    const CMAP_URL = "./node_modules/pdfjs-dist/cmaps/";
    const CMAP_PACKED = true;

    // Load the PDF file.
    const pdfDocument = await pdfjsLib.getDocument({ data: data, cMapUrl: CMAP_URL, cMapPacked: CMAP_PACKED }).promise;

    const canvasFactory = new NodeCanvasFactory();
    for (let pageNumber = 1; pageNumber <= pdfDocument.numPages; pageNumber++) {
        const page = await pdfDocument.getPage(pageNumber);

        // Render the page on a Node canvas with 100% scale.
        const viewport = page.getViewport({ scale: 1.0 });
        const canvasAndContext = canvasFactory.create(viewport.width, viewport.height);
        const renderContext = { canvasContext: canvasAndContext.context, viewport: viewport, canvasFactory: canvasFactory };
        await page.render(renderContext).promise;
        const image = canvasAndContext.canvas.toBuffer();

        const targetFile = `${targetPrefix}${pageNumber}.png`;

        fs.writeFileSync(`${targetFile}`, image);
    }

    return pdfDocument.numPages;
}