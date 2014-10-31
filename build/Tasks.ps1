Framework "4.5.1"

properties {

    # Configuration properties that are likely to be the same for every solution.
    $msBuildConfiguration = "Release"

    # Support properties that are likely to be the same for every solution.
    $solution = Get-Solution
    $solutionDirectory = Split-Path $solution -Parent
    $nuget = "$solutionDirectory\NuGet\NuGet.exe"
}

Task Package {
    Write-Host "todo"
}

TaskSetup {
    # All tasks expect to run from the solution directory.
    Push-Location $solutionDirectory
}

TaskTearDown {
    Pop-Location
}

FormatTaskName {
   param($taskName)

   Write-Host $taskName -ForegroundColor Cyan
   Write-Host "----------------------------------------------------------------------" -ForegroundColor Cyan
   Write-Host
}

# Get the full solution path.
#
# Convention dictates the solution file name is the solution directory name + .sln.
Function Get-Solution() {

    $solutionPath = Resolve-Path ..\
    $solutionDirectory = Split-Path $solutionPath -Leaf
    $solution = "$solutionPath\$solutionDirectory.sln"

    if (!(Test-Path $solution)) {
        throw "Cannot find solution file. Expected it to be '$solution'."        
    }
    
    Return $solution
}