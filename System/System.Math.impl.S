.code32

.global _sqrtd
.global _cosd
.global _sind
.global _tand
.global _absd
.global _log10d
.global _log2d
.global _lnd
.global _sqrd
.global _expd
.global _roundi
.global _truncatei
.global _pid

.set _value$,8
_sqrtd:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fsqrt
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_cosd:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fcos
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_sind:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fsin
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_tand:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fptan
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_absd:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fabs
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_log10d:
    push %ebp
    mov %esp, %ebp
    fld1
    fldl2t
    fdivp
    fldl _value$(%ebp)
    fyl2x
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_log2d:
    push %ebp
    mov %esp, %ebp
    fld1
    fldl _value$(%ebp)
    fyl2x
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_lnd:
    push %ebp
    mov %esp, %ebp
    fld1
    fldl2e
    fdivp
    fldl _value$(%ebp)
    fyl2x    
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
_sqrd:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fst %st(1)
    fmulp
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
.set _power$,16
_expd:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fld %st
    fmulp
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
.set _roundi$,-4
_roundi:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    frndint
    fisttpl _roundi$(%ebp)
    mov _roundi$(%ebp),%eax
    mov %ebp,%esp
    pop %ebp
    ret

.set _value$,8
.set _truncatei$,-4
_truncatei:
    push %ebp
    mov %esp, %ebp
    fldl _value$(%ebp)
    fisttpl _truncatei$(%ebp)
    mov _truncatei$(%ebp),%eax
    mov %ebp,%esp
    pop %ebp
    ret

_pid:
    push %ebp
    mov %esp, %ebp
    fldpi
    mov %ebp,%esp
    pop %ebp
    ret
