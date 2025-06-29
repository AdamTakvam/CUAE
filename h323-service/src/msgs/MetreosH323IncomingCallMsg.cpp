/**
 * $Id: MetreosH323IncomingCallMsg.cpp 12304 2005-10-22 22:08:37Z jdixson $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef H323_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "H323Common.h"

#include "FlatMap.h"
#include "msgs/MetreosH323IncomingCallMsg.h"

using namespace Metreos;
using namespace Metreos::H323;
using namespace Metreos::H323::Msgs;

MetreosH323IncomingCallMsg::MetreosH323IncomingCallMsg() : 
    responseReceivedMutex(),
    responseReceived(responseReceivedMutex)
{
    m_toNumber = m_toAlias = m_fromNumber = m_fromAlias = m_response = 0;
    m_fastStartState = 0;
}

MetreosH323IncomingCallMsg::~MetreosH323IncomingCallMsg()
{
    if(m_toNumber   != 0) delete[] m_toNumber;
    if(m_toAlias    != 0) delete[] m_toAlias;
    if(m_fromNumber != 0) delete[] m_fromNumber;
    if(m_fromAlias  != 0) delete[] m_fromAlias;
    if(m_response   != 0) delete[] m_response;
}
