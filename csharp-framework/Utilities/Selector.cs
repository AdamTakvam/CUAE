using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Metreos.Utilities;

namespace Metreos.Utilities
{
	/// <summary>
	/// Reports when a selection key has been selected for some operation.
	/// The operations selected are specified in the event mask. When the
	/// delegate is invoked, the registration of the socket with the selector
	/// is canceled. If further notifications are desired, the delegate
	/// may use the SelectionKey.Reregister method.
	/// </summary>
	public delegate int SelectedDelegate( SelectionKey key, int eventMask );

	/// <summary>
	/// Reports when a SelectedDelegate throws an exception.
	/// </summary>
	public delegate void SelectedExceptionDelegate( SelectionKey key, Exception e );

	/// <summary>
	/// A front end manager for a collection of threads performing
	/// Socket.Select operations.
	/// </summary>
	public class Selector
	{
		/// <summary>
		/// Constructs the object with DEFAULT_MAX_WAIT_MICROS for max wait time
		/// and a null SelectedExceptionDelegate.
		/// </summary>
		public Selector()
			: this( null )
		{
			// nothing to do.
		}

		/// <summary>
		/// Constructs the object with DEFAULT_MAX_WAIT_MICROS for max wait time.
		/// </summary>
		/// <param name="selectedException">A delegate to notify in the case that
		/// a SelectDelegate throws an exception.</param>
		public Selector( SelectedExceptionDelegate selectedException )
			: this( DEFAULT_MAX_WAIT_MICROS, selectedException )
		{
			// nothing to do.
		}

		/// <summary>
		/// Constructs the object.
		/// </summary>
		/// <param name="maxWaitMicros">The maximum wait time in microseconds that
		/// a Socket.Select operation will block waiting for activity. A typical
		/// value is in the range 10-20 milliseconds. This is the maximum amount
		/// of time that a socket added to the selector will have to wait to be
		/// included in a Socket.Select operation. Smaller makes the selector a
		/// bit more responsive at the expense of dramatically increased cpu load
		/// and in particular a greater amount of time spent in the kernel.</param>
		/// <param name="selectedException">A delegate to notify in the case that
		/// a SelectDelegate throws an exception.</param>
		public Selector( int maxWaitMicros, SelectedExceptionDelegate selectedException )
		{
			this.maxWaitMicros = maxWaitMicros;
			this.selectedException = selectedException;
		}

		private int maxWaitMicros;

		internal SelectedExceptionDelegate selectedException;

		/// <summary>
		/// The default maxWaitMicros.
		/// </summary>
		public const int DEFAULT_MAX_WAIT_MICROS = 20000;

		public void Start()
		{
			lock (this)
			{
				if (running)
					throw new InvalidOperationException( "already running" );
				
				running = true;
			}
		}

		public void Stop()
		{
			lock (this)
			{
				if (running)
				{
					RemoveAll();
					running = false;
				}
			}
		}

		private bool running;

		/// <summary>
		/// Registers the socket with the selector to receive notification of
		/// pending operations (SELECT_READ, SELECT_WRITE, SELECT_ERROR).
		/// </summary>
		/// 
		/// <param name="socket">A socket configured for non-blocking operations.</param>
		/// 
		/// <param name="eventMask">An event mask composed by or-ing together
		/// events from the list SELECT_READ, SELECT_WRITE, and SELECT_ERROR.</param>
		/// 
		/// <param name="selected">A delegate to notify when the socket is
		/// selected for one or more operations.</param>
		/// 
		/// <param name="data">An uninterpreted object stored in the SelectionKey.</param>
		/// 
		/// <returns>A new SelectionKey to represent the registration.</returns>
		public SelectionKey Register( Socket socket, int eventMask,
			SelectedDelegate selected, object data )
		{
			return Register( new SelectionKey( this, socket,
				selected, data ), eventMask );
		}
		
