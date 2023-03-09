.model flat,C

_ZN6System4Text8Encoding10destructorEv PROTO C

.data
$Vtbl__ZN6System4Text8Encoding	dd	_ZN6System4Text8Encoding10destructorEv
	dd	0
	dd	0


.code
_this$=8
_ZN6System4Text8Encoding11constructorEv PROC C EXPORT
	push ebp
	mov ebp, esp
	lea eax, dword ptr [$Vtbl__ZN6System4Text8Encoding]
	mov ecx, dword ptr _this$[ebp]
	mov dword ptr [ecx], eax
	mov esp, ebp
	pop ebp
	ret
_ZN6System4Text8Encoding11constructorEv ENDP
_this$=8
_ZN6System4Text8Encoding10destructorEv PROC C EXPORT
	push ebp
	mov ebp, esp
	mov esp, ebp
	pop ebp
	ret
_ZN6System4Text8Encoding10destructorEv ENDP
END
