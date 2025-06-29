// 
// mqClientTestCases.cpp - client process to exercise the MmsMqAppAdapter  
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mqClientTest.h"

extern MmsConfig*     config;
extern MmsMqWriter*   writer;
extern MmsMq*         reader;
extern MmsAppMessage* appxml;
extern MmsAppMessage* outxml;
extern QUEUEHANDLE clientID;
extern int  state;
extern int  command;
extern int  transID; 
extern int  provisoConxID;
extern int  returnPort;
extern int  interCommandSleepMs;  
extern int  isShowHeartbeatContent; 
extern int  heartbeatInterval;
extern int  isNoShowInboundXml;
extern int  isShowProvisionalContent;
extern int  isPassthruProvisional; 
extern char queueName[32];
extern char machineName[128];
extern char returnIP[32];



void sendConnectF(MmsAppMessage* outxml, const int sno, const int port, const char* s)
{
  // Send a connect command to start an IP session
  outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port);
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  sprintf(getbuf(),"TEST sending session %d %s\n",sno, s);          
  log();
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}


void sendConnectG(MmsAppMessage* outxml, const int conxID, const int confID)
{
  // Send a connect to join the indicated connection to the indicated conference
  outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID);
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], confID); 
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  printf("TEST sending conxID %d join conference %d\n",conxID,confID);
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}


void sendConnectH(MmsAppMessage* outxml, const int sno, const int port, const int confID)
{
  outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], confID); 
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port);
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  printf("TEST sending session %d connect and join conference %d\n",sno,confID);
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}


void sendDisconnectA(MmsAppMessage* outxml, const int conxID, const int confID)
{
  outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);    
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], confID);   
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  printf("TEST sending conx %d leave conference %d\n",conxID,confID);
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}



void sendDisconnectB(MmsAppMessage* outxml, const int conxID)
{
  outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);    
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  printf("TEST sending conx %d disconnect and implicit leave conference\n",conxID);
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}


int conferenceSequenceG(const unsigned int serverconnect) { return -1; }



