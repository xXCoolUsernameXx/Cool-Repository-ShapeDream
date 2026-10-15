@echo off
setlocal
cd /d "%~dp0"

echo [MemoryInsights] Building Debug DLL...
dotnet build MemoryInsights.csproj -c Debug

if errorlevel 1 (
    echo.
    echo [MemoryInsights] Build FAILED.
) else (
    echo.
    echo [MemoryInsights] Build succeeded.
    echo DLL: %CD%\bin\Debug\netstandard2.1\MemoryInsights.dll
)

echo.
pause
