.model flat,C

_ZN7Samples1A10destructorEv PROTO C
_ZN7Samples1B10destructorEv PROTO C
_ZN7Samples1B8GetValueEv PROTO C
_ZN7Samples1A11constructorEv PROTO C
_ZN6System7Console9WriteLineEAc PROTO C
_ZN7Samples1C10destructorEv PROTO C
_ZN7Samples1C8GetValueEv PROTO C
_ZN7Samples1B11constructorEi PROTO C
_ZN7Samples1D10destructorEv PROTO C
_ZN7Samples1D8GetValueEv PROTO C
_ZN6System4Math5FloorEd PROTO C
_ZN6System6Memory5AllocEi PROTO C
_ZN7Samples1C11constructorEii PROTO C
_ZN7Samples1D11constructorEid PROTO C
_ZN6System6Memory4FreeEPb PROTO C

.data
$Vtbl__ZN7Samples1A	dd	_ZN7Samples1A10destructorEv

$Vtbl__ZN7Samples1B	dd	_ZN7Samples1B10destructorEv
	dd	_ZN7Samples1B8GetValueEv

$String_0	db	68
	db	0
	db	101
	db	0
	db	115
	db	0
	db	116
	db	0
	db	114
	db	0
	db	111
	db	0
	db	121
	db	0
	db	105
	db	0
	db	110
	db	0
	db	103
	db	0
	db	32
	db	0
	db	66
	db	0
	db	0
	db	0

$String_1	db	66
	db	0
	db	32
	db	0
	db	71
	db	0
	db	101
	db	0
	db	116
	db	0
	db	86
	db	0
	db	97
	db	0
	db	108
	db	0
	db	117
	db	0
	db	101
	db	0
	db	0
	db	0

$Vtbl__ZN7Samples1C	dd	_ZN7Samples1C10destructorEv
	dd	_ZN7Samples1C8GetValueEv

$String_2	db	68
	db	0
	db	101
	db	0
	db	115
	db	0
	db	116
	db	0
	db	114
	db	0
	db	111
	db	0
	db	121
	db	0
	db	105
	db	0
	db	110
	db	0
	db	103
	db	0
	db	32
	db	0
	db	67
	db	0
	db	0
	db	0

$String_3	db	67
	db	0
	db	32
	db	0
	db	71
	db	0
	db	101
	db	0
	db	116
	db	0
	db	86
	db	0
	db	97
	db	0
	db	108
	db	0
	db	117
	db	0
	db	101
	db	0
	db	0
	db	0

$Vtbl__ZN7Samples1D	dd	_ZN7Samples1D10destructorEv
	dd	_ZN7Samples1D8GetValueEv

$String_4	db	68
	db	0
	db	101
	db	0
	db	115
	db	0
	db	116
	db	0
	db	114
	db	0
	db	111
	db	0
	db	121
	db	0
	db	105
	db	0
	db	110
	db	0
	db	103
	db	0
	db	32
	db	0
	db	68
	db	0
	db	0
	db	0

$String_5	db	68
	db	0
	db	32
	db	0
	db	71
	db	0
	db	101
	db	0
	db	116
	db	0
	db	86
	db	0
	db	97
	db	0
	db	108
	db	0
	db	117
	db	0
	db	101
	db	0
	db	0
	db	0

$Double_0	db	0
	db	0
	db	0
	db	0
	db	0
	db	0
	db	22
	db	64


.code
_this$=8
_ZN7Samples1A11constructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	lea eax,[$Vtbl__ZN7Samples1A]
	mov ecx,_this$[ebp]
	mov [ecx],eax
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1A11constructorEv ENDP
_this$=8
_ZN7Samples1A10destructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1A10destructorEv ENDP
_this$=8
_x$=12
_ZN7Samples1B11constructorEi PROC C EXPORT
	push ebp
	mov ebp,esp
	mov ecx,_this$[ebp]
	push ecx
	call _ZN7Samples1A11constructorEv
	add esp,4
	lea eax,[$Vtbl__ZN7Samples1B]
	mov ecx,_this$[ebp]
	mov [ecx],eax
	mov eax,dword ptr _x$[ebp]
	push eax
	mov ecx,_this$[ebp]
	add ecx,4
	pop eax
	mov [ecx],eax
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1B11constructorEi ENDP
_this$=8
_ZN7Samples1B10destructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	lea eax,[offset $String_0]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	mov eax,_this$[ebp]
	push eax
	call _ZN7Samples1A10destructorEv
	add esp,4
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1B10destructorEv ENDP
_GetValue$=-4
_this$=8
_ZN7Samples1B8GetValueEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	lea eax,[offset $String_1]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	mov ecx,_this$[ebp]
	add ecx,4
	mov eax,dword ptr [ecx]
	push eax
	pop eax
	mov _GetValue$[ebp],eax
	mov eax, dword ptr _GetValue$[ebp]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1B8GetValueEv ENDP
_this$=8
_x$=12
_y$=16
_ZN7Samples1C11constructorEii PROC C EXPORT
	push ebp
	mov ebp,esp
	mov eax,dword ptr _x$[ebp]
	push eax
	mov ecx,_this$[ebp]
	push ecx
	call _ZN7Samples1B11constructorEi
	add esp,8
	lea eax,[$Vtbl__ZN7Samples1C]
	mov ecx,_this$[ebp]
	mov [ecx],eax
	mov eax,dword ptr _y$[ebp]
	push eax
	mov ecx,_this$[ebp]
	add ecx,8
	pop eax
	mov [ecx],eax
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1C11constructorEii ENDP
_this$=8
_ZN7Samples1C10destructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	lea eax,[offset $String_2]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	mov eax,_this$[ebp]
	push eax
	call _ZN7Samples1B10destructorEv
	add esp,4
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1C10destructorEv ENDP
_GetValue$=-4
_this$=8
_ZN7Samples1C8GetValueEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	lea eax,[offset $String_3]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	mov ecx,_this$[ebp]
	add ecx,8
	mov eax,dword ptr [ecx]
	push eax
	lea eax,_this$[ebp]
	push eax
	call _ZN7Samples1B8GetValueEv
	add esp,4
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov _GetValue$[ebp],eax
	mov eax, dword ptr _GetValue$[ebp]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1C8GetValueEv ENDP
