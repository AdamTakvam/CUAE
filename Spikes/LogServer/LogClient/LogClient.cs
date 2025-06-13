using System;
using System.Threading;
using System.Diagnostics; 
using Metreos.LoggingFramework;
using Metreos.LogServerClient;
using Metreos.Interfaces;

namespace LogClientTest
{
	/// <summary>
	/// A log client test program.
	/// </summary>
	class LogClientTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Logger logger = Logger.Instance;
			ushort port = IServerLog.Default_Port;
			LogClient logClient = new LogClient("LogClientTest", port, logger, TraceLevel.Info);
            
            Thread.Sleep(2000);

            LogWriter writer = new LogWriter(TraceLevel.Info, "LogClientTest");

            while(true)
            {
                Console.WriteLine("Enter command");
                string input = Console.ReadLine();

                bool done = false;
                switch(input)
                {
                    case "q":
                        done = true;
                        break;
                    
                    case "i":
                        writer.Write(TraceLevel.Info, "Log test");
                        break;

					case "h":
						for (int i=0; i<100; i++)
							writer.Write(TraceLevel.Info, "Hundred lines test - " + i);
						break;
				}

                if(done)
                {
                    break;
                }
            }

			logClient.Dispose();
            Logger.Instance.Cleanup();
		}
	}
}
