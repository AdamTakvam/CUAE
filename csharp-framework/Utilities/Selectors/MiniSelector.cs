using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace Metreos.Utilities.Selectors
{
	public enum UpdateOp { Add, Modify, Remove };

	public class MiniSelector: SelectorBase
	{
		public MiniSelector( SuperSelector superSelector, int maxSockets,
			SelectedDelegate defaultSelected,
			SelectedExceptionDelegate defaultSelectedException,
            LogDelegate defaultLog
        )
			: base( defaultSelected, defaultSelectedException, defaultLog )
		{
			if (maxSockets < 0 || maxSockets > MAX_SOCKETS)
				throw new ArgumentException( "maxSockets < 1 || maxSockets > MAX_SOCKETS" );
			
			if (maxSockets == 0)
				maxSockets = MAX_SOCKETS;

			this.superSelector = superSelector;
			this.maxSockets = maxSockets;

            if(!InitInterrupt())
                throw new Exception("MiniSelector: Failed to initialize interrupting socket.");
		}

		private readonly SuperSelector superSelector;

		private readonly int maxSockets;

		internal const int MAX_SOCKETS = 62; // 63rd, 64th are reserved for intr

		// ////////// //
		// ATTRIBUTES //
		// ////////// //

		internal int MaxSockets { get { return maxSockets; } }

		internal int AvailSockets { get { return MaxSockets - Count; } }

		public SelectorBase Selector { get { return superSelector != null ? (SelectorBase) superSelector : (SelectorBase) this; } }

		// ////////////////////////////////// //
		// starting and stopping the selector //
		// ////////////////////////////////// //

		override protected void DoStart()
		{
			thread = new Thread( new ThreadStart( Run ) );
			thread.IsBackground = true;
			thread.Name = "selection thread "+thread.GetHashCode();
			thread.Start();
		}

		override protected void DoStop()
		{
			//Report(( "interrupting selector" );
			Interrupt();
            
			//Report(( "waiting for selection thread" );
			if (!thread.Join( 5000 ))
			{
				//Report(( "tired of waiting for selection thread; interrupting" );
				thread.Interrupt();

				//Report(( "waiting for selection thread after interrupt" );
				if (!thread.Join( 5000 ))
				{
					//Report(( "tired of waiting for selection thread; aborting" );
					thread.Abort();

					//Report(( "waiting for selection thread after abort" );
					thread.Join( 5000 );
				}
			}

			//Report(( "closing keys" );
			foreach (SelectionKey key in GetKeys( true ))
			{
				try
				{
					key.Close();
				}
				catch
				{
					// Do nothing.
				}
			}

            if(receiver != null)
            {
                receiver.Close();
                receiver = null;
            }

            if(sender != null)
            {
                sender.Close();
                sender = null;
            }

			//Report(( "done with stop of selector" );
		}

		private Thread thread;

		// ////////////////////////////////////////// //
		// adding and removing keys from the selector //
		// ////////////////////////////////////////// //

		public override void Register( SelectionKey key )
		{
			lock (key)
			{
				lock (this)
				{
					checkStarted();
				
					if (Count >= maxSockets)
						throw new InvalidOperationException( "Count >= maxSockets" );

					key.SetSelector( null, this );
					Add( key );
					if (superSelector != null)
						superSelector.Add( key );
					UpdateLists( key, UpdateOp.Add );
				}
			}
		}

		internal void Unregister( SelectionKey key )
		{
			lock (key)
			{
				lock (this)
				{
					key.SetSelector( this, null );
					Remove( key );
					if (superSelector != null)
						superSelector.Remove( key );
					UpdateLists( key, UpdateOp.Remove );
				}
			}
		}

		// /////////////// //
		// SELECTION LISTS //
		// /////////////// //

		internal void UpdateLists( SelectionKey key, UpdateOp op )
		{
			if (IsStarted)
				DoUpdateLists( key, op );
		}

		internal void DoUpdateLists( SelectionKey key, UpdateOp op )
		{
			Socket s = key.Socket;
			if (op != UpdateOp.Remove)
			{
				bool wantsRead = key.WantsAccept||key.WantsRead;
				if (wantsRead && !key.onReadList)
				{
					Add( readList, s );
					key.onReadList = true;
				}
				else if (key.onReadList && !wantsRead)
				{
					Remove( readList, s );
					key.onReadList = false;
				}
				
				bool wantsWrite = key.WantsConnect||key.WantsWrite;
				if (wantsWrite && !key.onWriteList)
				{
					Add( writeList, s );
					key.onWriteList = true;
				}
				else if (key.onWriteList && !wantsWrite)
				{
					Remove( writeList, s );
					key.onWriteList = false;
				}

				if (!key.onErrorList)
				{
					Add( errorList, s );
					key.onErrorList = true;
				}
			}
			else // op == UpdateOp.Remove
			{
				if (key.onReadList)
				{
					Remove( readList, s );
					key.onReadList = false;
				}
					
				if (key.onWriteList)
				{
					Remove( writeList, s );
					key.onWriteList = false;
				}
				
				if (key.onErrorList)
				{
					Remove( errorList, s );
					key.onErrorList = false;
				}
			}

			//DumpList( "readList", readList );
			//DumpList( "writeList", writeList );
			//DumpList( "errorList", errorList );

			Interrupt();
		}

		private void Add( IList list, Object value )
		{
			list.Add( value );
		}

		private void Remove( IList list, Object value )
		{
			list.Remove( value );
		}

		private void DumpList( string title, IList list )
		{
			Console.WriteLine( "dumping list {0}", title );
			foreach (Socket s in list)
			{
				Console.WriteLine( "  item = {0}", s.Handle );
			}
			Console.WriteLine( "done dumping list {0}", title );
		}

		private IList readList = new ArrayList();

		private IList writeList = new ArrayList();

		private IList errorList = new ArrayList();

		private void Run()
		{
			try
			{
				while (IsStarted)
					foreach (SelectionKey key in SelectKeys())
						key.CallSelected();
			}
			catch ( ThreadInterruptedException )
			{
				// ignore
			}
			catch ( ThreadAbortException )
			{
				// ignore
			}
		}

		private ICollection SelectKeys()
		{
			// before this point, we don't care whether we've been interrupted or
			// not, because all that leads up to what we are fixing to do: rebuild
			// the selection lists.

			lock (this)
			{
				selected.Clear();

				if (!IsStarted)
					return selected.Values;
                if(receiver == null || sender == null)
                    InitInterrupt();

				Assertion.Check( selectReadList.Count == 0, "selectReadList.Count == 0" );
                selectReadList.Add(receiver);
				selectReadList.AddRange( readList );
				//Console.WriteLine( "selectReadList.Count = {0}", selectReadList.Count );

				Assertion.Check( selectWriteList.Count == 0, "selectWriteList.Count == 0" );
				selectWriteList.AddRange( writeList );
                //Console.WriteLine( "selectWriteList.Count = {0}", selectWriteList.Count );
				
				Assertion.Check( selectErrorList.Count == 0, "selectErrorList.Count == 0" );
				selectErrorList.AddRange( errorList );

				//Console.WriteLine( "selectErrorList.Count = {0}", selectErrorList.Count );
			}
			
			// after this point, any interruption, closing, etc. will be reflected
			// in the closed state of intr. and the select will return if intr is
			// closed (or some other socket has activity).

			//Report( "calling Socket.Select" );
			bool failed;
			try
			{
				Socket.Select( selectReadList, selectWriteList, selectErrorList, Int32.MaxValue );
                failed = false;
			}
			catch ( SocketException e )
			{
				if (e.ErrorCode != 10038)
					throw e;
				// one of the sockets was closed.
				failed = true;
			}
			//Report(( "called Socket.Select failed = {0}", failed );

			lock (this)
			{
				if (failed)
				{
					// scan selectErrorList for closed sockets
					foreach (Socket socket in selectErrorList)
					{
						SelectionKey key = LookupKey( socket );
						if (key == null)
							continue;
						
						if (!key.IsOpen())
						{
							key.selectedForError = true;
							selected[key] = key;
						}
					}
				}
				else
				{
					// scan all three lists for events.
					foreach (Socket socket in selectReadList)
					{
                        if(socket == receiver) //the special interrupt socket, read the data out
                        {
                            try
                            {
                                receiver.Receive(interruptBuffer);
                            }
                            catch(Exception e)
                            {
                                //There is something wrong, try re-iniitialize 
                                defaultLog(TraceLevel.Error,
                                    "MiniSelector: MiniSelector encountered an error while receiving on interrupted receiver socket.", 
                                    e);
                                if (receiver != null)
                                    receiver.Close();
                                receiver = null;

                                if(sender != null)
                                    sender.Close();
                                sender = null;

                            }
                            continue;
                        }

						SelectionKey key = LookupKey( socket );
						if (key == null)
							continue;
						
						if (key.WantsAccept)
							key.selectedForAccept = true;
						else
							key.selectedForRead = true;

						selected[key] = key;
					}

					foreach (Socket socket in selectWriteList)
					{
						SelectionKey key = LookupKey( socket );
						if (key == null)
							continue;
						
						if (key.WantsConnect)
							key.selectedForConnect = true;
						else
							key.selectedForWrite = true;

						selected[key] = key;
					}

					foreach (Socket socket in selectErrorList)
					{
						SelectionKey key = LookupKey( socket );
						if (key == null)
							continue;
						
						key.selectedForError = true;
						selected[key] = key;
					}
				}

				selectReadList.Clear();
				selectWriteList.Clear();
				selectErrorList.Clear();
				return selected.Values;
			}
		}

//		private void Report( string msg )
//		{
//			Console.WriteLine( "{0}: {1}", DateTime.Now.ToString( TIME_FMT ), msg );
//		}

//		private void Report( string fmt, params object[] args )
//		{
//			Console.WriteLine( "{0}: {1}", DateTime.Now.ToString( TIME_FMT ), String.Format( fmt, args ) );
//		}

		private const String TIME_FMT = "HH:mm:ss.fff";
		
		private ArrayList selectReadList = new ArrayList();

		private ArrayList selectWriteList = new ArrayList();

		private ArrayList selectErrorList = new ArrayList();

		private IDictionary selected = new Hashtable();

        /// <summary>
        /// Intitialize the select interrupt mechanism. It creates a listening socket and
        /// waits for sender socket to connect. Once the connection is made, a receiver socket
        /// is created as well. The listening socket in turn is closed. All exceptions will be
        /// left to caller to handle.
        /// </summary>
        private bool InitInterrupt()
        {
            bool rc = false;
            Thread acceptThread = null;

            lock(this)
            {
                //reset the receiver/sender
                if(receiver != null)
                    receiver.Close();
                receiver = null;

                if(sender != null)
                    sender.Close();
                sender = null;
                
                //Create the listening socket
                defaultLog(TraceLevel.Verbose, "MiniSelector: Initializing interruping sockets...", null);
                acceptSocket = SelectionKey.NewTcpSocket(true);
                receiverAddr = new IPEndPoint(IPAddress.Loopback, 0);
                acceptSocket.Bind(receiverAddr);
                acceptSocket.Listen(1);
                receiverAddr = acceptSocket.LocalEndPoint as IPEndPoint;
                defaultLog(TraceLevel.Verbose, "MiniSelector: listening for interruping socket to connect...", null);

                //start a thread to listen
                acceptThread = new Thread(new ThreadStart(AcceptThreadProc));
                acceptThread.Start();

                //create and connect the sending socket 
                sender = SelectionKey.NewTcpSocket(true);
                defaultLog(TraceLevel.Verbose, "MiniSelector: interruping socket is connecting...", null);
                sender.Connect(receiverAddr);
                defaultLog(TraceLevel.Verbose, "MiniSelector: interruping socket is connected.", null);

            }

            //wait for listening thread to terminate
            acceptThread.Join(5000);
            acceptThread = null;

            try
            {
                if(acceptEvent.WaitOne())
                    acceptEvent.Reset();
            }
            catch(Exception)
            {
            }

            lock(this)
            {
                //we're done with listeing socket, close it
                acceptSocket.Close();
                acceptSocket = null;

                if(receiver == null || sender == null) //failed to setup interrupting mechanism
                {
                    defaultLog(TraceLevel.Error, "MiniSelector: Failed to setup interrupting sockets.", null);
                    rc = false;
                    if(sender != null)
                    {
                        sender.Close();
                        sender = null;
                    }

                    if(receiver != null)
                    {
                        receiver.Close();
                        receiver = null;
                    }
                }
                else
                {
                    defaultLog(TraceLevel.Verbose, "MiniSelector: interruping sockets are initialized.", null);
                    rc = true;
                }
            }

            return rc;
        }

        private void AcceptThreadProc()
        {
            try
            {
                Socket s = acceptSocket.Accept();
                lock(this)
                {
                    receiver = s;
                }
                acceptEvent.Set();
            }
            catch(Exception e)
            {
                defaultLog(TraceLevel.Error, "MiniSelector: Accept exception in AcceptThreadProc.", e);
            }
        }

        private void Interrupt()
        {
            defaultLog(TraceLevel.Verbose, "MiniSelector: interruping socket select...", null);
            lock(this)
            {
                try
                {
                    sender.Send(interruptData);
                }
                catch(Exception e)
                {
                    sender.Close();
                    sender = null;
                    defaultLog(TraceLevel.Error, "MiniSelector: Error sending interrupt data.", e); 
                }

              //  if(failed) //try to setup interrupt mechanism again
                //    InitInterrupt();
            }
        }

        //interrupt related sockets and buffers
        Socket acceptSocket = null;
        private Socket receiver = null;
        private Socket sender = null;
        private IPEndPoint receiverAddr = null;
        private byte[] interruptData = new byte[1];
        private byte[] interruptBuffer = new byte[64];
        private ManualResetEvent acceptEvent = new ManualResetEvent(false);
    }
}
