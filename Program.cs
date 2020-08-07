using System;

namespace rjiendaujughyi
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = AcuiParser.Parse(@"(Logger send:`data` to:`target`)");
            if (result.Success) {
                Console.WriteLine($"Succeeded!\nResult: {result.Value}");
            } else {
                Console.WriteLine($"Failed!\nResult: {result.Error}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}
