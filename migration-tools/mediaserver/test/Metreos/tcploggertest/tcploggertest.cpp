//
// tcploggertest.cpp
//
// provides a socket client to watch for media server tcp logger connections
// gather and display log messages from media server
//
#include "StdAfx.h"
#include "c:\workspace\metreos-mediaserver\include\Metreos\mmsConfig.h"
#include "ace/SOCK_Connector.h"
#include "ace/SOCK_Stream.h"
#include <conio.h>
#include <minmax.h>
#define MAXLOGRECSIZE 160
#define CONNECT_TIMEOUT_SECS  10
#define CONNECT_MESSAGE_EVERY 20


class Listener
{
  public:

  Listener() { m_stop = m_isSet = 0; m_isstopped = 0; m_bufp = buf; m_recvsize = MAXLOGRECSIZE; }

  int open()
  {
    int  result;
    if  (!m_isSet) 
         result = -1;      
    else result = this->connectServer(CONNECT_TIMEOUT_SECS);      
    return result;
  }



  int connectServer(const int timeoutsecs)
  {
    static int n;
    if  (n++ % CONNECT_MESSAGE_EVERY == 0)
         printf("SOCK attempting connect to %s %d ...\n",
                 m_remote_address.get_host_addr(), m_remote_address.get_port_number());
    MmsTime timeout(CONNECT_TIMEOUT_SECS);

    int  result = m_connector.connect(m_socketstream, m_remote_address, &timeout);

    if  (result != -1) printf("SOCK connected\n");
    return result;
  }



  int receive()
  {
    while(1)
    {
      if  (m_stop) break;
      bufinit();
      static const char* q = buf + sizeof(buf);
      const static int TIMEOUTSECS = CONNECT_TIMEOUT_SECS;
      ACE_Time_Value timeout(TIMEOUTSECS);
      const int maxchars = q - m_bufp; 

      static int n;
      if  (n++ % 10)
           printf("SOCK listening at %d ... \n", m_remote_address.get_port_number());

      int  bytecount = m_socketstream.recv_n(buf, maxchars, 0, &timeout);

      if  (bytecount > 0)
           this->printif(bytecount);
      else
      {    // printf("SOCK error recv\n");
           mmsSleep(TIMEOUTSECS);
      }
    }

    m_isstopped = TRUE;
    return 0;
  }



  void close()
  {
    m_socketstream.close();
  }



  void stop() { m_stop = TRUE; }

  int  isStopping() { return m_stop; }
  int  isStopped()  { return m_isstopped; }
  void stopped()    { m_isstopped = 1; }


  int setremoteaddr(char* ip, int port)
  {
    m_remote_address.set(port, ip, 1, AF_INET);
    m_isSet = 1;
    return 0;    
  }

  private:
  int   m_stop, m_isSet, m_isstopped, m_recvsize; 
  char* m_bufp;
  ACE_INET_Addr      m_remote_address;
  ACE_SOCK_Connector m_connector;
  ACE_SOCK_Stream    m_socketstream;
  char buf[MAXLOGRECSIZE];

  void bufinit() 
  { 
    static const char* q = buf + sizeof(buf);
    const int currentbuflen = q - m_bufp;
    memset(m_bufp,' ',currentbuflen);  
  }

  int  printif(const int bytesreceived)     // Print complete rec and delete
  {
    printf("SOCK received %d bytes\n",bytesreceived); // ******************
    const int bytesalreadyread = m_bufp - buf;
    m_bufp += bytesreceived;               // Adjust buffer pointer
    static const char* q = buf + sizeof(buf);
    char* p = buf;
    while(p < q && *p) p++;                 // Find null term if any
    if   (p >=q) return 0;

    printf(buf);                            // Display logrec
    const int bytesprinted  = p - buf;
    const int remainingsize = MAXLOGRECSIZE - bytesprinted;
    memmove (buf, ++p, remainingsize);      // Copy remainder over logrec
                                            // Readjust buffer pointer
    m_bufp = buf + ((bytesalreadyread + bytesreceived) -  bytesprinted);
    return bytesprinted;
  }
};



void svc(void* args)                        // Socket listener threadproc
{
  Listener* listener = (Listener*)args;
  int  result = -1;

  while(result == -1 && !listener->isStopping())
        result = listener->open();

  if  (listener->isStopping()) { listener->stopped(); return; }

  listener->receive();

  listener->close();
  return;
}



int main(int argc, char *argv[])
{
  MmsConfig* config = new MmsConfig;  
  #define  BUFSIZE 1024
  char c, buf[BUFSIZE]; buf[BUFSIZE-1]=0;      

  if  (config->readLocalConfigFile() == -1) 
  {    printf("Could not read config\n"); 
       return 0;
  }

  char* ipaddr = config->serverLogger.remoteAddress;
  int   nport  = config->serverLogger.remotePort;
  int   connectIntervalSecs = config->serverLogger.socketConnectIntervalSeconds; 

  Listener* listener = new Listener;

  if  (-1 == listener->setremoteaddr(ipaddr, nport)) return 0;
  printf("MAIN hit x at any time to exit ...\n\n");

  _beginthread(svc, 0, listener);
   
  while(1)
  {  
    c = 0;
    while(!c) c = _getch(); 
    if (c == 'x') break;
  }

  printf("\nMAIN shutdown initiated ...\n"); 
  listener->stop(); 
  while(!listener->isStopped()) mmsSleep(MMS_HALF_SECOND); 
  delete listener;
  delete config;

  printf("\nMAIN shutdown complete; any key to exit ... ");
  c=0; while(!c) c = _getch();
  return 0;
}