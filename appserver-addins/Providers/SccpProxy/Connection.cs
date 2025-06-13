using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Utilities.Selectors;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents a TCP socket connection to a CCM or SCCP
	/// client, although no distinction is made between the two.
	/// </summary>
	public abstract class Connection
	{
		/// <summary>
		/// Constructor that initializes member variables and creates a new
		/// thread to read on this connection.
		/// </summary>
		/// <param name="socket">TCP socket on which to do reads.</param>
		/// <param name="processIncomingPacket">Delegate to process incoming
		/// packets.</param>
		/// <param name="socketFailure">Delegate to handle socket failure.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		/// <param name="selector">The selector to use with the sockets we create.</param>
		public Connection(Metreos.Utilities.Selectors.SelectionKey key,
			QueueProcessor processIncomingPacket,
			QueueProcessorDelegate iqpd,
			OnSocketFailureDelegate socketFailure,
			SccpProxyProvider provider,
			SelectorBase selector)
		{
			this.key = key;
			this.processIncomingPacket = processIncomingPacket;
			this.iqpd = iqpd;
			this.socketFailure = socketFailure;
			this.provider = provider;
			this.selector = selector;
			readBuffer = new byte[Consts.ReadBufferLength];
			//packet = new System.IO.MemoryStream();
			//knowSupposedPacketLength = false;
			counterpartUniqueAddress = null;	// Don't know who the counterpart is yet.
			remoteAddress = (IPEndPoint) key.Socket.RemoteEndPoint;
			localAddress = (IPEndPoint) key.Socket.LocalEndPoint;
		}

		public QueueProcessor IncomingQueueProcessor { get { return processIncomingPacket; } }

		private abstract class Consts
		{
			/// <summary>
			/// Number of seconds for socket to linger after Close() is called
			/// on it.
			/// </summary>
			public const int LingerSecs = 30;

			/// <summary>
			/// Length field in packet header is a 32-bit integer. This field
			/// is necessary because TCP is a streaming protocol--packet
			/// boundaries are not significant.
			/// </summary>
			public const int LengthOfLength = 4;

			/// <summary>
			/// There is a 32-bit reserved field in the header after the length
			/// field that isn't used for anything.
			/// </summary>
			public const int LengthOfReserved = 4;

			/// <summary>
			/// Packet header length. Payload immediately follows header.
			/// </summary>
			public const int LengthOfHeader = LengthOfLength + LengthOfReserved;

			/// <summary>
			/// The maximum packet length we'll accept. includes the header length.
			/// </summary>
			public const int MaxPacketLength = 1024;

			/// <summary>
			/// The maximum body length we'll accept.
			/// </summary>
			public const int MaxBodyLength = MaxPacketLength - LengthOfHeader;

			/// <summary>
			///  Maximum read-buffer length. This is the most data we will accumulate
			///  in one shot.
			/// </summary>
			public const int ReadBufferLength = MaxPacketLength * 4;
		}

		/// <summary>
		/// Read-only property whose value is whether this connection is
		/// active--the socket has not been closed.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return key != null & key.Socket.Handle.ToInt32() >= 0;
			}
		}

		/// <summary>
		/// Refers back to provider in order to access methods in its base class.
		/// </summary>
		protected SccpProxyProvider provider;

		private SelectorBase selector;

		/// <summary>
		/// Session of which this connection is a part.
		/// </summary>
		/// <remarks>
		/// There is always a client connection and usually a CCM connection.
		/// </remarks>
		protected Session session;

		/// <summary>
		/// Property that has the value of the session this connection belongs
		/// to.
		/// </summary>
		public Session  MySession
		{
			get	{ return session; }
			set	{ session = value; }
		}

		/// <summary>
		/// TCP socket on which to do reads.
		/// </summary>
		private Metreos.Utilities.Selectors.SelectionKey key;

		public bool IsCurrentThread	{ get {	return false; } }

		/// <summary>
		/// Method to invoke to report (or not) that no counterpart has been
		/// found for the connection over which a message was sent.
		/// </summary>
		/// <param name="message">Message sent over connection for which we are
		/// looking for counterpart.</param>
		public abstract void ReportNoCounterpartOnProxy(Message message);

		/// <summary>
		/// Address that uniquely identifes this connection.
		/// </summary>
		/// <remarks>
		/// Server connections (between proxy and SCCP clients) all have the
		/// same local address--our listen port--while client connections
		/// (between proxy and CCMs) share the same remote addresses (several
		/// clients can be proxied to the same CCM).
		/// </remarks>
		public abstract IPEndPoint UniqueAddress
		{
			get;
		}

		/// <summary>
		/// Delegate to invoke upon socket failure.
		/// </summary>
		private OnSocketFailureDelegate socketFailure;

		/// <summary>
		/// This identifies the other connection with which this connection is
		/// associated.
		/// </summary>
		/// <remarks>
		/// One connection represents the connection with a CCM; the other, with a
		/// SCCP client. This is how the two are tied together within the
		/// connections table.
		/// </remarks>
		private IPEndPoint counterpartUniqueAddress;

		/// <summary>
		/// Accessor for address of this connection's counterpart. Connections
		/// are paired--one facing a CCM; another facing an SCCP client.
		/// </summary>
		public IPEndPoint CounterpartUniqueAddress
		{
			get { return counterpartUniqueAddress; }
			set	{ counterpartUniqueAddress = value;	}
		}

		/// <summary>
		/// "Start" connection by starting the read thread.
		/// </summary>
		public void Start()
		{
			Assertion.Check(key != null,
				"SccpProxyProvider: cannot start after socket closed");

			// Don't coalesce messages into the same packet.
			// Immediately transmit each message in its own packet.
			key.Socket.SetSocketOption(SocketOptionLevel.Tcp,
				SocketOptionName.NoDelay, 1);

			// The socket will linger as long as N second after Socket.Close
			// is called. Ensures that all data is written.
			key.Socket.SetSocketOption(SocketOptionLevel.Socket,
				SocketOptionName.Linger,
				new LingerOption(true, Consts.LingerSecs));

			key.Socket.Blocking = false;
			key.Selected = new Metreos.Utilities.Selectors.SelectedDelegate( Selected );

			// this has to be last.
			key.WantsRead = true;
		}

		/// <summary>
		/// Abort thread and close socket. Connection cannot be re-started
		/// with Start().
		/// </summary>
		public void Stop()
		{
			if (key != null)
			{
				key.Close();
				key = null;
			}
		}

		/// <summary>
		/// This object processes incoming packets through a queue in a
		/// separate thread.
		/// </summary>
		private QueueProcessor processIncomingPacket;

		private QueueProcessorDelegate iqpd;

		/// <summary>
		/// This read-only property is the address of the remote host (client
		/// or CCM) to which this connection is communicating.
		/// </summary>
		public IPEndPoint RemoteAddress
		{
			get
			{
				return remoteAddress;
			}
		}

		private IPEndPoint remoteAddress;

		/// <summary>
		/// This read-only property is the address of the local host (us) with
		/// which this connection is communicating.
		/// </summary>
		public IPEndPoint LocalAddress
		{
			get
			{
				return localAddress;
			}
		}

		private IPEndPoint localAddress;

		/// <summary>
		/// Raw read buffer. The result of each read is read directly into here.
		/// We then accumulate that into the variable-length packet buffer.
		/// </summary>
		private readonly byte[] readBuffer;

		/// <summary>
		/// Variable-length buffer for incoming data.
		/// </summary>
		//private System.IO.MemoryStream packet;

		/// <summary>
		/// Do we know the length of the next packet?
		/// </summary>
		/// <remarks>
		/// We don't know how big packet is until we have payload length.
		/// </remarks>
		//private bool knowSupposedPacketLength;

		/// <summary>
		/// The length of the packet which is based on the payload length as
		/// extracted from the data stream.
		/// </summary>
		/// <remarks>
		/// Undefined value when !knowSupposedPacketLength.
		/// </remarks>
		//private int supposedPacketLength;

		/// <summary>
		/// Whether the read thread is in the process of terminating.
		/// </summary>

		/// <summary>
		/// Message constructor.
		/// </summary>
		/// <param name="rawMessage">Byte array containing raw, binary SCCP
		/// message starting with the packet-length field.</param>
		/// <param name="fromRemote">Address of where message came from.</param>
		/// <param name="fromLocal">Address on localhost where message was sent to.</param>
		protected abstract Message CreateMessage(byte[] rawMessage,
			IPEndPoint fromRemote, IPEndPoint fromLocal);

		private void Selected(Metreos.Utilities.Selectors.SelectionKey key)
		{
			//provider.LogWrite(TraceLevel.Verbose, "Cnx: {0}: selected for {1}", this, eventMask);

			if (key.IsSelectedForError)
			{
				SelectedForError(key);
				return;
			}

			if (key.IsSelectedForRead)
				SelectedForRead(key);
		}

		private void SelectedForError(Metreos.Utilities.Selectors.SelectionKey key)
		{
			socketFailure(this, "selected for error");
			key.Close();
		}

		private int currentLength = 0;

		private int currentIndex = 0;

		private bool readingHeader = true;

		private int bytesNeeded = Consts.LengthOfHeader;

		private void SelectedForRead(Metreos.Utilities.Selectors.SelectionKey key)
		{
			try
			{
				try
				{
					if (currentLength >= readBuffer.Length)
						throw new Exception("stuck");
					
					//provider.LogWrite(TraceLevel.Verbose, "{0} bytesNeeded {1} currentLength {2}", this, bytesNeeded, currentLength);

					int bytesRead = key.Receive(readBuffer, currentLength,
						readBuffer.Length-currentLength);
					
					//provider.LogWrite(TraceLevel.Verbose, "{0} bytesRead {1}", this, bytesRead);

					if (bytesRead <= 0)
					{
						socketFailure(this, "end of data");
						key.Close();
						return;
					}

					currentLength += bytesRead;
				}
				catch ( SocketException e )
				{
					//provider.LogWrite(TraceLevel.Verbose, "{0} xsocket.Receive {1}", this, e.ErrorCode);

					if (e.ErrorCode != 10035)
						throw e;
				}
				
				//provider.LogWrite(TraceLevel.Verbose, "Cnx: {0}: read {1} bytes", this, bytesRead);

				while ((currentLength-currentIndex) >= bytesNeeded)
					ProcessReadData();
				
				CompactBuffer();
			}
			catch (Exception e)
			{
				socketFailure(this, "caught exception: "+e.Message);
				key.Close();
			}
		}

		private void ProcessReadData()
		{
			if (readingHeader)
			{
				Assertion.Check( bytesNeeded == Consts.LengthOfHeader,
					"bytesNeeded == Consts.LengthOfHeader" );

				int len = BitConverter.ToInt32( readBuffer, currentIndex );
				//int reserved = BitConverter.ToInt32( readBuffer, 4 );
				
				//provider.LogWrite( TraceLevel.Verbose, "{0} len {1}", this, len );

				if (len == 0)
				{
					// we've already read all 8 bytes of this otherwise
					// empty packet. so just reset the index and start
					// over.
					currentIndex += Consts.LengthOfHeader;
					return;
				}

				if (len < 0 || len > Consts.MaxBodyLength)
					throw new Exception( "len < 0 || len > Consts.MaxBodyLength" );

				readingHeader = false;
				bytesNeeded += len;
				return;
			}

			// we've got the body of a message.

			//provider.LogWrite(TraceLevel.Verbose, "{0} bytesNeeded {1} currentLength {2}: {3}", this, bytesNeeded, currentLength, dumpBuf(readBuffer, 0, currentLength));

			//dumpBuf(readBuffer, 0, currentLength);

			// copy out the current packet
			byte[] buf = new byte[bytesNeeded];
			Array.Copy( readBuffer, currentIndex, buf, 0, bytesNeeded );

			readingHeader = true;
			currentIndex += bytesNeeded;
			bytesNeeded = Consts.LengthOfHeader;

			//provider.LogWrite(TraceLevel.Verbose, "{0} sent {1}", this, dumpBuf(readBuffer, 0, bytesNeeded));
			processIncomingPacket.Enqueue(iqpd,
				CreateMessage(buf, remoteAddress, localAddress));
		}

		private void CompactBuffer()
		{
			if (currentIndex == currentLength)
			{
				// there is no data in the buffer.
				currentIndex = 0;
				currentLength = 0;
				return;
			}

			int availableSpace = Consts.ReadBufferLength - currentIndex;
			if (availableSpace < Consts.MaxPacketLength)
			{
				int n = currentLength-currentIndex;
				Array.Copy( readBuffer, currentIndex, readBuffer, 0, n );
				currentIndex = 0;
				currentLength = n;
			}
		}

		private string dumpBuf( byte[] buf, int index, int length )
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('[');
			for (int i = 0; i < length; i++)
			{
				if (i != 0)
					sb.Append(' ');
				sb.Append(buf[index+i]&255);
			}
			sb.Append(']');
			return sb.ToString();
		}

		/// <summary>
		/// Entry point for a thread that does blocking reads on this
		/// connection's socket.
		/// </summary>
		/// <remarks>
		/// Upon socket failure, it closes both this connection and, if
		/// present, its counterpart connection, notifies the app of the
		/// failure, and then kills the read thread for each connection.
		/// </remarks>
