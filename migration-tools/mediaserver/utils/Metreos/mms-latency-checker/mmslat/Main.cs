//
// mmslat/Main.cs
//
using System;
using System.Net;
using System.Xml;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;

using Metreos.Utilities;    // x:\samoa-framework\Utilities\obj\Debug\Metreos.Utilities.dll
using Metreos.Core.IPC.Xml; // x:\samoa-framework\Core\obj\Debug\Metreos.Core.dll
// command line example: msip(127.0.0.1) samples(8) hairpin(1) msglvl(0)


   
namespace mmslat
{	
	class main
	{		
		[STAThread]
		static void Main(string[] args)
		{		
            Console.WriteLine("\nMetreos conferenced media latency measurement tool v1.1\n");

            int  cmdlineErrors = Cmdline.Parse(args); 
            if  (cmdlineErrors > 0)             
                 Console.WriteLine("{0} command line errors detected", cmdlineErrors);
            else
            if (!Cmdline.IsRequiredArgsPresent)
                 Cmdline.ShowSyntax();
            else isCmdlineOK = true;

            if  (isCmdlineOK) 
            {
                Cmdline.ShowCurrent();

                new Thread(new ThreadStart(RunThread)).Start();
            }            
            else WaitForEnterKey();
		}


        /// <summary>State monitor thread procedure</summary> 
        public static void RunThread()
        {
            try
            {   LatMain.Instance.Start();
                
                while(LatMain.state != Const.STATE_CLOSED) Thread.Sleep(250);
            }
            catch { }

            WaitForEnterKey();
        }

        private static void WaitForEnterKey()
        {
            Console.Write("\nhit enter to exit ...");    
            Console.ReadLine();
        }

        public  static long packetIntervalMs = Const.DefaultPacketIntervalsMs;  
        public  static int  packetsOfMedia   = Const.DefaultPacketsOfMedia;
        public  static int  packetsOfSilence = Const.DefaultPacketsOfSilence;
        private static bool isCmdlineOK = false;

    }  // class main



    public class LatMain
    {
        #region singleton
        private LatMain() {}
        private static LatMain instance;
  
        public  static LatMain Instance
        {
            get 
            {   if (instance == null) instance = new LatMain();                 
                return instance;
            }
        }
        #endregion
         
