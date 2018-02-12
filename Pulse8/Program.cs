using Pulse8.Client;
using System;

namespace Pulse8
{
    class Program
    {
        private static ConsoleInteraction Interaction;
        static void Main(string[] args)
        {
            SetState(args);
            RunMain();
        }

        private static void RunMain()
        {
            while (Interaction.Continue)
            {
                Interaction.processInput();
            }
        }

        private static void SetState(string[] args)
        {
            switch (args.Length > 0 ? args[0] : "")
            {
                case "-ef":
                    Interaction = ConsoleInteraction.GetInstance(new Pulse8WebEF());
                    break;
                case "-sql":
                    Interaction = ConsoleInteraction.GetInstance(new Pulse8WebSQL());
                    break;
                default:
                    Interaction = ConsoleInteraction.InvalidInteraction();
                    break;
            }
        }

    }
}
