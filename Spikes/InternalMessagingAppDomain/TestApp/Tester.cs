using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestApp
{
    class Tester
    {
        private const int NUM_DOMAINS                   = 25;
        private const int NUM_MESSAGES_FOR_DELAY_TEST   = 100;
        private const int NUM_MESSAGES_FOR_SPEED_TEST   = 1000;
        private const int NUM_FIELDS_FOR_TESTS          = 10;

        private AppDomain[] domains = new AppDomain[NUM_DOMAINS];
        private AppDomainWorker[] workers = new AppDomainWorker[NUM_DOMAINS];

        private SimpleMessageQueueProvider mainQueue = new SimpleMessageQueueProvider();

        private string assemblyName = Assembly.GetExecutingAssembly().FullName;

        public void Go()
        {
            for(int i = 0; i < NUM_DOMAINS; i++)
            {
                CreateDomainTester(i);
            }

            SpeedTest();
            RoundTripDelayTest();
            BinarySerializationTest();

            StopAllDomainWorkers();
        }

        public void CreateDomainTester(int i)
        {
            domains[i] = AppDomain.CreateDomain("Domain Tester: " + i);
            workers[i] = (AppDomainWorker)domains[i].CreateInstanceAndUnwrap(assemblyName, "TestApp.AppDomainWorker");
            
            workers[i].Go();
        }

        public void RoundTripDelayTest()
        {
            InternalMessage msg = BuildLoadedTestMessage();

            DateTime msgSent;
            DateTime responseReceived;

            InternalMessage responseMsg;
            bool gotMessage = false;

            int currentDomain = 0;

            TimeSpan[] delays = new TimeSpan[NUM_MESSAGES_FOR_DELAY_TEST];

            for(int i = 0; i < NUM_MESSAGES_FOR_DELAY_TEST; i++)
            {
                msgSent = DateTime.Now;
                workers[currentDomain].PostMessage(msg);

                gotMessage = mainQueue.Receive(new TimeSpan(0, 0, 0, 10, 0), out responseMsg);

                if(gotMessage == true)
                {
                    responseReceived = DateTime.Now;
                    delays[i] = responseReceived - msgSent;
                }
                else
                {
                    Console.WriteLine("Did not get round-trip delay test response");
                    return;
                }

                currentDomain = (currentDomain + 1 >= NUM_DOMAINS) ? 0 : currentDomain++;
            }

            double sum = 0.0;
            double avgTime = 0.0;

            for(int j = 0; j < NUM_MESSAGES_FOR_DELAY_TEST; j++)
            {
                sum += delays[j].TotalMilliseconds;
            }

            avgTime = sum / NUM_MESSAGES_FOR_DELAY_TEST;

            Console.WriteLine("Round-trip delay test ({0:n0} msgs): {1:n2} ms", 
                NUM_MESSAGES_FOR_DELAY_TEST, avgTime);
        }

        public void SpeedTest()
        {
            InternalMessage msg = BuildLoadedTestMessage();

            InternalMessage responseMsg;
            bool gotMessage = false;

            int currentDomain = 0;

            System.DateTime start = System.DateTime.Now;
            for(int i = 0; i < NUM_MESSAGES_FOR_SPEED_TEST; i++)
            {
                workers[currentDomain].PostMessage(msg);

                gotMessage = mainQueue.Receive(new TimeSpan(0, 0, 0, 10, 0), out responseMsg);

                if(gotMessage == false)
                {
                    Console.WriteLine("Did not get speed test response");
                }

                currentDomain = (currentDomain + 1 >= NUM_DOMAINS) ? 0 : currentDomain++;
            }
            System.DateTime stop = System.DateTime.Now;  
            
            TimeSpan elapsed = stop - start;
            double msgPerSec = NUM_MESSAGES_FOR_SPEED_TEST * 2 / elapsed.TotalSeconds;
            Console.WriteLine("Method call speed test ({0:n0} msgs): {1:n2} ms, {2:n2} msg/sec", 
                NUM_MESSAGES_FOR_SPEED_TEST * 2, elapsed.TotalMilliseconds, msgPerSec);

        }

        public void BinarySerializationTest()
        {
            InternalMessage msg = BuildLoadedTestMessage();

            BinaryFormatter bf = new BinaryFormatter(
                new RemotingSurrogateSelector(),
                new StreamingContext(StreamingContextStates.CrossAppDomain));

            System.DateTime start = System.DateTime.Now;
            for(int i = 0; i < NUM_MESSAGES_FOR_SPEED_TEST; i++)
            {
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, msg);
                byte[] byteData = ms.ToArray();

                byteData = null;
                ms = null;
            }
            System.DateTime stop = System.DateTime.Now;

            TimeSpan elapsed = stop - start;
            Console.WriteLine("Binary serialization test ({0:n0} msgs): {1:n2} ms", 
                NUM_MESSAGES_FOR_SPEED_TEST, elapsed.TotalMilliseconds);
        }

        public void StopAllDomainWorkers()
        {
            InternalMessage msg = new InternalMessage();
            msg.MessageId = "QUIT";
            msg.sourceQueueWriter = mainQueue.GetMessageQueueWriter();
            
            for(int i = 0; i < NUM_DOMAINS; i++)
            {
                workers[i].PostMessage(msg);
            }

            for(int i = 0; i < NUM_DOMAINS; i++)
            {
                msg = null;
                bool gotMessage = false;
                gotMessage = mainQueue.Receive(new TimeSpan(0, 0, 0, 10, 0), out msg);

                if( (gotMessage == true) &&
                    (msg.MessageId == "QUITACK"))
                {
                    workers[i].ThreadDone.WaitOne();
                    AppDomain.Unload(domains[i]);
                }
                else
                {
                    Console.WriteLine("Did not get a response from domain {0}", i);
                }
            }
        }

        private InternalMessage BuildLoadedTestMessage()
        {
            InternalMessage msg = new InternalMessage();
            msg.MessageId = "SpeedTest";
            msg.sourceQueueWriter = mainQueue.GetMessageQueueWriter();
            msg.SourceType = "Boogah";
            msg.Source = "YourMommahIsSlow";
            
            for(int j = 0; j < NUM_FIELDS_FOR_TESTS; j++)
            {
                msg.Fields.Add(j.ToString(), "value value value");
            }

            return msg;
        }

        #region Main

        [STAThread]
        static void Main(string[] args)
        {
            Tester t = new Tester();
            t.Go();
        }

        #endregion
    }
}
