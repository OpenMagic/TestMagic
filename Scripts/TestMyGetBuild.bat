@echo off

rem Fake the MyGet build server environment.

call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat"
set PackageVersion=999.999

rem Do the MyGet build

pushd ..\
call MyGet.bat
popd 

echo.
echo.
pause