//		public void ReadSocket()
//		{
//			InitializeStatistics();
//
//			while (true)
//			{
//				try
//				{
//					//CollectReceiveInvokationStatistics();
//
//					int bytesRead = socket.Receive(readBuffer);
//					if (bytesRead <= 0)
//					{
//						// If thread is already in the process of terminating,
//						// there is no need to report it again.
//						if (!threadTerminating)
//						{
//							threadTerminating = true;
//
//							socketFailure(this, "bytes read: " + bytesRead +
//								" (" + remoteAddress + ")");
//						}
//						break;
//					}
//
//					//CollectReceiveStatistics(bytesRead);
//
//					// Process data we just read.
//					ProcessReadData(bytesRead);
//				}
//				catch (ThreadAbortException)
//				{
//					// If thread is already in the process of terminating,
//					// there is no need to report it again.
//					if (!threadTerminating)
//					{
//						threadTerminating = true;
//
//						// If simply because thread is terminating, don't log
//						// an error. Reset abort so that we can terminate
//						// gracefully rather than end right here.
//						Thread.ResetAbort();
//					}
//					break;
//				}
//				catch (SocketException e)
//				{
//					// If thread is already in the process of terminating,
//					// there is no need to report it again.
//					if (!threadTerminating)
//					{
//						threadTerminating = true;
//
//						// If not simply because thread is terminating,
//						// report socket failure.
//						if (e.ErrorCode != 10004)
//						{
//							socketFailure(this,
//								e.Message + " (" + remoteAddress + ")");
//						}
//					}
//					break;
//				}
//				catch (Exception e)
//				{
//					// If thread is already in the process of terminating,
//					// there is no need to report it again.
//					if (!threadTerminating)
//					{
//						threadTerminating = true;
//
//						socketFailure(this,
//							e.Message + " (" + remoteAddress + ")");
//					}
//					break;
//				}
//			}
//
//			//LogConnectionStatistics();
//		}

		/// <summary>Process the data in the readBuffer.</summary>
		/// <remarks>
		/// This could involve data previously read into the buffer; some data
		/// may be left in the readBuffer after processing (this would be the
		/// first part subsequent packet).
		/// </remarks>
		/// <param name="bytesRead">Number of bytes in readBuffer. Could be zero.</param>
