using System;
using System.Net;
using System.Threading;
using System.Collections;
using Metreos.SccpStack;

namespace TestSccpStack
{
	class TestStack
	{
		public void Start(string ipAddress)
		{
			SccpStack stack = new SccpStack();
			connection = stack.CreateConnection(new IPEndPoint(IPAddress.Parse(ipAddress), 2000));
			connection.ConnectedEvent += new ConnectedHandler(HandleConnected);
			connection.RegisterAckEvent += new SccpMessageHandler(HandleRegisterAck);
			connection.RegisterRejectEvent += new SccpMessageHandler(HandleRegisterReject);
			connection.CapabilitiesReqEvent += new SccpMessageHandler(HandleCapabilitiesReq);
			connection.ButtonTemplateEvent += new SccpMessageHandler(HandleButtonTemplate);
			connection.SoftkeyTemplateResEvent += new SccpMessageHandler(HandleSoftkeyTemplateRes);
			connection.SoftkeySetResEvent += new SccpMessageHandler(HandleSoftkeySetRes);
			connection.SelectSoftkeysEvent += new SccpMessageHandler(HandleSelectSoftkeys);
			connection.DisplayPromptStatusEvent += new SccpMessageHandler(HandleDisplayPromptStatus);
			connection.LineStatEvent += new SccpMessageHandler(HandleLineStat);
			connection.DefineTimeDateEvent += new SccpMessageHandler(HandleDefineTimeDate);
			connection.KeepaliveAckEvent += new SccpMessageHandler(HandleKeepaliveAck);
			connection.UnregisterAckEvent += new SccpMessageHandler(HandleUnregisterAck);

			new Thread(new ThreadStart(Run)).Start();
		}

		private SccpConnection connection;

		private void Run()
		{
			Console.WriteLine("Running");

			connected = false;
			if (connection.Start(5000))
			{
				lock (this)
				{
					Monitor.Wait(this);
				}
				if (connected)
				{
					// Need to distinguish between first and second of these messages.
					numberOfDisplayPromptStatusMessagesReceived = 0;

					// Kick off registration then wait until we are fully registered.
					connection.Send(new Register(new Sid()));
					lock (this)
					{
						Monitor.Wait(this);
					}

					// Send 6 Keepalives and wait for their Acks.
					numberOfExtraKeepalives = 5;
					Console.WriteLine("Sending Keepalive");
					connection.Send(new Keepalive());
					lock (this)
					{
						Monitor.Wait(this);
					}

					// Send Unregister and wait for UnregisterAck
					connection.Send(new Unregister());
					lock (this)
					{
						Monitor.Wait(this);
					}

					Console.WriteLine("We registered, exchanged keepalives, and unregistered!!!");
				}
				else
				{
					Console.WriteLine("Connect failed");
				}
				connection.Stop();
			}
			else
			{
				Console.WriteLine("Could not start connection");
			}
		}

		private volatile bool connected;

		private int numberOfExtraKeepalives;

		private bool HandleConnected(SccpConnection connection, bool connected)
		{
			this.connected = connected;
			lock (this)
			{
				Monitor.Pulse(this);
			}

			bool proceedWithReads = true;
			return proceedWithReads;
		}

		private void HandleRegisterAck(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received RegisterAck!!!");
		}

		private void HandleRegisterReject(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received RegisterReject!!! :-7");
		}

		private void HandleCapabilitiesReq(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received CapabilitiesReq");

			connection.Send(new HeadsetStatus(false));

			ArrayList capabilities = new ArrayList();
			capabilities.Add(new CapabilitiesRes.MediaCapability(PayloadType.WideBand256k, 120));
			capabilities.Add(new CapabilitiesRes.MediaCapability(PayloadType.G711Ulaw64k, 40));
			capabilities.Add(new CapabilitiesRes.MediaCapability(PayloadType.G711Alaw64k, 40));
			capabilities.Add(new CapabilitiesRes.MediaCapability(PayloadType.G729AnnexB, 60));
			capabilities.Add(new CapabilitiesRes.MediaCapability(PayloadType.G729AnnexAwAnnexB, 60));
			capabilities.Add(new CapabilitiesRes.MediaCapability(PayloadType.G729, 60));
			capabilities.Add(new CapabilitiesRes.MediaCapability(PayloadType.G729AnnexA, 60));
			connection.Send(new CapabilitiesRes(capabilities));

			connection.Send(new HeadsetStatus(false));
			connection.Send(new ButtonTemplateReq());
		}

		private void HandleButtonTemplate(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received ButtonTemplate");

			connection.Send(new SoftkeyTemplateReq());
		}

		private void HandleSoftkeyTemplateRes(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received SoftkeyTemplateRes");

			connection.Send(new SoftkeySetReq());
		}

		private void HandleSoftkeySetRes(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received SoftkeySetRes");

			// Do nothing.
		}

		private void HandleSelectSoftkeys(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received SelectSoftkeys");

			// Do nothing.
		}

		private const int numberOfLines = 6;
		private int lineStatReqLineNumber;
		private int numberOfDisplayPromptStatusMessagesReceived;

		private void HandleDisplayPromptStatus(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received DisplayPromptStatus");

			if (numberOfDisplayPromptStatusMessagesReceived++ == 0)
			{
				lineStatReqLineNumber = numberOfLines;
				connection.Send(new LineStatReq(lineStatReqLineNumber--));
			}
			else
			{
				// Indicate fully registered now.
				lock (this)
				{
					Monitor.Pulse(this);
				}
			}
		}

		private void HandleLineStat(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received LineStat");

			if (lineStatReqLineNumber >= 1)
			{
				connection.Send(new LineStatReq(lineStatReqLineNumber--));
			}
			else
			{
				connection.Send(new RegisterAvailableLines(numberOfLines));
				connection.Send(new TimeDateReq());
			}
		}

		private void HandleDefineTimeDate(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received DefineTimeDate");

			// Do nothing.
		}

		private void HandleUnregisterAck(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received UnregisterAck");

			// We're done!
			lock (this)
			{
				Monitor.Pulse(this);
			}
		}

		private void HandleKeepaliveAck(SccpConnection connection, SccpMessage message)
		{
			Console.WriteLine("Received KeepaliveAck");

			if (numberOfExtraKeepalives-- > 0)
			{
				Thread.Sleep(1000);	// Sleep 1 second between Keepalives
				Console.WriteLine("Sending Keepalive");
				connection.Send(new Keepalive());
			}
			else
			{
				lock (this)
				{
					Monitor.Pulse(this);
				}
			}
		}
	}
}
