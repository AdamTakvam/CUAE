using System;
using System.Diagnostics;
using System.Data; 
using System.Timers;

using MySql.Data.MySqlClient;

using Metreos.Core;            
using Metreos.ProviderFramework;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.Messaging;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;
namespace Metreos.Providers.REPLACE_WITH_YOUR_NAME
{
	[ProviderDecl("Shell Provider")] 
	[PackageDecl("Metreos.Providers.REPLACE_WITH_YOUR_NAME", "Description of your provider")]
	public class REPLACE_WITH_YOUR_NAMETelnetProvider : ProviderBase 
	{
		// Replace this with your name please before installing when in the training!
		protected const string ProviderNamespace = "Metreos.Providers.REPLACE_WITH_YOUR_NAME"; 
		
		protected SimpleSocketServer server;
		protected string buffer;

		protected ThreadPool pool;
		protected delegate void WriteDelegate(AsyncAction action, string message, int socketId);
		protected WriteDelegate writeSocket;

		public REPLACE_WITH_YOUR_NAMETelnetProvider(IConfigUtility configUtility) 
			: base(typeof(REPLACE_WITH_YOUR_NAMETelnetProvider), "REPLACE_WITH_YOUR_NAME's Telnet Provider", configUtility)
		{
			this.buffer = String.Empty;
			pool = new ThreadPool(5, 10, "SomeTelnetProvider"); 
			writeSocket = new WriteDelegate(WriteOutToSocket);
		}

		#region ProviderBase Implementation
		protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
		{
			configItems = null;  // these two lines are here only so that the provider compiles.
			extensions = null;   // you will most likely remove these when implementing your own

			///     TODO:
			///     1.  Defining which methods handle which actions that originate from apps.
			///     This is done by creating a relationship between an action name and a 
			///     HandleMessageDelegate class... and then adding that to the  messageCallbacks
			///     collection
			///     Example:
			this.messageCallbacks.Add(ProviderNamespace + "." + "Action1", 
				new HandleMessageDelegate(this.MyActionHandler));
				
			this.messageCallbacks.Add(ProviderNamespace + "." + "Write", 
				new HandleMessageDelegate(this.WriteMessageHandler));

			///     2.  Define configuration items
			///     A provider declares what is configurable in the management console in the 
			///     Initialize method.  Do so by defining how many configItems you have as
			///     an array, and then go about defining each configItem!
			///     
			///      
			///		configItems = new ConfigEntry[1];
			///		configItems[0] = new ConfigEntry("DB Name of Item", "Display Name of Item", Default_Value, 
			///			"Description of Item", IConfig.StandardFormat.YOUR_TYPE, is_required);


			///     3.  Define extension items
			///     A provider declares what are extensions (actions that the *administrator*
			///     can preform in the management console) in the Initialize method.  Do so by
			///     defining how many extensions you have as an array, and then go about defining
			///     each item!
			///     
			///     One important note about extensions.  Extensions use the the
			///     messageCallBacks collection defined above to determine which method is initiated based
			///     on the administrator invoking the extension.  The key is to simply ensure your extension
			///     uses the same name as a messageCallback.  Also, there is nothing to prevent you from
			///     using the same messageCallback for an action and an extension.
			///     
			extensions = new Extension[1];
			extensions[0] = new Extension(ProviderNamespace + "." + "Action1", "Description of the extension");
 
			return true;
		}


		protected override void RefreshConfiguration()
		{
			///    TODO:
			///    1.  Retrieve configuration values, and apply to provider.
			///    Use this.GetConfigValue("DB Name of Item") to get an object from the database
		}

		/// <summary>
		///     If this method fires, then an event we fired was handled by no application.
		/// </summary>
		protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
		{      

		}

		/// <summary>
		///     * Runs in "your" thread – not the application manager – 
		///           so that it doesn't slow down the startup of other providers.
		///     * You must call RegisterNamespace() here or applications can not use the actions of this provider.
		///     * Perform possibly time-consuming actions, e.g., initializing stack.
		///     * Note: Your provider should not send any events 
		///           (and will not receive any actions) until this method completes.
		/// </summary>
		protected override void OnStartup()
		{
			RegisterNamespace();

			pool.Start();
			
			server = new SimpleSocketServer("REPLACE_WITH_YOUR_NAME", 8300, false, this.LogLevel);
			server.OnNewConnection += new Metreos.Core.Sockets.NewConnectionDelegate(NewConnection);
			server.OnDataReceived += new Metreos.Core.Sockets.DataReceivedDelegate(DataReceived);
			server.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(CloseConnection);
			server.Start();

			base.OnStartup();
		}

