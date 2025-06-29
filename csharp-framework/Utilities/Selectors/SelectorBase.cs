using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Metreos.Utilities.Selectors
{
	/// <summary>
	/// Reports when a selection key has been selected for some operation.
	/// The operations selected are specified in the key.
	/// </summary>
	public delegate void SelectedDelegate( SelectionKey key );

	/// <summary>
	/// Reports when a SelectedDelegate throws an exception.
	/// </summary>
	public delegate void SelectedExceptionDelegate( SelectionKey key, Exception e );

    /// <summary>
    /// Reports the error from selector.
    /// </summary>
    /// <param name="msg">the error message</param>
    /// <param name="e">the exception if there is any. otherwise it is null.</param>
    public delegate void LogDelegate(System.Diagnostics.TraceLevel level, string msg, Exception e);

	/// <summary>
	/// Abstraction of the interface to selector.
	/// </summary>
	abstract public class SelectorBase: Startable
	{
		protected SelectorBase( SelectedDelegate defaultSelected,
			SelectedExceptionDelegate defaultSelectedException,
            LogDelegate defaultLog)
		{
			this.defaultSelected = defaultSelected;
			this.defaultSelectedException = defaultSelectedException;
            this.defaultLog = defaultLog;
		}

		protected readonly SelectedDelegate defaultSelected;

		protected readonly SelectedExceptionDelegate defaultSelectedException;

        protected readonly LogDelegate defaultLog;

		// ////////////////////////////////////// //
		// Registering a socket with the selector //
		// ////////////////////////////////////// //

		/// <summary>
		/// Registers a socket to receive notifications of enabled operations.
		/// The associated data object is set to null, the delegates are set
		/// to the defaults, and all notifications are disabled (except error).
		/// </summary>
		/// <param name="socket">The socket to be monitored for enabled operations.</param>
		/// <returns>The selection key which can be used to perform various
		/// operations on the socket.</returns>
		public SelectionKey Register( Socket socket )
		{
			return Register( socket, null, defaultSelected, defaultSelectedException,
				false, false, false, false );
		}

		/// <summary>
		/// Registers a socket to receive notifications of enabled operations.
		/// The delegates are set to the defaults, and all notifications are
		/// disabled (except error).
		/// </summary>
		/// <param name="socket">The socket to be monitored for enabled operations.</param>
		/// <param name="data">An uninterpreted data object to be associated with
		/// the selection key.</param>
		/// <returns>The selection key which can be used to perform various
		/// operations on the socket.</returns>
		public SelectionKey Register( Socket socket, Object data )
		{
			return Register( socket, data, defaultSelected, defaultSelectedException,
				false, false, false, false );
		}

		/// <summary>
		/// Registers a socket to receive notifications of enabled operations.
		/// The selected exception delegate is set to the default, and all
		/// notifications are disabled (except error).
		/// </summary>
		/// <param name="socket">The socket to be monitored for enabled operations.</param>
		/// <param name="data">An uninterpreted data object to be associated with
		/// the selection key.</param>
		/// <param name="selected">The delegate to notify when an enabled operation
		/// is possible.</param>
		/// <returns>The selection key which can be used to perform various
		/// operations on the socket.</returns>
		public SelectionKey Register( Socket socket, Object data,
			SelectedDelegate selected )
		{
			return Register( socket, data, selected, defaultSelectedException );
		}

        /// <summary>
        /// Registers a socket to receive notifications of enabled operations.
        /// The delegates are set to the defaults.
        /// </summary>
        /// <param name="socket">The socket to be monitored for enabled operations.</param>
        /// <param name="data">An uninterpreted data object to be associated with
        /// the selection key.</param>
        /// <param name="wantsAccept">Requests notification of enabled accept operation.</param>
        /// <param name="wantsConnect">Requests notification of enabled connect operation.</param>
        /// <param name="wantsRead">Requests notification of enabled read operation.</param>
        /// <param name="wantsWrite">Requests notification of enabled write operation.</param>
        /// <returns>The selection key which can be used to perform various
        /// operations on the socket.</returns>
        public SelectionKey Register(Socket socket, Object data,
            bool wantsAccept, bool wantsConnect, bool wantsRead, bool wantsWrite)
        {
            SelectionKey key = new SelectionKey(socket, data, defaultSelected, defaultSelectedException);
            key.SetWants(wantsAccept, wantsConnect, wantsRead, wantsWrite);

            Register(key);

            return key;
        }
        
        /// <summary>
		/// Registers a socket to receive notifications of enabled operations. All
		/// notifications are disabled (except error).
		/// </summary>
		/// <param name="socket">The socket to be monitored for enabled operations.</param>
		/// <param name="data">An uninterpreted data object to be associated with
		/// the selection key.</param>
		/// <param name="selected">The delegate to notify when an enabled operation
		/// is possible.</param>
		/// <param name="selectedException">The delegate to notify when the selected
		/// delegate throws an exception.</param>
		/// <returns>The selection key which can be used to perform various
		/// operations on the socket.</returns>
		public SelectionKey Register( Socket socket, Object data, SelectedDelegate selected,
			SelectedExceptionDelegate selectedException )
		{
			return Register( socket, data, selected, selectedException, false, false, false,
				false );
		}

		/// <summary>
		/// Registers a socket to receive notifications of enabled operations.
		/// </summary>
		/// <param name="socket">The socket to be monitored for enabled operations.</param>
		/// <param name="data">An uninterpreted data object to be associated with
		/// the selection key.</param>
		/// <param name="selected">The delegate to notify when an enabled operation
		/// is possible.</param>
		/// <param name="selectedException">The delegate to notify when the selected
		/// delegate throws an exception.</param>
		/// <param name="wantsAccept">Requests notification of enabled accept operation.</param>
		/// <param name="wantsConnect">Requests notification of enabled connect operation.</param>
		/// <param name="wantsRead">Requests notification of enabled read operation.</param>
		/// <param name="wantsWrite">Requests notification of enabled write operation.</param>
		/// <returns>The selection key which can be used to perform various
		/// operations on the socket.</returns>
		public SelectionKey Register( Socket socket, Object data, SelectedDelegate selected,
			SelectedExceptionDelegate selectedException, bool wantsAccept,
			bool wantsConnect, bool wantsRead, bool wantsWrite )
		{
			if (selected == null)
				selected = defaultSelected;

			if (selectedException == null)
				selectedException = defaultSelectedException;

			SelectionKey key = new SelectionKey( socket, data, selected, selectedException );
			key.SetWants( wantsAccept, wantsConnect, wantsRead, wantsWrite );
			
			Register( key );
			
			return key;
		}

		/// <summary>
		/// Registers a previously created selection key with the selector.
		/// </summary>
		/// <param name="key">The selection key to register.</param>
		abstract public void Register( SelectionKey key );

		/// <summary>
		/// Unregisters the socket from the selector.
		/// </summary>
		/// <param name="socket">A socket to be unregistered.</param>
		/// <returns>The selection key of the socket if registered.</returns>
		/// <remarks>This does not close the socket.</remarks>
		public SelectionKey Unregister( Socket socket )
		{
			SelectionKey key = LookupKey( socket );
			if (key != null)
				key.Unregister();
			return key;
		}

		// ///////////////////////// //
		// LOW-LEVEL KEYS MANAGEMENT //
		// ///////////////////////// //

		/// <summary>
		/// Returns the number of keys being managed.
		/// </summary>
		public int Count { get { return keys.Count; } }

		/// <summary>
		/// Returns the selection key for a given socket.
		/// </summary>
		/// <param name="socket">a socket to lookup</param>
		/// <returns>the selection key for a given socket</returns>
		public SelectionKey LookupKey( Socket socket )
		{
			return (SelectionKey) keys[socket];
		}

		internal void Add( SelectionKey key )
		{
			keys.Add( key.Socket, key );
		}

		internal void Remove( SelectionKey key )
		{
			lock (keys.SyncRoot)
			{
				Assertion.Check( key.Socket != null, "key.Socket != null" );
				Assertion.Check( keys.Contains( key.Socket ), "keys.Contains( key.Socket )" );
				keys.Remove( key.Socket );
				Assertion.Check( !keys.Contains( key.Socket ), "!keys.Contains( key.Socket )" );
			}
		}

		internal SelectionKey[] GetKeys( bool clear )
		{
			lock (keys.SyncRoot)
			{
				int n = keys.Count;
				SelectionKey[] x = new SelectionKey[n];
				keys.Values.CopyTo( x, 0 );
				if (clear)
					keys.Clear();
				return x;
			}
		}

		private IDictionary keys = Hashtable.Synchronized( new Hashtable() );
	}
}
