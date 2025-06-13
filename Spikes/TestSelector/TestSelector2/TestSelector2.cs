using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace Testing
{
    public class TestSelector2
    {
        public TimerManager tm;

        public long t0;
        static SelectorBase selector;
        static ArrayList sockets = new ArrayList();
        static void ThreadProc(Object state)
        {
            int lingerSec = 2;

            IPAddress addr = IPAddress.Parse("10.89.31.46");
            IPEndPoint endPoint = new IPEndPoint(addr, 2000);

            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.Linger,	// Ensure all data is written after Close
                    new LingerOption(true, lingerSec));
                s.Blocking = false;
                s.BeginConnect(endPoint, null, null);
                selector.Register(s, s, false, true, false, false);
                lock(sockets.SyncRoot)
                {
                    sockets.Add(s);
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("SocketException on initial connect: {0}", e);
            }

        }

        static void Main(string[] args)
        {
            TestSelector2 prog = new TestSelector2();

            selector = new SuperSelector(
                new Metreos.Utilities.Selectors.SelectedDelegate(Selected),
                new Metreos.Utilities.Selectors.SelectedExceptionDelegate(SelectedException),
                new Metreos.Utilities.Selectors.LogDelegate(Log));
            selector.Start();

            prog.tm = new TimerManager("tm", null, null, 5, 10);

            prog.t0 = HPTimer.Now();

            for(int i = 0; i < 10; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc));
            }

            Thread.Sleep(5000);

            selector.Stop();

            prog.tm.Shutdown();

            //close sockets
            lock(sockets.SyncRoot)
            {
                foreach(Socket s in sockets)
                {
                    s.Close();
                }
            }
            Console.WriteLine("Hit RETURN to quit");
            Console.ReadLine();
        }

        static void Selected(Metreos.Utilities.Selectors.SelectionKey key)
        {
            if(key.IsSelectedForAccept)
            {
                Console.WriteLine("accept selection");
                key.Accept();
                key.WantsAccept = false;
            }

            if(key.IsSelectedForRead)
            {
                Console.WriteLine("read selection");
                key.WantsRead = false;
            }

            if(key.IsSelectedForWrite)
            {
                Console.WriteLine("write selection");
                key.WantsWrite = false;
            }

            if(key.IsSelectedForConnect)
            {
                Console.WriteLine("Con: {0}: connected",
                    key.Socket.Handle);
                // (We should supposedly call EndConnect() here but it
                // apparently isn't needed.)
                //key.Socket.EndConnect(new IAsyncResult);
                key.WantsConnect = false;
            }

            if(key.IsSelectedForError)
            {
                Console.WriteLine("Error in Selected callback: {0}", key);
            }

        }

        public static void SelectedException(Metreos.Utilities.Selectors.SelectionKey key, Exception e)
        {
            Console.WriteLine("Con: selected exception: {0}", e);
        }

        private static void Log(TraceLevel level, string message, Exception e)
        {
            Console.WriteLine(message + (e != null ? ": " + e.Message : ""));
        }
    }
}
