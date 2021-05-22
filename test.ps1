$config = Get-Content -Path .\ColorfulItems\Data\Scripts\Sisk\Config.cs -Raw

Add-Type -TypeDefinition $config -Language CSharp

[Sisk.ColorfulIcons.Config]::Ores

Select-Xml -Path "D:\Hry\steamapps\common\SpaceEngineers\Content\Data\Components.sbc" -XPath '/Definitions/Components'