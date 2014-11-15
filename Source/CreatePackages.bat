@echo off
set nuget=solutions\.nuget\nuget.exe

set version=%1

%nuget% pack Yggdrasil.nuspec -Symbols -Version %version%