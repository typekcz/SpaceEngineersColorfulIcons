$mods_dir = "$env:APPDATA\SpaceEngineers\Mods";
$mod_name = "ColorfulItems";
$mod_dir = "$mods_dir\$mod_name";
$exclude = @(".git", ".gitignore", ".sln", ".csproj", "\bin\", "\obj\", "\Properties\");

if(Test-Path -Path "$mod_dir"){
	Remove-Item -Recurse "$mod_dir";
}

if(!(Test-Path -Path "$mod_dir")){
	New-Item -ItemType "directory" "$mod_dir" | Out-Null;
}

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
		Copy-Item -Path ".\$mod_name\$file" -Destination "$mod_dir$file" -Force;
	}
};