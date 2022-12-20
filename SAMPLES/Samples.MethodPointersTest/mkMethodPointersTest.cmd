set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\MethodPointersTest.asm /I..\System Samples.MethodPointersTest.tea
ml /c /Fo%OBJ_DIR%\MethodPointersTest.obj %OBJ_DIR%\MethodPointersTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\MethodPointersTest.pdb /out:%BIN_DIR%\MethodPointersTest.EXE %OBJ_DIR%\MethodPointersTest.obj %BIN_DIR%\System.lib 
