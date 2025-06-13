using System;
using System.IO;
using System.Net;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Core.IPC.Sftp;

namespace Metreos.SftpConsoleClient
{
	class Client
	{
        public const int DEFAULT_PORT   = 22;

		[STAThread]
		static void Main(string[] args)
		{
            // Print copyright info
            IConsoleApps.PrintHeaderText("Secure FTP Client");

            if(args.Length < 1 || args[0] == String.Empty)
            {
                PrintUsage();
                return;
            }

            string[] bits = args[0].Split('@');
            if(bits.Length != 2)
            {
                PrintUsage();
                return;
            }

            string username = null, password = null;
            string[] authBits = bits[0].Split(':');
            username = authBits[0];
            if(authBits.Length > 1)
                password = authBits[1];

            string host = bits[1];
            int port = DEFAULT_PORT;

            int colonIndex = host.IndexOf(":");
            if(colonIndex != -1)
            {
                int portIndex = colonIndex + 1;
                string portStr = host.Substring(portIndex, host.Length-portIndex);
                port = Convert.ToInt32(portStr);
                host = host.Substring(0, colonIndex);
            }

            // If password was not specified on command line, prompt for it
            if(password == null || password == String.Empty)
                password = GetPassword(username);

			Client c = new Client();
            c.Go(host, port, username, password);
		}

        private static string GetPassword(string username)
        {
            const string TEMP_FILENAME  = "_temp.dat";

            System.Windows.Forms.Form passwdDialog = new PasswordDialog();
            passwdDialog.ShowDialog();
            
            string password;
            using (StreamReader sr = new StreamReader(TEMP_FILENAME)) 
            {
                password = sr.ReadLine();
            }

            if(password == String.Empty)
                password = null;

            File.Delete(TEMP_FILENAME);

            return password;
        }

        private SftpClient client;
        private bool abort = false;

        public Client()
        {
            this.client = new SftpClient();
            this.client.onConnectionClosed = new VoidDelegate(OnConnectionClosed);
        }

        private void OnConnectionClosed()
        {
            Console.WriteLine("Connection closed");
            abort = true;
        }

        public void Go(string host, int port, string username, string password)
        {
            string failReason;
            
            // Encrypt the password
            password = Metreos.Utilities.Security.EncryptPassword(password);

            DateTime startTime = DateTime.Now;
            if(client.Open(host, port, username, password, out failReason) == false)
            {
                Console.WriteLine(failReason);
                return;
            }
            
            Console.WriteLine("[Connected ({0})]", DateTime.Now.Subtract(startTime).ToString());
            
            while(ProcessCommand() && abort == false) {}

            client.Close();
        }

        private bool ProcessCommand()
        {
            Console.WriteLine();
            Console.Write(client.CurrentDir + ">");
            string rawCmd = Console.ReadLine();
            if(rawCmd == String.Empty)
                return true;

            string[] cmdBits = rawCmd.Split(' ');
            switch(cmdBits[0])
            {
                case "cd":
                    ChangeDirectory(cmdBits);
                    break;
                case "ls":
                case "dir":
                    ListFiles();
                    break;
                case "get":
                    Get(cmdBits);
                    break;
                case "put":
                    Put(cmdBits);
                    break;
                case "rename":
                    Rename(cmdBits);
                    break;
                case "delete":
                    Delete(cmdBits);
                    break;
                case "help":
                    PrintCommandHelp(cmdBits.Length > 1 ? cmdBits[1] : "");
                    break;
                case "quit":
                    return false;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }

            return true;
        }

        private void ChangeDirectory(string[] cmdBits)
        {
            if(cmdBits.Length < 2)
            {
                PrintCommandHelp("cd");
                return;
            }

            if(client.ChangeDir(cmdBits[1]) == false)
            {
                Console.WriteLine("Failed to change directory to: " + cmdBits[1]);
            }
        }

        private void ListFiles()
        {
            bool wroteSomething = false;

            Console.WriteLine();

            foreach(string dirname in client.SubDirectories)
            {
                Console.WriteLine("[{0}]", dirname);
                wroteSomething = true;
            }

            foreach(string filename in client.Files)
            {
                Console.WriteLine(filename);
                wroteSomething = true;
            }
            
            if(wroteSomething == false)
            {
                Console.WriteLine("<empty>");
            }
        }

