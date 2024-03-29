﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tea.Compiler.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tea.Compiler.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The index expression must evaluate to an ordinal type..
        /// </summary>
        internal static string CodeGenerator_ArrayIndexIntExpected {
            get {
                return ResourceManager.GetString("CodeGenerator_ArrayIndexIntExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expression that is being indexed must evaluate to an array type..
        /// </summary>
        internal static string CodeGenerator_ArrayTypeExpected {
            get {
                return ResourceManager.GetString("CodeGenerator_ArrayTypeExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The element range for an array type must be a constant integer expression..
        /// </summary>
        internal static string CodeGenerator_ArrayTypeIntElementExpected {
            get {
                return ResourceManager.GetString("CodeGenerator_ArrayTypeIntElementExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Interface types can only be derived from other interface types. [{0}] is not an interface type..
        /// </summary>
        internal static string CodeGenerator_BaseInterfaceIsNotInterface {
            get {
                return ResourceManager.GetString("CodeGenerator_BaseInterfaceIsNotInterface", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type {0} cannot be used as a base type because it is not a class..
        /// </summary>
        internal static string CodeGenerator_BaseTypeNotClass {
            get {
                return ResourceManager.GetString("CodeGenerator_BaseTypeNotClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The condition provided does not evaluate to a boolean value. Alter the expression to evaluate to a boolean value..
        /// </summary>
        internal static string CodeGenerator_BooleanConditionExpected {
            get {
                return ResourceManager.GetString("CodeGenerator_BooleanConditionExpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expression that is being dereferenced must evaluate to a pointer type..
        /// </summary>
        internal static string CodeGenerator_CannotDerefNonPointer {
            get {
                return ResourceManager.GetString("CodeGenerator_CannotDerefNonPointer", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The character '{0}' cannot be encoded into a single TEA character.
        /// </summary>
        internal static string CodeGenerator_CannotEncodeChar
        {
            get {
                return ResourceManager.GetString("CodeGenerator_CannotEncodeChar", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot locate an appropriate constructor for {0} with the arguments given..
        /// </summary>
        internal static string CodeGenerator_CannotFindConstructor {
            get {
                return ResourceManager.GetString("CodeGenerator_CannotFindConstructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The class {0} does not fully implement interface {1}. It is missing a virtual or abstract declaration of method {2}..
        /// </summary>
        internal static string CodeGenerator_ClassDoesNotDeclareInterfaceMethod {
            get {
                return ResourceManager.GetString("CodeGenerator_ClassDoesNotDeclareInterfaceMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Constructor arguments are only supported on class types..
        /// </summary>
        internal static string CodeGenerator_ConstructorArgumentsAreOnlySupportedOnClassTypes {
            get {
                return ResourceManager.GetString("CodeGenerator_ConstructorArgumentsAreOnlySupportedOnClassTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting Arrays is not implemented..
        /// </summary>
        internal static string CodeGenerator_DeleteOperandArrayNotSupported {
            get {
                return ResourceManager.GetString("CodeGenerator_DeleteOperandArrayNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type of operand used for delete must be a pointer or array operand..
        /// </summary>
        internal static string CodeGenerator_DeleteOperandMustBePointerOrArray {
            get {
                return ResourceManager.GetString("CodeGenerator_DeleteOperandMustBePointerOrArray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The negative operator (-) is not supported on type {0}..
        /// </summary>
        internal static string CodeGenerator_NegativeNotSupported {
            get {
                return ResourceManager.GetString("CodeGenerator_NegativeNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No automatic conversion exists between {0} and {1} types..
        /// </summary>
        internal static string CodeGenerator_NoAutomaticConversion {
            get {
                return ResourceManager.GetString("CodeGenerator_NoAutomaticConversion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot reference inherited members of type {0} because it has no base class..
        /// </summary>
        internal static string CodeGenerator_NoBaseTypeForInherited {
            get {
                return ResourceManager.GetString("CodeGenerator_NoBaseTypeForInherited", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The not operator (not) is not supported on type {0}..
        /// </summary>
        internal static string CodeGenerator_NotNotSupported {
            get {
                return ResourceManager.GetString("CodeGenerator_NotNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The System.Memory class does not define a method Alloc..
        /// </summary>
        internal static string CodeGenerator_SystemMemoryMissingAlloc {
            get {
                return ResourceManager.GetString("CodeGenerator_SystemMemoryMissingAlloc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The System.Memory class does not define a method Free..
        /// </summary>
        internal static string CodeGenerator_SystemMemoryMissingFree {
            get {
                return ResourceManager.GetString("CodeGenerator_SystemMemoryMissingFree", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New and Delete require System to be used for the class System.Memory.
        /// </summary>
        internal static string CodeGenerator_SystemMemoryNotDeclared {
            get {
                return ResourceManager.GetString("CodeGenerator_SystemMemoryNotDeclared", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type {0} was already declared..
        /// </summary>
        internal static string CodeGenerator_TypeAlreadyDeclared {
            get {
                return ResourceManager.GetString("CodeGenerator_TypeAlreadyDeclared", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Undeclared identifier {0}..
        /// </summary>
        internal static string CodeGenerator_UndeclaredIdentifier {
            get {
                return ResourceManager.GetString("CodeGenerator_UndeclaredIdentifier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is not a member of {1}..
        /// </summary>
        internal static string CodeGenerator_UndeclaredMember {
            get {
                return ResourceManager.GetString("CodeGenerator_UndeclaredMember", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Procedure or function {0} must be declared before it can be defined..
        /// </summary>
        internal static string CodeGenerator_UndeclaredMethod {
            get {
                return ResourceManager.GetString("CodeGenerator_UndeclaredMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type {0} was not defined..
        /// </summary>
        internal static string CodeGenerator_UndefinedType {
            get {
                return ResourceManager.GetString("CodeGenerator_UndefinedType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of arguments passed to method {0} should be {1}, not {2}..
        /// </summary>
        internal static string CodeGenerator_UnexpectedArgumentCount {
            get {
                return ResourceManager.GetString("CodeGenerator_UnexpectedArgumentCount", resourceCulture);
            }
        }        
    }
}
