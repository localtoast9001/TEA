.model flat,C


.data

.code
_this$=8
_ZN6System12ExtendedMath7Complex11constructorEv PROC C EXPORT
	push ebp
	mov ebp,esp
	fldz
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,_this$[ebp]
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fldz
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,_this$[ebp]
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
_ZN6System12ExtendedMath7Complex11constructorEv ENDP
_this$=8
_r$=12
_i$=20
_ZN6System12ExtendedMath7Complex11constructorEdd PROC C EXPORT
	push ebp
	mov ebp,esp
	fld qword ptr _r$[ebp]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,_this$[ebp]
	mov esi,esp
	lea edi,[ecx]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr _i$[ebp]
	sub esp,8
	fstp qword ptr [esp]
	mov ecx,_this$[ebp]
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
_ZN6System12ExtendedMath7Complex11constructorEdd ENDP
_Real$=-8
_this$=8
_ZN6System12ExtendedMath7Complex4RealEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov ecx,_this$[ebp]
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,_Real$[ebp]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr _Real$[ebp]
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex4RealEv ENDP
_Imaginary$=-8
_this$=8
_ZN6System12ExtendedMath7Complex9ImaginaryEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,8
	mov ecx,_this$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	mov esi,esp
	lea edi,_Imaginary$[ebp]
	mov ecx,8
	cld
	rep movsb
	add esp,8
	fld qword ptr _Imaginary$[ebp]
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex9ImaginaryEv ENDP
_add$=-16
_a$=8
_b$=24
_ZN6System12ExtendedMath7Complex3addE_ZN6System12ExtendedMath7Complex_ZN6System12ExtendedMath7Complex PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,16
	sub esp,16
	lea ecx,_b$[ebp]
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	faddp
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_b$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	faddp
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[esp+16]
	push eax
	call _ZN6System12ExtendedMath7Complex11constructorEdd
	add esp,20
	mov esi,esp
	lea edi,_add$[ebp]
	mov ecx,16
	cld
	rep movsb
	add esp,16
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex3addE_ZN6System12ExtendedMath7Complex_ZN6System12ExtendedMath7Complex ENDP
_subtract$=-16
_a$=8
_b$=24
_ZN6System12ExtendedMath7Complex8subtractE_ZN6System12ExtendedMath7Complex_ZN6System12ExtendedMath7Complex PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,16
	sub esp,16
	lea ecx,_b$[ebp]
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fsubrp
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_b$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fsubrp
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[esp+16]
	push eax
	call _ZN6System12ExtendedMath7Complex11constructorEdd
	add esp,20
	mov esi,esp
	lea edi,_subtract$[ebp]
	mov ecx,16
	cld
	rep movsb
	add esp,16
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex8subtractE_ZN6System12ExtendedMath7Complex_ZN6System12ExtendedMath7Complex ENDP
_multiply$=-16
_a$=8
_b$=24
_ZN6System12ExtendedMath7Complex8multiplyE_ZN6System12ExtendedMath7Complexd PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,16
	sub esp,16
	fld qword ptr _b$[ebp]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	sub esp,8
	fstp qword ptr [esp]
	fld qword ptr _b$[ebp]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[esp+16]
	push eax
	call _ZN6System12ExtendedMath7Complex11constructorEdd
	add esp,20
	mov esi,esp
	lea edi,_multiply$[ebp]
	mov ecx,16
	cld
	rep movsb
	add esp,16
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex8multiplyE_ZN6System12ExtendedMath7Complexd ENDP
_multiply$=-16
_a$=8
_b$=24
_ZN6System12ExtendedMath7Complex8multiplyE_ZN6System12ExtendedMath7Complex_ZN6System12ExtendedMath7Complex PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,16
	sub esp,16
	lea ecx,_b$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_b$[ebp]
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	fld qword ptr [esp]
	add esp,8
	fsubrp
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_b$[ebp]
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_b$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fmulp
	fld qword ptr [esp]
	add esp,8
	faddp
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[esp+16]
	push eax
	call _ZN6System12ExtendedMath7Complex11constructorEdd
	add esp,20
	mov esi,esp
	lea edi,_multiply$[ebp]
	mov ecx,16
	cld
	rep movsb
	add esp,16
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex8multiplyE_ZN6System12ExtendedMath7Complex_ZN6System12ExtendedMath7Complex ENDP
_divide$=-16
_a$=8
_b$=24
_ZN6System12ExtendedMath7Complex6divideE_ZN6System12ExtendedMath7Complexd PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,16
	sub esp,16
	fld qword ptr _b$[ebp]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fdivrp
	sub esp,8
	fstp qword ptr [esp]
	fld qword ptr _b$[ebp]
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	fld qword ptr [esp]
	add esp,8
	fdivrp
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[esp+16]
	push eax
	call _ZN6System12ExtendedMath7Complex11constructorEdd
	add esp,20
	mov esi,esp
	lea edi,_divide$[ebp]
	mov ecx,16
	cld
	rep movsb
	add esp,16
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex6divideE_ZN6System12ExtendedMath7Complexd ENDP
_negative$=-16
_a$=8
_ZN6System12ExtendedMath7Complex8negativeE_ZN6System12ExtendedMath7Complex PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,16
	sub esp,16
	lea ecx,_a$[ebp]
	fld qword ptr [ecx]
	fldz
	fsubp
	sub esp,8
	fstp qword ptr [esp]
	lea ecx,_a$[ebp]
	add ecx,8
	fld qword ptr [ecx]
	fldz
	fsubp
	sub esp,8
	fstp qword ptr [esp]
	lea eax,[esp+16]
	push eax
	call _ZN6System12ExtendedMath7Complex11constructorEdd
	add esp,20
	mov esi,esp
	lea edi,_negative$[ebp]
	mov ecx,16
	cld
	rep movsb
	add esp,16
	mov esp,ebp
	pop ebp
	ret
_ZN6System12ExtendedMath7Complex8negativeE_ZN6System12ExtendedMath7Complex ENDP
END
