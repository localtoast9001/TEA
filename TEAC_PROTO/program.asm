; Copyright (C) 2013 Jon Rowlett. All rights reserved
.model flat,C

include list.inc

includelib <msvcrt.lib>

puts PROTO C, string:DWORD

.data
lpText		db "Hello, world.",0

.code
main PROC C EXPORT, __argc:DWORD, _targv:DWORD, _tenviron:DWORD
	push ebp
	mov ebp,esp
	push ebx
	push esi
	push edi
	lea eax,[offset lpText]
	push eax
	call [puts]
	add esp,4
	xor eax,eax
	pop edi
	pop esi
	pop ebx
	mov esp,ebp
	pop ebp
	ret
main ENDP
END
