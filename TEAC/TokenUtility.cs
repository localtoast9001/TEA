//-----------------------------------------------------------------------
// <copyright file="TokenUtility.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Extension methods for the <see cref="Token"/> class.
    /// </summary>
    internal static class TokenUtility
    {
        /// <summary>
        /// Checks if the token is the given keyword.
        /// </summary>
        /// <param name="token">The token to check.</param>
        /// <param name="keyword">The expected keyword.</param>
        /// <returns>True if a match; otherwise, false.</returns>
        public static bool Is(this Token? token, Keyword keyword)
        {
            KeywordToken? keyTok = token as KeywordToken;
            if (keyTok == null)
            {
                return false;
            }

            return keyTok.Value == keyword;
        }

        /// <summary>
        /// Checks if the given token is a relational operator.
        /// </summary>
        /// <param name="token">The token to check.</param>
        /// <returns>true if a relational token; othrewise, false.</returns>
        public static bool IsRelationalOperator(this Token? token)
        {
            return token.Is(Keyword.GreaterThan) ||
                token.Is(Keyword.GreaterThanOrEquals) ||
                token.Is(Keyword.LessThan) ||
                token.Is(Keyword.LessThanOrEquals) ||
                token.Is(Keyword.NotEqual) ||
                token.Is(Keyword.Equals);
        }
    }
}