		/// <summary>
		///     Guaranteed to be called on a graceful shutdown of the Application Server
		/// </summary>
		protected override void OnShutdown()
		{
			pool.Stop();
			server.Stop();
		}

		protected void ProcessCommand(string command, int socketId)
		{
			log.Write(TraceLevel.Info, "Received Command: {0}", command);

			if(command.IndexOf("CallMe") > -1)
			{
				int index = command.IndexOf(":");
				// Found a CallMe command!

				if(index > -1)
				{
					string to = command.Substring(index);

					CallMe(to, socketId);
				}
			}
		}

        

		#endregion

		#region Provider Events

		[Event(ProviderNamespace + "." + "CallMe", true, null, "Callback event", "Calling back")]
		[EventParam("to", typeof(System.String), true, "Who is the call to")]
		[EventParam("socketId", typeof(System.Int32), true, "The ID of the Socket")]
		protected void CallMe(string to, int socketId)
		{
			string scriptId = System.Guid.NewGuid().ToString();
			EventMessage msg = CreateEventMessage(
				ProviderNamespace + "." + "CallMe", 
				EventMessage.EventType.Triggering, 
				scriptId);
                            
			msg.AddField("to", to);
			msg.AddField("socketId", socketId);

			palWriter.PostMessage(msg);
		}

		#endregion

		#region Actions
        
		[Action(ProviderNamespace + "." + "Action1", false, "Action1", "Performs my action", false)]
		[ActionParam("Value1", typeof(string), true, false, "Value1 used to do XYZ")]
		[ResultData("ReturnValue", typeof(string), "Modified version of Value1")]
		protected void MyActionHandler(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			string value1 = null;
			action.InnerMessage.GetString("Value1", true, String.Empty, out value1);

			// Do some stuff.  
			string newValue = value1 + "_Changed";			

			// Once done, respond back to the application with success or failure, and any further result data

			Field resultDataField = new Field("ReturnValue", newValue);
			action.SendResponse(true, resultDataField);
		}

		[Action(ProviderNamespace + "." + "Write", false, "Write", "Writes to the telnet session", true /* IS ASYNC?? */)]
		[ActionParam("Message", typeof(string), true, false, "Write a message")]
		[ActionParam("SocketId", typeof(Int32), true, false, "The socket ID of the client")]
		protected void WriteMessageHandler(ActionBase action)
		{
			// Extract the info sent down from application out of the action message
			string message = null;
			int socketId = -1;

			action.InnerMessage.GetString("Message", true, String.Empty, out message);
			action.InnerMessage.GetInt32("SocketId", true, -1, out socketId);

			// Once done, respond back to the application with success or failure, and any further result data
			// For this example, this is the provisional response back to the application.   By sending this response back,
			// you've just made sure your application is going to keep chugging along.
			action.SendResponse(true);

			// It is your duty as an asynchronous action to take *minimal* time in the action
			// callback method (in this case, this method, 'WriteMessageHandler'.  You must do this because the provider
			// has a single thread processing actions incoming from applications.
			// So once you reach code which can take a long amount of time (like anything involving a socket!)
			// then you want to make sure that code is executed in a different thread so that you can relinquish
			// this thread processing your action callback

			// In this example, we'll satisfy this requirement by using a threadpool, throwing our code which 
			// talks to the socket on a new thread for processing.  Note that in your own code, you'll
			// always be passing along the ActionBase action class at a minimum, because it is that class
			// which let's you create the async event message.

			pool.PostRequest(writeSocket, new object[] { action as AsyncAction, message, socketId });
		}

		// If you declare an action asynchronous, then you'll need to 
		// declare it's callbacks!  If you don't do this, pgen.exe will throw an error when generating XML.

		[Event(ProviderNamespace + "." + "Write", true)] // Tell pgen.exe what action are we qualifying
		[EventParam("Duration", typeof(int), true, "The amount of time (ms) for how long the write took")] // declare any event params
		private void _WriteSuccess() {} // Name of method is arbitrary... choose whatever you like

