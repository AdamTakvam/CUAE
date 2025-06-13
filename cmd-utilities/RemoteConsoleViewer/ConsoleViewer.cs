using System;
using System.Threading;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.DebugFramework;

namespace Metreos.RemoteConsoleViewer
{
	public class ConsoleViewer
	{
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput,
            int wAttributes);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

		[STAThread]
		static void Main(string[] args)
		{
            IConsoleApps.PrintHeaderText("Application Server Remote Console Viewer");

            int port = 8140;

            CommandLineArguments clargs = new CommandLineArguments(args);

            if(clargs.IsParamPresent(Parameters.HelpParam))
            {
                PrintHelp();
                return;
            }

            StringCollection saParams = clargs.GetStandAloneParameters();
            if(saParams == null || saParams.Count != 1)
            {
                PrintHelp();
                return;
            }

            string host = saParams[0];

            string portStr = clargs.GetSingleParam(Parameters.Port);
            if(portStr != null && portStr != String.Empty)
                port = Convert.ToInt32(portStr);

            string username = clargs.GetSingleParam(Parameters.Username);
            if(username == null || username == String.Empty)
            {
                PrintHelp();
                return;
            }

            string password = clargs.GetSingleParam(Parameters.Password);
            if(password == null || password == String.Empty)
            {
                PrintHelp();
                return;
            }

            ConsoleViewer cv = new ConsoleViewer();
            cv.Start(args[0], port, username, password);
		}

        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: rconsole.exe [{0}] {1} [{2}] {3} {4}", 
                Parameters.Help.HelpParam, Parameters.Help.Host, Parameters.Help.Port, 
                Parameters.Help.Username, Parameters.Help.Password);
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("  {0,-20} IP address or hostname of server", Parameters.Help.Host);
            Console.WriteLine("  {0,-20} Authorized user name", Parameters.Help.Username);
            Console.WriteLine("  {0,-20} Password for user", Parameters.Help.Password);
            Console.WriteLine();
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("  {0,-20} Server listen port (default = 8140)", Parameters.Help.Port);
            Console.WriteLine("  {0,-20} Print this help screen", Parameters.Help.HelpParam);
        }

		private readonly RemoteConsoleClient client;
        private bool connected = false;

		public ConsoleViewer()
		{
			client = new RemoteConsoleClient();
		}

        private const string TerminateMsg = "Remote Console: Press \"q\" to terminate";

		internal void Start(string ipAddr, int port, string username, string password)
		{
			client.messageWriter += new ConsoleMessageDelegate(WriteLine);
            client.onClose += new VoidDelegate(OnClose);
            client.onAuthSuccess += new VoidDelegate(OnAuthSuccess);
            client.onAuthDenied += new VoidDelegate(OnAuthDenied);
			
            // Encrypt password
            password = Metreos.Utilities.Security.EncryptPassword(password);

            if(client.Start(ipAddr, port, username, password))
            {
                connected = true;

                Console.WriteLine("<Connected>");
                Console.WriteLine();
                Console.WriteLine(TerminateMsg);
			
                while(Console.ReadLine().ToLower() != "q") 
                {
                    Console.WriteLine();
                    Console.WriteLine(TerminateMsg);
                }

                connected = false;
                client.Close();
            }
            else
                Console.WriteLine("<Cannot connect to: " + ipAddr + ":" + port + ">");
		}

        private void OnClose()
        {
            if(connected)
            {
                if(client.BeginReconnect())
                    Console.WriteLine("<Connection lost. Reconnecting...>");
            }
        }

        private void OnAuthSuccess()
        {
            Console.WriteLine("<Authenticated Successfully>");
        }

        private void OnAuthDenied()
        {
            Console.WriteLine("<Authentication Denied>");
            
            // There's no point in going on...
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return;
        }

        private void WriteLine(string message)
        {
            const uint STD_OUTPUT_HANDLE = 0xfffffff5;

            // The 13th character indicates the log level
            string level = message.Substring(13, 1);

            // Determine color of line based on trace level.
            int color = (int)Background.Black;	// Start with a black background.
            switch (level)
            {
                case "E":
                    color |= (int)Foreground.BrightRed;
                    break;
                case "W":
                    color |= (int)Foreground.Yellow;
                    break;
                case "V":
                    color |= (int)Foreground.Grey;
                    break;
                case "I":
                default:
                    color |= (int)Foreground.White;
                    break;
            }

            IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            SetConsoleTextAttribute(hConsole, color);

            // If message contains the highlight indicator, highlight the
            // message by inverting the colors, e.g., red on black ->
            // black on red.
            if (message.IndexOf("!") != -1)
            {
                // Swap nibbles, e.g., 0x0c -> 0xc0.
                SetConsoleTextAttribute(hConsole, color << 4 | color >> 4);
            }

            Console.WriteLine(message);

            // Reset color back to white on black
            SetConsoleTextAttribute(hConsole, (int)Foreground.White | (int)Background.Black);
        }

        // Character attributes for Windows console screen buffers.
        private enum CharacterAttributes
        {
            FOREGROUND_BLUE = 0x0001,
            FOREGROUND_GREEN = 0x0002,
            FOREGROUND_RED = 0x0004,
            FOREGROUND_INTENSITY = 0x0008,
            BACKGROUND_BLUE = 0x0010,
            BACKGROUND_GREEN = 0x0020,
            BACKGROUND_RED = 0x0040,
            BACKGROUND_INTENSITY = 0x0080,
            COMMON_LVB_LEADING_BYTE = 0x0100,
            COMMON_LVB_TRAILING_BYTE = 0x0200,
            COMMON_LVB_GRID_HORIZONTAL = 0x0400,
            COMMON_LVB_GRID_LVERTICAL = 0x0800,
            COMMON_LVB_GRID_RVERTICAL = 0x1000,
            COMMON_LVB_REVERSE_VIDEO = 0x4000,
            COMMON_LVB_UNDERSCORE = 0x8000,
        }

        // Windows character attributes OR together for combined foreground colors.
        private enum Foreground
        {
            Black = 0,
            Blue = CharacterAttributes.FOREGROUND_BLUE,
            Green = CharacterAttributes.FOREGROUND_GREEN,
            Cyan = Blue | Green,
            Red = CharacterAttributes.FOREGROUND_RED,
            Purple = Blue | Red,
            Brown = Green | Red,
            White = Blue | Green | Red,
            Grey = CharacterAttributes.FOREGROUND_INTENSITY,
            BrightBlue = Blue | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightGreen = Green | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightCyan = Cyan | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightRed = Red | CharacterAttributes.FOREGROUND_INTENSITY,
            Pink = Purple | CharacterAttributes.FOREGROUND_INTENSITY,
            Yellow = Brown | CharacterAttributes.FOREGROUND_INTENSITY,
            BrightWhite = White | CharacterAttributes.FOREGROUND_INTENSITY
        }

        // Windows character attributes OR together for combined background colors.
        private enum Background
        {
            Black = Foreground.Black << 4,
            Blue = Foreground.Blue << 4,
            Green = Foreground.Green << 4,
            Cyan = Foreground.Cyan << 4,
            Red = Foreground.Red << 4,
            Purple = Foreground.Purple << 4,
            Brown = Foreground.Brown << 4,
            White = Foreground.White << 4,
            Grey = Foreground.Grey << 4,
            BrightBlue = Foreground.BrightBlue << 4,
            BrightGreen = Foreground.BrightGreen << 4,
            BrightCyan = Foreground.BrightCyan << 4,
            BrightRed = Foreground.BrightRed << 4,
            Pink = Foreground.Pink << 4,
            Yellow = Foreground.Yellow << 4,
            BrightWhite = Foreground.BrightWhite << 4
        }
	}
}
