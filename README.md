# PdfjsSharp

[![Nuget](https://img.shields.io/nuget/v/PdfjsSharp.svg)](https://www.nuget.org/packages/PdfjsSharp/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/c417a8e923da45ed90c302c4a23528ea)](https://www.codacy.com/gh/Codeuctivity/PdfjsSharp?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/PdfjsSharp&utm_campaign=Badge_Grade) [![Build status](https://ci.appveyor.com/api/projects/status/f5f4mvh98eqkjanp/branch/master?svg=true)](https://ci.appveyor.com/project/stesee/pdfjssharp/branch/master)
 [![Donate](https://img.shields.io/static/v1?label=Paypal&message=Donate&color=informational)](https://www.paypal.com/donate?hosted_button_id=7M7UFMMRTS7UE)

Brings Pdfjs to .net

## Feature

- Renders each page of a given pdf to pngs

## Dependencies

get

- nodejs from <https://nodejs.org/en/download/>
  - Windows - node 12 x64 or node 8 x64
  - Linux - node 10 x64
- .NET Framework 4.6.1 or .NET Core 2.0 or [something newer](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)

## Howto use

- Install nuget package [PdfjsSharp](https://www.nuget.org/packages/PdfjsSharp/)

```Csharp
using var rasterizer = new Rasterizer();
var actualImages = await rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);
Assert.Equal(1, actualImages.Count);
```

### Linux dependency

Tested with node v10.19.0, if you have problems try to install v10.19.0 . Thats the current version used in the apt package on Ubuntu 20.04.

### Windows dependency

Tested with node 12. Node 8 should also work.

## Development

### Windows

Visual Studio 2019 (16.8+) or .net 5 SDK

#### Steps to upate node_modules.win.\*.zip

```Powershell
nvm use 8;rm -R .\node_modules\; npm install --production;rm .\node_modules.win.node8.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.node8.zip
nvm use 12;rm -R .\node_modules\; npm install --production;rm .\node_modules.win.node12.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.node12.zip
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
sudo apt install dotnet-sdk-5.0 nodejs npm -y
echo export DOTNET_CLI_TELEMETRY_OPTOUT=1>> ~/.bash_profile
```

#### Steps to upate node_modules.linux.\*.zip

```Powershell
rm -R .\node_modules\ || npm install --production && rm node_modules.linux.zip && zip -r node_modules.linux.zip node_modules
```
