using System;
using System.Net; 
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using Metreos.Utilities;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.Messaging;
using Metreos.Samoa.FunctionalTestFramework;

using MediaLatencyTest = Metreos.TestBank.Core.Core.MediaLatency;

namespace Metreos.FunctionalTests.Standard.Core.MediaBench
{
    /// <summary>Test media latency interactively</summary>
    [Exclusive(IsExclusive=true)]
    [FunctionalTestImpl(IsAutomated=false)]
    public class MediaLatency : FunctionalTestBase
    {
        public const int packetsOfSilence = 50;
        public const int packetsOfMedia = 50; 
        public const string hairpinOption = "Hairpin";
        public const string testButton = "Test";
        public const string receiveIp = "RxIP";
        public const string receivePort = "RxPort";
        public const string exitTestButton = "Exit";

        private const int loopCount = 1000;
        private bool success;
        private bool exit;
        private string routingGuid;
        private Thread readThread;
        private TimerManager tm;
        private long start;
        private Socket readSocket;
        private AutoResetEvent are;
        private string rxIp;
        private string rxPort;
        private volatile bool testing;
        private volatile bool mediaSpike;
        private int packetCount;
        private Socket writeSocket;
        private IPEndPoint mediaserver;
        private uint time;
        private ushort sequence;
        private static byte[] noisyPayload;
        private static byte[] quietPayload;
        //private static long last = 0;

        public MediaLatency() : base(typeof( MediaLatency ))
        {
            are = new AutoResetEvent(false);
        }

        public override bool Execute()
        {
            Hashtable fields = new Hashtable();
            fields["testClientIp"] = Input[receiveIp];
            fields["testClientPort"] = Input[receivePort];
            fields["hairpin"] = (bool) Input[hairpinOption];

            // First we trigger script, and tell it to exit. After exit, we check that it exited or not
            TriggerScript( MediaLatencyTest.script1.FullName, fields );

            if(!WaitForSignal(MediaLatencyTest.script1.S_Simple.FullName) )
            {
                log.Write(System.Diagnostics.TraceLevel.Info, "No response from trigger.");
                return false;
            }

            if(success)
            {
                // Received success, so conference is ready for sending media

                readThread = new Thread(new ThreadStart(Receive));
                readThread.IsBackground = true;
                Thread.Sleep(1000); // Let media conference stabilize
                log.Write(TraceLevel.Info, "READY!");
                readThread.Start();
                
                are.WaitOne();
                SendEvent(MediaLatencyTest.script1.E_Shutdown.FullName, routingGuid);
                return true;

            }
            else
            {
                log.Write(TraceLevel.Error, "Did not succeed in starting conference");
                return false;
            }
        }

        private bool OnTestLatencyEvent(string name, string @value)
        {
            if(tm != null) return true;

            sequence = 1;
            time = 0;
            quietPayload = new byte[160];
            noisyPayload = new byte[160];
            for(int i = 0; i < quietPayload.Length; i++)
            {
                quietPayload[i] = (byte) 0xFF;
            }
            for(int i = 0; i < noisyPayload.Length; i++)
            {
                noisyPayload[i] = (byte) 0x5a;
            }
            
            writeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mediaserver = new IPEndPoint(IPAddress.Parse(rxIp), int.Parse(rxPort));
                
            tm = new TimerManager(this.GetType().FullName, new WakeupDelegate(WritePacket), new WakeupExceptionDelegate(TimerFailure), 1, 2);
            tm.Add(20);
            return true;
        }

        private void Ready(ActionMessage im)
        {
            routingGuid = im.RoutingGuid;
            success = (bool) im["success"];

            if(success)
            {
                rxIp = im["rxIp"] as string;
                rxPort = im["rxPort"] as string;
            }
        }    

        private void Receive()
        {
            readSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            readSocket.Bind(new IPEndPoint(IPAddress.Parse(Input[receiveIp] as string), int.Parse(Input[receivePort] as String)));

            byte[] buffer = new byte[172];
            
            int amountRead;
            while(!exit)
            {
                try
                {
                    amountRead = readSocket.Receive(buffer);
                }
                catch(Exception e)
                {
                    log.Write(TraceLevel.Error, "Read failed: {0}", e);
                    are.Set();
                    break;
                }
            
                if(amountRead == 0)
                {
                    log.Write(TraceLevel.Error, "Socket closed.  Exiting.");
                    are.Set();
                    break;
                }

                if(testing)
                {
                    byte test = System.Buffer.GetByte(buffer, 170);
                    byte test2 = System.Buffer.GetByte(buffer,171);

                    if((test == 0x7F || test == 0xFF) && (test2 == 0x7F) || (test2 == 0xFF))
                    {
                        // Silence.  Carry on.
                        continue;
                    }
                    else
                    {
                        long duration = Metreos.Utilities.HPTimer.MillisSince(start);
                        log.Write(TraceLevel.Info, "Milliseconds: {0}", duration);
                        this.testing = false;
                        continue;
                    }
                }
            }

            log.Write(TraceLevel.Info, "Read Thread exiting");
        }

