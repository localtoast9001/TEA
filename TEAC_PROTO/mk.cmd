set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

ml /c /Fo%OBJ_DIR%\program.obj program.asm
link /SUBSYSTEM:CONSOLE /out:%BIN_DIR%\TEAC.EXE %OBJ_DIR%\program.obj msvcrt.lib
