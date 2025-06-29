using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using Metreos.Core.Sockets;
using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.Core.IPC
{
    public abstract class IpcServer : SocketServerThreaded
    {
        public event NewConnectionDelegate OnNewConnection;
        public event CloseConnectionDelegate OnCloseConnection;

        protected Hashtable currentConnections;
        
        protected IpcServer(string taskname, ushort listenPort, bool loopback, TraceLevel logLevel)
            : base(taskname, listenPort, loopback, logLevel)
        {
            currentConnections = new Hashtable();
        }

        protected abstract void OnPayloadReceived(int socketId, string receiveInterface, byte[] payload);

        public override void Stop()
        {
            base.Stop();

            lock(currentConnections.SyncRoot)
            {
                foreach(ConnectionData connData in currentConnections.Values)
                {
                    connData.Dispose();
                }
            }
        }

        protected override void NewConnection(int socketId, string remoteHost)
        { 
            ConnectionData connData = new ConnectionData(socketId);
            
            lock(currentConnections.SyncRoot)
            {
                currentConnections[socketId] = connData; 
            }

            if(OnNewConnection != null)
            {
                OnNewConnection(socketId, remoteHost);
            }
        }

        protected override void ConnectionClosed(int socketId)
        {
            ConnectionData connData = null;
            lock(currentConnections.SyncRoot)
            {
                 connData = currentConnections[socketId] as ConnectionData;
            }

            if(connData != null)
            {
                connData.Dispose();
            }

            if(OnCloseConnection != null)
            {
                OnCloseConnection(socketId);
            }
        }

        protected override void DataReceived(int socketId, string receiveIpAddress, byte[] data, int dataLength)
        {
            ConnectionData connData = currentConnections[socketId] as ConnectionData;

            if(connData == null)
            {
                log.Write(TraceLevel.Error, "Unable to process data due to a loss of inner packet data");
                return;
            }

            // Append data returned from Read to (dynamically sized) memory
            // stream.
            connData.Packet.Position = connData.Packet.Length;    // Append (Reads move Position)
            connData.Packet.Write(data, 0, dataLength);

            // Process length and payload pairs until we don't have enough
            // data to continue.
            while (ProcessLength(connData) && ProcessPayload(connData, receiveIpAddress))
            {
                // nothing else to do
            }
            
            // If the value of the payload-length field in the data stream is
            // unusually large, have session restarted. This assumes that some
            // bug caused us to be out of synch. Hopefully it won't happen
            // again. :-) Otherwise, loop back for another read.    
            if (connData.SupposedPacketLength > IpcConsts.MaxPacketLength)
            {
                log.Write(TraceLevel.Warning, "Insane payload length: {0} > {1}",
                    connData.SupposedPacketLength, IpcConsts.MaxPacketLength);
                ResetState(connData);
            }
        }   

        protected bool ProcessLength(ConnectionData connData)
        {
            log.Write(TraceLevel.Verbose, "IpcServer::ProcessLength");

            // Return variable.
            bool continueProcessing;

            // If we have accumulated the length data for this packet, convert
            // to internal value. Otherwise, wait for more data to accumulate.
            if (!connData.KnowSupposedPacketLength)
            {
                if (connData.Packet.Length >= IpcConsts.LengthOfLength)
                {
                    int bytesRead;
                    byte[] lengthData = new byte[IpcConsts.LengthOfLength];

                    connData.Packet.Position = 0;    // Start reading from buffer start
                    try
                    {
                        bytesRead = connData.Packet.Read(lengthData, 0, lengthData.Length);
                    }
                    catch
                    {
                        continueProcessing = false;
                        return continueProcessing;
                    }

                    // Read right number of bytes for payload length?
                    Debug.Assert(bytesRead == IpcConsts.LengthOfLength);
                    Debug.Assert(connData.Packet.Position == IpcConsts.LengthOfLength);

					// TODO do this right: high bit is ADDITIONAL WORD FLAG,
					// next bit south is XML_FLAG.
					int x = BitConverter.ToInt32(lengthData, 0) & 0xffffff;
                    connData.SupposedPacketLength = x + lengthData.Length;

                    log.Write(TraceLevel.Verbose, 
                        "Received {0} bytes of payload length of {1}",
                        bytesRead, connData.SupposedPacketLength);

                    // Woohoo, now we can start accumulating the payload!
                    connData.KnowSupposedPacketLength = true;

                    // Continue processing message data after we return. Now
                    // that we know how big the payload is, we need to
                    // accumulate the payload.
                    continueProcessing = true;
                }
                else
                {
                    // No need to continue processing the accumulated message
                    // data because we don't even have the entire length field
                    // yet.
                    continueProcessing = false;
                }
            }
            else
            {
                // Continue processing message data. Since we have processed
                // the payload-length field, we are now just accumulating
                // payload data until we have all of it.
                continueProcessing = true;
            }

            return continueProcessing;
        }

        /// <summary>
        ///  Look for a complete payload in the data stream.
        ///
        /// If we have determined the supposed length of the payload, see if
        /// we have accumluated the entire payload yet. If we have, invoke the
        /// consumer callback to process this payload.
        /// Returns whether to continue processing the data we have read.
        ///
        /// </summary>
        protected bool ProcessPayload(ConnectionData connData, string receiveInterface)
        {
            log.Write(TraceLevel.Verbose, "IpcServer::ProcessPayload");

            // Return variable.
            bool continueProcessing;

            // If we know the packet length and have accumulated the entire
            // packet, hand it off. Otherwise, wait for more data to accumulate.
            if (connData.KnowSupposedPacketLength)
            {
                if (connData.Packet.Length >= connData.SupposedPacketLength)
                {
                    // Must position at the payload, after the length
                    connData.Packet.Position = IpcConsts.LengthOfLength;

                    // (If this is an empty packet--it has no payload--just
                    // ignore it. The client was just sending it to probe the
                    // socket to determine if the socket had failed. Dontcha just
                    // love TCP?)
                    if (SupposedPayloadLength(connData) != 0)
                    {
                        byte[] payload = new byte[SupposedPayloadLength(connData)];

                        // Read just the payload. We should have already read past
                        // the length field that precedes the payload, and we don't
                        // want to read any data following this payload (the next
                        // packet).

                        int bytesRead;
                        try
                        {
                            bytesRead = connData.Packet.Read(payload, 0, SupposedPayloadLength(connData));
                        }
                        catch
                        {
                            continueProcessing = false;
                            return continueProcessing;
                        }

                        // Correct number of bytes for packet, and are we positioned correctly?
                        Debug.Assert(bytesRead == SupposedPayloadLength(connData));
                        Debug.Assert(connData.Packet.Position == IpcConsts.LengthOfLength + SupposedPayloadLength(connData));

                        try
                        {
                            // Pass this into the child class
                            OnPayloadReceived(connData.SocketId, receiveInterface, payload);
                        }
                        catch
                        {
                            // Reset the state of the IPC system to reading the length bit, and stop processing this message
                            ResetState(connData);
                            return false;
                        }

                        payload = null;
                    }

                    // Move any remaining data after the payload, i.e., the
                    // beginning of the next message, to the beginning of the
                    // memory stream.
                    long newPacketLength = connData.Packet.Length - connData.Packet.Position;
                    byte[] newPacket = new byte[newPacketLength];
                    int remainingBytesRead = connData.Packet.Read(newPacket, 0, (int)newPacketLength);
                    Debug.Assert(remainingBytesRead == newPacketLength &&
                        connData.Packet.Length == connData.Packet.Position);  // Not all data read

                    connData.Packet.SetLength(0);
                    connData.Packet.Position = 0;  // Indicate no data to be read from stream.
                    connData.Packet.Write(newPacket, 0, (int)newPacketLength);
                    Debug.Assert(connData.Packet.Length == newPacketLength); // Couldn't put back data
                    newPacket = null;

                    // Now that we have finished with that packet, we don't
                    // know anything about the following packet, including
                    // length.
                    connData.KnowSupposedPacketLength = false;

                    // Continue processing message data after we return because
                    // the next message may have followed the payload we just
                    // processed.
                    continueProcessing = true;
                }
                else
                {
                    // No need to continue processing the accumulated message
                    // data because we haven't accumulated the end of the
                    // current packet yet.
                    continueProcessing = false;
                }
            }
            else
            {
                // No need to continue processing the accumulated message
                // data because we don't even know how big the payload is yet.
                continueProcessing = false;
            }

            return continueProcessing;
        }

        protected void ResetState(ConnectionData connData)
        {
            connData.Packet.SetLength(0);
            connData.Packet.Position = 0;
            connData.KnowSupposedPacketLength = false;
        }

        /// <summary>
        /// Send a flatmap message to the client.
        ///
        /// messageType - Message type.
        /// flatmap - Message body in the form of a list of parameters.
        /// 
        /// Returns whether the write succeeded.
        /// </summary>
        protected bool Write(int socketId, byte[] payload)
        {
            log.Write(TraceLevel.Verbose, "IpcServer::Write");
            
            // Write length and payload to TCP data stream.
            // (Length and payload can be sent in separate Writes because TCP
            // is a streaming protocol where datagram boundaries don't mean
            // anything at this level.)
            bool written = true;
            try
            {
                byte[] lengthData = BitConverter.GetBytes(payload.Length);
                SendData(socketId, lengthData, false);
                SendData(socketId, payload, false);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to send data. Exception is: {0}", Exceptions.FormatException(e));
                written = false;
            }

            return written;
        }

        protected bool WriteToAllSockets(byte[] payload)
        {
            bool written = true;
            try
            {
                byte[] lengthData = BitConverter.GetBytes(payload.Length);
                SendDataToAllSockets(lengthData);
                SendDataToAllSockets(payload);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Unable to send data. Exception is: {0}", Exceptions.FormatException(e));
                written = false;
            }

            return written;
        }

        /// <summary>
        /// The length of the payload as extracted from the data stream.
        /// Undefined value when !knowSupposedPacketLength. Note that this
        /// property is not an accessor for supposedPacketLength--it returns
        /// the *payload* length.
        /// </summary>
        private int SupposedPayloadLength(ConnectionData connData)
        {
            // Assure that payload length is defined.
            if(connData.SupposedPacketLength < IpcConsts.LengthOfLength)
                return 0;

            return connData.SupposedPacketLength - IpcConsts.LengthOfLength;
        }


        protected class ConnectionData : IDisposable
        {
            public MemoryStream Packet { get { return packet; } set { packet = value; } }
            public bool KnowSupposedPacketLength { get { return knowSupposedPacketLength; } set { knowSupposedPacketLength = value; } }
            public int SupposedPacketLength { get { return supposedPacketLength; } set { supposedPacketLength = value; } }
            public int SocketId { get { return socketId; } set { socketId = value; } }

            protected MemoryStream packet;
            protected int socketId;
            protected int supposedPacketLength;
            protected bool knowSupposedPacketLength;

            public ConnectionData(int socketId)
            {
                this.socketId = socketId;
                this.packet = new MemoryStream();
            }

            public void Dispose()
            {
                if(packet != null)
                {
                    packet.Close();
                }
            }
        }
    }
}