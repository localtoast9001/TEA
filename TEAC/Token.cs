//-----------------------------------------------------------------------
// <copyright file="Token.cs" company="Jon Rowlett">
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
    /// Lexical element produced by the <see cref="TokenReader"/> class.
    /// </summary>
    internal abstract class Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="path">The source file path.</param>
        /// <param name="line">The line where the token starts.</param>
        /// <param name="column">The first column where the token starts.</param>
        protected Token(string path, int line, int column)
        {
            this.Path = path;
            this.Line = line;
            this.Column = column;
        }

        /// <summary>
        /// Gets the path to the source file.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the line of the start of the token.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the column for the first character in the token.
        /// </summary>
        public int Column { get; }
    }
}
