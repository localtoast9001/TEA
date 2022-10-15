//-----------------------------------------------------------------------
// <copyright file="Parser.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Parses a source code into parse trees.
    /// </summary>
    public class Parser
    {
        private MessageLog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="log">The message log.</param>
        public Parser(MessageLog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Tries to parse the sequence of tokens produced by the reader into a <see cref="ProgramUnit"/>.
        /// </summary>
        /// <param name="reader">The token reader.</param>
        /// <param name="result">On success, receives the parsed result.</param>
        /// <returns>True if the source can be parsed with no errors; otherwise, false.</returns>
        public bool TryParse(
            TokenReader reader,
            out ProgramUnit? result)
        {
            result = null;
            Token? tok = reader.Peek();
            if (tok == null)
            {
                return false;
            }

            ProgramUnit programUnit = new ProgramUnit(tok);
            if (tok.Is(Keyword.Namespace))
            {
                reader.Read();
                string? namespaceDecl = null;
                if (!this.ParseFullNameDeclaration(reader, out namespaceDecl))
                {
                    return false;
                }

                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }

                programUnit.Namespace = namespaceDecl ?? string.Empty;
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Uses))
            {
                reader.Read();
                if (!this.ParseFullNameDeclaration(reader, out string? usesRef))
                {
                    return false;
                }

                programUnit.AddUses(usesRef);
                tok = reader.Peek();
                while (tok.Is(Keyword.Comma))
                {
                    reader.Read();
                    if (!this.ParseFullNameDeclaration(reader, out usesRef))
                    {
                        return false;
                    }

                    programUnit.AddUses(usesRef);
                    tok = reader.Peek();
                }

                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Type))
            {
                if (!this.ParseTypeBlock(reader, programUnit))
                {
                    return false;
                }
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Var))
            {
                VarBlock? globalVars = null;
                if (!this.ParseVarBlock(reader, true, out globalVars))
                {
                    return false;
                }

                programUnit.Variables = globalVars;
            }

            tok = reader.Peek();
            while (
                tok.Is(Keyword.Function) ||
                tok.Is(Keyword.Procedure) ||
                tok.Is(Keyword.Constructor) ||
                tok.Is(Keyword.Destructor))
            {
                if (!this.ParseMethodDefinition(reader, out MethodDefinition? methodDef))
                {
                    return false;
                }

                programUnit.AddMethod(methodDef!);
                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }

                tok = reader.Peek();
            }

            bool passed = this.ExpectEndOfFile(reader);
            if (passed)
            {
                result = programUnit;
            }

            return passed;
        }

        private bool ParseClassDeclaration(IdentifierToken typeName, TokenReader reader, out TypeDeclaration? type)
        {
            type = null;
            Token? tok = reader.Peek();
            bool isStatic = false;
            if (tok.Is(Keyword.Static))
            {
                isStatic = true;
                reader.Read();
                tok = reader.Peek();
            }

            if (!this.Expect(reader, Keyword.Class))
            {
                return false;
            }

            string? baseType = null;
            tok = reader.Peek();
            if (tok.Is(Keyword.LeftParen))
            {
                reader.Read();

                if (!this.ParseFullNameDeclaration(reader, out baseType))
                {
                    return false;
                }

                if (!this.Expect(reader, Keyword.RightParen))
                {
                    return false;
                }
            }

            ClassDeclaration classDecl = new ClassDeclaration(
                typeName,
                typeName.Value,
                baseType,
                isStatic);

            tok = reader.Peek();
            if (tok.Is(Keyword.Interface))
            {
                reader.Read();
                if (!this.Expect(reader, Keyword.LeftParen))
                {
                    return false;
                }

                string? interfaceName = null;
                if (!this.ParseFullNameDeclaration(reader, out interfaceName))
                {
                    return false;
                }

                classDecl.AddInterface(interfaceName!);
                tok = reader.Peek();
                while (tok.Is(Keyword.Comma))
                {
                    reader.Read();
                    if (!this.ParseFullNameDeclaration(reader, out interfaceName))
                    {
                        return false;
                    }

                    classDecl.AddInterface(interfaceName!);
                    tok = reader.Peek();
                }

                if (!this.Expect(reader, Keyword.RightParen))
                {
                    return false;
                }
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Public))
            {
                reader.Read();
                tok = reader.Peek();
                while (
                    tok.Is(Keyword.Procedure) ||
                    tok.Is(Keyword.Function) ||
                    tok.Is(Keyword.Static) ||
                    tok.Is(Keyword.Virtual) ||
                    tok.Is(Keyword.Abstract) ||
                    tok.Is(Keyword.Constructor) ||
                    tok.Is(Keyword.Destructor))
                {
                    MethodDeclaration? methodDecl = null;
                    if (!this.ParseMethodDeclaration(reader, out methodDecl))
                    {
                        return false;
                    }

                    classDecl.AddPublicMethod(methodDecl!);
                    if (!this.Expect(reader, Keyword.SemiColon))
                    {
                        return false;
                    }

                    tok = reader.Peek();
                }
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Protected))
            {
                reader.Read();
                tok = reader.Peek();
                while (
                    tok.Is(Keyword.Procedure) ||
                    tok.Is(Keyword.Function) ||
                    tok.Is(Keyword.Static) ||
                    tok.Is(Keyword.Virtual) ||
                    tok.Is(Keyword.Abstract) ||
                    tok.Is(Keyword.Constructor) ||
                    tok.Is(Keyword.Destructor))
                {
                    MethodDeclaration? methodDecl = null;
                    if (!this.ParseMethodDeclaration(reader, out methodDecl))
                    {
                        return false;
                    }

                    classDecl.AddProtectedMethod(methodDecl!);
                    if (!this.Expect(reader, Keyword.SemiColon))
                    {
                        return false;
                    }

                    tok = reader.Peek();
                }
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Private))
            {
                reader.Read();
                tok = reader.Peek();
                while (
                    tok.Is(Keyword.Procedure) ||
                    tok.Is(Keyword.Function) ||
                    tok.Is(Keyword.Static) ||
                    tok.Is(Keyword.Constructor) ||
                    tok.Is(Keyword.Destructor))
                {
                    MethodDeclaration? methodDecl = null;
                    if (!this.ParseMethodDeclaration(reader, out methodDecl))
                    {
                        return false;
                    }

                    classDecl.AddPrivateMethod(methodDecl!);
                    if (!this.Expect(reader, Keyword.SemiColon))
                    {
                        return false;
                    }

                    tok = reader.Peek();
                }
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Var))
            {
                VarBlock? varBlock = null;
                if (!this.ParseVarBlock(reader, false, out varBlock))
                {
                    return false;
                }

                classDecl.Fields = varBlock;
            }

            if (!this.Expect(reader, Keyword.End))
            {
                return false;
            }

            type = classDecl;
            return true;
        }

        private bool ParseMethodDefinition(TokenReader reader, out MethodDefinition? method)
        {
            method = null;
            Token? tok = reader.Peek();
            Token? start = tok;
            bool isFunction = false;
            if (tok.Is(Keyword.Constructor))
            {
                return this.ParseConstructorDefinition(reader, out method);
            }

            if (tok.Is(Keyword.Destructor))
            {
                return this.ParseDestructorDefinition(reader, out method);
            }

            if (tok.Is(Keyword.Function))
            {
                isFunction = true;
                reader.Read();
            }
            else
            {
                if (!this.Expect(reader, Keyword.Procedure))
                {
                    return false;
                }
            }

            string? methodName = null;
            if (!this.ParseFullNameDeclaration(reader, out methodName))
            {
                return false;
            }

            MethodDefinition methodDef = new MethodDefinition(
                start!,
                methodName!);

            if (!this.Expect(reader, Keyword.LeftParen))
            {
                return false;
            }

            tok = reader.Peek();
            while (!tok.Is(Keyword.RightParen))
            {
                ParameterDeclaration? parameter = null;
                if (!this.ParseParameterDeclaration(reader, out parameter))
                {
                    return false;
                }

                methodDef.AddParameter(parameter!);

                tok = reader.Peek();
                if (tok.Is(Keyword.SemiColon))
                {
                    reader.Read();
                    tok = reader.Peek();
                }
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                return false;
            }

            if (isFunction)
            {
                if (!this.Expect(reader, Keyword.Colon))
                {
                    return false;
                }

                TypeReference? returnType = null;
                if (!this.ParseTypeReference(reader, out returnType))
                {
                    return false;
                }

                methodDef.ReturnType = returnType;
            }

            if (!this.Expect(reader, Keyword.SemiColon))
            {
                return false;
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Extern))
            {
                reader.Read();
                string? externalImpl = null;
                tok = reader.Read();
                LiteralToken? litTok = tok as LiteralToken;
                if (tok == null || litTok == null || !(litTok.Value is string))
                {
                    string message = string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Properties.Resources.Parser_Unexpected,
                        tok != null ? tok.ToString() : Properties.Resources.Parser_EndOfFile);
                    this.log.Write(
                        new Message(
                            reader.Path,
                            reader.Line,
                            reader.Column,
                            Severity.Error,
                            message));
                    return false;
                }

                externalImpl = (string)litTok.Value;
                methodDef.ExternImpl = externalImpl;
            }
            else
            {
                if (tok.Is(Keyword.Var))
                {
                    VarBlock? varBlock = null;
                    if (!this.ParseVarBlock(reader, true, out varBlock))
                    {
                        return false;
                    }

                    methodDef.LocalVariables = varBlock;
                }

                BlockStatement? body = null;
                if (!this.ParseBlockStatement(reader, out body))
                {
                    return false;
                }

                methodDef.Body = body;
            }

            method = methodDef;
            return true;
        }

        private bool ParseConstructorDefinition(TokenReader reader, out MethodDefinition? method)
        {
            Token? start = reader.Peek();
            method = null;
            if (!this.Expect(reader, Keyword.Constructor))
            {
                return false;
            }

            string? methodName = null;
            if (!this.ParseFullNameDeclaration(reader, out methodName))
            {
                return false;
            }

            methodName = methodName + ".constructor";

            MethodDefinition methodDef = new MethodDefinition(
                start!,
                methodName!);

            if (!this.Expect(reader, Keyword.LeftParen))
            {
                return false;
            }

            Token? tok = reader.Peek();
            while (!tok.Is(Keyword.RightParen))
            {
                ParameterDeclaration? parameter = null;
                if (!this.ParseParameterDeclaration(reader, out parameter))
                {
                    return false;
                }

                methodDef.AddParameter(parameter!);

                tok = reader.Peek();
                if (tok.Is(Keyword.SemiColon))
                {
                    reader.Read();
                    tok = reader.Peek();
                }
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                return false;
            }

            if (!this.Expect(reader, Keyword.SemiColon))
            {
                return false;
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Inherited))
            {
                reader.Read();
                if (!this.Expect(reader, Keyword.LeftParen))
                {
                    return false;
                }

                tok = reader.Peek();
                while (!tok.Is(Keyword.RightParen))
                {
                    Expression? arg = null;
                    if (!this.ParseExpression(reader, out arg))
                    {
                        return false;
                    }

                    methodDef.BaseConstructorArguments.Add(arg!);
                    tok = reader.Peek();
                    if (!tok.Is(Keyword.Comma))
                    {
                        break;
                    }
                    else
                    {
                        reader.Read();
                    }
                }

                if (!this.Expect(reader, Keyword.RightParen))
                {
                    return false;
                }

                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Var))
            {
                VarBlock? varBlock = null;
                if (!this.ParseVarBlock(reader, true, out varBlock))
                {
                    return false;
                }

                methodDef.LocalVariables = varBlock;
            }

            BlockStatement? body = null;
            if (!this.ParseBlockStatement(reader, out body))
            {
                return false;
            }

            methodDef.Body = body;
            method = methodDef;
            return true;
        }

        private bool ParseDestructorDefinition(TokenReader reader, out MethodDefinition? method)
        {
            method = null;
            Token? start = reader.Peek();
            if (!this.Expect(reader, Keyword.Destructor))
            {
                return false;
            }

            string? methodName = null;
            if (!this.ParseFullNameDeclaration(reader, out methodName))
            {
                return false;
            }

            methodName = methodName! + ".destructor";

            MethodDefinition methodDef = new MethodDefinition(
                start!,
                methodName!);

            if (!this.Expect(reader, Keyword.LeftParen))
            {
                return false;
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                return false;
            }

            if (!this.Expect(reader, Keyword.SemiColon))
            {
                return false;
            }

            Token? tok = reader.Peek();
            if (tok.Is(Keyword.Var))
            {
                VarBlock? varBlock = null;
                if (!this.ParseVarBlock(reader, true, out varBlock))
                {
                    return false;
                }

                methodDef.LocalVariables = varBlock;
            }

            BlockStatement? body = null;
            if (!this.ParseBlockStatement(reader, out body))
            {
                return false;
            }

            methodDef.Body = body;
            method = methodDef;
            return true;
        }

        private bool ParseBlockStatement(TokenReader reader, out BlockStatement? result)
        {
            result = null;
            Token? tok = reader.Peek();
            Token? start = tok;
            if (!this.Expect(reader, Keyword.Begin))
            {
                return false;
            }

            BlockStatement block = new BlockStatement(start!);
            tok = reader.Peek();
            while (!tok.Is(Keyword.End))
            {
                Statement? statement = null;
                if (!this.ParseStatement(reader, out statement))
                {
                    return false;
                }

                if (statement != null)
                {
                    block.AddStatement(statement);
                }

                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }

                tok = reader.Peek();
            }

            if (!this.Expect(reader, Keyword.End))
            {
                return false;
            }

            result = block;
            return true;
        }

        private bool ParseStatement(TokenReader reader, out Statement? result)
        {
            result = null;
            Token? tok = reader.Peek();
            Token? start = tok;

            if (tok.Is(Keyword.SemiColon))
            {
                return true; // empty statement.
            }

            if (tok.Is(Keyword.If))
            {
                return this.ParseIfStatement(reader, out result);
            }

            if (tok.Is(Keyword.While))
            {
                return this.ParseWhileStatement(reader, out result);
            }

            if (tok.Is(Keyword.Delete))
            {
                return this.ParseDeleteStatement(reader, out result);
            }

            if (tok.Is(Keyword.Begin))
            {
                BlockStatement? block = null;
                bool success = this.ParseBlockStatement(reader, out block);
                result = block;
                return success;
            }

            ReferenceExpression? lhs = null;
            if (!this.ParseReferenceExpression(reader, out lhs))
            {
                return false;
            }

            tok = reader.Peek();
            if (tok.Is(Keyword.Assign))
            {
                reader.Read();
                Expression? rhs = null;
                if (!this.ParseExpression(reader, out rhs))
                {
                    return false;
                }

                result = new AssignmentStatement(start!, lhs!, rhs!);
                return true;
            }
            else
            {
                result = new CallStatement(start!, lhs!);
                return true;
            }
        }

        private bool ParseDeleteStatement(TokenReader reader, out Statement? statement)
        {
            statement = null;
            Token? start = reader.Peek();
            if (!this.Expect(reader, Keyword.Delete))
            {
                return false;
            }

            Expression? operand = null;
            if (!this.ParseExpression(reader, out operand))
            {
                return false;
            }

            statement = new DeleteStatement(start!, operand!);
            return true;
        }

        private bool ParseIfStatement(TokenReader reader, out Statement? statement)
        {
            Token? start = reader.Peek();
            statement = null;

            if (!this.Expect(reader, Keyword.If))
            {
                return false;
            }

            IfStatement ifStatement = new IfStatement(start!);
            Expression? condition = null;
            if (!this.ParseExpression(reader, out condition))
            {
                return false;
            }

            ifStatement.Condition = condition;
            if (!this.Expect(reader, Keyword.Then))
            {
                return false;
            }

            Statement? trueStatement = null;
            if (!this.ParseStatement(reader, out trueStatement))
            {
                return false;
            }

            ifStatement.TrueStatement = trueStatement;
            Token? tok = reader.Peek();
            if (tok.Is(Keyword.Else))
            {
                reader.Read();
                Statement? falseStatement = null;
                if (!this.ParseStatement(reader, out falseStatement))
                {
                    return false;
                }

                ifStatement.FalseStatement = falseStatement;
            }

            statement = ifStatement;
            return true;
        }

        private bool ParseWhileStatement(TokenReader reader, out Statement? statement)
        {
            Token? start = reader.Peek();
            statement = null;
            if (!this.Expect(reader, Keyword.While))
            {
                return false;
            }

            WhileStatement? whileStatement = new WhileStatement(start!);
            Expression? condition = null;
            if (!this.ParseExpression(reader, out condition))
            {
                return false;
            }

            whileStatement.Condition = condition;
            if (!this.Expect(reader, Keyword.Do))
            {
                return false;
            }

            Statement? bodyStatement = null;
            if (!this.ParseStatement(reader, out bodyStatement))
            {
                return false;
            }

            whileStatement.BodyStatement = bodyStatement;
            statement = whileStatement;
            return true;
        }

        private bool ParseExpression(TokenReader reader, out Expression? result)
        {
            result = null;
            Token? start = reader.Peek();
            Expression? firstTerm = null;
            if (!this.ParseSimpleExpression(reader, out firstTerm))
            {
                return false;
            }

            Token? tok = reader.Peek();
            if (tok.IsRelationalOperator())
            {
                reader.Read();
                Expression? term = null;
                if (!this.ParseSimpleExpression(reader, out term))
                {
                    return false;
                }

                result = new RelationalExpression(start!, firstTerm!, ((KeywordToken)tok!).Value, term!);
            }
            else
            {
                result = firstTerm;
            }

            return true;
        }

        private bool ParseReferenceExpression(TokenReader reader, out ReferenceExpression? result)
        {
            ReferenceExpression? inner = null;
            Token? tok = reader.Peek();
            string? identifier = null;
            if (tok.Is(Keyword.Inherited))
            {
                inner = new InheritedReferenceExpression(tok!);
                reader.Read();
            }
            else
            {
                if (!this.ExpectIdentifier(reader, out identifier))
                {
                    result = null;
                    return false;
                }

                inner = new NamedReferenceExpression(tok!, identifier!);
            }

            tok = reader.Peek();
            while (true)
            {
                if (tok.Is(Keyword.LeftParen))
                {
                    reader.Read();
                    CallReferenceExpression callExpr = new CallReferenceExpression(tok!, inner!);
                    inner = callExpr;
                    tok = reader.Peek();
                    if (!tok.Is(Keyword.RightParen))
                    {
                        Expression? arg = null;
                        if (!this.ParseExpression(reader, out arg))
                        {
                            result = null;
                            return false;
                        }

                        callExpr.AddArgument(arg!);

                        tok = reader.Peek();
                        while (tok.Is(Keyword.Comma))
                        {
                            reader.Read();
                            if (!this.ParseExpression(reader, out arg))
                            {
                                result = null;
                                return false;
                            }

                            callExpr.AddArgument(arg!);

                            tok = reader.Peek();
                        }
                    }

                    if (!this.Expect(reader, Keyword.RightParen))
                    {
                        result = null;
                        return false;
                    }
                }
                else if (tok.Is(Keyword.LeftBracket))
                {
                    reader.Read();
                    Expression? index = null;
                    if (!this.ParseExpression(reader, out index))
                    {
                        result = null;
                        return false;
                    }

                    if (!this.Expect(reader, Keyword.RightBracket))
                    {
                        result = null;
                        return false;
                    }

                    inner = new ArrayIndexReferenceExpression(tok!, inner!, index!);
                }
                else if (tok.Is(Keyword.Dot))
                {
                    reader.Read();
                    if (!this.ExpectIdentifier(reader, out identifier))
                    {
                        result = null;
                        return false;
                    }

                    inner = new MemberReferenceExpression(tok!, inner!, identifier!);
                }
                else if (tok.Is(Keyword.Pointer))
                {
                    reader.Read();
                    inner = new DereferenceExpression(tok!, inner!);
                }
                else
                {
                    break;
                }

                tok = reader.Peek();
            }

            result = inner;
            return true;
        }

        private bool ParseSimpleExpression(TokenReader reader, out Expression? result)
        {
            Token? start = reader.Peek();
            result = null;
            Expression? outer = null;
            if (!this.ParseTermExpression(reader, out outer))
            {
                return false;
            }

            Token? tok = reader.Peek();
            while (tok.Is(Keyword.Plus) || tok.Is(Keyword.Minus) || tok.Is(Keyword.Or))
            {
                reader.Read();
                Expression? term = null;
                if (!this.ParseTermExpression(reader, out term))
                {
                    return false;
                }

                outer = new SimpleExpression(start!, outer!, ((KeywordToken)tok!).Value, term!);

                tok = reader.Peek();
            }

            result = outer;
            return true;
        }

        private bool ParseTermExpression(TokenReader reader, out Expression? result)
        {
            result = null;
            Token? start = reader.Peek();
            Expression? outer = null;
            if (!this.ParseFactorExpression(reader, out outer))
            {
                return false;
            }

            Token? tok = reader.Peek();
            while (tok.Is(Keyword.Star) || tok.Is(Keyword.Slash) || tok.Is(Keyword.Div) || tok.Is(Keyword.And) || tok.Is(Keyword.Mod))
            {
                reader.Read();
                Expression? factor = null;
                if (!this.ParseFactorExpression(reader, out factor))
                {
                    return false;
                }

                outer = new TermExpression(start!, outer!, ((KeywordToken)tok!).Value, factor!);
                tok = reader.Peek();
            }

            result = outer;
            return true;
        }

        private bool ParseFactorExpression(TokenReader reader, out Expression? result)
        {
            Token? tok = reader.Peek();
            if (tok.Is(Keyword.LeftParen))
            {
                reader.Read();
                if (!this.ParseExpression(reader, out result))
                {
                    return false;
                }

                if (!this.Expect(reader, Keyword.RightParen))
                {
                    return false;
                }

                return true;
            }

            if (tok.Is(Keyword.Not))
            {
                return this.ParseNotExpression(reader, out result);
            }

            if (tok.Is(Keyword.Minus))
            {
                return this.ParseNegativeExpression(reader, out result);
            }

            if (tok.Is(Keyword.New))
            {
                return this.ParseNewExpression(reader, out result);
            }

            if (tok.Is(Keyword.Nil))
            {
                reader.Read();
                result = new LiteralExpression(tok!, null);
                return true;
            }

            if (tok.Is(Keyword.True))
            {
                reader.Read();
                result = new LiteralExpression(tok!, true);
                return true;
            }

            if (tok.Is(Keyword.False))
            {
                reader.Read();
                result = new LiteralExpression(tok!, false);
                return true;
            }

            LiteralToken? literalTok = tok as LiteralToken;
            if (literalTok != null)
            {
                result = new LiteralExpression(tok!, literalTok!.Value);
                reader.Read();
                return true;
            }

            if (tok.Is(Keyword.Address))
            {
                return this.ParseAddressExpression(reader, out result);
            }

            if (tok is IdentifierToken || tok.Is(Keyword.Inherited))
            {
                ReferenceExpression? refExpr = null;
                if (!this.ParseReferenceExpression(reader, out refExpr))
                {
                    result = null;
                    return false;
                }

                result = refExpr;
                return true;
            }

            string message = string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                Properties.Resources.Parser_Unexpected,
                tok != null ? tok.ToString() : Properties.Resources.Parser_EndOfFile);
            this.log.Write(new Message(reader.Path, reader.Line, reader.Column, Severity.Error, message));
            result = null;
            return false;
        }

        private bool ParseNotExpression(TokenReader reader, out Expression? expression)
        {
            expression = null;
            Token? start = reader.Peek();
            if (!this.Expect(reader, Keyword.Not))
            {
                return false;
            }

            Expression? inner = null;
            if (!this.ParseFactorExpression(reader, out inner))
            {
                return false;
            }

            expression = new NotExpression(start!, inner!);
            return true;
        }

        private bool ParseNegativeExpression(TokenReader reader, out Expression? expression)
        {
            expression = null;
            Token? start = reader.Peek();
            if (!this.Expect(reader, Keyword.Minus))
            {
                return false;
            }

            Expression? inner = null;
            if (!this.ParseFactorExpression(reader, out inner))
            {
                return false;
            }

            expression = new NegativeExpression(start!, inner!);
            return true;
        }

        private bool ParseAddressExpression(TokenReader reader, out Expression? expression)
        {
            expression = null;
            Token? tok = reader.Peek();
            if (!this.Expect(reader, Keyword.Address))
            {
                return false;
            }

            ReferenceExpression? refExpr = null;
            if (!this.ParseReferenceExpression(reader, out refExpr))
            {
                return false;
            }

            expression = new AddressExpression(tok!, refExpr!);
            return true;
        }

        private bool ParseNewExpression(TokenReader reader, out Expression? expression)
        {
            expression = null;
            Token? start = reader.Peek();
            if (!this.Expect(reader, Keyword.New))
            {
                return false;
            }

            TypeReference? typeRef = null;
            if (!this.ParseTypeReference(reader, out typeRef))
            {
                return false;
            }

            List<Expression> args = new List<Expression>();
            Token? tok = reader.Peek();
            if (tok.Is(Keyword.LeftParen))
            {
                reader.Read();
                tok = reader.Peek();
                while (tok != null && !tok.Is(Keyword.RightParen))
                {
                    Expression? arg = null;
                    if (!this.ParseExpression(reader, out arg))
                    {
                        return false;
                    }

                    args.Add(arg!);
                    tok = reader.Peek();
                    if (!tok.Is(Keyword.Comma))
                    {
                        break;
                    }
                    else
                    {
                        tok = reader.Read();
                    }
                }

                if (!this.Expect(reader, Keyword.RightParen))
                {
                    return false;
                }
            }

            expression = new NewExpression(start!, typeRef!, args);
            return true;
        }

        private bool ParseTypeBlock(TokenReader reader, ProgramUnit program)
        {
            if (!this.Expect(reader, Keyword.Type))
            {
                return false;
            }

            IdentifierToken? typeName = reader.Peek() as IdentifierToken;
            while (typeName != null)
            {
                reader.Read();
                TypeDeclaration? type = null;

                if (!this.Expect(reader, Keyword.Equals))
                {
                    return false;
                }

                bool isPublic = false;
                Token? tok = reader.Peek();
                if (tok.Is(Keyword.Public))
                {
                    isPublic = true;
                    reader.Read();
                    tok = reader.Peek();
                }

                if (tok.Is(Keyword.LeftParen))
                {
                    if (!this.ParseEnumDeclaration(typeName, reader, out type))
                    {
                        return false;
                    }
                }
                else if (tok.Is(Keyword.Class) || tok.Is(Keyword.Static))
                {
                    if (!this.ParseClassDeclaration(typeName, reader, out type))
                    {
                        return false;
                    }
                }
                else if (tok.Is(Keyword.Interface))
                {
                    if (!this.ParseInterfaceDeclaration(typeName, reader, out type))
                    {
                        return false;
                    }
                }
                else if (tok.Is(Keyword.Function) || tok.Is(Keyword.Procedure))
                {
                    if (!this.ParseMethodTypeDeclaration(typeName, reader, out type))
                    {
                        return false;
                    }
                }

                type!.IsPublic = isPublic;
                program.AddType(type!);
                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }

                typeName = reader.Peek() as IdentifierToken;
            }

            return true;
        }

        private bool ParseInterfaceDeclaration(IdentifierToken typeName, TokenReader reader, out TypeDeclaration? type)
        {
            InterfaceDeclaration interfaceDecl = new InterfaceDeclaration(typeName, typeName.Value);
            type = null;

            if (!this.Expect(reader, Keyword.Interface))
            {
                return false;
            }

            Token? tok = reader.Peek();
            if (tok.Is(Keyword.LeftParen))
            {
                reader.Read();
                string? baseInterfaceName = null;
                if (!this.ParseFullNameDeclaration(reader, out baseInterfaceName))
                {
                    return false;
                }

                if (!this.Expect(reader, Keyword.RightParen))
                {
                    return false;
                }

                interfaceDecl.BaseInterfaceType = baseInterfaceName;
            }

            tok = reader.Peek();
            while (tok != null && (tok.Is(Keyword.Function) || tok.Is(Keyword.Procedure)))
            {
                MethodDeclaration? method = null;
                if (!this.ParseMethodDeclaration(reader, out method))
                {
                    return false;
                }

                interfaceDecl.Methods.Add(method!);
                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }

                tok = reader.Peek();
            }

            if (!this.Expect(reader, Keyword.End))
            {
                return false;
            }

            type = interfaceDecl;
            return true;
        }

        private bool ParseMethodTypeDeclaration(IdentifierToken typeName, TokenReader reader, out TypeDeclaration? type)
        {
            MethodTypeDeclaration methodDecl = new MethodTypeDeclaration(typeName, typeName.Value);
            type = null;
            bool isFunction = false;

            Token? tok = reader.Peek();
            if (tok.Is(Keyword.Function))
            {
                isFunction = true;
            }

            reader.Read();

            tok = reader.Peek();
            if (tok.Is(Keyword.Of))
            {
                reader.Read();
                TypeReference? implicitArgType = null;
                if (!this.ParseTypeReference(reader, out implicitArgType))
                {
                    return false;
                }

                methodDecl.ImplicitArgType = implicitArgType;
            }

            if (!this.Expect(reader, Keyword.LeftParen))
            {
                return false;
            }

            tok = reader.Peek();
            while (!tok.Is(Keyword.RightParen))
            {
                ParameterDeclaration? parameter = null;
                if (!this.ParseParameterDeclaration(reader, out parameter))
                {
                    return false;
                }

                methodDecl.AddParameter(parameter!);

                tok = reader.Peek();
                if (tok.Is(Keyword.SemiColon))
                {
                    reader.Read();
                    tok = reader.Peek();
                }
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                return false;
            }

            if (isFunction)
            {
                if (!this.Expect(reader, Keyword.Colon))
                {
                    return false;
                }

                TypeReference? returnType = null;
                if (!this.ParseTypeReference(reader, out returnType))
                {
                    return false;
                }

                methodDecl.ReturnType = returnType;
            }

            type = methodDecl;
            return true;
        }

        private bool ParseEnumDeclaration(IdentifierToken typeName, TokenReader reader, out TypeDeclaration? type)
        {
            EnumDeclaration enumDecl = new EnumDeclaration(typeName, typeName.Value);
            type = null;
            if (!this.Expect(reader, Keyword.LeftParen))
            {
                return false;
            }

            string? name = null;
            if (!this.ExpectIdentifier(reader, out name))
            {
                return false;
            }

            enumDecl.AddValue(name!);

            Token? tok = reader.Peek();
            while (tok.Is(Keyword.Comma))
            {
                reader.Read();
                if (!this.ExpectIdentifier(reader, out name))
                {
                    return false;
                }

                enumDecl.AddValue(name!);

                tok = reader.Peek();
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                return false;
            }

            type = enumDecl;
            return true;
        }

        private bool ParseMethodDeclaration(TokenReader reader, out MethodDeclaration? method)
        {
            method = null;
            Token? tok = reader.Peek();
            Token? start = tok;
            if (tok.Is(Keyword.Constructor))
            {
                return this.ParseConstructorDeclaration(reader, out method);
            }

            if (tok.Is(Keyword.Destructor))
            {
                return this.ParseDestructorDeclaration(
                    reader,
                    false,
                    out method);
            }

            bool isStatic = false;
            bool isVirtual = false;
            bool isAbstract = false;
            if (tok.Is(Keyword.Static))
            {
                isStatic = true;
                reader.Read();
                tok = reader.Peek();
            }
            else if (tok.Is(Keyword.Abstract))
            {
                isAbstract = true;
                reader.Read();
                tok = reader.Peek();
            }
            else if (tok.Is(Keyword.Virtual))
            {
                isVirtual = true;
                reader.Read();
                tok = reader.Peek();
                if (tok.Is(Keyword.Destructor))
                {
                    return this.ParseDestructorDeclaration(
                        reader,
                        true,
                        out method);
                }
            }

            bool isFunction = false;
            if (tok.Is(Keyword.Function))
            {
                isFunction = true;
                reader.Read();
            }
            else
            {
                if (!this.Expect(reader, Keyword.Procedure))
                {
                    return false;
                }
            }

            string? methodName = null;
            if (!this.ExpectIdentifier(reader, out methodName))
            {
                return false;
            }

            MethodDeclaration methodDecl = new MethodDeclaration(
                start!,
                methodName!,
                isStatic,
                isVirtual,
                isAbstract);

            if (!this.Expect(reader, Keyword.LeftParen))
            {
                return false;
            }

            tok = reader.Peek();
            while (!tok.Is(Keyword.RightParen))
            {
                ParameterDeclaration? parameter = null;
                if (!this.ParseParameterDeclaration(reader, out parameter))
                {
                    return false;
                }

                methodDecl.AddParameter(parameter!);

                tok = reader.Peek();
                if (tok.Is(Keyword.SemiColon))
                {
                    reader.Read();
                    tok = reader.Peek();
                }
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                return false;
            }

            if (isFunction)
            {
                if (!this.Expect(reader, Keyword.Colon))
                {
                    return false;
                }

                TypeReference? returnType = null;
                if (!this.ParseTypeReference(reader, out returnType))
                {
                    return false;
                }

                methodDecl.ReturnType = returnType;
            }

            method = methodDecl;
            return true;
        }

        private bool ParseConstructorDeclaration(TokenReader reader, out MethodDeclaration? method)
        {
            Token? start = reader.Peek();
            if (!this.Expect(reader, Keyword.Constructor))
            {
                method = null;
                return false;
            }

            method = null;
            MethodDeclaration methodDecl = new MethodDeclaration(
                start!,
                "constructor",
                false,
                false,
                false);

            if (!this.Expect(reader, Keyword.LeftParen))
            {
                return false;
            }

            Token? tok = reader.Peek();
            while (!tok.Is(Keyword.RightParen))
            {
                ParameterDeclaration? parameter = null;
                if (!this.ParseParameterDeclaration(reader, out parameter))
                {
                    return false;
                }

                methodDecl.AddParameter(parameter!);

                tok = reader.Peek();
                if (tok.Is(Keyword.SemiColon))
                {
                    reader.Read();
                    tok = reader.Peek();
                }
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                return false;
            }

            method = methodDecl;
            return true;
        }

        private bool ParseDestructorDeclaration(
            TokenReader reader,
            bool isVirtual,
            out MethodDeclaration? method)
        {
            Token? tok = reader.Peek();
            if (!this.Expect(reader, Keyword.Destructor))
            {
                method = null;
                return false;
            }

            if (!this.Expect(reader, Keyword.LeftParen))
            {
                method = null;
                return false;
            }

            if (!this.Expect(reader, Keyword.RightParen))
            {
                method = null;
                return false;
            }

            method = new MethodDeclaration(
                tok!,
                "destructor",
                false,
                isVirtual,
                false);
            return true;
        }

        private bool ParseTypeReference(TokenReader reader, out TypeReference? type)
        {
            Token? tok = reader.Peek();
            Token? start = tok;
            type = null;

            if (tok.Is(Keyword.Pointer))
            {
                reader.Read();
                TypeReference? elementType = null;
                if (!this.ParseTypeReference(reader, out elementType))
                {
                    return false;
                }

                type = new PointerTypeReference(start!, elementType!);
                return true;
            }
            else if (tok.Is(Keyword.Array))
            {
                reader.Read();
                tok = reader.Peek();
                Expression? elementCount = null;
                if (tok.Is(Keyword.LeftBracket))
                {
                    reader.Read();
                    if (!this.ParseExpression(reader, out elementCount))
                    {
                        return false;
                    }

                    if (!this.Expect(reader, Keyword.RightBracket))
                    {
                        return false;
                    }

                    tok = reader.Peek();
                }

                if (!this.Expect(reader, Keyword.Of))
                {
                    return false;
                }

                TypeReference? elementType = null;
                if (!this.ParseTypeReference(reader, out elementType))
                {
                    return false;
                }

                type = new ArrayTypeReference(start!, elementCount!, elementType!);
                return true;
            }
            else
            {
                string? typeName = null;
                if (!this.ParseFullNameDeclaration(reader, out typeName))
                {
                    return false;
                }

                type = new NamedTypeReference(start!, typeName);
                return true;
            }
        }

        private bool ParseParameterDeclaration(TokenReader reader, out ParameterDeclaration? parameter)
        {
            List<string> parameterNames = new List<string>();
            parameter = null;
            Token? start = reader.Peek();
            string? parameterName = null;
            if (!this.ExpectIdentifier(reader, out parameterName))
            {
                return false;
            }

            parameterNames.Add(parameterName!);
            Token? tok = reader.Peek();
            while (tok.Is(Keyword.Comma))
            {
                reader.Read();
                if (!this.ExpectIdentifier(reader, out parameterName))
                {
                    return false;
                }

                parameterNames.Add(parameterName!);
                tok = reader.Peek();
            }

            if (!this.Expect(reader, Keyword.Colon))
            {
                return false;
            }

            TypeReference? type = null;
            if (!this.ParseTypeReference(reader, out type))
            {
                return false;
            }

            ParameterDeclaration parameterDecl = new ParameterDeclaration(
                start!,
                parameterNames,
                type!);

            parameter = parameterDecl;
            return true;
        }

        private bool ParseVariableDeclaration(TokenReader reader, bool allowInitializer, out VariableDeclaration? variable)
        {
            List<string> variableNames = new List<string>();
            variable = null;
            Token? start = reader.Peek();
            string? variableName = null;
            if (!this.ExpectIdentifier(reader, out variableName))
            {
                return false;
            }

            variableNames.Add(variableName!);
            Token? tok = reader.Peek();
            while (tok.Is(Keyword.Comma))
            {
                reader.Read();
                if (!this.ExpectIdentifier(reader, out variableName))
                {
                    return false;
                }

                variableNames.Add(variableName!);
                tok = reader.Peek();
            }

            if (!this.Expect(reader, Keyword.Colon))
            {
                return false;
            }

            TypeReference? type = null;
            if (!this.ParseTypeReference(reader, out type))
            {
                return false;
            }

            Expression? initExpr = null;
            if (allowInitializer)
            {
                tok = reader.Peek();
                if (tok.Is(Keyword.Equals))
                {
                    reader.Read();
                    if (!this.ParseExpression(reader, out initExpr))
                    {
                        return false;
                    }
                }
            }

            VariableDeclaration variableDecl = new VariableDeclaration(
                start!,
                variableNames,
                type!,
                initExpr);

            variable = variableDecl;
            return true;
        }

        private bool ParseVarBlock(
            TokenReader reader,
            bool allowInitializers,
            out VarBlock? value)
        {
            Token? start = reader.Peek();
            VarBlock varBlock = new VarBlock(start!);
            value = null;
            if (!this.Expect(reader, Keyword.Var))
            {
                return false;
            }

            Token? tok = reader.Peek();
            while (tok != null && tok is IdentifierToken)
            {
                VariableDeclaration? varDecl = null;
                if (!this.ParseVariableDeclaration(reader, allowInitializers, out varDecl))
                {
                    return false;
                }

                if (!this.Expect(reader, Keyword.SemiColon))
                {
                    return false;
                }

                varBlock.Variables.Add(varDecl!);
                tok = reader.Peek();
            }

            value = varBlock;
            return true;
        }

        private bool ParseFullNameDeclaration(TokenReader reader, out string value)
        {
            value = string.Empty;
            StringBuilder sb = new StringBuilder();
            string? identifier = null;
            if (!this.ExpectIdentifier(reader, out identifier))
            {
                return false;
            }

            sb.Append(identifier);
            Token? tok = reader.Peek();
            while (tok.Is(Keyword.Dot))
            {
                reader.Read();
                sb.Append('.');
                if (!this.ExpectIdentifier(reader, out identifier))
                {
                    return false;
                }

                sb.Append(identifier);
                tok = reader.Peek();
            }

            value = sb.ToString();
            return true;
        }

        private bool ExpectEndOfFile(TokenReader reader)
        {
            Token? tok = reader.Read();
            if (tok != null)
            {
                this.log.Write(new Message(
                    tok.Path,
                    tok.Line,
                    tok.Column,
                    Severity.Error,
                    Properties.Resources.Parser_EndOfFileExpected));
                return false;
            }

            return true;
        }

        private bool ExpectIdentifier(TokenReader reader, out string? value)
        {
            value = null;
            Token? token = reader.Read();
            if (token == null)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.Parser_IdentifierExpected,
                    Properties.Resources.Parser_EndOfFile);
                this.log.Write(new Message(reader.Path, reader.Line, reader.Column, Severity.Error, message));
                return false;
            }

            IdentifierToken? idTok = token as IdentifierToken;
            if (idTok == null)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.Parser_IdentifierExpected,
                    token);
                this.log.Write(new Message(token.Path, token.Line, token.Column, Severity.Error, message));
                return false;
            }

            value = idTok.Value;
            return true;
        }

        private bool Expect(TokenReader reader, Keyword keyword)
        {
            Token? token = reader.Read();
            if (token == null)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.Parser_KeywordExpected,
                    keyword,
                    Properties.Resources.Parser_EndOfFile);
                this.log.Write(new Message(reader.Path, reader.Line, reader.Column, Severity.Error, message));
                return false;
            }

            KeywordToken? keyToken = token as KeywordToken;
            if (keyToken == null || keyToken.Value != keyword)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.Parser_KeywordExpected,
                    keyword,
                    token.ToString());
                this.log.Write(new Message(token.Path, token.Line, token.Column, Severity.Error, message));
                return false;
            }

            return true;
        }
    }
}
