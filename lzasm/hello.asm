option unicode
model flat,stdcall

includelib "win32.lib"
procdesc MessageBoxW :dword,:dword,:dword,:dword

dataseg

lpText		dc "Hello, world!",0
lpExample	dc "Example",0

codeseg

	startupcode
	call MessageBoxW,0,offset lpText,offset lpExample,0
	exitcode
