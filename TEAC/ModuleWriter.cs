//-----------------------------------------------------------------------
// <copyright file="ModuleWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Base class of writers that serialize modules to an output format.
    /// </summary>
    internal abstract class ModuleWriter : IDisposable
    {
        /// <summary>
        /// Finalizes an instance of the <see cref="ModuleWriter"/> class.
        /// </summary>
        ~ModuleWriter()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Writes the given module to output.
        /// </summary>
        /// <param name="module">The module to write.</param>
        /// <returns>true if written successfully; otherwise, false.</returns>
        public abstract bool Write(Module module);

        /// <summary>
        /// Closes the writer.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Disposes the instance, freeing both managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        /// <param name="disposing">True to dispose both managed and unmanaged resources; false to only dispose unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
