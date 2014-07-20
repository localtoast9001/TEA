set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\Debug\TEAC.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

for /F "" %%i in (sources) do (
  %TEAC% /Fa%OBJ_DIR%\%%~ni.asm /I. /IIO /IGraphics /IText %%i
  if NOT ERRORLEVEL 1 (
    ml /c /Fo%OBJ_DIR%\%%~ni.obj %OBJ_DIR%\%%~ni.asm
  )
)

ml /c /Fo%OBJ_DIR%\System.Math.impl.obj System.Math.impl.asm
ml /c /Fo%OBJ_DIR%\System.Convert.impl.obj System.Convert.impl.asm
lib /SUBSYSTEM:CONSOLE /out:%BIN_DIR%\System.lib %OBJ_DIR%\System.Math.obj %OBJ_DIR%\System.Math.impl.obj %OBJ_DIR%\System.Console.obj %OBJ_DIR%\System.Convert.obj %OBJ_DIR%\System.Convert.impl.obj %OBJ_DIR%\System.String.obj %OBJ_DIR%\System.Memory.obj %OBJ_DIR%\System.IO.MemoryStream.obj %OBJ_DIR%\System.IO.FileStream.obj %OBJ_DIR%\System.IO.Stream.obj %OBJ_DIR%\System.Graphics.Point.obj %OBJ_DIR%\System.Graphics.Size.obj %OBJ_DIR%\System.Graphics.Image.obj %OBJ_DIR%\System.Graphics.PixelFormat.obj %OBJ_DIR%\System.Graphics.IO.ImageWriter.obj %OBJ_DIR%\System.Graphics.IO.TargaImageWriter.obj %OBJ_DIR%\System.Graphics.IO.WindowsBitmapImageWriter.obj %OBJ_DIR%\System.IO.TextReader.obj %OBJ_DIR%\System.IO.TextWriter.obj %OBJ_DIR%\System.IO.StreamReader.obj %OBJ_DIR%\System.IO.StreamWriter.obj %OBJ_DIR%\System.Text.SimpleStringBuilder.obj %OBJ_DIR%\System.Text.DefaultFormatter.obj %OBJ_DIR%\System.Text.ASCIIEncoding.obj %OBJ_DIR%\System.Text.CharacterUtility.obj %OBJ_DIR%\System.Text.Encoding.obj msvcrt.lib
