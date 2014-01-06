set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

del /q %OBJ_DIR%\*.obj

for /F "" %%i in (sources) do (
  %TEAC% /Fa%OBJ_DIR%\%%~ni.asm /I..\System /I..\Test /I..\System\IO /I..\System\Graphics /I..\System\Graphics\IO /I. /IShapes /ILights %%i
  if NOT ERRORLEVEL 1 (
    ml /c /Fo%OBJ_DIR%\%%~ni.obj %OBJ_DIR%\%%~ni.asm
  )
)

if NOT ERRORLEVEL 1 (
    link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\SystemTests.pdb /out:%BIN_DIR%\SystemTests.EXE %OBJ_DIR%\SystemTests.Program.obj %OBJ_DIR%\SystemTests.MathModule.obj %BIN_DIR%\Test.lib
)
