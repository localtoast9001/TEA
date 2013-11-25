.model flat,C

_ZN7Samples8Vector3D11GetLengthSqEv PROTO C
_ZN6System4Math4SqrtEd PROTO C
_ZN7Samples8Vector3D4SetXEd PROTO C
_ZN7Samples8Vector3D4SetYEd PROTO C
_ZN7Samples8Vector3D4SetZEd PROTO C
_ZN7Samples8Vector3D9GetLengthEv PROTO C
_ZN6System7Console9WriteLineEAc PROTO C

.data
$Double_0	db	0,0,0,0,0,0,0,64
$Double_1	db	0,0,0,0,0,0,8,64
$String_0	db	116,0,114,0,117,0,101,0,0,0
$String_1	db	102,0,97,0,108,0,115,0,101,0,0,0

.code
_ZN7Samples8Vector3D11constructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	fldz
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fldz
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,8
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fldz
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,16
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D11constructorEv ENDP
_ZN7Samples8Vector3D11constructorEddd PROC C EXPORT
	push ebp
	mov ebp,esp
	fld qword ptr [ebp+12]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr [ebp+20]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,8
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr [ebp+28]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,16
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D11constructorEddd ENDP
_ZN7Samples8Vector3D4GetXEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov ecx,[ebp+8]
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,[ebp-8]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr [ebp-8]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D4GetXEv ENDP
_ZN7Samples8Vector3D4GetYEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov ecx,[ebp+8]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,[ebp-8]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr [ebp-8]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D4GetYEv ENDP
_ZN7Samples8Vector3D4GetZEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov ecx,[ebp+8]
	add ecx,16
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,[ebp-8]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr [ebp-8]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D4GetZEv ENDP
_ZN7Samples8Vector3D4SetXEd PROC C EXPORT
	push ebp
	mov ebp,esp
	fld qword ptr [ebp+12]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D4SetXEd ENDP
_ZN7Samples8Vector3D4SetYEd PROC C EXPORT
	push ebp
	mov ebp,esp
	fld qword ptr [ebp+12]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,8
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D4SetYEd ENDP
_ZN7Samples8Vector3D4SetZEd PROC C EXPORT
	push ebp
	mov ebp,esp
	fld qword ptr [ebp+12]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,16
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D4SetZEd ENDP
_ZN7Samples8Vector3D9GetLengthEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov ecx,[ebp+8]
	push ecx
	call _ZN7Samples8Vector3D11GetLengthSqEv
	add esp,4
	sub esp,8
	fstp qword ptr [esp]
	call _ZN6System4Math4SqrtEd
	add esp,8
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,[ebp-8]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr [ebp-8]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D9GetLengthEv ENDP
_ZN7Samples8Vector3D11GetLengthSqEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov ecx,[ebp+8]
	add ecx,16
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,16
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	add ecx,8
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,[ebp+8]
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	fld qword ptr [esp]
	add esp,8
	faddp
	fld qword ptr [esp]
	add esp,8
	faddp
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,[ebp-8]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr [ebp-8]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples8Vector3D11GetLengthSqEv ENDP
_ZN7Samples7Program4MainEiAAc PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,48
	fld1
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[ebp-28]
	push eax
	call _ZN7Samples8Vector3D4SetXEd
	add esp,12
	fld qword ptr [$Double_0]
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[ebp-28]
	push eax
	call _ZN7Samples8Vector3D4SetYEd
	add esp,12
	fld qword ptr [$Double_1]
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[ebp-28]
	push eax
	call _ZN7Samples8Vector3D4SetZEd
	add esp,12
	lea eax,[ebp-28]
	push eax
	call _ZN7Samples8Vector3D11GetLengthSqEv
	add esp,4
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,[ebp-36]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	lea eax,[ebp-28]
	push eax
	call _ZN7Samples8Vector3D9GetLengthEv
	add esp,4
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,[ebp-44]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	mov eax,4
	push eax
	pop eax
	mov [ebp-48],eax
	xor eax,eax
	push eax
	mov eax,[ebp-48]
	pop ecx
	cmp eax,ecx
	setg al
	test al,al
	jz $Label_0
	lea eax,[offset $String_0]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	jmp $Label_1
$Label_0:	nop
	lea eax,[offset $String_1]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
$Label_1:	nop
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
