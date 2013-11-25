set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

ml /c /Fo%OBJ_DIR%\StringTest.obj StringTest.asm
ml /c /Fo%OBJ_DIR%\System.Math.obj System.Math.asm
ml /c /Fo%OBJ_DIR%\System.Math.impl.obj System.Math.impl.asm
ml /c /Fo%OBJ_DIR%\System.Console.obj System.Console.asm
ml /c /Fo%OBJ_DIR%\System.String.obj System.String.asm
ml /c /Fo%OBJ_DIR%\System.Memory.obj System.Memory.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Samples.StringTest.pdb /out:%BIN_DIR%\Samples.StringTest.EXE %OBJ_DIR%\StringTest.obj %OBJ_DIR%\System.Math.obj %OBJ_DIR%\System.Math.impl.obj %OBJ_DIR%\System.Console.obj %OBJ_DIR%\System.String.obj %OBJ_DIR%\System.Memory.obj msvcrt.lib