int conferenceSequenceF(const unsigned int serverconnect)  // (n)
{ 
  // This is a concurrent multi-conference test. Test can loop -- to do so,
  // change constant NUMITERATIONS and recompile.

  // This test requires 12 connections. If not available, a server busy
  // error will be received here from media server

  static int iters, isserverconnect, testIterations, port = 1000, conxIds[64], confIds[8];
  char* p = NULL;
  const int BATCHSIZE = 6, NUMITERATIONS = 1;
  if  (iters++ == 0) 
  {    sprintf(getbuf(),"\nBegin concurrent conference test iteration 1 of %d\n\n", NUMITERATIONS);          
       log();
  }
  int   n = 0, result = 1, isThereAnOutgoingMessage = 1, i;
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;     

  // FILE* log = freopen("C:\\Documents and Settings\\Administrator\\Desktop\\mqclienttest.log","w", stdout);

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1); 
         break;

    case 1:                                  
         if  (isserverconnect) clientID = getMessageClientID();  
       
         for(i=0; i < (BATCHSIZE-1); i++)   // Send batch of five ip connects
         {   sendConnectF(outxml, i, port+=2, "connect");
             delete outxml;
             outxml = new MmsAppMessage;
         }
                                            // ... and a sixth
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port+=2);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending session 5 connect\n");
         break;

    case 2:                                 // Get result of connect 1
         showResultCode();
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP(); isThereAnOutgoingMessage = 0;
         break;
    case 3:
         showResultCode();                  // Get result of connect 2
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP(); isThereAnOutgoingMessage = 0;
         break;
    case 4:
         showResultCode();                  // Get result of connect 3
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP(); isThereAnOutgoingMessage = 0;
         break;
    case 5:
         showResultCode();                  // Get result of connect 4
         if  (-1 == (conxIds[3] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP(); isThereAnOutgoingMessage = 0;
         break;
    case 6:
         showResultCode();                  // Get result of connect 5
         if  (-1 == (conxIds[4] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP(); isThereAnOutgoingMessage = 0;
         break;

    case 7:
         showResultCode();                  // Get result of connect 6
         if  (-1 == (conxIds[5] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                            // Send create conference
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         printf("TEST sending conxID %d create conference\n", conxIds[0]);
         break;

    case 8:
         showResultCode();                  // Get result of conference create
         if  (-1 == (confIds[0] = getExpectedConferenceID())) return -1;

         for(i=1; i <= (BATCHSIZE-2); i++)  // Send batch of four conference joins
         {   sendConnectG(outxml, conxIds[i], confIds[0]);
             delete outxml;
             outxml = new MmsAppMessage;
         }
                                            // Send the fith join
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[5]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], confIds[0]); 
         printf("TEST sending conxID %d join conference %d\n",conxIds[5],confIds[0]);
         break;

    case 9:
         showResultCode();                  // Get result of conference join 1 of 5
         isThereAnOutgoingMessage = 0;
         break;
    case 10:
         showResultCode();                  // Get result of conference join 2 of 5
         isThereAnOutgoingMessage = 0;
         break;
    case 11:
         showResultCode();                  // Get result of conference join 3 of 5
         isThereAnOutgoingMessage = 0;
         break;
    case 12:
         showResultCode();                  // Get result of conference join 4 of 5
         isThereAnOutgoingMessage = 0;
         break;

    case 13:
         showResultCode();                  // Get result of conference join 5 of 5
                                            // Send create session 7 create conf 2
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port+=2);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session 7 connect/create conference\n");
         break;

    case 14:    
         showResultCode();                  // Get result of session 7 create conf 2
         if  (-1 == (conxIds[6] = getExpectedConnectionID())) return -1;
         if  (-1 == (confIds[1] = getExpectedConferenceID())) return -1;

         for(i=7; i < (7 + (BATCHSIZE-2)); i++)   // Send batch of four session connect
         {   sendConnectH(outxml, i, port+=2, confIds[1]); // plus conference join
             delete outxml;
             outxml = new MmsAppMessage;
         }
                                            // Send session 12 connect + join conf 2
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], confIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port+=2);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session %d connect and join conference %d\n",i,confIds[1]);
         break;

    case 15:
         showResultCode();                  // Get result of connect + join 1 of 5
         if  (-1 == (conxIds[7] = getExpectedConnectionID())) return -1;
         isThereAnOutgoingMessage = 0;
         break;
    case 16:
         showResultCode();                  // Get result of connect + join 2 of 5
         if  (-1 == (conxIds[8] = getExpectedConnectionID())) return -1;
         isThereAnOutgoingMessage = 0;
         break;
    case 17:
         showResultCode();                  // Get result of connect + join 3 of 5
         if  (-1 == (conxIds[9] = getExpectedConnectionID())) return -1;
         isThereAnOutgoingMessage = 0;
         break;
    case 18:
         showResultCode();                  // Get result of connect + join 4 of 5
         if  (-1 == (conxIds[10] = getExpectedConnectionID())) return -1;
         isThereAnOutgoingMessage = 0;
         break;

    case 19:
         showResultCode();                  // Get result of connect + join 5 of 5
         if  (-1 == (conxIds[11] = getExpectedConnectionID())) return -1;
                 

         for(i=0; i < (BATCHSIZE-1); i++)   // Send batch of five conference 1 leaves
         {   sendDisconnectA(outxml, conxIds[i], confIds[0]);  
             delete outxml;
             outxml = new MmsAppMessage;
         }
                                            // Send the sixth leave
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);    
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[5]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], confIds[0]);   
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
         printf("TEST sending conx %d leave conference %d\n",conxIds[5],confIds[0]);
         break;

    case 20:
         showResultCode();                  // Get result of leave 1 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 21:
         showResultCode();                  // Get result of leave 2 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 22:
         showResultCode();                  // Get result of leave 3 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 23:
         showResultCode();                  // Get result of leave 4 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 24:
         showResultCode();                  // Get result of leave 5 of 6
         isThereAnOutgoingMessage = 0;
         break;

    case 25:
         showResultCode();                  // Get result of leave 6 of 6
          
         for(i=6; i < (6 + (BATCHSIZE-1)); i++)   // Send batch of five session discos
         {   sendDisconnectB(outxml, conxIds[i]); // with implicit conference disco 
             delete outxml;
             outxml = new MmsAppMessage;
         }

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);    
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[11]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
         printf("TEST sending conx %d disconnect and implicit leave conference\n",conxIds[11]);
         break;
     
    case 26:
         showResultCode();                  // Get result of disco/leave 1 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 27:
         showResultCode();                  // Get result of disco/leave 2 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 28:
         showResultCode();                  // Get result of disco/leave 3 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 29:
         showResultCode();                  // Get result of disco/leave 4 of 6
         isThereAnOutgoingMessage = 0;
         break;
    case 30:
         showResultCode();                  // Get result of disco/leave 5 of 6
         isThereAnOutgoingMessage = 0;
         break;

    case 31:                                // Disco from server 
         if  (-1 == checkResultCode()) return -1;

         if  (++testIterations < NUMITERATIONS) 
         {
              port = 1000;
                                            // Send batch of five ip connects
              for(i=0; i < (BATCHSIZE-1); i++)   
              {   sendConnectF(outxml, i, port+=2, "connect");
                  delete outxml;
                  outxml = new MmsAppMessage;
              }
                                            // ... and a sixth
              outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port+=2);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
              printf("TEST sending session 5 connect\n");
              mmsSleep(1);
              state = 1;                    // Loop back to begin another test iteration
         }
         else                               // We're done
         {    if  (serverconnect & SERVER_DISCO)
                   sendServerDisconnect(outxml); 
              testIterations = 0;

              result = -1; 
         }
  }


  if  (isThereAnOutgoingMessage)
  {
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       log("Outbound XML\n");
       sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();   
                                            
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int conferenceDisconnectTestA(const unsigned int serverconnect) // (o)
{
  // This test sequence connects to server, connects a few sessions to a single
  // conference, disconnects from conference implicitly closing all sessions
  // plus the conference, and disconnects server
  // This test was enhanced to pass server ID, testing media server ability
  // to recognize server ID, strip off server ID and reinsert it on return.

  static int iters, isserverconnect, conferees, confID, conxIds[4], serverID=1;
  if  (iters++ == 0) printf("\nBegin conference disco test A\n\n");
  char* p = NULL;
  int   n = 0, id=0, result = 1; 
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml, serverID);  // <<= server ID
         else return 0 == (state = 1);   
         break;

    case 1:                                 // Send a half connect
         if (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 

         // We would not usually include a connection ID with a half connect.
         // Assuming it is zero, it should make no difference. However herer
         // we are going to attach server ID to the connection ID. Media server
         // *should* still treat it a zero, or not present.
         
         // outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], serverID << 24);
         log("TEST sending party A half connect\n");
         break;

    case 2:                                 // Send full connect
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         // Now, the connection and conference IDs that come back will have 
         // server ID embedded -- we do not bother to strip it off, since we
         // wil use it this way the next time we send it to the media server.

         showReturnedPortAndIP();
         sprintf(getbuf(),"Returned connection ID was %d\n", conxIds[0]);          
         log();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending party A full connect\n");              
         break;

    case 3:                                 // Send conference connect
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0);
         log("TEST sending part A conference connect 1\n");
         break;    


    case 4:                                // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;         
         if  (-1 == (confID = getExpectedConferenceID())) return -1;
         sprintf(getbuf(),"Returned conference ID was %d\n", confID); log();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);        
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending session/conference connect 2\n");
         break;

    case 5:                                 // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         sprintf(getbuf(),"Returned connection ID was %d\n", conxIds[1]); log();

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         log("TEST sending session/conference connect 3\n");
         break;

    case 6:                                 // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         sprintf(getbuf(),"Returned connection ID was %d\n", conxIds[2]); log();

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1006);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending session/conference connect 4\n");
         break;

    case 7:
         showResultCode();                  // Send multisession disconnect
         conxIds[3] = getExpectedConnectionID(); 
         sprintf(getbuf(),"Returned connection ID was %d\n", conxIds[3]); log();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         log("TEST sending conference disconnect for all connections\n");              
         break;

    case 8:                                 // Disconnect from media server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  log("Outbound XML\n");
  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();  // show xml
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int conferenceSequenceE(const unsigned int serverconnect)  // (m)
{ 
  // This test sequence connects to server, connects two sessions to a single
  // conference, plays an announcement to party 2, discos both.
  // if IS_MONITOR_TEST is set, party 2 is a conference monitor.
  // These should be two tests, but we're running out of single-letter selections

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];

  //#define IS_MONITOR_TEST
  #ifdef  IS_MONITOR_TEST
  if  (iters++ == 0) log("\nBegin play to conference monitor test\n\n");
  #else 
  if  (iters++ == 0) log("\nBegin play to conferee test\n\n");
  #endif

  char* p = NULL;
  int   n = 0, result = 1; 
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;
                                            // Send a session connect
    case 1:                                 // with conference connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         log("TEST sending session/conference connect 1\n");
         break;    

    case 2:                                // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 

         #ifdef IS_MONITOR_TEST
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
                              MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::MONITOR]); 
         log("TEST sending session/conference connect 2 as conf monitor\n");
         #else 
         log("TEST sending session/conference connect 2\n");;
         #endif         
         break;

    case 3:                                 // Play to second conferee
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         log("TEST sending play\n");              
         break;

    case 4:                                 // Disco 1
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         sprintf(getbuf(),"TEST sending conference/session disconnect for connection %d\n",conxIds[1]);
         log();              
         break;

    case 5:                                 // Disco 0
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         sprintf(getbuf(),"TEST sending conference/session disconnect for connection %d\n",conxIds[0]); 
         log();
         break;            
 
    case 6:                                 // Disconnect from media server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}



