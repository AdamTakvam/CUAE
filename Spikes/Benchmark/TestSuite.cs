using System;
using System.IO;
using System.Threading;
using System.Collections;

//using Metreos.Utilities;

namespace Metreos.Benchmark
{
    public class TestSuite
    {
        #region Main

        [STAThread]
        static void Main(string[] args)
        {
            TestSuite test = new TestSuite();
            
            Console.WriteLine("Test starting...");

            string filename;
            long ns = test.Hashtable();
            Console.WriteLine("Hashtable test: {0}ms ({1}ns)", GetMs(ns), ns);
            ns = test.Arithmetic();
            Console.WriteLine("Arithmetic test: {0}ms ({1}ns)", GetMs(ns), ns);
            ns = test.DiskWrite(out filename);
            Console.WriteLine("DiskWrite test: {0}ms ({1}ns)", GetMs(ns), ns);
            ns = test.DiskRead(filename);
            Console.WriteLine("DiskRead test: {0}ms ({1}ns)", GetMs(ns), ns);
    
            Console.WriteLine("Gathering average over 100 runs of threading test...");
            
            long totalTime = 0;
            for(int i=0; i<100; i++)
            {
                totalTime += GetMs(test.Threads());
            }

            Console.WriteLine("Threads test: {0}ms", totalTime/100);

            Console.WriteLine("Test complete");
        }

        private static long GetMs(long ns)
        {
            return ns/1000000;
        }
        #endregion

        #region Tests

        public long Hashtable()
        {
            long startTime = HPTimer.Now();

            Hashtable hash = new Hashtable();

            for(int i=0; i<100000; i++)
            {
                hash[i+"a"] = "test";
            }

            foreach(DictionaryEntry de in hash)
            {
                string val = de.Value as string;
            }

            return HPTimer.NsSince(startTime);
        }

        public long DiskWrite(out string filename)
        {
            filename = "testfile.txt";

            if(File.Exists(filename))
                File.Delete(filename);

            long startTime = HPTimer.Now();

            FileStream fStream = null;
            try { fStream = File.Open(filename, FileMode.Create, FileAccess.Write); }
            catch(Exception e)
            {
                Console.WriteLine("Error creating test file: " + e.Message);
                return 0;
            }

            byte[] loremBytes = System.Text.Encoding.Default.GetBytes(lorem);

            // Write a 100KB test file
            for(int i=0; i<1000; i++)
            {
                fStream.Write(loremBytes, 0, loremBytes.Length);
                fStream.Flush();
            }

            fStream.Close();

            return HPTimer.NsSince(startTime);
        }

        public long DiskRead(string filename)
        {
            FileInfo fInfo = new FileInfo(filename);
            if(!fInfo.Exists)
                return 0;

            if(fInfo.Length < 100000)
            {
                Console.WriteLine("Test file is too small");
                return 0;
            }

            long startTime = HPTimer.Now();
            
            FileStream fStream = null;
            try { fStream = File.Open(filename, FileMode.Open, FileAccess.Read); }
            catch(Exception e)
            {
                Console.WriteLine("Error opening test file: " + e.Message);
                return 0;
            }
            
            byte[] buffer = new byte[100000];

            for(int i=0; i<1000; i++)
            {
                fStream.Read(buffer, i * 100, 100);
            }

            fStream.Close();

            return HPTimer.NsSince(startTime);
        }

        public long Arithmetic()
        {
            long startTime = HPTimer.Now();

            for(float i=1; i<10000000; i++)
            {
                float a = i / (float)Math.Sqrt(i);
            }

            return HPTimer.NsSince(startTime);
        }

        #region Thread test

        public long Threads()
        {
            long startTime = HPTimer.Now();

            this.thread1Q = Queue.Synchronized(new Queue());
            this.thread2Q = Queue.Synchronized(new Queue());

            Thread thread1 = new Thread(new ThreadStart(Thread1));
            thread1.IsBackground = true;

            Thread thread2 = new Thread(new ThreadStart(Thread2));
            thread2.IsBackground = true;

            thread1.Start();
            thread2.Start();

            thread1.Join();

            return HPTimer.NsSince(startTime);
        }

        private void Thread1()
        {
            string msg = null;

            for(int i=0; i<10000; i++)
            {
                msg = Convert.ToString(lorem[i % lorem.Length]);
                thread2Q.Enqueue(msg);
            }
            thread2Q.Enqueue("quit");

            // Now read queue 1
            while(msg != "quit")
            {
                if(thread1Q.Count == 0)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    msg = thread1Q.Dequeue() as string;
                }
            }
        }

        private void Thread2()
        {
            string msg = null;

            while(msg != "quit")
            {
                if(thread2Q.Count == 0)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    msg = thread2Q.Dequeue() as string;
                }
            }

            // Go back the other way
            for(int i=0; i<10000; i++)
            {
                msg = Convert.ToString(lorem[i % lorem.Length]);
                thread1Q.Enqueue(msg);
            }
            thread1Q.Enqueue("quit");
        }

        private Queue thread1Q;
        private Queue thread2Q;
        #endregion

        #endregion

        // This lorem ipsum is exactly 100 bytes
        private const string lorem = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Curabitur mollis pretium orci. Fusce porta egestas nisi. Vestibulum dolor nunc, viverra a, vestibulum eu, lacinia quis, dolor. Morbi porta. Cras erat justo, blandit vel, pharetra non, porttitor vitae, pede. Suspendisse pharetra mollis mauris. Cras non neque ac arcu porttitor elementum. Fusce vel purus. Curabitur dui. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce imperdiet, elit nec sagittis consequat, tortor arcu nonummy erat, nec placerat mauris elit ut tellus. Sed metus ligula, molestie vel, nonummy quis, tincidunt vel, risus. Vivamus pharetra laoreet felis. Vivamus pede libero, aliquet molestie, laoreet sed, mattis in, tellus. Maecenas placerat risus nec lectus mattis commodo. Donec eleifend. Aenean dictum, arcu in vehicula euismod, lectus arcu tincidunt leo, quis posuere enim urna a sem. Cras cursus pretium dui. Donec quis arcu in ipsum blandit faucibus. Nullam nibh massa nunc.";
	}
}
