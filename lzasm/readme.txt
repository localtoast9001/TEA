Lazy Assembler  Version 0.56
Homepage: http://lzasm.hotbox.ru
Download: http://lzasm.hotbox.ru/lzasm.zip
E-mail:   mailto:lzasm@hotbox.ru

BUGS FIXED
----------
- fixed 3DNow! instructions

NEW
---

-==========================-
 Differents from IDEAL mode
-==========================-

- don't support directives:
	p8086, p186, p286, p286n, no87, p8087, p287, p387, p487, p587, p687,
	masm51, nomasm51, quirks, smart, nosmart, version, name,
	display, emul, noemul, even, evendata, nolocals
	option {expr16|expr32}
	option casemap:{none|notpublic|all}
	option {dotname|nodotname}
	option {emulator|noemulator}
	option {ljmp|noljmp}
	option {scoped|noscoped}

- don't support predefined symbols:
	??date, ??time, ??version

- don't support language modifier:
	normal, windows, oddnear, oddfar

- don't support models:
	tchuge, tpascal

- support SSE, SSE2, SSE3 (PNI), SSE4, 3DNow!Pro instructions

- support directives:
	comment, .if, .break, .continue, .elseif, .else, .endif, .repeat,
	.until, .untilcxz, .while, .endw, import, includebin, echo
	option {ansi|unicode}
	option procalign:{1|2|4|8|16}
	option stackframe:{small|fast}
	oword, do, tchar, dc

- support predefined symbols:
        @date, @time, @unicode, @line

- support synonyms:
	repeat, for, forc, struct

- other features
	support hexadecimal constants define 0xA23
	support binary constants define 10101y
	lea optimization:
		lea eax,[2*edx] 	-> lea eax,[edx+edx]
		lea eax,[1+5*(edx+1)]	-> lea eax,[6+4*edx+edx]
	all symbols case sensitive
	floating point register reference st0,..,st7 or st(0),..,st(7)
	MMX register reference mm0,..,mm7 or mm(0),..,mm(7)
	SSE/SSE2/SSE3/SSE4 register reference xmm0,..,xmm7 or xmm(0),..,xmm(7)
	floating point constant as 32-bits immediate:
		mov eax,1.0
		mov [Two],2.0
		push 3.0
	support labels @@,@f,@b
	support 2^32 labels for conditional directives
	.if, .repeat, .while, ... directives generate @@xxxx labels instead @Cxxxx
	reductive record syntax (setting 1 bit width field is optinal)
	END directive is optional
	support @lzasm predefined symbol, define version of LZASM
	control of stack alignment with STACKALIGN, STACKUNALIGN directives

-==================-
 Directive keywords
-==================-

%		%bin		%conds		%cref		%crefall
%crefref	%crefuref	%ctls		%depth		%incl
%linum		%list		%listall	%listmacro	%listmacroall
%macs		%newpage	%noconds	%nocref		%noctls
%noincl		%nolist		%nolistmacro	%nomacs		%nosyms
%notrunc	%pagesize	%pcnt		%poplctl	%pushlctl
%subttl		%syms		%tabsize	%text		%title
%trunc		.break		.continue	.else		.elseif		
.endif		.endw           .if		.repeat		.until		
.untilcxz	.while          ?debug		alias		align		
arg		assume          byte		catstr		clrflag
codeseg		comm		comment         const		dataseg
db		dc		dd              df		do
dosseg		dp		dq              dt		dw
dword		echo		else            elseif		elseif1
elseif2		elseifb		elseifdef       elseifdif	elseifdifi
elseife		elseifidn	elseifidni      elseifnb	elseifndef
end		endif		endm            endp		ends
enum		equ		err		errif           errif1
errif2		errifb		errifdef	errifdif        errifdifi
errife		errifidn	errifidni	errifnb         errifndef
exitcode	exitm		export		extrn           extern
fardata		fastimul	flipflag	for             forc
fword		getfield	global		goto            group
ideal		if		if1		if2             ifb
ifdef		ifdif		ifdifi		ife             ifidn
ifidni		ifnb		ifndef		import          include
includebin	includelib	instr		irp		irpc
jumps		label		largestack	local		locals
macro		maskflag	model		multerrs	nojumps
nomulterrs	nowarn		option		org		oword
p386		p386p		p386n		p486		p486n
p586		p586n		p686		p686n		pmmx
popstate	proc		procdesc	public		proctype
publicdll	purge		pushstate	pword		qword
radix		real10		real4		real8		record
repeat		rept		retcode		sbyte		sdword
segment		setfield	setflag		sizestr		smallstack
stack		stackalign	stackunalign	startupcode	struc
struct		substr		sword		table           textequ
tblinit		tblinst		tblptr		tbyte		tchar
testflag	typedef		udataseg	ufardata	union
uses		warn		while		word

-========================-
 CPU instruction keywords
