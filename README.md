# PDFjsSharp

[![Nuget](https://img.shields.io/nuget/v/Codeuctivity.PdfjsSharp.svg)](https://www.nuget.org/packages/Codeuctivity.PdfjsSharp/) [![Build](https://github.com/Codeuctivity/PdfjsSharp/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/PdfjsSharp/actions/workflows/dotnet.yml) [![Donate](https://img.shields.io/static/v1?label=Paypal&message=Donate&color=informational)](https://www.paypal.com/donate?hosted_button_id=7M7UFMMRTS7UE)

Brings [PDF.js](https://github.com/mozilla/pdf.js) to .net

## Feature

- Renders each page of a PDF to PNGs

## Dependencies

get

- NodeJs
  - node 18 or 20 x64 (older node versions are supported by older PDFjsSharp versions)

## HowTo use

- Install nuget package [PdfjsSharp](https://www.nuget.org/packages/Codeuctivity.PdfjsSharp/)

```Csharp
using var rasterizer = new Rasterizer();
var actualImages = await rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);
Assert.Equal(1, actualImages.Count);
```

## Development

get .net 8 sdk

### Upgrade npm packages

- npx npm-check --update-all --skip-unused

#### Steps to update node_modules.win.\*.zip

```Powershell
 cd .\PdfjsSharp\
 nvm install 18;nvm use 18;rm -R .\node_modules\;npm install --omit=dev;rm .\node_modules.win.node18.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.node18.zip;Compress-Archive -Update .\Rasterize.mjs .\node_modules.win.node18.zip
 nvm install lts;nvm use lts;rm -R .\node_modules\;npm install --omit=dev;rm .\node_modules.win.node20.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.node20.zip;Compress-Archive -Update .\Rasterize.mjs .\node_modules.win.node20.zip
```

### Ubuntu 22.04

```bash
#In case you have no nvm
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/master/install.sh | bash 
exit
nvm install 20
sudo apt install dotnet8 zip
```

#### Steps to update node_modules.linux.node\*.zip

```bash
nvm install 18
nvm use 18
rm -R ./node_modules/ 
npm install --omit=dev && rm node_modules.linux.node18.zip && zip -r node_modules.linux.node18.zip node_modules && zip -g node_modules.linux.node18.zip Rasterize.mjs
nvm install 20
nvm use 20
rm -R ./node_modules/ 
npm install --omit=dev && rm node_modules.linux.node20.zip && zip -r node_modules.linux.node20.zip node_modules && zip -g node_modules.linux.node20.zip Rasterize.mjs
```
