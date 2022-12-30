//-----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.X86
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        private static readonly int[] RegisterSizes = new[]
        {
            1, // AL
            1, // BL
            1, // CL
            1, // DL
            1, // AH
            1, // BH
            1, // CH
            1, // DH
            2, // AX
            2, // BX
            2, // CX
            2, // DX
            2, // SP
            2, // BP
            2, // SI
            2, // DI
            4, // EAX
            4, // EBX
            4, // ECX
            4, // EDX
            4, // ESP
            4, // EBP
            4, // ESI
            4, // EDI
        };

        private static readonly byte[] RegisterCodes = new byte[]
        {
            0, // AL
            3, // BL
            1, // CL
            2, // DL
            4, // AH
            7, // BH
            5, // CH
            6, // DH
            0, // AX
            3, // BX
            1, // CX
            2, // DX
            4, // SP
            5, // BP
            6, // SI
            7, // DI
            0, // EAX
            3, // EBX
            1, // ECX
            2, // EDX
            4, // ESP
            5, // EBP
            6, // ESI
            7, // EDI
        };

        private static readonly Register[] ByteRegisterCodes = new Register[]
        {
            Register.AL,
            Register.CL,
            Register.DL,
            Register.BL,
            Register.AH,
            Register.CH,
            Register.DH,
            Register.BH,
        };

        private static readonly Register[] WordRegisterCodes = new Register[]
        {
            Register.AX,
            Register.CX,
            Register.DX,
            Register.BX,
            Register.SP,
            Register.BP,
            Register.SI,
            Register.DI,
        };

        private static readonly Register[] DWordRegisterCodes = new Register[]
        {
            Register.EAX,
            Register.ECX,
            Register.EDX,
            Register.EBX,
            Register.ESP,
            Register.EBP,
            Register.ESI,
            Register.EDI,
        };

        /// <summary>
        /// Gets the size of the register in bytes.
        /// </summary>
        /// <param name="reg">The register.</param>
        /// <returns>The size ain bytes of the register.</returns>
        public static int Size(this Register reg)
        {
            return RegisterSizes[(int)reg];
        }

        /// <summary>
        /// Gets the register code to add to the opcode for a single-byte instruction.
        /// </summary>
        /// <param name="reg">The register.</param>
        /// <returns>The code from 0-7 to add to the instruction opcode.</returns>
        public static byte RegisterCode(this Register reg)
        {
            return RegisterCodes[(int)reg];
        }

        /// <summary>
        /// Gets the register from the register code.
        /// </summary>
        /// <param name="offset">0-7 for the code.</param>
        /// <param name="size">The register size in bytes.</param>
        /// <returns>The decoded register.</returns>
        public static Register FromRegisterCode(this byte offset, int size)
        {
            if (offset > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            switch (size)
            {
                case sizeof(byte):
                    return ByteRegisterCodes[offset];
                case sizeof(ushort):
                    return WordRegisterCodes[offset];
                case sizeof(uint):
                    return DWordRegisterCodes[offset];
                default:
                    throw new ArgumentOutOfRangeException(nameof(size));
            }
        }

        /// <summary>
        /// Decodes bytes to a uint32.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <returns>The decoded value.</returns>
        internal static uint ToUInt32(this ReadOnlySpan<byte> data)
        {
            if (data.Length != sizeof(uint))
            {
                throw new ArgumentOutOfRangeException(nameof(data));
            }

            return (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
        }

        /// <summary>
        /// Decodes bytes to an int32.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <returns>The decoded value.</returns>
        internal static int ToInt32(this ReadOnlySpan<byte> data)
        {
            return (int)ToUInt32(data);
        }

        /// <summary>
        /// Decodes bytes to a uint16.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <returns>The decoded value.</returns>
        internal static ushort ToUInt16(this ReadOnlySpan<byte> data)
        {
            if (data.Length != sizeof(short))
            {
                throw new ArgumentOutOfRangeException(nameof(data));
            }

            return (ushort)(data[0] | (data[1] << 8));
        }

        /// <summary>
        /// Decodes bytes to a int16.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <returns>The decoded value.</returns>
        internal static short ToInt16(this ReadOnlySpan<byte> data)
        {
            return (short)ToUInt16(data);
        }

        /// <summary>
        /// Encodes data to a byte array.
        /// </summary>
        /// <param name="data">The the data to encode.</param>
        /// <returns>The encoded data.</returns>
        internal static byte[] ToBytes(this ushort data)
        {
            return new byte[]
            {
                (byte)data,
                (byte)(data >> 8),
            };
        }

        /// <summary>
        /// Encodes data to a byte array.
        /// </summary>
        /// <param name="data">The the data to encode.</param>
        /// <returns>The encoded data.</returns>
        internal static byte[] ToBytes(this short data)
        {
            return ToBytes((ushort)data);
        }

        /// <summary>
        /// Encodes data to a byte array.
        /// </summary>
        /// <param name="data">The the data to encode.</param>
        /// <returns>The encoded data.</returns>
        internal static byte[] ToBytes(this uint data)
        {
            return new byte[]
            {
                (byte)data,
                (byte)(data >> 8),
                (byte)(data >> 16),
                (byte)(data >> 24),
            };
        }

        /// <summary>
        /// Encodes data to a byte array.
        /// </summary>
        /// <param name="data">The the data to encode.</param>
        /// <returns>The encoded data.</returns>
        internal static byte[] ToBytes(this int data)
        {
            return ToBytes((uint)data);
        }
    }
}