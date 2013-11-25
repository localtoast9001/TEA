.model flat,C

_ZN6System6Memory5AllocEi PROTO C
_ZN6System12StringBuffer11constructorEv PROTO C
_ZN6System12StringBuffer6AddRefEv PROTO C
_ZN6System6Memory4FreeEPb PROTO C
_ZN6System12StringBuffer10destructorEv PROTO C
_ZN6System12StringBuffer7ReleaseEv PROTO C
_ZN6System12StringBuffer6CreateEAc PROTO C
_ZN6System12StringBuffer9GetBufferEv PROTO C

.data
$String_0	db	0,0

.code
_ZN6System12StringBuffer11constructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	add ecx,4
	pop eax
	mov [ecx],eax
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
	mov esp,ebp
	pop ebp
	ret
_ZN6System12StringBuffer11constructorEv ENDP
_ZN6System12StringBuffer6CreateEAc PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,20
	xor eax,eax
	mov [ebp-8],eax
	xor eax,eax
	mov [ebp-12],eax
	xor eax,eax
	mov [ebp-16],eax
	xor eax,eax
	push eax
	pop eax
	mov [ebp-4],eax
	xor eax,eax
	push eax
	mov eax,[ebp+8]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_0
$Label_2:	nop
	xor eax,eax
	push eax
	mov eax,dword ptr[ebp-8]
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov ebx,2
	imul eax,ebx
	add ecx,eax
	mov ax,word ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_3
	mov eax,1
	push eax
	mov eax,dword ptr[ebp-8]
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov [ebp-8],eax
	jmp $Label_2
$Label_3:	nop
	mov eax,1
	push eax
	mov eax,dword ptr[ebp-8]
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov [ebp-8],eax
	mov eax,2
	push eax
	mov eax,dword ptr[ebp-8]
	pop ecx
	imul eax,ecx
	push eax
	call _ZN6System6Memory5AllocEi
	add esp,4
	push eax
	pop eax
	mov [ebp-16],eax
	xor eax,eax
	push eax
	mov eax,[ebp-16]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_4
	mov eax,8
	push eax
	call _ZN6System6Memory5AllocEi
	add esp,4
	mov [ebp-20],eax
	test eax,eax
	jz $Label_5
	push eax
	call _ZN6System12StringBuffer11constructorEv
	add esp,4
$Label_5:	mov eax,[ebp-20]
	push eax
	pop eax
	mov [ebp-4],eax
	xor eax,eax
	push eax
	mov eax,dword ptr[ebp-4]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_6
	mov ecx,[ebp-4]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer6AddRefEv
	add esp,4
	mov eax,[ebp-16]
	push eax
	mov ecx,[ebp-4]
	lea ecx,[ecx]
	add ecx,4
	pop eax
	mov [ecx],eax
$Label_8:	nop
	mov eax,dword ptr[ebp-8]
	push eax
	mov eax,dword ptr[ebp-12]
	pop ecx
	cmp eax,ecx
	setl al
	test al,al
	jz $Label_9
	mov eax,dword ptr[ebp-12]
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov ebx,2
	imul eax,ebx
	add ecx,eax
	mov ax,word ptr[ecx]
	push eax
	mov eax,dword ptr[ebp-12]
	push eax
	mov ecx,[ebp-16]
	pop eax
	mov ebx,2
	imul eax,ebx
	add ecx,eax
	pop eax
	mov word ptr [ecx],ax
	mov eax,1
	push eax
	mov eax,dword ptr[ebp-12]
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov [ebp-12],eax
	jmp $Label_8
$Label_9:	nop
	jmp $Label_7
$Label_6:	nop
	mov eax,[ebp-16]
	push eax
	call _ZN6System6Memory4FreeEPb
	add esp,4
$Label_7:	nop
$Label_4:	nop
	jmp $Label_1
$Label_0:	nop
	xor eax,eax
	push eax
	pop eax
	mov [ebp-4],eax
$Label_1:	nop
	mov eax, dword ptr[ebp-4]
	mov esp,ebp
	pop ebp
	ret
_ZN6System12StringBuffer6CreateEAc ENDP
_ZN6System12StringBuffer6AddRefEv PROC C EXPORT
	push ebp
	mov ebp,esp
	mov eax,1
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	add eax,ecx
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
	mov esp,ebp
	pop ebp
	ret
_ZN6System12StringBuffer6AddRefEv ENDP
_ZN6System12StringBuffer7ReleaseEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	mov eax,1
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	sub eax,ecx
	push eax
	pop eax
	mov [ebp-4],eax
	xor eax,eax
	push eax
	mov eax,dword ptr[ebp-4]
	pop ecx
	cmp eax,ecx
	setle al
	test al,al
	jz $Label_10
	mov eax,dword ptr[ebp+8]
	push eax
	push eax
	call _ZN6System12StringBuffer10destructorEv
	add esp,4
	call _ZN6System6Memory4FreeEPb
	add esp,4
	jmp $Label_11
$Label_10:	nop
	mov eax,dword ptr[ebp-4]
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
$Label_11:	nop
	mov esp,ebp
	pop ebp
	ret