int conferenceSequenceD(const unsigned int serverconnect)
{
  // This test sequence connects to server, connects a few sessions to a single
  // conference, disconnects each session one by one from the conference without 
  // closing the sessions, then disconnects the sessions, and disconnects server.
  // Modified to play to host after host leaves conference, to test resolution of
  // an observed situation where play was inaudible in such a case.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conferencing test C\n\n");
  char* p = NULL;
  int   n = 0, result = 1; 
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;
                                            // Send a session connect
    case 1:                                 // with conference connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         printf("TEST sending session/conference connect 1\n");
         break;    

    case 2:                                // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         printf("TEST sending session/conference connect 2\n");
         break;

    case 3:                                 // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (n = getExpectedConferenceID())) return -1;
         if  (n != conferenceID) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], n);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         printf("TEST sending session/conference connect 3\n");
         break;

    case 4:                                 // Leave session 2 from conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);

         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (n = getExpectedConferenceID())) return -1;
         if  (n != conferenceID) return -1;

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
         printf("TEST sending conference disconnect for connection %d\n",conxIds[2]);              
         break;

    case 5:                                 // Leave session 1 from conf
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         printf("TEST sending conference disconnect for connection %d\n",conxIds[1]);              
         break;

    case 6:                                 // Leave session 0 from conf
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         printf("TEST sending conference disconnect for connection %d\n",conxIds[0]);              
         break;

    case 7:
         showResultCode();                  // Play to session 0
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::FILE_NAME], "thankyou.vox"); 
         printf("TEST sending session 1 play\n");
         break;

    case 8:
         showResultCode();                  // Disco session 2
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
         printf("TEST sending session disconnect for connection %d\n",conxIds[2]);
         break;

    case 9:
         showResultCode();                  // Disco session 1
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         printf("TEST sending session disconnect for connection %d\n",conxIds[1]);
         break;

    case 10:
         showResultCode();                  // Disco session 0
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         printf("TEST sending session disconnect for connection %d\n",conxIds[0]);
         break;

    case 11:                                // Disconnect from media server                                      
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int conferenceSequenceC(const unsigned int serverconnect) // (k)
{
  // This test sequence connects to server, connects a few sessions to a single
  // conference, disconnects each session from the conference, closing the
  // sessions as it does so, and disconnects server

  static int iters, isserverconnect, conferees, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conferencing test C\n\n");
  char* p = NULL;
  int   n = 0, result = 1; 
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;
                                            // Send a session connect
    case 1:                                 // with conference connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         printf("TEST sending session/conference connect 1\n");
         break;    

   case 2:                                 // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (n = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], n);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         printf("TEST sending session/conference connect 2\n");
         break;

   case 3:                                 // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (n = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], n);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         printf("TEST sending session/conference connect 3\n");
         break;

   case 4:                                 // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (n = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], n);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1006);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         printf("TEST sending session/conference connect 4\n");
         break;

    case 5:
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);

         if  (-1 == (conxIds[3] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[3]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[3]);              
         break;

    case 6:
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[2]);              
         break;

    case 7:
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[1]);              
         break;

    case 8:
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[0]);              
         break;

    case 9:                                 // Disconnect from media server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int conferenceSequenceB(const unsigned int serverconnect)
{
  // This test sequence connects to server, connects a session and conference,
  // simultaneously, disconnects session from conference, and session,
  // simultaneously, disconnects server

  static int iters, isserverconnect, serverID=1;
  if  (iters++ == 0) printf("\nBegin conferencing test B\n\n");
  char* p = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
         case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml, serverID);  // <<= server ID
         else return 0 == (state = 1);   
         break;  
                                            // Send a session connect
    case 1:                                 // with conference connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
                                            // Set "tone on r/o" conference attr
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ATTRIBUTE], 
                  // MmsAppMessageX::conferenceAttrNames[MmsAppMessageX::SOUND_TONE_WHEN_RECEIVE_ONLY]); 
                     MmsAppMessageX::conferenceAttrNames[MmsAppMessageX::SOUND_TONE]);
         log("TEST sending connect session to new conference\n");
         // log("TEST also setting SOUND TONE R/O conference attribute\n");
         log("TEST also setting SOUND TONE conference attribute\n");
         break;
                                            // Disconnect session from conference
    case 2:                                 // and disconnect session
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         if (-1 == (n = getExpectedConnectionID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n);  
         log("TEST sending conference and session disconnects\n");              
         break;

    case 3:                                 // Disconnect from media server
         showResultCode(); 
         if (serverconnect & SERVER_DISCO)
             sendServerDisconnect(outxml);            
         result = -1;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int conferenceSequenceA(const unsigned int serverconnect)  // 'i'
{
  // This test sequence connects to server, connects a session, 
  // connects session to conference,  disconnects session from conference,
  // disconnects session, disconnects server. 
  // Also tested here is settting of the "no tone" conference attribute,

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin conferencing test A\n\n");
  char* p = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;

    case 1:                                 // Send an IP connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session connect\n");
         break;

    case 2:                                 // Connect session to a new conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
                                            // Set "no tone" conference attr
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ATTRIBUTE], 
                              MmsAppMessageX::conferenceAttrNames[MmsAppMessageX::NO_TONE]); 
         printf("TEST sending connect session to new conference\n");
         printf("TEST also setting NO TONE conference attribute\n");
         break;

    case 3:                                 // Disconnect session from conference
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         if  (-1 == (n = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], n); 
         printf("TEST sending conference disconnect\n");              
         break;

    case 4:                                 // Disconnect session
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         printf("TEST sending session disconnect\n");              
         break;

    case 5:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}




