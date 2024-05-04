# TEA Language Specification
A TEA source file _*.tea_ defines a program unit which has definitions of code and data. It may also include declarations of types private to that file. Externally defined types are declared in a _.th_ file, similar to a header file in C. Unlike C, header files are located by the compiler and are not explicitly included by source files.
Both types of files are parsed according to the same grammar.

## Grammar Reference
The language is parsed given a stream of tokenized input from the source text file. The tokenized input omits comments and whitespace characters.

### Program Unit
_program\_unit_ ::= \( _namespace\_declaration_ | _e_ \) \( _uses_ | _e_ \) \( _type\_block_ | _e_ \) \( _var\_block_ | _e_ \) _method\_definition_* _eof_
_namespace\_declaration_ ::= `namespace` _full\_name_ `;`
_uses_ ::= `uses` _full\_name_ \( `,` _full\_name_ \)* `;`

### Type Declaration
_type\_block_ ::= `type` _type\_declaration_*
_type\_declaration_ ::= _identifier_ `=` \( `public` | _e_ \) \( _enum\_declaration_ | _class\_declaration_ | _interface\_declaration_ | _method\_type\_declaration_ \) `;`
_enum\_declaration_ ::= `(` _identifier_ \( `,` _identifier_ \)* `)`
_class\_declaration_ ::= \( `static` | _e_ \) `class` \( _base\_declaration_ | _e_ \) \( _interface\_impl\_declaration_ | _e_ \) \( _public\_block_ | _e_ \) \( _protected\_block_ | _e_ \) \( _private\_block_ | _e_ \) \( _var\_block_ | _e_ \) `end`
_base\_declaration_ ::= `(` _full\_name_ `)`
_interface\_impl\_declaration_ ::= `interface` `(` _full\_name_ \( `,` _full\_name_ \)* `)`
_public\_block_ ::= `public` _public\_or\_protected\_member\_declaration_*
_protected\_block_ ::= `protected` _public\_or\_protected\_member\_declaration_*
_private\_block_ ::= `private` _private\_member\_declaration_*
_public\_or\_protected\_member\_declaration_ ::= _constructor\_declaration_ | _virtual\_destructor\_declaration_ | _virtual\_or\_static\_method\_declaration_
_private\_member\_declaration_ ::= _constructor\_declaration_ | _destructor\_declaration_ | _static\_method\_declaration_
_interface\_declaration_ ::= `interface` \( _base\_interface_ | _e_ \) _method\_declaration_* `end`
_base\_interface_ ::= 
_method\_type\_declaration_ ::= _function\_type\_declaration_ | _procedure\_type\_declaration_
_function\_type\_declaration_ ::= `function` \( _implicit\_arg_ | _e_ \) _parameter\_list\_declaration_ `:` _type\_reference_
_procedure\_type\_declaration_ ::= `procedure` \( _implicit\_arg_ | _e_ \) _parameter\_list\_declaration_
_implicit\_arg_ ::= `of` _type\_reference_
_constructor\_declaration_ ::= `constructor` _parameter\_list\_declaration_ `;`
_virtual\_destructor\_declaration_ ::= \( `virtual` | _e_ \) _destructor\_declaration_ `;`
_virtual\_or\_static\_method\_declaration_ ::= \( \( `virtual` | `abstract` \) _method\_declaration_ \) | _static\_method\_declaration_
_destructor\_declaration_ ::= `destructor` `(` `)` `;`
_static\_method\_declaration_ ::= \( `static` | _e_ \) _method\_declaration_
_method\_declaration_ ::= _function\_declaration_ | _procedure\_declaration_
_function\_declaration_ ::= `function` _identifier_ _parameter\_list\_declaration_ `:` _type\_reference_ `;`
_procedure\_declaration_ ::= `procedure` _identifier_ _parameter\_list\_declaration_ `;`
_parameter\_list\_declaration_ ::= `(` _parameter\_declaration_ \( `;` _parameter\_declaration_ \)* `)`
_parameter\_declaration_ ::= _identifier_ \( `,` _identifier_ \)* `:` _type\_reference_


