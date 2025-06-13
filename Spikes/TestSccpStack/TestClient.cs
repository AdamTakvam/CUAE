using System;
using System.Net;
using System.Threading;
using System.Collections;
using Metreos.SccpStack;

namespace TestSccpStack
{
	class TestClient
	{
		private SccpClientSession session;

		public void Start()
		{
			SccpStack stack = new SccpStack();
			session = stack.CreateClientSession();
			session.AlertingEvent += new ClientMessageHandler(HandleAlerting);
			session.ConnectAckEvent += new ClientMessageHandler(HandleConnectAck);
			session.ConnectEvent += new ClientMessageHandler(HandleConnect);
			session.DeviceToUserDataRequestEvent += new ClientMessageHandler(HandleDeviceToUserDataRequest);
			session.DeviceToUserDataResponseEvent += new ClientMessageHandler(HandleDeviceToUserDataResponse);
			session.DigitEvent += new ClientMessageHandler(HandleDigit);
			session.FeatureRequestEvent += new ClientMessageHandler(HandleFeatureRequest);
			session.OffhookClientEvent += new ClientMessageHandler(HandleOffhookClient);
			session.OpenReceiveRequestEvent += new ClientMessageHandler(HandleOpenReceiveRequest);
			session.ProceedingEvent += new ClientMessageHandler(HandleProceeding);
			session.RegisteredEvent += new ClientMessageHandler(HandleRegistered);
			session.ReleaseCompleteEvent += new ClientMessageHandler(HandleReleaseComplete);
			session.ReleaseEvent += new ClientMessageHandler(HandleRelease);
			session.SetupAckEvent += new ClientMessageHandler(HandleSetupAck);
			session.SetupEvent += new ClientMessageHandler(HandleSetup);
			session.StartTransmitEvent += new ClientMessageHandler(HandleStartTransmit);
			session.UnregisteredEvent += new ClientMessageHandler(HandleUnregistered);

			// Build-test message constructors
			Alerting alerting = new Alerting();
			alerting = new Alerting(8);
			CloseSessionRequest closeSessionRequest = new CloseSessionRequest();
			Connect connect = new Connect();
			connect = new Connect(2);
			ConnectAck connectAck = new ConnectAck(1);
			connectAck = new ConnectAck();
			Digits digits = new Digits("*69");
			digits = new Digits(10, "12345678");
			OpenReceiveRequest openReceiveRequest = new OpenReceiveRequest(4,
				new MediaInfo(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5020),
				5, 13731, 20, PayloadType.G711Ulaw64k, true, 1, false, 240));
			openReceiveRequest = new OpenReceiveRequest(
				new MediaInfo(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5020),
				5, 13731, 20, PayloadType.G711Ulaw64k, true, 1, false, 240));
			OpenReceiveResponse openReceiveResponse = new OpenReceiveResponse(
				new MediaInfo(new IPEndPoint(IPAddress.Parse("123.39.68.127"), 13048),
				5, 13731, 20, PayloadType.G711Ulaw64k, true, 1, false, 240));
			openReceiveResponse = new OpenReceiveResponse(5,
				new MediaInfo(new IPEndPoint(IPAddress.Parse("123.39.68.127"), 13048),
				5, 13731, 20, PayloadType.G711Ulaw64k, G723BitRate.NotApplicable, true, 1, false, 240),
				SccpClientSession.Cause.Reject);
			OpenSessionRequest openSessionRequest;
			ArrayList addresses = new ArrayList();
			IPAddress address = IPAddress.Parse("10.1.10.151");
			IPEndPoint endpoint = new IPEndPoint(address, 2000);
			addresses.Add(new IPEndPoint(IPAddress.Parse("10.1.10.25"), 2000));
			addresses.Add(new IPEndPoint(IPAddress.Parse("10.1.10.10"), 2000));
			openSessionRequest = new OpenSessionRequest(endpoint);
			openSessionRequest = new OpenSessionRequest("SEP0006D708FD5E", endpoint);
			openSessionRequest = new OpenSessionRequest("SEP0006D708FD5E", address, endpoint, DeviceType.TapiRoutePoint, ProtocolVersion.Hawkbill);
			openSessionRequest = new OpenSessionRequest("SEP0006D708FD5E", address, endpoint, null, endpoint, DeviceType.TapiRoutePoint, ProtocolVersion.Hawkbill);
			openSessionRequest = new OpenSessionRequest(address, addresses);
			openSessionRequest = new OpenSessionRequest("SEP0006D708FD5E", addresses);
			openSessionRequest = new OpenSessionRequest("SEP0006D708FD5E", address, addresses, DeviceType.TapiRoutePoint, ProtocolVersion.Bravo);
			openSessionRequest = new OpenSessionRequest("SEP0006D708FD5E", address, addresses, null, endpoint, DeviceType.TapiRoutePoint, ProtocolVersion.Bravo);
			Proceeding proceeding = new Proceeding();
			Registered registered = new Registered();
			Release release = new Release();
			release = new Release(1);
			ReleaseComplete releaseComplete = new ReleaseComplete(2);
			releaseComplete = new ReleaseComplete();
			Setup setup = new Setup(3, "5551212");
			setup = new Setup("5551212");
			SetupAck setupAck = new SetupAck();
			setupAck = new SetupAck(1);
			StartTransmit startTransmit = new StartTransmit(6, new MediaInfo());
			startTransmit = new StartTransmit(new MediaInfo());
			Unregistered unregistered = new Unregistered();

