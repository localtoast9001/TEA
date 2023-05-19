; Copyright (C) 2013-2023 Jon Rowlett. All rights reserved.
; System.Math.impl.asm - Implementation of math routines for MASM.
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
    fisttp dword ptr _roundi$[ebp]
    mov eax,_roundi$[ebp]
    mov esp,ebp
    pop ebp
    ret
_roundi ENDP

_value$=8
_truncatei$=-4
_truncatei PROC C EXPORT
    push ebp
    mov ebp,esp
    fld qword ptr _value$[ebp]
    fisttp dword ptr _truncatei$[ebp]
    mov eax,_truncatei$[ebp]
    mov esp,ebp
    pop ebp
    ret
_truncatei ENDP

_pid PROC C EXPORT
    push ebp
    mov ebp,esp
    fldpi
    mov esp,ebp
    pop ebp
    ret
_pid ENDP
END
