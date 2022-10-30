# PDFjsSharp

[![Nuget](https://img.shields.io/nuget/v/Codeuctivity.PdfjsSharp.svg)](https://www.nuget.org/packages/Codeuctivity.PdfjsSharp/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/c417a8e923da45ed90c302c4a23528ea)](https://www.codacy.com/gh/Codeuctivity/PdfjsSharp?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/PdfjsSharp&utm_campaign=Badge_Grade) [![Build](https://github.com/Codeuctivity/PdfjsSharp/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Codeuctivity/PdfjsSharp/actions/workflows/dotnet.yml) [![Donate](https://img.shields.io/static/v1?label=Paypal&message=Donate&color=informational)](https://www.paypal.com/donate?hosted_button_id=7M7UFMMRTS7UE)

Brings Pdfjs to .net

## Feature

- Renders each page of a pdf to pngs

## Dependencies

get

- NodeJs
  - node 18 or 16 x64 (older node versions are supported by older PDFjsSharp versions)

- .NET Framework 4.6.1 or .NET Core 2.0 or [something newer](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)

## Howto use

- Install nuget package [PdfjsSharp](https://www.nuget.org/packages/Codeuctivity.PdfjsSharp/)

```Csharp
using var rasterizer = new Rasterizer();
var actualImages = await rasterizer.ConvertToPngAsync(@"../../../SourceTest.pdf", actualImagePath);
Assert.Equal(1, actualImages.Count);
```

## Development

### Windows

Visual Studio 2022 or .net 6 SDK

### Upgarde npm packages

- temporary remove 'MagicPrefix' from Rasterize.js
- npx npm-check --update-all
- revert changes in Rasterize.js

#### Steps to update node_modules.win.\*.zip

```Powershell
 cd .\PdfjsSharp\
 nvm install lts;nvm use lts;rm -R .\node_modules\;npm install --omit=dev;rm .\node_modules.win.18.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.18.zip
 nvm install 16;nvm use 16;rm -R .\node_modules\;npm install --omit=dev;rm .\node_modules.win.16.zip;Compress-Archive -LiteralPath .\node_modules\ -DestinationPath .\node_modules.win.16.zip
```

### Ubuntu 20.04

```bash
#In case you have no nvm
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.1/install.sh | bash && exit
# In case of using wsl: you need to set environment variables

#In case you have .net sdk not setup
export DOTNET_CLI_TELEMETRY_OPTOUT=1
sudo snap remove dotnet-sdk
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install apt-transport-https zip
sudo apt update
sudo apt install dotnet-sdk-6.0 -y
echo export DOTNET_CLI_TELEMETRY_OPTOUT=1>> ~/.bash_profile
nvm install 16
```

### Ubuntu 22.04

```bash
sudo snap remove dotnet-sdk
sudo apt remove 'dotnet*'
sudo apt remove 'aspnetcore*'
sudo apt remove 'netstandard*'
sudo rm /etc/apt/sources.list.d/microsoft-prod.list
sudo rm /etc/apt/sources.list.d/microsoft-prod.list.save
sudo apt update
sudo apt install dotnet6
```

#### Steps to update node_modules.linux.\*.zip

```bash
nvm install 16
nvm install lts
nvm use lts
rm -R ./node_modules/ 
npm install --production && rm node_modules.linux.18.zip && zip -r node_modules.linux.18.zip node_modules
nvm use 16
rm -R ./node_modules/ 
npm install --production && rm node_modules.linux.16.zip && zip -r node_modules.linux.16.zip node_modules
```
