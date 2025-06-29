//
// mmsLogDestLocalFile.cpp
//
// Logger pluggable destination for resident log file
//
#include "StdAfx.h"
#include "mmsLogger.h"
#include "mmsLogDestination.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



MmsLogDestinationLocalFile::MmsLogDestinationLocalFile(MmsConfig* cfig):
  MmsLogDestination(MmsLogDestination::LOCALFILE, "FILE"), 
  m_config(cfig), m_outFile(0)
{   
  this->init();
}



MmsLogDestinationLocalFile::MmsLogDestinationLocalFile
( MmsConfig* cfig, ACE_TCHAR* filename, int append, int isFullPath): 
  MmsLogDestination(MmsLogDestination::LOCALFILE, "FILE"), 
  m_config(cfig), m_outFile(0) 
{
  this->init();
  this->open(filename, append, isFullPath);
} 



MmsLogDestinationLocalFile::~MmsLogDestinationLocalFile() 
{
  this->close();                             
}



int MmsLogDestinationLocalFile::open
( const ACE_TCHAR *filename, int append, int isFullPath) 
{
  if  (this->isOpen()) return -1;

  if  (isFullPath)
       ACE_OS::strncpy(m_path, filename, MAXPATHLEN);
  else this->makePath(filename);
                                            // If server boot ...
  if  (m_linesout == 0 && m_config->serverLogger.backup)
  {
       ACE_TCHAR backupname[32];            // ... back up log if configged
       if  (-1 == this->backuplogfile(backupname))
            ACE_OS::printf("LOGD could not backup existing log\n");
  }

  m_linesout = 0;
                                   
  if  (NULL == (m_outFile = fopen(m_path, append? "a": "w")))
  {    ACE_OS::printf("LOGD >>> open error on %s\n", m_path);
       return -1;
  }
  else
  {    ACE_OS::printf("LOGD local logfile open as %s\n", m_path);
       if (m_config->serverLogger.flush)
           ACE_OS::printf("LOGD flush after write requested\n");
       return MmsLogDestination::open();
  }
}



int MmsLogDestinationLocalFile::printToDestination
( const ACE_TCHAR* buf, const void* param)
{
  if  (NULL == buf) return 0;

  int length = (int)param;                  // Length includes null term

  #ifdef MMS_POINTER_VALIDATION_ENABLED
  #ifdef MMS_WINPLATFORM 
  if  (IsBadReadPtr(buf, length? length: 16))
  {    ACE_OS::printf("LOGD >>> invalid pointer at printToDestination\n");
       return 0;
  }
  #endif
  #endif
                                           
  length = length > 0? --length: ACE_OS::strlen(buf);

  int  n = fwrite(buf, 1, length, m_outFile);

  if  (n)  
  {    m_linesout++;                   
      
       if (m_config->serverLogger.flush)    // Configged to flush every write?
           fflush(m_outFile);
  }
                                            // Cycle log file if at capacity
  m_maxlines = m_config->serverLogger.maxlines;
  if  (m_maxlines && m_maxlines < MMS_MIN_LOGLINES) 
       m_maxlines  = MMS_MIN_LOGLINES;

  if  (m_maxlines && m_linesout >= m_maxlines)
       this->cycle(NULL);

  return n;
}



int MmsLogDestinationLocalFile::close() 
{ 
  if  (MmsLogDestination::close() == 1) return -1;
  if  (m_outFile) fclose(m_outFile);
  m_outFile = NULL;
  return 0;
}



int MmsLogDestinationLocalFile::cycle(const MmsTask* caller)
{
  // Closes log file, renames log, reopens log file. Returns lines cycled
  // or -1 if rename or reopen error. 

  ACE_TCHAR backupname[32];
  const int backuplinecount = m_linesout;
  this->disable();                        // Suspend logging
  this->close();                          // Close  log file
                      
  const int renameresult = this->backuplogfile(backupname);
  if  (renameresult != -1)
       ACE_OS::printf("LOGD %d lines cycled to %s\n", backuplinecount, backupname);                                           
                                          // Reopen log file
  const int openresult   = this->open(m_path, 0, TRUE);  
  this->enable();                         // Resume logging

  return ((-1 == renameresult) || (-1 == openresult))? -1: backuplinecount;
} 

void MmsLogDestinationLocalFile::flushLog(const MmsTask* caller)
{
    this->disable();                        // Suspend logging
    fflush(m_outFile);
    this->enable();                         // Resume logging
}

int MmsLogDestinationLocalFile::backuplogfile(ACE_TCHAR* backupname)
{
  ACE_TCHAR newpath[MAXPATHLEN];  
  this->makeBackupFilename(backupname);
  int  result = this->rename(backupname, newpath, sizeof(newpath));
  return result;
}



int MmsLogDestinationLocalFile::rename
( const ACE_TCHAR* newname, ACE_TCHAR* newpath, const int pathbuflen)
{
  ACE_TCHAR* p = m_path + ACE_OS::strlen(m_path);
  while(p  > m_path  && *p != ACE_DIRECTORY_SEPARATOR_CHAR) p--;
  if   (p <= m_path) return -1;
  const int directorypathlength = (p - m_path) + 1; 
  memset(newpath, 0, pathbuflen);
  memcpy(newpath, m_path, directorypathlength);
  memcpy(newpath + directorypathlength, newname, ACE_OS::strlen(newname));
  const int result = ACE_OS::rename(m_path, newpath);
  return result;
}



void MmsLogDestinationLocalFile::makeBackupFilename(char* namebuf)
{
  long   l; time(&l);
  struct tm* t = ACE_OS::localtime(&l);
  ACE_OS::sprintf(namebuf, "%04d%02d%02d%02d%02d%02d", 
          t->tm_year+1900, t->tm_mon+1, t->tm_mday,
          t->tm_hour,      t->tm_min,   t->tm_sec);
  ACE_OS::strcat(namebuf, MMS_LOGFILE_EXTENSION);
}



void MmsLogDestinationLocalFile::makePath(const ACE_TCHAR *filename)
{ 
  const ACE_TCHAR *logDirectory = ACE_OS::getenv("MMSLOGDIR");
  if (logDirectory == NULL) logDirectory = "";

  ACE_OS::sprintf(m_path, "%s%s%s%s", logDirectory, MMS_LOG_DIRECTORY,
          ACE::basename(filename, ACE_DIRECTORY_SEPARATOR_CHAR),
          MMS_LOGFILE_EXTENSION);
}


 
void MmsLogDestinationLocalFile::init()
{
  ACE_ASSERT(m_config); 
  m_maxlines = m_linesout = 0;
}