_ZN6System12StringBuffer7ReleaseEv ENDP
_ZN6System12StringBuffer10destructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	add ecx,4
	mov eax,[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_12
	mov ecx,[ebp+8]
	add ecx,4
	mov eax,[ecx]
	push eax
	call _ZN6System6Memory4FreeEPb
	add esp,4
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	add ecx,4
	pop eax
	mov [ecx],eax
$Label_12:	nop
	mov esp,ebp
	pop ebp
	ret
_ZN6System12StringBuffer10destructorEv ENDP
_ZN6System12StringBuffer9GetBufferEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	mov ecx,[ebp+8]
	add ecx,4
	mov eax,[ecx]
	push eax
	pop eax
	mov [ebp-4],eax
	mov eax, dword ptr[ebp-4]
	mov esp,ebp
	pop ebp
	ret
_ZN6System12StringBuffer9GetBufferEv ENDP
_ZN6System6String11constructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
	mov esp,ebp
	pop ebp
	ret
_ZN6System6String11constructorEv ENDP
_ZN6System6String11constructorEP_ZN6System6String PROC C EXPORT
	push ebp
	mov ebp,esp
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
	xor eax,eax
	push eax
	mov eax,dword ptr[ebp+12]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_13
	mov ecx,[ebp+12]
	lea ecx,[ecx]
	mov eax,dword ptr[ecx]
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_14
	mov ecx,[ebp+8]
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer6AddRefEv
	add esp,4
$Label_14:	nop
$Label_13:	nop
	mov esp,ebp
	pop ebp
	ret
_ZN6System6String11constructorEP_ZN6System6String ENDP
_ZN6System6String10destructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_15
	mov ecx,[ebp+8]
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer7ReleaseEv
	add esp,4
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
$Label_15:	nop
	mov esp,ebp
	pop ebp
	ret
_ZN6System6String10destructorEv ENDP
_ZN6System6String6assignEP_ZN6System6String PROC C EXPORT
	push ebp
	mov ebp,esp
	xor eax,eax
	push eax
	mov eax,dword ptr[ebp+12]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_16
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_17
	mov ecx,[ebp+8]
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer7ReleaseEv
	add esp,4
$Label_17:	nop
	mov ecx,[ebp+12]
	lea ecx,[ecx]
	mov eax,dword ptr[ecx]
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_18
	mov ecx,[ebp+8]
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer6AddRefEv
	add esp,4
$Label_18:	nop
$Label_16:	nop
	mov esp,ebp
	pop ebp
	ret
_ZN6System6String6assignEP_ZN6System6String ENDP
_ZN6System6String9TryAssignEAc PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov eax,[ebp+12]
	push eax
	call _ZN6System12StringBuffer6CreateEAc
	add esp,4
	push eax
	pop eax
	mov [ebp-8],eax
	xor eax,eax
	push eax
	mov eax,dword ptr[ebp-8]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_19
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_21
	mov ecx,[ebp+8]
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer7ReleaseEv
	add esp,4
$Label_21:	nop
	mov eax,dword ptr[ebp-8]
	push eax
	mov ecx,[ebp+8]
	pop eax
	mov [ecx],eax
	mov eax,1
	push eax
	pop eax
	mov byte ptr [ebp-1],al
	jmp $Label_20
$Label_19:	nop
	xor eax,eax
	push eax
	pop eax
	mov byte ptr [ebp-1],al
$Label_20:	nop
	mov al, byte ptr[ebp-1]
	mov esp,ebp
	pop ebp
	ret
_ZN6System6String9TryAssignEAc ENDP
_ZN6System6String6LengthEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	xor eax,eax
	push eax
	pop eax
	mov [ebp-4],eax
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_22
	mov ecx,[ebp+8]
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer9GetBufferEv
	add esp,4
	push eax
	pop eax
	mov [ebp-8],eax
$Label_23:	nop
	xor eax,eax
	push eax
	mov eax,dword ptr[ebp-4]
	push eax
	mov ecx,[ebp-8]
	pop eax
	mov ebx,2
	imul eax,ebx
	add ecx,eax
	mov ax,word ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_24
	mov eax,1
	push eax
	mov eax,dword ptr[ebp-4]
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov [ebp-4],eax
	jmp $Label_23
$Label_24:	nop
$Label_22:	nop
	mov eax, dword ptr[ebp-4]
	mov esp,ebp
	pop ebp
	ret
_ZN6System6String6LengthEv ENDP
_ZN6System6String10CharactersEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	lea eax,[offset $String_0]
	push eax
	pop eax
	mov [ebp-4],eax
	xor eax,eax
	push eax
	mov ecx,[ebp+8]
	mov eax,dword ptr[ecx]
	pop ecx
	cmp eax,ecx
	setne al
	test al,al
	jz $Label_25
	mov ecx,[ebp+8]
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	call _ZN6System12StringBuffer9GetBufferEv
	add esp,4
	push eax
	pop eax
	mov [ebp-4],eax
$Label_25:	nop
	mov eax, dword ptr[ebp-4]
	mov esp,ebp
	pop ebp
	ret
_ZN6System6String10CharactersEv ENDP
END
