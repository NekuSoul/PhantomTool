# PhantomTool

Phantom Sealed generator for MTGA.

## Features

* Generate a list of cards for to use for a phantom Sealed match from your collection of cards.
* Use either the default rules for sealed or customize them.
* Built-in basic deck editor and export button for easy importing into MTGA.
* Automatically reads all available cards from the game. No update needed after a new set releases.

## Screenshots

![MainWindow](https://share.nekusoul.de/git/phantomtool/main.png)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FNekuSoul%2FPhantomTool.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FNekuSoul%2FPhantomTool?ref=badge_shield)

## Usage

### Requirements

* [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework-runtime)
* [Magic: The Gathering Arena](https://magic.wizards.com/en/mtgarena)

### How to use

* Grab the latest release from the releases section
* Extract the archive to a folder
* Launch `PhantomTool.exe`

### Known issues

* No collected cards are recognized.
  * Launch MTGA and exit after the main menu appears. Then select `File > Refresh collection`

## Build

* This tool makes use of AssetStudio. Get the latest release [from here](https://github.com/Perfare/AssetStudio) and copy the following libraries into the `/Dependencies` folder:
  * AssetStudio.dll
  * AssetStudioUtility.dll
  * SharpDX.dll
  * SharpDX.Mathematics.dll
  * System.Half.dll
  * TextureConverter.dll
  * TextureConverterWrapper.dll


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FNekuSoul%2FPhantomTool.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FNekuSoul%2FPhantomTool?ref=badge_large)