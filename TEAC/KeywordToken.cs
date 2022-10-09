//-----------------------------------------------------------------------
// <copyright file="KeywordToken.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    internal class KeywordToken : Token
    {
        private Keyword keyword;

        public KeywordToken(Keyword keyword, string path, int line, int column)
            : base(path, line, column)
        {
            this.keyword = keyword;
        }

        public Keyword Value
        {
            get { return this.keyword; }
        }

        public override string ToString()
        {
            return this.keyword.ToString().ToLowerInvariant();
        }
    }
}