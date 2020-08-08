using System;

namespace rjiendaujughyi
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.AcuiParser.Parse(@"
func Add value:int to:int -> int {
    println(`hello world!`)
}
func Main {
    println(`hello world!`)
}
");
            if (result.Success) {
                Console.WriteLine($"Succeeded!");
                foreach (var item in result.Value)
                {
                    Console.WriteLine($"{item}");
                }
                Console.WriteLine("===");
                foreach (var item in result.Value)
                {
                    Console.WriteLine($"{item.Transpile()}");
                }
            } else {
                Console.WriteLine($"Failed!\nResult: {result.Error}");
            }
        }
    }
}
