using System;
using System.Diagnostics;

namespace PerfCounter
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Counter
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            if(PerformanceCounterCategory.Exists("Metreos Samoa 1"))
            {
                PerformanceCounterCategory.Delete("Metreos Samoa 1");
            }
            
            System.Diagnostics.CounterCreationData p1 = new CounterCreationData();
            p1.CounterName = "# Running Apps";
            p1.CounterType = PerformanceCounterType.NumberOfItems64;

            CounterCreationDataCollection ccdc = new CounterCreationDataCollection();
            ccdc.Add(p1);
            
            PerformanceCounterCategory.Create("Metreos Samoa 1", "asdf", ccdc);

            if(PerformanceCounterCategory.CounterExists("# Running Apps", "Metreos Samoa 1"))
            {
                Console.WriteLine("Counter created successfully.");
            }
            else
            {
                Console.WriteLine("Counter could not be created");
                return;
            }

            PerformanceCounter pc = new PerformanceCounter();

            pc.ReadOnly = false;
            pc.CounterName = "# Running Apps";
            pc.CategoryName = "Metreos Samoa 1";

            Console.WriteLine("Keep pressing \"return\" to increment counter");

            pc.InstanceName = "a";
            pc.RawValue = 0;

            while(Console.ReadLine() == "")
            {
                Console.Write("Counter value: " + pc.Increment().ToString());
            }

            
		}
	}
}
