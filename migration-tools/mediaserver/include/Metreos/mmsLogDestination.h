// mmsLogDestination.h
// Pluggable logger destinations. Derive from MmsLogDestination and implement 
// printToDestination() to do the actual output operation. User code will not 
// call printToDestination directly however, but rather destination.out().
//
#ifndef MMS_LOGDESTINATION_H
#define MMS_LOGDESTINATION_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "ace/OS.h"   
#include "ace/ACE.h"
#include "ace/INET_Addr.h"
#include "ace/SOCK_Acceptor.h"   
#include "mms.h"
#include "mmsConfig.h"

#if defined(MMS_WINPLATFORM)
#define MMS_LOG_DIRECTORY ACE_TEXT ("log\\")
#else
#define MMS_LOG_DIRECTORY ACE_TEXT ("log/")
#endif 
#define MMS_LOGFILE_EXTENSION ACE_TEXT(".log")

#define LOGOFSTREAM FILE  
#define MMS_MIN_LOGLINES 300



class MmsLogDestination
{ 
  public:
  MmsLogDestination(int type=0, char* name=0): m_type(type), 
  m_isOpen(false), m_isEnabled(true), m_isLoggedInline(false) 
  { memset(m_name, 0, sizeof(m_name));
    if (name)
        memcpy(m_name, name, sizeof(m_name)-1);
  }  

  virtual ~MmsLogDestination() { this->close(); }
                                            // IDs must be powers of 2
  enum ID{UNKNOWN=0, STDOUT=1, DEBUG=2, LOCALFILE=4, SOCKET=8, LOGSERVER=16};

  int open()          { if ( m_isOpen) return -1; m_isOpen = true;  return 0;}
  virtual int close() { if (!m_isOpen) return -1; m_isOpen = false; return 0;}

  int out(const ACE_TCHAR* msg, const void* param=0) 
  { 
    return (m_isOpen && m_isEnabled)? this->printToDestination(msg, param): 0;
  }

  void enable()    {m_isEnabled = true; }
  void disable()   {m_isEnabled = false;}
  bool isEnabled() {return m_isOpen && m_isEnabled; }
  bool isOpen()    {return m_isOpen;}
  bool isInline()  {return m_isLoggedInline;}
  bool isConsole() {return m_type == STDOUT;}
  bool isSocket()  {return m_type == SOCKET;}
	bool isLogServer() { return m_type == LOGSERVER; }
  int  type()      {return m_type;}
  virtual int cycle(const MmsTask* caller) { return 0; }
  virtual void flushLog(const MmsTask* caller) { }
  
  protected:
  virtual int printToDestination(const ACE_TCHAR* msg, const void* param=0)=0;

  int  m_type;
  bool m_isOpen;
  bool m_isEnabled;                         // Logging to this dest suspended?
  bool m_isLoggedInline;                    // Not to be delegated to a thread?
  MmsConfig* m_config;
  ACE_TCHAR  m_name[16];                    // Name of destination
};



class MmsLogDestinationStdout: public MmsLogDestination
{
  public:
  MmsLogDestinationStdout(): MmsLogDestination(MmsLogDestination::STDOUT, "STDOUT")
  { m_isLoggedInline = m_isOpen = true; 
  }
 
  virtual int printToDestination(const ACE_TCHAR* msg, const void* param=0)
  {
    return ACE_OS::printf(msg);
  } 
};



class MmsLogDestinationSystemDebugger: public MmsLogDestination
{
  public:
  MmsLogDestinationSystemDebugger(bool isInline=true): 
  MmsLogDestination(MmsLogDestination::DEBUG, "DEBUG")
  { m_isLoggedInline = isInline; m_isOpen = true;  
  }
 
  virtual int printToDestination(const ACE_TCHAR* msg, const void* param=0)
  {
    #ifdef MMS_WINPLATFORM
    OutputDebugString(msg);
    return 0; 
    #else
    return 0;    
    #endif
  } 
};



class MmsLogDestinationLocalFile: public MmsLogDestination
{ 
  public:
  MmsLogDestinationLocalFile(MmsConfig* cfig);

  MmsLogDestinationLocalFile
 (MmsConfig*, ACE_TCHAR* filename, int append=0, int isFullPath=0);  

  virtual ~MmsLogDestinationLocalFile(); 

  int open(const ACE_TCHAR *filename, int append=0, int isFullPath=0); 

  virtual int printToDestination(const ACE_TCHAR* buf, const void* param);

  int close(); 
  
  int cycle(const MmsTask* caller);

  void flushLog(const MmsTask* caller);

  int backuplogfile(ACE_TCHAR* backupname);

  int rename(const ACE_TCHAR* newname, ACE_TCHAR* newpath, const int pathbuflen);
 
  void makeBackupFilename(char* namebuf);

  LOGOFSTREAM* outputFile() { return m_outFile; }

  protected:

  inline void makePath(const ACE_TCHAR *filename);

  void init();

  ACE_TCHAR    m_path[MAXPATHLEN];
  LOGOFSTREAM* m_outFile; 
  MmsConfig*   m_config;
  int          m_maxlines, m_linesout;
};   



class MmsLogDestinationSocket: public MmsLogDestination
{ 
  public:
  MmsLogDestinationSocket(MmsConfig* cfig);

  MmsLogDestinationSocket(MmsConfig* cfig, const int port); 

  virtual ~MmsLogDestinationSocket(); 

  int  open(const int port); 

  int  connect();
  int  waitforconnect();

  void disconnect();

  virtual int printToDestination(const ACE_TCHAR* buf, const void* param);

  int  close(); 

  int  isConnected() { return  m_isConnected; }
  int  notConnected(){ return !m_isConnected; }

  void resetConnectTimer(); 
  int  countdownConnectTimer();    

  int setlocaladdr(const int port);

  struct SocketConnectInfo
  {
    int   isThreadActive;
    int   isQuitRequest;
    int*  isConnected;
    ACE_thread_t       threadhandle;
    ACE_SOCK_Acceptor* acceptor; 
    ACE_SOCK_Stream*   socketstream; 
    ACE_INET_Addr*     addrLocal;
    ACE_INET_Addr*     addrRemote;
    SocketConnectInfo() { memset(this, 0, sizeof(SocketConnectInfo)); } 
  };
    
  protected:
  int                m_isConnected;
  int                m_isSetLocal;
  int                m_portLocal;           // A copy not in network byte order
  int                m_consecutiveErrors;
  ACE_INET_Addr      m_addrLocal;
  ACE_INET_Addr      m_addrRemote;
  ACE_SOCK_Acceptor  m_acceptor;
  ACE_SOCK_Stream    m_socketstream;

  MmsConfig*         m_config;  

  SocketConnectInfo  m_connectinfo;         

  void init();
};

class MmsLogDestinationLogServer: public MmsLogDestination
{
  public:
		MmsLogDestinationLogServer();
		virtual ~MmsLogDestinationLogServer(); 
		virtual int printToDestination(const ACE_TCHAR* msg, const void* param=0);

protected:
		int activate();
		int deactivate();
};


typedef MmsLogDestination* HDESTINATION;    // Destination handle type


#endif