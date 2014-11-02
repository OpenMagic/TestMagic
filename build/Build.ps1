<#
.SYNOPSIS
Builds this solution.

.DESCRIPTION
Builds this solution.

.PARAMETER Task
OPTIONAL - The build task to run. Default is Package.

.EXAMPLE
Build Compile

.EXAMPLE
Build Help

Gets list of build tasks.
#>

param (
    [string]$task = "Package"
)

# Always run this script from solution's root folder.
Push-Location "$PSScriptRoot\.."

Function Restore-NuGetPackageExcludeVersion([string]$packageId, [string]$packageVersion) {

    .\packages\NuGet\NuGet.exe install $packageId -Version $packageVersion -OutputDirectory packages -ExcludeVersion
}

try {
    Write-Host
    Write-Host "Preparing build environment" -ForegroundColor Cyan
    Write-Host "----------------------------------------------------------------------" -ForegroundColor Cyan
    Write-Host

    .\Build\Restore-NuGet.ps1
    
    Restore-NuGetPackageExcludeVersion psake 4.3.2 
    Restore-NuGetPackageExcludeVersion YamlDotNet 3.3.0
    
    Write-Host
    Import-Module .\packages\psake\tools\psake.psm1
    Invoke-psake .\Build\Tasks.ps1 $task
}
finally {

    Pop-Location
}