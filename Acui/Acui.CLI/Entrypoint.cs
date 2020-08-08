using System;
using System.IO;
using System.Diagnostics;
using CommandLine;

namespace acui.CLI
{
    [Verb("compile", HelpText = "Compile an Acui program")]
    class Compile
    {
        [Option('i', "input", Required = true, HelpText = "The command to compile.")]
        public string input { get; set; }
        [Option('o', "output", Required = true, HelpText = "The binary to output to.")]
        public string output { get; set; }
    }
    class Entry
    {
        public static void HandleCompile(Compile opts)
        {
            var data = File.ReadAllText(opts.input);
            var result = Parser.AcuiParser.Parse(data);
            if (result.Success) {
                using (var process = new Process()) {
                    process.StartInfo.FileName = "gcc";
                    process.StartInfo.ArgumentList.Add("-o");
                    process.StartInfo.ArgumentList.Add(opts.output);
                    process.StartInfo.ArgumentList.Add("-x");
                    process.StartInfo.ArgumentList.Add("c");
                    process.StartInfo.ArgumentList.Add("-");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;

                    process.Start();

                    var writer = process.StandardInput;

                    foreach (var item in result.Value)
                    {
                        writer.WriteLine($"{item.Transpile()}");
                    }

                    writer.Close();

                    process.WaitForExit();
                }
            } else {
                Console.WriteLine($"Failed to parse.\n\n{result.Error}");
            }
        }
        public static void Entrypoint(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Compile>(args)
                .WithParsed<Compile>(HandleCompile);
        }
    }
}