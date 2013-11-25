set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

ml /c /Fo%OBJ_DIR%\Vector3D.obj Vector3D.asm
ml /c /Fo%OBJ_DIR%\System.Math.obj System.Math.asm
ml /c /Fo%OBJ_DIR%\System.Math.impl.obj System.Math.impl.asm
ml /c /Fo%OBJ_DIR%\System.Console.obj System.Console.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Vector3D.pdb /out:%BIN_DIR%\Vector3D.EXE %OBJ_DIR%\Vector3D.obj %OBJ_DIR%\System.Math.obj %OBJ_DIR%\System.Math.impl.obj %OBJ_DIR%\System.Console.obj msvcrt.lib
