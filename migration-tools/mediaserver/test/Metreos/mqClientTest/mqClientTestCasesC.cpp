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

static char *connectcommand, *playcommand, *recordcommand, *adjustplaycommand, *conxidparamname;
static char *confidparamname, *termcondparamname, *disconnectcommand, *stopmediacommand;  
static char *filenameparamname, *volumeparamname, *speedparamname, *adjtypeparamname, *togtypeparamname;
static char *portparamname, *ipparamname, *transidparamname, *cmdtimeoutparamname;

#define ID_TIMEOUTCONFPLAY 53450

void setGlobalsC()
{
  MmsAppMessageX::instance();
  connectcommand    = MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT];
  disconnectcommand = MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT];
  recordcommand     = MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD];
  playcommand       = MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY];
  adjustplaycommand = MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_ADJUST_PLAY];
  stopmediacommand  = MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA];
  conxidparamname   = MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID];
  termcondparamname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
  filenameparamname = MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME];
  volumeparamname   = MmsAppMessageX::paramnames[MmsAppMessageX::VOLUME];
  speedparamname    = MmsAppMessageX::paramnames[MmsAppMessageX::SPEED];
  adjtypeparamname  = MmsAppMessageX::paramnames[MmsAppMessageX::ADJUST_TYPE];
  togtypeparamname  = MmsAppMessageX::paramnames[MmsAppMessageX::TOGGLE_TYPE];
  conxidparamname   = MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID];
  confidparamname   = MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID];
  portparamname     = MmsAppMessageX::paramnames[MmsAppMessageX::PORT];
  ipparamname       = MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS];
  transidparamname  = MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID];
  cmdtimeoutparamname = MmsAppMessageX::paramnames[MmsAppMessageX::COMMAND_TIMEOUT];
}




int conferencePlayToAll(const unsigned int serverconnect) // 'w'
{ 
  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference announcement test A\n\n");
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
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 2\n");
         break;

    case 3:                                 // Play audio to conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         //#define TEST_PRERECORDED_PLAYFILE "011F558E.vox"
         #ifdef  TEST_PRERECORDED_PLAYFILE 
         p = TEST_PRERECORDED_PLAYFILE;
         #else
         p = "thankyou.vox";
         #endif

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], p);          

         sprintf(getbuf(),"TEST sending play of '%s' to conference\n", p); log();
         break;
 
    case 4:                                 // Abandon connections & disco server
         if  (-1 != checkResultCode() 
           && -1 != (conxIds[2] = getExpectedConnectionID())) 
              printf("TEST PLAYTOCONF returned conxID %d\n",conxIds[2]);
         else printf("TEST PLAYTOCONF FAILED\n");

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



