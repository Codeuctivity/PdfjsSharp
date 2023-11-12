// based on https://github.com/mozilla/pdf.js/tree/master/examples/node/pdf2png
/*global Uint8Array*/

import { strict as assert } from "assert";
import Canvas from "canvas";
import fs from "fs";
import { getDocument } from "pdfjs-dist/legacy/build/pdf.mjs";

class NodeCanvasFactory {
  create(width, height) {
    assert(width > 0 && height > 0, "Invalid canvas size");
    const canvas = Canvas.createCanvas(width, height);
    const context = canvas.getContext("2d");
    return {
      canvas,
      context,
    };
  }

  reset(canvasAndContext, width, height) {
    assert(canvasAndContext.canvas, "Canvas is not specified");
    assert(width > 0 && height > 0, "Invalid canvas size");
    canvasAndContext.canvas.width = width;
    canvasAndContext.canvas.height = height;
  }

  destroy(canvasAndContext) {
    assert(canvasAndContext.canvas, "Canvas is not specified");

    // Zeroing the width and height cause Firefox to release graphics
    // resources immediately, which can greatly reduce memory consumption.
    canvasAndContext.canvas.width = 0;
    canvasAndContext.canvas.height = 0;
    canvasAndContext.canvas = null;
    canvasAndContext.context = null;
  }
}

export async function convertToPng(sourceFile, targetPrefix) {
  // Loading file from file system into typed array.
  const data = new Uint8Array(fs.readFileSync(sourceFile));

  // Some PDFs need external cmaps.
  const CMAP_URL = "../../../node_modules/pdfjs-dist/cmaps/";
  const CMAP_PACKED = true;

  // Where the standard fonts are located.
  const STANDARD_FONT_DATA_URL =
    "../../../node_modules/pdfjs-dist/standard_fonts/";

  const canvasFactory = new NodeCanvasFactory();

  // Loading file from file system into typed array.
  const pdfData = new Uint8Array(fs.readFileSync(sourceFile));

  // Load the PDF file.
  const loadingTask = getDocument({
    data: pdfData,
    cMapUrl: CMAP_URL,
    cMapPacked: CMAP_PACKED,
  });
  const pdfDocument = await loadingTask.promise;

  for (let pageNumber = 1; pageNumber <= pdfDocument.numPages; pageNumber++) {
    await processPage(pageNumber);
  }
  return pdfDocument.numPages;

  async function processPage(pageNumber) {
      console.log("# Processing page:", pageNumber);
      // Get the page.
      const page = await pdfDocument.getPage(pageNumber);
      // Render the page on a Node canvas with 100% scale.
      const viewport = page.getViewport({ scale: 1.0 });
      const canvasAndContext = canvasFactory.create(
        viewport.width,
        viewport.height
      );
      const renderContext = {
        canvasContext: canvasAndContext.context,
        viewport,
      };

      const renderTask = page.render(renderContext);
      await renderTask.promise;
      // Convert the canvas to an image buffer.
      const image = canvasAndContext.canvas.toBuffer();
      const targetFile = `${targetPrefix}${pageNumber}.png`;

      fs.writeFileSync(targetFile, image);
      console.log("Finished converting page", pageNumber, "to", targetFile);
      // Release page resources.
      page.cleanup();
  }
}

