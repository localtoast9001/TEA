//-----------------------------------------------------------------------
// <copyright file="KeywordToken.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Language token for a keyword.
    /// </summary>
    internal class KeywordToken : Token
    {
        private Keyword keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeywordToken"/> class.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="path">The source path.</param>
        /// <param name="line">The source line.</param>
        /// <param name="column">The source column.</param>
        public KeywordToken(Keyword keyword, string path, int line, int column)
            : base(path, line, column)
        {
            this.keyword = keyword;
        }

        /// <summary>
        /// Gets the keyword value.
        /// </summary>
        public Keyword Value
        {
            get { return this.keyword; }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.keyword.ToString().ToLowerInvariant();
        }
    }
}