int conferenceRecordBargeIn(const unsigned int serverconnect) 
{ 
  // Establish 2-party conference. Record the conference to arbitrary file
  // Pause. Send a stop media operation on the record connection. Hang up.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference barge-in test A\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1, conxID, isThereAnOutboundMessage = 1; 
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

    case 3:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 10000");   // 10 seconds failsafe

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending record conference\n");    

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         
         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(5);                       // Wait 5 seconds and stop recording          
             
         conxID = conxIds[1]+1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         printf("TEST sending stopMediaOperation on utility connection %d\n",conxID);                         
         break;
                                            
    case 4:                                 // Get result of play or stop
         checkResultCode();
         isThereAnOutboundMessage = 0;
         break;

    case 5:                                 // Get result of play or stop
         checkResultCode();
         if  (serverconnect & SERVER_DISCO) // Abandon conference
              sendServerDisconnect(outxml);            
         result = -1;
  }


  if  (isThereAnOutboundMessage)
  {                                          // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}



int conferenceRecordBargeInEx(const unsigned int serverconnect)  // (y)
{ 
  // Establish 2-party conference. Record the conference to arbitrary file
  // Add two more parties to the conference. Pause. Hang up.
  // Verify that (a) the hangup generated a stop media, (b) all abandoned 
  // connections were properly closed and released, and (c) all license
  // counts were restored to correct state.

  static int iters, isserverconnect, conferenceID, recorderConxID, conxIds[5];
  if  (iters++ == 0) log("\nBegin conference barge-in test B\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1; 
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
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 2\n");
         break;

    case 3:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 30000");   // 30 seconds failsafe
         isPassthruProvisional = TRUE;  // Cause provisional result to come thru this event loop
         log("TEST sending record conference\n");  
         break; 

    case 4:
         isPassthruProvisional = FALSE; 
         if (-1 == checkResultCode(1)) return -1;
         if (-1 == (conxIds[2] = getExpectedConnectionID())) return -1; 
         recorderConxID = conxIds[2];  // Connnection ID of recording drone conferee

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 3\n");
         break;

    case 5:
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[3] = getExpectedConnectionID())) return -1; 

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1006);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 4\n");
         break;

    case 6:                                  
         checkResultCode();
         log("TEST sending abandon of conference record, conference, and connections\n");
         if  (serverconnect & SERVER_DISCO) // Abandon record, conference, and connections
              sendServerDisconnect(outxml);            
         result = -1;
  }


  if  (isThereAnOutboundMessage)
  {                                         // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}



int conferenceRecordAll(const unsigned int serverconnect) 
{ 
  // Establish 2-party conference. Record the conference to named file
  // with timeout termination condition. Hang up.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference record test A\n\n");
  char* p = NULL, *termcondname = NULL;
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

    case 3:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"mmstestrecording.vox"); 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "silence 3000");   // 3 seconds silence
         printf("TEST sending record conference\n");              
         break;
 
    case 4:                                 // Abandon connections & disco server
         if  (-1 != checkResultCode() 
           && -1 != (conxIds[2] = getExpectedConnectionID())) 
              printf("TEST RECORDCONF returned conxID %d\n",conxIds[2]);
         else printf("TEST RECORDCONF FAILED\n");

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



int conferenceRecordAndCancel(const unsigned int serverconnect) 
{ 
  // Establish 2-party conference. Record the conference to named file.
  // Disco the conferees. Cancel the record session.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference record test A\n\n");
  char* p = NULL, *termcondname = NULL;
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

    case 3:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"mmstestrecording.vox"); 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "silence 30000");   // 30 seconds silence
         printf("TEST sending record conference\n");     

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message & wait 5
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());        
         delete outxml;
         outxml = new MmsAppMessage;                                            
         mmsSleep(5);          
                                            // Disco 1
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         printf("TEST sending conference/session disconnect conx %d\n",conxIds[0]);              
         break;

    case 4:                                 // Disco 2
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         printf("TEST sending conference/session disconnect conx %d\n",conxIds[1]);              
         break;

    case 5:                                 // Disco record sessions
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], provisoConxID);
         printf("TEST record utility session disconnect conx %d\n", provisoConxID);              
         break;

    case 6:                                 // Disco server
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



                                  
int conferenceAbandonRecording(const unsigned int serverconnect) // (F)
{ 
  // Establish 2-party conference. Record the conference to named file
  // with lengthy termination. Abandon the conference and ensure that the
  // utility session is among those automatically terminated.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) log("\nBegin conference record test A\n\n");
  char* p = NULL, *termcondname = NULL;
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
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 2\n");
         break;

    case 3:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"mmstestrecording.vox"); 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "silence 30000");   // 30 seconds silence
         isPassthruProvisional = TRUE;      // Route provisional thru state machine
         log("TEST sending record conference\n");   
         break;

    case 4:
         isPassthruProvisional = FALSE;     // Add a 3rd real party to conference  
         checkResultCode(1);                // in order to test case where utility session
                                            // was not the last conferee added to conference
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 3\n");
         break;

    case 5:                                 // Disco server connection
         showResultCode();                  // to test conference teardown                
         sendServerDisconnect(outxml);             
         result = -1;
         break;
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


                               
int abandonPlayToLoneConferee(const unsigned int serverconnect)   // (2)
{ 
  // Establish 1-party conference. Play to conferee. Simulate caller hangup
  // by stop media and full disco.
  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];

  //#define IS_MONITOR_TEST
  #ifdef  IS_MONITOR_TEST
  if  (iters++ == 0) log("\nBegin abandon single conf monitor play test\n\n");
  #else 
  if  (iters++ == 0) log("\nBegin abandon single party play test\n\n");
  #endif

  char* p = NULL, *termcondname = NULL;
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
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], 0); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         #ifdef IS_MONITOR_TEST
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
                              MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::MONITOR]); 
         log("TEST sending session/conference connect as conf monitor\n");
         #else 
         log("TEST sending session/conference connect 1\n");
         #endif
         break;    

    case 2:                                 // Play to conferee
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 20000");    
         log("TEST sending play to conferee\n");    
                                            
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message & wait 3
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());        
         delete outxml;
               
         mmsSleep(3);
         outxml = new MmsAppMessage;        // Send a blocking stop media                                        
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::BLOCK], 1);
         log("TEST sending stopMediaOperation\n");             
         break;

    case 3:                                 // Full disco
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         log("TEST sending session disconnect\n");              
         break;
 
    case 4:                                 // Disconnect from media server
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




