// PCapRTPWriter.h

/**
* RTP Writer writes RTP payload to a local file.  The class may support static or dynamic 
* data conversion to support popular audio format.
*/ 

#ifndef PCAP_RTPWRITER_H
#define PCAP_RTPWRITER_H

#ifdef PCAP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "PCapCommon.h" 

#define MAX_BUFFER_SIZE     4096        // 4K
#define MAX_FILE_PATH       256         

namespace Metreos
{

namespace PCap
{

class PCapRTPWriter
{
public:
    PCapRTPWriter();
    virtual ~PCapRTPWriter();

    bool CreateRTPFile(const char* folder, const u_int callIdentifier, const u_int dport);
    bool WritePacket(const void* payload, int len);

protected:
    bool WriteInternalPacketBuffer();
    void ResetInternalPacketBuffer();
    bool IsFileExist();

private:
    char szFilePath[MAX_FILE_PATH];
    char* pBuffer;
    char* pLoc;
    int nAvailableLen;
};  // PCapRTPWriter

}   // namespace PCap
}   // namespace Metreos

#endif // PCAP_RTPWRITER_H