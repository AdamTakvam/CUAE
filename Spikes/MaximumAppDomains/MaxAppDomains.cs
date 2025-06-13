using System;

namespace Metreos.MaximumAppDomains
{
    class MaxAppDomains
    {
        [STAThread]
        static void Main(string[] args)
        {
            int numDomainsToCreate = 1000;

            if(args.Length > 0)
            {
                numDomainsToCreate = Convert.ToInt32(args[0]);

                System.Diagnostics.Debug.Assert(numDomainsToCreate >= 0);
                System.Diagnostics.Debug.Assert(numDomainsToCreate <= 1000000);
            }
            
            Console.WriteLine("Creating {0} individual application domains", numDomainsToCreate);

            System.AppDomain[] domains = new System.AppDomain[numDomainsToCreate];

            for(int i = 0; i < numDomainsToCreate; i++)
            {
                domains[i] = AppDomain.CreateDomain("New domain " + i);
            }

            Console.WriteLine("Done creating application domains.");
            Console.WriteLine("Unloading domains:");

            for(int i = 0; i < numDomainsToCreate; i++)
            {
                if((i % 10) == 0)
                {
                    Console.Write("{0} ", i);
                }

                //System.Diagnostics.Debug.Assert(domains[i] != null);

                AppDomain.Unload(domains[i]);
            }
        }
    }
}
