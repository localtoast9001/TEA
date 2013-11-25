.model flat,C

_ZN6System7Console5WriteEAc PROTO C
_putws PROTO C

.data
$String_0	db	13,0,10,0,0,0

.code
_ZN6System7Console5WriteEAc PROC C EXPORT
	jmp _putws
_ZN6System7Console5WriteEAc ENDP
_ZN6System7Console9WriteLineEAc PROC C EXPORT
	push ebp
	mov ebp,esp
	mov eax,[ebp+8]
	push eax
	call _ZN6System7Console5WriteEAc
	add esp,4
	lea eax,[offset $String_0]
	push eax
	call _ZN6System7Console5WriteEAc
	add esp,4
	mov esp,ebp
	pop ebp
	ret
_ZN6System7Console9WriteLineEAc ENDP
END
