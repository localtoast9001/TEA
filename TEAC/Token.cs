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
        public string Path { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }

        protected Token(string path, int line, int column)
        {
            this.Path = path;
            this.Line = line;
            this.Column = column;
        }
    }
}
