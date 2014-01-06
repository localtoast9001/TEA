.model flat,C

.code
_int3 PROC C EXPORT
    push ebp
    mov ebp,esp
    int 3
    mov esp,ebp
    pop ebp
    ret
_int3 ENDP
END
