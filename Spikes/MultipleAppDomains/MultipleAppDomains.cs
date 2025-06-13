using System;

//
// This spike demonstrates using application domains to segment executing logic.
// The main method creates two application domains and then creates an instance
// of MyTestObject in each domain. MyTestObject will execute and print a line
// to the console as it counts from 1 to 100. To demonstrate the segmentation
// the first domain created, "Domain A", will intentionally divide by 0 after
// the 45th iteration. It will cause an exception and exit, however, the second
// domain, "Domain B", will continue executing just fine. Furthermore, the main
// domain (created inherently when the app begins executing), is not affected
// by Domain A's error.
//
// Build the project and then execute "MultipleAppDomains.exe" inside the
// bin/Debug directory. At some point in the middle of execution an exception 
// will appear on the screen as being unhandled and Domain A will stop reporting
// in; however, Domain B will continue to execute.
//

namespace MultipleAppDomains
{
    class MyTestObject : MarshalByRefObject
    {
        public System.Threading.ManualResetEvent done;

        public MyTestObject()
        {
            done = new System.Threading.ManualResetEvent(false);
        }

        public void Go()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.Output));
        }

        public void Output(object data)
        {
            for(int i = 0; i < 100; i++)
            {
                System.Console.WriteLine("Domain {0} reporting in", System.AppDomain.CurrentDomain.FriendlyName);

                if(System.AppDomain.CurrentDomain.FriendlyName == "Domain A" && i == 45)
                {
                    int a = i / (i-45);                     // Create a Divide by 0 exception.
                }

                System.Threading.Thread.Sleep(50);
            }

            done.Set();
        }
    }

    class AppDomainMain
    {
        [STAThread]
        static void Main(string[] args)
        {
            System.AppDomain domainA = null;
            System.AppDomain domainB = null;

            try
            {
                domainA = System.AppDomain.CreateDomain("Domain A");    // Create our domains.
                domainB = System.AppDomain.CreateDomain("Domain B");

                MyTestObject testA = (MyTestObject)domainA.CreateInstanceFromAndUnwrap(
                                                        "MultipleAppDomains.exe",           // Assembly file name
                                                        "MultipleAppDomains.MyTestObject"); // Full class name

                MyTestObject testB = (MyTestObject)domainB.CreateInstanceFromAndUnwrap(
                                                        "MultipleAppDomains.exe",           // Assembly file name
                                                        "MultipleAppDomains.MyTestObject"); // Full class name

                testA.Go();                                 // Kick off Domain A. Should die half-way through.
                testB.Go();                                 // Kick off Domain B

                testB.done.WaitOne(60000, false);           // Wait for Domain B to finish up.
            }
            finally
            {
                if(domainA != null)
                {
                    System.AppDomain.Unload(domainA);       // Unload the domain
                    domainA = null;
                }

                if(domainB != null)
                {
                    System.AppDomain.Unload(domainB);       // Unload the domain
                    domainB = null;
                }
            }
        }
    }
}
