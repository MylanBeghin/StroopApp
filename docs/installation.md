# Installation Guide

StroopApp is designed to be easy to install and run on Windows systems with minimal setup required.

## Quick Start (Recommended)

### Download Pre-built Executable

1. Go to the [Releases page](https://github.com/MylanBeghin/StroopApp/releases)
2. Download the latest `StroopApp.exe` from the release assets
3. Double-click `StroopApp.exe` to run
4. No installation required!

**Note**: The first launch may take a few seconds as the self-contained executable extracts dependencies.

## System Requirements

### Minimum Requirements
- **OS**: Windows 7 or later (tested on Windows 7, 10, and 11)
- **RAM**: 512 MB available memory
- **CPU**: Any modern processor (2015+)
- **Disk Space**: 200 MB for the application
- **Display**: 1280x720 resolution

### Recommended Requirements
- **OS**: Windows 10 or 11
- **RAM**: 1 GB available memory
- **Display**: 1920x1080 resolution (Full HD)

## Installation Methods

### Method 1: Standalone Executable (Recommended)

The standalone executable includes all dependencies and requires no additional setup.

**Advantages:**
- No .NET runtime installation needed
- Single file to manage
- Portable (can run from USB drive)
- No admin rights required

**Steps:**
1. Download `StroopApp.exe`
2. (Optional) Create a desktop shortcut
3. Run the application

### Method 2: Build from Source

For developers or researchers who want to modify the software:

**Prerequisites:**
- Visual Studio 2022 (version 17.8 or later) OR
- .NET 8 SDK

**Steps:**

```bash
# Clone the repository
git clone https://github.com/MylanBeghin/StroopApp.git
cd StroopApp

# Restore dependencies
dotnet restore StroopApp.sln

# Build the project
dotnet build StroopApp.sln --configuration Release

# Run the application
dotnet run --project StroopApp/StroopApp.csproj --configuration Release
```

**Build standalone executable:**

```powershell
# For PowerShell (single line)
dotnet publish StroopApp/StroopApp.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

```bash
# For Bash/Git Bash (multi-line with backslashes)
dotnet publish StroopApp/StroopApp.csproj \
  -c Release \
  -r win-x64 \
  --self-contained true \
  /p:PublishSingleFile=true \
  /p:IncludeNativeLibrariesForSelfExtract=true
```

The executable will be located at:  
`StroopApp/bin/Release/net8.0-windows/win-x64/publish/StroopApp.exe`

## First Launch

On first launch, StroopApp will:

1. Create a configuration directory at:  
   `%APPDATA%\StroopApp\`

2. Initialize default settings:
   - Language preference
   - Export folder location

3. Display the configuration page

## Configuration Folder

StroopApp stores configuration files in:
```
%APPDATA%\StroopApp\
|-- language.json          # UI language preference
|-- exportFolder.json      # Results export path
|-- profiles.json          # Experiment profiles
|-- participants.json      # Participant database
```

## Troubleshooting

### Application won't start

**Symptom**: Double-clicking the executable does nothing

**Solutions:**
1. Check Windows Defender or antivirus blocking the executable
2. Right-click the file, select Properties, then click Unblock
3. Ensure Windows 10 version 1809 or later

### "Missing DLL" errors

**Symptom**: Error about missing .NET components

**Solution:**  
This should not occur with the standalone executable. If it does:
1. Download the .NET 8 Desktop Runtime:  
   https://dotnet.microsoft.com/download/dotnet/8.0
2. Install and restart the application

### Performance issues

**Symptom**: Slow startup or sluggish UI

**Solutions:**
1. Close other resource-intensive applications
2. Ensure at least 1 GB free RAM
3. Check Task Manager for background processes

## Data Location

By default, experiment results are saved to:
```
%USERPROFILE%\Documents\StroopApp\Results\
```

You can configure a custom export location from the Configuration page.

## Uninstallation

To completely remove StroopApp:

1. Delete the executable file
2. (Optional) Delete configuration:  
   `%APPDATA%\StroopApp\`
3. (Optional) Delete experiment data:  
   `%USERPROFILE%\Documents\StroopApp\`

## Next Steps

- [Usage Guide](usage.md) - Learn how to use StroopApp
- [Experiment Configuration](experiments.md) - Understand all experiment parameters and common profile templates
