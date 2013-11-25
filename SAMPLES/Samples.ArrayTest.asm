.model flat,C

_ZN6System7Console9WriteLineEAc PROTO C

.data

.code
_ZN7Samples7Program4MainEiAAc PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,208
	xor eax,eax
	push eax
	pop eax
	mov [ebp-208],eax
$Label_0:	nop
	mov eax,99
	push eax
	mov eax,[ebp-208]
	pop ecx
	cmp eax,ecx
	setl al
	test al,al
	jz $Label_1
	mov eax,65
	push eax
	mov eax,[ebp-208]
	pop ecx
	add eax,ecx
	push eax
	mov eax,[ebp-208]
	push eax
	lea ecx,[ebp-204]
	pop eax
	mov ebx,2
	imul eax,ebx
	add ecx,eax
	pop eax
	mov word ptr [ecx],ax
	mov eax,1
	push eax
	mov eax,[ebp-208]
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov [ebp-208],eax
	jmp $Label_0
$Label_1:	nop
	xor eax,eax
	push eax
	mov eax,99
	push eax
	lea ecx,[ebp-204]
	pop eax
	mov ebx,2
	imul eax,ebx
	add ecx,eax
	pop eax
	mov word ptr [ecx],ax
	lea eax,[ebp-204]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	xor eax,eax
	push eax
	pop eax
	mov [ebp-4],eax
	mov eax,[ebp-4]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples7Program4MainEiAAc ENDP
wmain PROC C EXPORT
	push ebp
	mov ebp,esp    
    push [ebp+12]
    push [ebp+8]
    call _ZN7Samples7Program4MainEiAAc
    add esp,8
	mov esp,ebp
	pop ebp
    ret
wmain ENDP
END