### Program Body
_var\_block_ ::= `var` _variable\_declaration_*
_method\_definition_ ::= _constructor\_definition_ | _destructor\_definition_ | _function\_definition_ | _procedure\_definition_
_variable\_declaration_ ::= _identifier_ \( `,` _identifier_ \)* `:` _type\_reference_ \( _init\_expr_ | _e_ \) `;`
_init\_expr_ ::= `=` _expression_
_constructor\_definition_ ::= `constructor` _full\_name_ _parameter\_list\_declaration_ `;` \( _base\_constructor_ | _e_ \) _method\_body_
_base\_constructor_ ::= `inherited` _arg\_list_ `;`
_destructor\_definition_ ::= `destructor` _full\_name_ `(` `)` `;` _method\_body_
_function\_definition_ ::= `function` _full\_name_ _parameter\_list\_declaration_ `:` _type\_reference_ `;` _method\_body_
_procedure\_definition_ ::= `procedure` _full\_name_ _parameter\_list\_declaration_ `;` _method\_body_
_method\_body_ ::= \( _var\_block_ | _e_ \) _block\_statement_ `;`

### Statements
_statement_ ::= _if\_statement_ | _while\_statement_ | _block\_statement_ | _delete\_statement_ | _assign\_statement_ | _call\_statement_ | _e_
_block\_statement_ ::= `begin` \( _statement_ `;` \) `end`
_if\_statement_ ::= `if` _expression_ `then` _statement_ \( _else\_statement_ | _e_ \)
_else\_statement_ ::= `else` _statement_
_while\_statement_ ::= `while` _expression_ `do` _statement_
_delete\_statement_ ::= `delete` _expression_
_assign\_statement_ ::= _reference\_expression_ `:=` _expression_
_call\_statement_ ::= _reference\_expression_ 

### Expressions
_reference\_expression_ ::= `inherited` | _identifier_ | _member\_reference\_expression_ | _call\_expresion_ | _array\_index\_expression_ | _deref\_expression_
_member\_reference\_expression_ ::= _reference\_expression_ `.` _identifier_
_call\_expresion_ ::= _reference\_expression_ _arg\_list_
_array\_index\_expression_ ::= _reference\_expression_ `[` _expression_ `]`
_deref\_expression_ ::= `^` _reference\_expression_
_expression_ ::= _simple\_expression_ | _relational\_expresion_
_relational\_expresion_ ::= _simple\_expression_ \( `>` | `>=` | `<` | `<=` | `=` | `<>` \) _simple\_expression_
_simple\_expression_ ::= _term\_expression_ | \( _term\_expression_ \( `+` | `-` | `or` \) _term\_expression_ \)
_term\_expression_ ::= _factor\_expression_ | \( _factor\_expression_ \( `*` | `/` | `div` | `and` | `mod` \) _factor\_expression_ \)
_factor\_expression_ ::= _paren\_expression_ | _not\_expression_ | _minus\_expression_ | _new\_expresion_ | `nil` | `true` | `false` | _literal_ | _address\_expression_ | _reference\_expression_
_paren\_expression_ ::= `(` _expression_ `)`
_not\_expression_ ::= `not` _factor\_expression_
_minus\_expresion_ ::= `-` _factor\_expression_
_new\_expresion_ ::= `new` _type\_reference_ _arg\_list_
_address\_expression_ ::= `@` _reference\_expression_

### Common Elements
_type\_reference_ ::= _pointer\_type\_reference_ | _array\_type\_reference_ | _full\_name_
_pointer\_type\_reference_ ::= `^` _type\_reference_
_array\_type\_reference_ ::= `array` \( _array\_dim_ | _e_ \) `of` _type\_reference_
_array\_dim_ ::= `[` _expression_ `]`
_full\_name_ ::= _identifier_ \( `.` _identifier_ \)*
_arg\_list_ ::= `(` _expression_ \( `,` _expression_ \)* `)`
_literal_ ::= see token grammar
_identifier_ ::= see token grammar

## Token Grammar
TEA files are ASCII files.
Tokens are delimited by other tokens or by whitespace which is ignored. Whitespace characters are ' ', '\t', '\r', or '\n'.

### Comments
Comments either start with `{` and end with `}` or start with `(*` and end with `*)`. Comments can be multiline and inlude any other character.

### String Literals
String literals are sequences of single quoted `'` text or sequences of UTF-8 numeric character codes designated by the `#` followed by a non-negative decimal integer.
Within a quoted string a sequence of `''` indicates a single quote character.

### Number Literals
Number literals are a sequence of decimal digits optionally followed by a decimal dot and another sequence of decimal digits for a floating point number.

### Identifiers
Identifiers are case-sensitive and have the format _a-zA-Z\[_a-zA-Z0-9\]*.

