/**
 * $Id: MetreosH323Message.cpp 12304 2005-10-22 22:08:37Z jdixson $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef H323_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "H323Common.h"

#include "Flatmap.h"
#include "MetreosH323Message.h"
#include "msgs/MetreosH323MessageTypes.h"

using namespace Metreos;
using namespace Metreos::H323;

MetreosH323Message::MetreosH323Message(ACE_Allocator* a) : ACE_Message_Block(a) 
{ 
    this->msgInit();
}

MetreosH323Message::MetreosH323Message(const char *data, size_t size) : ACE_Message_Block(data, size) 
{ 
    this->msgInit();

    this->m_data = (char*)data;
}

MetreosH323Message::MetreosH323Message(size_t size, ACE_Message_Type type, ACE_Message_Block* cont, 
                                       const char* data) : ACE_Message_Block(size, type, cont, data) 
{ 
    this->msgInit();
}

MetreosH323Message::MetreosH323Message(const ACE_Message_Block &mb, size_t align) : ACE_Message_Block(mb,align) 
{ 
    this->msgInit();
}

MetreosH323Message::~MetreosH323Message()
{
    if(m_data   != 0) delete[] m_data;
    if(m_callId != 0) delete[] m_callId;
}

void  MetreosH323Message::msgInit()  
{ 
    this->m_isPersistent = false;
    this->m_msgType = this->m_userParam = this->m_flags = 0; 
    this->m_data = this->m_callId = 0;
}

FlatMapWriter* MetreosH323Message::createResponseWriter()
{
    FlatMapWriter* responseWriter = new FlatMapWriter();
    
    FlatMapReader reader(metreosData());

    char* tidStr = 0;
    reader.find(Params::TransactionId, &tidStr);
    int   tid = tidStr ? *((int*)tidStr) : 0;

    responseWriter->insert(Params::TransactionId, tid);

    return responseWriter;
}