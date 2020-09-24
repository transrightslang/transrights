namespace Rjiendaujughyi

module Documentor =
    open AST

    let rec documentProgram (node: TopLevel list) =
        let join (delimter: string) (list: string list) =
            System.String.Join(delimter, list)
        let wrap lhs rhs str = lhs + str + rhs
        let unwrapOr opt value =
            match opt with
            | Some(a) -> a
            | None -> value

        node
        |> List.map
            (fun a ->
                match a with
                | Func(name, args, replies, statements) ->
                    let formatArgument arg =
                        let (argName, argValue) = arg
                        sprintf "<RjiendaujughyiArgument name=\"%s\" type=\"%s\" />" argName argValue

                    sprintf "<RjiendaujughyiFunction name=\"%s\" replies=\"%s\">%s%s</RjiendaujughyiFunction>"
                        name
                        (unwrapOr replies "void")
                        (args |> (List.map formatArgument) |> (join "\n"))
                        ((statements |> List.map (
                            fun (_, b) ->
                                match b with
                                    | CommentStatement(text) ->
                                        sprintf "<RjiendaujughyiComment>%s</RjiendaujughyiComment>"
                                            text
                                    | _ -> ""
                        )) |> (join "\n"))

                | _ -> ""
            )
        |> join "\n"
        |> wrap "<RjiendaujughyiLibrary>\n" "\n</RjiendaujughyiLibrary>"
