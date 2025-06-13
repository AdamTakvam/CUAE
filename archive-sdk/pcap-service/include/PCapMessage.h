// PCapMessage.h

/**
 *
 * The primary message class passed amongst the various tasks throughout the
 * pcap process.  PCapMessage provides a sub-class of ACE_Message_Block.
 * 
 * A PCapMessage object can be constructed from a char* pointing to
 * a block of flatmap formatted data.
 */ 

#ifndef PCAP_MESSAGE_H
#define PCAP_MESSAGE_H

#ifdef PCAP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "PCapCommon.h" 
#include "pcap.h"

#include "ace/Message_Block.h"


namespace Metreos
{

namespace PCap
{

class PCapMessage : public ACE_Message_Block
{
public:
    PCapMessage(ACE_Allocator* a = 0);
    PCapMessage(const char *data, size_t size = 0);
    PCapMessage(const ACE_Message_Block &mb, size_t align);
    PCapMessage(size_t size, ACE_Message_Type type = MB_DATA, ACE_Message_Block* cont = 0, const char* data = 0);

    virtual ~PCapMessage();

    void  msgInit();

    int isEmpty() const                 { return this->base() == 0; }
    
    long param() const                  { return this->m_userParam; }     
    void param(long n)                  { this->m_userParam = n; }

    unsigned int type() const           { return this->m_msgType; }
    void type(unsigned int n)           { this->m_msgType = n; }

    char* metreosData() const           { return this->m_data; }

    bool isPersistent() const           { return this->m_isPersistent; }
    void makePersistent()               { this->m_isPersistent = true; }

    pcap_pkthdr* packetHeader() const { return this->m_packetHeader; }
    void packetHeader(const pcap_pkthdr* pts)
    {
        m_packetHeader = new pcap_pkthdr;
        ACE_OS::memset(m_packetHeader, 0, sizeof(pcap_pkthdr));
        ACE_OS::memcpy(m_packetHeader, pts, sizeof(pcap_pkthdr));         
    }

    skinny_call_data* callData() const { return this->m_callData; }
    void callData(const skinny_call_data* scd)
    {
        m_callData = new skinny_call_data;
        ACE_OS::memset(m_callData, 0, sizeof(skinny_call_data));
        ACE_OS::memcpy(m_callData, scd, sizeof(skinny_call_data));         
    }

protected:
    unsigned int        m_msgType;              // Message type ala Windows
    long                m_userParam;            // Arbitrary message parameter
    unsigned int        m_flags;                // Arbitrary user flags
    char*               m_data;                 // The flatmap data block
    bool                m_isPersistent;         // Persist the message?
    pcap_pkthdr*        m_packetHeader;         // Packet header
    skinny_call_data*   m_callData;             // Skinny call data
};

} // namespace PCap
} // namespace Metreos

#endif // PCAP_MESSAGE_H