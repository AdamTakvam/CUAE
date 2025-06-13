using System;

namespace DebugTrace
{
    class WorkerThread
    {
        public WorkerThread(string name)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.Go), name);
        }

        public void Go(object state)
        {
            System.Random ran = new System.Random();

            for(int i = 0; i < 250; i++)
            {
                System.Diagnostics.Trace.WriteLine((string)state + ": " + i);
                System.Threading.Thread.Sleep(ran.Next(250));
            }
        }
    }

    class Tracer
    {
        [STAThread]
        static void Main(string[] args)
        {
            int numToSpawn = 100;

            WorkerThread[] threads = new WorkerThread[numToSpawn];

            Console.WriteLine("Spawning {0} worker threads", numToSpawn);

            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(Console.Out));

            for(int i = 0; i < numToSpawn; i++)
            {
                threads[i] = new WorkerThread("WT" + i);
            }

            Console.ReadLine();
        }
    }
}
