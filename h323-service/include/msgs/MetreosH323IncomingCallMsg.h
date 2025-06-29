#ifndef METREOS_H323_INCOMING_CALL_MSG_H
#define METREOS_H323_INCOMING_CALL_MSG_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h"

#include "MetreosH323Message.h"

namespace Metreos
{

class FlatMapWriter;

namespace H323
{

class MetreosConnection;

namespace Msgs
{

class MetreosH323IncomingCallMsg : public MetreosH323Message
{
public:

    MetreosH323IncomingCallMsg();
    ~MetreosH323IncomingCallMsg();

    char* toNumber() const { return m_toNumber; }
    void  toNumber(const char* toNumber, size_t size) 
    { 
        m_toNumber = new char[size];
        ACE_OS::memset(m_toNumber, 0, size);
        ACE_OS::memcpy(m_toNumber, toNumber, size); 
    }

    char* toAlias() const { return m_toAlias; }
    void  toAlias(const char* toAlias, size_t size) 
    { 
        m_toAlias = new char[size];
        ACE_OS::memset(m_toAlias, 0, size);
        ACE_OS::memcpy(m_toAlias, toAlias, size); 
    }

    char* fromNumber() const { return m_fromNumber; }
    void  fromNumber(const char* fromNumber, size_t size) 
    { 
        m_fromNumber = new char[size];
        ACE_OS::memset(m_fromNumber, 0, size);
        ACE_OS::memcpy(m_fromNumber, fromNumber, size); 
    }

    char* fromAlias() const { return m_fromAlias; }
    void  fromAlias(const char* fromAlias, size_t size) 
    { 
        m_fromAlias = new char[size];
        ACE_OS::memset(m_fromAlias, 0, size);
        ACE_OS::memcpy(m_fromAlias, fromAlias, size); 
    }

    int  fastStartState() const { return m_fastStartState; }
    void fastStartState(const int fastStartState)
    {
        m_fastStartState = fastStartState;
    }

    char* response() const { return m_response; }
    void  response(const char* response, size_t size)
    {
        m_response = new char[size];
        ACE_OS::memset(m_response, 0, size);
        ACE_OS::memcpy(m_response, response, size); 
    }

    MetreosConnection* connection() const { return m_conx; }
    void connection(MetreosConnection* conx)
    {
        m_conx = conx;
    }

    ACE_Thread_Mutex responseReceivedMutex;
    ACE_Condition<ACE_Thread_Mutex> responseReceived;

protected:
    char* m_response;
    
    char* m_toNumber;
    char* m_toAlias;
    char* m_fromNumber;
    char* m_fromAlias;
    int   m_fastStartState;

    MetreosConnection* m_conx;
};

} // namespace Msgs
} // namespace H323
} // namespace Metreos

#endif // METREOS_H323_INCOMING_CALL_MSG_H