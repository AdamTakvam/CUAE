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

#define ID_TTSTESTA 53460            // iparamA -- identifies TTS test
#define DEFFAULT_TTSTEST 1           // iparamB is the ordinal of desired specific TTS test

#define ID_EXHAUSTCONNECTS 53446     // iparamA -- identifies exhaust connection test
#define EC_NUMCONNECTS         7     // iparamB -- how many connections to attempt

#define ID_HAIRPINNING 53444         // iparamA -- identifies hairpinning test
char* conxIDparamname, *fnparamname, *termcondpname;

static char *connectcommand, *playcommand, *recordcommand, *adjustplaycommand, *conxidparamname;
static char *confidparamname, *termcondparamname, *disconnectcommand, *stopmediacommand;  
static char *filenameparamname, *volumeparamname, *speedparamname, *adjtypeparamname, *togtypeparamname;
static char *portparamname, *ipparamname, *transidparamname;

void setGlobals()
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
}



int ttsTestA(const unsigned int serverconnect) // (+)
{
  // This test sequence connects to server, connects a session, plays a
  // file, interrupts the play, disconnects session, disconnects server

  static int iters, isserverconnect, whichtest;

  if (iters++ == 0)
  {
      sprintf(getbuf(),"\nBegin tts test A\n", queueName); log();
      // If config parameters are for this test, get test number from config
      whichtest = (config->iparamA == ID_TTSTESTA)? config->iparamB: DEFFAULT_TTSTEST;
      sprintf(getbuf(),"Configured test number is %d\n\n", whichtest); log();
      MmsAppMessageX::instance();
      fnparamname = MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME];
      mmsSleep(1);
  }

  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1;
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
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 

         switch(whichtest)
         {
           case 2:
                outxml->putParameter(fnparamname,"the number you entered was 1(512)687-2007.");
                break;
           case 3:
                outxml->putParameter(fnparamname,"the number you entered was +82-2-3016-8534.");
                break;
           case 4:
                outxml->putParameter(fnparamname,"the PIN number you entered was 1<vt_pause=20>2<vt_pause=20>3.");
                break;
           case 5:
                outxml->putParameter(fnparamname,"your package is 3g. over weight.");
                break;
           case 6:   // This string should be interpreted as a file name (ends with dot followed by 1-16 characters)
                outxml->putParameter(fnparamname,"your package is 3g. over weight");
                break;
           case 7:
                outxml->putParameter(fnparamname,"ttstest1.wav");
                outxml->putParameter(fnparamname,"if wave 1 played first, success.");
                break;
           case 8:
                outxml->putParameter(fnparamname,"if wave 1 plays next, success.");
                outxml->putParameter(fnparamname,"ttstest1.wav");
                break;
           case 9:
                outxml->putParameter(fnparamname,"if you hear this, fail.");
                outxml->putParameter(fnparamname,"nonexistent.wav");
                break;
           case 10:
                outxml->putParameter(fnparamname,"if the wave file plays next,");
                outxml->putParameter(fnparamname,"ttstest1.wav");
                outxml->putParameter(fnparamname,"and you hear this last, success,");
                break;
           case 11:
                outxml->putParameter(fnparamname,"if you hear this first,");
                outxml->putParameter(fnparamname,"and the wave follows this, success,");
                outxml->putParameter(fnparamname,"ttstest1.wav");
                break;
           case 12:
                outxml->putParameter(fnparamname,"ttstest1.wav");
                outxml->putParameter(fnparamname,"ttstest2.wav");
                outxml->putParameter(fnparamname,"if both waves preceded, success.");
                break;
           case 13:
                outxml->putParameter(fnparamname,"ttstest1.wav");
                outxml->putParameter(fnparamname,"if wave 1 preceded, and wave 2 follows, success.");
                outxml->putParameter(fnparamname,"ttstest2.wav");
                break;
           case 14:
                outxml->putParameter(fnparamname,"if this is first,");
                outxml->putParameter(fnparamname,"this is second,");
                outxml->putParameter(fnparamname,"and this is third, success.");
                break;
           case 15:
                outxml->putParameter(fnparamname,"bogus text 	 ~`^()_{}[]| end bogus text.");
                break;
           case 16:
                outxml->putParameter(fnparamname,
                " <prompt> the date is <say-as type=\"date\">12-25-2011</say-as>. thank you. <prompt>");
                break;
           case 1:
           default:
                outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],
                  "text to speech test 1 success");
         }
       
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 15000");   // 15 seconds
         log("TEST sending tts play\n");   
         break;
                                            
    case 3:                                 // Get result of play or stop
         checkResultCode();
         n = getExpectedConnectionID();
                                            // Send an IP disconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         log("TEST sending session disconnect\n");              
         break;

    case 4:                                 // Disconnect from media server
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