int conferenceRecordTerminationTest(const unsigned int serverconnect) 
{ 
  // Ensure that a recorded conference stops recording after the last party
  // has left the conference.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference record test B\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
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

    case 3:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"mmstestrecording.vox"); 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "silence 60000");   // 60 seconds silence

         printf("TEST sending record conference\n");    
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());                                            
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         
         delete outxml;
         outxml = new MmsAppMessage;
         mmsSleep(5);
                                            // Disco 1
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         printf("TEST sending conference/session disconnect conx %d\n",conxIds[0]);  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());                                            
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
         mmsSleep(1);
                                            // Disco 2
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         printf("TEST sending conference/session disconnect conx %d\n",conxIds[1]);               
         break;

    case 4:                                  
         showResultCode();                  // Catch record return
         isThereAnOutboundMessage = 0;
         break;

    case 5:                                  
         showResultCode();                  // Catch disco 1 return
         isThereAnOutboundMessage = 0;
         break;

    case 6:                                 // Catch disco 2 return; disco server  
         showResultCode();   
         printf("TEST waiting 5 seconds ...\n");  
         mmsSleep(5);                                      
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

  if  (isThereAnOutboundMessage)
  {                                         // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}




int conferencePlayToConfereesA(const unsigned int serverconnect) 
{ 
  // Play audio to each conferee in turn

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin per conferee play test A\n\n");
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

    case 3:                                 // Play audio to party 1
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         printf("TEST sending play\n");              
         break;

    case 4:                                 // Disco 1
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         printf("TEST sending conference/session disconnect conx %d\n",conxIds[0]);              
         break;

    case 5:                                 // Play audio to party 2
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         printf("TEST sending play\n");              
         break;

    case 6:                                 // Disco 1
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[1]);              
         break;
 
    case 7:                                 // Disconnect from media server
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



int conferencePlayToConfereesB(const unsigned int serverconnect) // (v)
{            
  // Connects 2 parties to a conference, pauses, mutes second party,
  // plays audio to second party, pauses, unmutes second party,
  // pauses, plays audio to second party, pasues, disco party 1,
  // disco party 2

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) log("\nBegin conferee play test A\n\n");
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
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         log("TEST sending session/conference connect 2\n");
         break;  

    case 3:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                          // Pause and mute conferee 2
         log("TEST 2 parties connected ... wait 5 secs and mute party 2 ...\n");
         mmsSleep(5);                      

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONFEREESETATTR]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
                                            // <field name="receiveOnly>1</field>
         outxml->putParameter(MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::RECEIVE_ONLY], 1);
         log("TEST sending conferee set attribute for party 2\n");              
         break;

    case 4:                                 
         showResultCode();                  // Play audio to party 2 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         log("TEST sending party 2 play announcement\n");              
         break;

    case 5:
         showResultCode();                  // Pause and unmute party 2
         log("TEST wait 5 secs and unmute party 2 ...\n");
         mmsSleep(5);                      
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONFEREESETATTR]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
                                            // <field name="receiveOnly>1</field>
         outxml->putParameter(MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::RECEIVE_ONLY], 0);
         log("TEST sending conferee reset attribute for party 2\n");              
         break;

    case 6:                                 
         showResultCode();                  // Play audio to party 2
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         log("TEST sending party 2 play announcement\n");              
         break;

    case 7:                                 
         showResultCode();                  // Disco 2
         log("TEST wait 2 secs and disco party 2 ...\n"); 
         mmsSleep(3);                                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         sprintf(getbuf(),"TEST sending conference/session disconnect conx %d\n",conxIds[1]); log();             
         break;

    case 8:                                 // Disco 1
         showResultCode(); 
         log("TEST wait 2 secs and disco party 1 ...\n"); 
         mmsSleep(2);                                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         sprintf(getbuf(),"TEST sending conference/session disconnect conx %d\n",conxIds[0]); log();
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

  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}
 