		/// <summary>
		/// Unregisters the socket, so that it no longer receives notification
		/// of pending operation. This is performed on a best effort basis. If
		/// </summary>
		/// <param name="socket">A socket previously registered.</param>
		public void Unregister( Socket socket )
		{
			SelectionKey key = GetKey( socket );
			if (key != null)
				Unregister( key );
		}

		internal SelectionKey Register( SelectionKey key, int eventMask )
		{
			CheckRunning();

			// Console.WriteLine( "Selector: registering key {0} for {1}", key, eventMask );

			if (key.selector != this)
				throw new ArgumentException( "this key belongs to another selector" );

			return AddKey( key, eventMask );
		}
		
		internal bool Unregister( SelectionKey key )
		{
			CheckRunning();

			// Console.WriteLine( "Selector: unregistering key {0}", key );

			if (key.selector != this)
				throw new ArgumentException( "this key belongs to another selector" );

			return RemoveKey( key );
		}

		internal void SetEventMask( SelectionKey key, int eventMask )
		{
			CheckRunning();

			if (key.selector != this)
				throw new ArgumentException( "this key belongs to another selector" );

			lock (keys)
			{
				RemoveKey( key );
				AddKey( key, eventMask );
			}
		}

		private void CheckRunning()
		{
			if (!running)
				throw new InvalidOperationException( "not running" );
		}
		
		////////////////////
		// ACCESS TO KEYS //
		////////////////////

		internal SelectionKey GetKey( Socket socket )
		{
			lock (keys)
			{
				return (SelectionKey) keys[socket];
			}
		}

		internal SelectionKey AddKey( SelectionKey key, int eventMask )
		{
			lock (keys)
			{
				// If no events selected, return existing or, if none existing, new key.
				if (eventMask == 0)
				{
					SelectionKey existingKey = keys[key.socket] as SelectionKey;
					return existingKey == null ? key : existingKey;
				}

				if (!keys.ContainsKey( key.socket ))
				{
					// Console.WriteLine( "Selector: adding key {0}", key );
					keys.Add( key.socket, key );
					key.eventMask = eventMask;
					readyKeys.Enqueue( key );
					ensureSelectorWrappers( keys.Count );
					Monitor.PulseAll( keys );
					return key;
				}
				else
				{
					// Console.WriteLine( "Selector: already had key {0}", key );
					return null;
				}
			}
		}

		internal bool RemoveKey( SelectionKey key )
		{
			lock (keys)
			{
				if (keys.ContainsKey( key.socket ))
				{
					// Console.WriteLine( "Selector: removing key {0}", key );
					keys.Remove( key.socket );
					//key.SetSelectorWrapper( null );
					// this key might be in the queue, but will be discarded
					// when the attempt is made to add it to a SelectorWrapper.
					return true;
				}
				else
				{
					// Console.WriteLine( "Selector: didn't have key {0}", key );
				}
				return false;
			}
		}

		private void RemoveAll()
		{
			lock (keys)
			{
				keys.Clear();
				readyKeys.Clear();
			}
		}

		internal bool IsKeyValid( SelectionKey key )
		{
			lock (keys)
			{
				return keys.ContainsKey( key.socket );
			}
		}

		private void ensureSelectorWrappers( int count )
		{
			while (count > capacity)
			{
				SelectorWrapper sw = new SelectorWrapper( this, SelectorWrapper.MAX_SOCKETS, maxWaitMicros );
				capacity += sw.GetMaxSockets();
				sw.Start();
			}
		}

