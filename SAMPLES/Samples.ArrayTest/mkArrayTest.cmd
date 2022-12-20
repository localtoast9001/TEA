set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\Samples.ArrayTest.asm /I..\System Samples.ArrayTest.tea
ml /c /Fo%OBJ_DIR%\Samples.ArrayTest.obj %OBJ_DIR%\Samples.ArrayTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Samples.ArrayTest.pdb /out:%BIN_DIR%\Samples.ArrayTest.EXE %OBJ_DIR%\Samples.ArrayTest.obj %BIN_DIR%\System.lib 