int coachPupilTest(const unsigned int serverconnect) 
{
  // Connects 3 parties to a conference, the second being coach, the third pupil.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) log("\nBegin coach/pupil test A\n\n");
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

    case 2:                                // Connect coach session to conf
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
                                                 // <field name="confereeAttr>coach 1</field>
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
                              MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::COACH]); 
         log("TEST setting coach attribute for party 2\n");  
         log("TEST sending session/conference connect 2\n");
         break;

    case 3:                                // Connect pupil session to conf
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
                                                 // <field name="confereeAttr>pupil 1</field>
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
                              MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::PUPIL]); 
         log("TEST setting pupil attribute for party 3\n");  
         log("TEST sending session/conference connect 3\n");
         break;

    case 4:    
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();                                                                         
         log("TEST wait 2 secs and set coach attribute off on party 2 ...\n");
         mmsSleep(2);                      
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONFEREESETATTR]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
                                            // <field name="coach>0</field>
         outxml->putParameter(MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::COACH], 0);
         log("TEST sending conferee reset attribute (coach off) for party 2\n");              
         break;

    case 5:                                 
         showResultCode();                  // Disco 2
         log("TEST wait 2 secs and disco party 2 ...\n"); 
         mmsSleep(2);                                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         sprintf(getbuf(),"TEST sending conference/session disconnect conx %d\n",conxIds[1]); log();                  
         break;

    case 6:                                 // Disco 1
         showResultCode(); 
         log("TEST wait 2 secs and disco party 1 ...\n"); 
         mmsSleep(2);                                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         sprintf(getbuf(),"TEST sending conference/session disconnect conx %d\n",conxIds[0]); log();             
         break;

    case 7:                                 // Disco 3
         showResultCode(); 
         log("TEST wait 2 secs and disco party 3 ...\n"); 
         mmsSleep(2);                                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
         sprintf(getbuf(),"TEST sending conference/session disconnect conx %d\n",conxIds[2]); log();            
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

  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}



int coachPupilTestB(const unsigned int serverconnect) // (&) 
{
  // Connects 3 parties to a conference, the second being coach, the third pupil.
  // Then adds a fourth party whose attribute is also coach.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) log("\nBegin coach/pupil test B\n\n");
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

    case 2:                                // Connect coach session to conf
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
                                                 // <field name="confereeAttr>coach 1</field>
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
                              MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::COACH]); 
         log("TEST setting coach attribute for party 2\n");  
         log("TEST sending session/conference connect 2\n");
         break;

    case 3:                                 // Connect pupil session to conf
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
                                                 // <field name="confereeAttr>pupil 1</field>
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
                              MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::PUPIL]); 
         log("TEST setting pupil attribute for party 3\n");  
         log("TEST sending session/conference connect 3\n");
         break;

    //case 4:                                 // Connect second coach to conf, replacing first
    //     if  (-1 == checkResultCode()) return -1;
    //     if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
    //     showReturnedPortAndIP();                                                                         
    //     log("TEST wait 2 secs and connect another coach ...\n");
    //     mmsSleep(2);  
    //                
    //     outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
    //     outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
    //     outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1006);
    //     outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
    //                                             // <field name="confereeAttr>coach 1</field>
    //     outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
    //                          MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::COACH]); 
    //     log("TEST setting coach attribute for party 4\n");  
    //     log("TEST sending session/conference connect 4\n");            
    //     break;


    case 4:                                 // Connect second pupil to conf, replacing first
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();                                                                         
         log("TEST wait 2 secs and connect another pupil ...\n");
         mmsSleep(2);  
                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1006);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
                                                 // <field name="confereeAttr>coach 1</field>
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE], 
                              MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::PUPIL]); 
         log("TEST setting pupil attribute for party 4\n");  
         log("TEST sending session/conference connect 4\n");            
         break;

    case 5:                                 // Set tariff tone
         if  (-1 == checkResultCode()) return -1;
         log("TEST wait 2 secs and set tariff tone on previous connection ...\n");
         mmsSleep(2);                                  
                                      
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONFEREESETATTR]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
                                            // <field name="tariffTone>1</field>
         outxml->putParameter(MmsAppMessageX::confereeAtrrNames[MmsAppMessageX::TARIFF_TONE], 1);       
         sprintf(getbuf(),"TEST sending conferee set tariff tone for party 3\n"); log();                  
         break;

    case 6:                                 // Disco the entire conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[3] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();                                                                         
         log("TEST wait 2 secs and disco conference ...\n");
         mmsSleep(2);                                  
                                      
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         sprintf(getbuf(),"TEST sending conference %d disconnect\n",conferenceID); log();                  
         break;
 
    case 7:                                 // Disconnect from media server
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



