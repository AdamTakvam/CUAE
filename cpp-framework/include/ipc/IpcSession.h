#ifndef IPC_BASE_H
#define IPC_BASE_H

#include "cpp-core.h"

#include "ace/OS.h"   
#include "ace/ACE.h"
#include "ace/Log_Msg.h"
#include "ace/INET_Addr.h"
#include "ace/SOCK_Acceptor.h"   
#include "ace/Thread_Manager.h"

#include "ipc/MemoryStream.h"

namespace Metreos
{

namespace IPC
{

class IpcConsumerInterface;

class CPPCORE_API IpcSession
{
public:
    IpcSession(IpcConsumerInterface* consumer, ACE_SOCK_Stream stream, 
        ACE_INET_Addr addr, int sessionId);
    virtual ~IpcSession();

    bool Start();
    void Stop();

    bool Write(const char* data, const size_t length);

    ACE_INET_Addr GetPeerAddr() const;

protected:
    bool WriteToStream(const ACE_TCHAR* data, const size_t length);
    bool ProcessPayload();
    bool ProcessLength();

#ifdef WIN_THREAD
	DWORD WINAPI ReadThreadFunc();
    static DWORD WINAPI ReadThreadThunkFunc(void* state);
#else
    // Read thread that reads data from a socket.
	ACE_THR_FUNC_RETURN ReadThreadFunc();
    // Static method that in turn calls back into the parent object.
    static ACE_THR_FUNC_RETURN ReadThreadThunkFunc(void* state);
#endif

    IpcConsumerInterface* callbacks;

    ACE_SOCK_Stream peerStream;
    ACE_INET_Addr   peerAddr;
#ifdef WIN_THREAD
	HANDLE			readThreadHandle;
	HANDLE			winWriteMutex;
#else
    ACE_thread_t    readThreadHandle;
#endif

    // Unique value managed by the consumer that we pass back in callback
    // (not sent to client).
    int id;

    // Result buffer for incoming data.
    MemoryStream packet;

    // Do we know the length of the next packet?
    // We don't know how big packet is until we have payload length.
    bool knowSupposedPacketLength;

    // The length of the packet which is based on the payload length as
    // extracted from the data stream.
    // Undefined value when !knowSupposedPacketLength.
    size_t supposedPacketLength;

    // The length of the payload as extracted from the data stream.
    // Undefined value when !knowSupposedPacketLength. Note that this
    // property is not an accessor for supposedPacketLength--it returns
    // the *payload* length.
    size_t SupposedPayloadLength()
    {
        // Assure that payload length is defined.
        ACE_ASSERT(supposedPacketLength >= LengthLength);

        return supposedPacketLength - LengthLength;
    }

    // Serializes access to TCP socket when writing length and then
    // payload datagrams so that they are not interleaved from different
    // threads.
    ACE_Thread_Mutex writeMutex;

    /*********************************************
     * Configuration Parameters
     */

    // Arbitrarily sized read-buffer length--how big of a chunk we read at
    // a time.
    const static size_t ReadBufferLength = 256;

    // Arbitrarily large limit to payload size. Length field could express
    // larger payload, but we have decided that anything larger is a
    // mistake and must be the result of a bug somewhere.
    const static int SanityCheckMaximumLength = 64 * 1024;

    // Maximum time to try a write.
    const static int WriteTimeoutSecs = 7;

    // How often we retry on send failure.
    const static u_int WriteSleepSecs = 1;

    // Length of the length field that immediately precedes the payload.
    // It is the length of just the payload, not of the length field +
    // payload.
    const static size_t LengthLength = 4;
};

} // namespace IPC

} // namespace Metreos

#endif // IPC_BASE_H