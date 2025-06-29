using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

using Metreos.Core.Sockets;
using Metreos.Utilities;

// Inter-Process Communication
namespace Metreos.Core.IPC
{
    /// <summary>
    /// Delegate for callback into consumer when an Open* or Reopen* has completed.
    /// </summary>
    public delegate void OnConnectDelegate( IpcClient ipcClient, bool reconnect );

	/// <summary>
	/// Delegate for callback into consumer when a packet has been received.
	/// </summary>
	public delegate void OnPacketReceivedDelegate( IpcClient ipcClient, Packet p );

	/// <summary>
	/// Delegate for callback into consumer when the session has been closed. If the
	/// close was requested via the Close method, the Exception passed will be null.
	/// If the close was requested because of an I/O exception or other problem reading
	/// from the network stream, then the Exception passed will reflect that problem.
	/// </summary>
	public delegate void OnCloseDelegate( IpcClient ipcClient, Exception e );

    /// <summary>
    /// Simple asynchronous IPC client to handle communication with an IPC
    /// server.
    /// </summary>
	public class IpcClient : IpcConnection, IDisposable
	{
		/// <summary>
		/// Constructs the ipc client with null remote end point, default
		/// write queue length, and default delay.
		/// </summary>
		public IpcClient()
			: this( null, DEFAULT_WRITE_QUEUE_LENGTH, DEFAULT_DELAY )
		{
			// nothing else to do.
		}

		/// <summary>
		/// Constructs the ipc client with default write queue length and default delay.
		/// </summary>
		/// <param name="remoteEp">The remote endpoint to use. May be specified as
		/// null and set later using assignment to RemoteEp.</param>
		public IpcClient( IPEndPoint remoteEp )
			: this( remoteEp, DEFAULT_WRITE_QUEUE_LENGTH, DEFAULT_DELAY )
		{
			// nothing else to do.
		}

		/// <summary>
		/// Constructs the ipc client.
		/// </summary>
		/// <param name="remoteEp">The remote endpoint to use. May be specified as
		/// null and set later using assignment to RemoteEp.</param>
		/// <param name="writeQueueLength">The length of the write queue. Specify
		/// 0 to disable queuing, or Int32.MaxValue to allow an infinite queue length.</param>
		/// <param name="delay">the delay in milliseconds between attempts to connect.
		/// specify 0 to not delay any more than necessary between attempts.</param>
		public IpcClient( IPEndPoint remoteEp, int writeQueueLength, int delay )
		{
			if (writeQueueLength < 0)
				throw new ArgumentException( "writeQueueLength < 0" );

			if (delay < 0)
				throw new ArgumentException( "delay < 0" );

			this.remoteEp = remoteEp;
			this.writeQueueLength = writeQueueLength;
			this.delay = delay;
		}

		private IPEndPoint remoteEp;
		
		private readonly int writeQueueLength;

		private readonly int delay;

		/// <summary>
		/// The default length of the write queue. Specify 0 to disable write queuing.
		/// </summary>
		public const int DEFAULT_WRITE_QUEUE_LENGTH = 0;

		/// <summary>
		/// The default delay between atttempts to connect (in ms).
		/// </summary>
		public const int DEFAULT_DELAY = 5000;

		#region Notification
		
		/// <summary>
		/// Delegate to inform when a connection is established.
		/// </summary>
		public OnConnectDelegate onConnect;
		
		/// <summary>
		/// Delegate to inform when a packet is received.
		/// </summary>
		public OnPacketReceivedDelegate onPacketReceived;
		
		/// <summary>
		/// Delegate to inform when a connection is closed.
		/// </summary>
		public OnCloseDelegate onClose;

		#endregion
		#region Properties

		public IPEndPoint RemoteEp
		{
			get { return remoteEp; }
			set { remoteEp = value; }
		}

		public override String ToString()
		{
			return "IpcClient("+remoteEp+", "+state+")";
		}

		#endregion
		#region Asynchronous Session Management

		public void Start()
		{
			lock (this)
			{
				PrepareToOpen();
				thread = StartThread(new ThreadStart(Run), "Run");
			}
		}

		private void Run()
		{
			while (state != State.CLOSED)
			{
				OpenConnectionX();
                try
                {
                    RunConnection();
                    CloseConnection( null );
                }
                catch(ThreadAbortException)
                {  // nothing
                }
                catch(Exception e)
                {
                    CloseConnection( e );
                }
			}
		}

		private void OpenConnectionX()
		{
			while (state == State.OPENING)
			{
				lock (this)
				{
					try
					{
						OpenConnection();
						return;
					}
					catch ( SocketException )
					{
						if (state == State.OPENING)
							Monitor.Wait( this, delay );
					}
				}
			}
		}

		public void Close()
		{
			Close( 0 );
		}

		public void Close( int timeout )
		{
			if (timeout < 0)
				throw new ArgumentException( "timeout < 0" );
			
			lock (this)
			{
				if (state != State.CLOSED)
				{
					state = State.CLOSED;

					//Console.WriteLine( "calling CloseConnection" );
					CloseConnection( new Exception( "closed" ) );
					//Console.WriteLine( "CloseConnection done" );
				}
			}

			//Console.WriteLine( "calling StopThread" );
            if(thread != Thread.CurrentThread)
            {
                StopThread( thread, timeout );
                thread = null;
            }
			//Console.WriteLine( "StopThread done" );
		}