int testRecordTerminationOnDigit(const unsigned int serverconnect)  // (I)
{
  static int iters, isserverconnect;
  if  (iters++ == 0) log("\nBegin record/playback test\n\n");
  char* p = NULL, *termcondname = NULL, *paramname = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage=1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  char   recordFilePath[128]={0};
  int    pathlen = 0, iserror=0;

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

    case 2:                                // Send record
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                             
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::FILE_NAME],"myrecfile.wav");

         // #define MMS_TESTING_VOX  

         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE]; 
         #ifdef MMS_TESTING_VOX
         outxml->putParameter(paramname, "format vox"); 
         outxml->putParameter(paramname, "encoding alaw");   
         outxml->putParameter(paramname, "bitrate 6"); 
         #else
         outxml->putParameter(paramname, "format wav"); 
         #endif        
                                            // Override recording expiration days
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::EXPIRES], 1);
                                            // Give it some termination conditions
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 10000");   // 10 seconds
         outxml->putParameter(termcondname, "silence 10000");   // 10 seconds
         outxml->putParameter(termcondname, "digit #");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         isShowProvisionalContent = TRUE; // Display provisional xml for record name
         log("TEST sending record\n");  
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
              
         printf("TEST waiting 5 seconds ...\n");  
         mmsSleep(5);                       // Send sendDigits after 5 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         log("TEST sending sendDigits\n");              
         break;
  
     case 3:                                // Get result of record or send digits
         isShowProvisionalContent = FALSE;  // Display provisional xml for record name
         checkResultCode();
         n = getExpectedConnectionID();
         isThereAnOutboundMessage = 0;
         break;

    case 4:                                 // Get result of record or send digits
         checkResultCode();
         n = getExpectedConnectionID();
                                            // Send an IP disconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         log("TEST sending session disconnect\n");              
         break;

    case 5:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }
                                             
  if  (isThereAnOutboundMessage)
  {
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



int attemptCommandOnHalfConnectedSession(const unsigned int serverconnect) 
{
  // This test sequence connects to server, connects a session without starting,
  // the session, sends a play audio on the session which should fail, 
  // disco session, disco server
  static int iters, isserverconnect, conxID;
  if  (iters++ == 0) printf("\nBegin half connect test\n\n");
  char* p = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1); 
         break;

    case 1:                                 // Send an half connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         printf("TEST sending session HALF CONNECT\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxID = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                            // Send play on lame session
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         printf("TEST sending play on half-connected session\n");              
         break;

    case 3:                                 // Send an IP disconnect
         if  (-1 != checkResultCode()) 
              printf("TEST ALERT: play succeeded on half connected session\n");
         else printf("TEST play failed as expected on half connect\n");
 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         printf("TEST sending session disconnect\n");              
         break;

    case 4:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
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

 

int sendHalfConnectSequence(const unsigned int serverconnect) // (c)
{
  // This test sequence connects to server, connects a session without starting,
  // the session, completes the session connection, disconnects session, 
  // disconnects server
  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin half connect test\n\n");
  char* p = NULL;
  int   n = 0, result = 1;
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
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         // outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 

         // Modify this test to include coder type, frame size, data flow direction
         // putConnectParam(outxml, "coderTypeRemote g711ulaw");  // OK
         // putConnectParam(outxml, "coderTypeLocal g711ulaw");   // OK
         // putConnectParam(outxml, "framesizeRemote 20");        // OK
         // putConnectParam(outxml, "framesizeLocal 20");         // OK
         // putConnectParam(outxml, "dataflowDirection ipSendOnly"); // OK  
         // putConnectParam(outxml, "dataflowDirection ipBidirectional"); // OK  

         // Include the following line to reserver G729 low bit rate coder
         // putConnectParam(outxml, "coderTypeRemote 8");  // 1=711u 2=711a 4=723 8=729

         log("TEST sending session half connect\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         // #define TESTING_BLOCKED_CONNECTION
         #ifdef  TESTING_BLOCKED_CONNECTION
         printf("TESTING_BLOCKED_CONNECTION\n");
         waitForChar();   // for testing a single connection mms w no connections available
         #endif

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         // Include the following line to use G729 low bit rate coder
         // putConnectParam(outxml, "coderTypeRemote 8");  // 1=711u 2=711a 4=723 8=729

         // Modify this test to include coder type, frame size, data flow direction
         // putConnectParam(outxml, "coderTypeRemote g711ulaw");   
         // putConnectParam(outxml, "coderTypeLocal g711ulaw");  
                                                           // low bit rate test
         // putConnectParam(outxml, "coderTypeRemote 4");  // 1=711u 2=711a 4=723 8=729  

         // putConnectParam(outxml, "framesizeRemote 20");        
         // putConnectParam(outxml, "framesizeLocal 20");          
         // putConnectParam(outxml, "dataflowDirection ipSendOnly");    
         // putConnectParam(outxml, "dataflowDirection ipBidirectional");    
   
         log("TEST sending final connect\n");              
         break;

    case 3:                                 // Send an IP disconnect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         log("TEST sending session disconnect\n");              
         break;

    case 4:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

                                            // Append transaction ID
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();

  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int testMaxConnections(const unsigned int serverconnect)  
{
  // This test sequence connects to server, connects as many sessions as it can
  // using the coder specified below, abandons all sessions and disconnects server
  static int   coder = G723; // from header file enum testcoder
  static int   iters, isserverconnect, port=8000, isMaxed, mmscoder = mmscoders[coder];
  static int   conxID, connections;
  static char  coderparam[32]; sprintf(coderparam, "coderTypeRemote %d", mmscoder); // 1=711u 2=711a 4=723 8=729

  if  (iters++ == 0) printf("\nBegin max connection test using coder %s\n\n", coderstrings[coder]);
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
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port+=2);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         putConnectParam(outxml, coderparam);       
         sprintf(getbuf(),"TEST sending connect %d\n", connections+1); log();
         break;

   case 2:
         if  (-1 == checkResultCode())   
         {
              isMaxed = TRUE;
              sprintf(getbuf(),"TEST connections maxed out at %d\n", connections); log();
              sendServerDisconnect(outxml); 
              result = -1;
         }
         else
         {    if  (-1 == (conxID = getExpectedConnectionID())) return -1;
              connections++;
              showReturnedPortAndIP();
              outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port+=2);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
              putConnectParam(outxml, coderparam);       
              sprintf(getbuf(),"TEST sending connect %d\n", connections+1); log(); 
         }            
         break;
  }

  if  (isThereAnOutboundMessage)
  {                                         
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);
       sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();                                          
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  if  (state == 2 && !isMaxed);             // State remains at 2 until connection fails
  else state++;

  return result;
} 


int testMaxConnectionsC(const unsigned int serverconnect)  // (5)
{
  // This test sequence connects to server, connects as many sessions as it can
  // using the coder specified below, abandons all sessions and disconnects server
  // This differs from testMaxConnections in that it does half connects, thereby
  // exercising the coder reservation logic.

  static int   coder = G723; // from header file enum testcoder
  static int   iters, isserverconnect, port=1000, isMaxed, mmscoder = mmscoders[coder];
  static int   conxID, connections;
  static char  coderparam[32]; sprintf(coderparam, "coderTypeRemote %d", mmscoder); // 1=711u 2=711a 4=723 8=729

  if  (iters++ == 0) printf("\nBegin max connection test using coder %s\n\n", coderstrings[coder]);
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

    case 1:                                 // Send an IP half connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         putConnectParam(outxml, coderparam); // specify coder on half connect only       
         sprintf(getbuf(),"TEST sending half connect %d\n", connections+1); log();
         break;

    case 2:
         if  ((-1 == checkResultCode()) || (-1 == (conxID = getExpectedConnectionID())))
         {    isMaxed = TRUE;
              sprintf(getbuf(),"TEST connections maxed out at %d\n", connections); log();
              sendServerDisconnect(outxml); 
              result = -1;
         }
         else
         {    connections++;
              outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port+=2);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
              // note we're not respecifying coder on full connect - it should use coder from half connect
              sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();
         }
         break;

    case 3:
         if  (-1 == checkResultCode())
         {    isMaxed = TRUE;
              sprintf(getbuf(),"TEST connections maxed out at %d\n", connections); log();
              sendServerDisconnect(outxml); 
              result = -1;
         }
         else
         {    outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
              putConnectParam(outxml, coderparam);  // specify coder on half connect only     
              sprintf(getbuf(),"TEST sending half connect %d\n", connections+1); log(); 
         }            
         break;
  }

  if  (isThereAnOutboundMessage)
  {                                         
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);
       sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();                                          
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  if  (state == 3)
       state = 2;
  else state++;

  return result;
} 



