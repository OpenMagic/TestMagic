@echo off

pushd %~dp0\..\

echo Current directory: 
cd
echo.

echo Setting up build environment...
echo -------------------------------
echo.

set config=%1
if "%config%" == "" (
   set config=Release
)

if "%PackageVersion%" == "" (

  rem Simulate myget.org environment.
  set PackageVersion=999.99
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

echo Restoring packages...
echo -------------------------------
rem Packages must be explicity restored otherwise NullGuard.Fody does not run.
rem
rem Packages folder must be packages otherwise MyGet will push dependant
rem packages to my feed.
echo.
.nuget\nuget restore -PackagesDirectory .\packages
if not "%errorlevel%" == "0" goto Error
echo.
echo.

echo Building solution...
echo -------------------------------
echo.

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild /p:Configuration=%config%
if not "%errorlevel%" == "0" goto Error
echo.
echo.

echo Running unit tests...
echo -------------------------------
echo.

if "%GallioEcho%" == "" (

  if exist "C:\Program Files\Gallio\bin\Gallio.Echo.exe" (

    echo Setting GallioEcho environment variable...
    set GallioEcho=""C:\Program Files\Gallio\bin\Gallio.Echo.exe""
    
  ) else (

	echo Gallio is required to run unit tests. Try cinst Gallio.
	goto Error
	
  )
)

"%GallioEcho%" Projects\TestMagic.Tests\bin\Release\TestMagic.Tests.dll
if not "%errorlevel%" == "0" goto Error
echo.
echo.

echo Building NuGet package...
echo -------------------------
echo.

if exist .\Build\nul (

  echo .\Build folder exists.
  
) else (

  echo Creating .\Build folder...
  md .\Build
)

echo.
echo Creating NuGet package...

.\.nuget\nuget pack .\.nuget\TestMagic.nuspec -o .\Build %Version%

if not "%errorlevel%" == "0" goto Error
echo.
echo.

:Success
echo.
echo.
echo Build was successful.
echo =====================
echo.
echo.
popd
exit 0

:Error
echo.
echo.
echo **************************
echo *** An error occurred. ***
echo **************************
echo.
echo.
popd
exit -1