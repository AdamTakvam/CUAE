//
// MmsSessionE.cpp
// 
// Session object utility methods
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#ifdef MMS_WINPLATFORM
#include <minmax.h>
#endif
#include <ctype.h>
#include "mmsSession.h"
#include "mmsParameterMap.h"
#include "mmsMediaEvent.h"
#include "mmsSessionManager.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsCommandTypes.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsSession::countdownSession()          
{ 
  // Update time remaining -- return -1 if timed out. Timers reset on expiration.
  if (isInfiniteSessionTimeout()) return 0; 
  ACE_Guard<ACE_Thread_Mutex> x(this->atomicOperationLock);
  const int result = this->timerSession.countdown();
  // MMSLOG((LM_DEBUG,"%s session time remaining %d\n", objname, result));
  return result == -1? -1: 0;
}



void MmsSession::resetSessionTimer(int secs)         
{
  if (secs == 0) secs = this->sessionTimeoutSecs;
  ACE_Guard<ACE_Thread_Mutex> x(this->atomicOperationLock);
  this->timerSession.reset(secs * 1000);
}



int MmsSession::isHandlingCommand()           
{ 
  // Determine if session is currently handling a command

  return (this->flags & MMS_SESSION_FLAG_HANDLING_COMMAND) != 0; 
}



void MmsSession::markHandlingCommand(int isBeginCriticalSection)
{
  // Set or reset flag indicating session is currently executing a command handler.
  // We can't begin another command, whether barging or not, on a session, while
  // we are executing this "critical section".

  ACE_Guard<ACE_Thread_Mutex> x(this->handlingCommandLock);

  if (isBeginCriticalSection)
      this->flags |=  MMS_SESSION_FLAG_HANDLING_COMMAND;
  else 
  { 
      this->flags &= ~MMS_SESSION_FLAG_HANDLING_COMMAND;

      if (this->pendingDisconnectMap)
      {
          // If a session or server disconnect arrived while handling the 
          // previous command, we will have cached the disconnect. We re-post
          // that disconnect command now that the command has completed.
          MMSLOG((LM_INFO,"%s resubmit deferred disconnect\n", objname)); 

          sessionMgr->postMessage(MMSM_SESSIONTASK, (long)pendingDisconnectMap);
          this->pendingDisconnectMap = NULL;
      }
  }
}                  



void MmsSession::replaceCoderBits(unsigned int& remoteattrs, 
  unsigned int& localattrs, const unsigned int newcoder)
{
  // Replace coder bits in attribute bitstrings   
  MMS_CLEAR_CODER_BITS(remoteattrs);
  MMS_CLEAR_CODER_BITS(localattrs);
  remoteattrs |= newcoder;
  localattrs  |= newcoder;      
}



void* MmsSession::getMmsConferenceObject(const int confID)
{
  MmsConference* mmsConference = NULL;
  MmsConferenceManager* conferenceMgr = sessionMgr->conferenceManager();
  if (conferenceMgr && confID) 
      conferenceMgr->findByConferenceID(confID, &mmsConference);
  return mmsConference;
}



int MmsSession::editIpAddress(char* ipAddr, const int isZeroOK, const int isEmptyOK)
{
  // isZeroOK  specifies if 0.0.0.0 IP is acceptable; default true
  // isEmptyOK specifies if null or empty IP is acceptable; default false

  if (ipAddr == NULL || strlen(ipAddr) < 1)  
      return isEmptyOK? 0: MMS_ERROR_PARAMETER_VALUE;
   
  std::vector<std::string> octets;
  std::string ipStr(ipAddr);
  int zeros = 0, result = 0;

  ipParser(ipStr, octets, ".");
  if (octets.size() != 4)     
      return MMS_ERROR_PARAMETER_VALUE;
         
  for(int i=0; i<4; i++)
  {
    int lzs = 0, len = 0, nzFound = 0;
    char* szOctet = (char*)octets[i].c_str();
                                      
    for(char* p = szOctet; *p; p++)    
    {
        if  (++len > 3)
             return MMS_ERROR_PARAMETER_VALUE;
        else
        if  (*p == '0')              // Count leading zeros
             if  (nzFound);
             else lzs++;
        else
        if  (isdigit(*p))            // Ensure numeric content
             nzFound = TRUE;
        else return MMS_ERROR_PARAMETER_VALUE;
    }

    const int iOctet = ACE_OS::atoi(szOctet);
    const int lzsOK  = iOctet? 0: 1; // Ensure no leading zeros

    if (lzs > lzsOK || iOctet > 255) return MMS_ERROR_PARAMETER_VALUE;
    if (iOctet == 0) zeros++;
  }

  if (!isZeroOK && zeros == 4) result = MMS_ERROR_PARAMETER_VALUE;
  return result;
}

                                                 

