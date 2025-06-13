using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using Metreos.Utilities;

namespace Test
{
	/// <summary></summary>
	public class BHCC
	{
        /*
         *  -bhca:  BHCA of Reservations
            -time:  Test time (s)
            -loginDuration:  Length of time to leave user logged in (s)
            -startMAC:  MAC Address of synchronized device/profile/user
            -count:   Count of synchronized device/profile/users
            -reserveUrl:  URL to issue reserve commands to
            -releaseUrl:  URL to issue release commands to  
            
            -bhca:10000 -time:1 -loginDuration:5 -startMAC:FFFFFF000000 -count:2500 -reserveUrl:http://10.1.14.128:8000/Reserve/10.1.14.29 -releaseUrl:http://10.1.14.128:8000/Release/10.1.14.29
         */
        private int bhca;
        private Metreos.Utilities.ThreadPool pool;
        private Metreos.Utilities.WorkRequestDelegate dele;

        private static XmlSerializer releaseSeri = new XmlSerializer(typeof(ReleaseResponseType));
        private static XmlSerializer reserveSeri = new XmlSerializer(typeof(ReserveResponseType));

        private volatile int completed;
        private volatile int attempted;
        private DateTime start;
        private TimeSpan testDuration;
        private TimeSpan reserveDuration;
        private long startMac;
        private int deviceCount;
        private string reserveUrl;
        private string releaseUrl;
        private volatile int outstanding;

        public BHCC()
		{
            System.Net.ServicePointManager.DefaultConnectionLimit = Int32.MaxValue;
            this.attempted = 0;
            this.completed = 0;
            this.dele = new WorkRequestDelegate(Session);     
		}

        private void Session(object state)
        {
            outstanding++;

            // Form correct device and username
            string device = ConvertMACToSEP(startMac + (attempted % deviceCount));
            string user = ConvertMACToUser(startMac + (attempted % deviceCount));

            attempted++;

            Console.WriteLine("{0}s after start--Reserve", DateTime.Now.Subtract(start).TotalSeconds);

            // Make the reservation
            ReserveRequestType request = new ReserveRequestType();
            request.DeviceName = device;
            request.CcmUser = user;
            request.First = "Mary";
            request.Last = "Joe";

            object responseObj;
            UrlStatus status = Web.XmlTransaction(reserveUrl, request, typeof(ReserveResponseType), out responseObj);

            ReserveResponseType response = (ReserveResponseType) responseObj;
            
            bool reserveSuccess = true;
            if(status == UrlStatus.Success)
            {
                if(response != null && response.ResultCode == "0")
                {
                }
                else if(response != null && response.ResultCode == "1000" && response.DiagnosticCode == "25")
                {
                    Console.WriteLine("Device logged into already--logging out");
                }
                else
                {
                    reserveSuccess = false;
                    XmlSerializer serializer = new XmlSerializer(typeof(ReserveResponseType));
                    StringWriter writer = new System.IO.StringWriter();
                    BHCC.reserveSeri.Serialize(writer, response);
                    Console.WriteLine("Error Response:\n" + writer.ToString());
                }
            }
            else
            {
                reserveSuccess = false;
                Console.WriteLine("Unable to make a reserve request for reason {0}", status);
            }

            if(reserveSuccess)
            {
                Thread.Sleep(reserveDuration);

                ReleaseRequestType request2 = new ReleaseRequestType();
                request2.DeviceName = device;
                request2.CcmUser = user;

                object responseObj2;
                Console.WriteLine("{0}s after start--Release", DateTime.Now.Subtract(start).TotalSeconds);
                UrlStatus status2 = Web.XmlTransaction(releaseUrl, request2, typeof(ReleaseResponseType), out responseObj2);

                ReleaseResponseType response2 = responseObj2 as ReleaseResponseType;
                
                if(status2 == UrlStatus.Success)
                {
                    if(response2 != null && response2.ResultCode == "0")
                    {
                        completed++;

                        if(completed % 100 == 0)
                        {
                            Console.WriteLine("{1} attempted | {2} completed", attempted, completed);
                        }
                    }
                    else
                    {
                        StringWriter writer = new System.IO.StringWriter();
                        BHCC.releaseSeri.Serialize(writer, response2);
                        Console.WriteLine("Error Response:\n" + writer.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Unable to make a release request for reason {0}", status2);
                }
            }
            lock(this)
            {
                outstanding--;
                Monitor.Pulse(this);
            }
        }
        
        public void Start(int bhca, int testTimeSeconds, int reserveDurationSeconds, 
            long startMac, int deviceCount, string reserveUrl, string releaseUrl )
        {
            this.releaseUrl = releaseUrl;
            this.reserveUrl = reserveUrl;
            this.startMac = startMac;
            this.deviceCount = deviceCount;
            this.reserveDuration = TimeSpan.FromSeconds(reserveDurationSeconds);
            this.testDuration = TimeSpan.FromSeconds(testTimeSeconds);
            this.bhca = bhca;

			pool = new Metreos.Utilities.ThreadPool(5, 500, this.GetType().ToString());
            pool.Priority = ThreadPriority.Normal;
            pool.Start();
            Console.WriteLine("Initializing threadpool... ");
            Thread.Sleep(5000);

            start = DateTime.Now;

            while(DateTime.Now.Subtract(start) < testDuration)
            {
                pool.PostRequest(dele);       

                Thread.Sleep((3600 * 1000) / bhca);
            }

            Console.WriteLine("Waiting for all requests to finish...");
            
            lock(this)
            {
                while(outstanding != 0)
                {
                    bool exit = Monitor.Wait(this, 60000);
                    if(exit)
                    {
                        
                    }
                    else
                    {
                        Console.WriteLine("coTired of waiting...");
                        break;
                    }
                }
            }

            Console.WriteLine("Test Results");
            Console.WriteLine("------------");
            Console.WriteLine("\n Attempted: " + attempted);
            Console.WriteLine("\n Completed: " + completed);

        }
        
        private string ConvertMACToSEP(long deviceMac)
        {
            return "SEP" + deviceMac.ToString("x").PadLeft(12, '0');
        }

        private string ConvertMACToDP(long deviceMac)
        {
            return "DP" + deviceMac.ToString("x").PadLeft(12, '0');
        }

        private string ConvertMACToUser(long deviceMac)
        {
            return "U" + deviceMac.ToString("x").PadLeft(12, '0');
        }
	}
}
