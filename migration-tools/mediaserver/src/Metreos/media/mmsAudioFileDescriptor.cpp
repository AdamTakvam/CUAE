//
// mmsAudioFileDescriptor.cpp
//
#include "StdAfx.h"
#ifdef MMS_WINPLATFORM
#pragma warning(disable:4786)
#endif
#include "mmsAudioFileDescriptor.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



void MmsAudioFileDescriptor::set(MmsDeviceVoice::MMS_PLAYRECINFO& info)
{ 
  // Translate MMS_PLAYRECINFO object to MmsAudioFileDescriptor record
  char*  p = NULL;
  switch(info.filetype) 
  { case MmsDeviceVoice::VOX: p = MMSAFD_VOX; break;
    case MmsDeviceVoice::WAV: p = MMSAFD_WAV; break;
  }
  if  (p) memcpy(type,p,4);   

  p = NULL;
  switch(info.mode) 
  { case MmsDeviceVoice::MULAW: p = MMSAFD_ULAW;  break;
    case MmsDeviceVoice::ALAW:  p = MMSAFD_ALAW;  break;
    case MmsDeviceVoice::PCM:   p = MMSAFD_PCM;   break;
    case MmsDeviceVoice::ADPCM: p = MMSAFD_ADPCM; break;
  }
  if  (p) memcpy(mode,p,4);

  p = NULL;
  switch(info.rate) 
  { case MmsDeviceVoice::RATE_6KHZ: p = MMSAFD_6KHZ;  break;
    case MmsDeviceVoice::RATE_8KHZ: p = MMSAFD_8KHZ;  break;
    case MmsDeviceVoice::RATE_11KHZ:p = MMSAFD_11KHZ; break;
  }
  if  (p) memcpy(rate,p,4);

  p = NULL;
  switch(info.samplesize) 
  { case MmsDeviceVoice::SIZE_4BIT: p = MMSAFD_4BIT;  break;
    case MmsDeviceVoice::SIZE_8BIT: p = MMSAFD_8BIT;  break;
    case MmsDeviceVoice::SIZE_16BIT:p = MMSAFD_16BIT; break;
  }
  if  (p) memcpy(samplesize,p,4);

}
      


void MmsAudioFileDescriptor::get(MmsDeviceVoice::MMS_PLAYRECINFO& info)
{    
  // Translate MmsAudioFileDescriptor record to MMS_PLAYRECINFO object  
  if  (memcmp(type,MMSAFD_VOX,4) == 0) info.filetype = MmsDeviceVoice::VOX; else 
  if  (memcmp(type,MMSAFD_WAV,4) == 0) info.filetype = MmsDeviceVoice::WAV; 

  if  (memcmp(mode,MMSAFD_ULAW,4)  == 0) info.mode = MmsDeviceVoice::MULAW; else 
  if  (memcmp(mode,MMSAFD_ALAW,4)  == 0) info.mode = MmsDeviceVoice::ALAW;  else 
  if  (memcmp(mode,MMSAFD_PCM, 4)  == 0) info.mode = MmsDeviceVoice::PCM;   else 
  if  (memcmp(mode,MMSAFD_ADPCM,4) == 0) info.mode = MmsDeviceVoice::ADPCM;  

  if  (memcmp(rate,MMSAFD_6KHZ,4)  == 0) info.rate = MmsDeviceVoice::RATE_6KHZ; else  
  if  (memcmp(rate,MMSAFD_8KHZ,4)  == 0) info.rate = MmsDeviceVoice::RATE_8KHZ; else  
  if  (memcmp(rate,MMSAFD_11KHZ,4) == 0) info.rate = MmsDeviceVoice::RATE_11KHZ;   

  if  (memcmp(samplesize,MMSAFD_4BIT,4)  == 0) info.samplesize = MmsDeviceVoice::SIZE_4BIT; else  
  if  (memcmp(samplesize,MMSAFD_8BIT,4)  == 0) info.samplesize = MmsDeviceVoice::SIZE_8BIT; else  
  if  (memcmp(samplesize,MMSAFD_16BIT,4) == 0) info.samplesize = MmsDeviceVoice::SIZE_16BIT;   
}



int MmsAudioFileDescriptor::write(MMSPLAYFILEINFO* pathinfo)
{
  // Persist to disk. We only write out our public data 
  const int recsize = this->calculateRecordSize();

  FILE* f = fopen(pathinfo->propfilepath,"w");
  if  (!f)  return 0;
  int   n = fwrite(this, recsize, 1, f); 
  fclose(f);
  return n;
}



int MmsAudioFileDescriptor::read(MMSPLAYFILEINFO* pathinfo)
{  
  return this->read(pathinfo->propfilepath);
}



int MmsAudioFileDescriptor::read(char* path)
{
  // Read from disk. We only read into our public data
  const int recsize = this->calculateRecordSize();

  FILE* f = fopen(path, "r");
  if  (!f)  return 0;
  int   n = fread(this, recsize, 1, f); 
  fclose(f);
  return n;
}



int MmsAudioFileDescriptor::erase(MMSPLAYFILEINFO* pathinfo)
{
  // Delete from disk
  return ACE_OS::unlink(pathinfo->propfilepath); 
}



