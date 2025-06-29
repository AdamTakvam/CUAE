using System;
using System.Diagnostics;

namespace Metreos.Utilities
{
    public enum PerfCounterType
    {
        CPU_Load
    }

	/// <summary>Performance Monitor wrapper</summary>
	public class PerfMonCounter : IDisposable
	{
        private PerformanceCounter counter;

		public PerfMonCounter(PerfCounterType type)
		{
            counter = CreateCounter(type);
            counter.NextValue();

            // Perf counters suck: don't mess with this.
            System.Threading.Thread.Sleep(350); 
		}

        /// <summary>Obtains a counter sample and returns the calculated value for it.</summary>
        /// <returns>The next calculated value that the system obtains for this counter.</returns>
        public float GetValue()
        {
            if(counter == null)
                throw new ObjectDisposedException(typeof(PerfMonCounter).Name);

            return counter.NextValue();
        }

        /// <summary>Computes average CPU load by dividing the process CPU time by the total run time.</summary>
        /// <returns>Average percent CPU utilization</returns>
        public static double GetAverageCPU()
        {
            TimeSpan runTime = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);
            return Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds / runTime.TotalMilliseconds;
        }

        /// <summary>Gets current memory usage (private bytes) for this process (in KB)</summary>
        /// <returns>Total KB used by this process</returns>
        public static double GetMemoryUsage()
        {
            return Process.GetCurrentProcess().PrivateMemorySize64 / 1000;
        }

        /// <summary>Obtains a counter sample and returns the calculated value for it.</summary>
        /// <param name="type">The type of counter to to request data from</param>
        /// <returns>The next calculated value that the system obtains for this counter.</returns>
        public static float GetValue(PerfCounterType type)
        {
            PerfMonCounter c = new PerfMonCounter(type);
            return c.GetValue();
        }

        protected static PerformanceCounter CreateCounter(PerfCounterType type)
        {
            switch(type)
            {
                case PerfCounterType.CPU_Load:
                    return new PerformanceCounter("Processor", "% Processor Time", "_Total");
                default:
                    return null;
            }
        }

        public void Dispose()
        {
            if(counter != null)
            {
                counter.Dispose();
                counter = null;
            }
        }
	}
}
