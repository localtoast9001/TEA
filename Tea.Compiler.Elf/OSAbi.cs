//-----------------------------------------------------------------------
// <copyright file="OSAbi.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// OS ABI identifier in the ELF header.
    /// </summary>
    public enum OSAbi : byte
    {
        /// <summary>
        /// System-V OS ABI.
        /// </summary>
        SystemV,

        /// <summary>
        /// HP UX OS ABI.
        /// </summary>
        HPUX,

        /// <summary>
        /// Net BSD ABI.
        /// </summary>
        NetBsd,

        /// <summary>
        /// Linux ABI.
        /// </summary>
        Linux,

        /// <summary>
        /// GNU Hurd ABI.
        /// </summary>
        GnuHurd,

        /// <summary>
        /// Solaris ABI.
        /// </summary>
        Solaris,

        /// <summary>
        /// AIX ABI.
        /// </summary>
        Aix,

        /// <summary>
        /// Irix ABI.
        /// </summary>
        Irix,

        /// <summary>
        /// FreeBSD ABI.
        /// </summary>
        FreeBsd,

        /// <summary>
        /// Tru64 ABI.
        /// </summary>
        Tru64,

        /// <summary>
        /// Novell Modesto ABI.
        /// </summary>
        NovellModesto,

        /// <summary>
        /// OpenBSD ABI.
        /// </summary>
        OpenBsd,

        /// <summary>
        /// Open VMS ABI.
        /// </summary>
        OpenVms,

        /// <summary>
        /// Non-stop kernel ABI.
        /// </summary>
        NonStopKernel,

        /// <summary>
        /// AROS ABI.
        /// </summary>
        Aros,

        /// <summary>
        /// Fenix OS ABI.
        /// </summary>
        FenixOS,

        /// <summary>
        /// Nuxi Cloud ABI.
        /// </summary>
        NuxiCloudAbi,

        /// <summary>
        /// Stratus Technologies Open VOS ABI.
        /// </summary>
        StratusTechnologiesOpenVos,
    }
}