        public override string[] GetRequiredTests()
        {
            return new string[] { ( MediaLatencyTest.FullName ) };
        }

        public override CallbackLink[] GetCallbacks()
        {
            return new CallbackLink[] { 
                                          new CallbackLink( MediaLatencyTest.script1.S_Simple.FullName , new FunctionalTestSignalDelegate(Ready)),
                                      };
        }

        public override ArrayList GetRequiredUserInput()
        {
            TestTextInputData receiveIpItem = new TestTextInputData(receiveIp,"Enter FTF Client IP" , receiveIp, "ENTER FTF Client IP", 80);
            TestTextInputData receivePortItem = new TestTextInputData(receivePort, receivePort, receivePort, "40000", 80);
            TestBooleanInputData hairpinOptionChoice = new TestBooleanInputData(hairpinOption, "Hairpin", hairpinOption, true);
            TestUserEvent testButtonEvent = 
                new TestUserEvent(testButton, "Test Latency", testButton, 
                testButton, new CommonTypes.AsyncUserInputCallback(OnTestLatencyEvent));

            TestUserEvent exitButtonEvent = 
                new TestUserEvent(exitTestButton, "Exit", exitTestButton, 
                exitTestButton, new CommonTypes.AsyncUserInputCallback(OnExit));

            ArrayList inputs = new ArrayList();
            inputs.Add(receiveIpItem);
            inputs.Add(receivePortItem);
            inputs.Add(hairpinOptionChoice);
            inputs.Add(testButtonEvent);
            inputs.Add(exitButtonEvent);

            return inputs;
        }


        public bool OnExit(string name, string @value)
        {
            if(tm != null)
            {
                this.tm.RemoveAll();
                this.tm.Shutdown();
                this.tm = null;
            }

            try
            {
                writeSocket.Close();
            }
            catch { }
            try
            {
                readSocket.Close();
            } 
            catch{}

            writeSocket = null;
            readSocket = null;
            this.exit = true;
            this.testing = false;
                
            return true;
        }


        public override void Initialize()
        {
            if(tm != null)
            {
                this.tm.RemoveAll();
                this.tm.Shutdown();
                this.tm = null;
            }

            try
            {
                writeSocket.Close();
            }
            catch { }
            try
            {
                readSocket.Close();
            } 
            catch{}

            writeSocket = null;
            readSocket = null;
            this.testing = false;
            this.mediaSpike = false;
            this.packetCount = 0;
            this.exit = false;
            this.success = false;
            this.routingGuid = null;
            this.readThread = null;
        }

        public override void Cleanup()
        {
            if(tm != null)
            {
                this.tm.RemoveAll();
                this.tm.Shutdown();
                this.tm = null;
            }
            
            try
            {
                writeSocket.Close();
            }
            catch { }
            try
            {
                readSocket.Close();
            } 
            catch{}

            writeSocket = null;
            readSocket = null;

            this.testing = false;
            this.mediaSpike = false;
            this.packetCount = 0;
            this.exit = false;
            this.success = false;
            this.readThread = null;
            this.routingGuid = null;
        }

        private void TimerFailure(TimerHandle handle, object state, Exception e)
        {
        }


        private long WritePacket(TimerHandle handle, object state)
        {
            tm.Add(20);

            // Create socket, send off packet
            byte[] header = new byte[12];
            byte byte1 = (byte) 128;
            byte ssrcLowBit = (byte) 100; 
            Array.Clear(header, 0, header.Length);
            header[0] = byte1;
            header[11] = ssrcLowBit;

            byte[] total = new byte[header.Length + 160];

            Array.Copy(header, 0, total, 0, header.Length);
            Array.Copy(mediaSpike ? noisyPayload : quietPayload, 0, total, header.Length, 160);
            
            if(start == 0) { start = HPTimer.Now(); }

            sequence++;
            byte[] sequenceWord = System.BitConverter.GetBytes(sequence);
            total[2] = sequenceWord[1];
            total[3] = sequenceWord[0];

            time += 160;
            byte[] timestamp = System.BitConverter.GetBytes(time);
            total[4] = timestamp[3];
            total[5] = timestamp[2];
            total[6] = timestamp[1];
            total[7] = timestamp[0];

            writeSocket.SendTo(total, mediaserver);

            // Determine if we are sending a new spike
            packetCount++;
            if(mediaSpike)
            {
                if(packetCount == packetsOfMedia - 1)
                {
                    mediaSpike = false;
                    packetCount = 0;
                    start = 0;
                }
            }
            else
            {
                if(packetCount == packetsOfSilence - 1)
                {
                    mediaSpike = true;
                    testing = true;
                    packetCount = 0;
                    start = 0;
                }
            }

            return 0;
        }
    } 
}
