# Source and Target directories
$source_dir = ".\textures\modded"
$target_dir = ".\PNG"

# Delete existing target directory if it exists
if (Test-Path -Path $target_dir -PathType Container) {
    Remove-Item -Path $target_dir -Recurse -Force
}

# Create a new target directory
New-Item -ItemType "directory" $target_dir | Out-Null

# Get XCF files in the source directory and its subdirectories
$xcfFiles = Get-ChildItem -Path "$source_dir\*.xcf" -File -Recurse

# Display progress information for XCF to PNG conversion
Write-Progress -Activity "Converting XCF to PNG" -Status "0% Complete:" -PercentComplete 0

foreach ($xcfFile in $xcfFiles) {

    # Calculate relative path and the new PNG file name
    $relativePath = $xcfFile.FullName.Substring((Resolve-Path ".\$source_dir").Path.Length + 1)
    $newFile = [System.IO.Path]::ChangeExtension($relativePath, "png")
    $outputPath = Join-Path -Path $target_dir -ChildPath $newFile

    # Create subdirectories in target directory if needed
    $targetSubDir = Join-Path -Path $target_dir -ChildPath (Split-Path -Path $newFile -Parent)
    if (-not (Test-Path -Path $targetSubDir -PathType Container)) {
        New-Item -ItemType "directory" -Force -Path $targetSubDir | Out-Null
    }

    # XCF to PNG conversion using magick
    & magick convert -layers merge -background none -alpha background $xcfFile.FullName $outputPath

    # Update progress information
    Write-Progress -Activity "Converting XCF to PNG" -Status "$(($xcfFiles.IndexOf($xcfFile) + 1) / $xcfFiles.Count * 100)% Complete:" -CurrentOperation $xcfFile.FullName -PercentComplete (($xcfFiles.IndexOf($xcfFile) + 1) / $xcfFiles.Count * 100)
}

# Get existing PNG files in the source directory and its subdirectories
$pngFiles = Get-ChildItem -Path "$source_dir\*.png" -File -Recurse

# Display progress information for moving existing PNG files
Write-Progress -Activity "Moving existing PNG files" -Status "0% Complete:" -PercentComplete 0

foreach ($pngFile in $pngFiles) {

    # Calculate relative path and the new PNG file name
    $relativePath = $pngFile.FullName.Substring((Resolve-Path ".\$source_dir").Path.Length + 1)
    $outputPath = Join-Path -Path $target_dir -ChildPath $relativePath

    # Create subdirectories in target directory if needed
    $targetSubDir = Join-Path -Path $target_dir -ChildPath (Split-Path -Path $relativePath -Parent)
    if (-not (Test-Path -Path $targetSubDir -PathType Container)) {
        New-Item -ItemType "directory" -Force -Path $targetSubDir | Out-Null
    }

    # Copy the existing PNG file to the target directory
    Copy-Item -Path $pngFile.FullName -Destination $outputPath -Force

    # Update progress information
    Write-Progress -Activity "Moving existing PNG files" -Status "$(($pngFiles.IndexOf($pngFile) + 1) / $pngFiles.Count * 100)% Complete:" -CurrentOperation $pngFile.FullName -PercentComplete (($pngFiles.IndexOf($pngFile) + 1) / $pngFiles.Count * 100)
}

# Display completion message
Write-Host "Conversion and move completed."
