.model flat,C

.code
_value$=8
_sqrtd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fsqrt
    mov esp,ebp
    pop ebp
    ret    
_sqrtd ENDP
_value$=8
_cosd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fcos
    mov esp,ebp
    pop ebp
    ret    
_cosd ENDP
_value$=8
_sind PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fsin
    mov esp,ebp
    pop ebp
    ret    
_sind ENDP
_value$=8
_tand PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fptan
    mov esp,ebp
    pop ebp
    ret
_tand ENDP
_value$=8
_absd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fabs
    mov esp,ebp
    pop ebp
    ret    
_absd ENDP
_value$=8
_log10d PROC C EXPORT
    push ebp
    mov ebp,esp
    fld1
    fldl2t
    fdivp
    fld qword ptr _value$[ebp]
    fyl2x    
    mov esp,ebp
    pop ebp
    ret    
_log10d ENDP
_value$=8
_log2d PROC C EXPORT
    push ebp
    mov ebp,esp
    fld1
    fld qword ptr _value$[ebp]
    fyl2x    
    mov esp,ebp
    pop ebp
    ret    
_log2d ENDP
_value$=8
_lnd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld1
    fldl2e
    fdivp
    fld qword ptr _value$[ebp]
    fyl2x    
    mov esp,ebp
    pop ebp
    ret    
_lnd ENDP
_value$=8
_sqrd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fst st(1)
    fmulp
    mov esp,ebp
    pop ebp
    ret    
_sqrd ENDP
_value$=8
_power$=16
_expd PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fld st
    fmulp
    mov esp,ebp
    pop ebp
    ret    
_expd ENDP
_value$=8
_roundi$=-4
_roundi PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    frndint
    fistp dword ptr _roundi$[ebp]
    mov eax,_roundi$[ebp]
    mov esp,ebp
    pop ebp
    ret    
_roundi ENDP
_value$=8
_floori$=-4
_floori PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fistp dword ptr _floori$[ebp]
    mov eax,_floori$[ebp]
    mov esp,ebp
    pop ebp
    ret    
_floori ENDP
_value$=8
_ceilingi$=-4
_ceilingi PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fld1
    faddp
    fistp dword ptr _ceilingi$[ebp]
    mov eax,_ceilingi$[ebp]
    mov esp,ebp
    pop ebp
    ret    
_ceilingi ENDP
_pid PROC C EXPORT
    push ebp
    mov ebp,esp
    fldpi
    mov esp,ebp
    pop ebp
    ret
_pid ENDP
END
