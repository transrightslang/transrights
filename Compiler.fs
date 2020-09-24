namespace Rjiendaujughyi

module Compiler =
    open AST

    let join (delimter: string) (list: string list) =
        System.String.Join(delimter, list)

    let rec compile (node: ASTNode) =
        let unwrapOr opt value =
            match opt with
                | Some(x) -> x
                | None -> value

        let argument arg =
            let (name, kind) = arg
            sprintf "%s %s" kind name

        let joinWithTrailing delimter list =
            (join delimter list) + delimter
        
        let joinWithPreceding delimter list =
            delimter + (join delimter list)

        match node with
        | ASTComment(comment) ->
            sprintf "//%s" comment

        | ASTTopLevel(topLevel) ->
            match topLevel with
                | Func(name, arguments, replies, statements) ->
                    sprintf "%s %s (%s) {\n%s}"
                        (unwrapOr replies "void")
                        (name)
                        (arguments |> List.map argument |> join ", ")
                        (statements |> List.map(ASTStatement >> compile) |> joinWithTrailing ";\n" )

                | Import(import) ->
                    sprintf "#include <%s.h>"
                        import

                | Comment(comment) -> comment |> (ASTComment >> compile)

        | ASTStatement(position, statement) ->
            let data =
                match statement with
                    | Expr(expr) -> expr |> (ASTExpression >> compile)
                    | Assign(assignment) -> assignment |> (ASTDeclaration >> compile)
                    | Decl(decl) -> decl |> (ASTDeclaration >> compile)
                    | CommentStatement(comment) -> comment |> (ASTComment >> compile)
                    | Reply(reply) ->
                        sprintf "return %s"
                            (reply |> (ASTExpression >> compile))

            sprintf "#line %d \"%s\"\n%s"
                position.Line
                position.StreamName
                data

        | ASTDeclaration(ident, expr) ->
            sprintf "__auto_type %s __attribute__((__cleanup__(acui_refDown))) = %s"
                ident
                (expr |> (ASTExpression >> compile))
        | ASTAssignment(ident, expr) ->
            sprintf "%s = %s"
                ident
                (expr |> (ASTExpression >> compile))
        | ASTExpression(expression) ->
            match expression with
                | Lit(literal) -> compile (literal |> ASTLiteral)
                | Msg(target, selector) ->
                    let selectorIdent (selector: Identifier * Expression option) = match selector with (a, _) -> a
                    let isSome (item: 'b * 'a option) = match item with (_, opt) -> opt.IsSome
                    let getSome (item: 'b * 'a option) = match item with (_, opt) -> opt.Value
                    let selectorLength = sprintf "%d" (selector |> (List.filter isSome)).Length

                    let selectorArguments =
                        selector
                        |> List.filter isSome
                        |> List.map(getSome >> ASTExpression >> compile)

                    sprintf "acui_sendMessage(%s, \"%s\", %s)"
                        (compile (target |> ASTExpression))
                        (joinWithPreceding ":" (selector |> List.map(selectorIdent >> string)))
                        (join ", " ([selectorLength] @ selectorArguments))

                | FuncCall(funct, args) ->
                    sprintf "%s(%s)"
                        funct
                        (join ", " (args |> List.map(ASTExpression >> compile)))

        | ASTLiteral(literal) ->
            match literal with
                | String(str) ->
                    sprintf "foundation_string_new(\"%s\")" str
                | Integer(num) ->
                    sprintf "%d" num
                | Ident(ident) -> compile (ident |> ASTIdentifier)

        | ASTIdentifier(identifier) -> identifier

    let compileProgram (program: TopLevel list) =
        program |> List.map (ASTTopLevel >> compile) |> join "\n"