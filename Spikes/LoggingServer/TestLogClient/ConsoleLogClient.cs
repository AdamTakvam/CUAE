using System;

using LoggingCore;

namespace TestLogClient
{
    class ConsoleLogClient
    {
        private static LogClient logClient;

        [STAThread]
        static void Main(string[] args)
        {
            logClient = new LogClient();
            logClient.StartLogSession("127.0.0.1", 6060);

            Console.WriteLine("Console log client.");
            
            PrintMenu();
            
            string selection = Console.ReadLine();
            
            while(selection.ToLower() != "q")
            {
                switch(selection)
                {
                    case "w":
                        HandleWriteLogMessage();
                        break;

                    case "s":
                        HandleStressTest();
                        break;

                    case "l":
                        HandleLongMessageTest();
                        break;
                }

                PrintMenu();
                selection = Console.ReadLine();
            }

            logClient.StopLogSession();
        }

        static void HandleWriteLogMessage()
        {
            Console.Write("Enter log message: ");
            string message = Console.ReadLine();

            Console.Write("Enter category: " );
            string category = Console.ReadLine();

            logClient.Write(message, category);
        }

        static void HandleStressTest()
        {
        }

        static void HandleLongMessageTest()
        {
        }

        static void PrintMenu()
        {
            Console.WriteLine("(w)rite log message");
            Console.WriteLine("(s)tress test");
            Console.WriteLine("(l)ong message test");
            Console.WriteLine("(q)uit");
            Console.Write("Enter selection: ");
        }
    }
}
