# TODO: do this with make

$source_dir = ".\textures\modded"
$target_dir = ".\ColorfulItems\Textures\GUI\Icons"
$texconvPath = "texconv"

# Create the target directory if it doesn't exist
if (!(Test-Path -Path $target_dir)) {
    New-Item -ItemType "directory" $target_dir | Out-Null
}

# Get all files in the source directory and its subdirectories
$files = Get-ChildItem -Path $source_dir -File -Recurse
$count = $files.Length
$done = 0

# Display progress information for the DDS conversion
Write-Progress -Activity "Converting XCF to DDS" -Status "0% Complete:" -PercentComplete 0

# Loop through each file in the source directory
foreach ($f in $files) {

    # Extract file and directory information
    $relativePath = $f.FullName.Substring((Resolve-Path ".\$source_dir").Path.Length + 1)
    $dir = Split-Path -Path $relativePath -Parent

    # Create subdirectories in the target directory if needed
    if ($dir -ne "" -and !(Test-Path -Path (Join-Path -Path $target_dir -ChildPath $dir))) {
        New-Item -ItemType "directory" -Force -Path (Join-Path -Path $target_dir -ChildPath $dir) | Out-Null
    }

    # Extract file extension
    $extension = $f.Extension.TrimStart('.')

    # Check if file is an XCF file and convert it to PNG
    if ($extension -eq "xcf") {

        # Generate new PNG file name
        $newfile = [System.IO.Path]::ChangeExtension($relativePath, "png")

        # XCF to PNG (MAGICK)
        magick convert -layers merge -background none -alpha background (Join-Path -Path $source_dir -ChildPath $relativePath) (Join-Path -Path $source_dir -ChildPath $newfile)
        
        # Update current file to the new PNG file
        $relativePath = $newfile
    }

    # PNG to DDS (TEXCONV)
    $out = & $texconvPath -o (Join-Path -Path $target_dir -ChildPath $dir) -ft dds -f BC3_UNORM -srgb -pmalpha -m 8 -y (Join-Path -Path $source_dir -ChildPath $relativePath) 2>&1
    if (-Not $?) {
        Write-Output $out
        break
    }

    # Remove temporary XCF to PNG file if created
    if ($extension -eq "xcf") {
        Remove-Item (Join-Path -Path $source_dir -ChildPath $relativePath)
    }

    # Update progress information
    $done++
    $pct = [Math]::Round(100 * $done / $count)
    Write-Progress -Activity "Converting XCF to DDS" -Status "$pct% Complete:" -CurrentOperation $relativePath -PercentComplete $pct
}
