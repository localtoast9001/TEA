# Copyright (C) 2013-2023 Jon Rowlett. All rights reserved.
# System.Diagnostics.Debug.impl.S - Implementation of debug routines for GAS.
.code32

.global _int3

_int3:
    push %ebp
    mov %esp, %ebp
    int $3
    mov %ebp, %esp
    pop %ebp
    ret
