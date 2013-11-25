using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class CodeGenerator
    {
        private MessageLog log;
        public CodeGenerator(MessageLog log)
        {
            this.log = log;
        }

        public bool CreateTypes(CompilerContext context, ProgramUnit programUnit)
        {
            bool failed = false;
            foreach (string uses in programUnit.Uses)
            {
                string includePath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(programUnit.Start.Path),
                    uses + ".th");
                if (System.IO.File.Exists(includePath))
                {
                    ProgramUnit usedProgramUnit = null;
                    using (TokenReader reader = new TokenReader(includePath, this.log))
                    {
                        Parser includeParser = new Parser(this.log);
                        if (includeParser.TryParse(reader, out usedProgramUnit))
                        {
                            if (!this.CreateTypes(context, usedProgramUnit))
                            {
                                failed = true;
                            }
                        }
                        else
                        {
                            failed = true;
                        }
                    }
                }
            }

            context.ClearUses();
            foreach (string uses in programUnit.Uses)
            {
                context.AddUses(uses);
            }

            context.Namespace = programUnit.Namespace;
            foreach (TypeDeclaration typeDecl in programUnit.Types)
            {
                TypeDefinition typeDef = null;
                if (!context.TryDeclareType(typeDecl.Name, out typeDef))
                {
                    string message = string.Format(
                        Properties.Resources.CodeGenerator_TypeAlreadyDeclared, 
                        typeDecl.Name);
                    log.Write(new Message(
                        typeDecl.Start.Path, 
                        typeDecl.Start.Line, 
                        typeDecl.Start.Column, 
                        Severity.Error,
                        message));
                    failed = true;
                }

                ClassDeclaration classDecl = typeDecl as ClassDeclaration;
                if (typeDecl != null)
                {
                    typeDef.IsClass = true;
                    typeDef.IsPublic = classDecl.IsPublic;
                    typeDef.IsStaticClass = classDecl.IsStatic;

                    foreach (MethodDeclaration meth in classDecl.PublicMethods)
                    {
                        MethodInfo methodInfo = null;
                        if (!this.TryCreateMethod(context, typeDef, meth, out methodInfo))
                        {
                            failed = true;
                            continue;
                        }

                        methodInfo.IsPublic = true;
                        typeDef.Methods.Add(methodInfo);
                    }

                    foreach (MethodDeclaration meth in classDecl.ProtectedMethods)
                    {
                        MethodInfo methodInfo = null;
                        if (!this.TryCreateMethod(context, typeDef, meth, out methodInfo))
                        {
                            failed = true;
                            continue;
                        }

                        methodInfo.IsProtected = true;
                        typeDef.Methods.Add(methodInfo);
                    }

                    foreach (MethodDeclaration meth in classDecl.PrivateMethods)
                    {
                        MethodInfo methodInfo = null;
                        if (!this.TryCreateMethod(context, typeDef, meth, out methodInfo))
                        {
                            failed = true;
                            continue;
                        }

                        methodInfo.IsProtected = true;
                        typeDef.Methods.Add(methodInfo);
                    }

                    int size = 0;
                    if (classDecl.Fields != null)
                    {
                        foreach (var field in classDecl.Fields.Variables)
                        {
                            TypeDefinition fieldType = null;
                            if (!this.TryResolveTypeReference(context, field.Type, out fieldType))
                            {
                                failed = true;
                                continue;
                            }

                            foreach (string identifier in field.VariableNames)
                            {
                                FieldInfo fieldInfo = new FieldInfo();
                                fieldInfo.Name = identifier;
                                fieldInfo.Type = fieldType;
                                fieldInfo.IsStatic = classDecl.IsStatic;
                                if (!fieldInfo.IsStatic)
                                {
                                    fieldInfo.Offset = size;
                                    size += fieldType.Size;
                                }

                                typeDef.Fields.Add(fieldInfo);
                            }
                        }
                    }

                    typeDef.Size = size;
                }
            }

            return !failed;
        }

        public bool CreateModule(CompilerContext context, ProgramUnit programUnit, out Module module)
        {
            bool failed = false;
            module = new Module();
            foreach (MethodDefinition methodDef in programUnit.Methods)
            {
                MethodInfo methodInfo = null;
                TypeDefinition type = null;
                List<TypeDefinition> parameterTypes = new List<TypeDefinition>();
                foreach (var paramDecl in methodDef.Parameters)
                {
                    TypeDefinition paramType = null;
                    if (!this.TryResolveTypeReference(context, paramDecl.Type, out paramType))
                    {
                        return false;
                    }

                    foreach (string paramName in paramDecl.ParameterNames)
                    {
                        parameterTypes.Add(paramType);
                    }
                }

                if (!context.TryFindMethodAndType(
                    methodDef.MethodNameReference, 
                    parameterTypes,
                    out type, 
                    out methodInfo))
                {
                    string message = string.Format(
                        Properties.Resources.CodeGenerator_UndeclaredMethod,
                        methodDef.MethodNameReference);
                    this.log.Write(new Message(
                        methodDef.Start.Path,
                        methodDef.Start.Line,
                        methodDef.Start.Column,
                        Severity.Error,
                        message));
                    failed = true;
                    continue;
                }

                MethodImpl methodImpl = new MethodImpl(module);
                methodImpl.Method = methodInfo;
                if (this.TryImplementMethod(methodImpl, context, methodDef, type))
                {
                    module.CodeSegment.Add(methodImpl);
                }
                else
                {
                    failed = true;
                }
            }

            return !failed;
        }

        private bool TryImplementMethod(
            MethodImpl method,
            CompilerContext context,
            MethodDefinition methodDef,
            TypeDefinition typeDef)
        {
            bool failed = false;
            if (!string.IsNullOrEmpty(methodDef.ExternImpl))
            {
                method.Module.AddExtern(methodDef.ExternImpl);
                method.Statements.Add(new AsmStatement { Instruction = "jmp " + methodDef.ExternImpl });
                return true;
            }

            Stack<LocalVariable> destructables = new Stack<LocalVariable>();
            Scope scope = context.BeginScope(method.Method);
            if (methodDef.LocalVariables != null)
            {
                foreach (VariableDeclaration varDecl in methodDef.LocalVariables.Variables)
                {
                    TypeDefinition varType = null;
                    if (!this.TryResolveTypeReference(context, varDecl.Type, out varType))
                    {
                        failed = true;
                        continue;
                    }

                    MethodInfo defaultConstructor = varType.GetDefaultConstructor();
                    MethodInfo destructor = varType.GetDestructor();
                    foreach (string varName in varDecl.VariableNames)
                    {
                        LocalVariable localVar = scope.DefineLocalVariable(varName, varType);
                        if (destructor != null)
                        {
                            destructables.Push(localVar);
                        }

                        if (varDecl.InitExpression != null)
                        {
                            TypeDefinition initValueType = null;
                            if (!this.TryEmitExpression(varDecl.InitExpression, context, scope, method, out initValueType))
                            {
                                failed = true;
                                continue;
                            }

                            if (varType.IsPointer || varType.IsArray && varType.ArrayElementCount == 0)
                            {
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov [ebp-{0}],eax", localVar.Offset) });
                            }
                            else if (varType.IsFloatingPoint)
                            {
                                if (varType.Size == 4)
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = string.Format("fstp dword ptr[ebp-{0}]", localVar.Offset) });
                                }
                                else
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = string.Format("fstp qword ptr[ebp-{0}]", localVar.Offset) });
                                }
                            }
                            else if (!varType.IsClass)
                            {
                                switch (varType.Size)
                                {
                                    case 1:
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov byte ptr[ebp-{0}],al", localVar.Offset) });
                                        break;
                                    case 2:
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov word ptr[ebp-{0}],ax", localVar.Offset) });
                                        break;
                                    case 4:
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov [ebp-{0}],eax", localVar.Offset) });
                                        break;
                                }
                            }
                        }
                        else if (defaultConstructor != null)
                        {
                            method.Module.AddProto(defaultConstructor);
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[ebp-{0}]", localVar.Offset) });
                            method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                            method.Statements.Add(new AsmStatement { Instruction = "call " + defaultConstructor.MangledName });
                            method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
                        }
                    }
                }
            }

            // statements alloc temp variables, so the frame setup code has to be last.
            if (!this.TryEmitBlockStatement(methodDef.Body, context, scope, method))
            {
                failed = true;
            }

            List<AsmStatement> frame = new List<AsmStatement>();
            frame.Add(new AsmStatement { Instruction = "push ebp" });
            frame.Add(new AsmStatement { Instruction = "mov ebp,esp" });
            if (scope.LocalOffset != 0)
            {
                int localOffset = scope.LocalOffset;
                int alignFix = localOffset % 4;
                if (alignFix != 0)
                {
                    localOffset += 4 - alignFix;
                }

                frame.Add(new AsmStatement { Instruction = "sub esp," + localOffset });
            }

            method.Statements.InsertRange(0, frame);

            while (destructables.Count > 0)
            {
                LocalVariable destructable = destructables.Pop();
                MethodInfo destructor = destructable.Type.GetDestructor();
                method.Module.AddProto(destructor);
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[ebp-{0}]", destructable.Offset) });
                method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                method.Statements.Add(new AsmStatement { Instruction = "call " + destructor.MangledName });
                method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
            }

            if (method.Method.ReturnType != null)
            {
                LocalVariable returnVar = scope.ReturnVariable;
                if (method.Method.ReturnType.IsFloatingPoint)
                {
                    if (method.Method.ReturnType.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("fld qword ptr [ebp-{0}]", returnVar.Offset) });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("fld dword ptr [ebp-{0}]", returnVar.Offset) });
                    }
                }
                else if (method.Method.ReturnType.Size <= 4 && !method.Method.ReturnType.IsClass)
                {
                    switch (method.Method.ReturnType.Size)
                    {
                        case 4:
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax, dword ptr[ebp-{0}]", returnVar.Offset) });
                            break;
                        case 2:
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov ax, word ptr[ebp-{0}]", returnVar.Offset) });
                            break;
                        case 1:
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov al, byte ptr[ebp-{0}]", returnVar.Offset) });
                            break;
                    }
                }
            }

            method.Statements.Add(new AsmStatement { Instruction = "mov esp,ebp" });
            method.Statements.Add(new AsmStatement { Instruction = "pop ebp" });
            method.Statements.Add(new AsmStatement { Instruction = "ret" });
            return !failed;
        }

        private bool TryEmitBlockStatement(
            BlockStatement statement,
            CompilerContext context,
            Scope scope,
            MethodImpl method)
        {
            bool failed = false;
            foreach (Statement s in statement.Statements)
            {
                if (!this.TryEmitStatement(s, context, scope, method))
                {
                    failed = true;
                }
            }

            return !failed;
        }

        private bool TryEmitStatement(
            Statement statement,
            CompilerContext context,
            Scope scope,
            MethodImpl method)
        {
            AssignmentStatement assignmentStatement = statement as AssignmentStatement;
            if (assignmentStatement != null)
            {
                return TryEmitAssignment(assignmentStatement, context, scope, method);
            }
            else
            {
                CallStatement callStatement = statement as CallStatement;
                if (callStatement != null)
                {
                    return TryEmitCall(callStatement, context, scope, method);
                }
                else
                {
                    IfStatement ifStatement = statement as IfStatement;
                    if (ifStatement != null)
                    {
                        return TryEmitIfStatement(ifStatement, context, scope, method);
                    }
                    else
                    {
                        WhileStatement whileStatement = statement as WhileStatement;
                        if (whileStatement != null)
                        {
                            return TryEmitWhileStatement(whileStatement, context, scope, method);
                        }
                        else
                        {
                            BlockStatement blockStatement = statement as BlockStatement;
                            if (blockStatement != null)
                            {
                                return TryEmitBlockStatement(blockStatement, context, scope, method);
                            }
                            else
                            {
                                DeleteStatement deleteStatement = statement as DeleteStatement;
                                if (deleteStatement != null)
                                {
                                    return TryEmitDeleteStatement(deleteStatement, context, scope, method);
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool TryEmitDeleteStatement(DeleteStatement deleteStatement, CompilerContext context, Scope scope, MethodImpl method)
        {
            TypeDefinition valueType = null;
            if (!this.TryEmitExpression(deleteStatement.Value, context, scope, method, out valueType))
            {
                return false;
            }

            if (!valueType.IsPointer && !valueType.IsArray)
            {
                this.log.Write(
                    new Message(
                        deleteStatement.Start.Path,
                        deleteStatement.Start.Line,
                        deleteStatement.Start.Column,
                        Severity.Error,
                        Properties.Resources.CodeGenerator_DeleteOperandMustBePointerOrArray));
                return false;
            }

            if (valueType.IsPointer)
            {
                this.PushResult(method, valueType);
                TypeDefinition innerType = valueType.InnerType;
                if (innerType != null)
                {
                    MethodInfo destructor = innerType.GetDestructor();
                    if (destructor != null)
                    {
                        this.PushResult(method, valueType);
                        method.Module.AddProto(destructor);
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("call {0}", destructor.MangledName) });
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("add esp,{0}", valueType.Size) });
                    }
                }

                if (!this.TryEmitFreeCall(deleteStatement.Start, context, scope, method))
                {
                    return false;
                }

                method.Statements.Add(new AsmStatement { Instruction = string.Format("add esp,{0}", valueType.Size) });
                return true;
            }

            log.Write(new Message(
                deleteStatement.Start.Path,
                deleteStatement.Start.Line,
                deleteStatement.Start.Column,
                Severity.Error,
                Properties.Resources.CodeGenerator_DeleteOperandArrayNotSupported));
            return false;
        }

        private bool TryFindSystemMemory(Token start, CompilerContext context, out TypeDefinition memoryType)
        {
            if (!context.TryFindTypeByName("System.Memory", out memoryType))
            {
                log.Write(new Message(
                    start.Path,
                    start.Line,
                    start.Column,
                    Severity.Error,
                    Properties.Resources.CodeGenerator_SystemMemoryNotDeclared));
                return false;
            }

            return true;
        }

        private bool TryEmitFreeCall(Token start, CompilerContext context, Scope scope, MethodImpl method)
        {
            TypeDefinition memoryType = null;
            if (!this.TryFindSystemMemory(start, context, out memoryType))
            {
                return false;
            }

            MethodInfo freeMethod = memoryType.Methods.FirstOrDefault(e => string.CompareOrdinal(e.Name, "Free") == 0);
            if (freeMethod == null)
            {
                log.Write(new Message(
                    start.Path,
                    start.Line,
                    start.Column,
                    Severity.Error,
                    Properties.Resources.CodeGenerator_SystemMemoryMissingFree));
                return false;
            }

            method.Module.AddProto(freeMethod);
            method.Statements.Add(new AsmStatement { Instruction = string.Format("call {0}", freeMethod.MangledName) });
            return true;
        }

        private bool TryEmitIfStatement(IfStatement ifStatement, CompilerContext context, Scope scope, MethodImpl method)
        {
            TypeDefinition conditionType = null;
            if (!this.TryEmitExpression(ifStatement.Condition, context, scope, method, out conditionType))
            {
                return false;
            }

            if (!this.ValidateConditionType(ifStatement.Condition, conditionType))
            {
                return false;
            }

            string elseLabel = null;
            if (ifStatement.FalseStatement != null)
            {
                elseLabel = method.Module.GetNextJumpLabel();
            }

            string endLabel = method.Module.GetNextJumpLabel();
            if (elseLabel == null)
            {
                elseLabel = endLabel;
            }

            method.Statements.Add(new AsmStatement { Instruction = "test al,al" });
            method.Statements.Add(new AsmStatement { Instruction = "jz " + elseLabel });
            if (!this.TryEmitStatement(ifStatement.TrueStatement, context, scope, method))
            {
                return false;
            }

            if (ifStatement.FalseStatement != null)
            {
                method.Statements.Add(new AsmStatement { Instruction = "jmp " + endLabel });
                method.Statements.Add(new AsmStatement { Instruction = "nop", Label = elseLabel });
                if (!this.TryEmitStatement(ifStatement.FalseStatement, context, scope, method))
                {
                    return false;
                }
            }

            method.Statements.Add(new AsmStatement { Instruction = "nop", Label = endLabel });
            return true;
        }

        private bool TryEmitWhileStatement(WhileStatement whileStatement, CompilerContext context, Scope scope, MethodImpl method)
        {
            string topLabel = method.Module.GetNextJumpLabel();
            method.Statements.Add(new AsmStatement { Instruction = "nop", Label = topLabel });
            TypeDefinition conditionType = null;
            if (!this.TryEmitExpression(whileStatement.Condition, context, scope, method, out conditionType))
            {
                return false;
            }

            if (!this.ValidateConditionType(whileStatement.Condition, conditionType))
            {
                return false;
            }

            string endLabel = method.Module.GetNextJumpLabel();

            method.Statements.Add(new AsmStatement { Instruction = "test al,al" });
            method.Statements.Add(new AsmStatement { Instruction = "jz " + endLabel });
            if (!this.TryEmitStatement(whileStatement.BodyStatement, context, scope, method))
            {
                return false;
            }

            method.Statements.Add(new AsmStatement { Instruction = "jmp " + topLabel });
            method.Statements.Add(new AsmStatement { Instruction = "nop", Label = endLabel });
            return true;
        }

        private bool ValidateConditionType(Expression condition, TypeDefinition conditionType)
        {
            string message = Properties.Resources.CodeGenerator_BooleanConditionExpected;
            if (string.CompareOrdinal(conditionType.MangledName, "f") != 0)
            {
                this.log.Write(new Message(
                    condition.Start.Path,
                    condition.Start.Line,
                    condition.Start.Column,
                    Severity.Error,
                    message));
                return false;
            }

            return true;
        }

        private bool TryEmitCall(CallStatement callStatement, CompilerContext context, Scope scope, MethodImpl method)
        {
            TypeDefinition typeDef = null;
            return this.TryEmitExpression(callStatement.Expression, context, scope, method, out typeDef);
        }

        private bool TryEmitAssignment(
            AssignmentStatement assignmentStatement, 
            CompilerContext context, 
            Scope scope, 
            MethodImpl method)
        {
            string location = null;
            TypeDefinition storageType = null;
            TypeDefinition valueType = null;
            if (!this.TryEmitExpression(assignmentStatement.Value, context, scope, method, out valueType))
            {
                return false;
            }

            this.PushResult(method, valueType);

            MethodInfo calleeMethod = null;
            if (!this.TryEmitReference(assignmentStatement.Storage, context, scope, method, out location, out storageType, out calleeMethod))
            {
                return false;
            }

            if (valueType.Size <= 4)
            {
                method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                switch (storageType.Size)
                {
                    case 1:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov byte ptr [{0}],al", location) });
                        } break;
                    case 2:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov word ptr [{0}],ax", location) });
                        } break;
                    default:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov [{0}],eax", location) });
                        } break;
                }
            }
            else
            {
                method.Statements.Add(new AsmStatement { Instruction = "mov esi,esp" });
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea edi,[{0}]", location) });
                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov ecx,{0}", valueType.Size) });
                method.Statements.Add(new AsmStatement { Instruction = "cld" });
                method.Statements.Add(new AsmStatement { Instruction = "rep movsb" });
                method.Statements.Add(new AsmStatement { Instruction = string.Format("add esp,{0}", valueType.Size) });
            }

            return true;
        }

        private bool TryEmitLiteralExpression(
            LiteralExpression expression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition valueType)
        {
            if (expression.Value == null)
            {
                method.Statements.Add(new AsmStatement { Instruction = "xor eax,eax" });
                return context.TryFindTypeByName("^", out valueType);
            }

            if (expression.Value is int)
            {
                if ((int)expression.Value == 0)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "xor eax,eax" });
                }
                else
                {
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax,{0}", expression.Value) });
                }

                return context.TryFindTypeByName("integer", out valueType);
            }

            if (expression.Value is char)
            {
                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax,{0}", (int)expression.Value) });
                return context.TryFindTypeByName("character", out valueType);
            }

            if (expression.Value is string)
            {
                string label = method.Module.DefineLiteralString((string)expression.Value);
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[offset {0}]", label) });
                return context.TryFindTypeByName("#0character", out valueType);
            }

            if (expression.Value is decimal)
            {
                decimal exprValue = (decimal)expression.Value;
                if (exprValue == 0.0M)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "fldz" });
                }
                else if (exprValue == 1.0M)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "fld1" });
                }
                else
                {
                    string label = method.Module.DefineConstant((double)exprValue);
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("fld qword ptr [{0}]", label) });
                }
                return context.TryFindTypeByName("double", out valueType);
            }

            if (expression.Value is bool)
            {
                bool boolVal = (bool)expression.Value;
                if (boolVal)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "mov eax,1" });
                }
                else
                {
                    method.Statements.Add(new AsmStatement { Instruction = "xor eax,eax" });
                }

                return context.TryFindTypeByName("boolean", out valueType);
            }

            valueType = null;
            return false;
        }

        private bool TryEmitExpression(
            Expression expression, 
            CompilerContext context, 
            Scope scope, 
            MethodImpl method, 
            out TypeDefinition valueType)
        {
            LiteralExpression literalExpr = expression as LiteralExpression;
            if (literalExpr != null)
            {
                return this.TryEmitLiteralExpression(literalExpr, context, scope, method, out valueType);
            }

            CallReferenceExpression callExpr = expression as CallReferenceExpression;
            if (callExpr != null)
            {
                int argSize = 0;
                Expression[] arguments = callExpr.Arguments.ToArray();
                for (int i = arguments.Length - 1; i >= 0; i--)
                {
                    TypeDefinition argType = null;
                    if (!this.TryEmitExpression(arguments[i], context, scope, method, out argType))
                    {
                        valueType = null;
                        return false;
                    }

                    argSize += argType.Size;
                    this.PushResult(method, argType);
                }

                string callLoc = null;
                TypeDefinition storageType = null;
                MethodInfo calleeMethod = null;
                if (!this.TryEmitReference(callExpr.Inner, context, scope, method, out callLoc, out storageType, out calleeMethod))
                {
                    valueType = null;
                    return false;
                }

                method.Statements.Add(new AsmStatement { Instruction = "call " + callLoc });
                if (!calleeMethod.IsStatic)
                {
                    argSize += 4;
                }

                if (argSize > 0)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + argSize.ToString() });
                }

                valueType = storageType;
                return true;
            }

            ReferenceExpression refExpr = expression as ReferenceExpression;
            if (refExpr != null)
            {
                string location = null;
                TypeDefinition storageType = null;
                MethodInfo calleeMethod = null;
                if (!this.TryEmitReference(refExpr, context, scope, method, out location, out storageType, out calleeMethod))
                {
                    valueType = null;
                    return false;
                }

                if (storageType.IsFloatingPoint)
                {
                    if (storageType.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr [" + location + "]" });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr [" + location + "]" });
                    }
                }
                else if (storageType.IsArray)
                {
                    if (storageType.ArrayElementCount > 0)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "lea eax,[" + location + "]" });
                        valueType = context.GetArrayType(storageType.InnerType, 0);
                        return true;
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "mov eax,[" + location + "]" });
                    }
                }
                else if (storageType.Size <= 4 && !storageType.IsClass)
                {
                    switch (storageType.Size)
                    {
                        case 4:
                            method.Statements.Add(new AsmStatement { Instruction = "mov eax,dword ptr[" + location + "]" });
                            break;
                        case 2:
                            method.Statements.Add(new AsmStatement { Instruction = "mov ax,word ptr[" + location + "]" });
                            break;
                        case 1:
                            method.Statements.Add(new AsmStatement { Instruction = "mov al,byte ptr[" + location + "]" });
                            break;
                    }
                }

                valueType = storageType;
                return true;
            }

            SimpleExpression simpleExpr = expression as SimpleExpression;
            if (simpleExpr != null)
            {
                return this.TryEmitSimpleExpression(simpleExpr, context, scope, method, out valueType);
            }

            TermExpression termExpr = expression as TermExpression;
            if (termExpr != null)
            {
                return this.TryEmitTermExpression(termExpr, context, scope, method, out valueType);
            }

            RelationalExpression relExpr = expression as RelationalExpression;
            if (relExpr != null)
            {
                return this.TryEmitRelationExpresion(relExpr, context, scope, method, out valueType);
            }

            AddressExpression addrExpr = expression as AddressExpression;
            if (addrExpr != null)
            {
                return this.TryEmitAddressExpression(addrExpr, context, scope, method, out valueType);
            }

            NewExpression newExpr = expression as NewExpression;
            if (newExpr != null)
            {
                return this.TryEmitNewExpression(newExpr, context, scope, method, out valueType);
            }

            valueType = null;
            return false;
        }

        private bool TryEmitNewExpression(
            NewExpression newExpr, 
            CompilerContext context, 
            Scope scope, 
            MethodImpl method, 
            out TypeDefinition valueType)
        {
            valueType = null;
            TypeDefinition objectType = null;
            if (!this.TryResolveTypeReference(context, newExpr.Type, out objectType))
            {
                valueType = null;
                return false;
            }

            if (!objectType.IsClass && newExpr.ConstructorArguments.Count() > 0)
            {
                log.Write(new Message(
                    newExpr.Start.Path,
                    newExpr.Start.Line,
                    newExpr.Start.Column,
                    Severity.Error,
                    Properties.Resources.CodeGenerator_ConstructorArgumentsAreOnlySupportedOnClassTypes));
                return false;
            }

            valueType = context.GetPointerType(objectType);

            if (!this.TryEmitAllocCall(newExpr.Start, context, scope, method, objectType.Size))
            {
                return false;
            }

            MethodInfo constructor = null;
            int argSize = 0;
            if (objectType.IsClass)
            {
                int nullTestStart = method.Statements.Count;
                List<TypeDefinition> argTypes = new List<TypeDefinition>();
                foreach (Expression arg in newExpr.ConstructorArguments.Reverse())
                {
                    TypeDefinition argType = null;
                    if (!this.TryEmitExpression(arg, context, scope, method, out argType))
                    {
                        return false;
                    }

                    this.PushResult(method, argType);
                    argSize += argType.Size;
                    argTypes.Insert(0, argType);
                }

                if (!context.TryFindConstructor(objectType, argTypes, out constructor))
                {
                    // ok to have no default constructor
                    if (argTypes.Count > 0)
                    {
                        string message = string.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
                            Properties.Resources.CodeGenerator_CannotFindConstructor,
                            objectType.FullName);
                        log.Write(new Message(
                            newExpr.Start.Path,
                            newExpr.Start.Line,
                            newExpr.Start.Column,
                            Severity.Error,
                            message));
                        return false;
                    }
                }

                if (constructor != null)
                {
                    LocalVariable tempPtr = scope.DefineTempVariable(valueType);
                    string jumpLabel = method.Module.GetNextJumpLabel();
                    List<AsmStatement> saveAlloc = new List<AsmStatement>();
                    saveAlloc.Add(new AsmStatement { Instruction = string.Format("mov [ebp-{0}],eax", tempPtr.Offset) });
                    saveAlloc.Add(new AsmStatement { Instruction = "test eax,eax" });
                    saveAlloc.Add(new AsmStatement { Instruction = "jz " + jumpLabel });
                    method.Statements.InsertRange(nullTestStart, saveAlloc);
                    method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                    method.Module.AddProto(constructor);
                    method.Statements.Add(new AsmStatement { Instruction = "call " + constructor.MangledName });
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("add esp,{0}", argSize + 4) });
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax,[ebp-{0}]", tempPtr.Offset), Label = jumpLabel });
                }
            }


            return true;
        }

        private bool TryEmitAllocCall(Token start, CompilerContext context, Scope scope, MethodImpl method, int size)
        {
            TypeDefinition memoryType = null;
            if (!this.TryFindSystemMemory(start, context, out memoryType))
            {
                return false;
            }

            MethodInfo allocMethod = memoryType.Methods.FirstOrDefault(e => string.CompareOrdinal("Alloc", e.Name) == 0);
            if (allocMethod == null)
            {
                log.Write(new Message(
                    start.Path,
                    start.Line,
                    start.Column,
                    Severity.Error,
                    Properties.Resources.CodeGenerator_SystemMemoryMissingAlloc));
                return false;
            }

            method.Module.AddProto(allocMethod);
            method.Statements.Add(new AsmStatement { Instruction = "mov eax," + size.ToString() });
            method.Statements.Add(new AsmStatement { Instruction = "push eax" });
            method.Statements.Add(new AsmStatement { Instruction = "call " + allocMethod.MangledName });
            method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
            return true;
        }

        private bool TryEmitAddressExpression(
            AddressExpression addrExpr,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition valueType)
        {
            string innerLoc = null;
            TypeDefinition storageType = null;
            MethodInfo calleeMethod = null;
            if (!this.TryEmitReference(addrExpr.Inner, context, scope, method, out innerLoc, out storageType, out calleeMethod))
            {
                valueType = null;
                return false;
            }

            method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[{0}]", innerLoc) });
            valueType = context.GetPointerType(storageType);
            return true;
        }

        private bool TryEmitRelationExpresion(
            RelationalExpression relExpr, 
            CompilerContext context, 
            Scope scope, 
            MethodImpl method, 
            out TypeDefinition valueType)
        {
            context.TryFindTypeByName("boolean", out valueType);
            TypeDefinition rightSideType = null;
            if (!this.TryEmitExpression(relExpr.Right, context, scope, method, out rightSideType))
            {
                return false;
            }

            this.PushResult(method, rightSideType);
            TypeDefinition leftSideType = null;
            if (!this.TryEmitExpression(relExpr.Left, context, scope, method, out leftSideType))
            {
                return false;
            }

            if (rightSideType.IsFloatingPoint)
            {
            } 
            else if(leftSideType.IsFloatingPoint)
            {
            }
            else
            {
                if(leftSideType.Size <= 4 && rightSideType.Size <= 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "pop ecx"});
                    method.Statements.Add(new AsmStatement { Instruction = "cmp eax,ecx"});
                    switch (relExpr.Operator)
                    {
                        case Keyword.Equals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "sete al" });
                            } break;
                        case Keyword.LessThan:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setl al" });
                            } break;
                        case Keyword.LessThanOrEquals:
                            { 
                                method.Statements.Add(new AsmStatement { Instruction = "setle al" });
                            } break;
                        case Keyword.GreaterThan:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setg al" });
                            } break;
                        case Keyword.GreaterThanOrEquals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setge al" });
                            } break;
                        case Keyword.NotEqual:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setne al" });
                            } break;
                    }
                }
            }

            return true;
        }

        private void PushResult(
            MethodImpl method,
            TypeDefinition type)
        {
            if (type.IsFloatingPoint)
            {
                if (type.Size == 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "sub esp,4" });
                    method.Statements.Add(new AsmStatement { Instruction = "fstp dword ptr [esp]" });
                }
                else
                {
                    method.Statements.Add(new AsmStatement { Instruction = "sub esp,8" });
                    method.Statements.Add(new AsmStatement { Instruction = "fstp qword ptr [esp]" });
                }
            }
            else if (type.IsArray)
            {
                method.Statements.Add(new AsmStatement { Instruction = "push eax" });
            }
            else if (type.Size <= 4)
            {
                method.Statements.Add(new AsmStatement { Instruction = "push eax" });
            }
        }

        private bool TryEmitSimpleExpression(
            SimpleExpression expression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition valueType)
        {
            TypeDefinition rightSideType = null;
            if (!this.TryEmitExpression(expression.Right, context, scope, method, out rightSideType))
            {
                valueType = null;
                return false;
            }

            this.PushResult(method, rightSideType);
            TypeDefinition leftSideType = null;
            if (!this.TryEmitExpression(expression.Left, context, scope, method, out leftSideType))
            {
                valueType = null;
                return false;
            }

            if (leftSideType.IsFloatingPoint)
            {
                if (rightSideType.IsFloatingPoint)
                {
                    if (rightSideType.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr [esp]" });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr [esp]" });
                    }

                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType.Size });
                }
                else if (rightSideType.Size <= 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "fild [esp]" });
                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType.Size });
                }

                switch (expression.Operator)
                {
                    case Keyword.Plus:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "faddp" });
                        } break;
                    case Keyword.Minus:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "fsubrp" });
                        } break;
                }
            }
            else if (leftSideType.Size <= 4)
            {
                if (rightSideType.Size <= 4 && !rightSideType.IsFloatingPoint)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "pop ecx" });
                    switch (expression.Operator)
                    {
                        case Keyword.Plus:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "add eax,ecx" });
                            } break;
                        case Keyword.Minus:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "sub eax,ecx" });
                            } break;
                        case Keyword.Or:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "or eax,ecx" });
                            } break;
                    }
                }
            }

            valueType = leftSideType;
            return true;
        }

        private bool TryEmitTermExpression(
            TermExpression expression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition valueType)
        {
            TypeDefinition rightSideType = null;
            if (!this.TryEmitExpression(expression.Right, context, scope, method, out rightSideType))
            {
                valueType = null;
                return false;
            }

            this.PushResult(method, rightSideType);
            TypeDefinition leftSideType = null;
            if (!this.TryEmitExpression(expression.Left, context, scope, method, out leftSideType))
            {
                valueType = null;
                return false;
            }

            if (leftSideType.IsFloatingPoint)
            {
                if (rightSideType.IsFloatingPoint)
                {
                    if (rightSideType.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr [esp]" });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr [esp]" });
                    }

                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType.Size });
                }
                else if (rightSideType.Size <= 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "fild [esp]" });
                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType.Size });
                }

                switch (expression.Operator)
                {
                    case Keyword.Star:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "fmulp" });
                        } break;
                    case Keyword.Slash:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "fdivrp" });
                        } break;
                }
            }
            else if (leftSideType.Size <= 4)
            {
                if (rightSideType.Size <= 4 && !rightSideType.IsFloatingPoint)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "pop ecx" });
                    switch (expression.Operator)
                    {
                        case Keyword.Star:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "imul eax,ecx" });
                            } break;
                        case Keyword.Mod:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "xor edx,edx" });
                                method.Statements.Add(new AsmStatement { Instruction = "idiv ecx" });
                                method.Statements.Add(new AsmStatement { Instruction = "mov eax,edx" });
                            } break;
                        case Keyword.And:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "and eax,ecx" });
                            } break;
                        case Keyword.Div:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "xor edx,edx" });
                                method.Statements.Add(new AsmStatement { Instruction = "idiv ecx" });
                            } break;
                    }
                }
            }

            valueType = leftSideType;
            return true;
        }

        private bool TryEmitReference(
            ReferenceExpression referenceExpression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out string location,
            out TypeDefinition storageType,
            out MethodInfo calleeMethod)
        {
            calleeMethod = null;
            NamedReferenceExpression namedRef = referenceExpression as NamedReferenceExpression;
            if (namedRef != null)
            {
                SymbolEntry symbol = null;
                if (scope.TryLookup(namedRef.Identifier, out symbol))
                {
                    storageType = symbol.Type;
                    LocalVariable localVar = symbol as LocalVariable;
                    if (localVar != null)
                    {
                        location = "ebp-" + localVar.Offset;
                        return true;
                    }
                    else
                    {
                        ParameterVariable parVar = symbol as ParameterVariable;
                        if (parVar != null)
                        {
                            location = "ebp+" + parVar.Offset;
                            return true;
                        }
                    }
                }

                foreach (var field in method.Method.Type.Fields)
                {
                    if (string.CompareOrdinal(field.Name, namedRef.Identifier) == 0)
                    {
                        SymbolEntry symThis = null;
                        if(!scope.TryLookup("this", out symThis))
                        {
                            storageType = null;
                            location = null;
                            return false;
                        }

                        method.Statements.Add(
                            new AsmStatement 
                            { 
                                Instruction = string.Format("mov ecx,[ebp+{0}]", ((ParameterVariable)symThis).Offset)
                            });
                        if (field.Offset > 0)
                        {
                            method.Statements.Add(
                                new AsmStatement { Instruction = "add ecx," + field.Offset });
                        }

                        storageType = field.Type;
                        location = "ecx";
                        return true;
                    }
                }

                foreach (var memberMethod in method.Method.Type.Methods)
                {
                    if (string.CompareOrdinal(memberMethod.Name, namedRef.Identifier) == 0)
                    {
                        method.Module.AddProto(memberMethod);
                        if (!memberMethod.IsStatic)
                        {
                            SymbolEntry symThis = null;
                            if (!scope.TryLookup("this", out symThis))
                            {
                                storageType = null;
                                location = null;
                                return false;
                            }

                            method.Statements.Add(
                                new AsmStatement
                                {
                                    Instruction = string.Format("mov ecx,[ebp+{0}]", ((ParameterVariable)symThis).Offset)
                                });

                            method.Statements.Add(new AsmStatement { Instruction = "push ecx" });
                        }

                        location = memberMethod.MangledName;
                        storageType = memberMethod.ReturnType;
                        calleeMethod = memberMethod;
                        return true;
                    }
                }

                if (context.TryFindTypeByName(namedRef.Identifier, out storageType))
                {
                    location = null;
                    return true;
                }

                string message = string.Format(
                    Properties.Resources.CodeGenerator_UndeclaredIdentifier,
                    namedRef.Identifier);
                log.Write(new Message(
                    namedRef.Start.Path,
                    namedRef.Start.Line,
                    namedRef.Start.Column,
                    Severity.Error,
                    message));
            }

            MemberReferenceExpression memberRef = referenceExpression as MemberReferenceExpression;
            if (memberRef != null)
            {
                string innerLoc = null;
                TypeDefinition innerType = null;
                MethodInfo innerCalleeMethod = null;
                if (this.TryEmitReference(memberRef.Inner, context, scope, method, out innerLoc, out innerType, out innerCalleeMethod))
                {
                    foreach (var field in innerType.Fields)
                    {
                        if (string.CompareOrdinal(field.Name, memberRef.MemberName) == 0)
                        {
                            method.Statements.Add(
                                new AsmStatement
                                {
                                    Instruction = string.Format("lea ecx,[{0}]", innerLoc)
                                });
                            if (field.Offset > 0)
                            {
                                method.Statements.Add(
                                    new AsmStatement { Instruction = "add ecx," + field.Offset });
                            }

                            storageType = field.Type;
                            location = "ecx";
                            return true;
                        }
                    }

                    foreach (var memberMethod in innerType.Methods)
                    {
                        if (string.CompareOrdinal(memberMethod.Name, memberRef.MemberName) == 0)
                        {
                            method.Module.AddProto(memberMethod);
                            if (!memberMethod.IsStatic)
                            {
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[{0}]", innerLoc) });
                                method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                            }

                            location = memberMethod.MangledName;
                            storageType = memberMethod.ReturnType;
                            calleeMethod = memberMethod;
                            return true;
                        }
                    }

                    string message = string.Format(
                        Properties.Resources.CodeGenerator_UndeclaredMember,
                        memberRef.MemberName,
                        innerType.FullName);
                    log.Write(new Message(
                        memberRef.Start.Path,
                        memberRef.Start.Line,
                        memberRef.Start.Column,
                        Severity.Error,
                        message));
                }
            }

            CallReferenceExpression callExpr = referenceExpression as CallReferenceExpression;
            if (callExpr != null)
            {
                location = null;
                return this.TryEmitExpression(callExpr, context, scope, method, out storageType);
            }

            ArrayIndexReferenceExpression arrayIndexExpr = referenceExpression as ArrayIndexReferenceExpression;
            if (arrayIndexExpr != null)
            {
                TypeDefinition indexType = null;
                if (!this.TryEmitExpression(
                    arrayIndexExpr.Index,
                    context,
                    scope,
                    method,
                    out indexType))
                {
                    location = null;
                    storageType = null;
                    return false;
                }

                if (indexType.IsFloatingPoint || indexType.IsArray || indexType.IsClass || indexType.IsPointer)
                {
                    this.log.Write(new Message(
                        arrayIndexExpr.Index.Start.Path,
                        arrayIndexExpr.Index.Start.Line,
                        arrayIndexExpr.Index.Start.Column,
                        Severity.Error,
                        Properties.Resources.CodeGenerator_ArrayIndexIntExpected));
                    location = null;
                    storageType = null;
                    return false;
                }

                method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                string innerLoc = null;
                TypeDefinition innerType = null;
                MethodInfo arrayCalleeMethod = null;
                if (!this.TryEmitReference(
                    arrayIndexExpr.Inner,
                    context,
                    scope,
                    method,
                    out innerLoc,
                    out innerType,
                    out arrayCalleeMethod))
                {
                    location = null;
                    storageType = null;
                    return false;
                }

                if (!innerType.IsArray)
                {
                    log.Write(new Message(
                        arrayIndexExpr.Inner.Start.Path,
                        arrayIndexExpr.Inner.Start.Line,
                        arrayIndexExpr.Inner.Start.Column,
                        Severity.Error,
                        Properties.Resources.CodeGenerator_ArrayTypeExpected));
                    location = null;
                    storageType = null;
                    return false;
                }

                if (innerType.Size > 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "lea ecx,[" + innerLoc + "]" });
                }
                else
                {
                    method.Statements.Add(new AsmStatement { Instruction = "mov ecx,[" + innerLoc + "]" });
                }

                method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                method.Statements.Add(new AsmStatement { Instruction = "mov ebx," + innerType.InnerType.Size.ToString() });
                method.Statements.Add(new AsmStatement { Instruction = "imul eax,ebx" });
                method.Statements.Add(new AsmStatement { Instruction = "add ecx,eax" });
                location = "ecx";
                storageType = innerType.InnerType;
                return true;
            }

            DereferenceExpression derefExpr = referenceExpression as DereferenceExpression;
            if (derefExpr != null)
            {
                string innerLoc = null;
                TypeDefinition innerType = null;
                MethodInfo innerCall = null;
                if (!this.TryEmitReference(
                    derefExpr.Inner, 
                    context, 
                    scope, 
                    method, 
                    out innerLoc, 
                    out innerType, 
                    out innerCall))
                {
                    location = null;
                    storageType = null;
                    return false;
                }

                if (!innerType.IsPointer)
                {
                    log.Write(new Message(
                        derefExpr.Start.Path,
                        derefExpr.Start.Line,
                        derefExpr.Start.Column,
                        Severity.Error,
                        Properties.Resources.CodeGenerator_CannotDerefNonPointer));
                    location = null;
                    storageType = null;
                    return false;
                }

                method.Statements.Add(new AsmStatement { Instruction = "mov ecx,[" + innerLoc + "]" });
                location = "ecx";
                storageType = innerType.InnerType;
                return true;
            }

            location = null;
            storageType = null;
            return false;
        }

        private bool TryCreateMethod(
            CompilerContext context, 
            TypeDefinition type,
            MethodDeclaration methodDecl, 
            out MethodInfo methodInfo)
        {
            methodInfo = new MethodInfo(type)
            {
                Name = methodDecl.MethodName, 
                IsStatic = methodDecl.IsStatic
            };

            TypeDefinition returnType = null;
            if (methodDecl.ReturnType != null)
            {
                if (!this.TryResolveTypeReference(context, methodDecl.ReturnType, out returnType))
                {
                    return false;
                }

                methodInfo.ReturnType = returnType;
            }

            foreach (ParameterDeclaration paramDecl in methodDecl.Parameters)
            {
                TypeDefinition paramDef = null;
                if (!this.TryResolveTypeReference(context, paramDecl.Type, out paramDef))
                {
                    return false;
                }

                foreach (string name in paramDecl.ParameterNames)
                {
                    ParameterInfo paramInfo = new ParameterInfo
                    {
                        Name = name,
                        Type = paramDef
                    };

                    methodInfo.Parameters.Add(paramInfo);
                }
            }

            return true;
        }

        private bool TryResolveTypeReference(
            CompilerContext context, 
            TypeReference typeRef, 
            out TypeDefinition typeDef)
        {
            NamedTypeReference namedType = typeRef as NamedTypeReference;
            if (namedType != null)
            {
                if (!context.TryFindTypeByName(namedType.TypeName, out typeDef))
                {
                    string message = string.Format(
                        Properties.Resources.CodeGenerator_UndefinedType,
                        namedType.TypeName);
                    this.log.Write(new Message(
                        namedType.Start.Path,
                        namedType.Start.Line,
                        namedType.Start.Column,
                        Severity.Error,
                        message));
                    return false;
                }

                return true;
            }

            PointerTypeReference pointerType = typeRef as PointerTypeReference;
            if (pointerType != null)
            {
                TypeDefinition innerType = null;
                if (!this.TryResolveTypeReference(context, pointerType.ElementType, out innerType))
                {
                    typeDef = null;
                    return false;
                }

                typeDef = context.GetPointerType(innerType);
                return true;
            }

            ArrayTypeReference arrayType = typeRef as ArrayTypeReference;
            if (arrayType != null)
            {
                TypeDefinition innerType = null;
                if (!this.TryResolveTypeReference(context, arrayType.ElementType, out innerType))
                {
                    typeDef = null;
                    return false;
                }

                int elementCount = 0;
                if (arrayType.ElementCount != null)
                {
                    LiteralExpression litElementCount = arrayType.ElementCount as LiteralExpression;
                    if (litElementCount == null || !(litElementCount.Value is int))
                    {
                        log.Write(new Message(
                            arrayType.ElementCount.Start.Path,
                            arrayType.ElementCount.Start.Line,
                            arrayType.ElementCount.Start.Column,
                            Severity.Error,
                            Properties.Resources.CodeGenerator_ArrayTypeIntElementExpected));
                        typeDef = null;
                        return false;
                    }

                    elementCount = (int)(litElementCount.Value);
                }

                typeDef = context.GetArrayType(innerType, elementCount);
                return true;
            }

            typeDef = null;
            return false;
        }
    }
}
