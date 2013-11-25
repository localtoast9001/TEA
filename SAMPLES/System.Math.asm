.model flat,C

_sqrtd PROTO C
_sind PROTO C
_cosd PROTO C

.data

.code
_ZN6System4Math4SqrtEd PROC C EXPORT
	jmp _sqrtd
_ZN6System4Math4SqrtEd ENDP
_ZN6System4Math3SinEd PROC C EXPORT
	jmp _sind
_ZN6System4Math3SinEd ENDP
_ZN6System4Math3CosEd PROC C EXPORT
	jmp _cosd
_ZN6System4Math3CosEd ENDP
END
