//-----------------------------------------------------------------------
// <copyright file="RM.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.X86
{
    using System;

    /// <summary>
    /// Encodes r/m8 r/m16 r/m32 access that can be part of an <see cref="X86Instruction"/>.
    /// </summary>
    /// <seealso cref="X86Instruction"/>
    public class RM
    {
        private readonly int size;

        private readonly byte[] code;

        private readonly RelocationEntry? relocation;

        private readonly string? label;

        private RM(ReadOnlySpan<byte> code, int size)
        : this(code, size, null, null)
        {
        }

        private RM(ReadOnlySpan<byte> code, int size, RelocationEntry? relocation, string? label)
        : this(code.ToArray(), size, relocation, label)
        {
        }

        private RM(byte[] code, int size)
        : this(code, size, null, null)
        {
        }

        private RM(byte[] code, int size, RelocationEntry? relocation, string? label)
        {
            if (size != sizeof(byte) && size != sizeof(ushort) && size != sizeof(uint))
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            this.code = code;
            this.size = size;
            this.relocation = relocation;
            this.label = label;
        }

        /// <summary>
        /// Gets the length of the code.
        /// </summary>
        public int Length => this.code.Length;

        /// <summary>
        /// Gets the operand size in bytes. Either 1, 2, or 4.
        /// </summary>
        public int OperandSize => this.size;

        /// <summary>
        /// Gets the relocation entry in this access if present.
        /// </summary>
        public RelocationEntry? Relocation => this.relocation;

        /// <summary>
        /// Gets the label or variable name for the displacement value.
        /// </summary>
        public string? Label => this.label;

        /// <summary>
        /// Creates the code from an existing instruction starting with the ModR/M byte after the opcode.
        /// </summary>
        /// <param name="code">The instruction sequence starting with the ModR/M byte.</param>
        /// <param name="size">The address size. Either 1, 2, or 4.</param>
        /// <param name="rel">Relocation entry from the instruction, if present.</param>
        /// <param name="label">Label for the displacement, if present.</param>
        /// <returns>A new instance of the <see cref="RM"/> class.</returns>
        public static RM FromModRM(ReadOnlySpan<byte> code, int size, RelocationEntry? rel, string? label)
        {
            if (code.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(code));
            }

            byte modrm = code[0];
            switch (modrm >> 6)
            {
                case 0:
                    {
                        byte rm = (byte)(modrm & 0x7);
                        switch (rm)
                        {
                            case 4:
                                throw new NotImplementedException();
                            case 5:
                                return new RM(code.Slice(0, 1 + sizeof(uint)), size, rel, label);
                            default:
                                return new RM(new byte[] { modrm }, size);
                        }
                    }

                case 1:
                    {
                        byte rm = (byte)(modrm & 0x7);
                        switch (rm)
                        {
                            case 4:
                                throw new NotImplementedException();
                            default:
                                return new RM(code.Slice(0, 1 + sizeof(byte)), size, rel, label);
                        }
                    }

                case 2:
                    {
                        byte rm = (byte)(modrm & 0x7);
                        switch (rm)
                        {
                            case 4:
                                throw new NotImplementedException();
                            default:
                                return new RM(code.Slice(0, 1 + sizeof(uint)), size, rel, label);
                        }
                    }

                default:
                    {
                        return new RM(new byte[] { modrm }, size);
                    }
            }
        }

        /// <summary>
        /// Creates a r/m32 for register access.
        /// </summary>
        /// <param name="reg">The register to access.</param>
        /// <returns>A new instance of the <see cref="RM"/> class.</returns>
        public static RM FromRegister(Register reg)
        {
            byte modrm = (byte)(0xc0 | reg.RegisterCode());
            return new RM(new ReadOnlySpan<byte>(new byte[] { modrm }), reg.Size());
        }

        /// <summary>
        /// Creates a r/m8,16,32 to access memory address from the given register.
        /// </summary>
        /// <param name="reg">The address register.</param>
        /// <param name="size">The operand size.</param>
        /// <returns>A new instance of the <see cref="RM"/> class.</returns>
        public static RM Address(Register reg, int size = sizeof(uint))
        {
            byte modrm = 0;
            switch (reg)
            {
                case Register.EAX:
                    modrm = 0;
                    break;
                case Register.ECX:
                    modrm = 1;
                    break;
                case Register.EDX:
                    modrm = 2;
                    break;
                case Register.EBX:
                    modrm = 3;
                    break;
                case Register.ESI:
                    modrm = 6;
                    break;
                case Register.EDI:
                    modrm = 7;
                    break;
                default:
                    throw new NotSupportedException($"Register {reg} is not supported.");
            }

            return new RM(new ReadOnlySpan<byte>(new byte[] { modrm }), size);
        }

        /// <summary>
        /// Creates a r/m8,16,32 to access memory address from the given displacement.
        /// </summary>
        /// <param name="symbol">The symbol reference.</param>
        /// <param name="size">The operand size.</param>
        /// <returns>A new instance of the <see cref="RM"/> class.</returns>
        public static RM Address(string symbol, int size = sizeof(uint))
        {
            byte[] code = new byte[] { 0x05, 0, 0, 0, 0 };
            RelocationEntry rel = new RelocationEntry(symbol, 1, sizeof(uint), false);
            return new RM(code, size, rel, null);
        }

        /// <summary>
        /// Creates a r/m8,16,32 to access memory address from the given register and displacement.
        /// </summary>
        /// <param name="reg">The register base.</param>
        /// <param name="disp">The 8-bit displacement to add to the register.</param>
        /// <param name="label">The optional label for the displacement value.</param>
        /// <param name="size">The operand size.</param>
        /// <returns>A new instance of the <see cref="RM"/> class.</returns>
        public static RM Address(Register reg, sbyte disp, string? label, int size = sizeof(uint))
        {
            byte rm = 0;
            switch (reg)
            {
                case Register.EAX:
                    rm = 0;
                    break;
                case Register.ECX:
                    rm = 1;
                    break;
                case Register.EDX:
                    rm = 2;
                    break;
                case Register.EBX:
                    rm = 3;
                    break;
                case Register.EBP:
                    rm = 5;
                    break;
                case Register.ESI:
                    rm = 6;
                    break;
                case Register.EDI:
                    rm = 7;
                    break;
                default:
                    throw new NotImplementedException($"Register {reg} is either not implemented or not supported.");
            }

            return new RM(new byte[] { (byte)(0x40 | rm), (byte)disp }, size, null, label);
        }

        /// <summary>
        /// Copies the content to a destination span.
        /// </summary>
        /// <param name="destination">The destination span.</param>
        public void CopyTo(Span<byte> destination)
        {
            this.code.CopyTo(destination);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            byte modrm = this.code[0];
            int mod = modrm >> 6;
            byte rm = (byte)(modrm & 0x7);
            string operandSize = string.Empty;
            switch (this.size)
            {
                case sizeof(byte):
                    operandSize = "byte ptr ";
                    break;
                case sizeof(ushort):
                    operandSize = "word ptr ";
                    break;
            }

            switch (mod)
            {
                case 0:
                    switch (rm)
                    {
                        case 0:
                            return $"{operandSize}[eax]";
                        case 1:
                            return $"{operandSize}[ecx]";
                        case 2:
                            return $"{operandSize}[edx]";
                        case 3:
                            return $"{operandSize}[ebx]";
                        case 5:
                            {
                                string str = string.Empty;
                                if (this.Relocation != null)
                                {
                                    str = this.Relocation.Symbol;
                                }
                                else
                                {
                                    ReadOnlySpan<byte> disp32 = new ReadOnlySpan<byte>(this.code, 1, sizeof(uint));
                                    str = disp32.ToUInt32().ToString();
                                }

                                return $"{operandSize}[{str}]";
                            }

                        case 6:
                            return $"{operandSize}[esi]";
                        case 7:
                            return $"{operandSize}[edi]";
                        default:
                            return "TODO";
                    }

                case 1:
                    {
                        string regBase = string.Empty;
                        switch (rm)
                        {
                            case 0:
                                regBase = "[eax]";
                                break;
                            case 1:
                                regBase = "[ecx]";
                                break;
                            case 2:
                                regBase = "[edx]";
                                break;
                            case 3:
                                regBase = "[ebx]";
                                break;
                            case 5:
                                regBase = "[ebp]";
                                break;
                            case 6:
                                regBase = "[esi]";
                                break;
                            case 7:
                                regBase = "[edi]";
                                break;
                            default:
                                return "TODO";
                        }

                        string disp8Str = string.Empty;
                        if (this.label != null)
                        {
                            disp8Str = this.label!;
                        }
                        else
                        {
                            sbyte disp8 = (sbyte)this.code[1];
                            disp8Str = disp8.ToString();
                        }

                        return string.Concat(operandSize, disp8Str, regBase);
                    }

                case 2:
                    break;
                default:
                    return rm.FromRegisterCode(this.size).ToString().ToLowerInvariant();
            }

            return "<<TODO>>";
        }
    }
}