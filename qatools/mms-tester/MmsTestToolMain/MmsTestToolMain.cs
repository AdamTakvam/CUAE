using System;
using System.Windows.Forms;
using System.Threading;

using Metreos.MmsTester.AdapterFramework;
using Metreos.MmsTester.AdapterManager;
using Metreos.MmsTester.ClientFramework;
using Metreos.MmsTester.ClientManager;
using Metreos.MmsTester.Custom.Clients;
using Metreos.MmsTester.Conduit;
using Metreos.MmsTester.Core;
using Metreos.MmsTester.Interfaces;
using Metreos.MmsTester.Custom.Adapters;
using Metreos.MmsTester.VisualInterfaceFramework;
using Metreos.MmsTester.VisualInterfaceManager;
//using Metreos.MmsTester.Custom.VisualInterfaces;


namespace Metreos.MmsTester.MmsTestToolMain
{
	/// <summary>
	/// Main entry point of the Metreos Media Server Testing Tool
	/// </summary>
	public class MmsTestToolMain
	{
        Thread visualTestThread;
        AutoResetEvent are;

		public MmsTestToolMain()
		{
            // This thread will allow the visual tool to run happily, without hanging like a silly boy
			visualTestThread = new Thread(new ThreadStart( StartVisualInterface ));
            
            Go();
		}

        public void Go()
        {
            // Load the conduit
            Conduit.Conduit conduit = new Conduit.Conduit();

            // Load the system resource pool
            ResourceProvider resourceProvider = new ResourceProvider(conduit);

            // Load all known adapters
            AdapterProvider adapterProvider = new AdapterProvider();
      
            AdapterManager.AdapterManager adapterManager = new AdapterManager.AdapterManager(adapterProvider, conduit);
            are = new AutoResetEvent(false);

            // Load all known clients
            ClientProvider clientProvider = new ClientProvider();

            // Load the client router, and give it access to all client assemblies
            ClientRouter clientManager = new ClientRouter(adapterManager, clientProvider, conduit);



            // Load all visual interfaces
            VisualInterfaceProvider visualInterfaceProvider = new VisualInterfaceProvider();  

            // Load the visual interface provider, and give it acces to all visual interface assemblies
            VisualInterfaceShell visualInterfaceShell = new VisualInterfaceShell(visualInterfaceProvider, conduit, ref are);

            Application.Run(visualInterfaceShell);

            // Start Visual Interfaces
            if(visualInterfaceShell.StartVisualInterfaces())
            {

            }
            else
            {
                Console.WriteLine("Failed to start visual interfaces");
            }

            // The tool can usually only be stopped by a visual interface shutdown event
            // Block until the end of the test
 
            are.WaitOne();
                      
        }

        public void StartVisualInterface()
        {  
            
        }

        [STAThread]
        static void Main(string[] args)
        {
            MmsTestToolMain mttm = new MmsTestToolMain();

            mttm = null;
        }
	}
}