        public void Start()
        {
            msglvl = Cmdline.msglvl;
            mmsClientA = new MmsClient(1);
            mmsClientA.raiseResponseReceived = new MmsResponseDelegate(OnMmsResponseReceived);

            if  (StartIpcCLient(mmsClientA, Cmdline.mediaServerIP, Const.MediaServerIpcPort)) 

                 EventLoop(Const.ServerConnectTransID);

            else state = Const.STATE_CLOSED;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Event loop -- take action based on current state
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static int EventLoop(int newstate)
        {
            if (newstate != 0) state = newstate;

            switch(state) // if MMS command, state and MMS transaction ID will be the same 
            {
               case Const.EventLimboState:   
                    break;

               case Const.ServerConnectTransID:
                    mmsClientA.PostMsg(MessageFactory.ServerConnect(state));
                    break;

               case Const.ConxAFullConnectTransID:
                    mmsClientA.PostMsg(MessageFactory.FullConnectToNewConference
                       (mmsClientA.ClientID, state, Cmdline.testClientIP, 
                        Cmdline.testClientPort, Cmdline.isHairpinning));                  
                    break;

               case Const.ConxBFullConnectTransID:   
                    mmsClientA.PostMsg(MessageFactory.FullConnectToExistingConference
                        (mmsClientA.ClientID, state, Cmdline.testClientIP, Cmdline.testClientPort, conferenceID));
                    break;

               case Const.StartSocketListenerState:
                    StartListening();
                    break;

               case Const.StartLatencyTestState:
                    LatMain.Instance.StartTest();
                    break;

               case Const.ContinueLatencyTestState:
                    break;

               case Const.STATE_SHUTDOWN:
               case Const.ServerDisconnectTransID: 
                    state = Const.ServerDisconnectTransID;             
                    mmsClientA.PostMsg(MessageFactory.ServerDisconnect(mmsClientA.ClientID, state));                   
                    break;

               case Const.STATE_CLOSED:
                    break;

               case Const.STATE_QUIT:
               default:  
                    StoptIpcClient(mmsClientA);
                    Cleanup(); 
                    state = Const.STATE_CLOSED;                                 
                    return 1;    
            }

            return 0;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // MMS response handler -- handle response from MMS based on current state
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Callback for media server response</summary>       
        public static void OnMmsResponseReceived(int clientID, string xml)
        {            
            Handlers.MmsResultData mmsResult = null;
            if (msglvl == Const.MSGLVL_DEBUG)    
            {   Console.WriteLine("\ninbound xml:");              
                Console.WriteLine(xml == null? "null": xml);
            }

            try  { mmsResult = InterpretMmsResult(xml); }
            catch(Exception x) { Console.WriteLine(x.Message); }

            int newstate = Const.STATE_QUIT;
            int transactionID = mmsResult == null? Const.STATE_QUIT: mmsResult.transID;
                      
            switch(transactionID)   
            { 
               case Const.ServerConnectTransID:
                    mmsClientA.ClientID = mmsResult.clientID;
                    newstate = mmsResult.resultcode == 0? 
                               Const.ConxAFullConnectTransID: Const.STATE_QUIT;

                    Console.WriteLine("test client at {0} connected to media server {1}",
                                       Cmdline.testClientIP, Cmdline.mediaServerIP);

                    SleepEx(interCommandWaitMs);
                    break;

               case Const.ConxAFullConnectTransID:
                    remoteIP = mmsResult.ipaddr;
                    connection1port = mmsResult.port; 
                    connection1IP   = mmsResult.ipaddr;
                    connectionID1 = mmsResult.connectionID & Const.CONXID_MASK;
                    conferenceID  = mmsResult.conferenceID & Const.CONXID_MASK;
                    newstate = mmsResult.resultcode == 0? 
                               Const.ConxBFullConnectTransID: Const.STATE_QUIT; 

                    Console.WriteLine("media server connection ID {0} connected", 
                                       mmsResult.connectionID);
                    Console.WriteLine("media server {0} conference {1} established", 
                                       Cmdline.isHairpinning? Const.conftypeHairpin: Const.conftypeFirmware, 
                                       mmsResult.conferenceID);

                    SleepEx(interCommandWaitMs); 
                    break; 

               case Const.ConxBFullConnectTransID:
                    connectionID2   = mmsResult.connectionID & Const.CONXID_MASK;
                    connection2port = mmsResult.port;
                    connection2IP   = mmsResult.ipaddr;

                    newstate = mmsResult.resultcode == 0? 
                               Const.StartSocketListenerState: Const.STATE_QUIT;  

                    Console.WriteLine("media server connection ID {0} joins conference {1}\n", 
                                       mmsResult.connectionID, mmsResult.conferenceID);

                    SleepEx(interCommandWaitMs);
                    break; 

               case Const.ServerDisconnectTransID:
                    newstate = Const.STATE_QUIT;
                    break;
 
               case Const.STATE_QUIT:
                    return;
            }

            EventLoop(newstate);
        }   // OnMmsResponseReceived



        /// <summary>Check mms response xml and populate result data structure</summary> 
        private static Handlers.MmsResultData InterpretMmsResult(string xml)
        {
            Handlers.MmsResultData rd = Handlers.InterpretMmsResult(xml);

            if (msglvl == Const.MSGLVL_DEBUG)
            {
                Console.WriteLine("\nmedia server response follows");
                Console.WriteLine("message ID ...... " + rd.messageID);
                Console.WriteLine("result code ..... " + rd.resultcode);
                Console.WriteLine("client ID ....... " + rd.clientID);
                Console.WriteLine("server ID ....... " + rd.serverID);
                Console.WriteLine("connection ID ... " + rd.connectionID);
                Console.WriteLine("conference ID ... " + rd.conferenceID);
                Console.WriteLine("transaction ID .. " + rd.transID);
                Console.WriteLine("term condition .. " + rd.termcond);
                Console.WriteLine("ip address ...... " + rd.ipaddr);
                Console.WriteLine("port ............ " + rd.port);
                Console.WriteLine();
            }

            return rd;
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Latency test - send RTP through MMS conference
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Initiate testing, with logging</summary>
        private void StartTest()
        {
            int nextstate = Const.STATE_SHUTDOWN;
            if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("starting latency test sequence ...");

            if (StartLatencyTest()) 
            {
                nextstate = Const.ContinueLatencyTestState;
                if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("latency test sequence started");
            }
            else Console.WriteLine("could not start latency test sequence - bailing");
         
            EventLoop(nextstate);           
        }


        /// <summary>Start the latency test</summary>
        private bool StartLatencyTest()
        {
            packetSequenceNumber = 1;
            totalSamples = 0;
            packetTime = 0;
            maxpackets = ((main.packetsOfMedia + main.packetsOfSilence) * Cmdline.samplesToTest) + 1;

            bool result  = false;
            string ip    = connection1IP;
            int    port  = connection1port;

            quietPayload = new byte[Const.RTP_PAYLOAD_SIZE];
            noisyPayload = new byte[Const.RTP_PAYLOAD_SIZE];

            for(int i = 0; i < Const.RTP_PAYLOAD_SIZE; i++)
            {
                quietPayload[i] = (byte) 0xff;
                noisyPayload[i] = (byte) 0x5a;
            }

            if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("creating write socket ...");

            try
            {   writeSocket = new Socket
                    (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                mediaServerEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                result = true;
            }
            catch
            {
                Console.WriteLine("could not create mms endpoint at " + ip + Const.slash + port);
            }

            if (!result) return false;
            if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("mms endpoint established at " 
                          + ip + Const.slash + port);
            if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("starting cadence timer ...");

            packetCadenceTimer = new TimerManager(this.GetType().FullName, 
                    new WakeupDelegate(OnPacketWriteInterval), 
                    new WakeupExceptionDelegate(OnWritePacketTimerFailure),
                    1,  // initial threads
                    2); // max threads

            packetCadenceTimer.Add(main.packetIntervalMs);

            Console.WriteLine("begin rtp packet cadence ...\n");   
            return true;
        }


        /// <summary>Actions on packet cadence timer fire</summary>
        private long OnPacketWriteInterval(TimerHandle h, object s)
        {
           int  nextstate = Const.STATE_SHUTDOWN;
           int  result = -1;

           if  (totpackets > maxpackets)
                Console.WriteLine("\npacket count overflow - bailing");
           else
           {    result = SendRtpPacket();
           
                if  (result >= 0)
                {    
                     totpackets++;
                     nextstate = Const.ContinueLatencyTestState;

                     #if(false)
                     if (msglvl == Const.MSGLVL_DEBUG) 
                         Console.Write(result == 1? Const.plus: Const.minus);
                     #endif
                }
                else Console.WriteLine("\ncould not send rtp packet - bailing");
           }
          
           EventLoop(nextstate);
           return 0;
        }


        /// <summary>Construct and write an rtp packet</summary>
        private int SendRtpPacket()
        {
            packetCadenceTimer.Add(main.packetIntervalMs); // is this necessary?
            byte[] packetSequence  = System.BitConverter.GetBytes(++packetSequenceNumber);
            byte[] packetTimestamp = System.BitConverter.GetBytes(packetTime += Const.RTP_PACKET_TIMESTAMP_INCREMENT);

            byte[] rtpHeader = new byte[Const.RTP_HEADER_SIZE]; 
            Array.Clear (rtpHeader, 0,  Const.RTP_HEADER_SIZE);
            rtpHeader[0] = (byte)128;
            rtpHeader[1] = (byte)100;           // ssrc low bit ?
            rtpHeader[2] = packetSequence[1];   // endian  
            rtpHeader[3] = packetSequence[0];
            rtpHeader[4] = packetTimestamp[3];  // endian
            rtpHeader[5] = packetTimestamp[2]; 
            rtpHeader[6] = packetTimestamp[1]; 
            rtpHeader[7] = packetTimestamp[0];         

            byte[] rtpPacket  = new byte[Const.RTP_PACKET_SIZE];
            byte[] rtpPayload = isTransmittingNoisyMedia? noisyPayload: quietPayload;
            int whichType = isTransmittingNoisyMedia? 1: 0;

            Array.Copy(rtpHeader,  0, rtpPacket, 0, Const.RTP_HEADER_SIZE);
            Array.Copy(rtpPayload, 0, rtpPacket, Const.RTP_HEADER_SIZE, Const.RTP_PAYLOAD_SIZE);

            SetElapsedPacketTimeStopwatch();
            bool result = false;

            try  
            {   writeSocket.SendTo(rtpPacket, mediaServerEndpoint); 
                result = true; 
            }
            catch(Exception x) { Console.WriteLine("socket send failed {0}", x); }
            if (!result) return -1;

            if (isTransmittingNoisyMedia)      // Currently transmitting non-silence
            {                                   
                if (packetCount++ == main.packetsOfMedia)
                {                              // Start transmitting silence
                    isTransmittingNoisyMedia = false;
                    packetCount = 0;
                    packetTimerStartTime = 0;  // Trigger new timer
                }
            }
            else                               // Currently transmitting silence
            if  (packetCount++ == main.packetsOfSilence)
            {                                  // Start transmitting non-silence
                 isTransmittingNoisyMedia   = true;
                 isSilenceToNoiseTransition = true;
                 packetCount = 0;
                 totalSamples++;
                 packetTimerStartTime = 0;     // Trigger new timer            
            }

            return whichType;
        }   // SendRtpPacket



        private void OnWritePacketTimerFailure(TimerHandle handle, object state, Exception x)
        {
            #if(false)
            Console.WriteLine("Timer failure {0}", x);
            EventLoop(Const.STATE_SHUTDOWN);
            #endif
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Socket receive  
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>Start rx socket listener, with logging</summary>
        private static void StartListening()
        {
            int nextstate = Const.STATE_SHUTDOWN;
            if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("starting listener ...");

            if (StartSocketListener())
            {   
                nextstate = Const.StartLatencyTestState;
                if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("listener started");
            }
            else Console.WriteLine("could not start socket listener - bailing");           
            
            EventLoop(nextstate);
        }


        /// <summary>Start the rx socket listener</summary>
        private static bool StartSocketListener()
        {
            bool result = false;

            try 
            {   readThread = new Thread(new ThreadStart(SocketReceive));
                readThread.IsBackground = true;
                readThread.Start();
                result = true;
            } 
            catch(Exception x) { Console.WriteLine(x.Message); }
          
            return result;
        }


        /// <summary>rx socket thread procedure</summary>
        private static void SocketReceive()
        {
            string ip   = Cmdline.testClientIP;
            int    port = Cmdline.testClientPort;  
          
            int    bytesRead = 0;
            byte[] readbuf = new byte[Const.RTP_PACKET_SIZE];

            readSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            readSocket.Bind(new IPEndPoint(IPAddress.Any, port));

            if (msglvl == Const.MSGLVL_DEBUG) 
                Console.WriteLine("receive socket bound to " + ip + Const.slash + port);
               
            while(true)
            {      
                bytesRead = 0;        
                 
                try{bytesRead = readSocket.Receive(readbuf);} catch { }   

                if (bytesRead == 0) 
                {   
                    if  (state >= Const.STATE_SHUTDOWN) return;
                    throw new Exception("socket closed -- terminating read thread");
                } 

                if (isSilenceToNoiseTransition)
                {
                    byte testByte1 = Buffer.GetByte(readbuf, Const.RTP_PACKET_SIZE - 2);
                    byte testByte2 = Buffer.GetByte(readbuf, Const.RTP_PACKET_SIZE - 1);                    
                    if (IsRtpSilence(testByte1) || IsRtpSilence(testByte2)) continue;

                    long elapsedMs = Metreos.Utilities.HPTimer.MillisSince(packetTimerStartTime); 
                    totelapsed += elapsedMs;
                    Console.WriteLine("sample {0} ms {1}", totalSamples, elapsedMs);
                    isSilenceToNoiseTransition = false;
                }

                if  (totalSamples >= Cmdline.samplesToTest)
                {
                     if (msglvl == Const.MSGLVL_DEBUG) 
                         Console.WriteLine("max samples reached -- normal rx thread termination");                                         
                
                     Console.WriteLine("\ntotal samples {0}: average ms/sample {1}",
                                        totalSamples, totelapsed / totalSamples);

                     state = Const.STATE_SHUTDOWN;
                     break;
                }
            }

            EventLoop(0);  // Run state loop on read thread end
             
        }  // SocketReceive()



        /// <summary>Determine if specified byte represent RTP silence</summary>
        private static bool IsRtpSilence(byte x)
        {
            switch(x) { case 0x7f: case 0xff: return true; }
            return false;
        }
      

        private static void SetElapsedPacketTimeStopwatch()
        {
            if (packetTimerStartTime == 0) 
                packetTimerStartTime = Metreos.Utilities.HPTimer.Now();
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Utility methods
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        private static void SleepEx(int ms)
        {
            if (msglvl == Const.MSGLVL_DEBUG) Console.WriteLine("sleeping " + ms + " ...");
            Thread.Sleep(ms);
        }


        /// <summary>Start the MMS IPC client socket</summary>       
        private static bool StartIpcCLient(MmsClient client, string ip, int port)
        {
            if (msglvl == Const.MSGLVL_DEBUG)
                Console.WriteLine("starting ipc client " + client.ClientID 
                                   + ": " + ip + Const.slash + port + " ...");

            bool result = client.Start(ip, port);

            if (result && msglvl == Const.MSGLVL_DEBUG)
                Console.WriteLine("ipc client " + client.ClientID + " started");

            return result;
        }



         /// <summary>Stop the MMS IPC client socket</summary>
        private static bool StoptIpcClient(MmsClient client)
        {
            if (msglvl == Const.MSGLVL_DEBUG)
                Console.WriteLine("\nstopping ipc client " + client.ClientID);

            try { client.Stop(); } catch { }
            return true;
        }


        private static void Cleanup()
        {
            if (packetCadenceTimer != null)
            {
                packetCadenceTimer.RemoveAll();
                packetCadenceTimer.Shutdown();
            }

            if (writeSocket != null)
                try{ writeSocket.Close(); } catch { }

            if (readSocket != null)
                try{ readSocket.Close();  } catch { }
        }
       

        private static MmsClient mmsClientA = null; 

        private static int transID;
        public  static int NewTransID() { return ++transID; }
        public  static int CurrentTransactionID { get { return transID; } set { transID = value; } }
        private const  int mmsClient1 = 1;

        private static int conferenceID = 0;
        private static int connectionID1= 0, connectionID2 = 0;
        private static int connection1port = 0, connection2port = 0;
        private static string remoteIP  = null;
        private static string connection1IP = null, connection2IP = null;

        public  static int state  = -1; 
        private static int maxpackets = 0, totpackets = 0;
        private static int totalSamples = 0;
        public  static int msglvl = Const.MSGLVL_DEBUG;  
        private static int interCommandWaitMs = 750; 
        private static long totelapsed = 0; 

        private ushort packetSequenceNumber = 0;
        private uint   packetTime  = 0;
        private int    packetCount = 0;
        private static byte[] quietPayload, noisyPayload;
        private static Metreos.Utilities.TimerManager packetCadenceTimer;

        private static bool isSilenceToNoiseTransition = false;
        private static bool isTransmittingNoisyMedia = false;
        private static long packetTimerStartTime = 0;

        private static IPEndPoint mediaServerEndpoint = null;
        private static Thread readThread = null;  
        private static Socket readSocket = null, writeSocket = null;

	}   // class latmain
}       // namespace
