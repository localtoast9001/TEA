set OBJ_DIR=%~dp0obj
set BIN_DIR=%~dp0..\bin
set TEAC=%~dp0..\TEAC\bin\debug\teac.exe
if not exist %BIN_DIR% (
  mkdir %BIN_DIR%
)

if not exist %OBJ_DIR% (
  mkdir %OBJ_DIR%
)

for /F "" %%i in (sources) do (
  %TEAC% /Fa%OBJ_DIR%\%%~ni.asm /I..\System /I..\System\IO /I..\System\Graphics /I..\System\Graphics\IO /I. /IShapes %%i
  if NOT ERRORLEVEL 1 (
    ml /c /Fo%OBJ_DIR%\%%~ni.obj %OBJ_DIR%\%%~ni.asm
  )
)

if NOT ERRORLEVEL 1 (
    link /SUBSYSTEM:CONSOLE /entry:wmainCRTStartup /DEBUG /PDB:%BIN_DIR%\RT.pdb /out:%BIN_DIR%\RT.EXE %OBJ_DIR%\RayTracer.Program.obj %OBJ_DIR%\RayTracer.Color.obj %OBJ_DIR%\RayTracer.Vector3D.obj %OBJ_DIR%\RayTracer.Shape.obj %OBJ_DIR%\RayTracer.Scene.obj %OBJ_DIR%\RayTracer.Camera.obj %OBJ_DIR%\RayTracer.Ray3D.obj %OBJ_DIR%\RayTracer.ShapeList.obj %OBJ_DIR%\RayTracer.Light.obj %OBJ_DIR%\RayTracer.LightList.obj %OBJ_DIR%\RayTracer.Shapes.Sphere.obj %BIN_DIR%\System.lib
)
