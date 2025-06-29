using System;
using System.Diagnostics;
using System.ComponentModel;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Creates a CallGen process
	/// </summary>
	public class CallGen
	{
        
        public const string FILE_NAME = "callgen323.exe";
        public const string PATH_TO_CALL_GENERATOR = "..//Tests//";

//	    private bool passive;
        private bool closeWindowOnExit;
	    private int numSignals;
        private int numPort;
        private int minSignalInterval;
        private int maxSignalInterval;
        private int minWaitTime;
        private int maxWaitTime;
        private int maxWaitEstablish;
        private Process callgen;
        public bool exited;

        public CallGen(int numSignals, int numMaxSimultaneousCalls, int numPort, int minSignalInterval, int maxSignalInterval, int minWaitTime, int maxWaitTime, int maxWaitEstablish, bool closeWindowOnExit)
		{
            callgen = new Process();

            //this.passive = passive;
            this.numSignals = numSignals;
            this.numPort = numPort;
            this.minSignalInterval = minSignalInterval;
            this.maxSignalInterval = maxSignalInterval;
            this.minWaitTime = minWaitTime;
            this.maxWaitTime = maxWaitTime;
            this.maxWaitEstablish = maxWaitEstablish;
            this.closeWindowOnExit = closeWindowOnExit;
            this.exited = false;

            // Setup the StartInfo class for the message generator
            // REFACTOR:  Implement events (to catch process exit signal) + redirect output to a file on exit
            
            callgen.StartInfo.WorkingDirectory = PATH_TO_CALL_GENERATOR;
            callgen.StartInfo.FileName = FILE_NAME;
            callgen.StartInfo.Arguments = "-n -m " + numMaxSimultaneousCalls + " -r " + numSignals + " -i *:" + numPort + " --tmincall " + minSignalInterval + " --tmaxcall " + maxSignalInterval + " --tminwait " + minWaitTime + " --tmaxwait " + maxWaitTime + " --tmaxest " + maxWaitEstablish + " localhost";
            callgen.EnableRaisingEvents = true;
            callgen.Exited += new EventHandler(this.OnExit);

		}

        public CallGen(int numPort, bool windowVisible)
        {
            callgen = new Process();

            this.numPort = numPort;
            //this.closeWindowOnExit = closeWindowOnExit;
            this.exited = false;

            // Setup the StartInfo class for the listener
            // REFACTOR:  Implement events (to catch process exit signal) + redirect output to a file on exit
            callgen.StartInfo.WorkingDirectory = PATH_TO_CALL_GENERATOR;
            callgen.StartInfo.FileName = FILE_NAME;
            callgen.StartInfo.Arguments = "-l -n -i *:" + numPort;
            callgen.EnableRaisingEvents = true;
            callgen.Exited += new EventHandler(this.OnExit);
        }

        public bool StartProcess()
        {
            try
            {
                callgen.Start();
            }
            catch(InvalidOperationException ioe)
            {
                Console.WriteLine("Error opening callgen323. Error message is as follows:\n" + ioe.Message);
                return false;
            }
            catch(Win32Exception w32e)
            {
                Console.WriteLine("Unable to start callgen323.  This application can be found at www.openh323.org.");
                Console.WriteLine("Place it in samoa/external-libs/.");
                Console.WriteLine("Recompile the Samoa solution, which will then copy over callgen323 into bin/Tests/");
                Console.WriteLine("CallGen323 must have access to these 3 libraries, also found at openh323.org:");
                Console.WriteLine("OpenH323.dll, PTLib.dll, and PWLib.dll");
                Console.WriteLine("Error message is as follows:\n" + w32e.Message);
                return false;
            }

            return true;
        }
        public bool KillProcess()
        {
            if(callgen.HasExited != true)
            {
                try
                {
                    callgen.Kill();
                }
                catch(Win32Exception w32e)
                {
                    Console.WriteLine(w32e.Message);
                    return false;
                }
                catch(InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                    return false;
                }
                catch(SystemException se)
                {
                    Console.WriteLine(se.Message);
                    return false;
                }
                
            }   

            return true;
        }
    
        public void OnExit(object sender, EventArgs e)
        {
            this.exited = true;
        }

        public void Cleanup()
        {
            if(callgen != null)
            {
                try
                {
                    if(closeWindowOnExit == true)
                    {
                        callgen.CloseMainWindow();
                    }
                    try
                    {

                        callgen.Close(); 
                        callgen.Dispose();
                    }
                    catch(Exception){}
                }
                catch(Exception){}
            }
        }
	}
}