int conferenceRecordWhilePlayToConferee(const unsigned int serverconnect) 
{            
  // Joins two to a conference, begins recording conference, plays to first conferee

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) log("\nBegin conference record and play to conferee\n\n");
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
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         log("TEST sending session/conference connect 2\n");
         break;  

    case 3:
         if  (-1 == checkResultCode()) return -1;    // Record conference
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();                                        

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         isPassthruProvisional = TRUE;      // Route provisional thru state machine
         log("TEST sending conference record\n");              
         break;

    case 4:                                 
         showResultCode();                  // Get recording ID and play audio to party 1 
         if (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         isPassthruProvisional = FALSE;
         log("TEST sending party 1 play announcement\n");              
         break;

    case 5:
         showResultCode();                  // Stop recording conference
         printf("TEST wait 5 secs and unmute party 2 ...\n");
         mmsSleep(5);                      
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);                                          
         log("TEST sending stop media on record session\n");              
         break;

    case 6:                                 
         showResultCode();                  // Disco 1
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         log("TEST disco 1\n");              
         break;

    case 7:                                 // Disco 2
         showResultCode(); 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         log("TEST disco 2\n");              
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

  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}



int playTwoToConfereeAndGetDigits(const unsigned int serverconnect) 
{            
  // Test for MMS-28, MMS-36

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  int  n = 0, result = 1, isThereAnOutboundMessage = 1;
  if  (iters++ == 0) printf("\nBegin conferee play test A\n\n");
  char* p = NULL;
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

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         showReturnedPortAndIP();                                          
                                   
         showResultCode();                  // Play two audio files to party 1 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox"); 
         log("TEST sending party 1 play announcement\n");              
         break;

    case 3:                                 // StopMedia does nothing, will fail w rc=7
         if  (-1 == checkResultCode(7)) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         log("TEST sending stopMediaOperation which will fail with rc=7\n");                         
         break;
                                             
    case 4:                                 // Get StopMedia result 
         if  (-1 == checkResultCode(7)) return -1; // Connect session 2 to conference
/*
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         log("TEST sending session/conference connect 2\n");
         break;
                                            // Get connect 2 result
    case 5:                                 // then send a GetDigits on session 1
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();  
*/
 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 5000");  
         log("TEST sending getDigits on session 1\n");    
         break;

/*
    case 6:                                 
         showResultCode();                  // Disco 2  
         log("TEST wait 3 secs and disco party 2 ...\n"); 
         mmsSleep(3);                                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         log("TEST sending conference/session disconnect 2\n");              
         break;
*/

    case 5:                                 // Disco 1
         showResultCode(); 
         log("TEST wait 3 secs and disco party 1 ...\n"); 
         mmsSleep(3);                                    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         log("TEST sending conference/session disconnect 1\n");              
         break;
 
    case 6:                                 // Disconnect from media server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }


  if  (isThereAnOutboundMessage)
  {                                          // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;  
}



int recordConfereeTestA(const unsigned int serverconnect) 
{ 
  // Conect on party to conference, record that conferee, with no term
  // condition, setting a brief command timeout. Command timeout should
  // terminate the conferee record as exepected.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference record test B\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
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

    case 2:                                 // Record conferee
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"mmstestrecording.vox");
         // Since media server checks every 3 seconds by default, the one-second
         // command timeout is not likely to hit within one second
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::COMMAND_TIMEOUT], 1000); 
         // Note no term condition -- we terminate on command timout
         p = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE]; 
         #ifdef MMS_TESTING_WAV
         outxml->putParameter(p, "format wav");
         #else
         outxml->putParameter(p, "format vox"); 
         #endif    
         printf("TEST sending record conferee 1\n");                
         break;

    case 3:                                 // Catch record term, disco server  
         showResultCode();   
         printf("TEST waiting 5 seconds ...\n");  
         mmsSleep(5);                                      
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

  if  (isThereAnOutboundMessage)
  {                                         // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int confereeRecordWhileBusy(const unsigned int serverconnect)  
{ 
  // Attempt to record a conferee while playing to conferee
  // This is not currently part of the test menu

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin record conferee while busy test\n\n");
  char* p = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
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

    case 2:                                 // Play audio to conferee
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         isPassthruProvisional = TRUE;      // Route provisional thru state machine
         printf("TEST sending first play\n");              
         break;
                                            
    case 3:                                 // Send record conferee after play provisional
         if (-1 == checkResultCode(1)) return -1;   
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         printf("TEST sending record\n"); 
         isPassthruProvisional = FALSE;             
         break;

    case 4:                                // Get record result
         if  (-1 == checkResultCode()) return -1;
         isThereAnOutboundMessage = 0;
         break;

    case 5:                                 // Get play result, play another to conferee
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         printf("TEST sending second play\n");              
         break;

    case 6:                                 // Disco session
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[1]);              
         break;
 
    case 7:                                 // Disconnect from media server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

                                             
  if  (isThereAnOutboundMessage)
  {                                         // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int conferenceTestPlayTimeout(const unsigned int serverconnect)  // 'z'
{ 
  static int iters, testiters, NUMITERS, isserverconnect, conferees, conferenceID, conxIds[4];
  #define TPP_DEFNUMITERS 1

  if (iters++ == 0)
  {
      sprintf(getbuf(),"\nbegin conference play timeout test A on %s\n\n", queueName); log();
      setGlobalsC();
                                            // If config parameters are for this test ...
      if (config->iparamA == ID_TIMEOUTCONFPLAY)
           NUMITERS = config->iparamB;      // ... get parameters from config
      else NUMITERS = TPP_DEFNUMITERS;      // Otherwise use default parameters

      sprintf(getbuf(),"configured iterations %d\n\n", NUMITERS); log();
      mmsSleep(2);
  }

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
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1000);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         outxml->putParameter(confidparamname, 0); 
         log("TEST sending session/conference connect 0\n");
         break;    

    case 2:                                // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1002);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         outxml->putParameter(confidparamname, conferenceID); 
         log("TEST sending session/conference connect 1\n");
         break;

    case 3:                                 // Play audio to conference
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(playcommand);
         outxml->putParameter(confidparamname, conferenceID); 
         outxml->putParameter(filenameparamname,"stoprec.vox"); 
         outxml->putParameter(filenameparamname,"goodbye.vox"); 
         outxml->putParameter(filenameparamname,"thankyou.vox"); 
         outxml->putParameter(cmdtimeoutparamname,5000); 
         log("TEST sending 20 sec play to conference with 5 sec timeout\n");              
         break;

    case 4:                                 // Disco from server 
         checkResultCode();

         if  (testiters++ < NUMITERS) 
         {                                  // Send next half connect
              outxml->putMessageID(playcommand);
              outxml->putParameter(confidparamname, conferenceID); 
              outxml->putParameter(filenameparamname,"stoprec.vox"); 
              outxml->putParameter(filenameparamname,"goodbye.vox"); 
              outxml->putParameter(filenameparamname,"thankyou.vox"); 
              outxml->putParameter(cmdtimeoutparamname,5000); 
              log("TEST sending 20 sec play to conference with 5 sec timeout\n"); 
              state = 3;                    // Loop back to do another play
         }
         else                               // We're done
         {    sendServerDisconnect(outxml);
              iters = 0;
              result = -1; 
         }

         break;
  }

                                            // Append transaction ID
  outxml->putParameter(transidparamname, ++transID);
  outxml->terminateReturnMessage(clientID);

  printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}



