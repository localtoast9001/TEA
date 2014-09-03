..\..\bin\TEAC.exe /FaTest1.asm Test1.tea
ml /c /Cp /Cx /Zi /Fotest1.obj Test1.asm
ml /c /Cp /Cx /Zi /Fotest1Main.obj Test1Main.asm
link /SUBSYSTEM:CONSOLE /DEBUG /OUT:Test1.exe /PDB:Test1.pdb Test1.obj Test1Main.obj msvcrt.lib
