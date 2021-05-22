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
$mod_dir_no_scripts = "$mods_dir\$($mod_name)NoScripts";
if(Test-Path -Path "$mod_dir_no_scripts"){
	Remove-Item -Recurse "$mod_dir_no_scripts";
}
New-Item -ItemType "directory" "$mod_dir_no_scripts" | Out-Null;
Copy-Item "$mod_dir\thumb.png" "$mod_dir_no_scripts\";
Copy-Item "$mod_dir\metadata.mod" "$mod_dir_no_scripts\";
Copy-Item "$mod_dir\modinfo.sbmi" "$mod_dir_no_scripts\";
Copy-Item -Recurse "$mod_dir\Textures" "$mod_dir_no_scripts\Textures";

New-Item -ItemType "directory" "$mod_dir_no_scripts\Data";

Copy-Item "$game_dir\Content\Data\AmmoMagazines.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Ammos.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Components.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\PhysicalItems.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\PhysicalItems_Economy.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item "$game_dir\Content\Data\Weapons.sbc" "$mod_dir_no_scripts\Data\";
Copy-Item -Recurse "$game_dir\Content\Data\CubeBlocks" "$mod_dir_no_scripts\Data\";