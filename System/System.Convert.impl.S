.code32

.global _loi8
.global _hii8
.global _loi
.global _hii
.global _loi2
.global _hii2
.global _tocharacteri
.global _tocharacteri2
.global _tointegerc
.global _tointegeri2
.global _toshortb
.global _toshortc
.global _tolongi
.global _toreali

.set _value$,8
_loi8:
    push %ebp
    mov %esp, %ebp
    mov _value$(%ebp), %eax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,12
_hii8:
    push %ebp
    mov %esp, %ebp
    mov _value$(%ebp), %eax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_loi:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,10
_hii:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_loi2:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movb _value$(%ebp), %al
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_hii2:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    shr $8,%eax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_tocharacteri:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_tocharacteri2:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_tointegerc:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_tointegeri2:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_toshortb:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movb _value$(%ebp), %al
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_toshortc:
    push %ebp
    mov %esp, %ebp
    xor %eax, %eax
    movw _value$(%ebp), %ax
    mov %ebp, %esp
    pop %ebp
    ret

.set _tolongi$,8
.set _value$,12
_tolongi:
    push %ebp
    mov %esp, %ebp
    mov _value$(%ebp), %eax
    mov _tolongi$(%ebp), %ecx
    mov %eax, (%ecx)
    xor %eax, %eax
    add $4, %ecx
    mov %eax, (%ecx)
    mov %ebp, %esp
    pop %ebp
    ret

.set _value$,8
_toreali:
    push %ebp
    mov %esp, %ebp
    fild _value$(%ebp)
    mov %ebp, %esp
    pop %ebp
    ret
