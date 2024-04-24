@echo off

dotnet build ../src/DriftwoodYeet.csproj
rmdir /s /q "..\bin"
mkdir "..\bin"
move "..\src\bin\Debug\netstandard2.1\DriftwoodYeet.dll" "..\bin\DriftwoodYeet.dll"