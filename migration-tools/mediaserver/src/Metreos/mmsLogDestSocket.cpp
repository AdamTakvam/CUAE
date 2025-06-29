//
// mmsLogDestSocket.cpp
//
// Logger pluggable destination for tcp socket
//
// At such time as the log destination is opened, we launch a thread
// which waits for a connection on the advertised port. Once connected,
// the thread exits, and socket writes commence for each log record.
// After some number of socket send errors, the client is assumed
// down, the socket is disconnected, and the wait thread is relaunched.
//
#include "StdAfx.h"
#include "mmsLogger.h"
#include "mmsLogDestination.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

static void* connectproc(void*);
#define DISCO_AFTER_N_SEND_ERRORS 3



                                            
int MmsLogDestinationSocket::printToDestination(const ACE_TCHAR* buf, const void* param)
{
  // Write log record to socket. If this plugin is installed, log records arrive
  // here regardless of whether a socket connection exists. If there is no 
  // current connection we do nothing and return.

  if  (this->notConnected()) return 0;       
  if  (buf == NULL) return 0;

  int  length = (int)param;                 // Logrec length includes null term
                                            // Write logrec to socket
  if  (m_socketstream.send(buf, length, 0) == -1)
  {                                         // If too many errors, ...
       if  (++m_consecutiveErrors == DISCO_AFTER_N_SEND_ERRORS)
       {                                     
            ACE_OS::printf("TCPL %s not responding\n", m_addrRemote.get_host_addr());
            this->disconnect();             // ... disconnect socket and ...
            this->waitforconnect();         // ... listen for another connection
       }

       return -1;
  }

  m_consecutiveErrors = 0;
  return 0;
}



MmsLogDestinationSocket::~MmsLogDestinationSocket() 
{
  this->close();                              
}



int MmsLogDestinationSocket::open(const int port) 
{
  // Open is a misnomer here. If we appear closed, we won't get called
  // by the logger, so we always appear open. Whether or not we're
  // connected is a separate issue.

  m_portLocal = port;

  if  (-1 == this->setlocaladdr(port)) return -1;

  this->waitforconnect();

  return MmsLogDestination::open();
}
  


int MmsLogDestinationSocket::waitforconnect() 
{
  // Launch a thread which will wait for a connection on the advertised port
 
  if  (!m_isSetLocal) return -1;

  m_consecutiveErrors = 0;
  m_connectinfo.isConnected  = &m_isConnected;
  m_connectinfo.acceptor     = &m_acceptor;
  m_connectinfo.socketstream = &m_socketstream;
  m_connectinfo.addrLocal    = &m_addrLocal;
  m_connectinfo.addrRemote   = &m_addrRemote;

  ACE_Thread_Manager::instance()->spawn((ACE_THR_FUNC)connectproc, 
      (void*)&m_connectinfo, THR_NEW_LWP|THR_JOINABLE, NULL, NULL, 
       ACE_DEFAULT_THREAD_PRIORITY, GROUPID_SOCKLISTENER); 

  return 0;
}



static void* connectproc(void* arg)          
{
  // Static thread proc for the connect thread. The thread exits when either
  // a connection is made, or this logger plugin is unplugged. We time out
  // every few seconds in order to check for and respond to a quit request,

  MmsLogDestinationSocket::SocketConnectInfo* p = 
 (MmsLogDestinationSocket::SocketConnectInfo*)arg;

  p->isThreadActive = 1;
  p->threadhandle = ACE_Thread_Manager::instance()->thr_self();
  int   n = 0;
  const int CONNECT_TIMEOUT_SECS = 3;

  while(1)
  {
    if  (p->isQuitRequest) break;           // Media server shutting down?

    MmsTime timeout (CONNECT_TIMEOUT_SECS); // Show reminder once/hr
    if  (n++ % (3600/CONNECT_TIMEOUT_SECS) == 0)
         ACE_OS::printf("TCPL thread %d waiting for connection at port %d\n", 
                         p->threadhandle, p->addrLocal->get_port_number()); 
                                            // Look for a connection
    if  (-1 == (p->acceptor->accept(*(p->socketstream), p->addrRemote, &timeout)))
         continue;
                                            // Indicate we're connected 
    *(p->isConnected) = 1;                  // so that logging can commence
    break;                                  // Exit connect loop
  }

  if  (p->isQuitRequest);
  else ACE_OS::printf("TCPL connected from %s %d\n", 
               p->addrRemote->get_host_addr(), p->addrRemote->get_port_number()); 
  p->isThreadActive = 0;
  return 0;                                 // Exit thread
}



void MmsLogDestinationSocket::disconnect()  // Disconnect the socket
{ 
  m_socketstream.close();
  m_isConnected = 0;
}

 

int MmsLogDestinationSocket::close()        // Close this logger plugin
{ 
  if  (MmsLogDestination::close() == 1) return -1;
  if  (this->isConnected())
       this->disconnect();

  if  (m_connectinfo.isThreadActive)
  {    m_connectinfo.isQuitRequest = 1;     // Wait on connector thread exit
       ACE_OS::printf("TCPL closing tcp logger\n");
       ACE_Thread_Manager::instance()->wait_grp(GROUPID_SOCKLISTENER);
  }

  return 0;
}



void MmsLogDestinationSocket::init() 
{ 
  ACE_ASSERT(m_config);
  m_isConnected  = m_isSetLocal = 0;
}



int MmsLogDestinationSocket::setlocaladdr(const int port)
{
  int  result;
  //if  (-1 == (result = m_addrLocal.set(port, INADDR_ANY)))
if  (-1 == (result = m_addrLocal.set(port)))
       printf("TCPL bad local address %d\n", port);
  else     
  if  (-1 == (result = m_acceptor.open(m_addrLocal)))
       printf("TCPL socket open error\n");     
  else result = m_isSetLocal = 1;
  return result;
}



MmsLogDestinationSocket::MmsLogDestinationSocket(MmsConfig* cfig):
  MmsLogDestination(MmsLogDestination::SOCKET, "SOCKET"), m_config(cfig) 
{  
  this->init();
}


                                            // Ctor
MmsLogDestinationSocket::MmsLogDestinationSocket
( MmsConfig* cfig, const int port): 
  MmsLogDestination(MmsLogDestination::SOCKET, "SOCKET"), m_config(cfig) 
{
  this->init();
  this->open(port);
} 


