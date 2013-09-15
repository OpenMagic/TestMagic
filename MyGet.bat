@echo off

CALL "%VS110COMNTOOLS%vsvars32.bat"

msbuild /p:Configuration=Release

pushd .nuget
nuget pack TestMagic.nuspec -Version %PackageVersion%
popd