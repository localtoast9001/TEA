//-----------------------------------------------------------------------
// <copyright file="Elf32ModuleWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.IO;

    /// <summary>
    /// Module writer for the elf32-little format.
    /// </summary>
    public class Elf32ModuleWriter : ModuleWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32ModuleWriter"/> class.
        /// </summary>
        /// <param name="path">The output file path.</param>
        public Elf32ModuleWriter(string path)
        {
            this.Output = new FileStream(path, FileMode.Create);
        }

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        internal Stream Output { get; }

        /// <inheritdoc/>
        public override bool Write(Module module)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.Output.Close();
        }
    }
}