int testMaxConnectionsB(const unsigned int serverconnect)  // (5)
{
  static int   coder = G729; // from header file enum testcoder e.g. G711U, G729
  static int   iters, isserverconnect, port=1000, mmscoder = mmscoders[coder];
  static int   conxID, conxIDs[8], connections, isMaxed, discos, isDiscoed, cycles, MAXCYCLES = 2;
  static char  coderparam[32]; 

  if  (iters++ == 0) 
  {
    setGlobals();
    sprintf(coderparam, "coderTypeRemote %d", mmscoder); // 1=711u 2=711a 4=723 8=729
    printf("\nBegin max connection test using coder %s\n\n", coderstrings[coder]);
  }
  int   n = 0, result = 1, myResult = 1, isThereAnOutboundMessage = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;

    case 1:                                 // Send an IP half connect 1
         if  (isserverconnect && !clientID) clientID = getMessageClientID();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 0);
         putConnectParam(outxml, coderparam); // specify coder on half connect only       
         sprintf(getbuf(),"TEST sending half connect %d\n", connections+1); log();
         break;

    case 2:
         myResult = checkResultCode(8);      // Send full connect 1
         if (-1 == myResult) return -1;
         if (-1 ==(conxID = getExpectedConnectionID())) return -1;               
         conxIDs[connections++] = conxID;  
         outxml->putMessageID(connectcommand);
         outxml->putParameter(conxidparamname, conxID); 
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();          
         break;

    case 3:                               // Send an IP half connect 2
         if  (isserverconnect && !clientID) clientID = getMessageClientID();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 0);
         putConnectParam(outxml, coderparam); // specify coder on half connect only       
         sprintf(getbuf(),"TEST sending half connect %d\n", connections+1); log();
         break;

    case 4:
         myResult = checkResultCode(8);     // Send full connect 2
         if (-1 == myResult) return -1;
         if (-1 ==(conxID = getExpectedConnectionID())) return -1;               
         conxIDs[connections++] = conxID;  
         outxml->putMessageID(connectcommand);
         outxml->putParameter(conxidparamname, conxID); 
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();          
         break;

    case 5:                                 // Send an IP half connect 3
         if  (isserverconnect && !clientID) clientID = getMessageClientID();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         putConnectParam(outxml, coderparam); // specify coder on half connect only       
         sprintf(getbuf(),"TEST sending half connect %d\n", connections+1); log();
         break;

    case 6:
         myResult = checkResultCode(8);      // Send full connect 3
         if (-1 == myResult) return -1;
         if (-1 ==(conxID = getExpectedConnectionID())) return -1;               
         conxIDs[connections++] = conxID;  
         outxml->putMessageID(connectcommand);
         outxml->putParameter(conxidparamname, conxID); 
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();          
         break;

    case 7:                                 // Send an IP half connect 4
         if  (isserverconnect && !clientID) clientID = getMessageClientID();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 0);
         putConnectParam(outxml, coderparam); // specify coder on half connect only       
         sprintf(getbuf(),"TEST sending half connect %d\n", connections+1); log();
         break;

    case 8:
         myResult = checkResultCode(8);      // Send full connect 4
         if (-1 == myResult) return -1;
         if (-1 ==(conxID = getExpectedConnectionID())) return -1;               
         conxIDs[connections++] = conxID;  
         outxml->putMessageID(connectcommand);
         outxml->putParameter(conxidparamname, conxID); 
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();          
         break;

    case 9:                                 // Send an IP half connect 5 EXPECTED TO FAIL 
         myResult = checkResultCode(8);      
         if  ((-1 == myResult) || (-1 == (conxID = getExpectedConnectionID()))) return -1; 
         conxIDs[connections++] = conxID;  
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 0);
         putConnectParam(outxml, coderparam); // LBR coder, but we're now out of LBR      
         sprintf(getbuf(),"TEST sending half connect %d expected to fail\n", connections+1); log();
         break;

    case 10:  // RC=8 expected from above operation when 4-LBR license in effect
         myResult = checkResultCode(8);      // disco 1
         if  (-1 == myResult) return -1; 
         log("previous connect failed due to out of LBR/n");
         discos = 0;
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIDs[discos]);
         sprintf(getbuf(),"TEST sending disco on conxID %d\n", conxIDs[discos++]); log(); 
         break;

    case 11:
         myResult = checkResultCode();      // disco 2
         if  (-1 == myResult) return -1; 
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIDs[discos]);
         sprintf(getbuf(),"TEST sending disco on conxID %d\n", conxIDs[discos++]); log(); 
         break;

    case 12:
         myResult = checkResultCode();      // disco 3
         if  (-1 == myResult) return -1; 
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIDs[discos]);
         sprintf(getbuf(),"TEST sending disco on conxID %d\n", conxIDs[discos++]); log(); 
         break;

    case 13:
         myResult = checkResultCode();      // disco 4
         if  (-1 == myResult) return -1; 
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIDs[discos]);
         sprintf(getbuf(),"TEST sending disco on conxID %d\n", conxIDs[discos++]); log(); 
         break;

    case 14:
         myResult = checkResultCode();      // full connect 1
         if  (-1 == myResult) return -1; 
         connections = 0; memset(conxIDs, 0, sizeof(int) * sizeof(conxIDs));              
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         putConnectParam(outxml, coderparam); // LBR coder, 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections+1); log();          
         break;

    case 15:                              // full connect 2
         myResult = checkResultCode(8);
         if  ((-1 == myResult) || (-1 == (conxID = getExpectedConnectionID()))) return -1; 
         conxIDs[connections++] = conxID; 
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         putConnectParam(outxml, coderparam); // LBR coder, 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();          
         break;

    case 16:
         myResult = checkResultCode(8);    // full connect 3
         if  ((-1 == myResult) || (-1 == (conxID = getExpectedConnectionID()))) return -1; 
         conxIDs[connections++] = conxID; 
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         putConnectParam(outxml, coderparam); // LBR coder, 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();          
         break;

    case 17:
         myResult = checkResultCode(8);    // full connect 4
         if  ((-1 == myResult) || (-1 == (conxID = getExpectedConnectionID()))) return -1; 
         conxIDs[connections++] = conxID; 
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, port+=2);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         putConnectParam(outxml, coderparam); // LBR coder, 
         sprintf(getbuf(),"TEST sending full connect %d\n", connections); log();          
         break;

    case 18:
         myResult = checkResultCode(8);    // disco 1
         if  ((-1 == myResult) || (-1 == (conxID = getExpectedConnectionID()))) return -1; 
         conxIDs[connections++] = conxID; 
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxID);
         sprintf(getbuf(),"TEST sending disco on conxID %d\n", conxID); log(); 
         break;

    case 19:                             // abandon and disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }
                                       
  outxml->putParameter(transidparamname, ++transID);
  outxml->terminateReturnMessage(clientID);
  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();                                          
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
} 



