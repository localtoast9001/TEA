.model flat,C

_ZN7Samples7Program7GetTextEv PROTO C
_ZN6System7Console9WriteLineEAc PROTO C

.data
$String_0	db	72,0,101,0,108,0,108,0,111,0,44,0,32,0,119,0,111,0,114,0,108,0,100,0,33,0,0,0

.code
_ZN7Samples7Program7GetTextEv PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	lea eax,[offset $String_0]
	mov [ebp-4], eax
	mov eax,[ebp-4]
	mov esp,ebp
	pop ebp
	ret
_ZN7Samples7Program7GetTextEv ENDP
_ZN7Samples7Program4MainEiAAc PROC C EXPORT
	push ebp
	mov ebp,esp
	sub esp,4
	call _ZN7Samples7Program7GetTextEv
	push eax
	call _ZN6System7Console9WriteLineEAc
	add esp,4
	xor eax,eax
	mov [ebp-4], eax
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
