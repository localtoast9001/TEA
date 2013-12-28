.model flat,C

.code
_value$=8
_loi8 PROC C EXPORT
    push ebp
    mov ebp,esp
    mov eax,dword ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_loi8 ENDP

_value$=12
_hii8 PROC C EXPORT
    push ebp
    mov ebp,esp
    mov eax,dword ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_hii8 ENDP

_value$=8
_loi PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_loi ENDP

_value$=10
_hii PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_hii ENDP

_value$=8
_loi2 PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov al,byte ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_loi2 ENDP

_value$=8
_hii2 PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    shr eax,8
    mov esp,ebp
    pop ebp
    ret
_hii2 ENDP

_value$=8
_tocharacteri PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_tocharacteri ENDP

_value$=8
_tocharacteri2 PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_tocharacteri2 ENDP

_value$=8
_tointegerc PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_tointegerc ENDP

_value$=8
_tointegeri2 PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_tointegeri2 ENDP

_value$=8
_toshortb PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov al,byte ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_toshortb ENDP

_value$=8
_toshortc PROC C EXPORT
    push ebp
    mov ebp,esp
    xor eax,eax
    mov ax,word ptr _value$[ebp]
    mov esp,ebp
    pop ebp
    ret
_toshortc ENDP

_tolongi$=8
_value$=12
_tolongi PROC C EXPORT
    push ebp
    mov ebp,esp
    mov eax,dword ptr _value$[ebp]
    mov ecx,dword ptr _tolongi$[ebp]
    mov dword ptr [ecx],eax
    xor eax,eax
    add ecx,4
    mov dword ptr [ecx],eax
    mov esp,ebp
    pop ebp
    ret
_tolongi ENDP

END