-========================-
- 386 Instructions
-
aaa		aad		aam		aas		adc
add		and		arpl		bound		bsf
bsr		bt		btc		btr		bts
call		cbw		cdq		clc		cld
cli		clts		cmc		cmp		cmps
cmpsb		cmpsd		cmpsw		cwd		cwde
daa		das		dec		div		enter
enterd		enterw		hlt		idiv            imul
in		inc		ins		insb            insd
insw		int		into		iret            iretd
iretw		ja		jae		jb              jbe
jc		jcxz		je		jecxz           jg
jge		jl		jle		jmp             jna
jnae		jnb		jnbe		jnc             jne
jng		jnge		jnl		jnle            jno
jnp		jns		jnz		jo              jp
jpe		jpo		js		jz              lahf
lar		lds		lea		leave           leaved
leavew		les		lfs		lgdt            lgs
lidt		lldt		lmsw		lock            lods
lodsb		lodsd		lodsw		loop            loopd
loopde		loopdne		loopdnz		loopdz          loope
looped		loopew		loopne		loopned         loopnew
loopnz		loopnzd		loopnzw		loopw           loopwe
loopwne		loopwnz		loopwz		loopz           loopzd
loopzw		lsl		lss		ltr             mov
movs		movsb		movsd		movsw           movsx
movzx		mul		neg		nop             not
or		out		outs		outsb           outsd
outsw		pop		popa		popad           popaw
popf		popfd		popfw		push            pusha
pushad		pushaw		pushd		pushf           pushfd
pushfw		pushw		rcl		rcr             rep
repe		repne		repnz		repz            ret
retf		retn		rol		ror             sahf
sal		sar		sbb		scas            scasb
scasd		scasw		segcs		segds           seges
segfs		seggs		segss		seta            setae
setalc		setb		setbe		setc            sete
setg		setge		setl		setle           setna
setnae		setnb		setnbe		setnc           setne
setng		setnge		setnl		setnle          setno
setnp		setns		setnz		seto            setp
setpe		setpo		sets		setz            sgdt
shl		shld		shr		shrd            sidt
sldt		smsw		stc		std             sti
stos		stosb		stosd		stosw           str
sub		test		verr		verw            wait
xchg		xlat		xlatb		xor

- 486 Instructions
-
bswap		cmpxchg		invd		invlpg		smi
wbinvd		xadd

- 586 Instructions
-
cmpxchg8b	cpuid		rdmsr		rdtsc		wrmsr

- 686 Instructions
-
cmova		cmovae		cmovb		cmovbe		cmovc
cmove		cmovg		cmovge		cmovl		cmovle
cmovna		cmovnae		cmovnb		cmovnbe		cmovnc
cmovne		cmovng		cmovnge		cmovnl		cmovnle
cmovno		cmovnp		cmovns		cmovnz		cmovo
cmovp		cmovpe		cmovpo		cmovs		cmovz		
rdpmc		rsm		syscall		sysenter	sysexit
sysret		ud		ud2

- FPU Instructions
-
esc		f2xm1		fabs		fadd		faddp
fbld		fbstp		fchs		fclex		fcom
fcomp		fcompp		fcos		fdecstp		fdisi
fdiv		fdivp		fdivr		fdivrp		feni
ffree		ffreep		fiadd		ficom		ficomp
fidiv		fidivr		fild		fimul		fincstp
finit		fist		fistp		fisub		fisubr
fld		fld1		fldcw		fldenv		fldenvd
fldenvw		fldl2e		fldl2t		fldlg2		fldln2
fldpi		fldz		fmul		fmulp		fnclex
fndisi		fneni		fninit		fnldenv		fnop
fnrstor		fnsave		fnsaved		fnsavew		fnstcw
fnstenv		fnstenvd	fnstenvw	fnstsw		fpatan
fprem		fprem1		fptan		frndint		frstor
frstord		frstorw		fsave		fsaved		fsavew
fscale		fsetpm		fsin		fsincos		fsqrt
fst		fstcw		fstenv		fstenvd		fstenvw
fstp		fstsw		fsub		fsubp		fsubr
fsubrp		ftst		fucom		fucomp		fucompp
fwait		fxam		fxch		fxtract		fyl2x
fyl2xp1

- 686 FPU Instructions
-
fcmovb		fcmovbe		fcmove		fcmovnb		fcmovnbe
fcmovne		fcmovnu		fcmovu		fcomi		fcomip
fucomi		fucomip

- MMX Instructions
-
emms		movd		movq		packssdw	packsswb
packuswb	paddb		paddd		paddsb		paddsw
paddusb		paddusw		paddw		pand		pandn
pcmpeqb		pcmpeqd		pcmpeqw		pcmpgtb		pcmpgtd
pcmpgtw		pmaddwd		pmulhw		pmullw		por
pslld		psllq		psllw		psrad		psraw
psrld		psrlq		psrlw		psubb		psubd
psubsb		psubsw		psubusb		psubusw		psubw
punpckhbw	punpckhdq	punpckhwd	punpcklbw	punpckldq
punpcklwd	pxor

