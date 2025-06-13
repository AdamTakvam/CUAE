using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Runtime.Serialization;

using Metreos.DebugFramework;

namespace DebugConnection
{
	class Client
	{
		[STAThread]
		static void Main(string[] args)
		{
            Console.WriteLine("Metreos Communications Environment Application Debugger");
            Console.WriteLine("Copyright (C) Metreos Corporation 2004. All Rights Reserved.");
            Console.WriteLine();

            if(args.Length == 2)
            {
                int port = 0;
                try
                {
                    port = int.Parse(args[1]);
                }
                catch {}

                if(port > 1024)
                {
                    Client client = new Client();
                    client.Start(args[0], port);
                    return;
                }
            }
            
            Console.WriteLine("Error: You must specify an IPaddress and port to connect to");
            Console.WriteLine();
            Console.WriteLine("  Client <IP address> <port>");
		}

        private TcpClient client;
        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool shutdown = false;

		private string appName = null;
		private string scriptName = null;
        private string currAction = null;
        private string transactionId = null;

        private DebugCommand.CommandType lastCommand;


        public Client() 
        {
            formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        }

        public void Start(string ipAddress, int remotePort)
        {
            IPAddress remoteIP;
            try
            {
                remoteIP = IPAddress.Parse(ipAddress);
            }
            catch
            {
                Console.WriteLine("Invalid IP Address: " + ipAddress);
                return;
            }

            client = new TcpClient();
            IPEndPoint remoteEP = new IPEndPoint(remoteIP, remotePort);
            
            try
            {
                client.Connect(remoteEP);
                stream = client.GetStream();
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot connect to: " + remoteEP);
                Console.WriteLine("Error: " + e.Message);
                return;
            }

            Thread listenThread = new Thread(new ThreadStart(ListenThread));
            listenThread.Start();

            Console.WriteLine("<Connected>");
            Console.WriteLine();

            string selection = "";
            
            while(selection != "0")
            {
                selection = Menu();

                switch(selection)
                {
                    case "0":
                        break;
                    case "1":
                        SetBreakpoint();
                        break;
                    case "2":
						ExecuteAction(true);
						break;
					case "3":
                        ExecuteAction(false);
                        break;
                    case "4":
                        StopDebugging();
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
            Console.WriteLine("1. Set Breakpoint");
            Console.WriteLine("2. Execute Action (step)");
			Console.WriteLine("3. Run");
            Console.WriteLine("4. Stop Debugging");
            Console.WriteLine();
            Console.Write("> ");
            return Console.ReadLine();
        }

        private void SetBreakpoint()
        {
            this.lastCommand = DebugCommand.CommandType.SetBreakpoint;
            this.transactionId = System.Guid.NewGuid().ToString();

			DebugCommand command = new DebugCommand();

			if(appName == null)
			{
				Console.Write("Application name: ");
				appName = Console.ReadLine();
			}

			if(scriptName == null)
			{
				Console.Write("Script name: ");
				scriptName = Console.ReadLine();
			}

			Console.Write("Action ID (enter for none): ");
			command.actionId = Console.ReadLine();

			if(command.actionId == "\n")
			{
				command.actionId = null;
			}
			
			command.appName = appName;
			command.scriptName = scriptName;
            command.type = lastCommand;
            command.transactionId = transactionId;

            lock(stream)
            {
                formatter.Serialize(stream, command);
                stream.Flush();
            }
        }

        private void ExecuteAction(bool step)
        {
            if(this.currAction == null)
            {
                Console.WriteLine("Debugging has not started. Set a breakpoint first");
                return;
            }

			if(step)
			{
				this.lastCommand = DebugCommand.CommandType.ExecuteAction;
			}
			else
			{
				this.lastCommand = DebugCommand.CommandType.Run;
			}

            this.transactionId = System.Guid.NewGuid().ToString();

            DebugCommand command = new DebugCommand();
            command.type = lastCommand;
            command.appName = appName;
            command.scriptName = scriptName;
            command.actionId = currAction;
            command.transactionId = transactionId;

            lock(stream)
            {
                formatter.Serialize(stream, command);
                stream.Flush();
            }
        }

        private void StopDebugging()
        {
            this.lastCommand = DebugCommand.CommandType.StopDebugging;
            this.transactionId = System.Guid.NewGuid().ToString();

            DebugCommand command = new DebugCommand();
            command.type = lastCommand;
            command.appName = "testApp";
            command.transactionId = transactionId;

            lock(stream)
            {
                formatter.Serialize(stream, command);
                stream.Flush();
            }
        }

        private void ListenThread()
        {
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

            if(command.type == DebugCommand.CommandType.HitBreakpoint)
            {
				Console.WriteLine("Function Variables: {0}", command.funcVars != null ? command.funcVars.ToString() : "<null>");
				Console.WriteLine("Script Variables: {0}", command.scriptVars != null ? command.scriptVars.ToString() : "<null>");
				Console.WriteLine("Session Data: {0}", command.sessionData != null ? command.sessionData.ToString() : "<null>");
				Console.WriteLine();
				IDictionaryEnumerator de = command.funcVars.GetEnumerator();
				while(de.MoveNext())
				{
					Console.WriteLine("Function variable: {0} = {1}", de.Key as string, de.Value.ToString());
				}
				this.currAction = command.actionId;
            }

            Console.WriteLine();
            SendResponse(true, command.transactionId);
        }

        private void ProcessResponse(DebugResponse response)
        {
            Console.WriteLine();
            Console.WriteLine("Got a '{0}' response!", response.success ? "success" : "failure");
            Console.WriteLine("For transaction ID: " + response.transactionId);

            if((this.transactionId == null) || (this.lastCommand == DebugCommand.CommandType.Undefined))
            {
                Console.WriteLine("Warning: received unsolicited response");
                return;
            }

            if(response.success == false)
            {
                Console.WriteLine("{0} command failed: {1}", lastCommand, response.failureReason != null ? response.failureReason : "<no reason specified>");
                this.transactionId = null;
                this.lastCommand = DebugCommand.CommandType.Undefined;
                return;
            }

            if(response.transactionId == this.transactionId)
            {
                switch(this.lastCommand)
                {
                    case DebugCommand.CommandType.ExecuteAction:
                        this.currAction = response.nextActionId;
                        Console.WriteLine("Current action: " + currAction);
                        break;
                    case DebugCommand.CommandType.SetBreakpoint:
                        Console.WriteLine("Breakpoint set successfully");
                        break;
                    case DebugCommand.CommandType.StopDebugging:
                        Console.WriteLine("Debugging stopped");
                        break;
                    default:
                        Console.WriteLine("Internal Error: No last command saved!");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Error: Could not correlate transaction ID");
            }
        }

        private void SendResponse(bool success, string transId)
        {
            DebugResponse response = new DebugResponse();
            response.success = success;
            response.transactionId = transId;

            lock(stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }
        }
	}
}