//		private void ProcessReadData(int bytesRead)
//		{
			// Append data returned from Read to (dynamically sized) memory stream.
			//packet.Position = packet.Length;    // Append (Reads move Position)
			//packet.Write(readBuffer, 0, bytesRead);

			// Process header and payload until we don't have enough data to continue.
//			while (ProcessHeader() && ProcessPacket())
//				continue;
//		}

		/// <summary>
		/// Look for the packet in the data stream.
		/// </summary>
		/// <remarks>
		/// If time for a header to occur in the input stream, see if there are
		/// enough bytes. If there are, convert payload length to an internal
		/// value and store it to determine when we have accumulated the entire
		/// payload.
		/// </remarks>
		/// <returns>Whether to continue processing the data we have read.</returns>
//		private bool ProcessHeader()
//		{
//			// Return variable.
//			bool continueProcessing;
//
//			// If we have accumulated the header for this packet, convert the
//			// length field to internal value. Otherwise, wait for more data to
//			// accumulate.
//			if (!knowSupposedPacketLength)
//			{
//				if (packet.Length >= Consts.LengthOfHeader)
//				{
//					int bytesRead;
//					byte[] lengthData = new byte[Consts.LengthOfLength];
//
//					packet.Position = 0;    // Start reading from buffer start
//					bytesRead = packet.Read(lengthData, 0, lengthData.Length);
//
//					if (bytesRead != Consts.LengthOfLength)
//						Assertion.Check(bytesRead == Consts.LengthOfLength,
//							"SccpProxyProvider: read wrong number of bytes (" + bytesRead.ToString() +
//							") for payload length (" + Consts.LengthOfLength.ToString() + ")");
//
//					if (packet.Position != Consts.LengthOfLength)
//						Assertion.Check(packet.Position == Consts.LengthOfLength,
//							"SccpProxyProvider: positioned incorrectly (" + packet.Position.ToString() +
//							") after recognizing payload length (" +
//							Consts.LengthOfLength.ToString() + ")");
//
//					supposedPacketLength =
//						BitConverter.ToInt32(lengthData, 0) + Consts.LengthOfHeader;
//
//					// Woohoo, now we can start accumulating the payload!
//					knowSupposedPacketLength = true;
//
//					// Continue. Now that we know how big the payload is, we
//					// need to accumulate the payload.
//					continueProcessing = true;
//				}
//				else
//				{
//					// No need to continue because we don't even have the
//					// entire length field yet.
//					continueProcessing = false;
//				}
//			}
//			else
//			{
//				// Continue. Since we have processed the payload-length field, we
//				// are now just accumulating payload data until we have all of it.
//				continueProcessing = true;
//			}
//
//			return continueProcessing;
//		}

		/// <summary>
		/// Look for a complete packet in the data stream.
		/// </summary>
		/// <remarks>
		/// If we have determined the supposed length of the payload, see if
		/// we have accumluated the entire packet yet. If we have, invoke the
		/// consumer callback to process this packet.
		/// </remarks>
		/// <returns>Whether to continue processing the data we have read.</returns>
