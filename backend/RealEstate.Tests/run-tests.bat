@echo off
echo  ========================================
echo  REAL ESTATE API - SUITE DE PRUEBAS
echo  ========================================

cd backend

echo  Compilando proyectos...
dotnet build --configuration Release

if %ERRORLEVEL% NEQ 0 (
    echo  Error en compilación
    pause
    exit /b 1
)

echo  Ejecutando pruebas unitarias...
dotnet test RealEstate.Tests --configuration Release --verbosity normal --logger "console;verbosity=detailed"

if %ERRORLEVEL% EQU 0 (
    echo  ¡Todas las pruebas pasaron!
) else (
    echo  Algunas pruebas fallaron
)

echo  Generando reporte de cobertura...
dotnet test RealEstate.Tests --collect:"XPlat Code Coverage" --results-directory ./TestResults

echo  ========================================
echo  PRUEBAS COMPLETADAS
echo  ========================================
