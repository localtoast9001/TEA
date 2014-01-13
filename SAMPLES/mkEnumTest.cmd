set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\EnumTest.asm /I..\System Samples.EnumTest.tea
ml /c /Fo%OBJ_DIR%\EnumTest.obj %OBJ_DIR%\EnumTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\EnumTest.pdb /out:%BIN_DIR%\EnumTest.EXE %OBJ_DIR%\EnumTest.obj %BIN_DIR%\System.lib 
