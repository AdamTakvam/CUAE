using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Type of delegate for processing an SCCP message once the deblocker
	/// has isolated it in the byte stream.
	/// </summary>
	internal delegate void ProcessMessageDelegate(byte[] buffer);

	/// <summary>
	/// Isolates discrete SCCP messages within a stream of bytes, where a
	/// variable number of bytes are made available to it at a time.
	/// </summary>
	internal class Deblocker
	{
		/// <summary>
		/// Constructs a deblocker.
		/// </summary>
		/// <param name="processMessage">Delegate for processing an SCCP
		/// message once it has been isolated in the byte stream.</param>
		/// <param name="log">Object through which log entries are generated.</param>
		internal Deblocker(ProcessMessageDelegate processMessage, LogWriter log)
		{
			packet = new MemoryStream();
			knowSupposedPacketLength = false;
			this.processMessage = processMessage;
			this.log = log;

			// Create object just so we have a reference on which to lock.
			syncRoot = new object();
		}

		/// <summary>
		/// Constants referenced within this class.
		/// </summary>
		private abstract class Const
		{
			/// <summary>
			/// Length field in packet header is a 32-bit integer.
			/// </summary>
			/// <remarks>
			/// This field is necessary because TCP is a streaming
			/// protocol--packet boundaries are not significant.
			/// </remarks>
			internal const int LengthOfLength = 4;

			/// <summary>
			/// There is a 32-bit reserved field in the header after the length
			/// field that isn't used for anything.
			/// </summary>
			internal const int LengthOfReserved = 4;

			/// <summary>
			/// Packet header length.
			/// </summary>
			/// <remarks>
			/// Payload immediately follows header.
			/// </remarks>
			internal const int LengthOfHeader = LengthOfLength + LengthOfReserved;
		}

		/// <summary>
		/// Object through which log entries are generated.
		/// </summary>
		/// <remarks>Access to this object does not need to be controlled
		/// because it is not modified after construction.</remarks>
		private readonly LogWriter log;

		/// <summary>
		/// Delegate for processing an SCCP message once it has been isolated
		/// in the byte stream.
		/// </summary>
		private readonly ProcessMessageDelegate processMessage;

		/// <summary>
		/// Variable-length buffer for incoming data.
		/// </summary>
		private readonly MemoryStream packet;

		/// <summary>
		/// Do we know the length of the next packet?
		/// </summary>
		/// <remarks>
		/// We don't know how big packet is until we have payload length.
		/// </remarks>
		private bool knowSupposedPacketLength;

		/// <summary>
		/// The length of the packet which is based on the payload length as
		/// extracted from the data stream.
		/// </summary>
		/// <remarks>
		/// Undefined value when !knowSupposedPacketLength.
		/// </remarks>
		private int supposedPacketLength;

		/// <summary>
		/// The broken flag indicates that an exception thrown during processing
		/// has destroyed the state information about the stream. No further
		/// processing is allowed.
		/// </summary>
		private bool broken = false;

		/// <summary>
		/// Lock on this otherwise useless object instead of "this."
		/// </summary>
		/// <remarks>Adam said that it's faster to lock on an inner object
		/// rather than "this" or an outer object.</remarks>
		private readonly object syncRoot;

		/// <summary>
		/// Process the data in the buffer.
		/// </summary>
		/// <remarks>
		/// This could involve data previously read into the buffer; some data
		/// may be left in the buffer after processing (this would be the
		/// first part of a subsequent packet).
		/// </remarks>
		/// <param name="buffer">Buffer containing data to process.</param>
		/// <param name="bytesRead">Number of bytes in buffer. Could be zero.</param>
		internal void ProcessReadData(byte[] buffer, int bytesRead)
		{
			// (Once upon a time saw a null-reference exception. These Asserts
			// are an attempt to localize problem. Haven't seen since, though.)
			Debug.Assert(log != null, "SccpStack: missing log");
			Debug.Assert(buffer != null, "SccpStack: missing buffer");
			Debug.Assert(processMessage != null, "SccpStack: missing process-message delegate");
			Debug.Assert(!broken, "SccpStack: Deblocker previously triggered exception");
			Debug.Assert(syncRoot != null, "SccpStack: missing syncRoot");
			Debug.Assert(packet != null, "SccpStack: missing packet");

			try
			{
				// In case invoked by multiple threads, only process one
				// datagram at a time.
				for (int i = 1; !Monitor.TryEnter(syncRoot, SccpStack.LockPollMs); ++i)
				{
					log.Write(TraceLevel.Warning,
						"Dbl: waited {0} times for syncRoot lock within ProcessReadData()", i);
				}
				long lockAcquiredNs = HPTimer.Now();
				try
				{
					// Append data returned from Read to (dynamically sized)
					// memory stream.
					try
					{
						packet.Position = packet.Length;    // Append (Reads move Position)
					}
					catch (Exception e)
					{
						log.Write(TraceLevel.Error,
							"Dbl: set-Position failed; packet.Capacity: {0}, .Position: {1}, .Length: {2}, {3}",
							packet.Capacity, packet.Position, packet.Length, e);
					}

					try
					{
						packet.Write(buffer, 0, bytesRead);
					}
					catch (Exception e)
					{
						log.Write(TraceLevel.Error,
							"Dbl: Write() failed; packet.Capacity: {0}, .Position: {1}, .Length: {2}, buffer.Length: {3}, bytesRead: {4}, {5}",
							packet.Capacity, packet.Position, packet.Length, buffer.Length, bytesRead, e);
					}

					// Process header and payload until we don't have enough data to continue.
                    // Let the exceptions fly
					int i = 0;
					while (ProcessHeader() && ProcessPacket())
					{
						++i;
					}

					if (i > 20)	// (Arbitrarily large number.)
					{
						log.Write(TraceLevel.Warning,
							"Dbl: suspiciously large number of messages ({0}) processed from buffer; " +
							"this datagram: {1}, next supposed message length: {2}",
							i, bytesRead,
							knowSupposedPacketLength ? supposedPacketLength.ToString() : "?");
					}

					if (packet.Length > 2000)	// (Arbitrarily large number.)
					{
						log.Write(TraceLevel.Warning,
							"Dbl: too many bytes ({0}) accumulating in buffer",
							packet.Length);
					}
				}
				finally
				{
					Monitor.Exit(syncRoot);
				}
				long lockHeldMs = (HPTimer.Now() - lockAcquiredNs) / 1000 / 1000;
				if (lockHeldMs > SccpStack.LockPollMs)
				{
					log.Write(TraceLevel.Warning,
						"Dbl: lock held long time ({0}ms) within ProcessReadData()",
						lockHeldMs);
				}
			}
			// (In case thread gets nuked while waiting on lock.)
			catch (ThreadInterruptedException e)
			{
				log.Write(TraceLevel.Error, "Dbl: {0}", e);
			}
			catch (Exception e)
			{
				log.Write(TraceLevel.Error, "Dbl: {0}", e);
			}
		}

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
		private bool ProcessHeader()
		{
			// Return variable.
			bool continueProcessing;

			// If we have accumulated the header for this packet, convert the
			// length field to internal value. Otherwise, wait for more data to
			// accumulate.
			if (!knowSupposedPacketLength)
			{
				if (packet.Length >= Const.LengthOfHeader)
				{
					int bytesRead;
					byte[] lengthData = new byte[Const.LengthOfLength];

					packet.Position = 0;    // Start reading from buffer start
					bytesRead = packet.Read(lengthData, 0, lengthData.Length);

					if (bytesRead != Const.LengthOfLength)
						throw new IOException(
							"SccpStack: read wrong number of bytes (" + bytesRead.ToString() +
							") for payload length (" + Const.LengthOfLength.ToString() + ")");
					
					if (packet.Position != Const.LengthOfLength)
						throw new IOException(
							"SccpStack: positioned incorrectly (" + packet.Position.ToString() +
							") after recognizing payload length (" +
							Const.LengthOfLength.ToString() + ")");

					int len = BitConverter.ToInt32(lengthData, 0);
					if (len < 0 || len > 2000)
						throw new IOException("ProcessHeader: packet len "+len+" seems unreasonable");
					supposedPacketLength = Const.LengthOfHeader + len;

					// Woohoo, now we can start accumulating the payload!
					knowSupposedPacketLength = true;

					// Continue. Now that we know how big the payload is, we
					// need to accumulate the payload.
					continueProcessing = true;
				}
				else
				{
					// No need to continue because we don't even have the
					// entire length field yet.
					continueProcessing = false;
				}
			}
			else
			{
				// Continue. Since we have processed the payload-length field, we
				// are now just accumulating payload data until we have all of it.
				continueProcessing = true;
			}

			return continueProcessing;
		}

		/// <summary>
		/// Look for a complete packet in the data stream.
		/// </summary>
		/// <remarks>
		/// If we have determined the supposed length of the payload, see if
		/// we have accumluated the entire packet yet. If we have, invoke the
		/// consumer callback to process this packet.
		/// </remarks>
		/// <returns>Whether to continue processing the data we have read.</returns>
		private bool ProcessPacket()
		{
			// Return variable.
			bool continueProcessing;

			// If we know the packet length and have accumulated the entire
			// packet, hand it off. Otherwise, wait for more data to accumulate.
			if (knowSupposedPacketLength)
			{
				if (packet.Length >= supposedPacketLength)
				{
					// (If this is an empty packet--it has no payload--just
					// ignore it. The client was just sending it to probe the
					// socket to determine if the socket had failed. Dontcha just
					// love TCP?)
					if (supposedPacketLength > Const.LengthOfHeader)
					{
						// Read just this packet, including the header (whose
						// length field we've already read). However, we don't
						// want to read any data following this packet (the
						// next message).

						// Position at beginning of packet, including header.
						packet.Position = 0;

						byte[] thisPacket = new byte[supposedPacketLength];
						int bytesRead = packet.Read(thisPacket, 0, thisPacket.Length);

						if (bytesRead != supposedPacketLength)
							throw new IOException(
								"SccpStack: read wrong number of bytes (" + bytesRead.ToString() +
								") for packet (" + supposedPacketLength.ToString() + ")");

						if (packet.Position != supposedPacketLength)
							throw new IOException(
								"SccpStack: positioned incorrectly (" + packet.Position.ToString() +
								") after recognizing payload (" +
								supposedPacketLength.ToString() + ")");

						processMessage(thisPacket);
					}
					else
					{
						if (supposedPacketLength != Const.LengthOfHeader)
							throw new IOException(
								"supposedPacketLength ("+supposedPacketLength+") != Const.LengthOfHeader ("+Const.LengthOfHeader+")");
						// Position after header.
						packet.Position = Const.LengthOfHeader;
					}

					// Move any remaining data after the payload, i.e., the
					// beginning of the next message, to the beginning of the
					// memory stream.
					byte[] newPacket = new byte[packet.Length - packet.Position];
					int remainingBytesRead = packet.Read(newPacket, 0, newPacket.Length);
					if (remainingBytesRead != newPacket.Length ||
							packet.Length != packet.Position)
						throw new IOException("SccpStack: not all data read");

					// Move buffer to front of memory stream.
					packet.SetLength(0);
					packet.Position = 0;
					packet.Write(newPacket, 0, newPacket.Length);
					if (packet.Length != newPacket.Length)
						throw new IOException("SccpStack: couldn't put back data");

					// Now that we have finished with that packet, we don't
					// know anything about the following packet, including
					// length.
					knowSupposedPacketLength = false;
					supposedPacketLength = -1;

					// Continue because the next message may have followed the
					// payload we just processed.
					continueProcessing = true;
				}
				else
				{
					// No need to continue because we haven't accumulated the
					// end of the current packet yet.
					continueProcessing = false;
				}
			}
			else
			{
				// No need to continue because we don't even know how big the
				// payload is yet.
				continueProcessing = false;
			}

			return continueProcessing;
		}
	}
}
