//-----------------------------------------------------------------------
// <copyright file="X86Instruction.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.X86
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// X86 instruction.
    /// </summary>
    public class X86Instruction : Instruction
    {
        private const byte RepPrefix = 0xf3;
        private const byte RepNZPrefix = 0xf2;
        private const byte LockPrefix = 0xf0;
        private const byte OperandSizeOverridePrefix = 0x66;

        private static readonly string[] SetccOpCodes = new string[]
        {
            "setb",
            "setae",
            "sete",
            "setne",
            "setbe",
            "seta",
            "sets",
            "setns",
            "setp",
            "setnp",
            "setl",
            "setge",
            "setle",
            "setg",
        };

        private readonly byte[] machineCode;

        private readonly RelocationEntry[] relocations;

        private readonly string? dispLabel;

        private X86Instruction(byte opCode)
        : this(new byte[] { opCode })
        {
        }

        private X86Instruction(byte[] code)
        : this(code, Array.Empty<RelocationEntry>(), null)
        {
        }

        private X86Instruction(byte[] code, RelocationEntry[] relocations, string? dispLabel)
        {
            this.machineCode = code;
            this.relocations = relocations;
            this.dispLabel = dispLabel;
        }

        /// <inheritdoc/>
        public override IEnumerable<RelocationEntry> RelocationEntries => this.relocations;

        /// <summary>
        /// Gets the label for any address displacement in the instruction.
        /// </summary>
        internal string? DispLabel => this.dispLabel;

        /// <summary>
        /// Creates an add instruction.
        /// </summary>
        /// <param name="reg">The destination register.</param>
        /// <param name="imm">The immediate byte.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Add(Register reg, byte imm)
        {
            if (reg == Register.AL)
            {
                return new X86Instruction(new byte[] { 0x04, imm });
            }
            else
            {
                switch (reg.Size())
                {
                    case 1:
                        return new X86Instruction(new byte[] { 0x80, (byte)(0xC0 + reg.RegisterCode()), imm });
                    case 2:
                        // TODO:
                        throw new NotImplementedException();
                    default:
                        return new X86Instruction(new byte[] { 0x83, (byte)(0xC0 + reg.RegisterCode()), imm });
                }
            }
        }

        /// <summary>
        /// Creates an add instruction.
        /// </summary>
        /// <param name="reg">The destination register.</param>
        /// <param name="imm">The immediate word.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Add(Register reg, ushort imm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an add instruction.
        /// </summary>
        /// <param name="reg">The destination register.</param>
        /// <param name="imm">The immediate dword.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Add(Register reg, uint imm)
        {
            if (reg.Size() != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(reg));
            }

            if (reg == Register.EAX)
            {
                byte[] code = new byte[sizeof(uint) + 1];
                code[0] = 0x05;
                Array.Copy(ToBytes(imm), 0, code, 1, sizeof(uint));
                return new X86Instruction(code);
            }
            else
            {
                byte[] code = new byte[2 + sizeof(uint)];
                code[0] = 0x81;
                code[1] = (byte)(0xC0 + reg.RegisterCode());
                Array.Copy(ToBytes(imm), 0, code, 2, sizeof(uint));
                return new X86Instruction(code);
            }
        }

        /// <summary>
        /// Creates an add instruction that adds the contents of a source register to the destination register.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Add(Register dest, Register src)
        {
            if (dest.Size() != src.Size())
            {
                throw new ArgumentOutOfRangeException(nameof(src));
            }

            byte srcReg = (byte)(src.RegisterCode() << 3);
            byte destReg = (byte)(dest.RegisterCode() | 0xC0);
            byte modRM = (byte)(srcReg | destReg);
            byte[] code = new byte[]
            {
                (byte)(dest.Size() == 1 ? 0x00 : 0x01),
                modRM,
            };

            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates an And instruction.
        /// </summary>
        /// <param name="reg">The target register.</param>
        /// <param name="imm">The immediate byte.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction And(Register reg, byte imm)
        {
            if (reg.Size() != sizeof(byte))
            {
                throw new ArgumentOutOfRangeException(nameof(reg));
            }

            return reg == Register.AL ?
                new X86Instruction(new byte[] { 0x24, imm }) :
                new X86Instruction(new byte[] { 0x80, (byte)(0xe0 | reg.RegisterCode()), imm });
        }

        /// <summary>
        /// Creates an And instruction.
        /// </summary>
        /// <param name="reg">The target register.</param>
        /// <param name="imm">The immediate dword.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction And(Register reg, uint imm)
        {
            if (reg.Size() != sizeof(uint))
            {
                throw new ArgumentOutOfRangeException(nameof(reg));
            }

            byte[] code = Array.Empty<byte>();
            if (reg == Register.EAX)
            {
                code = new byte[1 + sizeof(uint)];
                code[0] = 0x25;
                Array.Copy(ToBytes(imm), 0, code, 1, sizeof(uint));
            }
            else
            {
                code = new byte[2 + sizeof(uint)];
                code[0] = 0x81;
                code[1] = (byte)(0xe0 | reg.RegisterCode());
                Array.Copy(ToBytes(imm), 0, code, 2, sizeof(uint));
            }

            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates an add instruction that ands the contents of a source register to the destination register.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction And(Register dest, Register src)
        {
            if (dest.Size() != src.Size())
            {
                throw new ArgumentOutOfRangeException(nameof(src));
            }

            byte srcReg = (byte)(src.RegisterCode() << 3);
            byte destReg = (byte)(dest.RegisterCode() | 0xC0);
            byte modRM = (byte)(srcReg | destReg);
            byte[] code = new byte[]
            {
                (byte)(dest.Size() == 1 ? 0x20 : 0x21),
                modRM,
            };

            return new X86Instruction(code);
        }

        /// <summary>
        /// Create a call near instruction.
        /// </summary>
        /// <param name="symbol">The symbol reference.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Call(string symbol)
        {
            RelocationEntry rel = new RelocationEntry(symbol, 1, sizeof(uint), true);
            return new X86Instruction(new byte[] { 0xe8, 0xfc, 0xff, 0xff, 0xff }, new RelocationEntry[] { rel }, null);
        }

        /// <summary>
        /// Create a call near instruction.
        /// </summary>
        /// <param name="address">The location to call.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Call(RM address)
        {
            if (address.OperandSize != sizeof(uint))
            {
                throw new ArgumentOutOfRangeException(nameof(address));
            }

            byte[] code = new byte[1 + address.Length];
            code[0] = 0xff;
            address.CopyTo(new Span<byte>(code, 1, address.Length));
            code[1] |= 2 << 3;
            return new X86Instruction(code, GetRelocations(address, 1), address.Label);
        }

        /// <summary>
        /// Create a cld instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Cld()
        {
            return new X86Instruction(0xc);
        }

        /// <summary>
        /// Creates a cmp instruction.
        /// </summary>
        /// <param name="r1">The first register.</param>
        /// <param name="r2">The second register.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Cmp(Register r1, Register r2)
        {
            return Cmp(r1, RM.FromRegister(r2));
        }

        /// <summary>
        /// Creates a cmp instruction.
        /// </summary>
        /// <param name="r1">The first register.</param>
        /// <param name="rm">The second operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Cmp(Register r1, RM rm)
        {
            if (r1.Size() != rm.OperandSize)
            {
                throw new ArgumentOutOfRangeException(nameof(rm));
            }

            byte[] code = new byte[1 + rm.Length];
            code[0] = (byte)(rm.OperandSize == sizeof(byte) ? 0x3a : 0x3b);
            rm.CopyTo(new Span<byte>(code, 1, rm.Length));
            code[1] |= (byte)(r1.RegisterCode() << 3);
            return new X86Instruction(code);
        }

        /// <summary>
        /// Create a fnstsw ax near instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Fnstsw()
        {
            return new X86Instruction(new byte[] { 0xdf, 0xe0 });
        }

        /// <summary>
        /// Create a jmp near instruction.
        /// </summary>
        /// <param name="symbol">The symbol reference.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Jmp(string symbol)
        {
            RelocationEntry rel = new RelocationEntry(symbol, 1, sizeof(uint), true);
            return new X86Instruction(new byte[] { 0xe9, 0xfc, 0xff, 0xff, 0xff }, new RelocationEntry[] { rel }, null);
        }

        /// <summary>
        /// Create a jz near instruction.
        /// </summary>
        /// <param name="symbol">The symbol reference.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Jz(string symbol)
        {
            RelocationEntry rel = new RelocationEntry(symbol, 2, sizeof(uint), true);
            return new X86Instruction(new byte[] { 0x0f, 0x84, 0xfc, 0xff, 0xff, 0xff }, new RelocationEntry[] { rel }, null);
        }

        /// <summary>
        /// Creates a lea instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="rm">The memory access.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Lea(Register dest, RM rm)
        {
            byte[] code = new byte[1 + rm.Length];
            code[0] = 0x8d;
            rm.CopyTo(new Span<byte>(code, 1, rm.Length));
            code[1] |= (byte)(dest.RegisterCode() << 3);
            return new X86Instruction(code, GetRelocations(rm, 1), rm.Label);
        }

        /// <summary>
        /// Create a mov instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Mov(Register dest, Register src)
        {
            return Mov(RM.FromRegister(dest), src);
        }

        /// <summary>
        /// Create a mov instruction.
        /// </summary>
        /// <param name="dest">The destination register or memory.</param>
        /// <param name="src">The source register.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Mov(RM dest, Register src)
        {
            if (dest.OperandSize != src.Size())
            {
                throw new ArgumentOutOfRangeException(nameof(src));
            }

            byte opcode = (byte)(dest.OperandSize == sizeof(byte) ? 0x88 : 0x89);
            int index = 0;
            bool operandSizeOverride = dest.OperandSize == sizeof(ushort);
            byte[] code = new byte[(operandSizeOverride ? 2 : 1) + dest.Length];
            if (operandSizeOverride)
            {
                code[index++] = OperandSizeOverridePrefix;
            }

            code[index++] = opcode;
            dest.CopyTo(new Span<byte>(code, index, dest.Length));
            code[index] |= (byte)(src.RegisterCode() << 3);
            return new X86Instruction(code, GetRelocations(dest, index), dest.Label);
        }

        /// <summary>
        /// Create a mov instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source register or memory.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Mov(Register dest, RM src)
        {
            if (src.OperandSize != dest.Size())
            {
                throw new ArgumentOutOfRangeException(nameof(dest));
            }

            byte opcode = (byte)(src.OperandSize == sizeof(byte) ? 0x8a : 0x8b);
            int index = 0;
            bool operandSizeOverride = src.OperandSize == sizeof(ushort);
            byte[] code = new byte[(operandSizeOverride ? 2 : 1) + src.Length];
            if (operandSizeOverride)
            {
                code[index++] = OperandSizeOverridePrefix;
            }

            code[index++] = opcode;
            src.CopyTo(new Span<byte>(code, index, src.Length));
            code[index] |= (byte)(dest.RegisterCode() << 3);
            return new X86Instruction(code, GetRelocations(src, index), src.Label);
        }

        /// <summary>
        /// Create a mov instruction.
        /// </summary>
        /// <param name="dest">The destination register.</param>
        /// <param name="src">The source value.</param>
        /// <param name="symbol">The symbol to reference in a relocation.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Mov(Register dest, uint src, string? symbol = null)
        {
            byte[] code = new byte[1 + sizeof(uint)];
            code[0] = (byte)(0xb8 + dest.RegisterCode());
            Array.Copy(ToBytes(src), 0, code, 1, sizeof(uint));
            RelocationEntry[] rel = Array.Empty<RelocationEntry>();
            if (!string.IsNullOrEmpty(symbol))
            {
                rel = new RelocationEntry[]
                {
                    new RelocationEntry(symbol, 1, sizeof(uint), false),
                };
            }

            return new X86Instruction(code, rel, null);
        }

        /// <summary>
        /// Create a movsb instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Movsb()
        {
            return new X86Instruction(0xa4);
        }

        /// <summary>
        /// Creates an imul instruction that stores the result in the primary accumulator register.
        /// </summary>
        /// <param name="rm">The register/memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Imul(RM rm)
        {
            byte opcode = 0xf7;
            bool encodingPrefix = false;
            switch (rm.OperandSize)
            {
                case sizeof(byte):
                    opcode = 0xf6;
                    break;
                case sizeof(ushort):
                    encodingPrefix = true;
                    break;
            }

            byte[] code = new byte[rm.Length + (encodingPrefix ? 1 : 0) + 1];
            int index = 0;
            if (encodingPrefix)
            {
                code[index++] = OperandSizeOverridePrefix;
            }

            code[index++] = opcode;
            rm.CopyTo(new Span<byte>(code, index, rm.Length));
            code[index] |= (byte)(5 << 3);
            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates an imul instruction that stores the result in the destinationregister.
        /// </summary>
        /// <param name="dest">The destination register operand.</param>
        /// <param name="src">The register/memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Imul(Register dest, RM src)
        {
            if (dest == Register.AL || dest == Register.AX || dest == Register.EAX)
            {
                return Imul(src);
            }

            if (src.OperandSize != dest.Size())
            {
                throw new ArgumentOutOfRangeException(nameof(src));
            }

            bool encodingPrefix = src.OperandSize == sizeof(ushort);
            if (!encodingPrefix && src.OperandSize != sizeof(uint))
            {
                throw new ArgumentOutOfRangeException(nameof(src));
            }

            byte[] code = new byte[src.Length + (encodingPrefix ? 1 : 0) + 2];
            int index = 0;
            if (encodingPrefix)
            {
                code[index++] = OperandSizeOverridePrefix;
            }

            code[index++] = 0x0f;
            code[index++] = 0xaf;
            src.CopyTo(new Span<byte>(code, index, src.Length));
            code[index] |= (byte)(dest.RegisterCode() << 3);
            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates a NEG (2's complement) instruction.
        /// </summary>
        /// <param name="reg">The register to negate.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Neg(Register reg)
        {
            byte opCode = 0;
            switch (reg.Size())
            {
                case sizeof(byte):
                    opCode = 0xf6;
                    break;
                case sizeof(uint):
                    opCode = 0xf7;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new X86Instruction(new byte[] { opCode, (byte)(0xd8 | reg.RegisterCode()) });
        }

        /// <summary>
        /// Create a nop instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Nop()
        {
            return new X86Instruction(0x90);
        }

        /// <summary>
        /// Creates a NOT (1's complement) instruction.
        /// </summary>
        /// <param name="reg">The register to invert.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Not(Register reg)
        {
            byte opCode = 0;
            switch (reg.Size())
            {
                case sizeof(byte):
                    opCode = 0xf6;
                    break;
                case sizeof(uint):
                    opCode = 0xf7;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new X86Instruction(new byte[] { opCode, (byte)(0xd0 | reg.RegisterCode()) });
        }

        /// <summary>
        /// Create a push instruction.
        /// </summary>
        /// <param name="reg">The register to push.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Push(Register reg)
        {
            if (reg.Size() == sizeof(int))
            {
                return new X86Instruction((byte)(0x50 + reg.RegisterCode()));
            }

            throw new NotSupportedException($"Register {reg} is not supported.");
        }

        /// <summary>
        /// Create a pop instruction.
        /// </summary>
        /// <param name="reg">The register to pop into.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Pop(Register reg)
        {
            if (reg.Size() == sizeof(int))
            {
                return new X86Instruction((byte)(0x58 + reg.RegisterCode()));
            }

            throw new NotSupportedException($"Register {reg} is not supported.");
        }

        /// <summary>
        /// Creates a near ret instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Ret()
        {
            return new X86Instruction(0xc3);
        }

        /// <summary>
        /// Creates a sahf instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Sahf()
        {
            return new X86Instruction(0x9e);
        }

        /// <summary>
        /// Creates a seta instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Seta(RM rm)
        {
            return Setcc(0x97, rm);
        }

        /// <summary>
        /// Creates a setae instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setae(RM rm)
        {
            return Setcc(0x93, rm);
        }

        /// <summary>
        /// Creates a setb instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setb(RM rm)
        {
            return Setcc(0x92, rm);
        }

        /// <summary>
        /// Creates a setbe instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setbe(RM rm)
        {
            return Setcc(0x96, rm);
        }

        /// <summary>
        /// Creates a sete instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Sete(RM rm)
        {
            return Setcc(0x94, rm);
        }

        /// <summary>
        /// Creates a setg instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setg(RM rm)
        {
            return Setcc(0x9f, rm);
        }

        /// <summary>
        /// Creates a setge instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setge(RM rm)
        {
            return Setcc(0x9d, rm);
        }

        /// <summary>
        /// Creates a setl instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setl(RM rm)
        {
            return Setcc(0x9c, rm);
        }

        /// <summary>
        /// Creates a setle instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setle(RM rm)
        {
            return Setcc(0x9e, rm);
        }

        /// <summary>
        /// Creates a setne instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Setne(RM rm)
        {
            return Setcc(0x95, rm);
        }

        /// <summary>
        /// Creates a shl by one instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Shl(RM rm)
        {
            bool opprefix = rm.OperandSize == sizeof(ushort);
            byte[] code = new byte[1 + rm.Length + (opprefix ? 1 : 0)];
            int index = 0;
            if (opprefix)
            {
                code[index++] = OperandSizeOverridePrefix;
            }

            code[index++] = (byte)(rm.OperandSize == sizeof(byte) ? 0xd0 : 0xd1);
            rm.CopyTo(new Span<byte>(code, index, rm.Length));
            code[index] |= 4 << 3;
            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates a shl instruction.
        /// </summary>
        /// <param name="rm">The register or memory operand.</param>
        /// <param name="imm">The immediate value.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Shl(RM rm, byte imm)
        {
            if (imm == 1)
            {
                return Shl(rm);
            }

            bool opprefix = rm.OperandSize == sizeof(ushort);
            byte[] code = new byte[2 + rm.Length + (opprefix ? 1 : 0)];
            int index = 0;
            if (opprefix)
            {
                code[index++] = OperandSizeOverridePrefix;
            }

            code[index++] = (byte)(rm.OperandSize == sizeof(byte) ? 0xc0 : 0xc1);
            rm.CopyTo(new Span<byte>(code, index, rm.Length));
            code[index] |= 4 << 3;
            code[index + rm.Length] = imm;
            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates a sub instruction.
        /// </summary>
        /// <param name="reg">The register to subtract from.</param>
        /// <param name="imm">The immediate value to subtract.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Sub(Register reg, byte imm)
        {
            if (reg.Size() != sizeof(byte))
            {
                throw new ArgumentOutOfRangeException(nameof(reg));
            }

            if (reg == Register.AL)
            {
                return new X86Instruction(new byte[] { 0x2c, imm });
            }
            else
            {
                return new X86Instruction(new byte[] { 0x80, (byte)(0xe8 | reg.RegisterCode()), imm });
            }
        }

        /// <summary>
        /// Creates a sub instruction.
        /// </summary>
        /// <param name="reg">The register to subtract from.</param>
        /// <param name="imm">The immediate value to subtract.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Sub(Register reg, uint imm)
        {
            if (reg.Size() != sizeof(uint))
            {
                throw new ArgumentOutOfRangeException(nameof(reg));
            }

            byte[] code = Array.Empty<byte>();
            if (reg == Register.EAX)
            {
                code = new byte[1 + sizeof(uint)];
                code[0] = 0x2d;
                Array.Copy(ToBytes(imm), 0, code, 1, sizeof(uint));
            }
            else
            {
                code = new byte[2 + sizeof(uint)];
                code[0] = 0x81;
                code[1] = (byte)(0xe8 | reg.RegisterCode());
                Array.Copy(ToBytes(imm), 0, code, 2, sizeof(uint));
            }

            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates a sub instruction.
        /// </summary>
        /// <param name="dest">The register to subtract from and store the result.</param>
        /// <param name="src">The value to subtract.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Sub(Register dest, Register src)
        {
            if (dest.Size() != src.Size())
            {
                throw new ArgumentOutOfRangeException(nameof(src));
            }

            byte srcReg = (byte)(src.RegisterCode() << 3);
            byte destReg = (byte)(dest.RegisterCode() | 0xC0);
            byte modRM = (byte)(srcReg | destReg);
            byte[] code = new byte[]
            {
                (byte)(dest.Size() == 1 ? 0x28 : 0x29),
                modRM,
            };

            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates a test instruction.
        /// </summary>
        /// <param name="r1">The first register to compare.</param>
        /// <param name="r2">The second register to compare.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Test(Register r1, Register r2)
        {
            return Test(RM.FromRegister(r1), r2);
        }

        /// <summary>
        /// Creates a test instruction.
        /// </summary>
        /// <param name="rm">The value to compare.</param>
        /// <param name="reg">The register to compare.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Test(RM rm, Register reg)
        {
            if (rm.OperandSize != reg.Size())
            {
                throw new ArgumentOutOfRangeException(nameof(rm));
            }

            byte[] code = new byte[1 + rm.Length];
            code[0] = (byte)(rm.OperandSize == sizeof(byte) ? 0x84 : 0x85);
            rm.CopyTo(new Span<byte>(code, 1, rm.Length));
            code[1] |= (byte)(reg.RegisterCode() << 3);
            return new X86Instruction(code);
        }

        /// <summary>
        /// Creates an xor instruction.
        /// </summary>
        /// <param name="dest">The register to xor with and store the result.</param>
        /// <param name="src">The register to xor.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Xor(Register dest, Register src)
        {
            return Xor(dest, RM.FromRegister(src));
        }

        /// <summary>
        /// Creates an xor instruction.
        /// </summary>
        /// <param name="dest">The register to xor with and store the result.</param>
        /// <param name="src">The value to xor.</param>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public static X86Instruction Xor(Register dest, RM src)
        {
            if (dest.Size() != src.OperandSize)
            {
                throw new ArgumentOutOfRangeException(nameof(src));
            }

            byte[] code = new byte[1 + src.Length];
            code[0] = (byte)(src.OperandSize == sizeof(byte) ? 0x32 : 0x33);
            src.CopyTo(new Span<byte>(code, 1, src.Length));
            code[1] |= (byte)(dest.RegisterCode() << 3);
            return new X86Instruction(code);
        }

        /// <summary>
        /// Prepends the REP prefix on an instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public X86Instruction Rep()
        {
            return this.PrependPrefix(RepPrefix);
        }

        /// <summary>
        /// Prepends the REPNE/REPNZ prefix on an instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public X86Instruction RepNZ()
        {
            return this.PrependPrefix(RepNZPrefix);
        }

        /// <summary>
        /// Prepends the REP prefix on an instruction.
        /// </summary>
        /// <returns>A new instance of the <see cref="X86Instruction"/> class.</returns>
        public X86Instruction Lock()
        {
            return this.PrependPrefix(LockPrefix);
        }

        /// <inheritdoc/>
        public override byte[] ToArray()
        {
            return this.machineCode;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            int index = 0;
            bool operandSizeOverride = false;
            StringBuilder result = new StringBuilder();

            byte prefix = this.machineCode[index];
            while (IsPrefix(prefix))
            {
                switch (prefix)
                {
                    case RepPrefix:
                        result.Append("rep ");
                        break;
                    case RepNZPrefix:
                        result.Append("repnz ");
                        break;
                    case LockPrefix:
                        result.Append("lock ");
                        break;
                    case OperandSizeOverridePrefix:
                        operandSizeOverride = true;
                        break;
                }

                prefix = this.machineCode[++index];
            }

            byte opCode = this.machineCode[index++];

            if (opCode >= 0x50 && opCode < 0x58)
            {
                byte regCode = (byte)(opCode - 0x50);
                result.Append($"push {regCode.FromRegisterCode(sizeof(uint)).ToString().ToLowerInvariant()}");
            }
            else if (opCode >= 0x58 && opCode < 0x60)
            {
                byte regCode = (byte)(opCode - 0x58);
                result.Append($"pop {regCode.FromRegisterCode(sizeof(uint)).ToString().ToLowerInvariant()}");
            }
            else if (opCode >= 0xb8 && opCode < 0xc0)
            {
                byte regCode = (byte)(opCode - 0xb8);
                string imm = new ReadOnlySpan<byte>(this.machineCode, index, sizeof(uint)).ToUInt32().ToString();
                result.Append($"mov {regCode.FromRegisterCode(sizeof(uint)).ToString().ToLowerInvariant()}, {(this.relocations.Length > 0 ? this.relocations[0].Symbol : imm)}");
            }
            else
            {
                switch (opCode)
                {
                    case 0x0:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"add {rm}, {DecodeModRMReg(modRM, 1).ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x1:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(uint));
                            result.Append($"add {rm}, {DecodeModRMReg(modRM, 4).ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x4:
                        result.Append($"add al, {this.machineCode[index]}");
                        break;
                    case 0x5:
                        result.Append($"add eax, {new ReadOnlySpan<byte>(this.machineCode, index, sizeof(uint)).ToUInt32()}");
                        break;
                    case 0xc:
                        result.Append("cld");
                        break;
                    case 0x0f:
                        {
                            opCode = this.machineCode[index++];
                            switch (opCode)
                            {
                                case 0x84:
                                    result.Append($"jz {this.relocations[0].Symbol}");
                                    break;
                                case 0x92:
                                case 0x93:
                                case 0x94:
                                case 0x95:
                                case 0x96:
                                case 0x97:
                                case 0x98:
                                case 0x99:
                                case 0x9a:
                                case 0x9b:
                                case 0x9c:
                                case 0x9d:
                                case 0x9e:
                                case 0x9f:
                                    {
                                        string setccstr = SetccOpCodes[opCode - 0x92];
                                        RM rm = this.GetRM(index, sizeof(byte));
                                        result.Append($"{setccstr} {rm}");
                                    }

                                    break;
                                case 0xaf:
                                    {
                                        int operandSize = operandSizeOverride ? sizeof(ushort) : sizeof(uint);
                                        RM rm = this.GetRM(index, operandSize);
                                        Register reg = DecodeModRMReg(this.machineCode[index], operandSize);
                                        result.Append($"imul {reg.ToString().ToLowerInvariant()}, {rm}");
                                    }

                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        break;
                    case 0x20:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"and {rm}, {DecodeModRMReg(modRM, 1).ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x21:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(uint));
                            result.Append($"and {rm}, {DecodeModRMReg(modRM, 4).ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x24:
                        result.Append($"and al, {this.machineCode[index]}");
                        break;
                    case 0x25:
                        result.Append($"and eax, {new ReadOnlySpan<byte>(this.machineCode, index, sizeof(uint)).ToUInt32()}");
                        break;
                    case 0x28:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"sub {rm}, {DecodeModRMReg(modRM, 1).ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x29:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(uint));
                            result.Append($"sub {rm}, {DecodeModRMReg(modRM, 4).ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x2c:
                        result.Append($"sub al, {this.machineCode[index]}");
                        break;
                    case 0x2d:
                        result.Append($"sub eax, {new ReadOnlySpan<byte>(this.machineCode, index, sizeof(uint)).ToUInt32()}");
                        break;
                    case 0x32:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"xor {DecodeModRMReg(modRM, sizeof(byte)).ToString().ToLowerInvariant()}, {rm}");
                        }

                        break;
                    case 0x33:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(uint));
                            result.Append($"xor {DecodeModRMReg(modRM, sizeof(uint)).ToString().ToLowerInvariant()}, {rm}");
                        }

                        break;

                    case 0x3a:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"cmp {DecodeModRMReg(modRM, sizeof(byte)).ToString().ToLowerInvariant()}, {rm}");
                        }

                        break;
                    case 0x3b:
                        {
                            byte modRM = this.machineCode[index];
                            RM rm = this.GetRM(index, sizeof(uint));
                            result.Append($"cmp {DecodeModRMReg(modRM, sizeof(uint)).ToString().ToLowerInvariant()}, {rm}");
                        }

                        break;
                    case 0x80:
                        {
                            byte modrm = this.machineCode[index++];
                            Register r = ((byte)(modrm & 0x7)).FromRegisterCode(sizeof(byte));
                            byte opCodeMod = DecodeModRMOpcode(modrm);
                            switch (opCodeMod)
                            {
                                case 4:
                                    result.Append($"and {r.ToString().ToLowerInvariant()}, {this.machineCode[index]}");
                                    break;
                                case 5:
                                    result.Append($"sub {r.ToString().ToLowerInvariant()}, {this.machineCode[index]}");
                                    break;
                                default:
                                    result.Append($"add {r.ToString().ToLowerInvariant()}, {this.machineCode[index]}");
                                    break;
                            }
                        }

                        break;
                    case 0x81:
                        {
                            byte modrm = this.machineCode[index++];
                            Register r = ((byte)(modrm & 0x7)).FromRegisterCode(4);
                            byte opCodeMod = DecodeModRMOpcode(modrm);
                            switch (opCodeMod)
                            {
                                case 4:
                                    result.Append($"and {r.ToString().ToLowerInvariant()}, {new ReadOnlySpan<byte>(this.machineCode, index, sizeof(uint)).ToUInt32()}");
                                    break;
                                case 5:
                                    result.Append($"sub {r.ToString().ToLowerInvariant()}, {new ReadOnlySpan<byte>(this.machineCode, index, sizeof(uint)).ToUInt32()}");
                                    break;
                                default:
                                    result.Append($"add {r.ToString().ToLowerInvariant()}, {new ReadOnlySpan<byte>(this.machineCode, index, sizeof(uint)).ToUInt32()}");
                                    break;
                            }
                        }

                        break;
                    case 0x83:
                        {
                            byte modrm = this.machineCode[index++];
                            Register r = ((byte)(modrm & 0x7)).FromRegisterCode(sizeof(uint));
                            result.Append($"add {r.ToString().ToLowerInvariant()}, {this.machineCode[index]}");
                        }

                        break;
                    case 0x84:
                        {
                            byte modrm = this.machineCode[index];
                            Register r = DecodeModRMReg(modrm, sizeof(byte));
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"test {rm}, {r.ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x85:
                        {
                            byte modrm = this.machineCode[index];
                            Register r = DecodeModRMReg(modrm, sizeof(uint));
                            RM rm = this.GetRM(index, sizeof(uint));
                            result.Append($"test {rm}, {r.ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x88:
                        {
                            byte modrm = this.machineCode[index];
                            Register r = DecodeModRMReg(modrm, sizeof(byte));
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"mov {rm}, {r.ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x89:
                        {
                            byte modrm = this.machineCode[index];
                            int size = operandSizeOverride ? sizeof(ushort) : sizeof(uint);
                            Register r = DecodeModRMReg(modrm, size);
                            RM rm = this.GetRM(index, size);
                            result.Append($"mov {rm}, {r.ToString().ToLowerInvariant()}");
                        }

                        break;
                    case 0x8a:
                        {
                            byte modrm = this.machineCode[index];
                            Register r = DecodeModRMReg(modrm, sizeof(byte));
                            RM rm = this.GetRM(index, sizeof(byte));
                            result.Append($"mov {r.ToString().ToLowerInvariant()}, {rm}");
                        }

                        break;
                    case 0x8b:
                        {
                            byte modrm = this.machineCode[index];
                            int size = operandSizeOverride ? sizeof(ushort) : sizeof(uint);
                            Register r = DecodeModRMReg(modrm, size);
                            RM rm = this.GetRM(index, size);
                            result.Append($"mov {r.ToString().ToLowerInvariant()}, {rm}");
                        }

                        break;

                    case 0x8d:
                        {
                            byte modrm = this.machineCode[index];
                            Register r = DecodeModRMReg(modrm, sizeof(uint));
                            RM rm = this.GetRM(index, sizeof(uint));
                            result.Append($"lea {r.ToString().ToLowerInvariant()}, {rm}");
                        }

                        break;
                    case 0x9e:
                        result.Append("sahf");
                        break;
                    case 0xa4:
                        result.Append("movsb");
                        break;
                    case 0xc0:
                        {
                            byte modrm = this.machineCode[index];
                            switch (DecodeModRMOpcode(modrm))
                            {
                                case 4:
                                    {
                                        RM rm = this.GetRM(index, sizeof(byte));
                                        byte imm = this.machineCode[index + rm.Length];
                                        result.Append($"shl {rm}, {imm}");
                                    }

                                    break;
                                default:
                                    throw new NotSupportedException();
                            }
                        }

                        break;
                    case 0xc1:
                        {
                            byte modrm = this.machineCode[index];
                            switch (DecodeModRMOpcode(modrm))
                            {
                                case 4:
                                    {
                                        RM rm = this.GetRM(index, operandSizeOverride ? sizeof(ushort) : sizeof(uint));
                                        byte imm = this.machineCode[index + rm.Length];
                                        result.Append($"shl {rm}, {imm}");
                                    }

                                    break;
                                default:
                                    throw new NotSupportedException();
                            }
                        }

                        break;
                    case 0xc3:
                        result.Append("ret");
                        break;
                    case 0xd0:
                        {
                            byte modrm = this.machineCode[index];
                            switch (DecodeModRMOpcode(modrm))
                            {
                                case 4:
                                    {
                                        RM rm = this.GetRM(index, sizeof(byte));
                                        result.Append($"shl {rm}, 1");
                                    }

                                    break;
                                default:
                                    throw new NotSupportedException();
                            }
                        }

                        break;
                    case 0xd1:
                        {
                            byte modrm = this.machineCode[index];
                            switch (DecodeModRMOpcode(modrm))
                            {
                                case 4:
                                    {
                                        RM rm = this.GetRM(index, operandSizeOverride ? sizeof(ushort) : sizeof(uint));
                                        result.Append($"shl {rm}, 1");
                                    }

                                    break;
                                default:
                                    throw new NotSupportedException();
                            }
                        }

                        break;
                    case 0xdf:
                        {
                            byte opcode = this.machineCode[index++];
                            switch (opcode)
                            {
                                case 0xe0:
                                    result.Append("fnstsw ax");
                                    break;
                                default:
                                    throw new NotSupportedException();
                            }
                        }

                        break;
                    case 0xe8:
                        result.Append($"call {this.relocations[0].Symbol}");
                        break;
                    case 0xe9:
                        result.Append($"jmp {this.relocations[0].Symbol}");
                        break;
                    case 0xf6:
                        {
                            byte modrm = this.machineCode[index];
                            int opcode = DecodeModRMOpcode(modrm);
                            RM rm = this.GetRM(index, sizeof(byte));
                            switch (opcode)
                            {
                                case 0: // test
                                    throw new NotImplementedException();
                                case 2: // not
                                    result.Append($"not {rm}");
                                    break;
                                case 3: // neg
                                    result.Append($"neg {rm}");
                                    break;
                                case 5: // imul
                                    result.Append($"imul al, {rm}");
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        break;
                    case 0xf7:
                        {
                            byte modrm = this.machineCode[index];
                            int opcode = DecodeModRMOpcode(modrm);
                            RM rm = this.GetRM(index, operandSizeOverride ? sizeof(ushort) : sizeof(uint));
                            switch (opcode)
                            {
                                case 0: // test
                                    throw new NotImplementedException();
                                case 2: // not
                                    result.Append($"not {rm}");
                                    break;
                                case 3: // neg
                                    result.Append($"neg {rm}");
                                    break;
                                case 5:
                                    result.Append($"imul {(operandSizeOverride ? Register.AX : Register.EAX).ToString().ToLowerInvariant()}, {rm}");
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        break;
                    case 0xff:
                        {
                            byte modrm = this.machineCode[index];
                            int opcode = DecodeModRMOpcode(modrm);
                            switch (opcode)
                            {
                                case 2: // call near indirect
                                    {
                                        RM rm = this.GetRM(index, sizeof(uint));
                                        result.Append($"call {rm}");
                                    }

                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        break;
                    default:
                        result.Append("nop");
                        break;
                }
            }

            return result.ToString();
        }

        private static X86Instruction Setcc(byte opcode, RM rm)
        {
            byte[] code = new byte[2 + rm.Length];
            code[0] = 0x0f;
            code[1] = opcode;
            rm.CopyTo(new Span<byte>(code, 2, rm.Length));
            return new X86Instruction(code);
        }

        private static RelocationEntry[] GetRelocations(RM rm, int offset)
        {
            RelocationEntry? rel = rm.Relocation;
            RelocationEntry[] rels = Array.Empty<RelocationEntry>();
            if (rel != null)
            {
                rels = new RelocationEntry[]
                {
                    new RelocationEntry(rel.Symbol, (uint)(rel!.Offset + offset), rel!.Size, rel!.Relative),
                };
            }

            return rels;
        }

        private static Register DecodeModRMReg(byte modRM, int size)
        {
            byte code = DecodeModRMOpcode(modRM);
            return code.FromRegisterCode(size);
        }

        private static byte DecodeModRMOpcode(byte modRM)
        {
            return (byte)((modRM >> 3) & 0x7);
        }

        private static byte[] ToBytes(uint data)
        {
            return new byte[]
            {
                (byte)data,
                (byte)(data >> 8),
                (byte)(data >> 16),
                (byte)(data >> 24),
            };
        }

        private static bool IsPrefix(byte code)
        {
            return code == RepPrefix || code == RepNZPrefix || code == LockPrefix || code == OperandSizeOverridePrefix;
        }

        private bool HasPrefix()
        {
            byte prefix = this.machineCode[0];
            return IsPrefix(prefix);
        }

        private RM GetRM(int index, int size)
        {
            RelocationEntry? rel = null;
            if (this.relocations.Length > 0)
            {
                RelocationEntry src = this.relocations[0];
                rel = new RelocationEntry(src.Symbol, (uint)(src.Offset - index), src.Size, src.Relative);
            }

            return RM.FromModRM(
                new ReadOnlySpan<byte>(this.machineCode, index, this.machineCode.Length - index),
                size,
                rel,
                this.DispLabel);
        }

        private X86Instruction PrependPrefix(byte prefix)
        {
            if (this.HasPrefix() && this.machineCode[0] != OperandSizeOverridePrefix)
            {
                throw new InvalidOperationException($"Instruction [{this.ToString()}] already has a prefix.");
            }

            byte[] code = new byte[this.machineCode.Length + 1];
            code[0] = prefix;
            Array.Copy(this.machineCode, 0, code, 1, this.machineCode.Length);
            return new X86Instruction(code);
        }
    }
}