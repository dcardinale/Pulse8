using Pulse8.Client;
using System;

namespace Pulse8
{
    class Program
    {
        private static ConsoleInteraction Interaction;
        static void Main(string[] args)
        {
            switch (args.Length>0? args[0]:"")
            {
                case "-ef":
                    Interaction = ConsoleInteraction.GetInstance(new Pulse8WebEF());
                    break;
                case "-sql":
                    Interaction = ConsoleInteraction.GetInstance(new Pulse8WebSQL());
                    break;
                default:
                    ConsoleInteraction.InvalidInteraction();
                    return;
            }
            while (Interaction.Continue)
            {
                Interaction.processInput();
            }
        }

        
    }
}