		internal int GetSelectionKeys( SelectorWrapper selectorWrapper,
			SelectionKey[] selectionKeys )
		{
			int n = selectionKeys.Length;
			lock (keys)
			{
				while (running)
				{
					int k = 0;
					for (int i = 0; i < n; i++)
					{
						SelectionKey key = selectionKeys[i];
						if (key == null)
							continue;
						
						if (!IsKeyValid( key ))
						{
							selectionKeys[i] = null;
							continue;
						}

//						if (key.selectorWrapper != selectorWrapper)
//						{
//							selectionKeys[i] = null;
//							continue;
//						}

						// ok, the key at i is valid and we are merely
						// going to move it from i to the blank spot at
						// k. if i == k then we need not do anything.

						if (i != k)
						{
							Assertion.Check( selectionKeys[k] == null, "selectionKeys[k] == null" );
							selectionKeys[k] = key;
							selectionKeys[i] = null;
						}
						else // i == k
						{
							// the key at k is valid, so all k
							// needs to do is bump past it.
						}
						k++;
					}

					// k is the count of valid keys in selectionKeys.
					// update n so that we don't have to keep looking
					// at empty slots in selectionKeys.

					n = k;

					if (n == 0 && readyKeys.Count == 0)
					{
						// there are no keys in selectionKeys
						// and no keys to add, so just wait until
						// some are added.
						if (running)
							Monitor.Wait( keys, 10 );
						continue;
					}

					// either n > 0 || readyKeys.Count > 0

					int m = selectionKeys.Length;
					while (n < m && readyKeys.Count > 0)
					{
						SelectionKey key = (SelectionKey) readyKeys.Dequeue();
						if (IsKeyValid( key ))
						{
//							key.SetSelectorWrapper( selectorWrapper );
							selectionKeys[n++] = key;
						}
					}

					return n;
				}
				return 0;
			}
		}

		private Hashtable keys = new Hashtable();

		private Queue readyKeys = new Queue();

		private int capacity;
	}

	/// <summary>
	/// Represents the registration of a socket with a selector.
	/// </summary>
	public class SelectionKey
	{
		internal SelectionKey( Selector selector, Socket socket,
			SelectedDelegate selected, Object data )
		{
			this.selector = selector;
			this.socket = socket;
			this.selected = selected;
			this.data = data;
		}
		
		internal Selector selector;

		internal Socket socket;
		
		private SelectedDelegate selected;
		
		private Object data;
		
		internal int eventMask;
		
		public const int SELECT_READ = 1;

		public const int SELECT_ACCEPT = SELECT_READ;
		
		public const int SELECT_WRITE = 2;

		public const int SELECT_CONNECT = SELECT_WRITE;
		
		public const int SELECT_ERROR = 4;

		public static bool IsAccept( int mask )
		{
			return IsSet( mask, SELECT_ACCEPT );
		}

		public static bool IsConnect( int mask )
		{
			return IsSet( mask, SELECT_CONNECT );
		}

		public static bool IsRead( int mask )
		{
			return IsSet( mask, SELECT_READ );
		}

		public static bool IsWrite( int mask )
		{
			return IsSet( mask, SELECT_WRITE );
		}

		public static bool IsError( int mask )
		{
			return IsSet( mask, SELECT_ERROR );
		}

		public static bool IsSet( int mask1, int mask2 )
		{
			return (mask1 & mask2) != 0;
		}

		public Selector GetSelector()
		{
			return selector;
		}

		public Socket GetSocket()
		{
			return socket;
		}

		public Object GetData()
		{
			return data;
		}

		public int GetEventMask()
		{
			return eventMask;
		}

		public void Enable( int eventMask )
		{
			SetEventMask( this.eventMask | eventMask );
		}

		public void Disable( int eventMask )
		{
			SetEventMask( this.eventMask & ~eventMask );
		}

		public void SetData( Object data )
		{
			this.data = data;
		}

		public void SetEventMask( int eventMask )
		{
			selector.SetEventMask( this, eventMask );
		}

		public override string ToString()
		{
			if (socket.Connected)
				return "SelectionKey("+socket.LocalEndPoint+" <- "+socket.RemoteEndPoint+")";
			
			return "SelectionKey("+socket.LocalEndPoint+")";
		}

		/// <summary>
		/// Reregisters the socket (of this key) to again receive notification of
		/// events. The last event mask specified is used again. This may be done
		/// in the SelectedDelegate or outside it. If done in the SelectedDelegate,
		/// it may cause a recursive invocation of the method. Don't call Reregister
		/// until you are ready to receive more notifications!
		/// </summary>
		public void Reregister()
		{
			selector.Register( this, eventMask );
		}