		[Event(ProviderNamespace + "." + "Write", false)] // Tell pgen.exe what action are we qualifying
		[EventParam("ErrorMessage", typeof(string), true, "The message associated with an exception thrown on write attempt")] // declare any event params
		private void _WriteFailure() {} // Name of method is arbitrary... choose whatever you like
		

		private void WriteOutToSocket(AsyncAction action, string message, int socketId)
		{
			bool writeSuccess;
			DateTime start = DateTime.Now;
			TimeSpan duration = TimeSpan.Zero;
			string errorMessage = String.Empty;
			try
			{
				server.Write(socketId, message); // write to the socket
				
				duration = DateTime.Now.Subtract(start); // get how long it took,
				                                         // so we can send up in event later
				writeSuccess = true; // the write succeeded 
			}
			catch(Exception e)
			{
				errorMessage = e.Message; // save why the write failed,
									      // so we can send up in event later
				writeSuccess = false;  // the write failed
			}

			EventMessage asyncEventMessage = action.CreateAsyncCallback(writeSuccess);

			if(writeSuccess)
			{
				// The field name must correspond to the [EventParam] attribute declared below on _WriteSuccess
				asyncEventMessage.AddField("Duration", duration.TotalMilliseconds);
			}
			else
			{
				// The field name must correspond to the [EventParam] attribute declared below on _WriteFailure
				asyncEventMessage.AddField("ErrorMessage", errorMessage);
			}

			// punt off the event to the Provider Abstration Layer, 
			//which will route it to the Application Runtime
			palWriter.PostMessage(asyncEventMessage);
		}


		#endregion

		#region SocketServerImpl 

		protected class SimpleSocketServer : Metreos.Core.Sockets.SocketServerThreaded
		{
			public event Metreos.Core.Sockets.NewConnectionDelegate OnNewConnection;
			public event Metreos.Core.Sockets.DataReceivedDelegate OnDataReceived;
			public event Metreos.Core.Sockets.CloseConnectionDelegate OnCloseConnection;

			public SimpleSocketServer(string taskName, ushort listenPort, bool loopBackOnly, TraceLevel logLevel) : base (taskName, listenPort, loopBackOnly, logLevel)
			{

			}

			protected override void NewConnection(int socketId, string remoteHost)
			{
				if(OnNewConnection != null)
				{
					OnNewConnection(socketId, remoteHost);
				}
			}

			protected override void DataReceived(int socketId, string receiveIpAddress, byte[] data, int dataLength)
			{
				if(OnDataReceived != null)
				{
					OnDataReceived(socketId, receiveIpAddress, data, dataLength);
				}
			}
            
			protected override void ConnectionClosed(int socketId)
			{
				if(OnCloseConnection != null)
				{
					OnCloseConnection(socketId);
				}
			}

			public void Write(int socketId, string message)
			{
				this.SendData(socketId, System.Text.Encoding.ASCII.GetBytes(message), false);
			}

			public void Close(int socketId)
			{
				this.SendData(socketId, System.Text.Encoding.ASCII.GetBytes("Bye"), true);
			}
		}
		#endregion

		#region Provider Listen Logic
		private void NewConnection(int socketId, string remoteHost)
		{
			log.Write(TraceLevel.Info, "NEW CONN: ID {0} HOST {1}", socketId, remoteHost);
            
			server.Write(socketId, "Enter Command:");
		}

		private void DataReceived(int socketId, string receiveIpAddress, byte[] data, int dataLength)
		{
			for(int i=0; i < dataLength; i++)
			{
				char character = (char)data[i];

				if(IsNewLine(data, i))
				{
					if(buffer != String.Empty)
					{
						ProcessCommand(buffer, socketId);
						server.Write(socketId, "Enter Command:");
					}

					// Reset buffer
					buffer = String.Empty;
				}
				else
				{
					// Continue to build buffer
					buffer += character;
				}

			}

			if(buffer != String.Empty)
			{
				log.Write(TraceLevel.Info, "BUFFER {0}", buffer);
			}
			else
			{
				log.Write(TraceLevel.Info, "BUFFER [EMPTY]");
			}
		}

		private void CloseConnection(int socketId)
		{
			log.Write(TraceLevel.Info, "CLOSE: ID {0}", socketId);
		}

		private bool IsNewLine(byte[] data, int index)
		{
			return data[index] == 10 || data[index] == 13;
		}

		#endregion
	}
}