void MmsSession::ipParser(const std::string& ipaddr, 
  std::vector<std::string>& octets, const std::string& delimiters)
{
  std::string::size_type lastPos = ipaddr.find_first_not_of(delimiters, 0);
  std::string::size_type pos     = ipaddr.find_first_of(delimiters, lastPos);

  while (std::string::npos != pos || std::string::npos != lastPos)
  {
    // Found a token, add it to the vector.
    octets.push_back(ipaddr.substr(lastPos, pos - lastPos));
    // Skip delimiters.  Note the "not_of"
    lastPos = ipaddr.find_first_not_of(delimiters, pos);
    // Find next "non-delimiter"
    pos = ipaddr.find_first_of(delimiters, lastPos);
  }
}



int MmsSession::writeDescriptorFile
( MmsDeviceVoice::MMS_PLAYRECINFO& recinfo, MMSPLAYFILEINFO* fileinfo, const int exp) 
{     
  // Write a descriptor file, which is the .mms companion file accompanying each file
  // recorded by the media server. This is a text file containing expiration longevity,
  // codec information, etc. 
                                                        
  int expiration = exp == 0? this->getRecordFileExpiration():
                   exp  > 0? exp:           // Caller provided explicit expiration
                   0;                       // Caller negative means expire asap
                                               
  MmsAudioFileDescriptor proprec; 

  int result = proprec.write(recinfo, fileinfo, expiration);

  if (result == 0)                          // Nothing written
      MMSLOG((LM_ERROR,"%s write error on %s\n", objname, fileinfo->propfilepath));

  return result;
}



void MmsSession::encodeConferenceAttributes
 (const unsigned int mmsAttrsConference, const unsigned int mmsAttrsConferee,
  unsigned int& hmpAttrsConference, unsigned int& hmpAttrsConferee)
{
  if  (mmsAttrsConference &  MMS_CONFERENCE_SOUNDTONE)
       hmpAttrsConference |= MSCA_ND;
  if  (mmsAttrsConference &  MMS_CONFERENCE_RCVONLY_NOTONE)
       hmpAttrsConference |= MSCA_NN;

  if  (mmsAttrsConferee &  MMS_CONFEREE_RECEIVE_ONLY)
       hmpAttrsConferee |= MSPA_RO;
  if  (mmsAttrsConferee &  MMS_CONFEREE_TARIFF_TONE)
       hmpAttrsConferee |= MSPA_TARIFF;
  else
  if  (mmsAttrsConferee &  MMS_CONFEREE_COACH)
       hmpAttrsConferee |= MSPA_COACH;
  else
  if  (mmsAttrsConferee &  MMS_CONFEREE_PUPIL)
       hmpAttrsConferee |= MSPA_PUPIL;
}



void MmsCurrentCommand::copy(MmsCurrentCommand& that)
{
  that.command   = this->command;
  that.flatmap   = this->flatmap;
  that.flags     = this->flags;
  that.session   = this->session;
  that.operation = this->operation;
}



void MmsSession::dumpParameterMap(void* pmap)
{
  // Diagnostic dump of flatmap contents to log. Currently we do not    
  // recurse embedded flatmaps, but this can be easily changed to do so.

  if (NULL == pmap) return;
  MmsFlatMapReader map((const char*)pmap);
  char* val, *buf = new char[512];
  int   type, len, key, ival, count = map.size();
  __int64 ival64;
  double  dval;
  MMSLOG((LM_ERROR,"UTIL map dump of %d items\n", count));

  for(int i=0; i < count; i++)
  {
    len = map.get(i, &key, &val, &type);
    if  (len == 0)      
         MMSLOG((LM_DEBUG,"UTIL %d EMPTY\n", i));
    else switch(type)
    {
      case MmsFlatMap::datatype::BYTE:
           memset(buf, 0, sizeof(buf));
           memcpy(buf, val, len);
           MMSLOG((LM_DEBUG,"UTIL %d BYT %s\n", i, buf));
           break;
      case MmsFlatMap::datatype::INT:
           ival = *(int*)val;
           MMSLOG((LM_DEBUG,"UTIL %d INT %d\n", i, ival));
           break;
      case MmsFlatMap::datatype::LONG:
           ival64 = *(__int64*)val;
           MMSLOG((LM_DEBUG,"UTIL %d LNG 0x%I64x\n", i, ival64));
           break;
      case MmsFlatMap::datatype::DOUBLE:
           dval = *(double*)val;
           MMSLOG((LM_DEBUG,"UTIL %d DBL %f\n", i, dval));
           break;
      case MmsFlatMap::datatype::STRING:
           MMSLOG((LM_DEBUG,"UTIL %d STR %s\n", i, val));
           break;
      case MmsFlatMap::datatype::FLATMAP:
           MMSLOG((LM_DEBUG,"UTIL %d MAP\n", i));
           break;
    }
  }

  delete[] buf;
}


                                         
MmsConference::MmsConference(MmsDeviceConference* device,  
  const int conferenceID, const int attrs, const int isHairpin) 
{ 
  this->deviceConference = device;
  this->conferenceID = conferenceID;
  this->conferenceAttributes = attrs;
  this->ismonitored  = 0;
  this->isactive     = 1;
  this->ishairpin    = isHairpin;
}