		/// <summary>
		/// Reregisters the socket (of this key) to again receive notification of
		/// events. This may be done in the SelectedDelegate or outside it. If
		/// done in the SelectedDelegate, it may cause a recursive invocation of
		/// the method. Don't call Reregister until you are ready to receive more
		/// notifications!
		/// </summary>
		/// 
		/// <param name="eventMask">An event mask specifying the event notifications
		/// desired.</param>
		public void Reregister( int eventMask )
		{
			selector.Register( this, eventMask );
		}

		/// <summary>
		/// Unregisters the socket (of this key) from the selector.
		/// </summary>
		/// <returns>true if it worked (false if the key was not registered).</returns>
		public bool Unregister()
		{
			return selector.Unregister( this );
		}

//		internal void SetSelectorWrapper( SelectorWrapper selectorWrapper )
//		{
//			lock (this)
//			{
//				if (selectorWrapper != null &&
//						this.selectorWrapper != null &&
//							this.selectorWrapper != selectorWrapper)
//					throw new ArgumentException( "illegal ownership change" );
//			
//				this.selectorWrapper = selectorWrapper;
//			}
//		}

		//internal SelectorWrapper selectorWrapper;

		internal void fireSelected( int eventMask )
		{
			try
			{
				// Console.WriteLine( "SelectionKey: calling listener" );
				int mask = selected( this, eventMask );
				if (mask != 0)
					SetEventMask( mask );
			}
			catch ( Exception e )
			{
				// Console.WriteLine( "SelectionKey: caught exception from listener {0}: {1}", this, e );
				if (selector.selectedException != null)
					selector.selectedException( this, e );
			}
		}
	}

	internal class SelectorWrapper
	{
		internal SelectorWrapper( Selector selector, int maxSockets, int maxWaitMicros )
		{
			if (maxSockets < 1 || maxSockets > MAX_SOCKETS)
				throw new ArgumentException( "maxSockets < 1 || maxSockets > MAX_SOCKETS" );

			this.selector = selector;
			this.maxSockets = maxSockets;
			this.maxWaitMicros = maxWaitMicros;
		}

		private Selector selector;

		private int maxSockets;

		private int maxWaitMicros;

		internal const int MAX_SOCKETS = 64;

		internal int GetMaxSockets()
		{
			return maxSockets;
		}

		internal void Start()
		{
			lock (this)
			{
				if (running)
					throw new InvalidOperationException( "already running" );

				running = true;

				Thread selectionThread = new Thread( new ThreadStart( Run ) );
				selectionThread.IsBackground = true;
				selectionThread.Name = "selection thread "+selectionThread.GetHashCode();
				selectionThread.Start();
			}
		}

		private bool running;

