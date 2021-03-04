# TODO: do this with make
$source_dir = ".\textures\modded";
$target_dir = ".\ColorfulItems\Textures\GUI\Icons";

if(!(Test-Path -Path $target_dir)){
	New-Item -ItemType "directory" $target_dir | Out-Null;
}
$files = Get-ChildItem -Path "$source_dir\*" -File -Recurse;
$count = $files.Length;
$done = 0;
Write-Progress -Activity "Converting to DDS" -Status "0% Complete:" -PercentComplete 0;
foreach($f in $files) {
	$file = $f.FullName.Substring((Resolve-Path ".\$source_dir").Path.length);
	$dir = $file.Substring(0, $file.LastIndexOf("\"));
	if($dir -ne "" -and !(Test-Path -Path "$target_dir\$dir")){
		New-Item -ItemType "directory" "$target_dir\$dir" | Out-Null;
	}
	$extension = $file.Substring($file.LastIndexOf(".") + 1);
	if($extension -eq "xcf"){
		$newfile = $file.Substring(0, $file.Length-$extension.Length);
		$newfile = $newfile + "png";
		# XCF to PNG
		magick convert -layers merge -background none -alpha background "$source_dir\$file" "$source_dir\$newfile"
		$file = $newfile;
	}
	# PNG to DDS
	$out = & texconv -o "$target_dir\$dir" -ft dds -f BC3_UNORM -srgb -pmalpha -m 8 -y "$source_dir\$file" 2>&1
	if(-Not $?){
		Write-Output $out;
		break;
	}
	if($extension -eq "xcf"){
		Remove-Item "$source_dir\$newfile";
	}
	$done++;
	$pct = [Math]::Round(100*$done/$count);
	Write-Progress -Activity "Converting to DDS" -Status "$pct% Complete:" -CurrentOperation $file -PercentComplete $pct;
};