# Space Engineers Colorful Icons

Modification for Space Engineers. Install via [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=801185519).

Replaces default monochromatic icons of blocks, items, tools and weapons with colorful icons. More info in the Steam workshop description.

## Development info

Scripts require [texconv cli tool](https://github.com/microsoft/DirectXTex/releases) and [ImageMagick](https://imagemagick.org/) to be installed and in path.

### Convert DDS to PNG:
```
texconv.exe -ft png -o . "path\to\texture.dds"
```

### Create DDS files from PNG and XCF
```
.\makedds.ps1
```
This takes all PNG and XCF from `.\textures\modded` and saves them as DDS to `.\ColorfulItems\Textures\GUI\Icons`.

### Install mod
```
.\install.ps1
```
Run this to copy the mod directory (`.\ColorfulItems`) to the game's directory for local mods.