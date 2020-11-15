# PdfjsSharp

[![Nuget](https://img.shields.io/nuget/v/PdfjsSharp.svg)](https://www.nuget.org/packages/PdfjsSharp/) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/c417a8e923da45ed90c302c4a23528ea)](https://www.codacy.com/gh/Codeuctivity/PdfjsSharp?utm_source=github.com&utm_medium=referral&utm_content=Codeuctivity/PdfjsSharp&utm_campaign=Badge_Grade)[![Build Status](https://travis-ci.com/Codeuctivity/PdfjsSharp.svg?branch=travis)](https://travis-ci.com/Codeuctivity/PdfjsSharp)

Brings Pdfjs to .net

## Feature

- Renders each page of a given pdf to pngs

## Dependencies

get

- nodejs lts from <https://nodejs.org/en/download/>
- .NET Framework 4.6.1 or .NET Core 2.0 or [something newer](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)

## Howto use

- Install nuget package [PdfjsSharp](https://www.nuget.org/packages/PdfjsSharp/)

### Linux dependency

Tested with node v10.19.0, if you have problems try to install v10.19.0

### Windows dependency

Tested with node 8 and node 12.

```Csharp
using var rasterize = new Rasterize();
var pathsToImages = await rasterize.ConvertToPngAsync("./SourceTest.pdf", "./DestinationTest");
```

- "pathsToImages" will give a collection of paths to the rendered pages stored as png. E.g. DestinationTest1.png .

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