int testMaxConnectionsD(const unsigned int serverconnect)  // also (5)
{
  // Exhaust connections test where number of connect attempts is configured

  static int digitsiters, isserverconnect, port=1000;
  static int conxIds[2],  conferenceID, testIterations;
  static int NUMCONNECTS, connects; 

  if (testIterations == 0)
  {
      sprintf(getbuf(),"\nbegin exhaust connections test on %s\n", queueName); log();
      setGlobals();
                                            // If config parameters are for this test ...
      if (config->iparamA == ID_EXHAUSTCONNECTS)
      {
          NUMCONNECTS   = config->iparamB;  // ... get parameters from config
      }
      else
      {                                     // Otherwise use default parameters
          NUMCONNECTS = EC_NUMCONNECTS;
      }

      sprintf(getbuf(),"configured connect attempts %d\n\n", NUMCONNECTS); log();
      mmsSleep(2);
  }

  int  result = 1, i = 0, isThereAnOutboundMessage = TRUE;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage; 


  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1); 
         clientID = 0;
         testIterations = 0;
         isNoShowInboundXml  = 1;           // Disable XML display
         interCommandSleepMs = 0;           // Keep things moving  
         break;                         

    case 1:                                 // Send a half connect
         checkResultCode();
         if (!clientID) clientID = getMessageClientID();

         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 0);
         outxml->putParameter(ipparamname, " "); 
         sprintf(getbuf(),"sending connect %d\n", connects++); log();
         break;                                             
                                            // Get party A disco result
    case 2:                                 // Disco from server 
         checkResultCode();

         if  (testIterations < NUMCONNECTS) 
         {                                  // Send next half connect
              sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
              outxml->putMessageID(connectcommand);
              outxml->putParameter(portparamname, 0);
              outxml->putParameter(ipparamname, " "); 
              sprintf(getbuf(),"sending connect %d\n", connects++); log();
              state = 1;                    // Loop back to begin another test iteration
         }
         else                               // We're done
         {    if  (serverconnect & SERVER_DISCO)
                   sendServerDisconnect(outxml); 
              testIterations = 0;

              result = -1; 
         }

         break;
  }

                                              
  if  (isThereAnOutboundMessage)
  {                                          // Append transaction ID
       outxml->putParameter(transidparamname, ++transID);
       outxml->terminateReturnMessage(clientID);
       sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}



int testRetransmitConnection(const unsigned int serverconnect)  // (Y)
{
  static int iters, isserverconnect, conferees, confID, conxIds[4], serverID=1;
  if  (iters++ == 0) printf("\nBegin rexmit test\n\n");
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

         // We'll set the retransmit flag here on the half connect. We should
         // be able to set it on the full connect instead, so we should test
         // that case as well.
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::REXMIT_CONNECTION], 1); 

         // We would not usually include a connection ID with a half connect.
         // Assuming it is zero, it should make no difference. However herer
         // we are going to attach server ID to the connection ID. Media server
         // *should* still treat it a zero, or not present.
         
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], serverID << 24);
         log("TEST sending party A half connect\n");
         break;

    case 2:                                 
         if (-1 == checkResultCode()) return -1; // Send full connect
         if (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         // Now, the connection and conference IDs that come back will have 
         // server ID embedded -- we do not bother to strip it off, since we
         // will use it this way the next time we send it to the media server.

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

    case 4:                                 // Disconnect from media server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }
                                             
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);

  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();   
                                             
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int testAudioDescriptorFileSequence(const unsigned int serverconnect) // (f)
{
  // This test sequence connects to server, connects a session, records a
  // file, reads the descriptor file, plays the file back using the
  // attributes in the descriptor file, discos session, discos server

  // This test verifies that a file expiration specified with the record
  // command is correctly applied, and that audio attributes specified
  // with the record command are correctly applied both on record and playback.

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin record/playback test\n\n");
  char* p = NULL, *termcondname = NULL, *paramname = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  char   recordFilePath[128]={0}, descriptorPath[128], filebuf[128], *q;
  int    pathlen = 0, iserror=0;

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
         printf("TEST sending session connect\n");
         break;

    case 2:                                // Send record
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                             
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         //txml->putParameter(outxml->paramnames  [MmsAppMessageX::FILE_NAME],"myrecfile.vox");

         //#define MMS_TESTING_VOX    // define MMS_TESTING_VOX to record/play .vox files

         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE]; 
         #ifdef MMS_TESTING_VOX
         outxml->putParameter(paramname, "format vox"); 
         outxml->putParameter(paramname, "encoding alaw");   
         outxml->putParameter(paramname, "bitrate 6");     
         #else
         outxml->putParameter(paramname, "format wav");                   
         #endif 
                                            // Override recording expiration days
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::EXPIRES], 1);
                                            // Give it some termination conditions
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 3000");   // 3 seconds
         outxml->putParameter(termcondname, "digit #"); 
         isShowProvisionalContent = TRUE; // Display provisional xml for record name
 
         printf("TEST sending record\n");              
         break;

    case 3:                                 // Get name of record file, verify
         checkResultCode();                 // descriptor, and play it back
         isShowProvisionalContent = FALSE;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         if  (NULL == (p = getReturnedRecordPath())) return -1;

				 strcpy(recordFilePath, p);   

				 strcpy(descriptorPath, config->serverParams.audioBasePath);
         strcat(descriptorPath, "\\");     
         strcat(descriptorPath, recordFilePath); 
         pathlen = strlen(descriptorPath);  // Ensure descriptor written:

         q = descriptorPath + pathlen;      // Make a descriptor file path
         while(q > descriptorPath && *q != '.') q--; 

         if  (*q == '.')
				 {     strcpy(q,".mms");            // Open descriptor file
               FILE* f = fopen(descriptorPath,"r");
               if   (f)
               {     memset(filebuf,0, sizeof(filebuf));
                     fread(filebuf, 1, sizeof(filebuf)-1, f);
                     fclose(f);             // Display descriptor record
                     printf("%s\n",filebuf);
               }
               else printf("No descriptor file %s\n",descriptorPath);
         }      
         else printf("Bad record file path %s\n",recordFilePath);
                                            // Send a playback on the recording,
                                            // which should use the descriptor
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::FILE_NAME], recordFilePath); 

         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 10000");   // 10 seconds
         outxml->putParameter(termcondname, "digit #"); 

         printf("TEST sending play\n");              
         break;

    case 4:                                 // Send an IP disconnect
         checkResultCode();
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



