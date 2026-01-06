param(
    [switch]$System
)

$ErrorActionPreference = "Stop"

$BinaryName = "resolver"
$AppName = "Resolver"
$Repo = "DMiradakis/resolver"

if ($System) {
    $InstallDir = "$env:ProgramFiles\$AppName"
    $RequiresAdmin = $true
} else {
    $InstallDir = "$env:LOCALAPPDATA\Programs\$AppName"
    $RequiresAdmin = $false
}

# Check if running as admin when needed
if ($RequiresAdmin) {
    $IsAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    if (-not $IsAdmin) {
        Write-Error "System-wide installation requires administrator privileges. Run PowerShell as Administrator or omit -System flag."
        exit 1
    }
}

# Detect architecture
$Arch = if ([Environment]::Is64BitOperatingSystem) { 
    if ([Environment]::GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") -eq "ARM64") {
        "arm64"
    } else {
        "x64"
    }
} else {
    Write-Error "32-bit systems are not supported"
    exit 1
}

# Get latest version
$Version = $env:BINARY_VERSION
if (-not $Version -or $Version -eq "latest") {
    $Response = Invoke-RestMethod -Uri "https://api.github.com/repos/$Repo/releases/latest"
    $Version = $Response.tag_name
}

$DownloadUrl = "https://github.com/$Repo/releases/download/$Version/$BinaryName-windows-$Arch.exe"

Write-Host "Installing $BinaryName $Version for windows-$Arch to $InstallDir..."

# Create install directory
New-Item -ItemType Directory -Force -Path $InstallDir | Out-Null

# Download binary
$DestPath = Join-Path $InstallDir "$BinaryName.exe"
Invoke-WebRequest -Uri $DownloadUrl -OutFile $DestPath

Write-Host "✓ Installed $BinaryName to $DestPath"

# Add to PATH
if ($System) {
    $MachinePath = [Environment]::GetEnvironmentVariable("Path", "Machine")
    if ($MachinePath -notlike "*$InstallDir*") {
        [Environment]::SetEnvironmentVariable("Path", "$MachinePath;$InstallDir", "Machine")
        Write-Host "✓ Added to system PATH"
    }
} else {
    $UserPath = [Environment]::GetEnvironmentVariable("Path", "User")
    if ($UserPath -notlike "*$InstallDir*") {
        [Environment]::SetEnvironmentVariable("Path", "$UserPath;$InstallDir", "User")
        Write-Host "✓ Added to user PATH"
    }
}

Write-Host ""
Write-Host "⚠ Restart your terminal to use $BinaryName"