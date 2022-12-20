set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\Samples.FileStreamTest.asm /I..\System /I..\System\IO Samples.FileStreamTest.tea
ml /c /Fo%OBJ_DIR%\Samples.FileStreamTest.obj %OBJ_DIR%\Samples.FileStreamTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Samples.FileStreamTest.pdb /out:%BIN_DIR%\Samples.FileStreamTest.EXE %OBJ_DIR%\Samples.FileStreamTest.obj %BIN_DIR%\System.lib 
