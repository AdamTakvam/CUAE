using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Serialization;

namespace Metreos.DebugFramework
{
	public delegate void DebugCommandDelegate(DebugCommand command);
	public delegate void DebugResponseDelegate(DebugResponse response);

	/// <summary>
	/// Core library for implementing a debug client application
	/// </summary>
	public class DebugClient
	{
		public DebugCommandDelegate hitBreakpointHandler;
        public DebugCommandDelegate stopDebuggingHandler;
		public DebugResponseDelegate responseHandler;

		private string appName;
		private string scriptName;

        private string lastError;
        public string LastError { get { return lastError; } }

		private Thread listenThread;
		private TcpClient client;
		private NetworkStream stream;
		private IFormatter formatter;

		private string currActionId;
        private DebugCommand.CommandType pendingCommand;
		private bool debugging;
		private volatile bool shutdown;

		#region Construction/Startup/Shutdown
		public DebugClient() 
		{
			formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		}

        private void InitLocalVars()
        {
            pendingCommand = DebugCommand.CommandType.Undefined;
            debugging = false;
            shutdown = false;
        }

		public bool Start(string ipAddress, int remotePort)
		{
            if(listenThread != null)
            {
                Shutdown();
            }

            InitLocalVars();

            if(remotePort < 1024)
            {
                Write("Invalid port specified: " + remotePort.ToString() + Environment.NewLine);
                return false;
            }

			IPAddress remoteIP;
			try
			{
				remoteIP = IPAddress.Parse(ipAddress);
			}
			catch
			{
				Write("Invalid IP address: " + ipAddress + Environment.NewLine);
				return false;
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
				Write("Cannot connect to: " + remoteEP + " (" + e.Message + ")" );
				return false;
			}

			listenThread = new Thread(new ThreadStart(ListenThread));
            listenThread.Name = "Application debugger";
            listenThread.IsBackground = true;
            listenThread.SetApartmentState(ApartmentState.STA);  // Single-threaded apartment
			listenThread.Start();

			return true;
		}

		public void Shutdown()
		{
            if(listenThread == null) { return; }

			shutdown = true;
			if(listenThread.Join(2000) == false)
			{
                listenThread.Abort();
			}

            listenThread = null;

            if(stream != null)
            {
                stream.Close();
                stream = null;
            }
		}
		#endregion

		#region Commands

        public void StartDebugging(string appName, string scriptName, string transactionId)
        {
            Debug.Assert(hitBreakpointHandler != null, "Hit Breakpoint handler is null");
            Debug.Assert(stopDebuggingHandler!= null, "Stop Debugging handler is null");
            Debug.Assert(responseHandler != null, "Response handler is null");
            
            Debug.Assert(appName != null, "Application name is null");
            Debug.Assert(scriptName != null, "Script name is null");

            this.appName = appName;
            this.scriptName = scriptName;

            if(VerifyParams(false, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(null, transactionId, DebugCommand.CommandType.StartDebugging) == true)
            {
                debugging = true;
            }
            else
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
        }

        public void Ping(string transactionId)
        {
            if (SendCommand(null, transactionId, DebugCommand.CommandType.Ping) == false)
                responseHandler(CreateFailureResponse(transactionId));
        }

		public void SetBreakpoint(string actionId, string transactionId)
		{
			if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }
			
            if(SendCommand(actionId, transactionId, DebugCommand.CommandType.SetBreakpoint) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
		}

        public void GetBreakpoints(string transactionId)
        {
            if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }
			
            if(SendCommand(null, transactionId, DebugCommand.CommandType.GetBreakpoints) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
        }

        public void ClearBreakpoint(string actionId, string transactionId)
        {
            if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(actionId, transactionId, DebugCommand.CommandType.ClearBreakpoint) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
        }

		public void Run(string transactionId)
		{
			if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(currActionId, transactionId, DebugCommand.CommandType.Run) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
		}

        public void StepInto(string transactionId)
        {
            if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(currActionId, transactionId, DebugCommand.CommandType.StepInto) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
        }

        public void StepOver(string transactionId)
        {
            if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(currActionId, transactionId, DebugCommand.CommandType.StepOver) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
        }

        public void Break(string transactionId)
        {
            if(VerifyParams(true, ref transactionId) == false) 
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(null, transactionId, DebugCommand.CommandType.Break) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
        }

        public void UpdateValue(string transactionId, string varName, object varValue)
        {
            if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(null, transactionId, DebugCommand.CommandType.UpdateValue, varName, varValue) == false)
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
        }

