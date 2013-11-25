set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\StringTest.asm /I..\System StringTest.tea
ml /c /Fo%OBJ_DIR%\StringTest.obj %OBJ_DIR%\StringTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Samples.StringTest.pdb /out:%BIN_DIR%\Samples.StringTest.EXE %OBJ_DIR%\StringTest.obj %BIN_DIR%\System.lib
