using System;
using System.Linq;

namespace rjiendaujughyi
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = AcuiParser.Parse(@"
(Logger send:`data` to:`target`)
(Logger send:`data`)
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
