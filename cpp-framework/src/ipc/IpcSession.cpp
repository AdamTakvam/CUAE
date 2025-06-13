#include "stdafx.h"
#include "ipc/IpcSession.h"
#include "ipc/IpcConsumerInterface.h"

using namespace Metreos;
using namespace Metreos::IPC;

IpcSession::IpcSession(IpcConsumerInterface* consumer, ACE_SOCK_Stream stream, 
                       ACE_INET_Addr addr, int sessionId) :
    callbacks(consumer),
    peerStream(stream),
    peerAddr(addr),
    knowSupposedPacketLength(false),
    supposedPacketLength(0),
    packet(),
    writeMutex(),
    id(sessionId)
{
    ACE_ASSERT(callbacks != NULL);

    // Disable nagle
    long nagle = 1;
    peerStream.set_option(IPPROTO_TCP, TCP_NODELAY, &nagle, sizeof(nagle));
}

IpcSession::~IpcSession()
{
}

bool IpcSession::Start()
{
#ifdef WIN_THREAD
	DWORD dwThreadId = 0;
    readThreadHandle = CreateThread( 
									NULL,                         
									0,                           
									ReadThreadThunkFunc,         
									this,                
									0,                   
									&dwThreadId);        
	if (readThreadHandle == NULL) 
		return false;

	winWriteMutex = CreateMutex(NULL, FALSE, "WIN_THREAD_WRITE");
#else
    int returnValue = ACE_Thread_Manager::instance()->spawn(
        ReadThreadThunkFunc, this, THR_NEW_LWP | THR_JOINABLE,
        &readThreadHandle);
    if(returnValue < 0) return false;
#endif
    return true;
}

void IpcSession::Stop()
{
#ifdef WIN_THREAD
	peerStream.close();
	if (readThreadHandle != NULL)
		CloseHandle(readThreadHandle);

	if (winWriteMutex != NULL)
		CloseHandle(winWriteMutex);
#else
    ACE_Thread_Manager *tm = ACE_Thread_Manager::instance();
    ACE_ASSERT(tm != NULL);

    if(tm->testresume(readThreadHandle) == 1)
    {
        // Close the stream we are using to talk to our remote
        // party.  This will cause the blocking recv() operation
        // in the read thread to terminate with <= 0 bytes read
        // thus causing the read thread to exit.
        peerStream.close();
        tm->join(readThreadHandle);
    }
#endif
}

bool IpcSession::Write(const char* data, size_t length)
{
    ACE_ASSERT(data   != NULL);
    ACE_ASSERT(length != -1);

    bool written = false;   // Not written yet.

#ifdef WIN_THREAD
	DWORD dwWaitResult = WaitForSingleObject(winWriteMutex, INFINITE);
	if (dwWaitResult == WAIT_OBJECT_0)
	{
        written = WriteToStream((ACE_TCHAR *)&length, sizeof(length)) && WriteToStream(data, length);
	}
	ACE_Time_Value tv25	(0, 25*1000);		// 25 ms
	ACE_OS::sleep(tv25);
	ReleaseMutex(winWriteMutex);
#else
    // Write the length and data out to the socket.
    ACE_GUARD_ACTION(ACE_Thread_Mutex, guard, writeMutex,
        written = WriteToStream((ACE_TCHAR *)&length, sizeof(length)) && WriteToStream(data, length),
        written = false);
	guard.release();
#endif

    return written;
}

ACE_INET_Addr IpcSession::GetPeerAddr() const
{
    return peerAddr;
}

