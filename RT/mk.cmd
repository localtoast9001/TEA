set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

for /F "" %%i in (sources) do (
  %TEAC% /Fa%OBJ_DIR%\%%~ni.asm /I..\System /I. /IShapes %%i
  if NOT ERRORLEVEL 1 (
    ml /c /Fo%OBJ_DIR%\%%~ni.obj %OBJ_DIR%\%%~ni.asm
  )
)

