using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
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

    internal static class TokenUtility
    {
        public static bool Is(this Token? token, Keyword keyword)
        {
            KeywordToken? keyTok = token as KeywordToken;
            if (keyTok == null)
            {
                return false;
            }

            return keyTok.Value == keyword;
        }

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

    internal class IdentifierToken : Token
    {
        private string identifier;

        public IdentifierToken(string identifier, string path, int line, int column)
            : base(path, line, column)
        {
            this.identifier = identifier;
        }

        public string Value { get { return this.identifier; } }

        public override string ToString()
        {
            return this.identifier;
        }
    }

    public enum Keyword
    {
        Dot,
        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        SemiColon,
        Colon,
        Equals,
        Assign,
        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals,
        NotEqual,
        Comma,
        Address,
        Pointer,
        Star,
        Plus,
        Minus,
        Slash,
        Abstract,
        And,
        Array,
        Begin,
        Case,
        Class,
        Const,
        Constructor,
        Delete,
        Destructor,
        Div,
        Do,
        DownTo,
        Else,
        End,
        Extern,
        False,
        For,
        Function,
        If,
        In,
        Inherited,
        Interface,
        Mod,
        Namespace,
        New,
        Nil,
        Not,
        Of,
        Or,
        Packed,
        Private,
        Procedure,
        Program,
        Protected,
        Public,
        Record,	
        Repeat,
        Set,
        Static,
        Then,
        To,
        True,
        Type,
        Until,
        Uses,
        Var,
        Virtual,
        While
    }

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