int MmsAudioFileDescriptor::write
( MmsDeviceVoice::MMS_PLAYRECINFO& info, MMSPLAYFILEINFO* pathinfo, int days) 
{
  // Create and write a MmsAudioFileDescriptor record from MMS_PLAYRECINFO
  // Number of days to file expiration are set using parameter supplied.
  this->set(info);
  this->stamp();
  this->setExpiration(days);
  return this->write(pathinfo);
}



int MmsAudioFileDescriptor::read
( MmsDeviceVoice::MMS_PLAYRECINFO& info, MMSPLAYFILEINFO* pathinfo) 
{
  // Read a MmsAudioFileDescriptor record and return as MMS_PLAYRECINFO 
  if  (this->read(pathinfo) == 0) return 0;
  this->get(info);
  return 1;
}



void MmsAudioFileDescriptor::stamp()       // Timestamp this record
{
  // Timestamp MmsAudioFileDescriptor record with current date and time 
  ACE_Guard<ACE_Thread_Mutex> x(recordlock);
  long   l; time(&l);                                
  char   buf[20];
  struct tm* t = ACE_OS::localtime(&l);

  ACE_OS::sprintf(buf, MMSAFD_TIMESTAMPMASK, 
          t->tm_year+1900, t->tm_mon+1, t->tm_mday,
          t->tm_hour,      t->tm_min,   t->tm_sec);

  memcpy(this->timestamp, buf, 16);
}


                                            
void MmsAudioFileDescriptor::setExpiration(int days)
{
  // Set MmsAudioFileDescriptor record number of days to file expiration 
  char buf[5];
  if  (days > 9999)  days = 9999;
  if  (days < 0)     days = 0;
  sprintf(buf,"%04d",days);
  memcpy(this->duration, buf, 4);
}



int MmsAudioFileDescriptor::daysToExpiration()
{
  // Calculate number of days remaining before file expiration

  char buf[16]; memset(buf,0,sizeof(buf));
  memcpy(buf, this->duration, sizeof(this->duration));
  int    daysSpecified = atoi(buf);
 
  struct tm binstamp;
  this->stampToBinary(&binstamp);
  int    daysInTimestamp = this->toDays(&binstamp);

  int    daysInExpirationDate = daysInTimestamp + daysSpecified;

  ACE_Guard<ACE_Thread_Mutex> x(recordlock);
  long   l; time(&l);
  struct tm* today   = ACE_OS::localtime(&l);
  int    daysInToday = this->toDays(today);

  int    daysRemaining = daysInExpirationDate - daysInToday;
  return daysRemaining < 0? 0: daysRemaining;
}



int MmsAudioFileDescriptor::toDays(const struct tm* t)
{
  // Convert tm date to number of days since Jan 1 2000
  int days=0, i, baseyear = t->tm_year - 100;// 2000 = 0
  int dd[12]={31,28,31,30,31,30,31,31,30,31,30,31}, *pd;
  if (t->tm_year % 4 == 0) dd[1] = 29;

  for(i=0; i < baseyear; i++)
      days += (i % 4 == 0)? 366: 365;

  for(i=0, pd = dd; i < t->tm_mon; i++, pd++)   
      days += *pd; 

  days += t->tm_mday;
  
  return days;
}



void MmsAudioFileDescriptor::stampToBinary(struct tm* t)
{
  // Convert file timestamp back to a tm object
  // We don't bother to set julian day, weekday, dst

  char   buf[20], csave, *psave, *pstart;
  int    value = 0;
  memset(t,0,sizeof(struct tm));
  memcpy(buf, this->timestamp, sizeof(this->timestamp));

  pstart = buf;
  psave  = buf+4;
  csave  = *psave;
  *psave = '\0';
  value  = ACE_OS::atoi(pstart);
  t->tm_year = value - 1900;
  *psave = csave;

  pstart += 4;
  psave  += 2;
  csave  = *psave;
  *psave = '\0';
  value  = ACE_OS::atoi(pstart);
  t->tm_mon = value - 1;
  *psave = csave;

  pstart += 2;
  psave  += 2;
  csave  = *psave;
  *psave = '\0';
  value  = ACE_OS::atoi(pstart);
  t->tm_mday = value;
  *psave = csave;

  pstart += 3;
  psave  += 3;
  csave  = *psave;
  *psave = '\0';
  value  = ACE_OS::atoi(pstart);
  t->tm_hour = value;
  *psave = csave;  

  pstart += 2;
  psave  += 2;
  csave  = *psave;
  *psave = '\0';
  value  = ACE_OS::atoi(pstart);
  t->tm_min = value;
  *psave = csave;

  pstart += 2;
  psave  += 2;
  csave  = *psave;
  *psave = '\0';
  value  = ACE_OS::atoi(pstart);
  t->tm_sec = value;
  *psave = csave;
}



int MmsAudioFileDescriptor::isValid()
{
  return (memcmp(id1, MMSAFD_ID1, 4) == 0) && (memcmp(id2, MMSAFD_ID2, 4) == 0);
}



MmsAudioFileDescriptor::MmsAudioFileDescriptor() 
{ 
  const int recordsize = this->calculateRecordSize();
  memset(this, ' ', recordsize);
  memset(unused, 0, sizeof(unused)); 
  memcpy(id1, MMSAFD_ID1, 4); 
  memcpy(id2, MMSAFD_ID2, 4); 
  memcpy(version, MMSAFC_CURRENT_VERSION, 4);
}



