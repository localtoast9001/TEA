.model flat,C

.code
_sqrtd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr [ebp+8]
    fsqrt
    mov esp,ebp
    pop ebp
    ret    
_sqrtd ENDP
_cosd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr [ebp+8]
    fcos
    mov esp,ebp
    pop ebp
    ret    
_cosd ENDP
_sind PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr [ebp+8]
    fsin
    mov esp,ebp
    pop ebp
    ret    
_sind ENDP
END