int testAnnounceAndGetDigitsSequence(const unsigned int serverconnect)  // (b)
{
  // This test sequence connects to server, connects a session, plays an
  // announcement, gathers some digits, disconnects session, disconnects server

  static int iters, isserverconnect;
  if  (iters++ == 0) log("\nBegin announcement/get digits test\n\n");
  char* p = NULL, *paramname = NULL, *fn = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  // #define MMS_TESTING_FULLPATH
  #define MMS_TESTING_TTS

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
         // outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         log("TEST sending session/conference connect 1\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE]; 
                                
         outxml->putParameter(paramname, "format wav");
         fn = "welcome.wav";
         
         #if defined(MMS_TESTING_TTS)

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], 
              "The pin number you entered [vt_pause=500] is invalid."); 

         fn = "[vt_speed=240]please enter your pin number[/vt_speed], followed by the pound key."; 

         #elif defined(MMS_TESTING_FULLPATH)
         fn = "C:\\Program Files\\Cisco Systems\\Unified Application Environment\\MediaServer\\Audio\\myApplication\\en-US\\welcome.wav";
         outxml->putParameter(paramname, "format wav");
         #else
         outxml->putParameter(paramname, "format wav");
         #endif

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn); 
                 
         sprintf(getbuf(),"TEST sending play of %s\n", fn); log();              
         break;
    
    // set this define to test infinite timeout on the get digits operation
    // #define IS_TESTING_INFINITE_TIMEOUT

    case 3:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 

         #ifdef IS_TESTING_INFINITE_TIMEOUT
                                            // Set MMS command timeout infinite 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::COMMAND_TIMEOUT], 0);

         #else // IS_TESTING_INFINITE_TIMEOUT

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 5000");   // 5 seconds

         #endif // IS_TESTING_INFINITE_TIMEOUT

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxdigits 11"); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
        

         log("TEST sending get digits\n");              
         break;

    #ifndef IS_TESTING_INFINITE_TIMEOUT

    case 4:                                 // Send an IP disconnect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         log("TEST sending session disconnect\n");              
         break;

    case 5:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;

    #else   // IS_TESTING_INFINITE_TIMEOUT

    default: isThereAnOutboundMessage = 0;
     
    #endif  // IS_TESTING_INFINITE_TIMEOUT
  }


  if  (isThereAnOutboundMessage)
  {
      outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
      outxml->terminateReturnMessage(clientID);

      sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();   
                                                
      putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}     
    


int testAnnounceAndSendDigitsSequence(const unsigned int serverconnect)  // (e)
{
  // This test sequence connects to server, connects a session, plays an
  // announcement, sends some digits during playback, disconnects session, 
  // disconnects server

  static int iters, isserverconnect, serverID=1;
  if  (iters++ == 0) printf("\nBegin announcment/send digits test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  { 
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml, serverID);  // <<= server ID
         else return 0 == (state = 1);   
         break;  

    case 1:                                 // Send an IP full connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session connect\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                            // Play 15-second announcement
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");

         //outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],
         //  "you have 3 messages waiting. press one to play messages.");

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
                                            // ... with digit termination conds
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         // MmsAppMessageX::putParameter(termcondname, "maxdigits 1"); 
         outxml->putParameter(termcondname, "digit #");
                                            // ... and 6-second max
         outxml->putParameter(termcondname, "maxtime 6000");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending play\n");              
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send sendDigits after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         printf("TEST sending sendDigits\n");              
         break;
                                            
    case 3:                                 // Get result of play or send digits
         checkResultCode();
         n = getExpectedConnectionID();
         isThereAnOutboundMessage = 0;
         break;

    case 4:                                 // Get result of play or send digits
         checkResultCode();
         n = getExpectedConnectionID();
                                            // Send an IP disconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         printf("TEST sending session disconnect\n");              
         break;

    case 5:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

  if  (isThereAnOutboundMessage)
  {
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



int stopMediaOperationTestA(const unsigned int serverconnect) // (t)
{
  // This test sequence connects to server, connects a session, plays a file OR
  // does a GetDigits, interrupts media, disconnects session, disconnects server

  // #define DO_STOP_MEDIA_SYNC   // set this to block on dx_stopch

     #define TEST_STOP_GETDIGITS  // set this to getDigits instead of play

  static int iters, isserverconnect;
  if  (iters++ == 0) log("\nBegin stop media operation test A\n\n");
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
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         #ifdef TEST_STOP_GETDIGITS

         termcondpname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n);  
         outxml->putParameter(termcondpname, "maxtime 5000");   // 5 seconds
         outxml->putParameter(termcondpname, "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending getDigits\n");

         #else
                                                      // Play a few announcments
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 13000");   // 13 seconds
         outxml->putParameter(termcondname, "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         log("TEST sending play\n");

         #endif
   
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send stop after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n);
 
         #ifdef DO_STOP_MEDIA_SYNC
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::BLOCK], 1);
         #endif
 
         log("TEST sending stopMediaOperation\n");                         
         break;
                                            
    case 3:                                 // Get result of play or stop
         checkResultCode();
         n = getExpectedConnectionID();
         isThereAnOutboundMessage = 0;
         break;

    case 4:                                 // Get result of play or stop
         checkResultCode();
         n = getExpectedConnectionID();
                                            // Send an IP disconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         log("TEST sending session disconnect\n");              
         break;

    case 5:                                 // Disconnect from media server
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


