<#
.SYNOPSIS
Restores psake 4.3.2 package.
#>

# Always run this script from solution's root folder.
Push-Location "$PSScriptRoot\.."

try {
    .\packages\NuGet\NuGet.exe install psake -Version 4.3.2 -OutputDirectory packages -ExcludeVersion
}
finally {
    Pop-Location
}