set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\..\bin
set TEAC=%~dp0..\..\TEAC\bin\Debug\TEAC.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\System.ExtendedMath.Complex.asm System.ExtendedMath.Complex.tea
ml /c /Fo%OBJ_DIR%\System.ExtendedMath.Complex.obj %OBJ_DIR%\System.ExtendedMath.Complex.asm
lib /SUBSYSTEM:CONSOLE /out:%BIN_DIR%\System.ExtendedMath.lib %OBJ_DIR%\System.ExtendedMath.Complex.obj 
