.global _open
.global _close
.global _read
.global _write
.global _lseek
.global _get_errno
.global _commit
.global _tell

.code32

_open:
    jmp open

_close:
    jmp close

_read:
    jmp read

_write:
    jmp write

_lseek:
    jmp lseek

_commit:
    jmp fsync

.set _fd$,8
_tell:
    push %ebp
    mov %esp, %ebp
    pushl $1
    xor %eax, %eax
    push %eax
    push %eax
    push _fd$(%ebp)
    call lseek
    mov %ebp, %esp
    pop %ebp
    ret

.set _pValue$,8
_get_errno:
    push %ebp
    mov %esp, %ebp
    call __errno_location
    movl %eax, %ecx
    movl (%ecx), %eax
    movl _pValue$(%ebp), %edx
    test %edx, %edx
    jz _err$
    movl %eax, (%edx)
    xor %eax, %eax
    jmp _end$
_err$:
    mov $22, %eax
_end$:
    mov %ebp, %esp
    pop %ebp
    ret
