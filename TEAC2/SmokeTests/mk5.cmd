..\..\bin\TEAC.exe /FaTest5.asm Test5.tea
ml /c /Cp /Cx /Zi /Fotest5.obj Test5.asm
ml /c /Cp /Cx /Zi /Fotest1Main.obj Test1Main.asm
link /SUBSYSTEM:CONSOLE /DEBUG /OUT:Test5.exe /PDB:Test5.pdb Test5.obj Test1Main.obj msvcrt.lib
