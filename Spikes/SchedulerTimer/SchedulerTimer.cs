using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace Metreos.SchedulerTimer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		[STAThread]
		static void Main(string[] args)
		{
            Thread.Sleep(1); // Make sure we start executing at the beginning of an interrupt

            long startTime = HPTimer.Now();
            Thread.Sleep(1);
			Console.WriteLine("1ms sleep with default scheduler resolution: " + HPTimer.MillisSince(startTime));

            // Adjust scheduler frequency
            timeBeginPeriod(1);

            Thread.Sleep(1);  // Timer change doesn't take effect until next interrupt cycle

            startTime = HPTimer.Now();
            Thread.Sleep(1);
            Console.WriteLine("1ms sleep with fine scheduler resolution: " + HPTimer.MillisSince(startTime));

            Console.WriteLine();
            Console.WriteLine("Scheduler timer will remain adjusted until you press <enter>");
            Console.ReadLine();

            // Reset scheduler frequency
            timeEndPeriod(1);
		}

        [DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(uint period);

        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(uint period);
	}
}
