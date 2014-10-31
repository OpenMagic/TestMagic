<#
.SYNOPSIS
Restores NuGet.exe to [solution]\packages folder if it does not exist.
#>

# Always run this script from solution's root folder.
Push-Location "$PSScriptRoot\.."

try {

    $localNuGetDirectory = ".\packages\NuGet"

    if (!(Test-Path $localNuGetDirectory)) {
        Write-Verbose "Creating packages folder..."
        New-Item -Path $localNuGetDirectory -ItemType Directory | Out-Null
        Write-Verbose "Created packages folder."
    }

    $localNuGet = "$localNuGetDirectory\NuGet.exe"
    $remoteNuGet = "https://www.nuget.org/nuget.exe"

    if (Test-Path $localNuGet) {
        Write-Host """NuGet.exe"" is already installed."
        Exit
    }

    Write-Host "Installing 'NuGet.exe'..."
    Invoke-WebRequest -Uri $remoteNuGet -OutFile $localNuGet
    Write-Host "Successfully installed 'NuGet.exe'."
}
finally {

    Pop-Location
}