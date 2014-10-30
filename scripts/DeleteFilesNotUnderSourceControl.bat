@echo off

echo.
echo Changing to solution folder...
pushd %~dp0..\

echo.
echo Deleting files not under source control...
echo.
git clean -d --force -X

echo.
echo Changing back to original directory...
popd

echo.
echo Successfully deleted files not under source control.
echo.
pause