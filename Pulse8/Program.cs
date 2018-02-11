using pulse8Data;
using System;

namespace Pulse8
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($"Please enter a valid member ID{Environment.NewLine}Type \"Exit\" to exit application.");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int memberID))
                {
                    var sql = new Pulse8Data.Data();
                    var info = sql.getMemberInfoById(memberID);
                    for (var i = 0; i < info.Count; i++)
                    {
                        if (i == 0)
                            Console.WriteLine(info[i]);
                        else
                            Console.Write(info[i].GetCategoryInfo());
                    }
                    Console.WriteLine(Environment.NewLine + "Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
                if (input.ToLower() == "exit")
                    break;
            }
        }
    }
}
