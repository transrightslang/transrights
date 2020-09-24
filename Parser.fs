namespace Rjiendaujughyi

module Parser =
    open FParsec
    open AST

    let position: Parser<_,_> = fun stream -> FParsec.Reply stream.Position

    // Utilities
    let str s = pstring s
    let wrappedBy x y = between x x y
    let skipWhitespace x = between spaces spaces x

    // Terminals
    let lParen = str "("
    let rParen = str ")"
    let colon = str ":"
    let semicolon = str ";"
    // let comma = str ","
    let declaringEquals = str ":="
    let assigningEquals = skipStringCI "="
    // let colonWhitespace = colon |> between spaces
    let endOfStatement = semicolon <|> newlineReturn ";" <|> (eof >>% ";")
    let quote = str "`"

    // Comment
    let comment = str "//" >>. manyChars (noneOf "\n")

    // Literals
    let stringLiteral = manyChars (noneOf "`") |> wrappedBy quote |>> String
    let identifierLiteral: Parser<Identifier,unit> =
        let firstChar c = isLetter c || c = '_'
        let generalChar c = isLetter c || isDigit c || c = '_' || c = '.'

        many1Satisfy2L firstChar generalChar "identifier"
    let integerLiteral =
        pint64 |>> Integer

    let anyLiteral =
            (attempt stringLiteral)
        <|> (attempt (identifierLiteral |>> Ident))
        <|> (integerLiteral)

    // Expressions
    let (anyExpression: Parser<Expression,unit>), anyExpressionImpl = createParserForwardedToRef()
    let messageExpression =
        let uncolonedSelector =
            skipWhitespace identifierLiteral |>> fun ident -> (ident, None)

        let colonedSelector =
            (skipWhitespace (identifierLiteral .>>. opt ((skipWhitespace colon) >>. anyExpression)))

        let selector = attempt colonedSelector <|> attempt uncolonedSelector

        (anyExpression .>> spaces)
        .>>.
        (selector |> many)
        |> between lParen rParen
        |>> Msg

    let functionCallExpression =
        let arguments =
            sepBy anyExpression (attempt (skipWhitespace (str ",")))

        identifierLiteral .>> (skipWhitespace lParen) .>>. (skipWhitespace arguments) .>> spaces .>> rParen
        |>> FuncCall

    do anyExpressionImpl :=
            (attempt messageExpression)
        <|> (attempt functionCallExpression)
        <|> (anyLiteral |>> Lit)

    // Statements
    let assignmentLike sigil kind =
        spaces >>. (identifierLiteral .>>. ((skipWhitespace sigil) >>. anyExpression)) |>> kind

    let assignmentStatement =
        assignmentLike assigningEquals Assignment
    let declarationStatement =
        assignmentLike declaringEquals Declaration
    let replyStatement =
        skipWhitespace (str "reply") >>. anyExpression

    let anyStatement =
        position .>>.
            ((attempt declarationStatement |>> Decl)
        <|> (attempt assignmentStatement |>> Assign)
        <|> (attempt replyStatement |>> Reply)
        <|> (attempt comment |>> CommentStatement)
        <|> (anyExpression |>> Expr))
        .>> endOfStatement

    // Toplevels
    let functionToplevel =
        let argument =
            (attempt (skipWhitespace (identifierLiteral .>>. ((skipWhitespace colon) >>. identifierLiteral))))

        let replies =
            (opt ((str "replies") .>> spaces >>. identifierLiteral))

        (str "func") >>.
             (skipWhitespace identifierLiteral)
        .>>. (skipWhitespace (many argument))
        .>>. (skipWhitespace replies)
        .>>  (skipWhitespace (str "{"))
        .>>. (many (skipWhitespace anyStatement))
        .>>  (skipWhitespace (str "}"))
        |>> fun (((a, b), c), d) -> Func(a, b, c, d)

    let importToplevel =
        (str "import") >>. spaces >>. stringLiteral |>> fun a -> match a with String(content) -> Import(content) | _ -> Import("")

    let anyToplevel =
            (attempt functionToplevel)
        <|> (attempt comment |>> Comment)
        <|> (importToplevel)

    let program =
        let topLevel = anyToplevel .>> endOfStatement |> skipWhitespace

        spaces >>. topLevel |> many1

    let parseProgram programText filename = runParserOnString program () filename programText

    let test p str =
        match run p str with
        | Success (result, _, _) -> printfn "Success: %A" result
        | Failure (errorMessage, _, _) -> printfn "Failure: %s" errorMessage