// Write a raw buffer to the stream.
//
// data - Array of bytes to write to the stream.
// length - Number of bytes in array to write to the stream.
//
// Return whether the write succeeded.
bool IpcSession::WriteToStream(const ACE_TCHAR* data, const size_t length)
{
    ACE_ASSERT(data   != NULL);
    ACE_ASSERT(length != -1);

    bool written = false;   // Not written yet.

    // Start sending at beginning of data. This is maintained in case of
    // partial sends.
    size_t startSendAt = 0;

    while (true)
    {
        ssize_t bytesSent = peerStream.send(&data[startSendAt], length - startSendAt, 0);

        if (bytesSent == -1)                            // Error.
        {
            //ACE_DEBUG((LM_ERROR,
            //    CORE_DP "failed (%u, %s) to send %u bytes\n",
            //    errno, ACE_OS::strerror(errno), length - startSendAt));

            callbacks->OnSocketFailure(errno, id);
            written = false;
            break;
        }
        else
        {
            if ((size_t)bytesSent < length - startSendAt)  // Not all sent.
            {
                //ACE_DEBUG((LM_DEBUG, CORE_DP "sent %u of %u bytes\n",
                //    (size_t)bytesSent, length - startSendAt));
                
                startSendAt += bytesSent;               // Send again, starting where we left off.
            }
            else
            {
                if (bytesSent == length - startSendAt)  // Sent everything.
                {
                    //ACE_DEBUG((LM_DEBUG, CORE_DP "sent %u bytes\n",
                    //   (size_t)bytesSent));
                }
                else                                    // Sent too much.
                {
                    ACE_DEBUG((LM_ERROR, CORE_DP
                        "sent %d of %d bytes (too many)\n",
                         bytesSent, length - startSendAt));
                }

                // Return true and leave loop.
                written = true;
                break;
            }
        }
    }
    
    return written;
}

#ifdef WIN_THREAD
DWORD WINAPI IpcSession::ReadThreadFunc()
#else
ACE_THR_FUNC_RETURN IpcSession::ReadThreadFunc()
#endif
{
    ACE_DEBUG((LM_DEBUG, CORE_DP "IPC listener %d start\n", id));

    while(1)
    {
        char readBuffer[ReadBufferLength];

        // Blocking receive operation.
        ssize_t bytesRead = peerStream.recv(readBuffer, sizeof(readBuffer));

        // 0 bytes or less from the read indicates
        // closure of socket either by remote party
        // or at the request of our user and may 
        // also indicate some type of socket error.
        if(bytesRead <= 0) 
            break;

        //ACE_DEBUG((LM_DEBUG, CORE_DP "read %d bytes\n", bytesRead));

        // Append data returned from Read to (dynamically sized) 
        // memory stream.
        packet.Position(packet.Length());    // Append (Reads move Position)
        packet.Write(readBuffer, 0, bytesRead);

        //ACE_DEBUG((LM_DEBUG, CORE_DP "accumulated %d bytes\n", bytesRead));

        // Process length and payload pairs until we don't have enough
        // data to continue.
        while (ProcessLength() && ProcessPayload())
            continue;

        // If the value of the payload-length field in the data stream is
        // unusually large, have session restarted. This assumes that some
        // bug caused us to be out of synch. Hopefully it won't happen
        // again. :-) Otherwise, loop back for another read.
        if (supposedPacketLength > SanityCheckMaximumLength)
        {
            ACE_DEBUG((LM_ERROR, CORE_DP "excessive IPC payload length: %u > %d\n",
                supposedPacketLength, SanityCheckMaximumLength));

            peerStream.close();

            break;
        }
    }

    ACE_DEBUG((LM_DEBUG, CORE_DP "IPC listener %d exit\n", id));

    ACE_ASSERT(callbacks);
    callbacks->OnSessionStop(id);

    return 0;
}

#ifdef WIN_THREAD
DWORD WINAPI IpcSession::ReadThreadThunkFunc(void* state)
#else
ACE_THR_FUNC_RETURN IpcSession::ReadThreadThunkFunc(void* state)
#endif
{
    ACE_ASSERT(state);
    IpcSession* owner = static_cast<IpcSession*>(state);

    ACE_ASSERT(owner);
    return owner->ReadThreadFunc();
}