		public void StopDebugging(string transactionId)
		{
			if(VerifyParams(true, ref transactionId) == false) 
            { 
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
                return; 
            }

            if(SendCommand(currActionId, transactionId, DebugCommand.CommandType.StopDebugging) == true)
            {
                debugging = false;
            }
            else
            {
                DebugResponse resp = CreateFailureResponse(transactionId);
                responseHandler(resp);
            }
		}
		#endregion

		#region Socket thread
		private void ListenThread()
		{
			while(shutdown == false)
			{
				DebugMessage msg = null;

				if(stream.DataAvailable)
				{
					lock(stream)
					{
						try
						{
							msg = formatter.Deserialize(stream) as DebugMessage;
						}
						catch
						{
							Write("Could not read data from network");
						}
						stream.Flush();
					}

					if(msg == null) { continue; }

					if(msg is DebugCommand)
					{
						ProcessCommand(msg as DebugCommand);
					}
					else if(msg is DebugResponse)
					{
						ProcessResponse(msg as DebugResponse);
					}
					msg = null;
				}
				else
				{
					Thread.Sleep(50);
				}
			}
		}
		#endregion

		#region Events/Responses
		private void ProcessCommand(DebugCommand command)
		{
            switch(command.type)
            {
			    case DebugCommand.CommandType.HitBreakpoint:
				    if(hitBreakpointHandler != null)
				    {
					    hitBreakpointHandler(command);
				    }
				    this.currActionId = command.actionId;
                    break;

                case DebugCommand.CommandType.StopDebugging:
                    if(stopDebuggingHandler != null)
                    {
                        this.debugging = false;
                        stopDebuggingHandler(command);
                    }
                    this.currActionId = null;
                    break;
			}

			SendResponse(true, command.transactionId);
		}

		private void ProcessResponse(DebugResponse response)
		{
            pendingCommand = DebugCommand.CommandType.Undefined;

            if((response.success) && (pendingCommand != DebugCommand.CommandType.SetBreakpoint))
            {
                this.currActionId = response.nextActionId;
            }

			if(responseHandler != null)
			{
				responseHandler(response);
			}
		}
		#endregion

		#region Private helper methods
		private void SendResponse(bool success, string transId)
		{
			DebugResponse response = new DebugResponse();
			response.success = success;
			response.transactionId = transId;

			lock(stream)
			{
                try
                {
                    formatter.Serialize(stream, response);
                    stream.Flush();
                }
                catch(Exception e)
                {
                    Write("Failed to send response: " + e.Message);
                }
			}
		}

        private bool SendCommand(string actionId, string transactionId, DebugCommand.CommandType type)
        {
            return SendCommand(actionId, transactionId, type, null, null);
        }

        private bool SendCommand(string actionId, string transactionId, DebugCommand.CommandType type, string varName, object varValue)
        {
			if(stream == null) { return false; }

			DebugCommand command = new DebugCommand();
			command.actionId = actionId;
			command.appName = appName;
			command.scriptName = scriptName;
			command.type = type;
			command.transactionId = transactionId;

            if(type == DebugCommand.CommandType.UpdateValue)
            {
                if(varName == null)
                {
                    Write("No variable name specified in UpdateValue");
                    return false;
                }

                command.funcVars = new Hashtable();
                command.funcVars.Add(varName, varValue);
            }

			lock(stream)
			{
                try
                {
                    formatter.Serialize(stream, command);
                    stream.Flush();
                }
                catch(Exception e)
                {
                    Write("Failed to send command: " + e.Message);
                    responseHandler(CreateFailureResponse(transactionId));
                }
			}

            pendingCommand = type;
			return true;
		}

		private bool VerifyParams(bool assertDebugging, ref string transactionId)
		{
            if(transactionId == null)
            {
                transactionId = System.Guid.NewGuid().ToString();
            }

            if(pendingCommand != DebugCommand.CommandType.Undefined) 
            {
                Write("Error: You cannot issue another command until the previous transaction has completed");
                return false; 
            }

			if(assertDebugging)
			{
				if(debugging == false)
				{
					Write("Error: You must be attached to the server to perform this action");
					return false;
				}
			}

			return true;
		}

        private DebugResponse CreateFailureResponse(string transactionId)
        {
            DebugResponse resp = new DebugResponse();
            resp.transactionId = transactionId;
            resp.failReason = this.lastError;
            resp.success = false;

            return resp;
        }

		private void Write()
		{
			Write(String.Empty);
		}

		private void Write(string format, params object[] args)
		{
			string formattedMsg = String.Format(format, args);
			Write(formattedMsg);
		}

		private void Write(string message)
		{
			lastError = message;
		}
		#endregion
	}
}
