set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\Debug\TEAC.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

for /F "" %%i in (sources) do (
  %TEAC% /Fa%OBJ_DIR%\%%~ni.asm /I..\System %%i
  if NOT ERRORLEVEL 1 (
    ml /c /Fo%OBJ_DIR%\%%~ni.obj %OBJ_DIR%\%%~ni.asm
  )
)

lib /SUBSYSTEM:CONSOLE /out:%BIN_DIR%\Test.lib %OBJ_DIR%\Test.Test.obj %OBJ_DIR%\Test.TestContext.obj %OBJ_DIR%\Test.TestRunner.obj %OBJ_DIR%\Test.TestIterator.obj %OBJ_DIR%\Test.ArrayTestIterator.obj %BIN_DIR%\System.lib
