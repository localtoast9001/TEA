set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

ml /c /Fo%OBJ_DIR%\System.Console.obj System.Console.asm
ml /c /Fo%OBJ_DIR%\HelloWorld.obj HelloWorld.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\HelloWorld.pdb /out:%BIN_DIR%\HelloWorld.EXE %OBJ_DIR%\System.Console.obj %OBJ_DIR%\HelloWorld.obj msvcrt.lib