int getDigitsTimeout(const unsigned int serverconnect) // (t)
{
  // Connects one session, does a getdigits with a 30-second timeout, and waits
  static int iters, isserverconnect;
  if  (iters++ == 0) log("\nBegin getdigits timeout test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1;
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
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         termcondpname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::COMMAND_TIMEOUT], 30000); // 30 secs timeout 
         outxml->putParameter(termcondpname, "maxtime 180000");   // 3 minutes
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending getDigits\n");
         break;       
                                            
    case 3:                                  
         checkResultCode();
         Sleep(40);
         sendServerDisconnect(outxml);            
         result = -1;
  }

  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();   
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
} 



int stopMediaOperationTestB(const unsigned int serverconnect)
{
  // This test sequence connects to server, does not connect any session,
  // sends a stop media command using a bogus connection ID.
  // This test is not by default hooked up to the test suite menu,
  // We merely rename this to stopMediaOperationTestA to use it.

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin stop media test with no connections\n\n");
  int  result = 1, isThereAnOutboundMessage = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;

    case 1:
         if  (-1 == checkResultCode()) return -1;
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], 1); 
         printf("TEST sending stopMediaOperation\n");                         
         break;                                           

    case 3:                                 // Disconnect from media server
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


int stopMediaOperationTestC(const unsigned int serverconnect) // (t)
{
  // This test sends a stop media with no media to stop

  #define DO_STOP_MEDIA_SYNC   // set this to block on dx_stopch

  static int iters, isserverconnect, conxID;
  if  (iters++ == 0) printf("\nBegin stop media operation test A\n\n");
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
         printf("TEST sending session connect\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxID = getExpectedConnectionID())) return -1;                                                      
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox");
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 2000");   // 2 seconds
         log("TEST sending short play\n");                         
         break;
                                            
    case 3:                                 // Get result of play, send a stop media
         if  (-1 == checkResultCode()) return -1;
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         printf("TEST sending stopMediaOperation\n");           
         break;

    case 4:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
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



int disconnectBargeInTestA(const unsigned int serverconnect)  // (A)
{
  // This test sequence connects to server, connects a session, plays a
  // file, disconnects session interrupting play, disconnects server

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin stop media operation test A\n\n");
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
         printf("TEST sending session connect\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
                                                      // Play a few announcments
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 13000");   // 13 seconds
         outxml->putParameter(termcondname, "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);

         printf("TEST sending play\n");   
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send disco after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         printf("TEST sending session disconnect\n");                         
         break;

    case 3:                                 // Disconnect from media server
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



int disconnectBargeInTestB(const unsigned int serverconnect)
{
  // This test sequence establishes a conference, plays a
  // file to session 0 conferee, disconnects session 0 interrupting play, 
  // disconnects server

  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin stop media operation test A\n\n");
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
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         printf("TEST sending session/conference connect 3\n");
         break;

    case 4:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                                      // Play a few announcments
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 13000");   // 13 seconds
         outxml->putParameter(termcondname, "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);

         printf("TEST sending play to conx %d\n", conxIds[0]);   
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send disco after 3 seconds

         // If we want to test disco removing session from conference but keeping
         // session alive, include both conxID and confID. If we want to test
         // disco removing session from conference and closing session, specify
         // just conxID and not confID.

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         //txml->putParameter(outxml->paramnames  [MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         printf("TEST sending session disconnect\n");                         
         break;

    case 5:                                 // Disconnect from media server
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



int disconnectBargeInTestC(const unsigned int serverconnect)  // not on menu
{
  // This test sequence connects to server, connects a session, gets digits,
  // barge discos session, gets digits again, barge discos again,
  // and disconnects server

  static int iters, isserverconnect, conxID;
  if  (iters++ == 0) printf("\nBegin stop media operation test A\n\n");
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

    case 1:                                 // Send an IP connect with conference connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         printf("TEST sending session connect\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxID = getExpectedConnectionID())) return -1;
                                                      // Play a few announcments
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID);         
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 13000");   // 13 seconds
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);

         printf("TEST sending getdigits\n");   
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send disco after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         printf("TEST sending session disconnect\n");                         
         break;

    case 3:
         if  (-1 == checkResultCode()) return -1;
         isThereAnOutboundMessage = FALSE;
         break;

    case 4:                                 // Send an IP connect with conference connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0);  
         printf("TEST sending session connect\n");
         break;

    case 5:  
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxID = getExpectedConnectionID())) return -1;                                                     
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 13000");   // 13 seconds
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);

         printf("TEST sending getdigits\n");   
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send disco after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         printf("TEST sending session disconnect\n");                         
         break;

    case 6:                                 // Disconnect from media server
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



int recordAudioAndPlaybackSequence(const unsigned int serverconnect) // (a)
{
  // This test sequence connects to server, connects a session, records a
  // file, plays it back, disconnects session, disconnects server
  // Test modified to test either wav or vox format play/rec files.
  // Test modified to also test two-part silence parameter n:m
  // which is initial silence followed by non-initial silence

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin record/playback test\n\n");
  char* p = NULL, *termcondname = NULL, *paramname = NULL, *fn = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  // #define MMS_TESTING_WAV
  #define MMS_TESTING_FULLPATH
  // #define MMS_TESTING_MAPPED_DRIVE

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
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         putDefaultLocaleParameters(outxml);

         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE]; 

         #ifdef MMS_TESTING_WAV
         outxml->putParameter(paramname, "format wav"); 
         // fn = "adhoc.wav";
         // outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn); 
         #else
         outxml->putParameter(paramname, "format vox"); 
         #if defined(MMS_TESTING_FULLPATH)
         fn = "C:\\Documents and Settings\\james\\Desktop\\adhoc.vox";
         #elif defined(MMS_TESTING_MAPPED_DRIVE)
         fn = "$scratch\\adhoc.vox";
         #else
         fn = "adhoc.vox";
         #endif
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn); 
         #endif

         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 3000");   // 3 seconds
         // outxml->putParameter(termcondname, "silence 1000:3000"); 
 
         log("TEST sending record\n");  
         isShowProvisionalContent = TRUE;            
         break;

    case 3:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         if  (NULL == (p = getReturnedRecordPath())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::FILE_NAME], p); 
         putDefaultLocaleParameters(outxml);

         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 5000");   // 5 seconds
         outxml->putParameter(termcondname, "silence 1000:3000"); 

         sprintf(getbuf(),"TEST sending play of %s", p); log();
         break;

    case 4:                                 // Send an IP disconnect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         log("TEST sending session disconnect\n");              
         break;

    case 5:                                 // Disconnect from media server
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



int sendConcurrentCommandSequence(const unsigned int serverconnect)  // (h)
{
  // Sends a batch of connects, listening for the returns afterward.
  // Sends a batch of plays, listening later
  // Discos each connection and discos server

  static int iters, isserverconnect, conxIds[4];
  if  (iters++ == 0) printf("\nBegin concurrent command test\n\n");
  char* p = NULL;
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
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending session 1 connect\n");
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending session 2 connect\n");
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session 3 connect\n");
         break;

    case 2:                                 // Get result of connect 1
         showResultCode();
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         isThereAnOutboundMessage = 0;
         break;

    case 3:
         showResultCode();                  // Get result of connect 2
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         isThereAnOutboundMessage = 0;
         break;

    case 4:
         showResultCode();                  // Get result of connect 3
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
                                            // Send server query
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SERVER]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::QUERY], 
                 MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES]);
         printf("TEST sending server query\n");
         break;

    case 5:
         showResultCode();                  // Get result of query
                                            // Send play on each session
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending session 1 play\n");
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending session 2 play\n");
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;

         delete outxml;
         outxml = new MmsAppMessage;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         printf("TEST sending session 3 play\n");
         break;

    case 6:                                 // Get result of play 1
         showResultCode();
         isThereAnOutboundMessage = 0;
         break;

    case 7:                                 // Get result of play 2
         showResultCode();
         isThereAnOutboundMessage = 0;
         break;

    case 8:                                 // Get result of play 3
         showResultCode();                  // Send session 1 disco
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[0]);              
         break;

    case 9:                                 // Send session 2 disco
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[1]);              
         break;

    case 10:                                // Send session 3 disco
         showResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[2]);
         printf("TEST sending conference/session disconnect for connection %d\n",conxIds[2]);              
         break;
 
    case 11:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }


  if  (isThereAnOutboundMessage)
  {
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



int serverQuerySequenceA(const unsigned int serverconnect)
{
  static int iters, isserverconnect, conxIds[4];     
  if  (iters++ == 0) printf("\nBegin concurrent command test\n\n");
  char* p = NULL;
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
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SERVER]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::QUERY], 
                 MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES]);
         printf("TEST sending server query\n");
         break;

    case 2:                                  
         showResultCode();
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

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



