.code32

.global _int3

_int3:
    push %ebp
    mov %esp, %ebp
    int $3
    mov %ebp, %esp
    pop %ebp
    ret
