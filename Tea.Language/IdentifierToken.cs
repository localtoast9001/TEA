//-----------------------------------------------------------------------
// <copyright file="IdentifierToken.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Identifier leexical element.
    /// </summary>
    public class IdentifierToken : Token
    {
        private readonly string identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifierToken"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="path">The file path.</param>
        /// <param name="line">The line in the source file.</param>
        /// <param name="column">The column in the source file.</param>
        public IdentifierToken(string identifier, string path, int line, int column)
            : base(path, line, column)
        {
            this.identifier = identifier;
        }

        /// <summary>
        /// Gets the identifier value.
        /// </summary>
        public string Value
        {
            get { return this.identifier; }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.identifier;
        }
    }
}