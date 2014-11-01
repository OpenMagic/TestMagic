Framework "4.5.1"

properties {

    # Versosity levels
    $msBuildVerbosity = "minimal"
    $nuGetVerbosity = "normal"

    # Configuration properties that are likely to be the same for every solution.
    $msBuildConfiguration = "Release"

    # Support properties that are likely to be the same for every solution.
    $solution = Get-Solution
    $solutionDirectory = Split-Path $solution -Parent
    $artifactsDirectory = "$solutionDirectory\artifacts"
    $logsDirectory = "$artifactsDirectory\logs"
    $packagesDirectory = "$solutionDirectory\packages"
    $testsDirectory = "$solutionDirectory\tests"
    $nuget = "$packagesDirectory\NuGet\NuGet.exe"
    $xunit = "$packagesDirectory\xunit.runners\tools\xunit.console.clr4.exe"
    $msBuildLog = "$logsDirectory\msbuild.log"
}

# Cleans the solution by removing bin and obj for the requested configuration.
Task Clean {
    
    Exec { msbuild $solution /target:Clean /verbosity:$msBuildVerbosity /property:Configuration=$msBuildConfiguration }
    
    if (Test-Path $artifactsDirectory) {

        Write-Host "Deleting artifacts directory..."
        Remove-Item -LiteralPath $artifactsDirectory -Recurse -Force | Out-Null
        Write-Host "Deleted artifacts directory."
    }
    
    Write-Host "Creating artifacts directory..."
    New-Item -Type Directory -Path $artifactsDirectory | Out-Null
    New-Item -Type Directory -Path $artifactsDirectory\logs | Out-Null
    Write-Host "Deleted artifacts directory."

    Write-Host
}

# Restores all packages.
Task Restore-Packages {

    # Restore solution defined packages.
    Exec { & $nuget restore $solution -PackagesDirectory $packagesDirectory -Verbosity $nuGetVerbosity -ConfigFile .\NuGet.config -NonInteractive }

    # Install xunit.runners. 
    #
    # This script allows the most recent version of xunit.runners to be installed. At time of writing the current version was 1.9.2.
    if ((Test-Path $xunit)) {
        Write-Host "Package ""xunit.runners"" is already installed."
    } else {
        Write-Host "Installing 'xunit.runners'..."
        Exec { & $nuget install xunit.runners -OutputDirectory $packagesDirectory -ConfigFile .\NuGet.config -NonInteractive -ExcludeVersion }
    }

    # Validate xunit exe has been installed.
    if (!(Test-Path $xunit)) {
        throw "Cannot find '$xunit'."
    }

    Write-Host
}

# Compile the solution.
Task Compile -depends Clean, Restore-Packages {

    # psake cannot handle msbuild parameters with ;. The easiest workaround is to use variables when a parameter has multiple values.
	# It best to use variables for msbuild parameters that have multiple values others 
    $property = "Configuration=$msBuildConfiguration"
    $fileLoggerParameters = "LogFile=$msBuildLog;Verbosity=diagnostic;"

    Exec { msbuild $solution /target:ReBuild /verbosity:$msBuildVerbosity /property:$property /fileLoggerParameters:$fileLoggerParameters }

    Write-Host
}

# Test the solution.
Task Test -depends Restore-Packages, Compile {

    # For each directory in $testsDirectory run $xunit on $testDirectory's test assembly.
    Get-ChildItem $testsDirectory -Directory |
        ForEach-Object {

            $testDirectory = $_.FullName
            $testDirectoryName = $_.Name
            $testAssembly = "$testDirectory\bin\$msBuildConfiguration\$testDirectoryName.dll"
            
            Write-Host "Running tests in '$testAssembly'..."
            Write-Host
            Exec { & $xunit $testAssembly /xml "$logsDirectory\$testDirectoryName.xml" /html "$logsDirectory\$testDirectoryName.html" }
        }

    Write-Host
}

Task Package -depends Clean, Compile, Test {
    Write-Host "todo: Package"
    Write-Host "todo: Help"
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