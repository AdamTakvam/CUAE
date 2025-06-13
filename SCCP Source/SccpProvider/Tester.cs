using System;

using Metreos.SCCP;

namespace SccpProvider
{
	class Tester
	{
		[STAThread]
		static void Main(string[] args)
		{
			if(args.Length == 2)
			{
				ushort port = 0;
				try
				{
					port = ushort.Parse(args[1]);
				}
				catch
				{
					PrintHelp();
					return;
				}
				
				InitDll.Initialize();
				Tester tester = new Tester();
				tester.Start(args[0], port);
				InitDll.Terminate();
				return;
			}

			PrintHelp();
		}

		static void PrintHelp()
		{
			Console.WriteLine("Usage: SccpProvider <callmanager address> <callmanager port>");
		}

//
// Tester class implementation
//

		private ManagedProxy sccpProxy;
		private int openCallId;
		private int openLine = 1;

		public Tester()
		{
			sccpProxy = new ManagedProxy();
		}

		public void Start(string cmAddress, ushort port)
		{
			Console.WriteLine("Press <Enter> to connect");
			Console.ReadLine();

			Console.WriteLine(" --> Initializing SCCP stack");

			if(sccpProxy.Initialize() == false) 
			{
				Console.WriteLine(" --> SCCP stack initialization failed.");
				return;
			}

			// Register delegates with the stack
			sccpProxy.alertingDelegate = new AlertingDelegate(OnAlerting);
			sccpProxy.connectAckDelegate = new ConnectAckDelegate(OnConnectAck);
			sccpProxy.connectDelegate = new ConnectDelegate(OnConnect);
			sccpProxy.featureRequestDelegate = new FeatureRequestDelegate(OnFeatureRequest);
			sccpProxy.offhookDelegate = new OffhookDelegate(OnOffhook);
			sccpProxy.proceedingDelegate = new ProceedingDelegate(OnProceeding);
			sccpProxy.releaseCompleteDelegate = new ReleaseCompleteDelegate(OnReleaseComplete);
			sccpProxy.releaseDelegate = new ReleaseDelegate(OnRelease);
			sccpProxy.setupAckDelegate = new SetupAckDelegate(OnSetupAck);
			sccpProxy.setupDelegate = new SetupDelegate(OnSetup);

			Console.WriteLine(" --> Opening a session with CallManager '{0}:{1}'", cmAddress, port.ToString());

			sccpProxy.SendOpenSessionRequest(cmAddress, port);

			System.Threading.Thread.Sleep(5000);

			sccpProxy.SendOpenSessionRequest(cmAddress, port);

			Console.ReadLine();

			string selection = "";

			while(selection != "0")
			{
				selection = Menu();

				switch(selection)
				{
					case "":
					case "0":
						break;
					case "1":
						MakeCall();
						break;
					case "2":
						AcceptCall();
						break;
					case "3":
						Hangup();
						break;
					case "4":
						SendDigits();
						break;
					default:
						Console.WriteLine("Invalid selection");
						break;
				}

				Console.WriteLine();
			}
		}

		string Menu()
		{
			Console.WriteLine();
			Console.WriteLine("SCCP Menu");
			Console.WriteLine("--------------");
			Console.WriteLine("0. Quit");
			Console.WriteLine("1. Make Call");
			Console.WriteLine("2. Accept Call");
			Console.WriteLine("3. Hangup");
			Console.WriteLine("4. Send Digits");
			Console.Write("> ");
			return Console.ReadLine();
		}

		#region Commands
		void MakeCall()
		{
			Console.Write("Number to dial: ");
			string digitStr = Console.ReadLine();

			openCallId = 0;

			ConnInfo connInfo = new ConnInfo();
			connInfo.called_name = "fool";
			connInfo.called_number = digitStr;
			connInfo.calling_name = "Big Daddy";
			
			sccpProxy.SendSetup(openCallId, openLine, connInfo, digitStr, null);
		}

		void AcceptCall()
		{
			MediaInfo mediaInfo = new MediaInfo();
			mediaInfo.port = 1234;
			mediaInfo.payload_type = PayloadTypes.GAPI_PAYLOAD_TYPE_G711_ULAW_64K;

			sccpProxy.SendSetupAck(openCallId, openLine, mediaInfo);
			sccpProxy.SendAlerting(openCallId, openLine);
			sccpProxy.SendConnect(openCallId, openLine, mediaInfo);
		}

		void Hangup()
		{	
			sccpProxy.SendRelease(openCallId, openLine);
		}

		void SendDigits()
		{
		}
		#endregion

		#region Events
		void OnSetup(int callId, int line, ConnInfo conninfo, int alert_info, 
			int alerting_ring, int alerting_tone, MediaInfo media, int replaces)
		{
			openCallId = callId;
			Console.WriteLine(" --> Received 'setup' signal (callID: {0}, line: {1})", callId, line.ToString());

			if(media != null)
			{
				Console.WriteLine(" --> Media ({0}:{1}) at port {2}", 
					media.payload_type.ToString(), media.packet_size.ToString(), media.port.ToString());
			}
		}

		void OnOffhook(int callId, int line)
		{
			Console.WriteLine(" --> Received 'offhook' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnSetupAck(int callId, int line, int cause, ConnInfo conninfo, MediaInfo media)
		{
			Console.WriteLine(" --> Received 'setup' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnProceeding(int callId, int line, ConnInfo conninfo)
		{
			Console.WriteLine(" --> Received 'proceeding' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnAlerting(int callId, int line, ConnInfo conninfo, MediaInfo media, int inband)
		{
			Console.WriteLine(" --> Received 'alerting' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnConnect(int callId, int line, ConnInfo conninfo, MediaInfo media)
		{
			Console.WriteLine(" --> Received 'connect' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnConnectAck(int callId, int line, ConnInfo conninfo, MediaInfo media)
		{
			Console.WriteLine(" --> Received 'connectAck' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnRelease(int callId, int line)
		{
			sccpProxy.SendReleaseComplete(callId, line);
			Console.WriteLine(" --> Received 'release' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnReleaseComplete(int callId, int line)
		{
			Console.WriteLine(" --> Received 'release complete' signal (callID: {0}, line: {1})", callId, line.ToString());
		}

		void OnFeatureRequest(int callId, int line, int feature)
		{
			Console.WriteLine(" --> Received 'feature request' signal (callID: {0}, line: {1})", callId, line.ToString());
		}
		#endregion
	}
}