		private void Run()
		{
			//Console.WriteLine( "SelectorWrapper {0} starting", Thread.CurrentThread.Name );

			SelectionKey[] selectKeys = new SelectionKey[maxSockets];
			IList readList = new ArrayList();
			IList writeList = new ArrayList();
			IList errorList = new ArrayList();
			IDictionary selectedKeys = new Hashtable();
			IDictionary dead = new Hashtable();

			try
			{
				while (true)
				{
					Assertion.Check( readList.Count == 0, "readList.Count == 0" );
					Assertion.Check( writeList.Count == 0, "writeList.Count == 0" );
					Assertion.Check( errorList.Count == 0, "errorList.Count == 0" );
					Assertion.Check( selectedKeys.Count == 0, "selectedKeys.Count == 0" );

					int n = selector.GetSelectionKeys( this, selectKeys );
					if (n == 0)
						break;

					for (int i = 0; i < n; i++)
					{
						SelectionKey key = selectKeys[i];
						if (key == null)
							continue;

						int eventMask = key.eventMask;
						if ((eventMask & SelectionKey.SELECT_READ) != 0) readList.Add( key.socket );
						if ((eventMask & SelectionKey.SELECT_WRITE) != 0) writeList.Add( key.socket );
						if ((eventMask & SelectionKey.SELECT_ERROR) != 0) errorList.Add( key.socket );
					}

					while (true)
					{
						try
						{
							Socket.Select( readList, writeList, errorList, maxWaitMicros );
							break;
						}
						catch ( ArgumentException )
						{
							break;
						}
						catch ( SocketException )
						{
							// there is a bad socket in one or more of these lists.
							checkList( dead, readList );
							checkList( dead, writeList );
							checkList( dead, errorList );
						}
					}

					if (readList.Count == 0 && writeList.Count == 0 && errorList.Count == 0 && dead.Count == 0)
						continue;

					addSelectedKeys( selectedKeys, readList, SelectionKey.SELECT_READ );
					addSelectedKeys( selectedKeys, writeList, SelectionKey.SELECT_WRITE );
					addSelectedKeys( selectedKeys, errorList, SelectionKey.SELECT_ERROR );
					addSelectedKeys( selectedKeys, dead, SelectionKey.SELECT_ERROR );

					for (int i = 0; i < n; i++)
					{
						SelectionKey key = selectKeys[i];
						if (key == null)
							continue;

//						if (key.selectorWrapper != this)
//						{
//							// this key is no longer ours.
//							selectKeys[i] = null;
//							continue;
//						}

						object oMask = selectedKeys[key];
						if (oMask != null)
						{
							// the key was selected, so we remove it from our list.
							// also we unregister it because the select listener
							// will register it again if they want to. if the
							// unregister fails it means we should not call the
							// listener.

							selectKeys[i] = null;
							if (!key.Unregister())
								continue;

							Doit( key, (Int32) oMask );
						}
						else // oMask == null
						{
							// the key was not selected, so we will save it for next
							// time around.
						}
					}

					selectedKeys.Clear();

					WaitDoitsDone();
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( "SelectorWrapper {0} caught {1}", Thread.CurrentThread.Name, e );
			}
			finally
			{
				//Console.WriteLine( "SelectorWrapper {0} exiting", Thread.CurrentThread.Name );
			}
		}

		private void checkList( IDictionary dead, IList list )
		{
			int n = list.Count;
			int i = 0;
			while (i < n)
			{
				Socket s = (Socket) list[i];
				if (s.Connected)
				{
					i++;
					continue;
				}
				dead[s] = s;
				list.RemoveAt( i );
				n--;
			}
		}

		private void Doit( SelectionKey key, int eventMask )
		{
			key.fireSelected( eventMask );
		}

		private void WaitDoitsDone()
		{
			// nothing to do.
		}

		private void addSelectedKeys( IDictionary selectedKeys, IDictionary dead, int eventMask )
		{
			for (IDictionaryEnumerator i = dead.GetEnumerator(); i.MoveNext();)
			{
				Socket socket = (Socket) i.Key;

				SelectionKey key = selector.GetKey( socket );
				if (key == null)
					continue;

				object oMask = selectedKeys[key];
				if (oMask != null)
				{
					Int32 mask = (Int32) oMask;
					selectedKeys[key] = (mask | eventMask);
				}
				else
				{
					selectedKeys[key] = eventMask;
				}
				
			}
			dead.Clear();
		}

		private void addSelectedKeys( IDictionary selectedKeys, IList socketList, int eventMask )
		{
			for (IEnumerator i = socketList.GetEnumerator(); i.MoveNext();)
			{
				Socket socket = (Socket) i.Current;
				// Console.WriteLine( "selected socket {0} for {1}", socket.LocalEndPoint, eventMask );

				SelectionKey key = selector.GetKey( socket );
				if (key == null)
					continue;

				object oMask = selectedKeys[key];
				if (oMask != null)
				{
					Int32 mask = (Int32) oMask;
					selectedKeys[key] = (mask | eventMask);
				}
				else
				{
					selectedKeys[key] = eventMask;
				}
			}
			socketList.Clear();
		}
	}
}