		#endregion
		#region Synchronous Session Management

		public bool Open()
		{
			lock (this)
			{
				try
				{
					PrepareToOpen();
					OpenConnection();
					thread = StartThread(new ThreadStart(SyncReader), "SyncReader");
					return true;
				}
				catch ( SocketException e )
				{
					state = State.CLOSED;
					CloseConnection( e );
					return false;
				}
			}
		}

		private void SyncReader()
		{
			try
			{
				RunConnection();
				FinishSyncReader( null );
			}
			catch ( Exception e )
			{
				FinishSyncReader( e );
			}
		}

		private void FinishSyncReader( Exception e )
		{
			lock (this)
			{
				if (state != State.CLOSED)
				{
					state = State.CLOSED;
					CloseConnection( e );
					thread = null;
				}
			}
		}

		#endregion
		#region Thread Management

		private Thread StartThread( ThreadStart start, string what )
		{
			Thread t = new Thread( start );
			t.IsBackground = true;
			t.Name = ToString()+" "+what;
			t.Start();
			return t;
		}

		private void StopThread( Thread t, int timeout )
		{
			if (t != null)
			{
                if(!t.Join(timeout))
                    t.Abort();
			}
		}

		private Thread thread;

		#endregion
		#region State Management

		enum State { CLOSED, OPENING, OPENED };

		private State state
		{
			get
			{
				return _state;
			}

			set
			{
				lock (this)
				{
					_state = value;
					Monitor.PulseAll( this );
				}
			}
		}

		private State _state;

		#endregion
		#region Connection Management

		private void PrepareToOpen()
		{
			if (state != State.CLOSED)
				throw new InvalidOperationException( "already open" );

			if (remoteEp == null)
				throw new InvalidOperationException( "remoteEp == null" );

			if (onPacketReceived == null)
				throw new InvalidOperationException( "onPacketReceived == null" );

			state = State.OPENING;
		}

		private void OpenConnection()
		{
			lock (this)
			{
				if (state != State.OPENING)
					throw new Exception( "state not opening - "+state );

				tcpClient = new TcpClient();
				try
				{
					tcpClient.Connect( remoteEp );
					tcpClient.NoDelay = true; // disable nagle's algorithm

					NetworkStream networkStream = tcpClient.GetStream();
					inputStream = new BufferedStream( networkStream );
					outputStream = new BufferedStream( networkStream );

					SetupWriteQueuing();

					state = State.OPENED;

					if (onConnect != null)
						onConnect( this, reconnect );

					reconnect = true; // show that we've connected at least once
				}
				catch ( Exception e )
				{
					CloseConnection( e );
					throw e;
				}
			}
		}

		private TcpClient tcpClient;
		
		private Stream inputStream;

		private Stream outputStream;

		private bool reconnect;

		private void RunConnection()
		{
			Stream xis = inputStream;
			if (xis == null)
				return;

			while (state == State.OPENED)
			{
				Packet p = ReadPacket( xis );
				if (onPacketReceived != null)
					onPacketReceived( this, p );
			}
		}

		private void CloseConnection( Exception e )
		{
			lock (this)
			{
				if (tcpClient != null)
				{
					StopWriteQueuing( 100 ); // wait 100ms for it to finish

					if (inputStream != null)
					{
                        try { inputStream.Close(); }
                        catch { /* ignore */ }

						inputStream = null;
					}

					if (outputStream != null)
					{
                        try { outputStream.Close(); }
                        catch { /* ignore */ }

						outputStream = null;
					}

                    try { tcpClient.Close(); }
                    catch { /* ignore */ }

					tcpClient = null;

					if (state == State.OPENED)
						state = State.OPENING;

					if (onClose != null)
						onClose( this, e );
				}
			}
		}

		/// <summary>
		/// Permanently disposes of this object.
		/// </summary>
		public void Dispose()
		{
			Close( 1 ); // close and don't wait!
		}

		#endregion

		#region Outgoing data

		private void SetupWriteQueuing()
		{
			if (writeQueueLength > 0)
			{
				queue = new Queue();
				writeThread = StartThread(new ThreadStart(ProcessWriteQueue), "Writer");
			}
		}

		private void ProcessWriteQueue()
		{
			Stream os = outputStream;
			Queue q = queue;
			if (q == null || os == null)
				return;
			
			while (q == queue && os == outputStream)
			{
				Packet p = DequeuePacket( q );
				if (p != null)
					WritePacket( os, p );
			}
		}

		private Packet DequeuePacket( Queue q )
		{
			lock (q)
			{
				int count = 0;
				while (q == queue && (count = q.Count) == 0)
					Monitor.Wait( q );

				if (q != queue)
					return null;

				Assertion.Check( count > 0, "count > 0" );
				Packet p = (Packet) q.Dequeue();

				if (count == writeQueueLength)
					Monitor.Pulse( q ); // wakeup EnqueuePacket waiting on a full queue

				return p;
			}
		}

