using System;

namespace rjiendaujughyi
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.AcuiParser.Parse(@"
func Main {
    (Logger send:`data` to:`target`)
}
");
            if (result.Success) {
                Console.WriteLine($"Succeeded!");
                foreach (var item in result.Value)
                {
                    Console.WriteLine($"{item}");
                }
            } else {
                Console.WriteLine($"Failed!\nResult: {result.Error}");
            }
        }
    }
}