_this$=8
_x$=12
_t$=16
_ZN7Samples1D11constructorEid PROC C EXPORT
	push ebp
	mov ebp,esp
	mov eax,dword ptr _x$[ebp]
	push eax
	mov ecx,_this$[ebp]
	push ecx
	call _ZN7Samples1B11constructorEi
	add esp,8
	lea eax,[$Vtbl__ZN7Samples1D]
	mov ecx,_this$[ebp]
	mov [ecx],eax
	fld qword ptr _t$[ebp]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,_this$[ebp]
	add ecx,8
	pop eax
	pop edx
	lea ecx,[ecx]
	mov [ecx],eax
	add ecx,4
	mov [ecx],edx
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1D11constructorEid ENDP
_this$=8
_ZN7Samples1D10destructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	lea eax,[offset $String_4]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	mov eax,_this$[ebp]
	push eax
	call _ZN7Samples1B10destructorEv
	add esp,4
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1D10destructorEv ENDP
_GetValue$=-4
_this$=8
_ZN7Samples1D8GetValueEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	lea eax,[offset $String_5]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	mov ecx,_this$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	call _ZN6System4Math5FloorEd
	add esp,8
	push eax
	lea eax,_this$[ebp]
	push eax
	call _ZN7Samples1B8GetValueEv
	add esp,4
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov _GetValue$[ebp],eax
	mov eax, dword ptr _GetValue$[ebp]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples1D8GetValueEv ENDP
_Main$=-4
_argc$=8
_argv$=12
_a$=-24
_b$=-36
_c$=-48
_i$=-52
_$temp0$=-56
_$temp1$=-60
_$temp2$=-64
_ZN7Samples7Program4MainEiAAc PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,64
	mov eax,8
	push eax
	call _ZN6System6Memory5AllocEi
	add esp,4
	mov _$temp0$[ebp],eax
	test eax,eax
	jz $Label_0
	mov eax,1
	push eax
	mov eax,_$temp0$[ebp]
	push eax
	call _ZN7Samples1B11constructorEi
	add esp,8
$Label_0:	mov eax,_$temp0$[ebp]
	push eax
	xor eax,eax
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	pop eax
	mov [ecx],eax
	mov eax,12
	push eax
	call _ZN6System6Memory5AllocEi
	add esp,4
	mov _$temp1$[ebp],eax
	test eax,eax
	jz $Label_1
	mov eax,3
	push eax
	mov eax,2
	push eax
	mov eax,_$temp1$[ebp]
	push eax
	call _ZN7Samples1C11constructorEii
	add esp,12
$Label_1:	mov eax,_$temp1$[ebp]
	push eax
	mov eax,1
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	pop eax
	mov [ecx],eax
	mov eax,16
	push eax
	call _ZN6System6Memory5AllocEi
	add esp,4
	mov _$temp2$[ebp],eax
	test eax,eax
	jz $Label_2
	fld qword ptr [$Double_0]
	sub esp,8
	fstp qword ptr [esp]
	mov eax,4
	push eax
	mov eax,_$temp2$[ebp]
	push eax
	call _ZN7Samples1D11constructorEid
	add esp,16
$Label_2:	mov eax,_$temp2$[ebp]
	push eax
	mov eax,2
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	pop eax
	mov [ecx],eax
	xor eax,eax
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	mov eax,dword ptr [ecx]
	push eax
	xor eax,eax
	push eax
	lea ecx,_a$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	pop eax
	mov [ecx],eax
	mov eax,1
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	mov eax,dword ptr [ecx]
	push eax
	mov eax,1
	push eax
	lea ecx,_a$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	pop eax
	mov [ecx],eax
	mov eax,2
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	mov eax,dword ptr [ecx]
	push eax
	mov eax,2
	push eax
	lea ecx,_a$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	pop eax
	mov [ecx],eax
	xor eax,eax
	push eax
	pop eax
	mov _i$[ebp],eax
$Label_3:	nop
	mov eax,3
	push eax
	mov eax,dword ptr _i$[ebp]
	pop ecx
	cmp eax,ecx
	setl al
	test al,al
	jz $Label_4
	mov eax,dword ptr _i$[ebp]
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	mov ecx,[ecx]
	lea eax,[ecx]
	push eax
	mov ecx,[eax]
	add ecx,4
	call dword ptr [ecx]
	add esp,4
	push eax
	mov eax,dword ptr _i$[ebp]
	push eax
	lea ecx,_c$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	pop eax
	mov [ecx],eax
	mov eax,dword ptr _i$[ebp]
	push eax
	lea ecx,_b$[ebp]
	pop eax
	shl eax,2
	add ecx,eax
	mov eax,dword ptr [ecx]
	push eax
	push eax
	mov ecx,[eax]
	call dword ptr [ecx]
	add esp,4
	call _ZN6System6Memory4FreeEPb
	add esp,4
	mov eax,1
	push eax
	mov eax,dword ptr _i$[ebp]
	pop ecx
	add eax,ecx
	push eax
	pop eax
	mov _i$[ebp],eax
	jmp $Label_3
$Label_4:	nop
	mov eax, dword ptr _Main$[ebp]
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
