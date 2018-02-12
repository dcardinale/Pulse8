using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pulse8.Client;

namespace Pulse8
{
    public class ConsoleInteraction
    {
        readonly IPulse8WebClient Client;
        static ConsoleInteraction interaction;
        public bool Continue { get; set; } = true;
        
        readonly string Options = $"Please enter a valid member ID{Environment.NewLine}" +
                                    $"Type \"Exit\" to exit application.{Environment.NewLine}"+
                                    $"Type \"All\" to see all members";

        ConsoleInteraction(IPulse8WebClient client) => Client = client;
        ConsoleInteraction() => Continue = false;
        public static ConsoleInteraction GetInstance(IPulse8WebClient client)
        {
            if (interaction == null)
                interaction = new ConsoleInteraction(client);

            return interaction;
        }

        public void processInput()
        {
            Console.WriteLine(Options);
            var input = Console.ReadLine();
            if (int.TryParse(input, out int memberID))
            {
                WriteToConsole(Client.GetMemberInfo(memberID));
            }
            else if (input.ToLower() == "all")
                foreach(var group in Client.GetAllMemberInfo().GroupBy(m => m.MemberID)) { WriteToConsole(group.ToList()); }
            else if (input.ToLower() == "exit")
                Continue = false;
        }

        void WriteToConsole(List<Pulse8Models.MemberInfo> info)
        {
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
        public static ConsoleInteraction InvalidInteraction()
        {
            Console.WriteLine("The value entered was not a valid switch. Please close application and start again.");
            Console.ReadKey();
            return new ConsoleInteraction();
        }

    }
}
