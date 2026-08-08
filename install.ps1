$mods_dir = "$env:APPDATA\SpaceEngineers\Mods";
$mod_name = "ColorfulItems";
$mod_dir = "$mods_dir\$mod_name";
$exclude = @(".git", ".gitignore", ".sln", ".csproj", "\bin\", "\obj\", "\Properties\", ".md", "LICENSE", "\Utils\Logging\", "\Utils\Net\", "\Utils\Profiler\", "\Utils\TerminalControls\");

if(Test-Path -Path "$mod_dir"){
	Remove-Item -Recurse "$mod_dir";
}
New-Item -ItemType "directory" "$mod_dir" | Out-Null;

#Copy-Item -Recurse -Exclude $copy_exclude ".\$mod_name\*" "$mod_dir";
#Get-ChildItem -Path ".\$mod_name\*" -Exclude $copy_exclude -File -Recurse | Copy-Item -Destination {Join-Path $mod_dir $_.FullName.Substring((Resolve-Path ".\$mod_name").Path.length)} -Recurse

Get-ChildItem -Path ".\$mod_name\*" -File -Recurse | ForEach-Object {
	$skip = $false;
	$file = $_.FullName.Substring((Resolve-Path ".\$mod_name").Path.length);
	foreach($e in $exclude){
		if($_.FullName.Contains($e)){
			$skip = $true;
			return;
		}
	}
	if(!$skip){
		$dir = $file.Substring(0, $file.LastIndexOf("\"));
		if($dir -ne "" -and !(Test-Path -Path "$mod_dir$dir")){
			New-Item -ItemType "directory" "$mod_dir$dir" | Out-Null;
		}
		Copy-Item -Path ".\$mod_name$file" -Destination "$mod_dir$dir\";
	}
};

# No scripts version
$game_dir = $env:SPACEENGINEERS_GAME_DIR;
$textures_sub_dir = "Colorful Icons";
$mod_dir_no_scripts = "$mods_dir\$($mod_name)NoScripts";
if(Test-Path -Path "$mod_dir_no_scripts"){
	Remove-Item -Recurse "$mod_dir_no_scripts";
}
New-Item -ItemType "directory" "$mod_dir_no_scripts" | Out-Null;
Copy-Item "$mod_dir\thumb.png" "$mod_dir_no_scripts\";
Copy-Item "$mod_dir\metadata.mod" "$mod_dir_no_scripts\";
Copy-Item "$mod_dir\modinfo.sbmi" "$mod_dir_no_scripts\";
Copy-Item -Recurse "$mod_dir\Textures" "$mod_dir_no_scripts\Textures\$textures_sub_dir";

New-Item -ItemType "directory" "$mod_dir_no_scripts\Data";

Copy-Item "$game_dir\Content\Data\AmmoMagazines.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Ammos.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\BlockVariantGroups.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Blueprints.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\BlueprintClasses.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Blueprints_Food.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Components.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\PhysicalItems.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\PhysicalItems_Food.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Weapons.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item -Recurse "$game_dir\Content\Data\CubeBlocks" "$mod_dir_no_scripts\Data\";

Copy-Item ".\ColorfulItemsNoScripts\modinfo.sbmi" "$mod_dir_no_scripts\"

$config = Get-Content -Path .\ColorfulItems\Data\Scripts\Sisk\Config.cs -Raw

$sbcFiles = Get-ChildItem -Path "$mod_dir_no_scripts\Data" -Filter *.sbc -Recurse | Select-Object -ExpandProperty FullName

# Wrapped in job, so that we can run it again without starting new PowerShell, otherwise Add-Type fails.
Start-Job -ScriptBlock {
	Add-Type -TypeDefinition $using:config -Language CSharp

	$iconMap = @{}
	$dicts = @(
		[Sisk.ColorfulIcons.Config]::Components,
		[Sisk.ColorfulIcons.Config]::Blocks,
		[Sisk.ColorfulIcons.Config]::Ingots,
		[Sisk.ColorfulIcons.Config]::Ores,
		[Sisk.ColorfulIcons.Config]::Tools
	)

	foreach ($dict in $dicts) {
		if ($null -eq $dict) { continue }
		foreach ($entry in $dict.GetEnumerator()) {
			$rawKey = $entry.Key
			$rawPath = $entry.Value
			if ([string]::IsNullOrEmpty($rawKey) -or [string]::IsNullOrEmpty($rawPath)) { continue }

			$iconPath = $rawPath -replace "/", "\"
			if ($iconPath -match "^Textures\\") {
				$modIconPath = $iconPath -replace "^Textures\\", "Textures\$using:textures_sub_dir\"
			} else {
				$modIconPath = "Textures\$using:textures_sub_dir\$iconPath"
			}

			$parts = $rawKey.Split('/')
			$typeIdPart = $parts[0]
			$subtypeIdPart = if ($parts.Length -gt 1) { $parts[1] } else { "" }
			if ($subtypeIdPart -eq "(null)") { $subtypeIdPart = "" }

			$shortTypeId = if ($typeIdPart.StartsWith("MyObjectBuilder_")) { $typeIdPart.Substring(16) } else { $typeIdPart }

			$iconMap["$shortTypeId/$subtypeIdPart"] = $modIconPath
			$iconMap["MyObjectBuilder_$shortTypeId/$subtypeIdPart"] = $modIconPath
		}
	}

	foreach ($sbc in $using:sbcFiles) {
		$xml = New-Object System.Xml.XmlDocument
		$xml.PreserveWhitespace = $true
		$xml.Load($sbc)

		$idNodes = $xml.GetElementsByTagName("Id")
		$modified = $false

		foreach ($idNode in $idNodes) {
			$typeNode = $idNode.SelectSingleNode("TypeId")
			$subtypeNode = $idNode.SelectSingleNode("SubtypeId")

			$typeId = if ($typeNode) { $typeNode.InnerText } else { $idNode.GetAttribute("TypeId") }
			if ([string]::IsNullOrEmpty($typeId)) { $typeId = $idNode.GetAttribute("Type") }

			$subtypeId = if ($subtypeNode) { $subtypeNode.InnerText } else { $idNode.GetAttribute("SubtypeId") }
			if ([string]::IsNullOrEmpty($subtypeId)) { $subtypeId = $idNode.GetAttribute("Subtype") }

			if ($null -ne $typeId) { $typeId = $typeId.Trim() } else { $typeId = "" }
			if ($null -ne $subtypeId) { $subtypeId = $subtypeId.Trim() } else { $subtypeId = "" }

			if ([string]::IsNullOrEmpty($typeId)) { continue }

			$lookupKey = "$typeId/$subtypeId"

			if ($iconMap.ContainsKey($lookupKey)) {
				$newIconPath = $iconMap[$lookupKey]
				$defNode = $idNode.ParentNode

				if ($null -ne $defNode) {
					$iconNodes = $defNode.SelectNodes(".//Icon")
					if ($null -ne $iconNodes -and $iconNodes.Count -gt 0) {
						$iconNodes[0].InnerText = $newIconPath
						$modified = $true
					} else {
						$newIconNode = $xml.CreateElement("Icon")
						$newIconNode.InnerText = $newIconPath
						$defNode.AppendChild($newIconNode)
						$modified = $true
					}
				}
			}
		}

		if ($modified) {
			$xml.Save($sbc)
		}
	}
} | Receive-Job -Wait -AutoRemoveJob
