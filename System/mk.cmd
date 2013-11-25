set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\Debug\TEAC.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\System.Math.asm System.Math.tea
%TEAC% /Fa%OBJ_DIR%\System.Console.asm System.Console.tea
%TEAC% /Fa%OBJ_DIR%\System.String.asm System.String.tea
%TEAC% /Fa%OBJ_DIR%\System.Memory.asm System.Memory.tea
ml /c /Fo%OBJ_DIR%\System.Math.obj %OBJ_DIR%\System.Math.asm
ml /c /Fo%OBJ_DIR%\System.Math.impl.obj System.Math.impl.asm
ml /c /Fo%OBJ_DIR%\System.Memory.obj %OBJ_DIR%\System.Memory.asm
ml /c /Fo%OBJ_DIR%\System.Console.obj %OBJ_DIR%\System.Console.asm
ml /c /Fo%OBJ_DIR%\System.String.obj %OBJ_DIR%\System.String.asm
lib /SUBSYSTEM:CONSOLE /out:%BIN_DIR%\System.lib %OBJ_DIR%\System.Math.obj %OBJ_DIR%\System.Math.impl.obj %OBJ_DIR%\System.Console.obj %OBJ_DIR%\System.String.obj %OBJ_DIR%\System.Memory.obj msvcrt.lib
