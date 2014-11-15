@echo off
del *.nupkg /f /q
call CreatePackages.bat %1

for %%f in (*.nupkg) do %nuget% push %%f