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
        [Option('v', "verbose", Default = false)]
        public bool verbose { get; set; }
        public void Handle() {
            var data = File.ReadAllText(input);
            var result = Parser.AcuiParser.Parse(data);
            if (result.Success) {
                using (var process = new Process()) {
                    process.StartInfo.FileName = "gcc";
                    process.StartInfo.ArgumentList.Add("-I/usr/include/Acui");
                    process.StartInfo.ArgumentList.Add("-lAcuiFoundationKit");
                    process.StartInfo.ArgumentList.Add("-g");
                    process.StartInfo.ArgumentList.Add("-o");
                    process.StartInfo.ArgumentList.Add(output);
                    process.StartInfo.ArgumentList.Add("-w");
                    process.StartInfo.ArgumentList.Add("-x");
                    process.StartInfo.ArgumentList.Add("c");
                    process.StartInfo.ArgumentList.Add("-");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;

                    process.Start();

                    var writer = process.StandardInput;

                    writer.WriteLine($"#include <FoundationKit/Foundation.h>");

                    if (verbose) {
                        foreach (var item in result.Value)
                        {
                            Console.WriteLine($"{item}");
                        }
                    }

                    foreach (var item in result.Value)
                    {
                        writer.WriteLine($"{item.Transpile()}");
                        if (verbose) {
                            Console.WriteLine($"{item.Transpile()}");
                        }
                    }

                    writer.Close();

                    process.WaitForExit();
                }
            } else {
                Console.WriteLine($"Failed to parse.\n\n{result.Error}");
            }
        }
    }
    [Verb("format", HelpText = "Format an Acui Program")]
    class Format
    {
        [Option('i', "input", Required = true, HelpText = "The command to compile.")]
        public string input { get; set; }
        public void Handle() {
            var data = File.ReadAllText(input);
            var result = Parser.AcuiParser.Parse(data);
            if (result.Success) {
                using (var stream = new System.IO.StreamWriter(input)) {
                    foreach (var item in result.Value)
                    {
                        stream.WriteLine($"{item}");
                    }
                }
            } else {
                Console.WriteLine($"Your file has errors, please fix them before running acui format again:\n\n{result.Error}");
            }
        }
    }
    class Entry
    {
        public static void Entrypoint(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Compile,Format>(args)
                .WithParsed<Compile>(c => c.Handle())
                .WithParsed<Format>(f => f.Handle());
        }
    }
}