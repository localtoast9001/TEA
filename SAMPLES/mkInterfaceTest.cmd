set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\InterfaceTest.asm /I..\System Samples.InterfaceTest.tea
ml /c /Fo%OBJ_DIR%\InterfaceTest.obj %OBJ_DIR%\InterfaceTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\InterfaceTest.pdb /out:%BIN_DIR%\InterfaceTest.EXE %OBJ_DIR%\InterfaceTest.obj %BIN_DIR%\System.lib 
