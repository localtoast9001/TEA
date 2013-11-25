; Copyright (C) 2013 Jon Rowlett. All rights reserved
.model flat,C

include list.inc

includelib <msvcrt.lib>

malloc PROTO C, size:DWORD
free PROTO C, p:DWORD

.code
List_Append PROC pList : DWORD, data : DWORD
	push ebp
	mov ebp,esp
	sub esp,4
	invoke malloc,8
	mov eax,[ebp-4]
	mov edx,[ebp+8]
	mov [eax+4],edx
	mov esp,ebp
	pop ebp
	ret 8	
ENDP
END
