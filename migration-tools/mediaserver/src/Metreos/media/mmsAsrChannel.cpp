//
// mmsAsrChannel.cpp
// Container class to hold common ASR channel state and logic
//
#include "StdAfx.h"
#include "mmsAsrChannel.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


///////////////////////////////////////////////////////////
// MmsAsrChannel default constructor
MmsAsrChannel::MmsAsrChannel(unsigned long sessionId, unsigned long opId)
{
  this->m_sessionId = sessionId;
  this->m_opId = opId;
  m_asrChannel = NULL;
}

///////////////////////////////////////////////////////////
// MmsAsrChannel destructor
MmsAsrChannel::~MmsAsrChannel()
{
}

