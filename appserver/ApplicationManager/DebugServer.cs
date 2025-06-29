using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Serialization;

using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.DebugFramework;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.Configuration;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.AppServer.ApplicationManager
{
	public delegate bool BreakpointDelegate(string appName, string scriptName, string actionId, string transId, out string failReason);
	public delegate bool DebugStateDelegate(string appName, string scriptName, string transId, out string failReason);
	public delegate bool ExecuteActionDelegate(string appName, string scriptName, string actionId, string transId, out string failReason);
    public delegate bool UpdateValueDelegate(string appName, string scriptName, Hashtable funcVars, Hashtable scriptVars, string transId, out string failReason);

	public class DebugServer : Loggable
	{
		public BreakpointDelegate handleSetBreakpoint;
        public BreakpointDelegate handleClearBreakpoint;
        public DebugStateDelegate handleStartDebugging;
        public DebugStateDelegate handleBreak;
		public DebugStateDelegate handleStopDebugging;
        public DebugStateDelegate handleGetBreakpoints;
		public ExecuteActionDelegate handleStepOver;
        public ExecuteActionDelegate handleStepInto;
        public ExecuteActionDelegate handleRun;
        public UpdateValueDelegate handleUpdateValue;

		private ushort listenPort;
		private Thread listenThread;
		private NetworkStream stream;
		private IFormatter formatter;

		private Hashtable transactionTable;

		private bool connected = false;
		private volatile bool shutdown = false;
        private object streamLock = new object();

		#region Construction/Startup/Shutdown
		public DebugServer(TraceLevel logLevel)
			: base(logLevel, typeof(DebugServer).Name)
		{
			formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			transactionTable = new Hashtable();

			listenThread = null;
			stream = null;
		}

		public void Start(ushort listenPort)
		{
			if(listenPort > 1024)
			{
				this.listenPort = listenPort;
				listenThread = new Thread(new ThreadStart(ListenThread));
                listenThread.IsBackground = true;
                listenThread.Name = "Debug Server Socket";
				listenThread.Start();
			}
			else
			{
				log.Write(TraceLevel.Warning, "Invalid Debug Server listen port. No debugging services will be available");
			}
		}

		public void Stop()
		{
			if(listenThread == null) { return; }

			shutdown = true;

            if((connected) && (stream != null))
            {
                try
                {
                    stream.Flush();
                    stream.Close();
                }
                catch { /* Already closed. */ }
            }
			
			if(listenThread.Join(2000) == false)
			{
				log.Write(TraceLevel.Warning, "Debug Server did not shutdown gracefully");

    			listenThread.Abort();
			}
		}
		#endregion

		#region Socket Thread
		private void ListenThread()
		{
            Assertion.Check(handleStartDebugging != null, "Start Debugging delegate not connected");
			Assertion.Check(handleSetBreakpoint != null, "Set Breakpoint delegate not connected");
			Assertion.Check(handleStopDebugging != null, "Stop Debugging delegate not connected");
			Assertion.Check(handleRun != null, "Run delegate not connected");
            Assertion.Check(handleStepOver != null, "StepOver delegate not connected");
            Assertion.Check(handleStepInto != null, "StepInto delegate not connected");
            Assertion.Check(handleBreak != null, "Break delegate not connected");

			log.Write(TraceLevel.Info, "Debug server started");
			TcpListener listener = null;

			try
			{
				listener = new TcpListener(IPAddress.Any, listenPort);
				listener.Start();
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Info, "Could not listen on port {0}: {1} ", listenPort, e.Message);
				return;
			}

			Socket socket = AcceptClient(listener);
			if(socket == null) { return; }
			log.Write(TraceLevel.Info, "Client connected");

			while(shutdown == false)
			{
				if((socket.Connected == false) || (connected == false))
				{
					log.Write(TraceLevel.Info, "Client disconnected");
					socket = AcceptClient(listener);
					if(socket == null) { return; }
					log.Write(TraceLevel.Info, "New client connected");
				}

                try
                {
                    byte[] buffer = new byte[1];
                    if(socket.Receive(buffer, SocketFlags.Peek) > 0)
                    {
                        GetDebugMessage();
                    }
                    else
                    {
                        Disconnect();
                    }
                }
                catch 
                {
                    Disconnect();
                }
			}

			log.Write(TraceLevel.Info, "Debug server stopped");
		}

		private Socket AcceptClient(TcpListener listener)
		{
			while(listener.Pending() == false)
			{
				Thread.Sleep(200);

				if(shutdown) { return null; }
			}

			Socket socket = listener.AcceptSocket();
			stream = new NetworkStream(socket, true);
			connected = true;

			return socket;
		}

        private void Disconnect()
        {
            connected = false;

            lock(streamLock)
            {
                try
                {
                    stream.Flush();
                    stream.Close();
                    stream = null;
                }
                catch {}
            }
        }

		private void GetDebugMessage()
		{
            if(connected == false) { return; }

            Assertion.Check(stream != null, "Stream is null while reading from socket");

			DebugMessage dMsg = null;

			lock(streamLock)
			{
				try
				{
					dMsg = (DebugMessage)formatter.Deserialize(stream);
				}
				catch(InvalidCastException)
				{
					log.Write(TraceLevel.Warning, "Invalid message sent to debug server");
					return;
				}
				catch(Exception)
				{
					Disconnect();
					return;
				}
				stream.Flush();
			}

			if(dMsg == null) 
			{
				// Close the stream and disconnect?
				return; 
			}

			if(dMsg is DebugCommand)
			{
				ProcessCommand(dMsg as DebugCommand);
			}
			else if(dMsg is DebugResponse)
			{
				ProcessResponse(dMsg as DebugResponse);
			}
			dMsg = null;
		}

		private void ProcessCommand(DebugCommand command)
		{
			log.Write(TraceLevel.Info, "Got a '{0}' command", command.type.ToString());
			log.Write(TraceLevel.Verbose, "For application: " + command.appName);
			log.Write(TraceLevel.Verbose, "For script: " + command.scriptName);
			log.Write(TraceLevel.Verbose, "For action: {0}", command.actionId != null ? command.actionId : "<null>");
			log.Write(TraceLevel.Verbose, "With transaction ID: {0}", command.transactionId != null ? command.transactionId : "<null>");

			string failReason = null;
			switch(command.type)
			{
                case DebugCommand.CommandType.StartDebugging:
                    if(handleStartDebugging(command.appName, command.scriptName, command.transactionId, out failReason) == false)
                    {
                        SendFailureResponse(command.transactionId, failReason);
                    }
                    break;
                case DebugCommand.CommandType.StopDebugging:
                    if(handleStopDebugging(command.appName, command.scriptName, command.transactionId, out failReason) == false)
                    {
                        SendFailureResponse(command.transactionId, failReason);
                    }
                    break;
                case DebugCommand.CommandType.Break:
                    if(handleBreak(command.appName, command.scriptName, command.transactionId, out failReason) == false)
                    {
                        SendFailureResponse(command.transactionId, failReason);
                    }
                    break;
				case DebugCommand.CommandType.Run:
					if(handleRun(command.appName, command.scriptName, command.actionId, command.transactionId, out failReason) == false)
					{
						SendFailureResponse(command.transactionId, failReason);
					}
					break;
				case DebugCommand.CommandType.StepInto:
					if(handleStepInto(command.appName, command.scriptName, command.actionId, command.transactionId, out failReason) == false)
					{
						SendFailureResponse(command.transactionId, failReason);
					}
					break;
                case DebugCommand.CommandType.StepOver:
                    if(handleStepOver(command.appName, command.scriptName, command.actionId, command.transactionId, out failReason) == false)
                    {
                        SendFailureResponse(command.transactionId, failReason);
                    }
                    break;
				case DebugCommand.CommandType.SetBreakpoint:
					if(handleSetBreakpoint(command.appName, command.scriptName, command.actionId, command.transactionId, out failReason) == false)
					{
						SendFailureResponse(command.transactionId, failReason);
					}
					break;
                case DebugCommand.CommandType.GetBreakpoints:
                    if(handleGetBreakpoints(command.appName, command.scriptName, command.transactionId, out failReason) == false)
                    {
                        SendFailureResponse(command.transactionId, failReason);
                    }
                    break;
                case DebugCommand.CommandType.ClearBreakpoint:
                    if(handleClearBreakpoint(command.appName, command.scriptName, command.actionId, command.transactionId, out failReason) == false)
                    {
                        SendFailureResponse(command.transactionId, failReason);
                    }
                    break;
                case DebugCommand.CommandType.UpdateValue:
                    if(handleUpdateValue(command.appName, command.scriptName, command.funcVars, command.scriptVars, command.transactionId, out failReason) == false)
                    {
                        SendFailureResponse(command.transactionId, failReason);
                    }
                    break;
                case DebugCommand.CommandType.Ping:
                    SendSuccessResponse(command.transactionId, "");
                    break;
				default:
					log.Write(TraceLevel.Warning, "Received indeciferable message on debug server interface");
					break;
			}
		}

		private void ProcessResponse(DebugResponse response)
		{
			log.Write(TraceLevel.Verbose, "Got a '{0}' response", response.success ? "success" : "failure");
			log.Write(TraceLevel.Verbose, "For transaction ID: " + response.transactionId);

			CommandMessage cMsg = transactionTable[response.transactionId] as CommandMessage;
			if(cMsg == null)
			{
				log.Write(TraceLevel.Warning, "Debug response received with no associated transaction");
				return;
			}

			ArrayList fields = new ArrayList();
			fields.Add(new Field(ICommands.Fields.DEBUG_ACTION_ID, response.nextActionId));

			string responseStr = response.success == true ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
			cMsg.SendResponse(responseStr, fields, false);
		}

		#endregion

		#region Message senders

		internal void SendFailureResponse(string transId, string failureReason)
		{
			SendResponse(false, null, failureReason, transId, null, null, null, null, null);
		}

		internal void SendSuccessResponse(string transId, string actionId)
		{
			SendResponse(true, null, null, transId, actionId, null, null, null, null);
		}

		internal void SendResponse(bool success, string resultStr, string failureReason, string transId, string actionId, 
			Hashtable funcVars, Hashtable scriptVars, SessionData sData, Stack callStack)
		{
            if(stream == null) { return; }
			if(success == true) { Assertion.Check(failureReason == null, "Failure reason not null in successful response"); }

			DebugResponse response = new DebugResponse();
			response.success = success;
			response.actionResult = resultStr;
			response.failReason = failureReason;
			response.transactionId = transId;
			response.nextActionId = actionId;

			response.funcVars = funcVars;
			response.scriptVars = scriptVars;
			response.sessionData = sData;
			response.callStack = callStack;

            // Don't try to send DB connections over
            if(response.sessionData != null)
                response.sessionData.DbConnections.Clear();  // Don't worry, this is a copy

            lock(streamLock)
            {
                try
                {
                    formatter.Serialize(stream, response);
                }
                catch
                {
                    log.Write(TraceLevel.Warning, "Could not serialize variables or session data. Sending reduced response.");

                    // We have to send something. Start ripping stuff out and try again
                    response.funcVars = null;
                    response.scriptVars = null;
                    response.sessionData = null;

                    try { formatter.Serialize(stream, response); }
                    catch
                    {
                        log.Write(TraceLevel.Error, "Failed to send response to client");
                    }
                }
                finally
                {
                    stream.Flush();
                }
            }
		}

        internal void SendCommand(CommandMessage cMsg)
        {
            if(stream == null) { return; }

            DebugCommand command = new DebugCommand();
            command.type = (DebugCommand.CommandType) Enum.Parse(typeof(DebugCommand.CommandType), cMsg.MessageId, true);
            command.transactionId = System.Guid.NewGuid().ToString();
            command.appName = cMsg[ICommands.Fields.APP_NAME] as string;
            command.scriptName = cMsg[ICommands.Fields.SCRIPT_NAME] as string;
            command.actionId = cMsg[ICommands.Fields.DEBUG_ACTION_ID] as string;
            command.funcVars = cMsg[ICommands.Fields.FUNCTION_VARS] as Hashtable;
            command.scriptVars = cMsg[ICommands.Fields.SCRIPT_VARS] as Hashtable;
            command.sessionData = cMsg[ICommands.Fields.SESSION_DATA] as SessionData;
            command.failReason = cMsg[ICommands.Fields.FAIL_REASON] as string;
            command.callStack = cMsg[ICommands.Fields.ACTION_STACK] as Stack;

            // Don't try to send DB connections over
            if(command.sessionData != null)
                command.sessionData.DbConnections.Clear();  // Don't worry, this is a copy

            transactionTable[command.transactionId] = cMsg;

            lock(streamLock)
            {
                if(stream.CanWrite)
                {
                    try
                    {
                        formatter.Serialize(stream, command);
                        log.Write(TraceLevel.Info, "Sent debugger command: " + command.type);
                    }
                    catch
                    {
                        log.Write(TraceLevel.Warning, "Could not serialize variables or session data. Sending reduced command.");

                        // We have to send something. Start ripping stuff out and try again
                        command.funcVars = null;
                        command.scriptVars = null;
                        command.sessionData = null;

                        try { formatter.Serialize(stream, command); }
                        catch
                        {
                            log.Write(TraceLevel.Error, "Failed to send command to client: " + command.type);
                        }
                    }
                    finally
                    {
                        stream.Flush();
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        }
		#endregion
	}
}