		private void EnqueuePacket( Packet p )
		{
			Queue q = queue;
			if (q == null)
				throw new IOException( "closed" );
			
			lock (q)
			{
				int count = 0;
				while (q == queue && (count = q.Count) >= writeQueueLength)
					Monitor.Wait( q );

				if (q != queue)
					throw new IOException( "closed" );

				Assertion.Check( count < writeQueueLength, "count < writeQueueLength" );
				q.Enqueue( p );

				if (count == 0)
					Monitor.Pulse( q ); // wakeup DequeuePacket waiting on empty queue
			}
		}

		private void StopWriteQueuing( int timeout )
		{
			Queue q = queue;
			if (q != null)
			{
				queue = null;

				lock (q)
				{
					q.Clear();
					Monitor.PulseAll( q );
				}

				StopThread( writeThread, timeout );
				writeThread = null;
			}
		}

		private Queue queue;

		private Thread writeThread;

		public bool Write( byte[] data )
		{
			return Write( 0, 0, data );
		}

		public bool Write( int flag, byte[] data )
		{
			return Write( flag, 0, data );
		}

		public bool Write( int flag, int flag2, byte[] data )
		{
			if ((flag & ~FLAG_MASK) != 0)
				throw new ArgumentException( "(flag & ~FLAG_MASK) != 0" );

			if ((data.Length & ~LENGTH_MASK) != 0)
				throw new ArgumentException( "(data.Length & ~LENGTH_MASK) != 0" );

			if (flag2 == 0)
				flag &= ~HAS_ADDITIONAL_FLAG;
			else
				flag |= HAS_ADDITIONAL_FLAG;

			flag = (flag << FLAG_SHIFT) | data.Length;

			try
			{
				Packet p = new Packet( flag, flag2, data );

				if (queue != null)
				{
					EnqueuePacket( p );
				}
				else
				{
					lock (writeSync)
					{
						WritePacket( outputStream, p );
					}
				}
			}
			catch ( Exception e )
			{
				CloseConnection( e );
				return false;
			}

			return true;
		}

		private Object writeSync = new Object();

		#endregion
	}

	public class Packet
	{
		public Packet( int flag, int flag2, byte[] buf )
		{
			this.flag = flag;
			this.flag2 = flag2;
			this.buf = buf;
		}

		public readonly int flag;
		
		public readonly int flag2;
		
		public readonly byte[] buf;
	}

	public class IpcConnection
	{
		public Packet ReadPacket( Stream s )
		{
			int flag = ReadInt32( s );
					
			int length = flag & LENGTH_MASK;
			flag = (flag >> FLAG_SHIFT) & FLAG_MASK;

			int flag2;
			if ((flag & HAS_ADDITIONAL_FLAG) != 0)
				flag2 = ReadInt32( s );
			else
				flag2 = 0;

			if (length == 0)
				return new Packet( flag, flag2, new byte[0] );

			if (length > IpcConsts.MaxPacketLength)
				throw new IOException( "length > MAX_LENGTH" );

			byte[] buf = new byte[length];
			ReadBytes( s, buf );

			return new Packet( flag, flag2, buf );
		}

		public int ReadInt32( Stream s )
		{
			ReadBytes( s, readInt32buf );
			return BitConverter.ToInt32( readInt32buf, 0 );
		}

		private byte[] readInt32buf = new byte[4];

		public void ReadBytes( Stream s, byte[] buf )
		{
			if (s == null)
				throw new IOException( "closed" );

			int n = buf.Length;
			int i = 0;
			while (i < n)
			{
				int k = s.Read( buf, i, n-i );
				if (k == 0)
					throw new IOException( "eof" );
				i += k;
			}
		}

		public void WritePacket( Stream s, Packet p )
		{
			if (s == null)
				throw new IOException( "closed" );

			WriteInt32( s, p.flag );

			if (p.flag2 != 0)
				WriteInt32( s, p.flag2 );

			WriteBytes( s, p.buf );

			s.Flush();
		}

		public void WriteInt32( Stream s, int value )
		{
			WriteBytes( s, BitConverter.GetBytes( value ) );
		}

		public void WriteBytes( Stream s, byte[] buf )
		{
			s.Write( buf, 0, buf.Length );
		}

		/// <summary>
		/// Mask of bits that may be specified in flag.
		/// </summary>
		public const int FLAG_MASK = 0xff;

		/// <summary>
		/// Number of bits that the flag is shifted when combined with the length.
		/// </summary>
		public const int FLAG_SHIFT = 24;

		/// <summary>
		/// Mask of bits that may be specified in the length;
		/// </summary>
		public const int LENGTH_MASK = 0xffffff;

		/// <summary>
		/// Flag in first word of IPC header which says there is a second header word after the first.
		/// </summary>
		public const int HAS_ADDITIONAL_FLAG = 0x80;

		/// <summary>
		/// Empty flag value for the first word of the IPC header.
		/// </summary>
		public const int NO_FLAG = 0x00;
	}
}
