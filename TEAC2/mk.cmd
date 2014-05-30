set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

del /q %OBJ_DIR%\*.obj

for /F "" %%i in (sources) do (
  %TEAC% /Fa%OBJ_DIR%\%%~ni.asm /I..\System /I..\System\IO /I..\System\Text /I. %%i
  if NOT ERRORLEVEL 1 (
    ml /c /Fo%OBJ_DIR%\%%~ni.obj %OBJ_DIR%\%%~ni.asm
  )
)

if NOT ERRORLEVEL 1 (
    link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\TEAC2.pdb ^
    /out:%BIN_DIR%\TEAC2.EXE ^
    %OBJ_DIR%\TEAC.Arguments.obj ^
    %OBJ_DIR%\TEAC.CodeGenerator.obj ^
    %OBJ_DIR%\TEAC.CompilerContext.obj ^
    %OBJ_DIR%\TEAC.Expression.obj ^
    %OBJ_DIR%\TEAC.LinkedListNodeOfMessage.obj ^
    %OBJ_DIR%\TEAC.LinkedListOfMethodDefinition.obj ^
    %OBJ_DIR%\TEAC.LinkedListOfString.obj ^
    %OBJ_DIR%\TEAC.LinkedListOfTypeDeclaration.obj ^
    %OBJ_DIR%\TEAC.Message.obj ^
    %OBJ_DIR%\TEAC.MessageLog.obj ^
    %OBJ_DIR%\TEAC.MessageLogEnumerator.obj ^
    %OBJ_DIR%\TEAC.ParseNode.obj ^
    %OBJ_DIR%\TEAC.Parser.obj ^
    %OBJ_DIR%\TEAC.Program.obj ^
    %OBJ_DIR%\TEAC.ProgramUnit.obj ^
    %OBJ_DIR%\TEAC.Statement.obj ^
    %OBJ_DIR%\TEAC.Token.obj ^
    %OBJ_DIR%\TEAC.TokenReader.obj ^
    %OBJ_DIR%\TEAC.TypeDeclaration.obj ^
    %BIN_DIR%\System.lib
)
