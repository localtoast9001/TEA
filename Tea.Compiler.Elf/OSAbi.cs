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
        SystemV,
        HPUX,
        NetBsd,
        Linux,
        GnuHurd,
        Solaris,
        Aix,
        Irix,
        FreeBsd,
        Tru64,
        NovellModesto,
        OpenBsd,
        OpenVms,
        NonStopKernel,
        Aros,
        FenixOS,
        NuxiCloudAbi,
        StratusTechnologiesOpenVos,
    }
}