int announceAndJoinTwoPartyConference()
{
  // Test one of use case wherein when app creates a conference, voice
  // does not flow unless the parties involved have been played a vox
  // announcement prior to joining the conference. 
  // This test sequence connects to server, connects two sessions, plays
  // an announcement on session 1, creates a conference with session 1,
  // plays announcement to session 2, moves session 2 into conference,
  // waits for input to continue, and discos.

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin play and join conference test\n\n");
  char* p = NULL, c=0;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  static char ipA[]="127.0.0.1", ipB[]="127.0.0.1", playfile[]="thankyou.vox";
  static int portA = 1000,      portB = 1002, conxIds[2] = {0,0};

  switch(state)
  {
    case 0:                                 // Connect to media server
         sendServerConnect(outxml);
         isserverconnect = TRUE;
         break;

    case 1:                                 // Send session 1 connect 
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], portA);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], ipA); 
         printf("TEST sending session 1 connect\n");
         break;

    case 2:                                 // Send session 2 connect 
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], portB);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], ipB); 
         printf("TEST sending session 2 connect\n");
         break;

    case 3:                                 // Play to session 1
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::FILE_NAME], playfile); 
         printf("TEST sending session 1 play\n");
         break;

    case 4:                                 // Play to session 2
         checkResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::FILE_NAME], playfile); 
         printf("TEST sending session 2 play\n");
         break;

    case 5:                                 // Connect session 1 to a new conference
         checkResultCode();                  
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         printf("TEST sending connect session 1 to new conference\n");
         break;

    case 6:                                 // Join session 2 to that conference
         checkResultCode();                          
         if  (-1 == (n = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], n); 
         printf("TEST sending connect session 2 to that conference\n");
         break;

    case 7:                                 // Wait for input and bail
         checkResultCode(); 
         WAITFORKEY("\nany key ..."); 
         sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int doNotAnnounceTwoPartyConference()
{
  // Test two of use case wherein when app creates a conference, voice
  // does not flow unless the parties involved have been played a vox
  // announcement prior to joining the conference. 
  // This test sequence connects to server, connects two sessions, 
  // moves each into conference, waits for input, and bails.

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin join conference without announce test\n\n");
  char* p = NULL, c=0;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  static char ipA[]="127.0.0.1", ipB[]="127.0.0.1", playfile[]="thankyou.vox";
  static int portA = 1000,      portB = 1002, conxIds[2] = {0,0};

  switch(state)
  {
    case 0:                                 // Connect to media server
         sendServerConnect(outxml);
         isserverconnect = TRUE;
         break;

    case 1:                                 // Send session 1 connect 
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], portA);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], ipA); 
         printf("TEST sending session 1 connect\n");
         break;

    case 2:                                 // Send session 2 connect 
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], portB);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], ipB); 
         printf("TEST sending session 2 connect\n");
         break;

    case 3:                                 // Connect session 1 to a new conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
            
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         printf("TEST sending connect session 1 to new conference\n");
         break;

    case 4:                                 // Join session 2 to that conference
         checkResultCode();                          
         if  (-1 == (n = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], n); 
         printf("TEST sending connect session 2 to that conference\n");
         break;

    case 5:                                 // Wait for input and bail
         checkResultCode(); 
         WAITFORKEY("\nany key ...");
         sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int  confereeSetAttrA(const unsigned int serverconnect)
{
  // This test sequence connects to server, connects a few sessions to a single
  // conference, mutes one conferee, waits, unmutes conferee, waits,
  // disconnects from conference, and disconnects server

  static int iters, isserverconnect, conferees, confID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference disco test A\n\n");
  char* p = NULL;
  int   n = 0, result = 1; 
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;
                                            // Send session 1 connect
    case 1:                                 // with conference connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         printf("TEST sending session/conference connect 1\n");
         break;    

    case 2:                                 // Connect session 2 to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (confID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session/conference connect 2\n");
         break;

    case 3:                                 // Connect session 3 to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session/conference connect 3\n");
         break;

    case 4:                                 // Connect session 4 to conf
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1006);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session/conference connect 4\n");
         break;

    case 5:
         showResultCode();                  // Mute session 4
         conxIds[3] = getExpectedConnectionID(); 
         printf("TEST 4 parties connected ... wait 5 secs and mute party 3 ...\n");
         mmsSleep(5);                      

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONFEREESETATTR]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
                                            // <field name="receiveOnly>1</field>
         outxml->putParameter(MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::RECEIVE_ONLY], 1);
         printf("TEST sending conferee set attribute for party 3\n");              
         break;

    case 6:                                 // Unmute session 4
         if  (-1 == checkResultCode()) return -1;
         printf("TEST session 3 is mute ... wait 5 secs and unmute ...\n");
         mmsSleep(5); 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONFEREESETATTR]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
                                            // <field name="receiveOnly>0</field>
         outxml->putParameter(MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::RECEIVE_ONLY], 0);
         printf("TEST sending conferee set attribute for party 3\n");      
         break;

    case 7:
         showResultCode();                  // Send multisession disconnect
         printf("TEST session 3 umuted ... wait 5 secs and disco ...\n");
         mmsSleep(5); 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], confID);
         printf("TEST sending conference disconnect for all connections\n");              
         break;

    case 8:                                 // Disconnect from media server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}


