using System;
using System.Collections;

namespace SynchronizedQueue
{
    class MainClass
    {
        public Queue q;
        public volatile bool done;

        [STAThread]
        static void Main(string[] args)
        {
            MainClass mc = new MainClass();

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(mc.Consumer));

            for(int i = 0; i < 10; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(mc.Producer), i.ToString());
            }

            Console.WriteLine("Running. Press enter to quit");

            Console.ReadLine();

            mc.done = true;

            System.Threading.Thread.Sleep(1000);
        }

        public MainClass()
        {
            q = new Queue();
            q = Queue.Synchronized(q);

            Console.WriteLine("Queue synchronized? {0}", q.IsSynchronized);
        }

        public void Consumer(object state)
        {
            while(this.done != true)
            {
                while(q.Count > 0)
                {
                    string s = (string)q.Dequeue();
                
                    Console.WriteLine(s + " : # q messages -> {0}", q.Count);
                }
            }
        }

        public void Producer(object state)
        {
            System.Random r = new System.Random();

            while(this.done != true)
            {
                q.Enqueue("Producer thread " + (string)state + " enqueuing a message");

                int waitTime = r.Next(0, 25);

                System.Threading.Thread.Sleep(waitTime);
            }
        }
    }
}
