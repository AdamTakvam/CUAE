using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace TestSelector
{
	class Class1
	{
		[STAThread]
		static void Main(string[] args)
		{
			//Test( 4000, 13*60*60*1000, 30000 );
			//Test2();
			Test3( 10, 300000, 100 ); // 300 second run, 100 per second.
		}

		static void Test3( int nThreads, int runTime, int interval )
		{
			int nCycles = runTime / interval;

			SelectorBase selector = new SuperSelector( null,
				new SelectedExceptionDelegate( SelectedException ) );
			
			EchoListener echoListener = new EchoListener( 0, selector, 1999 );
			echoListener.Start();

			IList ts = new ArrayList();
			for (int i = 0; i < nThreads; i++)
				ts.Add( new Thread( new ThreadStart( new Test3Runner( selector, nCycles, i, interval ).Run ) ) );

			Report( "Test3 starting..." );

			foreach (Thread t in ts)
				t.Start();

			foreach (Thread t in ts)
				t.Join();

			Report( "Test3 done" );

			Pause();
		}

		class Test3Runner
		{
			public Test3Runner( SelectorBase selector, int nCycles, int id, int interval )
			{
				this.selector = selector;
				this.nCycles = nCycles;
				this.id = id;
				this.interval = interval;
			}

			private SelectorBase selector;

			private int nCycles;

			private int id;

			private int interval;

			public void Run()
			{
				for (int i = 0; i < nCycles; i++)
				{
					if (i > 0)
						Thread.Sleep( interval );

					Socket s = SelectionKey.NewTcpSocket( false );
					SelectionKey k = selector.Register( s, null,
						new SelectedDelegate( Selected ), null,
						false, true, false, false );
					k.Connect( null, new IPEndPoint( IPAddress.Loopback, 1999 ) );
					WaitDone();
				}
				ReportStats();
			}

			public void Selected( SelectionKey key )
			{
				if (key.IsSelectedForConnect)
				{
					Report( "{0} selected for connect", id );
					key.WantsConnect = false;
					key.WantsRead = true;
					key.WantsWrite = true;
					return;
				}

				if (key.IsSelectedForWrite)
				{
					Report( "{0} selected for write", id );
					byte[] buf = new byte[1];
					key.Send( buf );
					key.Shutdown( SocketShutdown.Send );
					key.WantsWrite = false;
					return;
				}

				if (key.IsSelectedForRead)
				{
					Report( "{0} selected for read", id );
					byte[] buf = new byte[10];
					int n = key.Receive( buf );
					if (n != 0)
						return;
					
					Report( "{0} selected for read got eof", id );
					key.Close();
					SayDone( true );
					return;
				}

				if (key.IsSelectedForError)
				{
					Report( "{0} selected for error", id );
					key.Close();
					SayDone( false );
					return;
				}

				Report( "{0} selected for what i dunno {1} {2} {3} {4} {5}",
					id, key.IsSelectedForAccept, key.IsSelectedForConnect,
					key.IsSelectedForRead, key.IsSelectedForWrite, key.IsSelectedForError );
			}

			private void WaitDone()
			{
				lock (this)
				{
					while (!done)
						Monitor.Wait( this );
					done = false;
				}
			}

			private void SayDone( bool ok )
			{
				lock (this)
				{
					total++;
					if (ok)
						total_ok++;
					done = true;
					Monitor.Pulse( this );
				}
			}

			public void ReportStats()
			{
				Class1.ReportError( "total {0} total_ok {1}", total, total_ok );
			}

			private bool done;

			private int total;

			private int total_ok;
		}

		static void Test2()
		{
			SelectorBase selector = new SuperSelector( null,
				new SelectedExceptionDelegate( SelectedException ) );

			Report( "starting" );
			selector.Start();
			Report( "started" );

			Thread.Sleep( 300 );

			Report( "registering" );
			SelectionKey key = selector.Register( SelectionKey.NewTcpSocket( false ) );
			Report( "listening" );
			key.Listen( new IPEndPoint( IPAddress.Any, 1999 ), 250 );
			Report( "listened" );

			Thread.Sleep( 120000 );

			Report( "stopping" );
			selector.Stop();
			Report( "stopped" );

			Pause();
		}

		private static void Selected( SelectionKey key )
		{
			Report( "{0} selected for {1} {2} {3} {4} {5}", key, key.IsSelectedForAccept,
				key.IsSelectedForConnect, key.IsSelectedForRead,
				key.IsSelectedForWrite, key.IsSelectedForError );

			if (key.IsSelectedForAccept)
			{
				Socket s = key.Accept();
				s.Blocking = false;
				SelectionKey k = key.Selector.Register( s );
				k.WantsRead = true;
				return;
			}

			if (key.IsSelectedForRead)
			{
				byte[] buf = new byte[1024];
				int n = key.Receive( buf );
				if (n > 0)
					key.Send( buf, 0, n );
				else
					key.Close();
				return;
			}

			if (key.IsSelectedForError)
			{
				key.Close();
				return;
			}
		}

		private static void SelectedException( SelectionKey key, Exception e )
		{
			Report( "{0} caught {1}", key, e );
		}

		public static void Report( String msg )
		{
			//Console.WriteLine( "{0}: {1}", DateTime.Now.ToString(TIME_FMT), msg );
		}

		public static void Report( String fmt, params object[] args )
		{
			//Console.WriteLine( "{0}: {1}", DateTime.Now.ToString(TIME_FMT), String.Format( fmt, args ) );
		}

		private const String TIME_FMT = "HH:mm:ss.fff";

		static void Pause()
		{
			Console.WriteLine( "hit any key to quit" );
			Console.ReadLine();
		}

		static void Test( int n, int time, int interval )
		{
			const int PORT = 1999;
			int count = time/interval;
			const int BYTES_PER_PING = 12;

			Class1.ReportError( "starting {0} senders", n );

			SelectorBase selector = new SuperSelector( null, null );
			selector.Start();

			EchoListener echoListener = new EchoListener( 0, selector, PORT );
			echoListener.Start();

			Metreos.Utilities.TimerManager tm = new Metreos.Utilities.TimerManager( "alarm clock", null,
				new Metreos.Utilities.WakeupExceptionDelegate( WakeupException ), 1, 5 );
			
			long t0 = Metreos.Utilities.HPTimer.Now();
			
			EchoClient[] senders = new EchoClient[n];
			for (int i = 0; i < n; i++)
			{
				EchoClient es = new EchoClient( i, selector, PORT, count,
					interval, BYTES_PER_PING, tm );
				senders[i] = es;
				es.Start();
			}

			Class1.ReportError( "started {0} senders in {1} seconds", n, Metreos.Utilities.HPTimer.SecondsSince( t0 ) );

			Stats sendStats = new Stats();
			for (int i = 0; i < n; i++)
			{
				EchoClient es = senders[i];
				senders[i] = null;
				es.WaitDone( sendStats );
				es.Stop();
			}

			long seconds = Metreos.Utilities.HPTimer.SecondsSince( t0 );

			sendStats.Print( seconds, n );

			echoListener.Stop();

			Pause();
		}

		private static void xSelectedException( SelectionKey key, Exception e )
		{
			ReportError( "caught {0}", e );
		}

		public static void WakeupException( Metreos.Utilities.TimerHandle th, object data, Exception e )
		{
			ReportError( "caught {0}", e );
		}

		public static void ReportError( string fmt, params object[] args )
		{
			Console.WriteLine( fmt, args );
		}
	}

	abstract public class EchoBase
	{
		public EchoBase( int id, SelectorBase selector )
		{
			this.id = id;
			this.selector = selector;
		}

		protected readonly int id;
		
		protected readonly SelectorBase selector;

		abstract public void Start();

		virtual public void Stop()
		{ Done(); }

		virtual protected void SelectedForError( SelectionKey key )
		{ Done(); }

		virtual protected void SelectedForAccept( SelectionKey key )
		{ throw new NotImplementedException( "SelectedForAccept" ); }

		virtual protected void SelectedForConnect( SelectionKey key )
		{ throw new NotImplementedException( "SelectedForConnect" ); }

		virtual protected void SelectedForRead( SelectionKey key )
		{ throw new NotImplementedException( "SelectedForRead" ); }

		virtual protected void SelectedForWrite( SelectionKey key )
		{ throw new NotImplementedException( "SelectedForWrite" ); }

		protected SelectionKey xkey;

		protected void Selected( SelectionKey key )
		{
			if (key.IsSelectedForError)
			{
				SelectedForError( key );
				return;
			}

			if (key.IsSelectedForAccept)
			{
				SelectedForAccept( key );
				return;
			}

			if (key.IsSelectedForConnect)
			{
				SelectedForConnect( key );
				return;
			}

			if (key.IsSelectedForRead)
				SelectedForRead( key );

			if (key.IsSelectedForWrite)
				SelectedForWrite( key );
		}

		protected void SelectedException( SelectionKey key, Exception e )
		{
			Class1.ReportError( "{0} threw exception {1}", key, e );
		}

		virtual protected void Done()
		{
			lock (this)
			{
				if (xkey.IsOpen())
				{
					xkey.Close();
					done = true;
					Monitor.PulseAll( this );
				}
			}
		}

		public bool WaitDone( long waitStart, long maxDelaySecs, Stats stats )
		{
			lock (this)
			{
				while (!done && Metreos.Utilities.HPTimer.SecondsSince( waitStart ) < maxDelaySecs)
					Monitor.Wait( this, 50 );
			
				// TODO update stats

				return done;
			}
		}

		protected bool done;
	}

	public class EchoListener: EchoBase
	{
		public EchoListener( int id, SelectorBase selector, int port )
			: base( id, selector )
		{
			this.port = port;
		}
		
		private int port;

		override public void Start()
		{
			Socket xsocket = SelectionKey.NewTcpSocket( false );
			
			//Class1.Report( "{0} listening", this );

			xkey = selector.Register( xsocket, null, new SelectedDelegate( Selected ),
				new SelectedExceptionDelegate( SelectedException ), true,
				false, false, false );

			try
			{
				xkey.Listen( new IPEndPoint( IPAddress.Any, port ), 250 );
			}
			catch ( Exception e )
			{
				Class1.ReportError( "{0} could not listen because {1}", this, e );
				Done();
			}
		}

		override protected void Done()
		{
			//Class1.Report( "{0} stopping listening", this );
			base.Done();
		}

		override public string ToString()
		{
			return string.Format( "EchoListener {0} {1}", id, xkey );
		}

		override protected void SelectedForAccept( SelectionKey key )
		{
			Socket s;
			try
			{
				s = key.Accept();
				if (s == null || !s.Connected)
					return;
			}
			catch ( ObjectDisposedException )
			{
				// listener has been closed.
				return;
			}

			int i = nextReaderId++;

			//Class1.Report( "{0} accepted {1} -> {2}", this, s.LocalEndPoint, s.RemoteEndPoint );
			
			new EchoServer( i, selector, s ).Start();
		}

		private int nextReaderId;
	}

	public class EchoClient: EchoBase
	{
		public EchoClient( int id, SelectorBase selector, int port,
			int pingCount, int pingInterval, int bytesPerPing, Metreos.Utilities.TimerManager tm )
			: base( id, selector )
		{
			this.port = port;
			this.pingCount = pingCount;
			this.pingInterval = pingInterval;
			this.bytesPerPing = bytesPerPing;
			this.tm = tm;
		}
		
		private int port;
		
		private int pingCount;
		
		private int pingInterval;
		
		private int bytesPerPing;

		private Metreos.Utilities.TimerManager tm;

		override public void Start()
		{
			Socket xsocket = SelectionKey.NewTcpSocket( false );
			
			xkey = selector.Register( xsocket, null, new SelectedDelegate( Selected ),
				new SelectedExceptionDelegate( SelectedException ), false, true,
				false, false );

			try
			{
				xkey.Connect( null, new IPEndPoint( IPAddress.Loopback, port ) );
			}
			catch ( Exception e )
			{
				Class1.ReportError( "{0} could not connect because {1}", this, e );
				Done();
			}
		}

		override protected void Done()
		{
			//Class1.Report( "{0} done", this );
			lock (this)
			{
				base.Done ();
				Monitor.PulseAll( this );
			}
		}

		override protected void SelectedForError( SelectionKey key )
		{
			Class1.ReportError( "{0} failed", this );
			base.SelectedForError( key );
		}

		override protected void SelectedForConnect( SelectionKey key )
		{
			//Class1.Report( "{0} connected", this );
			key.WantsConnect = false;
			key.WantsRead = true;
			tm.Add( rnd.Next( pingInterval ), new Metreos.Utilities.WakeupDelegate( Wakeup ) );
		}

		override public string ToString()
		{
			return string.Format( "EchoClient {0} {1}", id, xkey );
		}

		override protected void SelectedForRead( SelectionKey key )
		{
			//Class1.Report( "{0} selected for read", this );

			bool ok = pr.Receive( key );
			if (!ok)
			{
				//Class1.Report( "{0} stats {1} {2} {3}", this, sent, received, receivedDelay/received );
				Done();
				return;
			}

			while (pr.Length >= length)
			{
				if (lengthNeeded)
				{
					length = pr.ReadInt();
					if (length > MAX_PACKET_SIZE)
						throw new IOException( "length > MAX_PACKET_SIZE: "+length );
					lengthNeeded = false;
					//Class1.Report( "{0} length = {1}", this, length );
				}
				else
				{
					PacketReader pingPr = new PacketReader( length, 0 );
					pingPr.ReceiveFrom( pr, length );
					length = INT_LENGTH;
					lengthNeeded = true;
					int pingId = pingPr.ReadInt();
					long t = pingPr.ReadLong();
					long delay = Metreos.Utilities.HPTimer.NsSince( t );
					//Class1.Report( "{0} pingId {1} delay {2}", this, pingId, delay );
					received++;
					receivedDelay += delay;
				}
			}
		}

		private const int MAX_PACKET_SIZE = 1024;

		private const int INT_LENGTH = 4;

		PacketReader pr = new PacketReader( MAX_PACKET_SIZE*4, MAX_PACKET_SIZE );

		private int length = INT_LENGTH;

		private bool lengthNeeded = true;

		private int received;

		private long receivedDelay;

		private long Wakeup( Metreos.Utilities.TimerHandle th, object data )
		{
			//Class1.Report( "{0} wakeup", this );

			if (sent < pingCount)
			{
				SendPing( xkey, sent++ );
				return pingInterval;
			}

			//Class1.Report( "{0} shutting down send", this );
			xkey.Shutdown( SocketShutdown.Send );
			return 0;
		}

		private void SendPing( SelectionKey key, int pingId )
		{
			pingPw.Reset();
			pingPw.Write( pingId );
			pingPw.Write( Metreos.Utilities.HPTimer.Now() );
			SendPacket( key, pingPw );
		}

		private PacketWriter pingPw = new PacketWriter();

		private void SendPacket( SelectionKey key, PacketWriter pkt )
		{
			pw.Write( pkt.Length );
			pkt.WriteTo( pw );
			key.WantsWrite = pw.FlushTo( key );
		}

		protected override void SelectedForWrite( SelectionKey key )
		{
			key.WantsWrite = pw.FlushTo( key );
		}

		private PacketWriter pw = new PacketWriter();

		public void WaitDone( Stats sendStats )
		{
			lock (this)
			{
				while (!done)
					Monitor.Wait( this );

				sendStats.sent += sent;
				sendStats.received += received;
				sendStats.receivedDelay += (receivedDelay / received);
			}
		}

		private int sent;

		private Random rnd = new Random();

		private byte[] readBuf = new byte[1024];
	}

	public class EchoServer: EchoBase
	{
		public EchoServer( int id, SelectorBase selector, Socket socket )
			: base( id, selector )
		{
			this.socket = socket;
		}

		private Socket socket;

		override public string ToString()
		{
			return string.Format( "EchoServer {0} {1}", id, xkey );
		}

		override public void Start()
		{
			Metreos.Utilities.Assertion.Check( socket != null, "socket != null" );
			socket.Blocking = false;
			try
			{
				xkey = selector.Register( socket, null, new SelectedDelegate( Selected ),
					new SelectedExceptionDelegate( SelectedException ), false, false,
					false, false );
			}
			catch ( Exception e )
			{
				Class1.ReportError( "caught {0}", e );
				throw e;
			}
			//Class1.ReportError( "xkey = {0}", xkey );
			Metreos.Utilities.Assertion.Check( xkey != null, "xkey != null" );
			xkey.WantsRead = true;
			socket = null;
		}

		override protected void Done()
		{
			//Class1.Report( "{0} done", this );
			base.Done ();
		}

		private byte[] AllocBuf()
		{
			return new byte[100];
		}

		private void FreeBuf( byte[] buf )
		{
			// TODO do nothing for now.
		}

		override protected void SelectedForRead( SelectionKey unused )
		{
			//Class1.Report( "{0} selected for read", this );

			byte[] buf = AllocBuf();
			if (buf == null)
			{
				xkey.WantsRead = false;
				return;
			}

			int k;
			try
			{
				k = xkey.Receive( buf );
			}
			catch ( SocketException )
			{
				k = 0;
			}

			//Class1.Report( "{0} receive k = {1}", this, k );
			
			if (k == 0)
			{
				FreeBuf( buf );
				xkey.WantsRead = false;
				DoneIfQueueEmpty( true );
				return;
			}

			if (DoWrite( buf, 0, k, true ) >= MAX_WRITE_QUEUE_LEN)
			{
				xkey.WantsRead = false;
				return;
			}
		}

		private bool readShutdown;

		private const int MAX_WRITE_QUEUE_LEN = 6;

		public void Write( byte[] buf )
		{
			Write( buf, 0, buf.Length );
		}

		public void Write( byte[] buf, int index, int length )
		{
			DoWrite( buf, index, length, false );
		}

		private void DoneIfQueueEmpty( bool input )
		{
			lock (queue)
			{
				if (input)
				{
					// we're being called from SelectForRead
					readShutdown = true;
					//Class1.Report( "{0} shutting down receive", this );
					xkey.Shutdown( SocketShutdown.Receive );
				}

				// we're being called from SelectedForWrite
				if (readShutdown && queue.Count == 0)
				{
					//Class1.Report( "{0} shutting down send", this );
					xkey.Shutdown( SocketShutdown.Send );
					Done();
				}
			}
		}

		private int DoWrite( byte[] buf, int index, int length, bool saveBuf )
		{
			lock (queue)
			{
				if (queue.Count > 0)
				{
					queue.Enqueue( new WriteItem( buf, index, length, saveBuf ) );
					xkey.WantsWrite = true;
				}
				else // write queue is empty, try writing directly.
				{
					int k;
					try
					{
						k = xkey.Send( buf, index, length );
					}
					catch ( SocketException e )
					{
						if (e.ErrorCode != 10035)
							throw e;
						//Class1.Report( "{0} socket exception error code = {1}", this, e.ErrorCode );
						k = 0;
					}

					//Class1.Report( "{0} send k = {1}", this, k );
					if (k < length)
					{
						queue.Enqueue( new WriteItem( buf, index+k, length-k, saveBuf ) );
						xkey.WantsWrite = true;
					}
					else // k == length
					{
						if (saveBuf)
							FreeBuf( buf );
					}
				}
				return queue.Count;
			}
		}

		override protected void SelectedForWrite( SelectionKey unused )
		{
			Metreos.Utilities.Assertion.Check( unused == this.xkey, "unused == this.key" );
			Class1.Report( "{0} selected for write", this );
			lock (queue)
			{
				while (queue.Count > 0)
				{
					WriteItem item = (WriteItem) queue.Peek();
					int k = xkey.Send( item.buf, item.index, item.length );
					//Class1.Report( "{0} send k = {1}", this, k );
					if (k == 0)
						break;

					if (k < item.length)
					{
						// we didn't write all of this item.
						item.index += k;
						item.length -= k;
						break;
					}

					// we finished this item, nuke it.
					queue.Dequeue();
					if (item.saveBuf)
						FreeBuf( item.buf );
				}

				if (queue.Count < MAX_WRITE_QUEUE_LEN && !readShutdown)
					xkey.WantsRead = true;
				
				DoneIfQueueEmpty( false );
				if (queue.Count == 0)
					xkey.WantsWrite = false;
			}
		}

		private Queue queue = Queue.Synchronized( new Queue() );
	}

	public class WriteItem
	{
		public WriteItem( byte[] buf, int index, int length, bool saveBuf )
		{
			this.buf = buf;
			this.index = index;
			this.length = length;
			this.saveBuf = saveBuf;
		}

		public byte[] buf;
		
		public int index;
		
		public int length;

		public bool saveBuf;
	}

	public class Stats
	{
		public int sent;

		public int received;

		public long receivedDelay;

		public void Print( long seconds, int senders )
		{
			double rate = ((double) sent)/seconds;
			Class1.ReportError( "Stats: sent {0} received {1} in {2} seconds ({3} per second) delay {4}",
				sent, received, seconds, rate, receivedDelay/senders );
		}
	}

	public class PacketWriter
	{
		public PacketWriter()
		{
			// nothing to do.
		}

		public int Length { get { return length - offset; } }

		private int offset;

		private int length;

		public void Reset()
		{
			offset = 0;
			length = 0;
		}

		public void Write( int value )
		{
			Write( BitConverter.GetBytes( value ) );
		}

		public void Write( long value )
		{
			Write( BitConverter.GetBytes( value ) );
		}

		public void Write( byte[] value )
		{
			Write( value, 0, value.Length );
		}

		public void Write( byte[] value, int off, int len )
		{
			EnsureSpace( len );
			Array.Copy( value, off, buf, length, len );
			length += len;
		}

		public byte[] ToArray()
		{
			int usedLength = length - offset;
			byte[] b = new byte[usedLength];
			Array.Copy( buf, offset, b, 0, usedLength );
			return b;
		}

		public void WriteTo( PacketWriter other )
		{
			int usedLength = length - offset;
			other.Write( buf, offset, usedLength );
		}

		public bool FlushTo( SelectionKey key )
		{
			int usedLength = length - offset;
			if (usedLength > 0)
			{
				int k = key.Send( buf, offset, usedLength );
				if (k > 0)
					offset += k;
				return length > offset;
			}
			return false;
		}

		private void EnsureSpace( int len )
		{
			if (buf != null)
			{
				// three cases:
				// 1. there is space between logical and and physical end

				if (length + len <= buf.Length)
					return;

				// 2. there is space in the buffer, but not at the logical end

				int usedLength = length - offset;
				if (usedLength + len <= buf.Length)
				{
					CompactBuffer();
					return;
				}

				// 3. there is not enough space in the buffer.

				ReallocBuffer( usedLength + len );
			}
			else
			{
				ReallocBuffer( len );
			}
		}

		private void CompactBuffer()
		{
			int usedLength = length - offset;
			Array.Copy( buf, offset, buf, 0, usedLength );
			offset = 0;
			length = usedLength;
		}

		private void ReallocBuffer( int neededLength )
		{
			// calculate a power of two > neededLength
			int k = buf != null ? buf.Length : DEFAULT_BUFFER_SIZE;
			while (k <= neededLength)
				k *= 2;

			// alloc the new buffer
			byte[] b = new byte[k];
			if (buf != null)
			{
				int usedLength = length - offset;
				Array.Copy( buf, offset, b, 0, usedLength );
				offset = 0;
				length = usedLength;
			}
			buf = b;
		}

		private byte[] buf;

		private const int DEFAULT_BUFFER_SIZE = 32;
	}

	public class PacketReader
	{
		public PacketReader( int bufLen, int threshold )
		{
			buf = new byte[bufLen];
			this.threshold = threshold;
		}

		private byte[] buf;

		private readonly int threshold;

		private int offset;

		private int length;

		public int Length { get { return length - offset; } }

		public bool Receive( SelectionKey key )
		{
			// two cases:
			// 1. the available space in the buffer is < threshold

			if (buf.Length - length < threshold)
			{
				int usedLength = length - offset;
				Array.Copy( buf, offset, buf, 0, usedLength );
				offset = 0;
				length = usedLength;
			}
			
			// 2. the available space in the buffer is >= threshold

			int k = key.Receive( buf, length, buf.Length - length );

			if (k == 0)
				return false;
			
			length += k;
			return true;
		}

		public void ReceiveFrom( PacketReader other, int len )
		{
			other.ReadBytes( buf, length, len );
			length += len;
		}

		public int ReadInt()
		{
			EnsureSpace( SIZEOF_INT32 );
			int value = BitConverter.ToInt32( buf, offset );
			offset += SIZEOF_INT32;
			return value;
		}

		private const int SIZEOF_INT32 = 4;

		public long ReadLong()
		{
			EnsureSpace( SIZEOF_INT64 );
			long value = BitConverter.ToInt64( buf, offset );
			offset += SIZEOF_INT64;
			return value;
		}

		private const int SIZEOF_INT64 = 8;

		public void ReadBytes( byte[] b, int off, int len )
		{
			EnsureSpace( len );
			Array.Copy( buf, offset, b, off, len );
			offset += len;
		}

		private void EnsureSpace( int len )
		{
			if (length - offset < len)
				throw new IOException( "length - offset < len" );
		}
	}
}
