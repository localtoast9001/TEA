set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

%TEAC% /Fa%OBJ_DIR%\Samples.ImageWriterTest.asm /I..\System /I..\System\IO /I..\System\Graphics /I..\System\Graphics\IO Samples.ImageWriterTest.tea
ml /c /Fo%OBJ_DIR%\Samples.ImageWriterTest.obj %OBJ_DIR%\Samples.ImageWriterTest.asm
link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\Samples.ImageWriterTest.pdb /out:%BIN_DIR%\Samples.ImageWriterTest.EXE %OBJ_DIR%\Samples.ImageWriterTest.obj %BIN_DIR%\System.lib 
