/**
 * $Id: MetreosH323Message.h 12304 2005-10-22 22:08:37Z jdixson $
 *
 * The primary message class passed amongst the various tasks throughout the
 * H.323 stack process.  MetreosH323Message provides a sub-class of 
 * ACE_Message_Block and makes several common fields available (e.g., call ID).
 * 
 * A MetreosH323Message object can be constructed from a char* pointing to
 * a block of flatmap formatted data.
 */ 
#ifndef METREOS_H323_MESSAGE_H
#define METREOS_H323_MESSAGE_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h" // Include this first to make sure we don't have
                        // conflicts between ACE and OpenH323.

#include "ace/Message_Block.h"

namespace Metreos
{

// Forward delcare our flatmap writer class.
class FlatMapWriter;

namespace H323
{

// Forward declare our connection class.
class MetreosConnection;


/**
 * The primary message class sent amongst components within the Metreos H.323
 * stack process.
 */ 
class MetreosH323Message : public ACE_Message_Block
{
public:
    MetreosH323Message(ACE_Allocator* a = 0);
    MetreosH323Message(const char *data, size_t size = 0);
    MetreosH323Message(const ACE_Message_Block &mb, size_t align);
    MetreosH323Message(size_t size, ACE_Message_Type type = MB_DATA, ACE_Message_Block* cont = 0, const char* data = 0);

    virtual ~MetreosH323Message();

    void  msgInit();

    int isEmpty() const                 { return this->base() == 0; }
    
    long param() const                  { return this->m_userParam; }     
    void param(long n)                  { this->m_userParam = n; }

    unsigned int type() const           { return this->m_msgType; }
    void type(unsigned int n)           { this->m_msgType = n; }

    char* metreosData() const           { return this->m_data; }

    void conx(MetreosConnection* conx)  { this->m_conx = conx; }
    MetreosConnection* conx() const     { return this->m_conx; }

    bool isPersistent() const           { return this->m_isPersistent; }
    void makePersistent()               { this->m_isPersistent = true; }

    size_t callIdLen() const            { return m_callIdLen; }
    char* callId() const                { return m_callId; }
    void  callId(const char* callId, size_t size) 
    { 
        m_callIdLen = size;
        m_callId = new char[size];
        ACE_OS::memset(m_callId, 0, size);
        ACE_OS::memcpy(m_callId, callId, size); 
    }

    /**
     * Create a flatmap response writer for this message.
     * Essentially, take m_data and read the transaction ID
     * from the message and insert into into the writer
     * that we return.
     *
     * NOTE: I don't know if this is required any more.  This
     * is left over from the old H.323 code.  Need to check to
     * see if the transaction ID stuff is still valid.  Currently,
     * it is benign to leave this in. --LRM 8/8/2005
     */
    FlatMapWriter* createResponseWriter();

protected:
    unsigned int        m_msgType;              // Message type ala Windows
    long                m_userParam;            // Arbitrary message parameter
    unsigned int        m_flags;                // Arbitrary user flags
    char*               m_data;                 // The flatmap data block
    bool                m_isPersistent;         // Persist the message?

    char*               m_callId;
    size_t              m_callIdLen;

    MetreosConnection*  m_conx;
};

} // namespace H323
} // namespace Metreos

#endif // METREOS_H323_MESSAGE_H