// Look for a length field in the data stream.
//
// If time for a payload-length field to occur in the input stream,
// see if there are enough bytes. If there are, convert it to an
// internal value and store it to determine when we have accumulated
// the entire payload.
//
// Returns whether to continue processing the data we have read.
bool IpcSession::ProcessLength()
{
    // Return variable.
    bool continueProcessing;

    // If we have accumulated the length data for this packet, convert
    // to internal value. Otherwise, wait for more data to accumulate.
    if (knowSupposedPacketLength == false)
    {
        if (packet.Length() >= LengthLength)
        {
            int bytesRead;
            char lengthData[LengthLength];

            packet.Position(0);    // Start reading from buffer start
            bytesRead = packet.Read(lengthData, 0, sizeof lengthData);

            // Read right number of bytes for payload length?
            ACE_ASSERT(bytesRead == LengthLength);
            ACE_ASSERT(packet.Position() == LengthLength);

            supposedPacketLength = *(int *)lengthData + sizeof(lengthData);

            //ACE_DEBUG((LM_DEBUG, CORE_DP
            //    "received %u bytes of payload length of %u\n",
            //    bytesRead, supposedPacketLength));

            // Woohoo, now we can start accumulating the payload!
            knowSupposedPacketLength = true;

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

// Look for a complete payload in the data stream.
//
// If we have determined the supposed length of the payload, see if
// we have accumluated the entire payload yet. If we have, invoke the
// consumer callback to process this payload.
//
// Returns whether to continue processing the data we have read.
bool IpcSession::ProcessPayload()
{
    // Return variable.
    bool continueProcessing;

    // If we know the packet length and have accumulated the entire
    // packet, hand it off. Otherwise, wait for more data to accumulate.
    if (knowSupposedPacketLength)
    {
        if (packet.Length() >= supposedPacketLength)
        {
            // Must position at the payload, after the length
            packet.Position(LengthLength);

            // (If this is an empty packet--it has no payload--just
            // ignore it. The client was just sending it to probe the
            // socket to determine if the socket had failed. Dontcha just
            // love TCP?)
            if (SupposedPayloadLength() != 0)
            {
                // Read just the payload, skipping the length field (which
                // we've already read). We should have already read past
                // the length field that precedes the payload, and we don't
                // want to read any data following this payload (the next
                // packet).

                char* payload = new char[SupposedPayloadLength()];
                int bytesRead = packet.Read(payload, 0, SupposedPayloadLength());

                // Correct number of bytes for packet, and are we positioned
                // correctly?
                ACE_ASSERT(bytesRead == SupposedPayloadLength());
                ACE_ASSERT(packet.Position() ==
                    LengthLength + SupposedPayloadLength());

                callbacks->OnIncomingData(payload, bytesRead, id);

                delete[] payload;
            }

            // Move any remaining data after the payload, i.e., the
            // beginning of the next message, to the beginning of the
            // memory stream.
            size_t newPacketLength = packet.Length() - packet.Position();
            char *newPacket = new char[newPacketLength];
            int bytesRead = packet.Read(newPacket, 0, newPacketLength);
            ACE_ASSERT(bytesRead == newPacketLength &&
                packet.Length() == packet.Position());  // Not all data read

            // Move buffer to front of memory stream.
            packet.SetLength(0);    // Indicate no data written to stream.
            packet.Position(0);     // Indicate no data to be read from stream.
            packet.Write(newPacket, 0, newPacketLength);
            ACE_ASSERT(packet.Length() == newPacketLength); // Couldn't put back data
            delete[] newPacket;

            // Now that we have finished with that packet, we don't know
            // anything about the following packet, including length.
            knowSupposedPacketLength = false;

            // Continue processing message data after we return because the
            // next message may have followed the payload we just processed.
            continueProcessing = true;
        }
        else
        {
            // No need to continue processing the accumulated message data
            // because we haven't accumulated the end of the current packet yet.
            continueProcessing = false;
        }
    }
    else
    {
        // No need to continue processing the accumulated message data because
        // we don't even know how big the payload is yet.
        continueProcessing = false;
    }

    return continueProcessing;
}