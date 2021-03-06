set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\Samples.ExtendedMathTest.asm /I..\System /I..\System\ExtendedMath Samples.ExtendedMathTest.tea
ml /c /Fo%OBJ_DIR%\Samples.ExtendedMathTest.obj %OBJ_DIR%\Samples.ExtendedMathTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Samples.ExtendedMathTest.pdb /out:%BIN_DIR%\Samples.ExtendedMathTest.EXE %OBJ_DIR%\Samples.ExtendedMathTest.obj %BIN_DIR%\System.lib %BIN_DIR%\System.ExtendedMath.lib 
