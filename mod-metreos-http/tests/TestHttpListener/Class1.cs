using System;
using System.Diagnostics;
using Metreos.Core.IPC;
using Metreos.Core.IPC.Flatmaps;

namespace TestHttpListener
{
	class HttpListener
	{
		private IpcFlatmapServer flatmapServer;

		public void Start()
		{
			flatmapServer                    = new IpcFlatmapServer("Log Server", 9434, TraceLevel.Info);
			flatmapServer.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(this.OnCloseConnection);
			flatmapServer.OnMessageReceived += new Metreos.Core.IPC.Flatmaps.IpcFlatmapServer.OnMessageReceivedDelegate(this.OnMessageReceieved);
			flatmapServer.OnCloseConnection += new Metreos.Core.Sockets.CloseConnectionDelegate(this.OnCloseConnection);
			flatmapServer.Start();
		}

		public void Stop()
		{
			flatmapServer.Stop();
		}
		
		private void OnNewConnection(int socketId, string remoteHost)
		{ 
			Console.WriteLine("New Connection");
		}

		private void OnMessageReceieved(int socketId, string receiveInterface, int messageType, FlatmapList message)
		{
			Console.WriteLine("++++++ New Message ++++++");
			string sUUID = null;
			for (int i=0; i<message.Count; i++)
			{
				if (message.GetAt(i).key == 100)
				{
					sUUID = message.GetAt(i).dataValue.ToString();
				}
				Console.WriteLine(message.GetAt(i).dataValue.ToString());
			}

			if (sUUID != null)
			{
				FlatmapList flatmap = new FlatmapList();
				// UUID
				flatmap.Add(100, sUUID);
				// Content Type
				flatmap.Add(109, "text/plain; charset=ISO-8859-1");
				// Body
				flatmap.Add(106, "Hello from test program!");
				flatmapServer.Write(socketId, 3001, flatmap);
				Console.WriteLine("------ Message Notified ------");
			}
		}

		private void OnCloseConnection(int socketId)
		{
			Console.WriteLine("Connection Closed");
		}
	}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			HttpListener listener = new HttpListener();
			listener.Start();

			while(true)
			{
				Console.WriteLine("Enter 'q' to quit");
				string userInput = Console.ReadLine();
				if(0 == String.Compare(userInput, "q", true))
				{
					break;
				}
			}

			listener.Stop();
		}
	}

}