int reconnectRemoteIP(const unsigned int serverconnect) // (C) 
{
  // This test sequence connects to server, connects a session, reconnects
  // the session on a different port, disconnects session, 
  // connects session, immediately sends session reconnect on new port
  // without waiting for first connect to complete, expecting session
  // busy failure; waits for both responses; sends a reconnect on
  // same port and IP, expecting already connected error; disconnects
  // session; disconnnects from server

  static int iters, isserverconnect, conxID;
  if  (iters++ == 0) printf("\nBegin reconnect remote IP test A\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;

    case 1:                                 // Send an IP connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending session connect\n");
         break;

   case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxID = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 2000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending ip/port reconnect w no attribute changes\n");              
         break;

    case 3:
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 2000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         putConnectParam(outxml, "coderTypeRemote g711alaw");   
         log("TEST sending connection attribute change w no port/ip change\n");              
         break;

    case 4:
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 2000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1");  
         log("TEST sending first connect on existing connection with no media parameter changes\n");              
         break;

    case 5:
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 2000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1");  
         log("TEST sending second connect on existing connection with no media parameter changes\n");              
         break;

    case 6:
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 2000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1");         
         putConnectParam(outxml, "dataflowDirection ipSendOnly");     
         log("TEST sending dataflow direction change on existing connection\n");                             
         break;

    case 7:
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         log("TEST sending session disconnect\n");                         
         break;

    case 8:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

  if  (isThereAnOutboundMessage)
  {                                         // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
} 



int reconnectRemoteIPToConference(const unsigned int serverconnect) 
{
  // Connect 3 sessions to a conference, then change the port of the first
  // conferee's connection.

  static int iters, isserverconnect, conferees, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference play timeout test A\n\n");
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
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         printf("TEST sending session/conference connect 2\n");
         break;

   case 3:                                // Connect another session to conf
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         printf("TEST sending session/conference connect 2\n");
         break;

    case 4:                                 // Change conferee zero's port
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1006);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         printf("TEST reconnect of conferee 0 session\n");
         break;
             
    case 5:                                 // Abandon connections & disco server
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



int reconnectOverMedia(const unsigned int serverconnect)  // (3)
{
  // This test sequence connects to server, connects a session, launches concurrent
  // play and getDigits operations, and sends a connect with "modify" parameter set, 
  // while media is executing on the two opera
  // The reconnect should occur and voice should continue to flow subsequently.

  static int iters, isserverconnect, conxID;
  if  (iters++ == 0) printf("\nBegin reconnect over media test A\n\n");
  char* p = NULL, *cmdname = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;

    case 1:                                 // Send an IP connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending session connect\n");
         break;

    case 2:                                 // Send a receiveDigits
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxID = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         cmdname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(cmdname, "maxtime 10000"); 
         isPassthruProvisional = TRUE;
         break;    
                                        
    case 3:                                // Send a play 
         if (-1 == checkResultCode(1)) return -1;
         isPassthruProvisional = FALSE;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         cmdname = MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME]; 
         outxml->putParameter(cmdname,"stoprec.vox");
         outxml->putParameter(cmdname,"thankyou.vox");
         outxml->putParameter(cmdname,"goodbye.vox");
         cmdname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(cmdname, "maxtime 10000");     
         log("sending play then wait 3 seconds ...\n"); 
  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         
         delete outxml;
         outxml = new MmsAppMessage;
         mmsSleep(3);
                                            // Send barging reconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 2000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         // "modify" indicates we are explicitly reconnecting. When we are not barging media,
         // we don't need this parameter, the differing IP/port is enough to indicate reconnect,
         // but when the session is busy, we need the modify flag to know to barge the session.
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::MODIFY], 1);
         log("TEST sending reconnect w no attribute changes\n");              
         break;

    case 4:                                 // Get result of reconnect
         showResultCode();
         showReturnedPortAndIP();
         isThereAnOutboundMessage = FALSE;                                   
         break;

    case 5:                                 // Get result of play
         showResultCode();
         isThereAnOutboundMessage = FALSE;                       
         break;

    case 6:                                 // Get result of receiveDigits
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         log("TEST sending session disconnect\n");                         
         break;

    case 7:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

  if  (isThereAnOutboundMessage)
  {                                         // Append transaction ID
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);

       sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
} 


