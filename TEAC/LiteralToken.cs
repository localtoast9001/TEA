//-----------------------------------------------------------------------
// <copyright file="LiteralToken.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// A token for a literal value lexical element.
    /// </summary>
    internal class LiteralToken : Token
    {
        private object value;

        public LiteralToken(object value, string path, int line, int column)
            : base(path, line, column)
        {
            this.value = value;
        }

        public object Value { get { return this.value; } }

        public override string ToString()
        {
            return this.value?.ToString() ?? string.Empty;
        }
    }
}