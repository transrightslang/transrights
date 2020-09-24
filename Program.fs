namespace Rjiendaujughyi


module Main =
    open Parser
    open Compiler
    open FParsec

    [<EntryPoint>]
    let main argv =
        let args = argv |> Array.toList
        match args.Head with
        | "compile" ->
            let file = System.IO.File.ReadAllText args.Tail.Head
            match (parseProgram file args.Tail.Head) with
                | Success(output, _, _) ->
                    use proc = new System.Diagnostics.Process ()
                    proc.StartInfo.FileName <- "gcc"
                    proc.StartInfo.ArgumentList.Add("-I/usr/include/Acui")
                    proc.StartInfo.ArgumentList.Add("-lAcuiFoundationKit")
                    proc.StartInfo.ArgumentList.Add("-g")
                    proc.StartInfo.ArgumentList.Add("-o")
                    proc.StartInfo.ArgumentList.Add(args.Tail.Tail.Head)
                    proc.StartInfo.ArgumentList.Add("-w")
                    proc.StartInfo.ArgumentList.Add("-x")
                    proc.StartInfo.ArgumentList.Add("c")
                    proc.StartInfo.ArgumentList.Add("-")
                    proc.StartInfo.UseShellExecute <- false
                    proc.StartInfo.RedirectStandardInput <- true
                    proc.Start() |> ignore
                    proc.StandardInput.WriteLine("#include <FoundationKit/Foundation.h>")
                    proc.StandardInput.WriteLine(compileProgram output)
                    proc.StandardInput.Close()
                    proc.WaitForExit()
                    ignore ""
                | Failure(fail, _, _) -> printfn "Failure: %s" fail
        | "transpile" ->
            let file = System.IO.File.ReadAllText args.Tail.Head
            match (parseProgram file args.Tail.Head) with
                | Success(output, _, _) ->
                    printfn "#include <FoundationKit/Foundation.h>"
                    printfn "%s" (compileProgram output)
                    ignore ""
                | Failure(fail, _, _) -> printfn "Failure: %s" fail
        | _ -> ignore ""

        0