int testConnectParameterState(const unsigned int serverconnect)
{
  // Tests ability to set various connect parameters  such as coder, 
  // framesize, VAD, dataflow direction, and subsequently maintain or override 
  // said parameters when a session is reconnected on IP and/or port change.
  // Media server engineering diagnostic 0x1 should be set, in order to
  // observe the connect parameters actually used for each connection

  static int iters, isserverconnect;
  if  (iters++ == 0) 
  {    printf("\nBegin connect parameter state test\n");
       printf("Set media server engineering diagnostic 0x1 to verify results\n\n");
  }
  char* p = NULL, c=0;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  static char ipA[]="127.0.0.1", ipB[]="127.0.0.1", ipMulticast[]="239.253.0.1";  
  static int  ports  [5] = {1000,1002,1004,1006,1008};
  static int  conxIds[5] = {0,0,0,0,0};

  switch(state)
  {
    case 0:                                 // Connect to media server
         sendServerConnect(outxml);
         isserverconnect = TRUE;
         break;

    case 1:                                 // Send session 1 connect 
         if  (isserverconnect) clientID = getMessageClientID();
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::PORT, ports[3]);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipMulticast);
         putConnectParam  (outxml, "dataflowDirection multicastServer");
         printf("TEST sending session 1 multicast connect\n");
         break;

    case 2:                                 // Send session 2 connect 
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
          putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::PORT, ports[0]);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA);
         putConnectParam  (outxml, "coderTypeRemote g711alaw");
         printf("TEST sending session 2 connect\n");
         break;

    case 3:                                 // Send session 3 connect 
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::PORT, ports[1]);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA);
         putConnectParam  (outxml, "framesizeRemote 10"); 
         printf("TEST sending session 3 connect\n");
         break;

    case 4:                                 // Send session 4 connect 
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::PORT, ports[2]);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA);
         putConnectParam  (outxml, "vadEnableRemote 1"); 
         printf("TEST sending session 4 connect\n");
         break;

    case 5:                                 // Send session 5 connect 
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[3] = getExpectedConnectionID())) return -1;
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::PORT, ports[4]);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA);
         putConnectParam  (outxml, "framesizeLocal 30");
         printf("TEST sending session 5 connect\n");
         break;

    case 6:                                 // Reconnect session 1
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[4] = getExpectedConnectionID())) return -1;
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::CONNECTION_ID, conxIds[0]);
         putParam(outxml,  MmsAppMessageX::PORT, ports[0]+1000);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA);  
         printf("TEST sending session 1 reconnect\n");      
         break;

    case 7:                                 // Reconnect session 2
         showResultCode();
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::CONNECTION_ID, conxIds[1]);
         putParam(outxml,  MmsAppMessageX::PORT, ports[1]+1000);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA); 
         printf("TEST sending session 2 reconnect\n");             
         break;


    case 8:                                 // Reconnect session 3
         showResultCode();
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::CONNECTION_ID, conxIds[2]);
         putParam(outxml,  MmsAppMessageX::PORT, ports[2]+1000);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA);  
         printf("TEST sending session 3 reconnect\n");            
         break;

    case 9:                                 // Reconnect session 4
         showResultCode();
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::CONNECTION_ID, conxIds[3]);
         putParam(outxml,  MmsAppMessageX::PORT, ports[3]+1000);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA); 
         printf("TEST sending session 4 reconnect\n");             
         break;


    case 10:                                // Reconnect session 5
         showResultCode();
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::CONNECTION_ID, conxIds[4]);
         putParam(outxml,  MmsAppMessageX::PORT, ports[4]+1000);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA); 
         printf("TEST sending session 5 reconnect\n");             
         break;

    case 11:                                // Reconnect session 1 again
         showResultCode();                  // switching coder at same time
         putConnectCommand(outxml);
         putParam(outxml,  MmsAppMessageX::CONNECTION_ID, conxIds[0]);
         putParam(outxml,  MmsAppMessageX::PORT, ports[0]);
         putParam(outxml,  MmsAppMessageX::IP_ADDRESS, ipA); 
         putConnectParam  (outxml, "coderTypeRemote g711ulaw");
         printf("TEST sending session 1 reconnect/coder switch\n");             
         break;

    case 12:                                // Abandon all connections and disco
         showResultCode(); 
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




