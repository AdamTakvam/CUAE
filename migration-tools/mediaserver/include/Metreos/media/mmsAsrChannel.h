// mmsAsrChannel.h
// Container class to hold common ASR channel state and logic

#ifndef MMS_ASRCHANNEL_H
#define MMS_ASRCHANNEL_H

#include "mmsDeviceVoice.h"

// To store data we need for each ASR channel, this is engine specific data
typedef struct AsrChannel { void *internal; } AsrChannel;

// MmsAsrChannel: Container class to hold common ASR channel information
class MmsAsrChannel 
{
public: 
  MmsAsrChannel(unsigned long sessionId, unsigned long opId);
  ~MmsAsrChannel();

  void setAsrChannel(AsrChannel* c) { m_asrChannel = c; }
  AsrChannel* getAsrChannel() { return m_asrChannel; }
  
private:
  unsigned long m_sessionId;      // Media server session id
  unsigned long m_opId;           // operation id
  AsrChannel* m_asrChannel;       // Engine specific ASR channel info
};

#endif