//		private bool ProcessPacket()
//		{
//			// Return variable.
//			bool continueProcessing;
//
//			// If we know the packet length and have accumulated the entire
//			// packet, hand it off. Otherwise, wait for more data to accumulate.
//			if (knowSupposedPacketLength)
//			{
//				if (packet.Length >= supposedPacketLength)
//				{
//					// (If this is an empty packet--it has no payload--just
//					// ignore it. The client was just sending it to probe the
//					// socket to determine if the socket had failed. Dontcha just
//					// love TCP?)
//					if (supposedPacketLength != 0)
//					{
//						// Read just this packet, including the header (whose
//						// length field we've already read). However, we don't
//						// want to read any data following this packet (the
//						// next message).
//
//						// Position at beginning of packet, including header.
//						packet.Position = 0;
//
//						byte[] thisPacket = new byte[supposedPacketLength];
//						int bytesRead = packet.Read(thisPacket, 0, thisPacket.Length);
//
//						if (bytesRead != supposedPacketLength)
//							Assertion.Check(bytesRead == supposedPacketLength,
//								"SccpProxyProvider: read wrong number of bytes (" + bytesRead.ToString() +
//								") for packet (" + supposedPacketLength.ToString() + ")");
//
//						if (packet.Position != supposedPacketLength)
//							Assertion.Check(packet.Position == supposedPacketLength,
//								"SccpProxyProvider: positioned incorrectly (" + packet.Position.ToString() +
//								") after recognizing payload (" +
//								supposedPacketLength.ToString() + ")");
//
//						// Submit this packet for processing in a separate thread.
//						Message message = CreateMessage(thisPacket,
//							remoteAddress,
//							localAddress);
//						processIncomingPacket.Enqueue(iqpd, message);
//					}
//					else
//					{
//						// Position after header.
//						packet.Position = Consts.LengthOfHeader;
//					}
//
//					// Move any remaining data after the payload, i.e., the
//					// beginning of the next message, to the beginning of the
//					// memory stream.
//					byte[] newPacket = new byte[packet.Length - packet.Position];
//					int remainingBytesRead = packet.Read(newPacket, 0, newPacket.Length);
//					Assertion.Check(remainingBytesRead == newPacket.Length &&
//						packet.Length == packet.Position,
//						"SccpProxyProvider: not all data read");
//
//					// Move buffer to front of memory stream.
//					packet.SetLength(0);
//					packet.Position = 0;
//					packet.Write(newPacket, 0, newPacket.Length);
//					Assertion.Check(packet.Length == newPacket.Length,
//						"SccpProxyProvider: couldn't put back data");
//
//					// Now that we have finished with that packet, we don't
//					// know anything about the following packet, including
//					// length.
//					knowSupposedPacketLength = false;
//
//					// Continue because the next message may have followed the
//					// payload we just processed.
//					continueProcessing = true;
//				}
//				else
//				{
//					// No need to continue because we haven't accumulated the
//					// end of the current packet yet.
//					continueProcessing = false;
//				}
//			}
//			else
//			{
//				// No need to continue because we don't even know how big the
//				// payload is yet.
//				continueProcessing = false;
//			}
//
//			return continueProcessing;
//		}

		/// <summary>
		/// Send message contents on this connection object's socket.
		/// </summary>
		/// <remarks>
		/// This method basically traps any exceptions and "converts" them to a
		/// return value.
		/// </remarks>
		/// <param name="message">Message whose contents is to be sent.</param>
		public bool Send(Message message)
		{
			bool sent;

			try
			{
				//CollectSendInvokationStatistics();

				//provider.LogWrite(TraceLevel.Verbose, "{0} sending {1} bytes", this, b.Length);
				sent = socketSend(message.xContents);
				//provider.LogWrite(TraceLevel.Verbose, "{0} sent {1}", this, sent);

				long delay = message.Delay;

				//provider.LogWrite( TraceLevel.Verbose, "{0} sent {1} delay {2}", this, message, delay );
				//CollectSendStatistics(bytesSent);
			}
			catch
			{
				sent = false;
			}

			if (!sent)
			{
				provider.LogWrite(TraceLevel.Warning,
					"{0}: could not send {1}",
					this, message);
			}

			return sent;
		}

		private bool socketSend(byte[] buf)
		{
			int n = buf.Length;
			if (n == 0)
				return true;

			lock (sendSync)
			{
				int i = 0;
				while (i < n)
				{
					int k = key.Send(buf, i, n-i);
					if (k < 0)
						return false;

					if (k == 0)
					{
						Thread.Sleep( 2 );
						continue;
					}

					i += k;
				}
			}
			return true;
		}

		private object sendSync = new object();

		#region Statistics

		private long totalReceiveInvokations;
		private long totalSendInvokations;
		private long totalReceiveReturns;
		private long totalSendReturns;
		private long totalBytesRead;
		private long totalBytesSent;

		[Conditional("DEBUG")]
		private void InitializeStatistics()
		{
			totalReceiveInvokations = 0;
			totalSendInvokations = 0;
			totalReceiveReturns = 0;
			totalSendReturns = 0;
			totalBytesRead = 0;
			totalBytesSent = 0;
		}

		[Conditional("DEBUG")]
		private void LogConnectionStatistics()
		{
			provider.LogWrite(TraceLevel.Verbose,
				"{0}: Receive: {1}/{2} {3} bytes, Send: {4}/{5} {6} bytes",
				this,
				totalReceiveInvokations, totalReceiveReturns, totalBytesRead,
				totalSendInvokations, totalSendReturns, totalBytesSent);
		}

		[Conditional("DEBUG")]
		private void CollectReceiveInvokationStatistics()
		{
			++totalReceiveInvokations;
		}

		[Conditional("DEBUG")]
		private void CollectReceiveStatistics(int bytesRead)
		{
			++totalReceiveReturns;
			totalBytesRead += bytesRead;
		}

		[Conditional("DEBUG")]
		private void CollectSendInvokationStatistics()
		{
			++totalSendInvokations;
		}

		[Conditional("DEBUG")]
		private void CollectSendStatistics(int bytesSent)
		{
			++totalSendReturns;
			totalBytesSent += bytesSent;
		}

		#endregion
	}
}
