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
$game_dir = "D:\Hry\steamapps\common\SpaceEngineers";
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
Copy-Item "$game_dir\Content\Data\Components.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\PhysicalItems.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\PhysicalItems_Economy.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Weapons.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item -Recurse "$game_dir\Content\Data\CubeBlocks" "$mod_dir_no_scripts\Data\";

$config = Get-Content -Path .\ColorfulItems\Data\Scripts\Sisk\Config.cs -Raw

$sbcFiles = @(
	"AmmoMagazines.sbc",
	"Ammos.sbc",
	"BlockVariantGroups.sbc",
	"Components.sbc",
	"PhysicalItems.sbc",
	"PhysicalItems_Economy.sbc",
	"Weapons.sbc",
	"CubeBlocks/CubeBlocks.sbc",
	"CubeBlocks/CubeBlocks_Armor.sbc",
	"CubeBlocks/CubeBlocks_ArmorPanels.sbc",
	"CubeBlocks/CubeBlocks_Armor_2.sbc",
	"CubeBlocks/CubeBlocks_Armor_3.sbc",
	"CubeBlocks/CubeBlocks_Automation.sbc",
	"CubeBlocks/CubeBlocks_Communications.sbc",
	"CubeBlocks/CubeBlocks_Control.sbc",
	"CubeBlocks/CubeBlocks_DecorativePack.sbc",
	"CubeBlocks/CubeBlocks_DecorativePack2.sbc",
	"CubeBlocks/CubeBlocks_Doors.sbc",
	"CubeBlocks/CubeBlocks_Economy.sbc",
	"CubeBlocks/CubeBlocks_Energy.sbc",
	"CubeBlocks/CubeBlocks_Extras.sbc",
	"CubeBlocks/CubeBlocks_Frostbite.sbc",
	"CubeBlocks/CubeBlocks_Gravity.sbc",
	"CubeBlocks/CubeBlocks_GridAIPack.sbc",
	"CubeBlocks/CubeBlocks_IndustrialPack.sbc",
	"CubeBlocks/CubeBlocks_Interiors.sbc",
	"CubeBlocks/CubeBlocks_LCDPanels.sbc",
	"CubeBlocks/CubeBlocks_Lights.sbc",
	"CubeBlocks/CubeBlocks_Logistics.sbc",
	"CubeBlocks/CubeBlocks_Mechanical.sbc",
	"CubeBlocks/CubeBlocks_Medical.sbc",
	"CubeBlocks/CubeBlocks_Production.sbc",
	"CubeBlocks/CubeBlocks_ScrapRacePack.sbc",
	"CubeBlocks/CubeBlocks_SparksOfTheFuturePack.sbc",
	"CubeBlocks/CubeBlocks_Symbols.sbc",
	"CubeBlocks/CubeBlocks_Thrusters.sbc",
	"CubeBlocks/CubeBlocks_Tools.sbc",
	"CubeBlocks/CubeBlocks_Utility.sbc",
	"CubeBlocks/CubeBlocks_Warfare1.sbc",
	"CubeBlocks/CubeBlocks_Warfare2.sbc",
	"CubeBlocks/CubeBlocks_Weapons.sbc",
	"CubeBlocks/CubeBlocks_Wheels.sbc",
	"CubeBlocks/CubeBlocks_Windows.sbc"
);

# Wrapped in job, so that we can run it again without starting new PowerShell, otherwise Add-Type fails.
Start-Job -ScriptBlock {
	function replace_icons_paths {
		param (
			$content,
			$paths
		)
		foreach ($value in $paths){
			$icon_path = $value -replace "/", "\";
			$icon_path_mod = $icon_path -replace "Textures\\", "Textures\$textures_sub_dir\";
			$content = ($content -replace [Regex]::Escape($icon_path), $icon_path_mod);
		}
		return $content;
	}

	Add-Type -TypeDefinition $using:config -Language CSharp

	foreach ($sbc in $using:sbcFiles){
		$sbcContent = Get-Content -Path "$using:game_dir\Content\Data\$sbc";
		$sbcContent = replace_icons_paths $sbcContent $([Sisk.ColorfulIcons.Config]::Components.Values);
		$sbcContent = replace_icons_paths $sbcContent $([Sisk.ColorfulIcons.Config]::Blocks.Values);
		$sbcContent = replace_icons_paths $sbcContent $([Sisk.ColorfulIcons.Config]::Ingots.Values);
		$sbcContent = replace_icons_paths $sbcContent $([Sisk.ColorfulIcons.Config]::Ores.Values);
		$sbcContent = replace_icons_paths $sbcContent $([Sisk.ColorfulIcons.Config]::Tools.Values);
		Set-Content -Path "$using:mod_dir_no_scripts\Data\$sbc" -Value $sbcContent -Encoding UTF8
	}
} | Receive-Job -Wait -AutoRemoveJob