int sethsTestA()
{
  // This test sequence connects to server, connects a session without starting,
  // the session, completes the session connection, disconnects session, 
  // disconnects server
  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin consecutive connect test\n\n");
  char* p = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0: 
         sendServerConnect(outxml);   
         isserverconnect = TRUE;
         break;

    case 1:                                 // Send an IP connect
         clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session CONNECT\n");
         break;

    case 2:                                 // Send an IP disconnect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         printf("TEST sending session disconnect\n");              
         break;

    case 3:                                  // Disconnect from media server        
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         printf("TEST sending first media server disconnect\n");           
         break;

    case 4:                                 // Send second server connect
         sendServerConnect(outxml);        
         break;

    case 5:                                 // Send an IP connect
         clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         printf("TEST sending session CONNECT\n");
         break;

    case 6:                                 // Send an IP disconnect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         printf("TEST sending session disconnect\n");              
         break;

    case 7:                                  // Disconnect from media server        
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


int heartbeatTestA(const unsigned int serverconnect)
{
  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin heartbeat test A\n\n");
  char* p = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0: 
         heartbeatInterval = 10;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::HEARTBEAT_INTERVAL], heartbeatInterval);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::HEARTBEAT_PAYLOAD], 
                              MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES]);
         #ifndef USING_DEFAULT_APP_QUEUE             
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::MACHINE_NAME], machineName);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::QUEUE_NAME],   queueName);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID], 0);
         #endif
         printf("TEST sending media server connect\n");       
         isserverconnect = TRUE;
         break;

    case 1:                                 
         clientID = getMessageClientID();
         isShowHeartbeatContent = TRUE;
         printf("TEST sleeping %d secs ...\n", heartbeatInterval);
         mmsSleep(heartbeatInterval);       // Wait long enough for a heartbeat
 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SERVER]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::QUERY], 
                 MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES]);
         printf("TEST sending server query\n");
         break;

    case 2:                                 // Disconnect from media server 
         sendServerDisconnect(outxml);              
         result = -1;
         break;
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



