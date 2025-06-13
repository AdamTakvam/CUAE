using System;
using System.Diagnostics;

using Metreos.Messaging;
using Metreos.Messaging.Ipc;

using LoggingCore;

namespace ServerProcess
{
    enum LogClientState
    {
        IDLE,
        PENDING_MSG,
        MSG_COMPLETE
    }

    class LogClientSession
    {
        private const int LOG_MSG_HEADER_LENGTH = 8;
        private const int MAX_LOG_MSG_SIZE      = 131072;

        public LogClientState state;

        public long pendingMsgLength  = 0;

        public long totalBytesReceived = 0;

        public byte[] logHeaderData = new byte[LOG_MSG_HEADER_LENGTH];
        public byte[] logMessageData;

        public void AppendMessageData(byte[] newMsgData, int dataLength)
        {
            totalBytesReceived += dataLength;

            if(state == LogClientState.IDLE)
            {
                long copyLength = dataLength >= LOG_MSG_HEADER_LENGTH ? LOG_MSG_HEADER_LENGTH : dataLength;
                Debug.Assert(copyLength <= logHeaderData.Length);

                long copyIndex = totalBytesReceived - dataLength;
                Debug.Assert(copyIndex >= 0 && copyIndex < logHeaderData.Length);

                Array.Copy(newMsgData, 0, logHeaderData, copyIndex, copyLength);

                ProcessHeader();

                if(totalBytesReceived > LOG_MSG_HEADER_LENGTH) 
                {
                    ProcessMessage(newMsgData, dataLength, LOG_MSG_HEADER_LENGTH);
                }
                
            }
            else
            {
                ProcessMessage(newMsgData, dataLength, 0);
            }

            if(state == LogClientState.MSG_COMPLETE)
            {
                LogMessage msg = LogMessage.FromByteArray(logMessageData);
                Console.WriteLine("LogMessageComplete: {0} - {1}", msg.msgCategory, msg.msgData);

                Reset();
            }
        }

        private void Reset()
        {
            state = LogClientState.IDLE;

            pendingMsgLength = 0;
            totalBytesReceived = 0;

            logHeaderData.Initialize();
            logMessageData.Initialize();
        }

        private void ProcessHeader()
        {
            Debug.Assert(state == LogClientState.IDLE);

            
            if(totalBytesReceived >= LOG_MSG_HEADER_LENGTH)
            {
                pendingMsgLength = BitConverter.ToInt64(logHeaderData, 0);

                if(pendingMsgLength > MAX_LOG_MSG_SIZE)
                {
                    Console.WriteLine("Expected message is too large, ignoring");
                    
                    // What to do here?  We probably want to issue a client reset at
                    // this point in case we're out of sync.
                }
                else
                {
                    logMessageData = new byte[pendingMsgLength];
                    state = LogClientState.PENDING_MSG;
                    Console.WriteLine("Header detected: {0}", pendingMsgLength);
                }
            }
        }

        private void ProcessMessage(byte[] newMsgData, int dataLength, int srcIndex)
        {
            Debug.Assert(state == LogClientState.PENDING_MSG);

            long copyLength = dataLength - srcIndex;
            Debug.Assert(copyLength <= logMessageData.Length);

            long destIndex = totalBytesReceived - LOG_MSG_HEADER_LENGTH - dataLength;
            Debug.Assert(destIndex >= 0 && destIndex < logMessageData.Length);

            Array.Copy(newMsgData, srcIndex, logMessageData, destIndex, copyLength);

            if((totalBytesReceived - LOG_MSG_HEADER_LENGTH) >= pendingMsgLength)
            {
                state = LogClientState.MSG_COMPLETE;
            }
        }
    }

}
