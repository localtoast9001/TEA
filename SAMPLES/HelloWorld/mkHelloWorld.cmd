set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\HelloWorld.asm /I..\System HelloWorld.tea
ml /c /Fo%OBJ_DIR%\HelloWorld.obj %OBJ_DIR%\HelloWorld.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\HelloWorld.pdb /out:%BIN_DIR%\HelloWorld.EXE %OBJ_DIR%\HelloWorld.obj %BIN_DIR%\System.lib
