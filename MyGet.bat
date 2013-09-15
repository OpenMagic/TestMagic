@echo off

msbuild /p:Configuration=Release
echo.
echo.

mstest /TestContainer:Projects\TestMagic.Tests\bin\Release\TestMagic.Tests.dll
echo.
echo.

pushd .nuget
nuget pack TestMagic.nuspec -Version %PackageVersion%
popd