# PdfjsSharp

[![Nuget](https://img.shields.io/nuget/v/PdfjsSharp.svg)](https://www.nuget.org/packages/PdfjsSharp/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/c417a8e923da45ed90c302c4a23528ea)](https://www.codacy.com/gh/Codeuctivity/PdfjsSharp?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/PdfjsSharp&utm_campaign=Badge_Grade) [![Build](https://github.com/Codeuctivity/PdfjsSharp/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/PdfjsSharp/actions/workflows/dotnet.yml) [![Donate](https://img.shields.io/static/v1?label=Paypal&message=Donate&color=informational)](https://www.paypal.com/donate?hosted_button_id=7M7UFMMRTS7UE)

Brings Pdfjs to .net

## Feature

- Renders each page of a pdf to pngs

## Dependencies

get

- NodeJs
  - node 12 or 14 x64
- .NET Framework 4.6.1 or .NET Core 2.0 or [something newer](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)

## Howto use

- Install nuget package [PdfjsSharp](https://www.nuget.org/packages/PdfjsSharp/)

```Csharp
using var rasterizer = new Rasterizer();
var actualImages = await rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);
Assert.Equal(1, actualImages.Count);
```

## Development

### Windows

Visual Studio 2022 or .net 6 SDK

#### Steps to update node_modules.win.\*.zip

```Powershell
cd .\PdfjsSharp\
nvm use 12;rm -R .\node_modules\; npm install --production;rm .\node_modules.win.node12.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.node12.zip
nvm use 14;rm -R .\node_modules\; npm install --production;rm .\node_modules.win.node14.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.node14.zip
```

### Ubuntu 20.04

```bash
export DOTNET_CLI_TELEMETRY_OPTOUT=1
sudo snap remove dotnet-sdk
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install apt-transport-https
sudo apt update
sudo apt install dotnet-sdk-6.0 -y
echo export DOTNET_CLI_TELEMETRY_OPTOUT=1>> ~/.bash_profile
nvm use 12
```

#### Steps to update node_modules.linux.\*.zip

```bash
nvm use 12;rm -R ./node_modules/ || npm install --production && rm node_modules.linux.node12.zip && zip -r node_modules.linux.node12.zip node_modules
nvm use 14;rm -R ./node_modules/ || npm install --production && rm node_modules.linux.node14.zip && zip -r node_modules.linux.node14.zip node_modules
```
