using System;
using System.Collections;

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
		private int openLine = 1;
		public System.Diagnostics.TraceLevel TraceLevel
		{
			set
			{
				sccpProxy.SetTraceLevel(value);
			}
		}
		uint ChosenPort = 1234;
		PayloadTypes ChosenPayloadType = PayloadTypes.GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
		private int currDevice = 1;


		Hashtable devices = new Hashtable();
		public class Device
		{
			public enum State 
			{
				Unregistered,	// Not registered with a CM and not trying to (Closed)
				Registering,	// In the process of registering with a CM (Opening)
				Registered,		// Registered with a CM (Opened)
				Unregistering	// In the process of unregistering with a CM (new state)
			}
			public class Line
			{
				public enum State
				{
					Idle,			// Not involved in a call in any way (Onhook)
					IncomingCall,	// Our phone is ringing (Incoming)
					Dialing,		// Sending one digit at a time while offhook (new state; skipped if already have number)
					OutgoingCall,	// Their phone is ringing (Outgoing)
					Active,			// Call established and active
					Holding			// Call established but on hold
				}

				public State state = State.Idle;
			}

			public State state;
			public Hashtable lines = new Hashtable();
		}

		public Tester()
		{
			sccpProxy = new ManagedProxy();
		}

		public void Start(string cmAddress, ushort port)
		{
			devices.Clear();		// Just being neat about it.

			Console.WriteLine(" --> Initializing SCCP stack");

			if (!sccpProxy.Initialize(System.Diagnostics.TraceLevel.Verbose))
			{
				Console.WriteLine(" --> SCCP stack initialization failed.");
				return;
			}

			Console.WriteLine(" --> SCCP stack initialized");

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
			sccpProxy.registeredDelegate = new RegisteredDelegate(OnRegistered);
			sccpProxy.unregisteredDelegate = new UnregisteredDelegate(OnUnregistered);
			sccpProxy.openReceiveRequestDelegate = new OpenReceiveRequestDelegate(OnOpenReceiveRequest);
			sccpProxy.logWriteDelegate = new LogWriteDelegate(LogWrite);
			sccpProxy.startTransmitDelegate = new StartTransmitDelegate(OnStartTransmit);

			string command;
			string parm1;
			string parm2;
			Hashtable traceLevels = new Hashtable();
			traceLevels.Add("v", System.Diagnostics.TraceLevel.Verbose);
			traceLevels.Add("i", System.Diagnostics.TraceLevel.Info);
			traceLevels.Add("w", System.Diagnostics.TraceLevel.Warning);
			traceLevels.Add("e", System.Diagnostics.TraceLevel.Error);
			traceLevels.Add("o", System.Diagnostics.TraceLevel.Off);

			do
			{
				Menu(out command, out parm1, out parm2);

				switch(command)
				{
					case "":
					case "q":
						break;
					case "r":
						Register(currDevice, GetMACAddress(currDevice), cmAddress, port);
						break;
					case "u":
						Unregister(currDevice);
						break;
					case "c":
						MakeCall(parm1, currDevice, openLine);
						break;
					case "l":
						SetLine(currDevice, parm1);
						break;
					case "d":
						SetDevice(parm1);
						break;
					case "a":
						AcceptCall(currDevice, openLine);
						break;
					case "h":
						Hangup(currDevice, openLine);
						break;
					case "g":
						SendDigits(currDevice, openLine);
						break;
					case "f":
						SendFeature(currDevice, openLine, parm1);
						break;
					case "t":
						if (traceLevels.Contains(parm1))
						{
							TraceLevel = (System.Diagnostics.TraceLevel)traceLevels[parm1];
						}
						else
						{
							Console.WriteLine(" --> Invalid trace level ({0})", parm1);
						}
						break;
					default:
						Console.WriteLine("Invalid selection");
						break;
				}

				Console.WriteLine();
			} while(command != "q");

			Console.WriteLine(" --> Shutting down");
			sccpProxy.Shutdown();
		}

		void Menu(out string command, out string parm1, out string parm2)
		{
			string raw;
			string[] lexemes;

			Console.WriteLine();
			Console.WriteLine("SCCP Menu");
			Console.WriteLine("--------------");
			Console.WriteLine("r)egister");
			Console.WriteLine("l)ine set (1-N)");
			Console.WriteLine("d)evice set (1-N)");
			Console.WriteLine("u)nregister");
			Console.WriteLine("c)all (9999999)");
			Console.WriteLine("a)nswer");
			Console.WriteLine("h)angup");
			Console.WriteLine("di(g)its send");
			Console.WriteLine("t)race level (v/i/w/e/o)");
			Console.WriteLine("f)eature (h/r/t/d/s)");
			Console.WriteLine("q)uit");
			Console.Write("> ");
			raw = Console.ReadLine();

			lexemes = raw.Split(' ');
			command = lexemes.Length > 0 ? lexemes[0] : "";
			parm1 = lexemes.Length > 1 ? lexemes[1] : "";
			parm2 = lexemes.Length > 2 ? lexemes[2] : "";
		}

		private void LogWrite(System.Diagnostics.TraceLevel traceLevel, string text)
		{
			Console.Write(" {0} {1}", traceLevel.ToString()[0], text);
		}

		#region Commands
		void Register(int device, string deviceId, string cmAddress, ushort port)
		{
			if (GetDeviceState(device) == Device.State.Unregistered)
			{
				SetDeviceState(device, Device.State.Registering);

				Console.WriteLine(" --> Registering with CallManager at '{0}:{1}' as device {2}",
					cmAddress, port.ToString(), device.ToString());

				sccpProxy.SendOpenSessionRequest(device, deviceId, cmAddress, port);
			}
			else
			{
				Console.WriteLine(" --> Must be unregistered in order to register device {0} at '{1}:{2}'",
					device.ToString(), cmAddress, port.ToString());
			}
		}

		// Release all active calls and unregister with CM
		void Unregister(int device)
		{
			if (GetDeviceState(device) == Device.State.Registered)
			{
				Console.WriteLine(" --> Unregistering device {0} with CallManager",
					device.ToString());

				SetDeviceState(device, Device.State.Unregistering);

				sccpProxy.SendResetSessionRequest(device);
			}
			else
			{
				Console.WriteLine(" --> Device {0} must be registered in order to unregister",
					device.ToString());
			}
		}

		void MakeCall(string digitStr, int device, int line)
		{
			if (GetDeviceState(device) == Device.State.Registered &&
					GetLineState(device, line) == Device.Line.State.Idle)
			{
				if (digitStr == "")
				{
					Console.Write("Number to dial: ");
					digitStr = Console.ReadLine();
				}

				ConnInfo connInfo = new ConnInfo();
				connInfo.called_name = "fool";
				connInfo.called_number = digitStr;
				connInfo.calling_name = "Big Daddy";

				SetLineState(device, line, Device.Line.State.OutgoingCall);

				sccpProxy.SendSetup(device, line, connInfo, digitStr, null);

				Console.WriteLine(" --> Calling {0} on device {1}, line: {2})",
					digitStr, device.ToString(), line.ToString());
			}
			else
			{
				Console.WriteLine(" --> Must be registered and idle in order to make a call");
			}
		}

		void AcceptCall(int device, int line)
		{
			if (GetDeviceState(device) == Device.State.Registered &&
					GetLineState(device, line) == Device.Line.State.IncomingCall)
			{
				MediaInfo mediaInfo = new MediaInfo();
				mediaInfo.port = ChosenPort;
				mediaInfo.payload_type = ChosenPayloadType;

				Console.WriteLine(" --> Answering call on device {0}, line {1}",
					device.ToString(), line.ToString());

				sccpProxy.SendConnect(device, line, mediaInfo);

				SetLineState(device, line, Device.Line.State.Active);
			}
			else
			{
				Console.WriteLine(" --> Must have incoming call on device {0}, line {1} in order to accept it",
					device.ToString(), line.ToString());
			}
		}

		void Hangup(int device, int line)
		{
			/*
				To hangup, we must be registered and offhook.
			*/
			if (GetDeviceState(device) == Device.State.Registered &&
					IsLineOffhook(device, line))
			{
				Console.WriteLine(" --> Hanging up call on device {0}, line {1}",
						device.ToString(), line.ToString());

				sccpProxy.SendRelease(device, line);
			}
			else
			{
				Console.WriteLine(" --> To hangup call on device {0}, line {1}, " +
						"it must be registered and offhook",
						device.ToString(), line.ToString());
			}
		}

		void SendDigits(int device, int line)
		{
		}

		void SendFeature(int device, int line, string featureStr)
		{
			FeatureCodes code;
			bool isGoodCode = true;

			switch (featureStr)
			{
				case "h":
					code = FeatureCodes.Hold;
					break;
				case "r":
					code = FeatureCodes.Resume;
					break;
				case "t":
					code = FeatureCodes.Transfer;
					break;
				case "d":
					code = FeatureCodes.Redial;
					break;
				case "s":
					code = FeatureCodes.SpeedDial;
					break;
				default:
					code = FeatureCodes.SpeedDial;
					isGoodCode = false;
					Console.WriteLine(" --> Bad feature code {0}; must be " +
							"h)old, r)esume, t)ransfer, re(d)ial, or s)peed dial",
							device.ToString(), line.ToString());
					break;
			}

			if (isGoodCode)
			{
				if (GetDeviceState(device) == Device.State.Registered &&
					GetLineState(device, line) == Device.Line.State.Active)
				{
					Console.WriteLine(" --> Sending feature code {0} on device {1}, line {2}",
						code, device.ToString(), line.ToString());

					sccpProxy.SendFeatureRequest(device, line, code);
				}
				else
				{
					Console.WriteLine(" --> Must have active call on device {0}, line {1} in order to send feature code",
						device.ToString(), line.ToString());
				}
			}
		}

		void SetLine(int device, string line)
		{
			if (line != "")
			{
				if (line == "?") 
				{
					int lines;

					lines = sccpProxy.GetNumberOfLines(device);
					Console.WriteLine(" --> {0} lines on device {1}",
						lines.ToString(), device.ToString());
				}
				else
				{
					openLine = Int32.Parse(line);
					Console.WriteLine(" --> Line set to {0}", line);
				}
			}
			else
			{
				// Attempt to toggle between 1 and 2 if no line specified
				switch (openLine)
				{
					case 1:
						SetLine(device, "2");
						break;
					case 2:
						SetLine(device, "1");
						break;
					default:
						Console.WriteLine(" --> Must specify line");
						break;
				}
			}
		}

		void SetDevice(string deviceId)
		{
			if (deviceId != "")
			{
				currDevice = Int32.Parse(deviceId);
				Console.WriteLine(" --> Device set to {0}", currDevice.ToString());
			}
			else
			{
				Console.WriteLine(" --> Must specify device");
			}
		}
		
		Device.Line.State GetLineState(int device, int line)
		{
			if (devices.Contains(device))
			{
				Device deviceEntry = (Device)devices[device];

				if (deviceEntry.lines.Contains(line))
				{
					return ((Device.Line)deviceEntry.lines[line]).state;
				}
				else
				{
					return Device.Line.State.Idle;
				}
			}
			else
			{
				return Device.Line.State.Idle;
			}
		}

		void SetLineState(int device, int line, Device.Line.State state)
		{
			if (devices.Contains(device))
			{
				if (((Device)devices[device]).lines.Contains(line))
				{
					if (state == Device.Line.State.Idle)
					{
						((Device)devices[device]).lines.Remove(line);
					}
					else
					{
						((Device.Line)(((Device)devices[device]).lines[line])).state = state;
					}
				}
				else
				{
					Device.Line lineEntry = new Device.Line();
					lineEntry.state = state;
					((Device)devices[device]).lines.Add(line, lineEntry);
				}
			}
			else
			{
				Console.WriteLine(" --> Trying to set line state for a non-existant device ({0})!!!\n",
						device.ToString());
			}
		}

		Device.State GetDeviceState(int device)
		{
			if (devices.Contains(device))
			{
				return ((Device)devices[device]).state;
			}
			else
			{
				return Device.State.Unregistered;
			}
		}

		// Setting device to unregistered just removes it from the list of devices.
		void SetDeviceState(int device, Device.State state)
		{
			if (devices.Contains(device))
			{
				if (state == Device.State.Unregistered)
				{
					devices.Remove(device);
				}
				else
				{
					((Device)devices[device]).state = state;
				}
			}
			else
			{
				if (state != Device.State.Unregistered)
				{
					Device deviceEntry = new Device();
					deviceEntry.state = state;
					devices.Add(device, deviceEntry);
				}
			}
		}

		bool IsLineOffhook(int device, int line)
		{
			return GetLineState(device, line) == Device.Line.State.Active ||
					GetLineState(device, line) == Device.Line.State.Dialing ||
					GetLineState(device, line) == Device.Line.State.Holding ||
					GetLineState(device, line) == Device.Line.State.OutgoingCall;
		}

		string GetMACAddress(int device)
		{
			string MACAddress = String.Format("{0}{1}", "55500000000", device.ToString());

			return MACAddress;
		}
		#endregion

		#region Events
		void OnRegistered(int device)
		{
			SetDeviceState(device, Device.State.Registered);
			Console.WriteLine(" --> Device {0} is registered with CM", device.ToString());
		}

		void OnUnregistered(int device)
		{
			SetDeviceState(device, Device.State.Unregistered);
			Console.WriteLine(" --> Device {0} is unregistered with CM", device.ToString());
		}

		/*
			First time provider sees call, so needs device number so it can
			later associate it with call id.
		*/
		void OnSetup(int device, int line, ConnInfo conninfo)
		{
			SetLineState(device, line, Device.Line.State.IncomingCall);

			Console.WriteLine(" --> Received 'setup' signal on device {0}, line: {1})",
				device.ToString(), line.ToString());

			MediaInfo mediaInfo = new MediaInfo();
			mediaInfo.port = ChosenPort;
			mediaInfo.payload_type = ChosenPayloadType;

			sccpProxy.SendSetupAck(device, line, mediaInfo);
			sccpProxy.SendAlerting(device, line);
		}

		void OnOffhook(int device, int line)
		{
			Console.WriteLine(" --> Received 'offhook' signal on device {0}, line: {1})",
				device.ToString(), line.ToString());
		}

		void OnSetupAck(int device, int line, int cause, ConnInfo conninfo, MediaInfo media)
		{
			Console.WriteLine(" --> Received 'setupAck' signal on device {0}, line {1})",
				device.ToString(), line.ToString());
		}

		void OnProceeding(int device, int line, ConnInfo conninfo)
		{
			Console.WriteLine(" --> Received 'proceeding' signal on device {0}, line {1})",
				device.ToString(), line.ToString());
		}

		void OnAlerting(int device, int line, ConnInfo conninfo, MediaInfo media, int inband)
		{
			Console.WriteLine(" --> Received 'alerting' signal on device {0}, line {1})",
				device.ToString(), line.ToString());
		}

		void OnConnect(int device, int line, ConnInfo conninfo, MediaInfo media)
		{
			Console.WriteLine(" --> Received 'connect' signal on device {0}, line {1})",
				device.ToString(), line.ToString());
		}

		void OnConnectAck(int device, int line, ConnInfo conninfo, MediaInfo media)
		{
			Console.WriteLine(" --> Received 'connectAck' signal on device {0}, line {1})",
				device.ToString(), line.ToString());
		}

		void OnOpenReceiveRequest(int device, int line, MediaInfo media)
		{
			Console.WriteLine(" --> Received 'openReceiveRequest' signal on device {0}, line {1})",
				device.ToString(), line.ToString());

			sccpProxy.SendOpenReceiveResponse(device, line, media);
		}

		void OnStartTransmit(int device, int line, MediaInfo media)
		{
			SetLineState(device, line, Device.Line.State.Active);
		}

		void OnRelease(int device, int line)
		{
			Console.WriteLine(" --> Received 'release' signal on device {0}, line {1})",
				device.ToString(), line.ToString());

			sccpProxy.SendReleaseComplete(device, line);

			SetLineState(device, line, Device.Line.State.Idle);
		}

		void OnReleaseComplete(int device, int line)
		{
			Console.WriteLine(" --> Received 'release complete' signal on device {0}, line {1})",
				device.ToString(), line.ToString());

			SetLineState(device, line, Device.Line.State.Idle);
		}

		void OnFeatureRequest(int device, int line, int feature)
		{
			Console.WriteLine(" --> Received 'feature request' signal on device {0}, line {1})",
				device.ToString(), line.ToString());
		}
		#endregion
	}
}
