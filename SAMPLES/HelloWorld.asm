.model flat,C

_ZN6System7Console9WriteLineEAc PROTO C

.data
$String_0	db	72
	db	0
	db	101
	db	0
	db	108
	db	0
	db	108
	db	0
	db	111
	db	0
	db	32
	db	0
	db	87
	db	0
	db	111
	db	0
	db	114
	db	0
	db	108
	db	0
	db	100
	db	0
	db	33
	db	0
	db	0
	db	0


.code
_Main$=-4
_argc$=8
_argv$=12
_ZN7Samples7Program4MainEiAAc PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	lea eax,[offset $String_0]
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	xor eax,eax
	push eax
	pop eax
	mov _Main$[ebp],eax
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
