# PdfjsSharp

[![Build status](https://ci.appveyor.com/api/projects/status/f5f4mvh98eqkjanp/branch/master?svg=true)](https://ci.appveyor.com/project/stesee/pdfjssharp/branch/master) [![Nuget](https://img.shields.io/nuget/v/PdfjsSharp.svg)](https://www.nuget.org/packages/PdfjsSharp/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/c417a8e923da45ed90c302c4a23528ea)](https://www.codacy.com/gh/Codeuctivity/PdfjsSharp?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/PdfjsSharp&utm_campaign=Badge_Grade)

Brings Pdfjs to .net

## Dependencies

get nodejs lts from <https://nodejs.org/en/download/>
.net core 3.1 runtime

## Howto use

- Install nuget package
- Use it

```Csharp
var actualImages = await PdfjsSharp.Rasterize.ConvertToPngAsync("pathToSome.pdf", "somePathToAOutputLocation");
```

- "actualImages" will give a collection of paths to the renderd pages as png

## Development

### Windows

Visual Studio 2019 or .net Core Sdk 3.1

### Ubuntu 20.04

```bash
sudo snap remove dotnet-sdk
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install apt-transport-https
sudo apt update
sudo apt install dotnet-sdk-3.1
sudo apt install nodejs npm -y
```
