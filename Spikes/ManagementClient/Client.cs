using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Xml.Serialization;

using Metreos.Interfaces;
using Metreos.Core.IPC.Xml;

namespace ManagementClient
{
    class Client
    {
        private const string DefaultAddr    = "127.0.0.1";
        private const int DefaultPort       = 8120;

        #region Main()

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Metreos Communications Environment Management Client");
            Console.WriteLine("Copyright (C) Metreos Corporation 2004-2005. All Rights Reserved.");
            Console.WriteLine();

            Client client = new Client();

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
                    client.Start(args[0], port);
                    return;
                }
            }
            else if(args.Length == 1)
            {
                if(args[0] == "?")
                {
                    Console.WriteLine("Error: You must specify an IP address and port to connect to");
                    Console.WriteLine();
                    Console.WriteLine("  mgmt <IP address> <port>");
                }
                else
                {
                    client.Start(args[0], DefaultPort);
                }
            }
            else
            {
                client.Start(DefaultAddr, DefaultPort);
            }
        }
        #endregion

        private IpcXmlClient client;
        private XmlSerializer commandSerializer;
        private XmlSerializer responseSerializer;

        private responseType response = null;

        public Client()
        {
            commandSerializer = new XmlSerializer(typeof(commandType));
            responseSerializer = new XmlSerializer(typeof(responseType));
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

            IPEndPoint remoteEP = new IPEndPoint(remoteIP, remotePort);
            client = new IpcXmlClient(remoteEP);
            client.onXmlMessageReceived += new OnXmlMessageReceivedDelegate(client_OnMessageReceived);
            try
            {
                client.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot connect to '{0}:{1}': {2}", remoteIP, remotePort, e.Message);
                return;
            }

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
                        ClearCallTable();
                        break;
                    case "2":
                        GarbageCollect();
                        break;
                    case "3":
                        GetProvStatus();
                        break;
                    case "4":
                        EndAllCalls();
                        break;
                    case "5":
                        PrintDiags(IConfig.CoreComponentNames.TEL_MANAGER);
                        break;
                    case "6":
                        PrintDiags(IConfig.CoreComponentNames.ARE);
                        break;
                    case "7":
                        PrintDiags(IConfig.CoreComponentNames.ROUTER);
                        break;
                    case "8":
                        PrintDiags(IConfig.CoreComponentNames.PROV_MANAGER);
                        break;
                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }

                Console.WriteLine();
                Thread.Sleep(500);    // Make it look like this is really hard  :)
            }
        }

        private string Menu()
        {
            Console.WriteLine("Metreos Application Server Debugger");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine();
            Console.WriteLine("0. Quit");
            Console.WriteLine("1. Clear TM call table");
            Console.WriteLine("2. Invoke garbage collection");
            Console.WriteLine("3. Get Provisioning Status");
            Console.WriteLine("4. End all calls");
            Console.WriteLine("5. Print TM Diags");
            Console.WriteLine("6. Print ARE Diags");
            Console.WriteLine("7. Print Router Diags");
            Console.WriteLine("8. Print Provider Manager Diags");            
            Console.WriteLine();
            Console.Write("> ");
            return Console.ReadLine();
        }

        private void ClearCallTable()
        {
            SendCommand(IManagement.Commands.ClearCallTable);
        }

        private void GarbageCollect()
        {
            SendCommand(IManagement.Commands.GarbageCollect);
        }

        #region Get Media Provisioning Status

        private void GetProvStatus()
        {
            Console.Write("AppName: ");
            string appName = Console.ReadLine();
            Console.Write("Verbose report (y/n)? ");
            string vChoice = Console.ReadLine();

            bool verbose = false;
            if(vChoice.ToLower().StartsWith("y"))
                verbose = true;

            //string appName = "ActiveRelay";

            lock(this)
            {
                Console.Write("Progress: ");
            
                if(!verbose)
                    Console.Write("[");

                bool started = false;
                bool done = false;
                bool first = true;
                uint lProgress = 100;

                while(!done)
                {
                    SendCommand(IManagement.Commands.GetProvisioningStatus, IManagement.ParameterNames.APP_NAME, appName);
                    Monitor.Wait(this);

                    if(response != null)
                    {
                        uint progress = GetProgress();

                        if(first)
                        {
                            if(verbose)
                                Console.Write(progress);
                                
                            first = false;
                        }
                        else
                        {
                            if(verbose)
                                Console.Write(", " + progress);
                            else
                            {
                                if(progress != lProgress)
                                {
                                    Console.Write("-");
                                    lProgress = progress;
                                }
                            }
                        }

                        if(progress == 100)
                        {
                            if(started)
                            {
                                done = true;
                                if(!verbose)
                                    Console.WriteLine("]");
                            }
                        }
                        else
                            started = true;
                    }
                }
            }
        }

        private uint GetProgress()
        {
            foreach(string result in response.resultList)
            {
                if(result.StartsWith("Progress:"))
                {
                    string pStr = result.Substring(10);
                    try 
                    {
                        double pValue = double.Parse(pStr); 
                        return (uint)(pValue * 100);
                    }
                    catch {}
                }
            }
            return 0;
        }
        #endregion

        private void PrintDiags(string componentName)
        {
            SendCommand(IManagement.Commands.PrintDiags, IManagement.ParameterNames.NAME, componentName);
        }

        private void EndAllCalls()
        {
            SendCommand(IManagement.Commands.EndAllCalls);
        }

        private void SendCommand(IManagement.Commands cmdName)
        {
            SendCommand(cmdName, null, null);
        }

        private void SendCommand(IManagement.Commands cmdName, string paramName, string paramValue)
        {
            commandType command = new commandType();
            command.name = cmdName.ToString();

            if(paramName != null)
            {
                command.param = new paramType[1];
                command.param[0] = new paramType();
                command.param[0].name = paramName;
                command.param[0].Value = paramValue;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            commandSerializer.Serialize(sw, command);

            client.Write(sb.ToString());

            //Console.WriteLine("Command sent successfully");
        }

        private void client_OnMessageReceived(IpcXmlClient client, string message)
        {
            lock(this)
            {
                try
                {
                    System.IO.StringReader sr = new System.IO.StringReader(message);
                    response = (responseType) responseSerializer.Deserialize(sr);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Could not read data from network. Error: " + e.Message);
                    response = null;
                }

                Monitor.Pulse(this);
            }

            //Console.WriteLine("Got {0} response:\n{1}", response.type.ToString(), message);
        }
    }
}
