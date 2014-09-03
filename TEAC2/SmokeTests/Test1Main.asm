.model flat, syscall

?Main@Program@SmokeTests@@SAHHQAQAG@Z PROTO 

.code

_argc$=8    ; integer
_argv$=12    ; #0#0character
wmain PROC C EXPORT
        push ebp
        mov ebp,esp
        push _argv$[ebp]
        push _argc$[ebp]
        call ?Main@Program@SmokeTests@@SAHHQAQAG@Z
        mov esp,ebp
        pop ebp
        ret
wmain ENDP

END
