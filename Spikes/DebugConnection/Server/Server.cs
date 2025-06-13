using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;

using Metreos.DebugFramework;

namespace DebugConnection
{
	class Server
	{
		[STAThread]
		static void Main(string[] args)
		{
            Console.WriteLine();

            if(args.Length == 1)
            {
                int listenPort = 0;
                try
                {
                    listenPort = int.Parse(args[0]);
                }
                catch {}

                if(listenPort > 1024)
                {
                    Server server = new Server();
                    server.Start(listenPort);
                    return;
                }
            }
            
            Console.WriteLine("You must specify a port to listen on (greater than 1024)");
            Console.WriteLine("and a port to connect to:");
            Console.WriteLine();
            Console.WriteLine("  Server <listenPort>");
		}

        private TcpListener listener;
        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool shutdown = false;

        int currAction = 1;

        public Server() 
        {
            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        }

        public void Start(int listenPort)
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), listenPort);
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not listen on port: " + listenPort);
                Console.WriteLine("Reason: " + e.Message);
                return;
            }

            Thread listenThread = new Thread(new ThreadStart(ListenThread));
            listenThread.Start();
            
            string selection = "";
            
            while(selection != "0")
            {
                selection = Menu();

                switch(selection)
                {
                    case "0":
                        break;
                    case "1":
                        HitBreakpoint();
                        break;
                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }

                Console.WriteLine();
            }

            shutdown = true;
        }

        private string Menu()
        {
            Console.WriteLine("Metreos Application Server Debugger");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine();
            Console.WriteLine("0. Quit");
            Console.WriteLine("1. Hit breakpoint");
            Console.WriteLine();
            Console.Write("> ");
            return Console.ReadLine();
        }

        private void HitBreakpoint()
        {
            DebugCommand command = new DebugCommand();
            command.type = DebugCommand.CommandType.HitBreakpoint;
            command.appName = "testApp";
            command.scriptName = "testScript";
            command.actionId = this.currAction.ToString();
            command.transactionId = System.Guid.NewGuid().ToString();

            lock(stream)
            {
                formatter.Serialize(stream, command);
                stream.Flush();
            }
        }

        private void ListenThread()
        {
            try
            {
                listener.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not start server");
                Console.WriteLine("Error: " + e.Message);
                return;
            }

            while(listener.Pending() == false)
            {
                Thread.Sleep(200);

                if(shutdown) { return; }
            }

            TcpClient client = listener.AcceptTcpClient();
            stream = client.GetStream();

            while(shutdown == false)
            {
                object dataObj = null;

                if(stream.DataAvailable)
                {
                    lock(stream)
                    {
                        try
                        {
                            dataObj = formatter.Deserialize(stream);
                        }
                        catch
                        {
                            Console.WriteLine("Could not read data from network");
                        }
                        stream.Flush();
                    }

                    if(dataObj == null) { continue; }

                    if(dataObj is DebugCommand)
                    {
                        ProcessCommand(dataObj as DebugCommand);
                    }
                    else if(dataObj is DebugResponse)
                    {
                        ProcessResponse(dataObj as DebugResponse);
                    }
                    dataObj = null;
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        private void ProcessCommand(DebugCommand command)
        {
            Console.WriteLine();
            Console.WriteLine("Got a '{0}' command!", command.type.ToString());
            Console.WriteLine("For application: " + command.appName);
            Console.WriteLine("For script: " + command.scriptName);
            Console.WriteLine("For action: " + command.actionId);
            Console.WriteLine("With transaction ID: " + command.transactionId);

            switch(command.type)
            {
                case DebugCommand.CommandType.ExecuteAction:
                    SendResponse(true, command.transactionId, (currAction++).ToString());
                    break;
                case DebugCommand.CommandType.SetBreakpoint:
                    SendResponse(true, command.transactionId, currAction.ToString());
                    break;
                case DebugCommand.CommandType.StopDebugging:
                    SendResponse(true, command.transactionId, null);
                    break;
                default:
                    Console.WriteLine("What in the world are you talkin' about?");
                    break;
            }
        }

        private void ProcessResponse(DebugResponse response)
        {
            Console.WriteLine();
            Console.WriteLine("Got a '{0}' response!", response.success ? "success" : "failure");
            Console.WriteLine("For transaction ID: " + response.transactionId);
        }

        private void SendResponse(bool success, string transId, string actionId)
        {
            DebugResponse response = new DebugResponse();
            response.success = success;
            response.transactionId = transId;
            response.nextActionId = actionId;

            lock(stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }
        }
	}
}
