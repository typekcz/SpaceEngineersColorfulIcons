# Generate LCDTextureDefinitions for use in scripts

$config = Get-Content -Path .\ColorfulItems\Data\Scripts\Sisk\Config.cs -Raw
Add-Type -TypeDefinition $config -Language CSharp

function Write-LCDTextureDefinitions {
	param (
		$definitions,
		$name
	)

	$xmlObjectsettings = New-Object System.Xml.XmlWriterSettings
	$xmlObjectsettings.Indent = $true
	$xmlObjectsettings.IndentChars = "    "
	$xmlWriter = [System.Xml.XmlWriter]::Create(".\ColorfulItems\Data\LCDTextures_$name.sbc", $xmlObjectsettings)
	$xmlWriter.WriteStartDocument()
	$xmlWriter.WriteStartElement("Definitions")
	$xmlWriter.WriteAttributeString("xmlns", "xsi", $null, "http://www.w3.org/2001/XMLSchema-instance")
	$xmlWriter.WriteAttributeString("xmlns", "xsd", $null, "http://www.w3.org/2001/XMLSchema")
	$xmlWriter.WriteStartElement("LCDTextures")

	foreach($def in $definitions.GetEnumerator()){
		# Skip blueprint definitions
		if($def.Key.StartsWith("MyObjectBuilder_BlueprintDefinition")){
			continue
		}
		$subtype = $def.Key -replace "^[^_]+", "ColorfulIcons"

		$xmlWriter.WriteStartElement("LCDTextureDefinition")

		$xmlWriter.WriteStartElement("Id")
		$xmlWriter.WriteAttributeString("Type", "LCDTextureDefinition")
		$xmlWriter.WriteAttributeString("Subtype", $subtype)
		$xmlWriter.WriteEndElement() # Id

		$xmlWriter.WriteElementString("SpritePath", $def.Value)
		$xmlWriter.WriteElementString("TexturePath", $def.Value)
		$xmlWriter.WriteElementString("Selectable", "false")

		$xmlWriter.WriteEndElement() # LCDTextureDefinition
	}

	$xmlWriter.WriteEndElement() # LCDTextures
	$xmlWriter.WriteEndElement() # Definitions
	$xmlWriter.WriteEndDocument()
	$xmlWriter.Flush()
	$xmlWriter.Close()
}

Write-LCDTextureDefinitions $([Sisk.ColorfulIcons.Config]::Blocks) "Blocks"
Write-LCDTextureDefinitions $([Sisk.ColorfulIcons.Config]::Components) "Components"
Write-LCDTextureDefinitions $([Sisk.ColorfulIcons.Config]::Ingots) "Ingots"
Write-LCDTextureDefinitions $([Sisk.ColorfulIcons.Config]::Ores) "Ores"
Write-LCDTextureDefinitions $([Sisk.ColorfulIcons.Config]::Tools) "Tools"
Write-LCDTextureDefinitions $([Sisk.ColorfulIcons.Config]::OldComponents) "OldComponents"