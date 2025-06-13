// PCapMessage.cpp

#include "stdafx.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "PCapCommon.h"

#include "Flatmap.h"
#include "PCapMessage.h"

using namespace Metreos;
using namespace Metreos::PCap;

PCapMessage::PCapMessage(ACE_Allocator* a) : ACE_Message_Block(a) 
{ 
    this->msgInit();
}

PCapMessage::PCapMessage(const char *data, size_t size) : ACE_Message_Block(data, size) 
{ 
    this->msgInit();

    this->m_data = (char*)data;
}

PCapMessage::PCapMessage(size_t size, ACE_Message_Type type, ACE_Message_Block* cont, 
                                       const char* data) : ACE_Message_Block(size, type, cont, data) 
{ 
    this->msgInit();
}

PCapMessage::PCapMessage(const ACE_Message_Block &mb, size_t align) : ACE_Message_Block(mb,align) 
{ 
    this->msgInit();
}

PCapMessage::~PCapMessage()
{
    if(m_data != 0) 
        delete[] m_data;

    if (m_callData != 0)
        delete m_callData;

    if (m_packetHeader != 0)
        delete m_packetHeader;
}

void  PCapMessage::msgInit()  
{ 
    this->m_isPersistent = false;
    this->m_msgType = this->m_userParam = this->m_flags = 0; 
    this->m_data = 0;
    this->m_callData = 0;
    this->m_packetHeader = 0;
}
