using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class TokenReader : IDisposable
    {
        private static readonly Dictionary<string, Keyword> IdentifierKeywordMap = new Dictionary<string, Keyword>(StringComparer.Ordinal)
        {
            { "abstract", Keyword.Abstract },
            { "and", Keyword.And },
            { "array", Keyword.Array },
            { "begin", Keyword.Begin },
            { "case", Keyword.Case },
            { "class", Keyword.Class },
            { "const", Keyword.Const },
            { "constructor", Keyword.Constructor },
            { "delete", Keyword.Delete },
            { "destructor", Keyword.Destructor },
            { "div", Keyword.Div },
            { "do", Keyword.Do },
            { "downto", Keyword.DownTo },
            { "else", Keyword.Else },
            { "end", Keyword.End },
            { "extern", Keyword.Extern },
            { "false", Keyword.False },
            { "for", Keyword.For },
            { "function", Keyword.Function },
            { "if", Keyword.If },
            { "in", Keyword.In },
            { "inherited", Keyword.Inherited },
            { "mod", Keyword.Mod },
            { "namespace", Keyword.Namespace },
            { "new", Keyword.New },
            { "nil", Keyword.Nil },
            { "not", Keyword.Not },
            { "of", Keyword.Of },
            { "or", Keyword.Or },
            { "packed", Keyword.Packed },
            { "private", Keyword.Private },
            { "procedure", Keyword.Procedure },
            { "program", Keyword.Program },
            { "protected", Keyword.Protected },
            { "public", Keyword.Public },
            { "record", Keyword.Record },	
            { "repeat", Keyword.Repeat },
            { "set", Keyword.Set },
            { "static", Keyword.Static },
            { "then", Keyword.Then },
            { "to", Keyword.To },
            { "true", Keyword.True },
            { "type", Keyword.Type },
            { "until", Keyword.Until },
            { "uses", Keyword.Uses },
            { "var", Keyword.Var },
            { "virtual", Keyword.Virtual },
            { "while", Keyword.While }
        };

        private string path;
        private StreamReader inner;
        private int line;
        private int column;
        private Token next;
        private MessageLog log;

        public TokenReader(string path, MessageLog log)
        {
            this.path = path;
            this.inner = new StreamReader(path, Encoding.ASCII);
            this.line = 1;
            this.column = 1;
            this.log = log;
        }

        public string Path { get { return this.path; } }
        public int Line { get { return this.line; } }
        public int Column { get { return this.column; } }

        public void Close()
        {
            this.inner.Close();
        }

        public void Dispose()
        {
            this.inner.Dispose();
        }

        public Token Read()
        {
            if (this.next == null)
            {
                this.Peek();
            }

            Token result = this.next;
            this.next = null;
            return result;
        }

        public Token Peek()
        {
            if (this.next == null)
            {
                this.next = this.InnerRead();
            }

            return this.next;
        }

        private Token InnerRead()
        {
            Token token = this.EatCommentsAndWhiteSpace();
            if (token != null)
            {
                return token;
            }

            int ch = this.inner.Peek();
            if (ch <= 0)
            {
                return null;
            }

            if (char.IsDigit((char)ch))
            {
                return this.ReadNumber();
            }
            else if (char.IsLetter((char)ch) || ch == (int)'_')
            {
                return this.ReadIdentifierOrKeyword();
            }
            else if (ch == (int)'\'' || ch == (int)'#')
            {
                return this.ReadStringLiteral();
            }
            else
            {
                int lineStart = this.line;
                int colStart = this.column;
                switch ((char)ch)
                {
                    case '.':
                        {
                            this.ReadChar();
                            ch = this.inner.Peek();
                            if (ch > 0 && char.IsDigit((char)ch))
                            {
                                return this.ReadDecimalAfterDot();
                            }
                            else
                            {
                                return new KeywordToken(Keyword.Dot, this.path, lineStart, colStart);
                            }
                        }
                    case ';':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.SemiColon, this.path, lineStart, colStart);
                        }
                    case ':':
                        {
                            this.ReadChar();
                            ch = this.inner.Peek();
                            if (ch == (int)'=')
                            {
                                this.ReadChar();
                                return new KeywordToken(Keyword.Assign, this.path, lineStart, colStart);
                            }

                            return new KeywordToken(Keyword.Colon, this.path, lineStart, colStart);
                        }
                    case '=':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Equals, this.path, lineStart, colStart);
                        } 
                    case ',':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Comma, this.path, lineStart, colStart);
                        }
                    case '<':
                        {
                            this.ReadChar();
                            ch = this.inner.Peek();
                            if (ch == (int)'>')
                            {
                                this.ReadChar();
                                return new KeywordToken(Keyword.NotEqual, this.path, lineStart, colStart);
                            }
                            else if (ch == (int)'=')
                            {
                                this.ReadChar();
                                return new KeywordToken(Keyword.LessThanOrEquals, this.path, lineStart, colStart);
                            }

                            return new KeywordToken(Keyword.LessThan, this.path, lineStart, colStart);
                        }
                    case '>':
                        {
                            this.ReadChar();
                            ch = this.inner.Peek();
                            if (ch == (int)'=')
                            {
                                this.ReadChar();
                                return new KeywordToken(Keyword.GreaterThanOrEquals, this.path, lineStart, colStart);
                            }

                            return new KeywordToken(Keyword.GreaterThan, this.path, lineStart, colStart);
                        }
                    case '@':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Address, this.path, lineStart, colStart);
                        }
                    case '*':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Star, this.path, lineStart, colStart);
                        }
                    case '/':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Slash, this.path, lineStart, colStart);
                        }
                    case '+':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Plus, this.path, lineStart, colStart);
                        }
                    case '-':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Minus, this.path, lineStart, colStart);
                        }
                    case '^':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.Pointer, this.path, lineStart, colStart);
                        }
                    case ')':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.RightParen, this.path, lineStart, colStart);
                        }
                    case '[':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.LeftBracket, this.path, lineStart, colStart);
                        }
                    case ']':
                        {
                            this.ReadChar();
                            return new KeywordToken(Keyword.RightBracket, this.path, lineStart, colStart);
                        }
                    default:
                        {
                            string message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.TokenReader_UnsupportedChar,
                                (char)ch);
                            this.ReadChar();
                            this.log.Write(new Message(this.path, this.line, this.column, Severity.Error, message));
                            return null;
                        }
                }
            }
        }

        private Token ReadNumber()
        {
            int intPart = 0;
            int lineStart = this.line;
            int colStart = this.column;
            int ch = this.inner.Peek();
            while (ch > 0 && char.IsDigit((char)ch))
            {
                intPart *= 10;
                intPart += ch - (int)'0';
                this.ReadChar();
                ch = this.inner.Peek();
            }

            if (ch == (int)'.')
            {
                this.ReadChar();
                decimal decimalPart = this.InnerReadDecimalAfterDot();
                return new LiteralToken((decimal)intPart + decimalPart, this.path, lineStart, colStart);
            }
            else
            {
                return new LiteralToken(intPart, this.path, lineStart, colStart);
            }
        }

        private Token ReadDecimalAfterDot()
        {
            int lineStart = this.line;
            int colStart = this.column;
            decimal value = this.InnerReadDecimalAfterDot();
            return new LiteralToken(value, this.path, lineStart, colStart);
        }

        private decimal InnerReadDecimalAfterDot()
        {
            int ch = this.inner.Peek();
            decimal factor = 0.1M;
            decimal result = 0;
            while (ch > 0 && char.IsDigit((char)ch))
            {
                int digit = ch - (int)'0';
                result += (decimal)digit * factor;
                factor *= 0.1M;
                this.ReadChar();
                ch = this.inner.Peek();
            }

            return result;
        }

        private Token ReadStringLiteral()
        {
            StringBuilder sb = new StringBuilder();
            int startLine = this.line;
            int startColumn = this.column;
            int ch = this.inner.Peek();
            while (ch > 0)
            {
                char chr = (char)ch;
                if (chr == '\'')
                {
                    this.ReadChar();
                    ch = this.inner.Peek();
                    while (ch > 0)
                    {
                        if (ch == '\'')
                        {
                            this.ReadChar();
                            ch = this.inner.Peek();
                            if (ch <= 0 || ch != (int)'\'')
                            {
                                break;
                            }
                        }

                        sb.Append((char)ch);
                        this.ReadChar();
                        ch = this.inner.Peek();
                    }

                    if (ch <= 0)
                    {
                        this.log.Write(
                            new Message(
                                this.path, 
                                startLine, 
                                startColumn, 
                                Severity.Error, 
                                Properties.Resources.TokenReader_StringNotTerminated));
                        return null;
                    }
                }
                else if (chr == '#')
                {
                    this.ReadChar();
                    ch = this.ReadChar();
                    int charValue = 0;
                    if (ch <= 0 || !char.IsDigit((char)ch))
                    {
                        log.Write(new Message(
                            this.path, this.line, this.column, Severity.Error, Properties.Resources.TokenReader_NoDigitAfterHash));
                        return null;
                    }

                    charValue = ch - (int)'0';
                    ch = this.inner.Peek();
                    while (ch >= 0 && char.IsDigit((char)ch))
                    {
                        charValue *= 10;
                        charValue += ch - (int)'0';
                        this.ReadChar();
                        ch = this.inner.Peek();
                    }

                    sb.Append((char)charValue);
                }
                else
                {
                    break;
                }
            }

            return new LiteralToken(sb.ToString(), this.path, startLine, startColumn);
        }

        private Token ReadIdentifierOrKeyword()
        {
            int startLine = this.line;
            int startColumn = this.column;
            StringBuilder element = new StringBuilder();
            int ch = this.inner.Peek();
            while (ch > 0)
            {
                char chr = (char)ch;
                if (chr == '_' || char.IsLetterOrDigit(chr))
                {
                    element.Append(chr);
                }
                else
                {
                    break;
                }

                this.ReadChar();
                ch = this.inner.Peek();
            }

            Keyword keyword = Keyword.And;
            if (IdentifierKeywordMap.TryGetValue(element.ToString(), out keyword))
            {
                return new KeywordToken(keyword, this.path, startLine, startColumn);
            }
            else
            {
                return new IdentifierToken(element.ToString(), this.path, startLine, startColumn);
            }
        }

        private Token EatCommentsAndWhiteSpace()
        {
            while (true)
            {
                this.EatWhiteSpace();
                int ch = this.inner.Peek();
                if (ch <= 0)
                {
                    return null;
                }

                if ((char)ch == '(')
                {
                    this.ReadChar();
                    ch = this.inner.Peek();
                    if (ch <= 0 || (char)ch != '*')
                    {
                        return new KeywordToken(Keyword.LeftParen, this.path, this.line, this.column);
                    }
                }
                else if ((char)ch == '{')
                {
                    this.ReadChar();
                }
                else
                {
                    return null;
                }

                bool terminated = false;
                int lineStart = this.line;
                int columnStart = this.column;
                ch = this.inner.Peek();
                while (ch > 0)
                {
                    this.ReadChar();
                    if ((char)ch == '}')
                    {
                        terminated = true;
                        break;
                    }
                    else if ((char)ch == '*')
                    {
                        ch = this.inner.Peek();
                        if (ch == (int)')')
                        {
                            this.ReadChar();
                            terminated = true;
                            break;
                        }
                    }

                    ch = this.inner.Peek();
                }

                if (!terminated)
                {
                    log.Write(new Message(
                        this.path, 
                        lineStart, 
                        columnStart, 
                        Severity.Error, 
                        Properties.Resources.TokenReader_CommentNotTerminated));
                }
            }
        }

        private void EatWhiteSpace()
        {
            int ch = this.inner.Peek();
            while (ch > 0 && char.IsWhiteSpace((char)ch))
            {
                this.ReadChar();
                ch = this.inner.Peek();
            }
        }

        private int ReadChar()
        {
            int ch = this.inner.Read();
            if (ch > 0)
            {
                if((char)ch == '\n')
                {
                    this.column = 0;
                    this.line++;
                }

                this.column++;
            }

            return ch;
        }
    }
}
