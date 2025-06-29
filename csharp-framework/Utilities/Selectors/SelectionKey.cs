using System;
using System.Net;
using System.Net.Sockets;

namespace Metreos.Utilities.Selectors
{
	/// <summary>
	/// Represents the registration of a socket with a superSelector.
	/// </summary>
	public class SelectionKey
	{
		internal SelectionKey( Socket socket, Object data, SelectedDelegate selected,
			SelectedExceptionDelegate selectedException )
		{
			if (selectedException == null)
				throw new ArgumentException( "selectedException == null" );

			this.socket = socket;
			this.data = data;
			this.selected = selected;
			this.selectedException = selectedException;
		}

		private readonly Socket socket;
		
		private Object data;
		
		private SelectedDelegate selected;

		private SelectedExceptionDelegate selectedException;

		// //////////// //
		// REGISTRATION //
		// //////////// //

		internal void SetSelector( MiniSelector oldSelector, MiniSelector newSelector )
		{
			lock (this)
			{
				if (selector != oldSelector)
					throw new InvalidOperationException( "selector != oldSelector" );

				if (newSelector != null)
				{
					if (socket == null || socket.Handle.ToInt32() < 0)
						throw new ArgumentException( "socket is null or closed" );

					if (socket.Blocking)
						throw new ArgumentException( "socket is blocking" );
				}
				
				selector = newSelector;
			}
		}

		private void UpdateLists()
		{
			lock (this)
			{
				if (selector != null)
				{
					lock (selector)
					{
						selector.UpdateLists( this, UpdateOp.Modify );
					}
				}
			}
		}

		public void Unregister()
		{
			lock (this)
			{
				if (selector != null)
					selector.Unregister( this );
			}
		}

		public SelectorBase Selector { get { return selector.Selector; } }

		private MiniSelector selector;

		// ////////// //
		// ATTRIBUTES //
		// ////////// //

		public Socket Socket { get { return socket; } }

		public Object Data { get { return data; } set { data = value; } }

		public SelectedDelegate Selected { get { return selected; } set { selected = value; } }

		// //////////////// //
		// LIST MEMBERSHIPS //
		// //////////////// //
		
		internal bool onReadList;
		
		internal bool onWriteList;

		internal bool onErrorList;

		// ////////////// //
		// ENABLED EVENTS //
		// ////////////// //

		public void SetWants( bool wantsAccept, bool wantsConnect, bool wantsRead, bool wantsWrite )
		{
			// you can specify wantsAccept, wantsConnect, or either wantsRead or wantsWrite.
			if (wantsAccept && (wantsConnect || wantsRead || wantsWrite))
				throw new ArgumentException( "wantsAccept && (wantsConnect || wantsRead || wantsWrite)" );

			if (wantsConnect && (wantsRead || wantsWrite))
				throw new ArgumentException( "wantsConnect && (wantsRead || wantsWrite)" );

			lock (this)
			{
				this.wantsAccept = wantsAccept;
				this.wantsConnect = wantsConnect;
				this.wantsRead = wantsRead;
				this.wantsWrite = wantsWrite;
				UpdateLists();
			}
		}

		public bool WantsAccept
		{
			get { return wantsAccept; }
			set { lock (this) { wantsAccept = value; UpdateLists(); } }
		}
		private bool wantsAccept;

		public bool WantsConnect
		{
			get { return wantsConnect; }
			set { lock (this) { wantsConnect = value; UpdateLists(); } }
		}
		private bool wantsConnect;

		public bool WantsRead
		{
			get { return wantsRead; }
			set { lock (this) { wantsRead = value; UpdateLists(); } }
		}
		private bool wantsRead;

		public bool WantsWrite
		{
			get { return wantsWrite; }
			set { lock (this) { wantsWrite = value; UpdateLists(); } }
		}
		private bool wantsWrite;

		// /////////////// //
		// SELECTED EVENTS //
		// /////////////// //

		internal void CallSelected()
		{
			try
			{
				if (selected == null)
					throw new NullReferenceException( "selected == null" );
				
				selected( this );
			}
			catch ( Exception e )
			{
				selectedException( this, e );
			}
			finally
			{
				selectedForAccept = false;
				selectedForConnect = false;
				selectedForRead = false;
				selectedForWrite = false;
				selectedForError = false;
			}
		}

		public bool IsSelectedForAccept
		{
			get { return selectedForAccept; }
		}
		internal bool selectedForAccept;

		public bool IsSelectedForConnect
		{
			get { return selectedForConnect; }
		}
		internal bool selectedForConnect;

		public bool IsSelectedForRead
		{
			get { return selectedForRead; }
		}
		internal bool selectedForRead;

		public bool IsSelectedForWrite
		{
			get { return selectedForWrite; }
		}
		internal bool selectedForWrite;

		public bool IsSelectedForError
		{
			get { return selectedForError; }
		}
		internal bool selectedForError;

		// ///////// //
		// SHORTCUTS //
		// ///////// //

		public void Listen( IPEndPoint localEp, int queueLength )
		{
			socket.Bind( localEp );
			socket.Listen( queueLength );
			SetWants( true, false, false, false );
		}

		public Socket Accept()
		{
			return socket.Accept();
		}

		public void Connect( IPEndPoint localEp, IPEndPoint remoteEp )
		{
			try
			{
				if (localEp != null)
					socket.Bind( localEp );

				socket.Connect( remoteEp );

				SetWants( false, true, false, false );
			}
			catch ( SocketException e )
			{
				if (e.ErrorCode != 10035)
					throw e;
			}
		}

		public int Receive( byte[] buf )
		{
			return socket.Receive( buf, 0, buf.Length, SocketFlags.None );
		}

		public int Receive( byte[] buf, int offset, int length )
		{
			return socket.Receive( buf, offset, length, SocketFlags.None );
		}

		public int Send( byte[] buf )
		{
			return socket.Send( buf, 0, buf.Length, SocketFlags.None );
		}

		public int Send( byte[] buf, int offset, int length )
		{
			return socket.Send( buf, offset, length, SocketFlags.None );
		}

		public void Shutdown( SocketShutdown how )
		{
			socket.Shutdown( how );
		}

		public bool IsOpen()
		{
			return socket.Handle.ToInt32() >= 0;
		}

		public void Close()
		{
			Unregister();
			socket.Close();
		}

		// //// //
		// MISC //
		// //// //

		public override string ToString()
		{
			try
			{
				return "SelectionKey("+socket.LocalEndPoint+" <- "+socket.RemoteEndPoint+")";
			}
			catch
			{
				try
				{
					return "SelectionKey("+socket.LocalEndPoint+")";
				}
				catch
				{
					return "SelectionKey(unknown)";
				}
			}
		}

		public static Socket NewTcpSocket( bool blocking )
		{
			Socket s = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
			s.Blocking = blocking;
			return s;
		}
	}
}
