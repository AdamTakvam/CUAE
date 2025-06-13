using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Metreos.Core.IPC;
using System.Diagnostics;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace Testing
{
    
	class TestSelector
	{
		[STAThread]
		static void Main(string[] args)
		{
			TestSelector prog = new TestSelector();

            SelectorBase selector = new SuperSelector(
                new Metreos.Utilities.Selectors.SelectedDelegate(Selected),
                new Metreos.Utilities.Selectors.SelectedExceptionDelegate(SelectedException),
                new Metreos.Utilities.Selectors.LogDelegate(Log));
            selector.Start();

			prog.tm = new TimerManager( "tm", null, null, 5, 10 );

			prog.MakeListener( prog, selector, 6208 );

			prog.t0 = HPTimer.Now();

			IList pings = new ArrayList();
			for (int i = 0; i < 35; i++)
				pings.Add( prog.MakePing( prog, selector, 6208, 2000, 10 ) );
			
			Console.WriteLine( "{0}, created {1} pings",
				HPTimer.SecondsSince( prog.t0 ), pings.Count );

			for (IEnumerator i = pings.GetEnumerator(); i.MoveNext();)
			{
				Ping p = (Ping) i.Current;
				while (!p.Finished())
					Thread.Sleep( 100 );
			}

			Console.WriteLine( "{0} pings finished", HPTimer.SecondsSince( prog.t0 ) );

			Thread.Sleep( 15000 );

			Console.WriteLine( "sent = "+prog.sent+", received = "+prog.received );

			selector.Stop();

			prog.tm.Shutdown();

			Console.WriteLine( "Hit RETURN to quit" );
			Console.ReadLine();
		}

		public const int REPORT_COUNT = 2500;

		public TimerManager tm;

		public long t0;

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
                    key.Data);
                // (We should supposedly call EndConnect() here but it
                // apparently isn't needed.)

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

		public void MakeListener( TestSelector prog, SelectorBase selector, int port )
		{
			IPEndPoint endPoint = new IPEndPoint( IPAddress.Any, port );
			
			Socket s = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
			s.Blocking = false;
			s.Bind( endPoint );
			s.Listen( 250 );
			
			Accepter accepter = new Accepter( prog );
            selector.Register(s, this, true, false, false, false);
		}

		public Ping MakePing( TestSelector prog, SelectorBase selector, int port, int interval, int count )
		{
            int lingerSec = 2;

			IPEndPoint endPoint = new IPEndPoint( IPAddress.Loopback, port );


			SocketException excp = null;
			for (int i = 0; i < 5; i++)
			{
				try
				{
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    s.SetSocketOption(SocketOptionLevel.Socket,
                        SocketOptionName.Linger,	// Ensure all data is written after Close
                        new LingerOption(true, lingerSec));
                    s.Blocking = false;
                    s.BeginConnect(endPoint, null, null);
                    selector.Register(s, this, false, true, false, false);
                    break;
				}
				catch ( SocketException e )
				{
					excp = e;
				}
			}

			Ping p = new Ping( prog, interval, count );
			return p;
		}

		public void pingSent()
		{
			lock (this)
			{
				sent++;
				if (sent%REPORT_COUNT == 0)
					Console.WriteLine( "{0} sent = {1}", HPTimer.SecondsSince( t0 ), sent );
			}
		}

		private int sent;

		public void pingReceived()
		{
			lock (this)
			{
				received++;
				if (received%REPORT_COUNT == 0)
					Console.WriteLine( "{0} received = {1}", HPTimer.SecondsSince( t0 ), received );
			}
		}

		private int received;
	}

	class Accepter
	{
		public Accepter( TestSelector prog )
		{
			this.prog = prog;
		}

		private TestSelector prog;

        internal void Selected(Metreos.Utilities.Selectors.SelectionKey key)
        {
            key.SetWants(false, false, true, false);
            if(key.Socket != null)
            {
                key.Socket.Blocking = false;
                Reader reader = new Reader(prog);
            }
        }

        /// <summary>
        /// Logs exception from within Selector.
        /// </summary>
        /// <param name="key">Relevant key.</param>
        /// <param name="e">Exception information, e.g., text describing the exception.</param>
        public static void SelectedException(Metreos.Utilities.Selectors.SelectionKey key, Exception e)
        {
            Console.WriteLine("Con: selected exception: {0}", e);
        }
    
    }

	class Reader
	{
		public Reader( TestSelector prog )
		{
			this.prog = prog;
		}

		private TestSelector prog;

        public int KeySelected(Metreos.Utilities.Selectors.SelectionKey key)
		{
			if (key.IsSelectedForRead)
			{
				byte[] buf = new byte[256];
				int n = key.Socket.Receive( buf );
			
				if (n == 0)
				{
					key.Socket.Close();
					return 0;
				}

				for (int i = 0; i < n; i++)
				{
					int ping = buf[i];
					// Console.WriteLine( "Reader {0}: received ping {1}", key, ping );
					prog.pingReceived();
				}

                return 0;
			}

			if (key.IsSelectedForError)
			{
				key.Socket.Close();
				return 0;
			}

			Console.WriteLine( "Reader {0}: selected for what i dunno", key );
			return 0;
		}
	}

	class Ping
	{
		public Ping( TestSelector prog, int interval, int count )
		{
			this.prog = prog;
			this.interval = interval;
			this.count = count;
		}

		private TestSelector prog;

		private int interval;

		private int count;

		private SelectionKey key;

		public long Wakeup( TimerHandle th, object state )
		{
			int ping = nextPing++;

			byte[] buf = { (byte) ping };

			// Console.WriteLine( "---------------" );
			// Console.WriteLine( "Ping {0}: sending ping {1}", key, ping );

			int n = key.Socket.Send( buf );
			if (n != 1)
				return 0;
			
			prog.pingSent();

			if (Finished())
				return 0;

			return interval;
		}

		public bool Finished()
		{
			return nextPing >= count;
		}

		private int nextPing = 0;
	}
}
