//-----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Tea.Language;

    /// <summary>
    /// Generates the code from a parse tree.
    /// </summary>
    public class CodeGenerator
    {
        private readonly MessageLog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerator"/> class.
        /// </summary>
        /// <param name="log">The message log.</param>
        public CodeGenerator(MessageLog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Creates the types.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="programUnit">The source program unit.</param>
        /// <returns>True if the types could be created without error; otherwise, false.</returns>
        public bool CreateTypes(CompilerContext context, ProgramUnit programUnit)
        {
            bool failed = false;
            foreach (string uses in programUnit.Uses)
            {
                List<string> searchDirs = new List<string>();
                string? dirName = System.IO.Path.GetDirectoryName(programUnit.Start.Path);
                searchDirs.Add(dirName!);
                searchDirs.AddRange(context.Includes);
                string? headerPath = null;
                foreach (string dir in searchDirs)
                {
                    string includePath = System.IO.Path.Combine(
                        dir,
                        uses + ".th");
                    if (System.IO.File.Exists(includePath))
                    {
                        headerPath = includePath;
                        break;
                    }
                }

                if (headerPath != null && !context.AlreadyUsed(uses))
                {
                    context.AddAlreadyUsed(uses);
                    ProgramUnit? usedProgramUnit = null;
                    using (TokenReader reader = new TokenReader(headerPath!, this.log))
                    {
                        Parser includeParser = new Parser(this.log);
                        if (includeParser.TryParse(reader, out usedProgramUnit))
                        {
                            if (!this.CreateTypes(context, usedProgramUnit!))
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
                TypeDefinition? typeDef = null;
                if (!context.TryDeclareType(typeDecl.Name, out typeDef))
                {
                    string message = string.Format(
                        Properties.Resources.CodeGenerator_TypeAlreadyDeclared,
                        typeDecl.Name);
                    this.log.Write(new Message(
                        typeDecl.Start.Path,
                        typeDecl.Start.Line,
                        typeDecl.Start.Column,
                        Severity.Error,
                        message));
                    failed = true;
                }
            }

            foreach (TypeDeclaration typeDecl in programUnit.Types)
            {
                TypeDefinition? typeDef = null;
                if (!context.TryFindTypeByName(typeDecl.Name, out typeDef))
                {
                    failed = true;
                    continue;
                }

                EnumDeclaration? enumDecl = typeDecl as EnumDeclaration;
                if (enumDecl != null)
                {
                    typeDef!.IsEnum = true;
                    typeDef!.IsPublic = true;
                    typeDef!.Size = 4;
                    int constVal = 0;
                    foreach (string val in enumDecl.Values)
                    {
                        typeDef.EnumValues.Add(val, constVal++);
                    }
                }

                MethodTypeDeclaration? methodDecl = typeDecl as MethodTypeDeclaration;
                if (methodDecl != null)
                {
                    typeDef!.IsMethod = true;
                    typeDef!.IsPublic = true;
                    typeDef!.Size = 4;
                    if (methodDecl!.ImplicitArgType != null)
                    {
                        TypeDefinition? implicitArgType = null;
                        if (!this.TryResolveTypeReference(context, methodDecl!.ImplicitArgType, out implicitArgType))
                        {
                            failed = true;
                        }
                        else
                        {
                            typeDef!.MethodImplicitArgType = implicitArgType;
                        }

                        TypeDefinition? returnType = null;
                        if (!this.TryResolveTypeReference(context, methodDecl!.ReturnType!, out returnType))
                        {
                            failed = true;
                        }
                        else
                        {
                            typeDef!.MethodReturnType = returnType;
                        }

                        foreach (ParameterDeclaration paramDecl in methodDecl.Parameters)
                        {
                            TypeDefinition? paramType = null;
                            if (!this.TryResolveTypeReference(context, paramDecl.Type, out paramType))
                            {
                                failed = true;
                            }
                            else
                            {
                                foreach (string name in paramDecl.ParameterNames)
                                {
                                    typeDef!.MethodParamTypes.Add(paramType!);
                                }
                            }
                        }
                    }
                }

                InterfaceDeclaration? interfaceDecl = typeDecl as InterfaceDeclaration;
                if (interfaceDecl != null)
                {
                    typeDef!.IsInterface = true;
                    typeDef!.IsPublic = interfaceDecl.IsPublic;
                    if (interfaceDecl!.BaseInterfaceType != null)
                    {
                        TypeDefinition? baseType = null;
                        if (!context.TryFindTypeByName(interfaceDecl!.BaseInterfaceType!, out baseType))
                        {
                            string message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.CodeGenerator_UndefinedType,
                                interfaceDecl.BaseInterfaceType);
                            this.log.Write(new Message(
                                interfaceDecl.Start.Path,
                                interfaceDecl.Start.Line,
                                interfaceDecl.Start.Column,
                                Severity.Error,
                                message));
                            failed = true;
                        }

                        if (!baseType!.IsInterface)
                        {
                            string message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.CodeGenerator_BaseInterfaceIsNotInterface,
                                interfaceDecl!.BaseInterfaceType!);
                            this.log.Write(new Message(
                                interfaceDecl!.Start.Path,
                                interfaceDecl!.Start.Line,
                                interfaceDecl!.Start.Column,
                                Severity.Error,
                                message));
                            failed = true;
                        }

                        typeDef.BaseClass = baseType;
                    }

                    foreach (MethodDeclaration meth in interfaceDecl.Methods)
                    {
                        MethodInfo? methodInfo = null;
                        if (!this.TryCreateMethod(context, typeDef!, meth, out methodInfo))
                        {
                            failed = true;
                            continue;
                        }

                        methodInfo!.IsPublic = true;
                        typeDef!.Methods.Add(methodInfo!);
                    }

                    FieldInfo? vtblPtr = typeDef.GetVTablePointer();
                    if (vtblPtr == null)
                    {
                        vtblPtr = typeDef!.AddVTablePointer(context, 0);
                    }

                    typeDef!.Size = vtblPtr!.Type!.Size;
                }

                ClassDeclaration? classDecl = typeDecl as ClassDeclaration;
                if (classDecl != null)
                {
                    typeDef!.IsClass = true;
                    typeDef!.IsPublic = classDecl!.IsPublic;
                    typeDef!.IsStaticClass = classDecl!.IsStatic;
                    if (classDecl!.BaseType != null)
                    {
                        TypeDefinition? baseType = null;
                        if (!context.TryFindTypeByName(classDecl!.BaseType, out baseType))
                        {
                            string message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.CodeGenerator_UndefinedType,
                                classDecl!.BaseType);
                            this.log.Write(new Message(
                                classDecl!.Start.Path,
                                classDecl!.Start.Line,
                                classDecl!.Start.Column,
                                Severity.Error,
                                message));
                            failed = true;
                        }
                        else
                        {
                            if (!baseType!.IsClass || baseType!.IsStaticClass)
                            {
                                string message = string.Format(
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    Properties.Resources.CodeGenerator_BaseTypeNotClass,
                                    baseType!.FullName);
                                this.log.Write(new Message(
                                    classDecl!.Start.Path,
                                    classDecl!.Start.Line,
                                    classDecl!.Start.Column,
                                    Severity.Error,
                                    message));
                                failed = true;
                            }
                            else
                            {
                                typeDef!.BaseClass = baseType;
                            }
                        }
                    }

                    bool hasVirtualMethods = false;
                    foreach (MethodDeclaration meth in classDecl.PublicMethods)
                    {
                        MethodInfo? methodInfo = null;
                        if (!this.TryCreateMethod(context, typeDef, meth, out methodInfo))
                        {
                            failed = true;
                            continue;
                        }

                        methodInfo!.IsPublic = true;
                        typeDef.Methods.Add(methodInfo!);
                        if (methodInfo!.IsVirtual)
                        {
                            hasVirtualMethods = true;
                        }
                    }

                    foreach (MethodDeclaration meth in classDecl.ProtectedMethods)
                    {
                        MethodInfo? methodInfo = null;
                        if (!this.TryCreateMethod(context, typeDef, meth, out methodInfo))
                        {
                            failed = true;
                            continue;
                        }

                        methodInfo!.IsProtected = true;
                        typeDef.Methods.Add(methodInfo!);
                        if (methodInfo!.IsVirtual)
                        {
                            hasVirtualMethods = true;
                        }
                    }

                    foreach (MethodDeclaration meth in classDecl.PrivateMethods)
                    {
                        MethodInfo? methodInfo = null;
                        if (!this.TryCreateMethod(context, typeDef, meth, out methodInfo))
                        {
                            failed = true;
                            continue;
                        }

                        typeDef.Methods.Add(methodInfo!);
                        if (methodInfo!.IsVirtual)
                        {
                            hasVirtualMethods = true;
                        }
                    }

                    int size = 0;
                    if (typeDef.BaseClass != null)
                    {
                        size = typeDef.BaseClass.Size;
                    }

                    if (hasVirtualMethods || classDecl.Interfaces.Count() > 0)
                    {
                        FieldInfo? vtblPtr = typeDef.GetVTablePointer();
                        if (vtblPtr == null)
                        {
                            vtblPtr = typeDef.AddVTablePointer(context, size);
                            size += vtblPtr!.Type!.Size;
                        }
                    }

                    foreach (var intfDecl in classDecl.Interfaces)
                    {
                        TypeDefinition? intfDef = null;
                        if (!context.TryFindTypeByName(intfDecl, out intfDef))
                        {
                            string message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.CodeGenerator_UndefinedType,
                                intfDecl);
                            this.log.Write(new Message(
                                classDecl.Start.Path,
                                classDecl.Start.Line,
                                classDecl.Start.Column,
                                Severity.Error,
                                message));
                            failed = true;
                            continue;
                        }

                        FieldInfo? intfTable = typeDef.GetInterfaceTablePointer(intfDef!);
                        if (intfTable == null)
                        {
                            intfTable = typeDef.AddInterfaceTablePointer(context, size, intfDef!);
                            size += intfTable!.Type!.Size;
                        }

                        typeDef.AddInterface(intfDef!, intfTable!.Offset);

                        TypeDefinition? intf = intfDef;
                        while (intf != null)
                        {
                            foreach (var meth in intf.Methods)
                            {
                                MethodInfo? matchingMeth = typeDef.FindMethod(meth.Name!, meth.Parameters.Select(e => e.Type!).ToList());
                                if (matchingMeth == null || !matchingMeth.IsVirtual)
                                {
                                    failed = true;
                                    string message = string.Format(
                                        System.Globalization.CultureInfo.CurrentCulture,
                                        Properties.Resources.CodeGenerator_ClassDoesNotDeclareInterfaceMethod,
                                        typeDef.FullName,
                                        intf.FullName,
                                        meth.Name);
                                    this.log.Write(new Message(
                                        classDecl.Start.Path,
                                        classDecl.Start.Line,
                                        classDecl.Start.Column,
                                        Severity.Error,
                                        message));
                                }
                            }

                            intf = intf.BaseClass;
                        }
                    }

                    if (classDecl.Fields != null)
                    {
                        foreach (var field in classDecl.Fields.Variables)
                        {
                            TypeDefinition? fieldType = null;
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
                                    if (fieldType!.Size == 0)
                                    {
                                        string message = string.Format(
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            "The Type [{0}] for field [{1}] must be defined before it can be used as a field.",
                                            fieldInfo.Type!.FullName,
                                            identifier);
                                        this.log.Write(new Message(
                                            field.Start.Path,
                                            field.Start.Line,
                                            field.Start.Column,
                                            Severity.Error,
                                            message));
                                        failed = true;
                                    }

                                    size += fieldType!.Size;
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

        /// <summary>
        /// Creates a module from the given parse tree.
        /// </summary>
        /// <param name="context">The compiler context.</param>
        /// <param name="programUnit">The source program unit.</param>
        /// <param name="module">On success, receives the created module.</param>
        /// <returns>True if there were no errors; otherwise, false.</returns>
        public bool CreateModule(CompilerContext context, ProgramUnit programUnit, out Module module)
        {
            bool failed = false;
            module = new Module();
            foreach (MethodDefinition methodDef in programUnit.Methods)
            {
                MethodInfo? methodInfo = null;
                TypeDefinition? type = null;
                List<TypeDefinition> parameterTypes = new List<TypeDefinition>();
                foreach (var paramDecl in methodDef.Parameters)
                {
                    TypeDefinition? paramType = null;
                    if (!this.TryResolveTypeReference(context, paramDecl.Type, out paramType))
                    {
                        return false;
                    }

                    foreach (string paramName in paramDecl.ParameterNames)
                    {
                        parameterTypes.Add(paramType!);
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
                if (this.TryImplementMethod(methodImpl, context, methodDef, type!))
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
            if (method.Method!.Type!.GetVTablePointer() != null)
            {
                method.Module!.DefineVTable(method.Method!.Type!);
            }

            if (method.Method!.Type!.IsClass)
            {
                method.Module!.DefineInterfaceTables(method.Method!.Type!);
            }

            if (!string.IsNullOrEmpty(methodDef.ExternImpl))
            {
                method.Module!.AddExtern(methodDef.ExternImpl!);
                method.Statements.Add(new AsmStatement { Instruction = "jmp " + methodDef.ExternImpl });
                return true;
            }

            Stack<LocalVariable> destructables = new Stack<LocalVariable>();
            Scope scope = context.BeginScope(method.Method!);
            if (string.CompareOrdinal(method.Method!.Name, "constructor") == 0)
            {
                if (method.Method!.Type!.BaseClass != null)
                {
                    MethodInfo? baseInit = null;
                    int argSize = 0;
                    if (methodDef.BaseConstructorArguments.Count > 0)
                    {
                        List<TypeDefinition> argTypes = new List<TypeDefinition>();
                        foreach (Expression arg in methodDef.BaseConstructorArguments.Reverse())
                        {
                            TypeDefinition? valueType = null;
                            if (!this.TryEmitExpression(arg, context, scope, method, out valueType))
                            {
                                return false;
                            }

                            this.PushResult(method, valueType!);
                            argSize += ((valueType!.Size + 3) / 4) * 4;
                            argTypes.Insert(0, valueType!);
                        }

                        baseInit = method.Method.Type.BaseClass.FindConstructor(argTypes);
                        if (baseInit == null)
                        {
                            string message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.CodeGenerator_CannotFindConstructor,
                                method.Method.Type.BaseClass);
                            this.log.Write(new Message(
                                methodDef.Start.Path,
                                methodDef.Start.Line,
                                methodDef.Start.Column,
                                Severity.Error,
                                message));
                            return false;
                        }
                    }
                    else
                    {
                        baseInit = method.Method.Type.BaseClass.GetDefaultConstructor();
                    }

                    if (baseInit != null)
                    {
                        method.Module!.AddProto(baseInit!);
                        method.Statements.Add(new AsmStatement { Instruction = "mov ecx,_this$[ebp]" });
                        method.Statements.Add(new AsmStatement { Instruction = "push ecx" });
                        argSize += 4;
                        method.Statements.Add(new AsmStatement { Instruction = "call " + baseInit!.MangledName });
                        method.Statements.Add(new AsmStatement { Instruction = "add esp," + argSize });
                    }
                }

                FieldInfo? vtable = method.Method!.Type!.GetVTablePointer();
                if (vtable != null)
                {
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[$Vtbl_{0}]", method.Method!.Type!.MangledName) });
                    method.Statements.Add(new AsmStatement { Instruction = "mov ecx,_this$[ebp]" });
                    if (vtable.Offset > 0)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("add ecx,{0}", vtable!.Offset) });
                    }

                    method.Statements.Add(new AsmStatement { Instruction = "mov [ecx],eax" });
                }

                foreach (TypeDefinition intf in method.Method!.Type!.GetAllInterfaces().Select(e => e.Key))
                {
                    FieldInfo? intfTable = method.Method.Type.GetInterfaceTablePointer(intf);
                    string tableSymbol = "$Vtbl_" + intf.MangledName + "_" + method.Method!.Type!.MangledName;
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[{0}]", tableSymbol) });
                    method.Statements.Add(new AsmStatement { Instruction = "mov ecx,_this$[ebp]" });
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("add ecx,{0}", intfTable!.Offset) });
                    method.Statements.Add(new AsmStatement { Instruction = "mov [ecx],eax" });
                }

                // init member vars
                foreach (FieldInfo field in method.Method!.Type!.Fields)
                {
                    if (field.IsStatic || !field.Type!.IsClass)
                    {
                        continue;
                    }

                    MethodInfo? memberConstructor = field.Type.GetDefaultConstructor();
                    if (memberConstructor != null)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "mov ecx,_this$[ebp]" });
                        if (field.Offset > 0)
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("add ecx,{0}", field.Offset) });
                        }

                        method.Statements.Add(new AsmStatement { Instruction = "push ecx" });
                        method.Module!.AddProto(memberConstructor!);
                        method.Statements.Add(new AsmStatement { Instruction = "call " + memberConstructor!.MangledName });
                        method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
                    }
                }
            }

            if (methodDef.LocalVariables != null)
            {
                foreach (VariableDeclaration varDecl in methodDef.LocalVariables!.Variables)
                {
                    TypeDefinition? varType = null;
                    if (!this.TryResolveTypeReference(context, varDecl.Type, out varType))
                    {
                        failed = true;
                        continue;
                    }

                    MethodInfo? defaultConstructor = varType!.GetDefaultConstructor();
                    MethodInfo? destructor = varType!.GetDestructor();
                    foreach (string varName in varDecl.VariableNames)
                    {
                        LocalVariable localVar = scope.DefineLocalVariable(varName, varType!);
                        if (destructor != null)
                        {
                            destructables.Push(localVar);
                        }

                        if (varDecl.InitExpression != null)
                        {
                            TypeDefinition? initValueType = null;
                            if (!this.TryEmitExpression(varDecl.InitExpression, context, scope, method, out initValueType))
                            {
                                failed = true;
                                continue;
                            }

                            if (varType!.IsPointer || (varType!.IsArray && varType!.ArrayElementCount == 0))
                            {
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov _{0}$[ebp],eax", localVar.Name) });
                            }
                            else if (varType!.IsFloatingPoint)
                            {
                                if (varType!.Size == 4)
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = string.Format("fstp dword ptr _{0}$[ebp]", localVar.Name) });
                                }
                                else
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = string.Format("fstp qword ptr _{0}$[ebp]", localVar.Name) });
                                }
                            }
                            else if (!varType!.IsClass)
                            {
                                switch (varType!.Size)
                                {
                                    case 1:
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov byte ptr _{0}$[ebp],al", localVar.Name) });
                                        break;
                                    case 2:
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov word ptr _{0}$[ebp],ax", localVar.Name) });
                                        break;
                                    case 4:
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov _{0}$[ebp],eax", localVar.Name) });
                                        break;
                                }
                            }
                        }
                        else if (defaultConstructor != null)
                        {
                            method.Module!.AddProto(defaultConstructor!);
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,_{0}$[ebp]", localVar.Name) });
                            method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                            method.Statements.Add(new AsmStatement { Instruction = "call " + defaultConstructor!.MangledName });
                            method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
                        }
                    }
                }
            }

            // statements alloc temp variables, so the frame setup code has to be last.
            if (!this.TryEmitBlockStatement(methodDef.Body!, context, scope, method))
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
                MethodInfo? destructor = destructable.Type!.GetDestructor();
                method.Module!.AddProto(destructor!);
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,_{0}$[ebp]", destructable.Name) });
                method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                method.Statements.Add(new AsmStatement { Instruction = "call " + destructor!.MangledName });
                method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
            }

            if (method.Method!.ReturnType != null)
            {
                SymbolEntry returnVar = scope.ReturnVariable!;
                if (method.Method!.ReturnType!.IsFloatingPoint)
                {
                    if (method.Method!.ReturnType!.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("fld qword ptr _{0}$[ebp]", returnVar.Name) });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("fld dword ptr _{0}$[ebp]", returnVar.Name) });
                    }
                }
                else if (method.Method!.ReturnType!.Size <= 8 && !method.Method!.ReturnType!.IsClass)
                {
                    if (method.Method!.ReturnType!.Size > 4)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("lea ecx,dword ptr _{0}$[ebp]", returnVar.Name) });
                        method.Statements.Add(new AsmStatement { Instruction = "mov eax,dword ptr [ecx]" });
                        method.Statements.Add(new AsmStatement { Instruction = "add ecx,4" });
                        switch (method.Method!.ReturnType!.Size)
                        {
                            case 8:
                                method.Statements.Add(new AsmStatement { Instruction = "mov edx,dword ptr [ecx]" });
                                break;
                            case 7:
                                method.Statements.Add(new AsmStatement { Instruction = "mov edx,dword ptr [ecx]" });
                                break;
                            case 6:
                                method.Statements.Add(new AsmStatement { Instruction = "mov dx,word ptr [ecx]" });
                                break;
                            case 5:
                                method.Statements.Add(new AsmStatement { Instruction = "mov dl,byte ptr [ecx]" });
                                break;
                        }
                    }
                    else
                    {
                        switch (method.Method!.ReturnType!.Size)
                        {
                            case 4:
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax, dword ptr _{0}$[ebp]", returnVar.Name) });
                                break;
                            case 3:
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax, dword ptr _{0}$[ebp]", returnVar.Name) });
                                break;
                            case 2:
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov ax, word ptr _{0}$[ebp]", returnVar.Name) });
                                break;
                            case 1:
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov al, byte ptr _{0}$[ebp]", returnVar.Name) });
                                if (string.CompareOrdinal(returnVar.Type!.MangledName, "f") == 0)
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = "and eax,1" });
                                }

                                break;
                        }
                    }
                }
                else
                {
                    method.Statements.Add(
                        new AsmStatement { Instruction = string.Format("mov edx, _{0}$[ebp]", scope.LargeReturnVariable!.Name) });
                    if (!this.TryEmitCopy(
                        string.Format("_{0}$[ebp]", returnVar.Name),
                        "[edx]",
                        returnVar!.Type!,
                        context,
                        scope,
                        method))
                    {
                        failed = true;
                    }
                }
            }

            if (string.CompareOrdinal("destructor", method.Method!.Name) == 0)
            {
                // call destructors on all fields.
                foreach (FieldInfo field in method.Method!.Type!.Fields)
                {
                    if (!field.IsStatic)
                    {
                        if (field.Type!.IsClass)
                        {
                            MethodInfo? memberDestructor = field.Type!.GetDestructor();
                            if (memberDestructor != null)
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "mov ecx,_this$[ebp]" });
                                if (field.Offset > 0)
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = string.Format("add ecx,{0}", field.Offset) });
                                }

                                method.Statements.Add(new AsmStatement { Instruction = "push ecx" });
                                if (memberDestructor!.IsVirtual)
                                {
                                    FieldInfo? memberVTable = field.Type!.GetVTablePointer();
                                    if (memberVTable!.Offset > 0)
                                    {
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("add ecx,{0}", memberVTable!.Offset) });
                                    }

                                    method.Statements.Add(new AsmStatement { Instruction = "mov eax,[ecx]" });
                                    if (memberDestructor!.VTableIndex > 0)
                                    {
                                        method.Statements.Add(new AsmStatement { Instruction = string.Format("add eax,{0}", memberDestructor!.VTableIndex * 4) });
                                    }

                                    method.Statements.Add(new AsmStatement { Instruction = "call dword ptr [eax]" });
                                }
                                else
                                {
                                    method.Module!.AddProto(memberDestructor!);
                                    method.Statements.Add(new AsmStatement { Instruction = "call " + memberDestructor!.MangledName });
                                }

                                method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
                            }
                        }
                    }
                }

                // call base class destructor at the end.
                TypeDefinition? baseClass = method.Method!.Type!.BaseClass;
                if (baseClass != null)
                {
                    MethodInfo? baseDestructor = baseClass.GetDestructor();
                    if (baseDestructor != null)
                    {
                        method.Module!.AddProto(baseDestructor!);
                        method.Statements.Add(new AsmStatement { Instruction = "mov eax,_this$[ebp]" });
                        method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                        method.Statements.Add(new AsmStatement { Instruction = "call " + baseDestructor!.MangledName });
                        method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
                    }
                }
            }

            method.Statements.Add(new AsmStatement { Instruction = "mov esp,ebp" });
            method.Statements.Add(new AsmStatement { Instruction = "pop ebp" });
            method.Statements.Add(new AsmStatement { Instruction = "ret" });
            scope.SaveSymbols(method.Symbols);
            return !failed;
        }

        private bool TryEmitCopy(
            string sourceLocation,
            string targetLocation,
            TypeDefinition type,
            CompilerContext context,
            Scope scope,
            MethodImpl method)
        {
            if (string.CompareOrdinal("[ebx]", sourceLocation) != 0)
            {
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea ebx,{0}", sourceLocation) });
            }

            if (string.CompareOrdinal("[edx]", targetLocation) != 0)
            {
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea edx,{0}", targetLocation) });
            }

            int size = type.Size;
            while (size >= 4)
            {
                method.Statements.Add(new AsmStatement { Instruction = "mov eax, dword ptr [ebx]" });
                method.Statements.Add(new AsmStatement { Instruction = "mov dword ptr [edx],eax" });
                method.Statements.Add(new AsmStatement { Instruction = "add ebx,4" });
                method.Statements.Add(new AsmStatement { Instruction = "add edx,4" });
                size -= 4;
            }

            if (size >= 2)
            {
                method.Statements.Add(new AsmStatement { Instruction = "mov ax, word ptr [ebx]" });
                method.Statements.Add(new AsmStatement { Instruction = "mov word ptr [edx],ax" });
                method.Statements.Add(new AsmStatement { Instruction = "add ebx,2" });
                method.Statements.Add(new AsmStatement { Instruction = "add edx,2" });
                size -= 2;
            }

            if (size >= 1)
            {
                method.Statements.Add(new AsmStatement { Instruction = "mov al, byte ptr [ebx]" });
                method.Statements.Add(new AsmStatement { Instruction = "mov byte ptr [edx],al" });
            }

            return true;
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
            AssignmentStatement? assignmentStatement = statement as AssignmentStatement;
            if (assignmentStatement != null)
            {
                return this.TryEmitAssignment(assignmentStatement, context, scope, method);
            }
            else
            {
                CallStatement? callStatement = statement as CallStatement;
                if (callStatement != null)
                {
                    return this.TryEmitCall(callStatement, context, scope, method);
                }
                else
                {
                    IfStatement? ifStatement = statement as IfStatement;
                    if (ifStatement != null)
                    {
                        return this.TryEmitIfStatement(ifStatement, context, scope, method);
                    }
                    else
                    {
                        WhileStatement? whileStatement = statement as WhileStatement;
                        if (whileStatement != null)
                        {
                            return this.TryEmitWhileStatement(whileStatement, context, scope, method);
                        }
                        else
                        {
                            BlockStatement? blockStatement = statement as BlockStatement;
                            if (blockStatement != null)
                            {
                                return this.TryEmitBlockStatement(blockStatement, context, scope, method);
                            }
                            else
                            {
                                DeleteStatement? deleteStatement = statement as DeleteStatement;
                                if (deleteStatement != null)
                                {
                                    return this.TryEmitDeleteStatement(deleteStatement, context, scope, method);
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
            TypeDefinition? valueType = null;
            if (!this.TryEmitExpression(deleteStatement.Value, context, scope, method, out valueType))
            {
                return false;
            }

            if (!valueType!.IsPointer && !valueType!.IsArray)
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

            if (valueType!.IsPointer)
            {
                this.PushResult(method, valueType!);
                TypeDefinition? innerType = valueType!.InnerType;
                if (innerType != null)
                {
                    MethodInfo? destructor = innerType.GetDestructor();
                    if (destructor != null)
                    {
                        this.PushResult(method, valueType!);
                        method.Module!.AddProto(destructor);
                        if (destructor!.IsVirtual)
                        {
                            FieldInfo? vtblPointer = innerType.GetVTablePointer();
                            if (vtblPointer!.Offset > 0)
                            {
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("add eax,{0}", vtblPointer!.Offset) });
                            }

                            method.Statements.Add(new AsmStatement { Instruction = "mov ecx,[eax]" });
                            if (destructor.VTableIndex > 0)
                            {
                                method.Statements.Add(new AsmStatement { Instruction = string.Format("add ecx,{0}", destructor.VTableIndex * 4) });
                            }

                            method.Statements.Add(new AsmStatement { Instruction = "call dword ptr [ecx]" });
                        }
                        else
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("call {0}", destructor.MangledName) });
                        }

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

            this.log.Write(new Message(
                deleteStatement.Start.Path,
                deleteStatement.Start.Line,
                deleteStatement.Start.Column,
                Severity.Error,
                Properties.Resources.CodeGenerator_DeleteOperandArrayNotSupported));
            return false;
        }

        private bool TryFindSystemMemory(Token start, CompilerContext context, out TypeDefinition? memoryType)
        {
            if (!context.TryFindTypeByName("System.Memory", out memoryType))
            {
                this.log.Write(new Message(
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
            TypeDefinition? memoryType = null;
            if (!this.TryFindSystemMemory(start, context, out memoryType))
            {
                return false;
            }

            MethodInfo? freeMethod = memoryType!.Methods.FirstOrDefault(e => string.CompareOrdinal(e.Name, "Free") == 0);
            if (freeMethod == null)
            {
                this.log.Write(new Message(
                    start.Path,
                    start.Line,
                    start.Column,
                    Severity.Error,
                    Properties.Resources.CodeGenerator_SystemMemoryMissingFree));
                return false;
            }

            method.Module!.AddProto(freeMethod!);
            method.Statements.Add(new AsmStatement { Instruction = string.Format("call {0}", freeMethod!.MangledName) });
            return true;
        }

        private bool TryEmitIfStatement(IfStatement ifStatement, CompilerContext context, Scope scope, MethodImpl method)
        {
            TypeDefinition? conditionType = null;
            if (!this.TryEmitExpression(ifStatement.Condition!, context, scope, method, out conditionType))
            {
                return false;
            }

            if (!this.ValidateConditionType(ifStatement.Condition!, conditionType!))
            {
                return false;
            }

            string? elseLabel = null;
            if (ifStatement.FalseStatement != null)
            {
                elseLabel = method.Module!.GetNextJumpLabel();
            }

            string endLabel = method.Module!.GetNextJumpLabel();
            if (elseLabel == null)
            {
                elseLabel = endLabel;
            }

            method.Statements.Add(new AsmStatement { Instruction = "test al,al" });
            method.Statements.Add(new AsmStatement { Instruction = "jz " + elseLabel });
            if (!this.TryEmitStatement(ifStatement.TrueStatement!, context, scope, method))
            {
                return false;
            }

            if (ifStatement.FalseStatement != null)
            {
                method.Statements.Add(new AsmStatement { Instruction = "jmp " + endLabel });
                method.Statements.Add(new AsmStatement { Instruction = "nop", Label = elseLabel });
                if (!this.TryEmitStatement(ifStatement.FalseStatement!, context, scope, method))
                {
                    return false;
                }
            }

            method.Statements.Add(new AsmStatement { Instruction = "nop", Label = endLabel });
            return true;
        }

        private bool TryEmitWhileStatement(WhileStatement whileStatement, CompilerContext context, Scope scope, MethodImpl method)
        {
            string topLabel = method.Module!.GetNextJumpLabel();
            method.Statements.Add(new AsmStatement { Instruction = "nop", Label = topLabel });
            TypeDefinition? conditionType = null;
            if (!this.TryEmitExpression(whileStatement.Condition!, context, scope, method, out conditionType))
            {
                return false;
            }

            if (!this.ValidateConditionType(whileStatement.Condition!, conditionType!))
            {
                return false;
            }

            string endLabel = method.Module.GetNextJumpLabel();

            method.Statements.Add(new AsmStatement { Instruction = "test al,al" });
            method.Statements.Add(new AsmStatement { Instruction = "jz " + endLabel });
            if (!this.TryEmitStatement(whileStatement.BodyStatement!, context, scope, method))
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
            TypeDefinition? typeDef = null;
            return this.TryEmitExpression(callStatement.Expression, context, scope, method, out typeDef);
        }

        private bool TryEmitAssignment(
            AssignmentStatement assignmentStatement,
            CompilerContext context,
            Scope scope,
            MethodImpl method)
        {
            string? location = null;
            TypeDefinition? storageType = null;
            TypeDefinition? valueType = null;
            if (!this.TryEmitExpression(assignmentStatement.Value, context, scope, method, out valueType))
            {
                return false;
            }

            this.PushResult(method, valueType!);

            MethodInfo? calleeMethod = null;
            if (!this.TryEmitReference(assignmentStatement.Storage, context, scope, method, out location, out storageType, out calleeMethod))
            {
                return false;
            }

            int castOffset = 0;
            if (!this.ValidateCanCast(assignmentStatement, storageType!, valueType!, out castOffset))
            {
                return false;
            }

            if (valueType!.Size == 8 && !valueType!.IsClass)
            {
                method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                method.Statements.Add(new AsmStatement { Instruction = "pop edx" });
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea ecx,{0}", location) });
                method.Statements.Add(new AsmStatement { Instruction = "mov [ecx],eax" });
                method.Statements.Add(new AsmStatement { Instruction = "add ecx,4" });
                method.Statements.Add(new AsmStatement { Instruction = "mov [ecx],edx" });
            }
            else if (valueType!.Size <= 4 && !valueType!.IsClass)
            {
                method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                if (castOffset > 0)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "add eax," + castOffset });
                }

                switch (storageType!.Size)
                {
                    case 1:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov byte ptr {0},al", location) });
                        }

                        break;
                    case 2:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov word ptr {0},ax", location) });
                        }

                        break;
                    default:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("mov {0},eax", location) });
                        }

                        break;
                }
            }
            else
            {
                method.Statements.Add(new AsmStatement { Instruction = "mov esi,esp" });
                method.Statements.Add(new AsmStatement { Instruction = string.Format("lea edi,{0}", location) });
                method.Statements.Add(new AsmStatement { Instruction = string.Format("mov ecx,{0}", valueType!.Size) });
                method.Statements.Add(new AsmStatement { Instruction = "cld" });
                method.Statements.Add(new AsmStatement { Instruction = "rep movsb" });
                method.Statements.Add(new AsmStatement { Instruction = string.Format("add esp,{0}", valueType!.Size) });
            }

            return true;
        }

        private bool ValidateCanCast(
            ParseNode node,
            TypeDefinition storageType,
            TypeDefinition valueType,
            out int offset)
        {
            offset = 0;
            if (string.CompareOrdinal(storageType.MangledName, valueType.MangledName) == 0)
            {
                return true;
            }

            bool canCast = false;
            if (valueType.IsArray && storageType.IsPointer)
            {
                if (valueType.ArrayElementCount == 0)
                {
                    canCast = storageType.InnerType == null ||
                        string.CompareOrdinal(storageType.InnerType!.MangledName, "b") == 0 ||
                        string.CompareOrdinal(valueType.InnerType!.MangledName, "b") == 0 ||
                        this.ValidateCanCast(node, storageType.InnerType!, valueType.InnerType!, out offset);
                }
            }
            else if (valueType.IsPointer && storageType.IsArray)
            {
                if (storageType.ArrayElementCount == 0)
                {
                    canCast = valueType.InnerType == null ||
                        string.CompareOrdinal(storageType.InnerType!.MangledName, "b") == 0 ||
                        string.CompareOrdinal(valueType.InnerType!.MangledName, "b") == 0 ||
                        this.ValidateCanCast(node, storageType.InnerType!, valueType.InnerType!, out offset);
                }
            }
            else if (valueType.IsPointer && storageType.IsPointer)
            {
                canCast = storageType.InnerType == null ||
                    valueType.InnerType == null;
                if (!canCast)
                {
                    if (valueType.InnerType!.IsClass && storageType.InnerType!.IsInterface)
                    {
                        FieldInfo? intfTable = valueType.InnerType!.GetInterfaceTablePointer(storageType.InnerType!);
                        canCast = intfTable != null;
                        if (canCast)
                        {
                            offset = intfTable!.Offset;
                        }
                    }
                    else if ((valueType.InnerType!.IsClass && storageType.InnerType!.IsClass) ||
                        (valueType.InnerType!.IsInterface && storageType.InnerType!.IsInterface))
                    {
                        TypeDefinition? testClass = valueType.InnerType;
                        while (!canCast && testClass != null)
                        {
                            if (string.CompareOrdinal(testClass!.MangledName, storageType.InnerType!.MangledName) == 0)
                            {
                                canCast = true;
                            }
                            else
                            {
                                testClass = testClass!.BaseClass;
                            }
                        }
                    }
                    else
                    {
                        canCast = string.CompareOrdinal(storageType.InnerType!.MangledName, "b") == 0 ||
                            string.CompareOrdinal(valueType.InnerType!.MangledName, "b") == 0 ||
                            this.ValidateCanCast(node, storageType.InnerType!, valueType.InnerType!, out offset);
                    }
                }
            }
            else if (valueType.IsArray && storageType.IsArray)
            {
                if (storageType.ArrayElementCount == 0)
                {
                    canCast = this.ValidateCanCast(node, storageType.InnerType!, valueType.InnerType!, out offset);
                }
            }
            else if (valueType.IsMethod && storageType.IsMethod)
            {
                if ((valueType.MethodReturnType == null) == (storageType.MethodReturnType == null))
                {
                    canCast = true;
                    if (valueType.MethodReturnType != null)
                    {
                        canCast = string.CompareOrdinal(
                            storageType.MethodReturnType?.MangledName,
                            valueType.MethodReturnType?.MangledName) == 0;
                    }
                }

                if (canCast)
                {
                    if ((storageType.MethodImplicitArgType == null) == (valueType.MethodImplicitArgType != null))
                    {
                        if (valueType.MethodImplicitArgType != null)
                        {
                            canCast = this.ValidateCanCast(
                                node,
                                valueType.MethodImplicitArgType!,
                                storageType.MethodImplicitArgType!,
                                out offset);
                        }
                    }
                }

                if (canCast)
                {
                    canCast = storageType.MethodParamTypes.Count == valueType.MethodParamTypes.Count;
                    if (canCast)
                    {
                        for (int i = 0; i < storageType.MethodParamTypes.Count; i++)
                        {
                            if (string.CompareOrdinal(
                                storageType.MethodParamTypes[i].MangledName,
                                valueType.MethodParamTypes[i].MangledName) != 0)
                            {
                                canCast = false;
                                break;
                            }
                        }
                    }
                }
            }

            if (!canCast)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    Properties.Resources.CodeGenerator_NoAutomaticConversion,
                    storageType.FullName,
                    valueType.FullName);
                this.log.Write(new Message(
                    node.Start.Path,
                    node.Start.Line,
                    node.Start.Column,
                    Severity.Error,
                    message));
            }

            return canCast;
        }

        private bool TryEmitLiteralExpression(
            LiteralExpression expression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition? valueType)
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
                string label = method.Module!.DefineLiteralString((string)expression.Value);
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
                    string label = method.Module!.DefineConstant((double)exprValue);
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
            out TypeDefinition? valueType)
        {
            LiteralExpression? literalExpr = expression as LiteralExpression;
            if (literalExpr != null)
            {
                return this.TryEmitLiteralExpression(literalExpr, context, scope, method, out valueType);
            }

            CallReferenceExpression? callExpr = expression as CallReferenceExpression;
            if (callExpr != null)
            {
                int argSize = 0;
                int argStatementStart = method.Statements.Count;
                Expression[] arguments = callExpr.Arguments.ToArray();
                List<TypeDefinition> argTypes = new List<TypeDefinition>();
                List<int> argValueOffsets = new List<int>();
                for (int i = arguments.Length - 1; i >= 0; i--)
                {
                    TypeDefinition? argType = null;
                    if (!this.TryEmitExpression(arguments[i], context, scope, method, out argType))
                    {
                        valueType = null;
                        return false;
                    }

                    argTypes.Insert(0, argType!);
                    argSize += ((argType!.Size + 3) / 4) * 4;
                    argValueOffsets.Insert(0, method.Statements.Count);
                    this.PushResult(method, argType!);
                }

                bool hasDestructables = argTypes.Any(e => e.GetDestructor() != null);
                bool savedRegResult = false;

                string? callLoc = null;
                TypeDefinition? storageType = null;
                MethodInfo? calleeMethod = null;
                if (!this.TryEmitReference(callExpr.Inner, context, scope, method, out callLoc, out storageType, out calleeMethod))
                {
                    valueType = null;
                    return false;
                }

                // fix up calling overloads here vs. inside TryEmitReference.
                if (calleeMethod != null)
                {
                    MethodInfo? overload = calleeMethod!.Type!.FindMethod(calleeMethod!.Name!, argTypes);
                    if (overload != null)
                    {
                        if (string.CompareOrdinal(calleeMethod!.MangledName, callLoc) == 0)
                        {
                            callLoc = overload!.MangledName;
                        }

                        method.Module!.AddProto(overload!);
                        calleeMethod = overload;
                        storageType = overload!.ReturnType;
                    }
                }

                if (storageType != null)
                {
                    if (callLoc == null)
                    {
                        // must be a constructor call.
                        MethodInfo? constructor = storageType.FindConstructor(argTypes);
                        if (constructor == null)
                        {
                            string message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.CodeGenerator_CannotFindConstructor,
                                storageType.FullName);
                            this.log.Write(new Message(
                                callExpr.Start.Path,
                                callExpr.Start.Line,
                                callExpr.Start.Column,
                                Severity.Error,
                                message));
                            valueType = null;
                            return false;
                        }

                        calleeMethod = constructor;
                        callLoc = constructor.MangledName;
                        method.Module!.AddProto(constructor);

                        // create room on the stack for the result.
                        AsmStatement resultPush = new AsmStatement { Instruction = string.Format("sub esp,{0}", storageType.Size) };
                        method.Statements.Insert(argStatementStart, resultPush);

                        // push the ref to the storage for the class on the stack.
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,[esp+{0}]", argSize) });
                        method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                    }
                    else if (storageType.IsClass || (!storageType.IsFloatingPoint && storageType.Size > 8))
                    {
                        // create room on the stack for the result.
                        AsmStatement resultPush = new AsmStatement { Instruction = string.Format("sub esp,{0}", storageType.Size) };
                        method.Statements.Insert(argStatementStart, resultPush);

                        // push the ref to the storage for the class on the stack.
                        method.Statements.Add(new AsmStatement
                            {
                                Instruction = string.Format(
                                    "lea ebx,[esp+{0}]",
                                    calleeMethod!.IsStatic ? argSize : argSize + 4),
                            });
                        method.Statements.Add(new AsmStatement { Instruction = "push ebx" });
                        argSize += 4;
                    }
                    else if (hasDestructables)
                    {
                        // create room on the stack for the result.
                        AsmStatement resultPush = new AsmStatement { Instruction = string.Format("sub esp,{0}", storageType.Size) };
                        method.Statements.Insert(argStatementStart, resultPush);
                        savedRegResult = true;
                    }
                }

                if (calleeMethod == null && storageType != null && storageType.IsMethod)
                {
                    calleeMethod = storageType.CreateMethodInfoForMethodType();
                    storageType = calleeMethod.ReturnType;
                }

                if (calleeMethod!.IsVirtual && callExpr!.Inner!.UseVirtualDispatch)
                {
                    FieldInfo? vtablePtr = calleeMethod!.Type!.GetVTablePointer();
                    if (string.CompareOrdinal(callLoc, "[eax]") != 0)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,{0}", callLoc) });
                    }

                    if (vtablePtr!.Offset > 0)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("add eax,{0}", vtablePtr!.Offset) });
                    }

                    method.Statements.Add(new AsmStatement { Instruction = "mov ecx,[eax]" });
                    if (calleeMethod!.VTableIndex > 0)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("add ecx,{0}", calleeMethod!.VTableIndex * 4) });
                    }

                    method.Statements.Add(new AsmStatement { Instruction = "call dword ptr [ecx]" });
                }
                else
                {
                    if (callLoc!.Contains("["))
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "call dword ptr " + callLoc! });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "call " + callLoc! });
                    }
                }

                if (!calleeMethod.IsStatic)
                {
                    argSize += 4;
                }

                if (argSize > 0)
                {
                    if (hasDestructables)
                    {
                        if (savedRegResult)
                        {
                            if (storageType!.IsFloatingPoint)
                            {
                                switch (storageType!.Size)
                                {
                                    case 10:
                                        method.Statements.Add(new AsmStatement { Instruction = "fstp tword ptr [esp+" + argSize + "]" });
                                        break;
                                    case 8:
                                        method.Statements.Add(new AsmStatement { Instruction = "fstp qword ptr [esp+" + argSize + "]" });
                                        break;
                                    case 4:
                                        method.Statements.Add(new AsmStatement { Instruction = "fstp dword ptr [esp+" + argSize + "]" });
                                        break;
                                }
                            }
                            else
                            {
                                if (storageType!.Size > 4)
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = "mov dword ptr [esp+" + (argSize + 4) + "],edx" });
                                }

                                switch (storageType!.Size % 4)
                                {
                                    case 0:
                                        method.Statements.Add(new AsmStatement { Instruction = "mov dword ptr [esp+" + argSize + "],eax" });
                                        break;
                                    case 3:
                                        method.Statements.Add(new AsmStatement { Instruction = "mov dword ptr [esp+" + argSize + "],eax" });
                                        break;
                                    case 2:
                                        method.Statements.Add(new AsmStatement { Instruction = "mov word ptr [esp+" + argSize + "],ax" });
                                        break;
                                    case 1:
                                        method.Statements.Add(new AsmStatement { Instruction = "mov byte ptr [esp+" + argSize + "],al" });
                                        break;
                                }
                            }
                        }

                        if (!calleeMethod!.IsStatic)
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
                        }

                        foreach (var arg in argTypes)
                        {
                            MethodInfo? argDes = arg.GetDestructor();
                            if (argDes != null)
                            {
                                method.Module!.AddProto(argDes);
                                method.Statements.Add(new AsmStatement { Instruction = "lea ecx,[esp]" });
                                method.Statements.Add(new AsmStatement { Instruction = "push ecx" });
                                method.Statements.Add(new AsmStatement { Instruction = "call " + argDes!.MangledName });
                                method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
                            }

                            method.Statements.Add(new AsmStatement { Instruction = "add esp," + arg!.Size });
                        }

                        if (savedRegResult)
                        {
                            if (storageType!.IsFloatingPoint)
                            {
                                switch (storageType!.Size)
                                {
                                    case 10:
                                        method.Statements.Add(new AsmStatement { Instruction = "fld tword ptr [esp]" });
                                        break;
                                    case 8:
                                        method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr [esp]" });
                                        break;
                                    case 4:
                                        method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr [esp]" });
                                        break;
                                }

                                method.Statements.Add(new AsmStatement { Instruction = "add esp," + storageType!.Size });
                            }
                            else
                            {
                                if (storageType!.Size > 4)
                                {
                                    method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                                    method.Statements.Add(new AsmStatement { Instruction = "pop edx" });
                                }
                                else
                                {
                                    switch (storageType!.Size)
                                    {
                                        case 4:
                                            method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                                            break;
                                        case 3:
                                            method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                                            break;
                                        case 2:
                                            method.Statements.Add(new AsmStatement { Instruction = "pop ax" });
                                            break;
                                        case 1:
                                            method.Statements.Add(new AsmStatement { Instruction = "pop al" });
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "add esp," + argSize.ToString() });
                    }
                }

                if (calleeMethod!.Parameters.Count != argTypes.Count)
                {
                    string message = string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        Properties.Resources.CodeGenerator_UnexpectedArgumentCount,
                        calleeMethod.Name,
                        calleeMethod!.Parameters.Count,
                        argTypes.Count);
                    this.log.Write(new Message(
                        expression.Start.Path,
                        expression.Start.Line,
                        expression.Start.Column,
                        Severity.Error,
                        message));
                    valueType = null;
                    return false;
                }

                bool argsValid = true;
                for (int i = 0; i < calleeMethod!.Parameters.Count; i++)
                {
                    int pointerCastOffset = 0;
                    if (!this.ValidateCanCast(expression, calleeMethod!.Parameters[i].Type!, argTypes[i], out pointerCastOffset))
                    {
                        argsValid = false;
                    }

                    if (pointerCastOffset > 0)
                    {
                        method.Statements.Insert(
                            argValueOffsets[i],
                            new AsmStatement { Instruction = "add eax," + pointerCastOffset });
                    }
                }

                valueType = storageType;
                return argsValid;
            }

            ReferenceExpression? refExpr = expression as ReferenceExpression;
            if (refExpr != null)
            {
                string? location = null;
                TypeDefinition? storageType = null;
                MethodInfo? calleeMethod = null;
                if (!this.TryEmitReference(refExpr!, context, scope, method, out location, out storageType, out calleeMethod))
                {
                    valueType = null;
                    return false;
                }

                // method pointer.
                if (storageType == null)
                {
                    if (!calleeMethod!.IsVirtual)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "mov eax, " + calleeMethod!.MangledName });
                    }

                    valueType = context.GetMethodType(calleeMethod!);
                    return true;
                }

                if (storageType!.IsFloatingPoint)
                {
                    if (storageType.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr " + location });
                    }
                    else if (storageType.Size == 4)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr " + location });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld tword ptr " + location });
                    }
                }
                else if (storageType!.IsArray)
                {
                    if (storageType!.ArrayElementCount > 0)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "lea eax," + location });
                        valueType = context.GetArrayType(storageType!.InnerType!, 0);
                        return true;
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "mov eax," + location });
                    }
                }
                else if (storageType!.Size <= 8 && !storageType!.IsClass)
                {
                    if (location != null)
                    {
                        switch (storageType!.Size)
                        {
                            case 8:
                                method.Statements.Add(new AsmStatement { Instruction = "lea ecx, " + location! });
                                method.Statements.Add(new AsmStatement { Instruction = "mov eax,[ecx]" });
                                method.Statements.Add(new AsmStatement { Instruction = "add ecx,4" });
                                method.Statements.Add(new AsmStatement { Instruction = "mov edx,[ecx]" });
                                break;
                            case 4:
                                method.Statements.Add(new AsmStatement { Instruction = "mov eax,dword ptr " + location! });
                                break;
                            case 2:
                                method.Statements.Add(new AsmStatement { Instruction = "mov ax,word ptr " + location! });
                                break;
                            case 1:
                                method.Statements.Add(new AsmStatement { Instruction = "mov al,byte ptr " + location! });
                                break;
                        }
                    }
                }
                else if (storageType!.IsClass)
                {
                    // construct a new copy on the stack.
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,{0}", location) });
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("sub esp,{0}", storageType!.Size) });
                    MethodInfo? copyConstructor = storageType!.GetCopyConstructor(context);
                    if (copyConstructor != null)
                    {
                        method.Module!.AddProto(copyConstructor!);
                        method.Statements.Add(new AsmStatement { Instruction = "lea ecx,[esp]" });
                        method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                        method.Statements.Add(new AsmStatement { Instruction = "push ecx" });
                        method.Statements.Add(new AsmStatement { Instruction = "call " + copyConstructor!.MangledName });
                        method.Statements.Add(new AsmStatement { Instruction = "add esp,8" });
                    }
                    else
                    {
                        // raw copy.
                        method.Statements.Add(new AsmStatement { Instruction = "mov edi,esp" });
                        method.Statements.Add(new AsmStatement { Instruction = "mov esi,eax" });
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov ecx,{0}", storageType.Size) });
                        method.Statements.Add(new AsmStatement { Instruction = "cld" });
                        method.Statements.Add(new AsmStatement { Instruction = "rep movsb" });
                    }
                }

                valueType = storageType;
                return true;
            }

            SimpleExpression? simpleExpr = expression as SimpleExpression;
            if (simpleExpr != null)
            {
                return this.TryEmitSimpleExpression(simpleExpr, context, scope, method, out valueType);
            }

            TermExpression? termExpr = expression as TermExpression;
            if (termExpr != null)
            {
                return this.TryEmitTermExpression(termExpr, context, scope, method, out valueType);
            }

            RelationalExpression? relExpr = expression as RelationalExpression;
            if (relExpr != null)
            {
                return this.TryEmitRelationExpresion(relExpr, context, scope, method, out valueType);
            }

            AddressExpression? addrExpr = expression as AddressExpression;
            if (addrExpr != null)
            {
                return this.TryEmitAddressExpression(addrExpr, context, scope, method, out valueType);
            }

            NewExpression? newExpr = expression as NewExpression;
            if (newExpr != null)
            {
                return this.TryEmitNewExpression(newExpr, context, scope, method, out valueType);
            }

            NotExpression? notExpr = expression as NotExpression;
            if (notExpr != null)
            {
                return this.TryEmitNotExpression(notExpr, context, scope, method, out valueType);
            }

            NegativeExpression? negExpr = expression as NegativeExpression;
            if (negExpr != null)
            {
                return this.TryEmitNegativeExpression(negExpr, context, scope, method, out valueType);
            }

            valueType = null;
            return false;
        }

        private bool TryEmitNegativeExpression(
            NegativeExpression expression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition? valueType)
        {
            if (!this.TryEmitExpression(expression.Inner!, context, scope, method, out valueType))
            {
                return false;
            }

            if (valueType!.IsFloatingPoint)
            {
                method.Statements.Add(new AsmStatement { Instruction = "fchs" });
            }
            else if (valueType!.IsPointer || valueType!.IsClass || valueType!.IsArray || valueType!.IsInterface)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.CodeGenerator_NegativeNotSupported,
                    valueType!.FullName);
                this.log.Write(new Message(
                    expression.Start.Path,
                    expression.Start.Line,
                    expression.Start.Column,
                    Severity.Error,
                    message));
                return false;
            }
            else
            {
                method.Statements.Add(new AsmStatement { Instruction = "neg eax" });
            }

            return true;
        }

        private bool TryEmitNotExpression(
            NotExpression expression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition? valueType)
        {
            if (!this.TryEmitExpression(expression.Inner!, context, scope, method, out valueType))
            {
                return false;
            }

            if (valueType!.IsPointer || valueType!.IsClass || valueType!.IsArray || valueType!.IsInterface || valueType!.IsFloatingPoint
                || valueType!.Size > 4)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Properties.Resources.CodeGenerator_NotNotSupported,
                    valueType!.FullName);
                this.log.Write(new Message(
                    expression.Start.Path,
                    expression.Start.Line,
                    expression.Start.Column,
                    Severity.Error,
                    message));
                return false;
            }
            else
            {
                switch (valueType!.Size)
                {
                    case 4:
                        method.Statements.Add(new AsmStatement { Instruction = "not eax" });
                        break;
                    case 2:
                        method.Statements.Add(new AsmStatement { Instruction = "not ax" });
                        break;
                    case 1:
                        method.Statements.Add(new AsmStatement { Instruction = "not al" });
                        break;
                }

                if (string.CompareOrdinal(valueType!.MangledName, "f") == 0)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "and al,1" });
                }
            }

            return true;
        }

        private bool TryEmitNewExpression(
            NewExpression newExpr,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition? valueType)
        {
            valueType = null;
            TypeDefinition? objectType = null;
            if (!this.TryResolveTypeReference(context, newExpr.Type, out objectType))
            {
                valueType = null;
                return false;
            }

            if (!objectType!.IsClass && newExpr.ConstructorArguments.Count() > 0)
            {
                this.log.Write(new Message(
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

            MethodInfo? constructor = null;
            int argSize = 0;
            if (objectType.IsClass)
            {
                int nullTestStart = method.Statements.Count;
                List<TypeDefinition> argTypes = new List<TypeDefinition>();
                foreach (Expression arg in newExpr.ConstructorArguments.Reverse())
                {
                    TypeDefinition? argType = null;
                    if (!this.TryEmitExpression(arg, context, scope, method, out argType))
                    {
                        return false;
                    }

                    this.PushResult(method, argType!);
                    argSize += ((argType!.Size + 3) / 4) * 4;
                    argTypes.Insert(0, argType!);
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
                        this.log.Write(new Message(
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
                    string jumpLabel = method.Module!.GetNextJumpLabel();
                    List<AsmStatement> saveAlloc = new List<AsmStatement>();
                    saveAlloc.Add(new AsmStatement { Instruction = string.Format("mov _{0}$[ebp],eax", tempPtr.Name) });
                    saveAlloc.Add(new AsmStatement { Instruction = "test eax,eax" });
                    saveAlloc.Add(new AsmStatement { Instruction = "jz " + jumpLabel });
                    method.Statements.InsertRange(nullTestStart, saveAlloc);
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax,_{0}$[ebp]", tempPtr.Name) });
                    method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                    method.Module.AddProto(constructor);
                    method.Statements.Add(new AsmStatement { Instruction = "call " + constructor!.MangledName });
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("add esp,{0}", argSize + 4) });
                    method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax,_{0}$[ebp]", tempPtr.Name), Label = jumpLabel });
                }
            }

            return true;
        }

        private bool TryEmitAllocCall(Token start, CompilerContext context, Scope scope, MethodImpl method, int size)
        {
            TypeDefinition? memoryType = null;
            if (!this.TryFindSystemMemory(start, context, out memoryType))
            {
                return false;
            }

            MethodInfo? allocMethod = memoryType!.Methods.FirstOrDefault(e => string.CompareOrdinal("Alloc", e.Name) == 0);
            if (allocMethod == null)
            {
                this.log.Write(new Message(
                    start.Path,
                    start.Line,
                    start.Column,
                    Severity.Error,
                    Properties.Resources.CodeGenerator_SystemMemoryMissingAlloc));
                return false;
            }

            method.Module!.AddProto(allocMethod!);
            method.Statements.Add(new AsmStatement { Instruction = "mov eax," + size.ToString() });
            method.Statements.Add(new AsmStatement { Instruction = "push eax" });
            method.Statements.Add(new AsmStatement { Instruction = "call " + allocMethod!.MangledName });
            method.Statements.Add(new AsmStatement { Instruction = "add esp,4" });
            return true;
        }

        private bool TryEmitAddressExpression(
            AddressExpression addrExpr,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition? valueType)
        {
            string? innerLoc = null;
            TypeDefinition? storageType = null;
            MethodInfo? calleeMethod = null;
            if (!this.TryEmitReference(addrExpr.Inner, context, scope, method, out innerLoc, out storageType, out calleeMethod))
            {
                valueType = null;
                return false;
            }

            method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,{0}", innerLoc) });
            valueType = context.GetPointerType(storageType!);
            return true;
        }

        private bool TryEmitRelationExpresion(
            RelationalExpression relExpr,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition? valueType)
        {
            context.TryFindTypeByName("boolean", out valueType);
            TypeDefinition? rightSideType = null;
            if (!this.TryEmitExpression(relExpr.Right, context, scope, method, out rightSideType))
            {
                return false;
            }

            this.PushResult(method, rightSideType!);
            TypeDefinition? leftSideType = null;
            if (!this.TryEmitExpression(relExpr.Left, context, scope, method, out leftSideType))
            {
                return false;
            }

            if (rightSideType!.IsFloatingPoint)
            {
                if (leftSideType!.IsFloatingPoint)
                {
                    switch (rightSideType!.Size)
                    {
                        case 4:
                            method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr [esp]" });
                            break;
                        case 8:
                            method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr [esp]" });
                            break;
                        default:
                            method.Statements.Add(new AsmStatement { Instruction = "fld tword ptr [esp]" });
                            break;
                    }

                    method.Statements.Add(new AsmStatement { Instruction = string.Format("add esp,{0}", rightSideType!.Size) });
                    method.Statements.Add(new AsmStatement { Instruction = "fcompp" });
                    method.Statements.Add(new AsmStatement { Instruction = "fnstsw	ax" });
                    method.Statements.Add(new AsmStatement { Instruction = "sahf" });
                    switch (relExpr.Operator)
                    {
                        case Keyword.Equals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "sete al" });
                            }

                            break;
                        case Keyword.LessThan:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "seta al" });
                            }

                            break;
                        case Keyword.LessThanOrEquals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setae al" });
                            }

                            break;
                        case Keyword.GreaterThan:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setb al" });
                            }

                            break;
                        case Keyword.GreaterThanOrEquals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setbe al" });
                            }

                            break;
                        case Keyword.NotEqual:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setne al" });
                            }

                            break;
                    }
                }
                else
                {
                    string message = string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        Properties.Resources.CodeGenerator_NoAutomaticConversion,
                        leftSideType!.FullName,
                        rightSideType!.FullName);
                    this.log.Write(new Message(
                        relExpr.Start.Path,
                        relExpr.Start.Line,
                        relExpr.Start.Column,
                        Severity.Error,
                        message));
                    return false;
                }
            }
            else if (leftSideType!.IsFloatingPoint)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    Properties.Resources.CodeGenerator_NoAutomaticConversion,
                    leftSideType!.FullName,
                    rightSideType!.FullName);
                this.log.Write(new Message(
                    relExpr.Start.Path,
                    relExpr.Start.Line,
                    relExpr.Start.Column,
                    Severity.Error,
                    message));
                return false;
            }
            else
            {
                if (leftSideType!.Size <= 4 && rightSideType!.Size <= 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "pop ecx" });
                    method.Statements.Add(new AsmStatement { Instruction = "cmp eax,ecx" });
                    switch (relExpr.Operator)
                    {
                        case Keyword.Equals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "sete al" });
                            }

                            break;
                        case Keyword.LessThan:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setl al" });
                            }

                            break;
                        case Keyword.LessThanOrEquals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setle al" });
                            }

                            break;
                        case Keyword.GreaterThan:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setg al" });
                            }

                            break;
                        case Keyword.GreaterThanOrEquals:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setge al" });
                            }

                            break;
                        case Keyword.NotEqual:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "setne al" });
                            }

                            break;
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
                else if (type.Size == 8)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "sub esp,8" });
                    method.Statements.Add(new AsmStatement { Instruction = "fstp qword ptr [esp]" });
                }
                else
                {
                    method.Statements.Add(new AsmStatement { Instruction = "sub esp,10" });
                    method.Statements.Add(new AsmStatement { Instruction = "fstp tword ptr [esp]" });
                }
            }
            else if (type.IsArray)
            {
                method.Statements.Add(new AsmStatement { Instruction = "push eax" });
            }
            else if (!type.IsClass)
            {
                if (type.Size <= 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                }
                else if (type.Size == 8)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "push edx" });
                    method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                }
            }
        }

        private bool TryEmitSimpleExpression(
            SimpleExpression expression,
            CompilerContext context,
            Scope scope,
            MethodImpl method,
            out TypeDefinition? valueType)
        {
            TypeDefinition? rightSideType = null;
            if (!this.TryEmitExpression(expression.Right, context, scope, method, out rightSideType))
            {
                valueType = null;
                return false;
            }

            this.PushResult(method, rightSideType!);
            TypeDefinition? leftSideType = null;
            if (!this.TryEmitExpression(expression.Left, context, scope, method, out leftSideType))
            {
                valueType = null;
                return false;
            }

            if (leftSideType!.IsFloatingPoint)
            {
                if (rightSideType!.IsFloatingPoint)
                {
                    if (rightSideType!.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr [esp]" });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr [esp]" });
                    }

                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType.Size });
                }
                else if (rightSideType!.Size <= 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "fild [esp]" });
                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType.Size });
                }

                switch (expression.Operator)
                {
                    case Keyword.Plus:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "faddp" });
                        }

                        break;
                    case Keyword.Minus:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "fsubp" });
                        }

                        break;
                }
            }
            else if (leftSideType!.Size <= 4)
            {
                if (rightSideType!.Size <= 4 && !rightSideType!.IsFloatingPoint)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "pop ecx" });
                    switch (expression.Operator)
                    {
                        case Keyword.Plus:
                            {
                                if (leftSideType.IsPointer || leftSideType.IsArray)
                                {
                                    int elementSize = leftSideType!.InnerType!.Size;
                                    method.Statements.Add(new AsmStatement { Instruction = "imul ecx," + elementSize });
                                }

                                method.Statements.Add(new AsmStatement { Instruction = "add eax,ecx" });
                            }

                            break;
                        case Keyword.Minus:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "sub eax,ecx" });
                            }

                            break;
                        case Keyword.Or:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "or eax,ecx" });
                            }

                            break;
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
            out TypeDefinition? valueType)
        {
            TypeDefinition? rightSideType = null;
            if (!this.TryEmitExpression(expression.Right, context, scope, method, out rightSideType))
            {
                valueType = null;
                return false;
            }

            this.PushResult(method, rightSideType!);
            TypeDefinition? leftSideType = null;
            if (!this.TryEmitExpression(expression.Left, context, scope, method, out leftSideType))
            {
                valueType = null;
                return false;
            }

            if (leftSideType!.IsFloatingPoint)
            {
                if (rightSideType!.IsFloatingPoint)
                {
                    if (rightSideType!.Size == 8)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld qword ptr [esp]" });
                    }
                    else
                    {
                        method.Statements.Add(new AsmStatement { Instruction = "fld dword ptr [esp]" });
                    }

                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType!.Size });
                }
                else if (rightSideType.Size <= 4)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "fild [esp]" });
                    method.Statements.Add(new AsmStatement { Instruction = "add esp," + rightSideType!.Size });
                }

                switch (expression.Operator)
                {
                    case Keyword.Star:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "fmulp" });
                        }

                        break;
                    case Keyword.Slash:
                        {
                            method.Statements.Add(new AsmStatement { Instruction = "fdivp" });
                        }

                        break;
                }
            }
            else if (leftSideType!.Size <= 4)
            {
                if (rightSideType!.Size <= 4 && !rightSideType!.IsFloatingPoint)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "pop ecx" });
                    switch (expression.Operator)
                    {
                        case Keyword.Star:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "imul eax,ecx" });
                            }

                            break;
                        case Keyword.Mod:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "xor edx,edx" });
                                method.Statements.Add(new AsmStatement { Instruction = "idiv ecx" });
                                method.Statements.Add(new AsmStatement { Instruction = "mov eax,edx" });
                            }

                            break;
                        case Keyword.And:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "and eax,ecx" });
                            }

                            break;
                        case Keyword.Div:
                            {
                                method.Statements.Add(new AsmStatement { Instruction = "xor edx,edx" });
                                method.Statements.Add(new AsmStatement { Instruction = "idiv ecx" });
                            }

                            break;
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
            out string? location,
            out TypeDefinition? storageType,
            out MethodInfo? calleeMethod)
        {
            string? message = null;
            calleeMethod = null;
            NamedReferenceExpression? namedRef = referenceExpression as NamedReferenceExpression;
            if (namedRef != null)
            {
                SymbolEntry? symbol = null;
                if (scope.TryLookup(namedRef.Identifier, out symbol))
                {
                    storageType = symbol!.Type;
                    LocalVariable? localVar = symbol as LocalVariable;
                    if (localVar != null)
                    {
                        location = string.Format("_{0}$[ebp]", localVar.Name);
                        return true;
                    }
                    else
                    {
                        ParameterVariable? parVar = symbol as ParameterVariable;
                        if (parVar != null)
                        {
                            location = string.Format("_{0}$[ebp]", parVar.Name);
                            return true;
                        }
                    }
                }

                foreach (var field in method.Method?.Type?.Fields ?? new List<FieldInfo>())
                {
                    if (string.CompareOrdinal(field.Name, namedRef.Identifier) == 0)
                    {
                        SymbolEntry? symThis = null;
                        if (!scope.TryLookup("this", out symThis))
                        {
                            storageType = null;
                            location = null;
                            return false;
                        }

                        method.Statements.Add(
                            new AsmStatement
                            {
                                Instruction = "mov ecx,_this$[ebp]",
                            });
                        if (field.Offset > 0)
                        {
                            method.Statements.Add(
                                new AsmStatement { Instruction = "add ecx," + field.Offset });
                        }

                        storageType = field.Type;
                        location = "[ecx]";
                        return true;
                    }
                }

                var memberMethod = method.Method!.Type!.FindMethod(namedRef!.Identifier);
                if (memberMethod != null)
                {
                    method.Module!.AddProto(memberMethod!);
                    if (!memberMethod.IsStatic)
                    {
                        SymbolEntry? symThis = null;
                        if (!scope.TryLookup("this", out symThis))
                        {
                            storageType = null;
                            location = null;
                            return false;
                        }

                        method.Statements.Add(
                            new AsmStatement
                            {
                                Instruction = "mov ecx,_this$[ebp]",
                            });

                        method.Statements.Add(new AsmStatement { Instruction = "push ecx" });
                    }

                    if (memberMethod.IsVirtual)
                    {
                        location = "[ecx]";
                    }
                    else
                    {
                        location = memberMethod.MangledName;
                    }

                    storageType = memberMethod.ReturnType;
                    calleeMethod = memberMethod;
                    return true;
                }

                if (context.TryFindTypeByName(namedRef.Identifier, out storageType))
                {
                    location = null;
                    return true;
                }

                message = string.Format(
                    Properties.Resources.CodeGenerator_UndeclaredIdentifier,
                    namedRef.Identifier);
                this.log.Write(new Message(
                    namedRef.Start.Path,
                    namedRef.Start.Line,
                    namedRef.Start.Column,
                    Severity.Error,
                    message));
            }

            MemberReferenceExpression? memberRef = referenceExpression as MemberReferenceExpression;
            if (memberRef != null)
            {
                string? innerLoc = null;
                TypeDefinition? innerType = null;
                MethodInfo? innerCalleeMethod = null;
                if (this.TryEmitReference(memberRef.Inner, context, scope, method, out innerLoc, out innerType, out innerCalleeMethod))
                {
                    if (innerType!.IsEnum && innerLoc == null)
                    {
                        calleeMethod = null;
                        location = null;
                        storageType = innerType;
                        int enumValue = 0;
                        if (!innerType.EnumValues.TryGetValue(memberRef.MemberName, out enumValue))
                        {
                            message = string.Format(
                                System.Globalization.CultureInfo.CurrentCulture,
                                Properties.Resources.CodeGenerator_UndeclaredMember,
                                memberRef.MemberName,
                                innerType.FullName);
                            this.log.Write(
                                new Message(
                                    memberRef.Start.Path,
                                    memberRef.Start.Line,
                                    memberRef.Start.Column,
                                    Severity.Error,
                                    message));
                            return false;
                        }

                        method.Statements.Add(new AsmStatement { Instruction = string.Format("mov eax,{0}", enumValue) });
                        return true;
                    }

                    foreach (var field in innerType.Fields)
                    {
                        if (string.CompareOrdinal(field.Name, memberRef.MemberName) == 0)
                        {
                            method.Statements.Add(
                                new AsmStatement
                                {
                                    Instruction = string.Format("lea ecx,{0}", innerLoc),
                                });
                            if (field.Offset > 0)
                            {
                                method.Statements.Add(
                                    new AsmStatement { Instruction = "add ecx," + field.Offset });
                            }

                            storageType = field.Type;
                            location = "[ecx]";
                            return true;
                        }
                    }

                    var memberMethod = innerType.FindMethod(memberRef.MemberName);
                    if (memberMethod != null)
                    {
                        method.Module?.AddProto(memberMethod);
                        if (!memberMethod.IsStatic)
                        {
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("lea eax,{0}", innerLoc) });
                            method.Statements.Add(new AsmStatement { Instruction = "push eax" });
                        }

                        if (memberMethod.IsVirtual && memberRef.UseVirtualDispatch)
                        {
                            location = "[eax]";
                        }
                        else
                        {
                            location = memberMethod.MangledName;
                        }

                        storageType = memberMethod.ReturnType;
                        calleeMethod = memberMethod;
                        return true;
                    }

                    message = string.Format(
                        Properties.Resources.CodeGenerator_UndeclaredMember,
                        memberRef.MemberName,
                        innerType.FullName);
                    this.log.Write(new Message(
                        memberRef.Start.Path,
                        memberRef.Start.Line,
                        memberRef.Start.Column,
                        Severity.Error,
                        message));
                }
            }

            CallReferenceExpression? callExpr = referenceExpression as CallReferenceExpression;
            if (callExpr != null)
            {
                location = null;
                return this.TryEmitExpression(callExpr, context, scope, method, out storageType);
            }

            ArrayIndexReferenceExpression? arrayIndexExpr = referenceExpression as ArrayIndexReferenceExpression;
            if (arrayIndexExpr != null)
            {
                TypeDefinition? indexType = null;
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

                if (indexType!.IsFloatingPoint || indexType!.IsArray || indexType!.IsClass || indexType!.IsPointer)
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
                string? innerLoc = null;
                TypeDefinition? innerType = null;
                MethodInfo? arrayCalleeMethod = null;
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

                if (!innerType!.IsArray)
                {
                    this.log.Write(new Message(
                        arrayIndexExpr.Inner.Start.Path,
                        arrayIndexExpr.Inner.Start.Line,
                        arrayIndexExpr.Inner.Start.Column,
                        Severity.Error,
                        Properties.Resources.CodeGenerator_ArrayTypeExpected));
                    location = null;
                    storageType = null;
                    return false;
                }

                if (innerType.ArrayElementCount > 0)
                {
                    method.Statements.Add(new AsmStatement { Instruction = "lea ecx," + innerLoc });
                }
                else
                {
                    method.Statements.Add(new AsmStatement { Instruction = "mov ecx," + innerLoc });
                }

                method.Statements.Add(new AsmStatement { Instruction = "pop eax" });
                int elementSize = innerType!.InnerType!.Size;
                if (elementSize > 1)
                {
                    int shiftSize = 0;
                    switch (elementSize)
                    {
                        case 2:
                            shiftSize = 1;
                            break;
                        case 4:
                            shiftSize = 2;
                            break;
                        case 8:
                            shiftSize = 3;
                            break;
                        case 16:
                            shiftSize = 4;
                            break;
                        case 32:
                            shiftSize = 5;
                            break;
                        case 64:
                            shiftSize = 6;
                            break;
                        default:
                            method.Statements.Add(new AsmStatement { Instruction = string.Format("imul eax,{0}", elementSize) });
                            break;
                    }

                    if (shiftSize != 0)
                    {
                        method.Statements.Add(new AsmStatement { Instruction = string.Format("shl eax,{0}", shiftSize) });
                    }
                }

                method.Statements.Add(new AsmStatement { Instruction = "add ecx,eax" });
                location = "[ecx]";
                storageType = innerType.InnerType;
                return true;
            }

            DereferenceExpression? derefExpr = referenceExpression as DereferenceExpression;
            if (derefExpr != null)
            {
                string? innerLoc = null;
                TypeDefinition? innerType = null;
                MethodInfo? innerCall = null;
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

                if (!innerType!.IsPointer)
                {
                    this.log.Write(new Message(
                        derefExpr.Start.Path,
                        derefExpr.Start.Line,
                        derefExpr.Start.Column,
                        Severity.Error,
                        Properties.Resources.CodeGenerator_CannotDerefNonPointer));
                    location = null;
                    storageType = null;
                    return false;
                }

                method.Statements.Add(new AsmStatement { Instruction = "mov ecx," + innerLoc });
                location = "[ecx]";
                storageType = innerType.InnerType;
                return true;
            }

            InheritedReferenceExpression? inhRefExpr = referenceExpression as InheritedReferenceExpression;
            if (inhRefExpr != null)
            {
                storageType = method.Method?.Type?.BaseClass;
                if (storageType == null)
                {
                    message = string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Properties.Resources.CodeGenerator_NoBaseTypeForInherited,
                        method.Method?.Type?.FullName);
                    this.log.Write(new Message(
                        inhRefExpr.Start.Path,
                        inhRefExpr.Start.Line,
                        inhRefExpr.Start.Column,
                        Severity.Error,
                        message));
                    location = null;
                    return false;
                }

                SymbolEntry? thisSymbol = null;
                if (!scope.TryLookup("this", out thisSymbol))
                {
                    // reference a static on the base class.
                    location = null;
                }
                else
                {
                    location = "_this$[ebp]";
                }

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
            out MethodInfo? methodInfo)
        {
            methodInfo = new MethodInfo(type)
            {
                Name = methodDecl.MethodName,
                IsStatic = methodDecl.IsStatic,
                IsVirtual = methodDecl.IsVirtual || methodDecl.IsAbstract || type.IsInterface,
                IsAbstract = methodDecl.IsAbstract || type.IsInterface,
            };

            TypeDefinition? returnType = null;
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
                TypeDefinition? paramDef = null;
                if (!this.TryResolveTypeReference(context, paramDecl.Type, out paramDef))
                {
                    return false;
                }

                foreach (string name in paramDecl.ParameterNames)
                {
                    ParameterInfo paramInfo = new ParameterInfo
                    {
                        Name = name,
                        Type = paramDef,
                    };

                    methodInfo.Parameters.Add(paramInfo);
                }
            }

            if (methodInfo.IsVirtual)
            {
                methodInfo.AssignVTableIndex();
            }

            return true;
        }

        private bool TryResolveTypeReference(
            CompilerContext context,
            TypeReference typeRef,
            out TypeDefinition? typeDef)
        {
            NamedTypeReference? namedType = typeRef as NamedTypeReference;
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

            PointerTypeReference? pointerType = typeRef as PointerTypeReference;
            if (pointerType != null)
            {
                TypeDefinition? innerType = null;
                if (!this.TryResolveTypeReference(context, pointerType.ElementType, out innerType))
                {
                    typeDef = null;
                    return false;
                }

                typeDef = context.GetPointerType(innerType!);
                return true;
            }

            ArrayTypeReference? arrayType = typeRef as ArrayTypeReference;
            if (arrayType != null)
            {
                TypeDefinition? innerType = null;
                if (!this.TryResolveTypeReference(context, arrayType.ElementType, out innerType))
                {
                    typeDef = null;
                    return false;
                }

                int elementCount = 0;
                if (arrayType.ElementCount != null)
                {
                    LiteralExpression? litElementCount = arrayType.ElementCount as LiteralExpression;
                    if (litElementCount == null || !(litElementCount.Value is int))
                    {
                        this.log.Write(new Message(
                            arrayType.ElementCount.Start.Path,
                            arrayType.ElementCount.Start.Line,
                            arrayType.ElementCount.Start.Column,
                            Severity.Error,
                            Properties.Resources.CodeGenerator_ArrayTypeIntElementExpected));
                        typeDef = null;
                        return false;
                    }

                    elementCount = (int)litElementCount.Value;
                }

                typeDef = context.GetArrayType(innerType!, elementCount);
                return true;
            }

            typeDef = null;
            return false;
        }
    }
}
