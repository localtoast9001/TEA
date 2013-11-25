.model flat,C

malloc PROTO C
free PROTO C

.data

.code
_ZN6System6Memory5AllocEi PROC C EXPORT
	jmp malloc
_ZN6System6Memory5AllocEi ENDP
_ZN6System6Memory4FreeEPb PROC C EXPORT
	jmp free
_ZN6System6Memory4FreeEPb ENDP
END
