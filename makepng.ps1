# Source and Target directories
$source_dir = ".\textures\modded"
$target_dir = ".\PNG"

# Delete existing target directory if it exists
if (Test-Path -Path $target_dir -PathType Container) {
    Remove-Item -Path $target_dir -Recurse -Force
}

# Create a new target directory
New-Item -ItemType "directory" $target_dir | Out-Null

# Get files in the source directory and its subdirectories
$files = Get-ChildItem -Path "$source_dir\*" -File -Recurse
$count = $files.Length
$done = 0

# Display progress information
Write-Progress -Activity "Converting to PNG" -Status "0% Complete:" -PercentComplete 0

# Loop through each file in source directory
foreach ($file in $files) {

    # Calculate relative path and the new PNG file name
    $relativePath = $file.FullName.Substring((Resolve-Path ".\$source_dir").Path.Length + 1)
    $newFile = [System.IO.Path]::ChangeExtension($relativePath, "png")
    $outputPath = Join-Path -Path $target_dir -ChildPath $newFile

    # Create subdirectories in target directory if needed
    $targetSubDir = Join-Path -Path $target_dir -ChildPath (Split-Path -Path $newFile -Parent)
    if (-not (Test-Path -Path $targetSubDir -PathType Container)) {
        New-Item -ItemType "directory" -Force -Path $targetSubDir | Out-Null
    }

    # Check if file is an XCF file and convert it to PNG
    if ($file.Extension -eq ".xcf") {

        # XCF to PNG
        & magick convert -layers merge -background none -alpha background $file.FullName $outputPath

    }

    # Update progress information
    $done++
    $pct = [Math]::Round(100 * $done / $count)
    Write-Progress -Activity "Converting XCF to PNG" -Status "$pct% Complete:" -CurrentOperation $file.FullName -PercentComplete $pct
}