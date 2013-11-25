set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\Vector3D.asm /I..\System Vector3D.tea
ml /c /Fo%OBJ_DIR%\Vector3D.obj %OBJ_DIR%\Vector3D.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Vector3D.pdb /out:%BIN_DIR%\Vector3D.EXE %OBJ_DIR%\Vector3D.obj %BIN_DIR%\System.lib 