        private void Get(string[] cmdBits)
        {
            if(cmdBits.Length < 2)
            {
                PrintCommandHelp("get");
                return;
            }

            ProgressBar progressBar = new ProgressBar();

            string failReason = null;
            DirectoryInfo localDir = new DirectoryInfo(".");
            if(client.Download(cmdBits[1], localDir, progressBar, out failReason) == false)
            {
                Console.WriteLine(failReason);
            }
        }

        private void Put(string[] cmdBits)
        {
            if(cmdBits.Length < 2)
            {
                PrintCommandHelp("put");
                return;
            }

            ProgressBar progressBar = new ProgressBar();

            string failReason = null;
            FileInfo localFile = new FileInfo(cmdBits[1]);
            if(client.Upload(localFile, progressBar, out failReason) == false)
            {
                Console.WriteLine(failReason);
            }
        }

        private void Rename(string[] cmdBits)
        {
            if(cmdBits.Length < 3)
            {
                PrintCommandHelp("rename");
                return;
            }

            if(client.Rename(cmdBits[1], cmdBits[2]) == false)
            {
                Console.WriteLine("Failed to rename file '{0}' to '{1}'", cmdBits[1], cmdBits[2]);
            }
        }

        private void Delete(string[] cmdBits)
        {
            if(cmdBits.Length < 2)
            {
                PrintCommandHelp("delete");
                return;
            }

            if(client.Delete(cmdBits[1]) == false)
            {
                Console.WriteLine("Failed to delete '{0}'", cmdBits[1]);
            }
        }

        private void PrintCommandHelp(string commandName)
        {
            Console.WriteLine();

            switch(commandName)
            {
                case "cd":
                    Console.WriteLine(" cd <directory>");
                    Console.WriteLine("   directory - Name of directory to change to");
                    break;
                case "ls":
                case "dir":
                    Console.WriteLine(" ls - No parameters");
                    break;
                case "get":
                    Console.WriteLine(" get <filename>");
                    Console.WriteLine("   filename - Name of file to download");
                    break;
                case "put":
                    Console.WriteLine(" put <filename>");
                    Console.WriteLine("   filename - Name of file to upload");
                    break;
                case "rename":
                    Console.WriteLine(" rename <filename1> <filename2>");
                    Console.WriteLine("   filename1 - Current filename");
                    Console.WriteLine("   filename2 - New filename");
                    break;
                case "delete":
                    Console.WriteLine(" delete <filename>");
                    Console.WriteLine("   filename - Name of file to delete");
                    break;
                default:
                    Console.WriteLine(" cd     - Change Directory");
                    Console.WriteLine(" ls     - List files");
                    Console.WriteLine(" get    - Download file from current directory");
                    Console.WriteLine(" put    - Upload file to current directory");
                    Console.WriteLine(" rename - Rename file in current directory");
                    Console.WriteLine(" delete - Delete file in current directory");
                    Console.WriteLine(" help   - Show this message");
                    Console.WriteLine(" quit   - Quit");
                    Console.WriteLine();
                    Console.WriteLine("Type 'help' followed by a command name to get more specific help");
                    break;
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: MSFTP.EXE <username>:<password>@<hostname>[:<port>]");
            Console.WriteLine();
            Console.WriteLine("Required Parameters:");
            Console.WriteLine("   username: Username for SFTP server");
            Console.WriteLine("   password: Password for username");
            Console.WriteLine("   hostname: IP address or hostname of SFTP server");
            Console.WriteLine();
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("   port: Port number of SFTP server (default: {0})", DEFAULT_PORT);
            //            Console.WriteLine("  {0,-20} Search path for unmanaged DLL dependencies", IPackageGen.PARAM_SEARCH_HELP);
            //            Console.WriteLine("  {0,-20} Overwrite without prompting", IPackageGen.PARAM_OVERWRITE_HELP);
            //            Console.WriteLine("  {0,-20} Print this help screen", IPackageGen.PARAM_HELP_HELP);
            //            Console.WriteLine();
            //            Console.WriteLine("Note:");
            //            Console.WriteLine("  The output file will have the same name as the input file except it will");
            //            Console.WriteLine("  have a .xml extension");
        }
	}
}
