@echo off

set CWD=%~dp0
set SEVEN=C:\Program Files\7-zip
set PATH=%PATH%;%SEVEN%

if "%1" NEQ "" (
  if "%1" EQU "-c" goto clean
)

:build
dotnet publish -c Release ..\src
7z a hitlady-release ..\src\bin\Release\net6.0\publish\*
goto end

:clean
del hitlady-release.7z
rmdir /S /Q ..\src\bin
rmdir /S /Q ..\src\obj

:end