int testPlaytone(const unsigned int serverconnect)  // (W)
{                                                                  
  static int iters, isserverconnect, conxID;
  if  (iters++ == 0) printf("\nBegin playTone test\n\n");
  char* p = NULL, *paramname = NULL, *fn = NULL;
  int   n = 0, result = 1;
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
         printf("TEST sending session connect\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxID = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAYTONE]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_TONE_ATTRIBUTE]; 
                                
         outxml->putParameter(paramname, "frequency1 600");
         outxml->putParameter(paramname, "amplitude1 -15");
         outxml->putParameter(paramname, "frequency2 800");
         outxml->putParameter(paramname, "amplitude2 -14");
         outxml->putParameter(paramname, "duration 1500");

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         
         printf("TEST sending playtone\n", fn);              
         break;

    case 3:                                 // Send an IP disconnect
         checkResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         printf("TEST sending session disconnect\n");              
         break;

    case 4:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
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



int hairpinTestA(const unsigned int serverconnect) // (6)
{
  // This test exercises conference hairpinning, digits, record, and play to
  // hairpinned party, and HMP conference promotion/demotion.

  // The test sequence connects to server, connects two parties to a conference,
  // leaves and rejoins party 2, gets digits from party 2, records party 2 and plays
  // recording back to party 2, adds a third party to conference, leaves each
  // conferee in turn, disconnects server

  // This test behaves differently depending upon config parameters controlling
  // conference hairpinning and HMP promotion/demotion behavior.

  // When iparamA is 53444, iparamB and iparamC are interpreted as follows:
  // #define ID_HAIRPINNING 53444           // iparamA

  // These are valid values for the client "hairpin" parameter
  // this value is passed in config iparamB
  #define HPIN_CLIENT_PARAM_NOHAIRPIN 0     // iparamB
  #define HPIN_CLIENT_PARAM_HAIRPIN   1     // iparamB

  // These are valid values for the client "hairpinPromote" parameter
  // this value is passed in config iparamC
  #define HPIN_CLIENT_PARAM_PROMOTE_UNSPEC  0   // iparamC
  #define HPIN_CLIENT_PARAM_PROMOTE_PROMOTE 1   // iparamC
  #define HPIN_CLIENT_PARAM_PROMOTE_PROMOTE_DEMOTE 2  // iparamC
  #define HPIN_CLIENT_PARAM_PROMOTE_PROMOTE_NEVER  4  // iparamC

  // These are defined in config.h, and are valid values for the 
  // config file "Server.hairpinOpts" entry
  // #define MMS_DEFAULT_HAIRPIN_OPTS              0
  // #define MMS_HAIRPIN_OFF_UNLESS_OVERRIDE       0
  // #define MMS_HAIRPIN_ON_UNLESS_OVERRIDE        1
  // #define MMS_HAIRPIN_NEVER                     2

  // These are defined in config.h, and are valid values for the  
  // config file "Server.hairpinPromotionOpts" entry,  
  // #define MMS_DEFAULT_HAIRPIN_PROMOTE           1
  // #define MMS_HPIN_PROMOTE_OFF_UNLESS_OVERRIDE  0
  // #define MMS_HPIN_PROMOTE_ON_UNLESS_OVERRIDE   1  
  // #define MMS_HPIN_DEMOTE_ON_UNLESS_OVERRIDE    2 
  // #define MMS_HPIN_PROMOTE_NEVER                4
  // #define MMS_HPIN_DEMOTE_NEVER                 8 

  #define HPIN_TEST_ABANDON_CONFERENCE // define to abandon rather than leave each party

  static int iters, isserverconnect, conferenceID, conxIds[8];
  static int clientHairpinParam = HPIN_CLIENT_PARAM_HAIRPIN;
  static int clientPromoteParam = HPIN_CLIENT_PARAM_PROMOTE_PROMOTE;

  if (iters++ == 0)
  {
      sprintf(getbuf(),"\nTEST begin hairpin test A\n"); log();
      const int isParamsForThisTest = config->iparamA == ID_HAIRPINNING;

      if (isParamsForThisTest)
      {   // If config parameters are for this test, get params from config
          clientHairpinParam = config->iparamB;
          clientPromoteParam = config->iparamC;
      }

      sprintf(getbuf(),"TEST client param 'hairpin' is %d\n", clientHairpinParam); log();
      sprintf(getbuf(),"TEST client param 'hairpinPromote' is %d\n\n", clientPromoteParam); log();
      MmsAppMessageX::instance();
      conxIDparamname = MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID];
      mmsSleep(MMS_HALF_SECOND);
  }

  char* p = NULL, *termcondname = NULL;
  int   n = 0, isThereAnOutboundMessage = 1, result = 1;

  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;

    case 1:                                 // Send party 1 IP connect with conf connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::HAIRPIN], clientHairpinParam); 
         log("TEST sending session/conference connect 1\n");
         break;

    case 2:                                // Connect party 2 to conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         // showReturnedPortAndIP();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], conferenceID);
         log("TEST sending join party 2\n");
         break;
                                            
    case 3:                                 // Take party 2 out of conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1]   = getExpectedConnectionID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(conxIDparamname, conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);
         log("TEST sending leave party 2\n");              
         break;

    #if(1)
    case 4:                                 // Play to single "hairpinnee"
         if  (-1 == checkResultCode()) return -1;
         fnparamname = MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME];
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(conxIDparamname, conxIds[0]); 
         outxml->putParameter(fnparamname,"thankyou.vox");
         log("TEST sending play to lone party 1\n");
         break;
    #else
    case 4:                                 // Alternate bogus stop media test
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(conxIDparamname, conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::BLOCK], 1); 
         log("TEST sending unexpected stopMediaOperation\n");                         
         break;
    #endif        

    case 5:                                // Join party 2 back to conference
         if  (-1 == checkResultCode()) return -1;      
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);         
         outxml->putParameter(conxIDparamname, conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         log("TEST sending rejoin party 2\n");
         break;

    case 6:                                // Get digits and send digits on party 2
         if  (-1 == checkResultCode()) return -1;  
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;  
         termcondpname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(conxIDparamname, conxIds[1]); 
         outxml->putParameter(termcondpname, "maxtime 5000");   // 5 seconds
         outxml->putParameter(termcondpname, "maxdigits 4"); 
         outxml->putParameter(termcondpname, "digit #"); 

         log("TEST sending get digits on party 2\n");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;

         log("TEST waiting 1 seconds ...\n");                            
         mmsSleep(1);                       // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(conxIDparamname, conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"1123#");
         log("TEST sending sendDigits to party 2\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         isThereAnOutboundMessage = false;
         mmsSleep(MMS_HALF_SECOND);           
         break;

    case 7:                                 // Terminations for both sendDigits
         checkResultCode();                 // and getDigits are expected, 
         isThereAnOutboundMessage = false;  // so wait for both
         break;

    case 8:
         checkResultCode();                 // Record conferee 2
         fnparamname = MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME];
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(conxIDparamname, conxIds[1]); 
         outxml->putParameter(fnparamname,"myrecfile.vox");
         outxml->putParameter(termcondpname, "maxtime 2000");   // 2 seconds
         log("TEST sending record party 2\n");

    case 9:
         checkResultCode();                 // Play conferee 2
         fnparamname = MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME];
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(conxIDparamname, conxIds[1]); 
         outxml->putParameter(fnparamname,"myrecfile.vox");
         outxml->putParameter(termcondpname, "maxtime 3000");    
         log("TEST sending play party 2\n");
         break;

    case 10:                                // Attempt to join a third party to conference
         if  (-1 == checkResultCode()) return -1;  
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;    
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1004);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         log("TEST sending join party 3\n");
         break;

    #ifndef HPIN_TEST_ABANDON_CONFERENCE
    case 11:                                // Leave and disco party 3
         checkResultCode();
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;            
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(conxIDparamname, conxIds[2]); 
         printf("TEST sending leave and disco party 3\n");              
         break;

    case 11:                                // Leave and disco party 2
         checkResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(conxIDparamname, conxIds[1]); 
         log("TEST sending leave and disco party 2\n");              
         break;

    case 13:                                // Leave and disco party 1
         checkResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(conxIDparamname, conxIds[0]); 
         log("TEST sending leave and disco party 1\n");              
         break;
                                            
    case 14:  
    #else   // #ifndef HPIN_TEST_ABANDON_CONFERENCE  
    case 11:
    #endif  // #ifndef HPIN_TEST_ABANDON_CONFERENCE                                                                     
         if  (serverconnect & SERVER_DISCO) // Disconnect from media server
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