Param (
    [Parameter(Mandatory=$True)]
    [string]$testsFolder,

    [Parameter(Mandatory=$True)]
    [string]$buildConfiguration
    )

if (-Not (Test-Path $testsFolder)) {
    throw "testsPath '$testsFolder' does not exist."
}

$msTest = """$(vswhere -property installationPath)\Common7\IDE\MSTest.exe"""

Get-ChildItem -Path $testsFolder -Directory |
        ForEach-Object {

            $fullFolder = $_.FullName
            $folderName = $_.Name
            $testAssembly = """$fullFolder\bin\$buildConfiguration\$folderName.dll"""

            Write-Host "Running tests in $testAssembly..."
            Write-Host "----------------------------------------------------------------------"
            Write-Host

            Invoke-Expression "&$mstest /testcontainer:$testAssembly"
            
            if ($LASTEXITCODE -ne 0) {
                throw "One or more unit tests failed in $testAssembly"
            }

            Write-Host
            Write-Host "Successfully ran all tests in $folderName."
            Write-Host
        }