			new Thread(new ThreadStart(Run)).Start();
		}

		private bool registered;
		private bool shutdownRequested;
		private bool inCall;

		public void Shutdown()
		{
			shutdownRequested = true;
		}

		public void Hold()
		{
			session.Send(new FeatureRequest(FeatureRequest.Feature.Hold));
		}

		public void Resume()
		{
			session.Send(new FeatureRequest(FeatureRequest.Feature.Resume));
		}

		public void Hangup()
		{
			session.Send(new Release());
		}

		public void Digits(string digits)
		{
			if (inCall)
			{
				session.Send(new Digits(digits));
			}
			else
			{
				session.Send(new Setup(digits));
			}
		}

		public void Redial()
		{
			session.Send(new FeatureRequest(FeatureRequest.Feature.Redial));
		}

		public void Speeddial()
		{
			session.Send(new FeatureRequest(FeatureRequest.Feature.Speeddial));
		}

		private void Run()
		{
			Console.WriteLine("Registering");

			registered = false;
			shutdownRequested = false;
			inCall = false;

			session.Send(new OpenSessionRequest(new IPEndPoint(IPAddress.Parse("10.1.10.25"), 2000)));

			Console.WriteLine("Waiting for registration ack");
			lock (this)
			{
				// Wait 10 seconds for registration.
				Monitor.Wait(this, 10 * 1000);
			}

			if (registered)
			{
				// Wait for user to indicate session teardown
				while (!shutdownRequested)
				{
					Thread.Sleep(500);
				}

				Console.WriteLine("Unregistering");
				session.Send(new CloseSessionRequest());

				Console.WriteLine("Waiting for unregistration ack");
				lock (this)
				{
					// Wait 5 seconds for unregistration.
					Monitor.Wait(this, 10 * 1000);
				}

				if (registered)
				{
					Console.WriteLine("Unregistration failed!!!");
				}
			}
			else
			{
				Console.WriteLine("Registration failed!!!");
			}

			Console.WriteLine("Done");
		}

		private void HandleAlerting(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("Alerting received");
		}

		private void HandleConnectAck(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("ConnectAck received");
		}

		private void HandleConnect(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("Connect received");

			inCall = true;
		}

		private void HandleDeviceToUserDataRequest(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("DeviceToUserDataRequest received");
		}

		private void HandleDeviceToUserDataResponse(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("DeviceToUserDataResponse received");
		}

		private void HandleDigit(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("Digit received: {0}", ((Digits)message).digits);
		}

		private void HandleFeatureRequest(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("FeatureRequest received");
		}

		private void HandleOffhookClient(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("OffhookClient received");
		}

		private void HandleOpenReceiveRequest(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("OpenReceiveRequest received");

			session.Send(new OpenReceiveResponse(((OpenReceiveRequest)message).media));
		}

		private void HandleProceeding(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("Proceeding received");
		}

		private void HandleRegistered(SccpClientSession session, ClientMessage message)
		{
			Console.Write("Registered received for");
			foreach (Line line in ((Registered)message).lines)
			{
				Console.Write(" {0}/{1}/{2}/{3}/{4}",
					line.number, line.directoryNumber, line.fullyQualifiedDisplayName,
					line.label, line.displayOptions);
			}
			Console.WriteLine();

			registered = true;
			Monitor.Pulse(this);
		}

		private void HandleReleaseComplete(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("ReleaseComplete received");

			inCall = false;
		}

		private void HandleRelease(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("Release received");

			session.Send(new ReleaseComplete(((Release)message).lineNumber));

			inCall = false;
		}

		private void HandleSetupAck(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("SetupAck received");
		}

		private void HandleSetup(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("Setup received from {0}", ((Setup)message).callingPartyNumber);

			session.Send(new SetupAck());
			session.Send(new Proceeding());
			session.Send(new Alerting());
			session.Send(new Connect());

			inCall = true;
		}

		private void HandleStartTransmit(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("StartTransmit received");
		}

		private void HandleUnregistered(SccpClientSession session, ClientMessage message)
		{
			Console.WriteLine("Unregistered received");

			registered = false;
			inCall = false;	// (Just in case we lost registration without terminating call.)
			Monitor.Pulse(this);
		}
	}
}
