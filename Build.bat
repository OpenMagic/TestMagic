@echo off

rem =======================================================================
rem Bootstapper to build this solution.
rem
rem Build.bat
rem ---------
rem This option is ideal for double-clicking this file in Windows Explorer.
rem
rem It cleans, compiles & tests the solution then pauses.
rem
rem Build.bat continuous or Build.bat c
rem -----------------------------------
rem Same as "Build.bat" but when the build completes it prompts to run
rem the build again.
rem
rem Build.bat help
rem --------------
rem Get a list of all build tasks.
rem
rem Build.bat %1
rem ------------
rem %1 equals the build task to run
rem
rem Build.bat %1 continuous or Build.bat %1 c
rem ------------
rem Same as "Build.bat %1" but when the build completes it prompts to run
rem the build again.
rem
rem =======================================================================

:Build

pushd %~dp0

set BuildTask = %1

if "%1" == "continuous" set BuildTask = 
if "%1" == "c" set BuildTask = 

powershell.exe -file .\Build\Build.ps1 %BuildTask%

popd

if "%1" == "" (
    echo.
    echo.
    echo Press any key to exit . . .
    pause > nul
    goto exit
)

if "%1" == "continuous" goto Continuous
if "%2" == "continuous" goto Continuous
if "%1" == "c" goto Continuous
if "%2" == "c" goto Continuous
goto exit

:Continuous
echo.
echo.
echo Press any key to build again . . .
pause > nul
echo.
goto Build

:exit