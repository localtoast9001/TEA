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

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralToken"/> class.
        /// </summary>
        /// <param name="value">The literal value.</param>
        /// <param name="path">The source file path.</param>
        /// <param name="line">The source file line.</param>
        /// <param name="column">The source file column.</param>
        public LiteralToken(object value, string path, int line, int column)
            : base(path, line, column)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the literal value.
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.value?.ToString() ?? string.Empty;
        }
    }
}