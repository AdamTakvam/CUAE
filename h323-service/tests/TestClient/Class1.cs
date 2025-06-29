using System;
using System.Net;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Messaging.MediaCaps;

using Metreos.CallControl.H323;

namespace TestClient
{
    class Class1
    {
        private static Metreos.CallControl.H323.H323IpcClient client;

        [STAThread]
        static void Main(string[] args)
        {
            client = new Metreos.CallControl.H323.H323IpcClient();
            client.Log = new Metreos.LoggingFramework.LogWriter();

            client.onIncomingCall     += new OnIncomingCallDelegate(client_onIncomingCall);
            client.onGotCapabilities  += new OnGotCapabilitiesDelegate(client_onGotCapabilities);
            client.onCallEstablished  += new OnCallEstablishedDelegate(client_onCallEstablished);
            client.onCallEstablished += new OnCallEstablishedDelegate(client_onCallEstablished);
            client.onMediaEstablished += new OnMediaDelegate(client_onMediaEstablished);
            client.onMediaChanged     += new OnMediaDelegate(client_onMediaChanged);
            client.onCallCleared      += new OnCallClearedDelegate(client_onCallCleared);
            client.onGotDigits        += new OnGotDigitsDelegate(client_onGotDigits);

            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8500);
            client.Startup(ipe);

            client.SendH323StartStackMessage();

            Console.WriteLine("Stack started.");
            Console.WriteLine("Press 'q' to quit.");

            string choice = Console.ReadLine().ToLower();
            while(choice != "q")
            {
                if(choice.StartsWith("m"))
                {
                    string[] mcArgs = choice.Split(null, 2);
                    if(mcArgs.Length >= 2)
                    {
                        Console.WriteLine("Calling {0}", mcArgs[1]);
                        MediaCapsField caps = new MediaCapsField();
                        caps.Add(IMediaControl.Codecs.G711u, new uint[] { 20, 30 });
                        client.SendMakeCallMessage(5000, mcArgs[1], "Test", "DN", "10.1.1.5", 5000, caps);
                    }
                }
                else
                {
                    Console.WriteLine("Press 'q' to quit.");
                }

                choice = Console.ReadLine().ToLower();
            }

            client.SendH323StopStackMessage();

            client.Close();
        }

        private static void client_onIncomingCall(string callId, string from, string to, string displayName)
        {
            Console.WriteLine("{0}: Incoming call: {1} -> {2}", callId, from, to);

            client.SendAcceptMessage(callId, "H323 Foo");
           
            MediaCapsField sampleCaps = new MediaCapsField();
            sampleCaps.Add(IMediaControl.Codecs.G711u, new uint[] { 10, 20, 30 } );
            sampleCaps.Add(IMediaControl.Codecs.G711a, new uint[] { 10, 20, 30 } );
            
            client.SendSetMediaMessage(callId, null, 0, null, 0, sampleCaps);
            
            client.SendAnswerMessage(callId, "DN");
        }

        private static void client_onGotCapabilities(string callId, Metreos.Messaging.MediaCaps.MediaCapsField caps)
        {
            Console.WriteLine("{0}: Got caps", callId);
            Console.WriteLine(caps.ToString());
            
            Random r = new Random();
            int port = r.Next(40000, 42000);

            client.SendSetMediaMessage(callId, "10.1.12.152", (uint)port, IMediaControl.Codecs.G711u.ToString(), 20, null);
        }

        private static void client_onCallEstablished(string callId, string to, string from)
        {
            Console.WriteLine("{0}: Established", callId);
        }

        private static void client_onMediaEstablished(string callId, uint direction, string txIP, 
            uint txPort, IMediaControl.Codecs rxCodec, uint rxFramesize)
        {
            Console.WriteLine("{0}: Media Established txIp: {1} txPort: {2}", callId, txIP, txPort);
        }

        private static void client_onCallCleared(string callId, ICallControl.EndReason reason)
        {
            Console.WriteLine("{0}: Cleared. Reason: {1}", callId, reason);
        }

        private static void client_onMediaChanged(string callId, uint direction, string txIP, 
            uint txPort, IMediaControl.Codecs rxCodec, uint rxFramesize)
        {
            Console.WriteLine("{0}: Media Changed", callId);
        }

        private static void client_onGotDigits(string callId, string digits)
        {
            Console.WriteLine("{0}: Got Digit: {1}", callId, digits);
        }
    }
}
