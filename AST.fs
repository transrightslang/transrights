namespace Rjiendaujughyi

module AST =
    type Identifier = string

    type Literal =
        | String of string
        | Integer of int64
        | Ident of Identifier

    type Expression =
        | Lit of Literal
        | Msg of target: Expression * selector: (Identifier * Expression option) list
        | FuncCall of call: Identifier * arguments: (Expression list)

    type Declaration = Identifier * Expression
    type Assignment = Identifier * Expression

    type Statement =
        | Expr of Expression
        | Assign of Assignment
        | Decl of Declaration
        | Reply of Expression

    type TopLevel =
        | Func of name: Identifier * arguments: (Identifier * Identifier) list * replies: Identifier option * statements: Statement list
        | Import of string

    type ASTNode =
        | ASTTopLevel of TopLevel
        | ASTStatement of Statement
        | ASTDeclaration of Declaration
        | ASTAssignment of Assignment
        | ASTExpression of Expression
        | ASTLiteral of Literal
        | ASTIdentifier of Identifier