- MMX Extension Instructions (SSE)
-
maskmovq	movntq		pavgb		pavgw		pextrw
pinsrw		pmaxsw		pmaxub		pminsw		pminub
pmovmskb	pmulhuw		prefetchnta	prefetcht0	prefetcht1
prefetcht2	psadbw		pshufw		sfence

- SSE Instructions (3DNow! Pro)
-
addps		addss		andnps		andps		cmpeqps
cmpeqss		cmpleps		cmpless		cmpltps		cmpltss
cmpneqps	cmpneqss	cmpnleps	cmpnless	cmpnltps
cmpnltss	cmpordps	cmpordss	cmpps		cmpss
cmpuordps	cmpuordss	comiss		cvtpi2ps	cvtps2pi
cvtsi2ss	cvtss2si	cvttps2pi	cvttss2si	divps
divss		fxrstor		fxsave		ldmxcsr		maxps
maxss		minps		minss		movaps		movhlps
movhps		movlhps		movlps		movmskps	movntps
movss		movups		mulps		mulss		orps
rcpps		rcpss		rsqrtps		rsqrtss		shufps
sqrtps		sqrtss		stmxcsr		subps		subss
ucomiss		unpckhps	unpcklps	xorps

- SSE2 Instructions
-
addpd		addsd		andnpd		andpd		clflush
cmpeqpd		cmpeqsd		cmplepd		cmplesd		cmpltpd
cmpltsd		cmpneqpd	cmpneqsd	cmpnlepd	cmpnlesd
cmpnltpd	cmpnltsd	cmpordpd	cmpordsd	cmppd
cmpsd		cmpuordpd	cmpuordsd	comisd		cvtdq2pd
cvtdq2ps	cvtpd2dq	cvtpd2pi	cvtpd2ps	cvtpi2pd
cvtps2dq	cvtps2pd	cvtsd2si	cvtsd2ss	cvtsi2sd
cvtss2sd	cvttpd2dq	cvttpd2pi	cvttps2dq	cvttsd2si
divpd		divsd		lfence		maskmovdqu	maxpd
maxsd		mfence		minpd		minsd		movapd
movd		movdq2q		movdqa		movdqu		movhpd
movlpd		movmskpd	movntdq		movnti		movntpd
movq		movq2dq		movsd		movupd		mulpd
mulsd		orpd		packssdw	packsswb	packuswb
paddb		paddd		paddq		paddsb		paddsw
paddusb		paddusw		paddw		pand		pandn
pause		pavgb		pavgw		pcmpeqb		pcmpeqd
pcmpeqw		pcmpgtb		pcmpgtd		pcmpgtw		pextrw
pinsrw		pmaddwd		pmaxsw		pmaxub		pminsw
pminub		pmovmskb	pmulhw		pmulhuw		pmullw
pmuludq		por		psadbw		pshufd		pshufhw
pshuflw		pslld		pslldq		psllq		psllw
psrad		psraw		psrld		psrldq		psrlq
psrlw		psubb		psubd		psubq		psubsb
psubsw		psubusb		psubusw		psubw		punpckhbw
punpckhdq	punpckhqdq	punpckhwd	punpcklbw	punpckldq
punpcklqdq	punpcklwd	pxor		shufpd		sqrtpd
sqrtsd		subpd		subsd		ucomisd		unpckhpd
unpcklpd	xorpd

- SSE3 Instructions
-
addsubpd	addsubps	fisttp		haddpd		haddps
hsubpd		hsubps		lddqu		monitor		movddup
movshdup	movsldup	mwait

- SSE4 Instructions
-
pabsb		pabsd		pabsw		palignr		phaddd
phaddsw		phaddw		phsubd		phsubsw		phsubw
pmaddubsw	pmulhrsw	pshufb		psignb		psignd
psignw

- 3DNow! Instructions
-
femms		pavgusb		pf2id		pfacc		pfadd
pfcmpeq		pfcmpge		pfcmpgt		pfmax		pfmin
pfmul		pfrcp		pfrcpit1	pfrcpit2	pfrsqit1
pfrsqrt		pfsub		pfsubr		pi2fd		pmulhrw
prefetch	prefetchw

- 3DNow! Extension Instructions
-
pf2iw		pfnacc		pfpnacc		pi2fw		pswapd

- Extension Instructions
-
rdtscp

-==================-
 Predefined symbols
-==================-

@32Bit		@code		@CodeSize	@Cpu		@curseg
@data		@DataSize	@date		@fardata	@fardata?
@FileName	@Interface	@line		@lzasm		@Model
@stack		@time		@unicode	@WordSize	@Object
??filename

-=========-
 Operators
-=========-
Priority Operators
1	( ) [ ] length mask offset seg size width
2	high low
3	+ -(unary)
4	* / mod shl shr
5	+ -(binary)
6	eq ge gt le lt ne
7	not
8	and
9	or xor
10	:(segment override)
11	.(structure member selector)
12	small large ptr high(before pointer) low(before pointer) short symtype
