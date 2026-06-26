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

### Generate LCDTextureDefinitions
```
.\gen_lcdtexturedef.ps1
```
This generates sbc files with LCDTextureDefinitions for icons to be used in user scripts.

### Install mod
```
.\install.ps1
```
Run this to copy the mod directory (`.\ColorfulItems`) to the game's directory for local mods.
## Inter-mod config API

The scripted mod publishes a local inter-mod API on message channel `205103492865`
(`(801185519 << 8) | 1`, derived from the Colorful Icons workshop id).
A client sends its own `long` reply channel as the payload. Colorful Icons replies on
that channel with this dictionary of delegates:

```csharp
payload["GetConfigVersion"] as Func<Version>,          // Api Version
payload["GetConfig"] as Func<string>,                  // Get current ModSettings as XML
payload["GetConfigBinary"] as Func<byte[]>,            // Get current ModSettings as a binary stream byte[]
payload["SubscribeConfigChanged"] as Action<Action>,   // Subscribe for ConfigChanged events
payload["UnsubscribeConfigChanged"] as Action<Action>  // Unsubscribe from ConfigChanged events
```

`ColorfulIconsApiClient.cs` is a copyable client
wrapper. The consuming mod owns the reply channel (recomended derive it from its ownworkshop id.)
