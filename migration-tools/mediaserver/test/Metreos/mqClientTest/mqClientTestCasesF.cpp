// 
// mqClientTestCases.cpp - client process to exercise the MmsMqAppAdapter  
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mqClientTest.h"
#include <minmax.h>

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
extern int  returnPort1;
extern int  interCommandSleepMs;  
extern int  isShowHeartbeatContent; 
extern int  heartbeatInterval;
extern int  isNoShowInboundXml;
extern int  isShowProvisionalContent;
extern int  isPassthruProvisional; 
extern char queueName[32];
extern char machineName[128];
extern char returnIP[32];
extern char returnIP1[32];

static char *connectcommand, *playcommand, *recordcommand, *adjustplaycommand, *conxidparamname;
static char *confidparamname, *termcondparamname, *disconnectcommand, *stopmediacommand;  
static char *filenameparamname, *volumeparamname, *speedparamname, *adjtypeparamname, *togtypeparamname;
static char *portparamname, *ipparamname, *transidparamname;

#define ID_HALFCONNECTLOOP    53445         // iparamA
#define HCL_NUMITERATIONS     20            // iparamB -- how many iterations to run
#define HCL_DISCODELAY        0             // iparamC -- how long to delay before disco


void setGlobalsF()
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



int testConcurrentTTS(const unsigned int serverconnect) // ({)
{ 
  static int iters, isserverconnect, conferenceID, conxIds[5];
  static char hugeTTSstring[1024];

  if (iters++ == 0) 
  { log("\nbegin concurrent TTS test\n\n");
    setGlobalsF();

    strcpy(hugeTTSstring, 
    //          1         2         3         4         5         6         7         8
    // 12345678901234567890123456789012345678901234567890123456789012345678901234567890
      "now is the time, for all good men to come to the aid of their party. now is the "  
      "time for all good men to come to the aid of their party. now is the time for all"  
      " good men to come to the aid of their party. now is the time for all good men to"  
      " come to the aid of their party. now is the time for all good men to come to the"  
      " aid of their party. now is the time, for all good men, to come to, the aid of, " 
      "their party. now is the time for all good men to come to the aid of their party." 
      "now is the time, for all good men to come to the aid of their party. now is the "  
      "time for all good men to come to the aid of their party. now is the time for all"  
      " good men to come to the aid of their party. now is the time for all good men to"  
      " come to the aid of their party. now is the time for all good men to come to the"  
      " aid of their party. now is the time, for all good men, to come to, the aid of, " 
      "their party. now is the time for all good men to come to the aid of their party."                 
    );
  }
  
  int   n = 0, result=1, isThereAnOutboundMessage=1;
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;
                                             
    case 1:                                 // Connect party 1
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1000);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         log("TEST sending session connect 0\n");
         break; 

    case 2:                                 // Connect party 2
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1002);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         log("TEST sending session connect 1\n");
         break;    

    case 3:                                // Connect party 3
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1004);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         log("TEST sending session connect 2\n");
         break;   

    case 4:                                 // Connect party 4
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1006);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         log("TEST sending session connect 3\n");
         break;  

    case 5:                                 // Connect party 5
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[3] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1008);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         log("TEST sending session connect 4\n");
         break;     

    case 6:   // send a bunch of huge concurrent tts plays     
         if  (-1 == checkResultCode()) return -1; // play to 0 
         if  (-1 == (conxIds[4] = getExpectedConnectionID())) return -1;  
         outxml->putMessageID(playcommand);
         outxml->putParameter(conxidparamname, conxIds[0]); 
         outxml->putParameter(filenameparamname, hugeTTSstring);                  
         log("TEST sending huge TTS play to session 0\n"); 
         outxml->putParameter(transidparamname, ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log(); 
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());            
         delete outxml;
         outxml = new MmsAppMessage; 

         outxml->putMessageID(playcommand);
         outxml->putParameter(conxidparamname, conxIds[1]); 
         outxml->putParameter(filenameparamname, hugeTTSstring);                  
         log("TEST sending huge TTS play to session 1\n"); 
         outxml->putParameter(transidparamname, ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log(); 
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());            
         delete outxml;
         outxml = new MmsAppMessage; 

         outxml->putMessageID(playcommand);
         outxml->putParameter(conxidparamname, conxIds[2]); 
         outxml->putParameter(filenameparamname, hugeTTSstring);                  
         log("TEST sending huge TTS play to session 2\n"); 
         outxml->putParameter(transidparamname, ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log(); 
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());            
         delete outxml;
         outxml = new MmsAppMessage; 

         outxml->putMessageID(playcommand);
         outxml->putParameter(conxidparamname, conxIds[3]); 
         outxml->putParameter(filenameparamname, hugeTTSstring);                  
         log("TEST sending huge TTS play to session 3\n"); 
         outxml->putParameter(transidparamname, ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log(); 
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());            
         delete outxml;
         outxml = new MmsAppMessage; 

         outxml->putMessageID(playcommand);
         outxml->putParameter(conxidparamname, conxIds[4]); 
         outxml->putParameter(filenameparamname, hugeTTSstring);                  
         log("TEST sending huge TTS play to session 4\n"); 
        
         break;

    case 7:                                        // play 0 result 
         isThereAnOutboundMessage = FALSE;        
         break; 

    case 8:                                        // play 1 result 
         isThereAnOutboundMessage = FALSE;        
         break; 

    case 9:                                        // play 2 result 
         isThereAnOutboundMessage = FALSE;        
         break; 

    case 10:                                       // play 3 result 
         isThereAnOutboundMessage = FALSE;        
         break; 

    case 11:                                       // play 4 result   
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
  }


  if (isThereAnOutboundMessage)
  {
      outxml->putParameter(transidparamname, ++transID);
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



int testVolumeSpeedControl(const unsigned int serverconnect) // (7)
{ 
  // Tests a few vol/speed options
  // 1. Volume, on Play command
  // 2. Speed, on Play command
  // 3. Invalid volume value, on Play
  // 4. adjustPlay command, no other command active
  // 5. adjustPlay command, play command active

  static int iters, isserverconnect, conferenceID, conxIds[4];

  if (iters++ == 0) 
  { log("\nbegin volume/speed control test\n\n");
    setGlobalsF();
  }
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result=1, isThereAnOutboundMessage=1;
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:                                 // Connect to media server
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1);   
         break;
                                             
    case 1:                                 // Send a session connect
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
                                            // Check if volume/speed configged
         if (config->hmp.volume != 0 || config->hmp.speed != 0)
             log("\nTEST WARNING volume or speed configured -- subtests will not begin with default\n\n");                                             
         log("TEST sending session connect 1\n");
         break;   
   
    //case 2:                                 // Do a play with volume adjustment
    //     if  (-1 == checkResultCode()) return -1;
    //     if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;        
    //     outxml->putMessageID(playcommand);
    //     outxml->putParameter(conxidparamname, conxIds[0]); 
    //     outxml->putParameter(filenameparamname,"stoprec.vox");
    //     outxml->putParameter(termcondparamname, "maxtime 1500"); 
    //     outxml->putParameter(volumeparamname, 5); 
    //     log("TEST sending play with volume adjustment\n");   
    //     break;   

    //case 3:                                 // Do a play with speed adjustment 
    //     if  (-1 == checkResultCode()) return -1;
    //     outxml->putMessageID(playcommand);
    //     outxml->putParameter(conxidparamname, conxIds[0]); 
    //     outxml->putParameter(filenameparamname,"stoprec.vox");
    //     outxml->putParameter(termcondparamname, "maxtime 1500"); 
    //     outxml->putParameter(speedparamname, 5); 
    //     log("TEST sending play with speed adjustment\n");   
    //     break;  

    //case 4:                                 // Do a play with invalid speed adjustment 
    //     if  (-1 == checkResultCode()) return -1;
    //     outxml->putMessageID(playcommand);
    //     outxml->putParameter(conxidparamname, conxIds[0]); 
    //     outxml->putParameter(filenameparamname,"stoprec.vox");
    //     outxml->putParameter(termcondparamname, "maxtime 1500"); 
    //     outxml->putParameter(speedparamname, -11); 
    //     log("TEST sending play with INVALID speed adjustment, should return 27\n");   
    //     break; 
    //     
    //case 5:                                 // Do an adjustPlay comand with no command active 
    //     if  (-1 == checkResultCode(27)) return -1; // indicate rc=27 OK
    //     outxml->putMessageID(adjustplaycommand);
    //     outxml->putParameter(conxidparamname, conxIds[0]); 
    //     outxml->putParameter(volumeparamname, 3); 
    //     outxml->putParameter(adjtypeparamname, "abs");
    //     log("TEST sending adjustPlay with adjtype 'abs'\n");   
    //     break; 
    //
    //case 6:  
    //     if  (-1 == checkResultCode()) return -1;
    //     isPassthruProvisional = TRUE;            // we want to see play provisional
    //     outxml->putMessageID(playcommand);
    //     outxml->putParameter(conxidparamname, conxIds[0]); 
    //     outxml->putParameter(filenameparamname,"stoprec.vox");
    //     outxml->putParameter(termcondparamname, "maxtime 4000"); 
    //     log("TEST sending play with NO adjustments\n"); 
    //     break;
  
    //case 7:
    //     if (-1 == checkResultCode(1)) return -1; // play provisional
    //     isPassthruProvisional = FALSE;           // Send adjustPlay over the play 
    //     outxml->putMessageID(adjustplaycommand);
    //     outxml->putParameter(conxidparamname, conxIds[0]); 
    //     outxml->putParameter(volumeparamname, -1); 
    //     outxml->putParameter(adjtypeparamname, "rel");
    //     log("TEST sending adjustPlay with adjtype 'rel'\n");   
    //     break;  

    //case 8:
    //     if (-1 == checkResultCode(1)) return -1; // adjustPlay result 
    //     isThereAnOutboundMessage = FALSE;        
    //     break;   

    case 2:       
         if  (-1 == checkResultCode()) return -1; // play final result; send another play
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1; // *** remove ***
         outxml->putMessageID(playcommand);
         outxml->putParameter(conxidparamname, conxIds[0]); 
         outxml->putParameter(filenameparamname,"stoprec.vox");
         outxml->putParameter(termcondparamname, "maxtime 3000"); 
         isPassthruProvisional = TRUE; 
         log("TEST sending play\n");   
         break;

    case 3:
         if (-1 == checkResultCode(1)) return -1;// play provisional; send a toggle adjustment
         isPassthruProvisional = FALSE;
         outxml->putMessageID(adjustplaycommand);
         outxml->putParameter(conxidparamname, conxIds[0]); 
         outxml->putParameter(volumeparamname, 0); 
         outxml->putParameter(adjtypeparamname, "tog");
         outxml->putParameter(togtypeparamname, 1);
         log("TEST sending adjustPlay with adjtype 'tog', togtype 1\n");  
         break;

    case 4:
         checkResultCode();                 // adjustPlay result 
         isThereAnOutboundMessage = FALSE;        
         break;  

    case 5:                                
         checkResultCode();                 // play final result 
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
  }


  if (isThereAnOutboundMessage)
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



int voiceRecTest(const unsigned int serverconnect)    // (8)
{   
  static int iters, isserverconnect, conxIds[2];
  if  (iters++ == 0) printf("\nBegin voice recognition test\n\n");
  char* p = NULL, *paramname = NULL, *fn = NULL, *gn = NULL, *gn1 = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;
  int isThereAnOutboundMessage = 1;

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
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         printf("TEST sending session 1 connect\n");
         break;

    case 2:                                 // Send an IP half connect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         // backup port and IP
         returnPort1 = returnPort; 
         memset(&returnIP1, 0, sizeof(returnIP1));
         memcpy(&returnIP1, &returnIP, sizeof(returnIP));
        
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         printf("TEST sending session 1 connect\n");
         break;

    case 3:                                 // Send a full connect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], returnPort);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], returnIP); 
         printf("TEST sending session 1 connect to session 2 local IP/port %s %d\n", returnIP, returnPort);
         break;

    case 4:                                 // Send a full connect
         if  (-1 == checkResultCode()) return -1;
         
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], returnPort1);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], returnIP1); 
         printf("TEST sending session 2 connect to session 1 local IP/port %s %d\n", returnIP1, returnPort1);
         break;

    case 5:
         {
         if  (-1 == checkResultCode()) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_VOICEREC]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 

         char c=0; 
         printf("\n\nEnter '1' to skip voice prompt, or any other key to continue:\n"); 
         while(!c) {
            c=_getch();
         }

         if (c != '1')
         {
           paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE];                                 
           outxml->putParameter(paramname, "format wav");
           fn = "welcome.wav";
           outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn);  
         }

         gn = "digits.grxml";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::GRAMMAR_NAME], gn);  

         printf("TEST sending VR request with prompt of %s and grammar of %s\n", fn, gn);  

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
            
         delete outxml;

         outxml = new MmsAppMessage;                                            
         mmsSleep(2);                       // Wait 5 seconds and start playing

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         fn = "longprompt.vox";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],fn);
         printf("TEST sending session 2 play to session 1\n", fn); 
         break;
         }

    case 6:                                 // Get result of play or stop

         checkResultCode();
         isThereAnOutboundMessage = 0;
         break;

    case 7:
         showVoiceRecognitionResult();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         log("TEST sending session 1 disconnect\n");              
         break;

    case 8:
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         log("TEST sending session 2 disconnect\n");              
         break;

    case 9:                                 // Get result of play or stop
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
     


int concurrentPlayGetDigits(const unsigned int serverconnect)     // (9)
{
  // This test sequence connects to server, connects a session, plays an
  // announcement and gathers some digits, disconnects session, disconnects server

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin concurrent play and get digits test\n\n");
  char* p = NULL, *paramname = NULL, *fn = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage=1;
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
         printf("TEST sending session/conference connect 1\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         isPassthruProvisional = TRUE;            // we want to see play provisional
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE];                                 
         outxml->putParameter(paramname, "format wav");
         fn = "sample_music.wav";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION], "digit #"); 
                 
         printf("TEST sending play of %s\n", fn);              
         break;

    case 3:                                 
         if  (-1 == checkResultCode(1)) return -1; 
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         isPassthruProvisional = TRUE;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 10000");   // 10 seconds
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxdigits 10"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         break;

    case 4:                                 
         if  (-1 == checkResultCode(1)) return -1; 
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         isPassthruProvisional = FALSE;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);   
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], "123");  
         log("TEST sending digits 123\n");         
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());         
         delete outxml;

         mmsSleep(1);                       

         outxml = new MmsAppMessage;                                           
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);   
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], "#");  
         log("TEST sending digits #\n");         

         break;

    case 5:                                 // senddigits
         checkResultCode(1);
         isThereAnOutboundMessage = 0;
         break;

    case 6:                                 // senddigits
         checkResultCode(1);
         isThereAnOutboundMessage = 0;
         break;

    case 7:                                 // Send an IP disconnect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         printf("TEST sending session disconnect\n");              
         break;

    case 8:                                 // Disconnect from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

  if (isThereAnOutboundMessage)
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



int concurrentVoiceRecGetDigits(const unsigned int serverconnect)     // @
{
  static int iters, isserverconnect, conxIds[2];
  if  (iters++ == 0) printf("\nBegin voice recognition test\n\n");
  char* p = NULL, *paramname = NULL, *fn = NULL, *gn = NULL, *gn1 = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;
  int isThereAnOutboundMessage = 1;

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
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         printf("TEST sending session 1 connect\n");
         break;

    case 2:                                 // Send an IP half connect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         // backup port and IP
         returnPort1 = returnPort; 
         memset(&returnIP1, 0, sizeof(returnIP1));
         memcpy(&returnIP1, &returnIP, sizeof(returnIP));
        
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         printf("TEST sending session 1 connect\n");
         break;

    case 3:                                 // Send a full connect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], returnPort);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], returnIP); 
         printf("TEST sending session 1 connect to session 2 local IP/port %s %d\n", returnIP, returnPort);
         break;

    case 4:                                 // Send a full connect
         if  (-1 == checkResultCode()) return -1;
         
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], returnPort1);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], returnIP1); 
         printf("TEST sending session 2 connect to session 1 local IP/port %s %d\n", returnIP1, returnPort1);
         break;

    case 5:                                 
         if (-1 == checkResultCode()) return -1; 
         isPassthruProvisional = TRUE;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 10000");   // 10 seconds
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxdigits 10"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         break;

    case 6:
         if  (-1 == checkResultCode(1)) return -1;
         isPassthruProvisional = TRUE;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_VOICEREC]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE];                                 
         outxml->putParameter(paramname, "format wav");
         fn = "welcome.wav";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn);  
         gn = "digits.grxml";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::GRAMMAR_NAME], gn);  
         printf("TEST sending VR request with prompt of %s and grammar of %s\n", fn, gn);  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());         
         delete outxml;

         outxml = new MmsAppMessage;                                            
         mmsSleep(2);                       // Wait 2 seconds and start playing

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         fn = "longprompt.vox";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],fn);
         printf("TEST sending session 2 play to session 1\n", fn);              
         break;

    case 7:                                 // Get result of play or stop
         if (-1 == checkResultCode(1)) return -1; 
         isPassthruProvisional = FALSE;
         isThereAnOutboundMessage = 0;
         break;

    case 8:                                 
         if (-1 == checkResultCode()) return -1; 

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);   
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], "123");  
         log("TEST sending digits 123\n");         
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());         
         delete outxml;

         mmsSleep(1);                       

         outxml = new MmsAppMessage;                                           
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);   
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], "#");  
         log("TEST sending digits #\n");         
         break;

    case 9:                                
         checkResultCode(1);
         isThereAnOutboundMessage = 0;
         break;

    case 10:                                
         checkResultCode(1);
         isThereAnOutboundMessage = 0;
         break;

    case 11:
         showVoiceRecognitionResult();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         log("TEST sending session 1 disconnect\n");              
         break;

    case 12:
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         log("TEST sending session 2 disconnect\n");              
         break;

    case 13:                                 // Get result of play or stop
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



int stopMediaOnConcurrentOperations(const unsigned int serverconnect)     // (^)
{
  // Tests stop media on either operation of two concurrent operations,
  // and on both operations

  static int iters, isserverconnect, conxID, playOpID, recordOpID;
  if  (iters++ == 0) printf("\nBegin concurrent stop media test\n\n");
  char* p = NULL, *paramname = NULL, *fn = NULL;
  int   n = 0, result = 1, isThereAnOutboundMessage=1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;

  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1); 
         break;

    case 1:                                 // Send an IP connect
         if (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending session connect\n");
         break;

    case 2:
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxID = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         isPassthruProvisional = TRUE;      // We want to see play provisional
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE];                                 
         outxml->putParameter(paramname, "format wav");
         fn = "sample_music.wav";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];
         outxml->putParameter(paramname, "maxtime 15000");    
         sprintf(getbuf(),"TEST sending play of %s\n", fn); log();               
         break;

    case 3:                                 // Check play provisional, send record
         if (-1 == checkResultCode(1)) return -1;    
         if (-1 == (playOpID = getExpectedOperationID())) return -1;
         isPassthruProvisional = TRUE;      // We want record provisional
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];
         outxml->putParameter(paramname, "maxtime 10000");  
         log("TEST sending record\n");  
         break;

    case 4:                                 // Check record provisional, send stop media
         if (-1 == checkResultCode(1)) return -1; 
         if (-1 == (recordOpID = getExpectedOperationID())) return -1; 
         log("TEST wait 3 seconds and send stop media on record\n"); 
         mmsSleep(3);
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::OPERATION_ID], recordOpID);
         break;

    case 5:                                 // Get record termination result
         if (-1 == checkResultCode()) return -1; 
         isThereAnOutboundMessage = 0;
         break;

    case 6:                                 // Get stop media result, send stop media on play
         if (-1 == checkResultCode()) return -1; 
         log("TEST wait 2 seconds and send stop media on play\n"); 
         mmsSleep(2);
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::OPERATION_ID], playOpID);
         break;

    case 7:                                 // Get play termination result
         if (-1 == checkResultCode()) return -1; 
         isThereAnOutboundMessage = 0;
         break;

    case 8:                                 // Get stop media result and send a new play
         if (-1 == checkResultCode()) return -1;
         isPassthruProvisional = TRUE;      // We want to see play provisional
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE];                                 
         outxml->putParameter(paramname, "format wav");
         fn = "sample_music.wav";
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], fn); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];
         outxml->putParameter(paramname, "maxtime 15000");    
         sprintf(getbuf(),"TEST sending play of %s\n", fn); log();               
         break;

    case 9:                                 // Check play provisional, send record
         if (-1 == checkResultCode(1)) return -1;    
         isPassthruProvisional = TRUE;      // We want record provisional
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];
         outxml->putParameter(paramname, "maxtime 10000");  
         log("TEST sending record\n");  
         break;

    case 10:                                // Check record provisional, send stop media all
         if (-1 == checkResultCode(1)) return -1; 
         log("TEST wait 3 seconds and send stop media on all operations\n"); 
         mmsSleep(3);
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID);
         break;

    case 11:                                // Get stop media result and send session disco
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         printf("TEST sending session disconnect\n");              
         break;

    case 12:                                 // Get session disco result; disco from media server
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

  if (isThereAnOutboundMessage)
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


 
int conferenceStopMediaOnUtilitySession(const unsigned int serverconnect) 
{ 
  // (1) Establish a 2-party conference. Play to conference. Save conx ID of utility
  // session and stop media on that play. Utility session should be removed
  // from conference. 

  static int iters, isserverconnect, recordConxID, conferenceID, conxIds[4];
  int   n = 0, result=1, thisConxID=0, waitsecs=0, isThereAnOutboundMessage=1;  
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  if (iters++ == 0) 
  { log("\nbegin conference stop media on utility session test\n\n");
    setGlobalsF();
  }

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
         log("TEST sending session/conference connect 1\n");
         break; 

    case 2:                                 // Connect party 2
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1002);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         outxml->putParameter(confidparamname, conferenceID); 
         log("TEST sending session/conference connect 2\n");
         break;  

    case 3:                                 // Record conference
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(recordcommand);
         outxml->putParameter(confidparamname, conferenceID); 
         outxml->putParameter(filenameparamname,"mmstestrecording.vox"); 
         outxml->putParameter(termcondparamname, "silence 30000");   // 30 seconds silence
         isPassthruProvisional = TRUE;      // Ask for provisional result from record
         log("TEST sending record conference\n");    
         break;

    case 4:                                 // Provisional result for record
         isPassthruProvisional = FALSE;
         checkResultCode(1); 
         if (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;  
         log("TEST got record drone conx ID -- wait 3 secs...\n"); 
         sprintf(getbuf(),"TEST found record drone conx ID %d -- wait 3 secs...\n", conxIds[2]); log(); 
         mmsSleep(3);

         outxml->putMessageID(stopmediacommand);
         outxml->putParameter(conxidparamname, conxIds[2]); 
         log("TEST sending stop media on conf record conx ID\n");                        
         break; 
                                             
    case 5:                                  
         showResultCode();                
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIds[1]);
         log("TEST sending disco on party 1\n");            
         break;

    case 6:                                  
         showResultCode();                
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIds[0]);
         log("TEST sending disco on party 0\n");            
         break;

    case 7:                                 // Disco server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

  if  (isThereAnOutboundMessage)                             
  {
      outxml->putParameter(transidparamname, ++transID);
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


 
int conferenceDeleteUtilitySession(const unsigned int serverconnect) 
{ 
  // (1) Establish a 2-party conference. Play to conference. Save conx ID of utility
  // session and disco that connection. Utility session should be removed
  // from conference. 

  static int iters, isserverconnect, recordConxID, conferenceID, conxIds[4];
  int   n = 0, result=1, thisConxID=0, waitsecs=0, isThereAnOutboundMessage=1;  
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  if (iters++ == 0) 
  { log("\nbegin conference delete utility session test\n\n");
    setGlobalsF();
  }

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
         log("TEST sending session/conference connect 1\n");
         break; 

    case 2:                                 // Connect party 2
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1002);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         outxml->putParameter(confidparamname, conferenceID); 
         log("TEST sending session/conference connect 2\n");
         break;  

    case 3:                                 // Record conference
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(recordcommand);
         outxml->putParameter(confidparamname, conferenceID); 
         outxml->putParameter(filenameparamname,"mmstestrecording.vox"); 
         outxml->putParameter(termcondparamname, "silence 30000");   // 30 seconds silence
         isPassthruProvisional = TRUE;      // Ask for provisional result from record
         log("TEST sending record conference\n");    
         break;

    case 4:                                 // Provisional result for record
         isPassthruProvisional = FALSE;
         checkResultCode(1); 
         if (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;  
         log("TEST got record drone conx ID -- wait 3 secs...\n"); 
         sprintf(getbuf(),"TEST found record drone conx ID %d -- wait 2 secs...\n", conxIds[2]); log(); 
         mmsSleep(2);

         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIds[2]); 
         log("TEST sending disconnect on conf record conx ID\n");                        
         break; 
                                             
    case 5:                                  
         showResultCode();                
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIds[1]);
         log("TEST sending disco on party 1\n");            
         break;

    case 6:                                  
         showResultCode();                
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIds[0]);
         log("TEST sending disco on party 0\n");            
         break;

    case 7:                                 // Disco server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

  if  (isThereAnOutboundMessage)                             
  {
      outxml->putParameter(transidparamname, ++transID);
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



int conferenceEmptyPlayAndRecord(const unsigned int serverconnect) 
{ 
  // (1) Establish a 2-party conference. Play to conference. Record conference.
  // Remove the 2 real parties from conference. Both utility sessions should be  
  // removed.

  static int iters, isserverconnect, recordConxID, conferenceID, conxIds[4];
  int   n = 0, result=1, thisConxID=0, waitsecs=0, isThereAnOutboundMessage=1;  
  if   (outxml) delete outxml;
  outxml = new MmsAppMessage;

  if (iters++ == 0) 
  { log("\nbegin conference empty with two utility sessions test\n\n");
    setGlobalsF();
  }

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
         log("TEST sending session/conference connect 1\n");
         break; 

    case 2:                                 // Connect party 2
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 1002);
         outxml->putParameter(ipparamname, "127.0.0.1"); 
         outxml->putParameter(confidparamname, conferenceID); 
         log("TEST sending session/conference connect 2\n");
         break;  

    case 3:                                 // Record conference
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(recordcommand);
         outxml->putParameter(confidparamname, conferenceID); 
         outxml->putParameter(filenameparamname,"mmstestrecording.vox"); 
         outxml->putParameter(termcondparamname, "silence 30000");   // 30 seconds silence
         isPassthruProvisional = TRUE;      // Ask for provisional result from record
         log("TEST sending record conference\n");    
         break;

    case 4:                                 // Provisional result for record
         checkResultCode(1); 
         if (-1 == (conxIds[2] = getExpectedConnectionID())) return -1;  
         sprintf(getbuf(),"TEST found record drone conx ID %d -- wait 2 secs...\n", conxIds[2]); log(); 
         mmsSleep(2);

         outxml->putMessageID(playcommand); // Do a play
         outxml->putParameter(confidparamname, conferenceID); 
         outxml->putParameter(filenameparamname,"sample_music.wav");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE], "format wav"); 
         outxml->putParameter(termcondparamname, "maxtime 10000");    
         isPassthruProvisional = TRUE;      // Ask for provisional result from play
         log("TEST sending play to conference\n");    
         break;

    case 5:                                 // Provisional result for play
         isPassthruProvisional = FALSE;
         checkResultCode(1); 
         if (-1 == (conxIds[3] = getExpectedConnectionID())) return -1;  
         sprintf(getbuf(),"TEST found play drone conx ID %d -- wait 2 secs...\n", conxIds[2]); log(); 
         mmsSleep(2);
              
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIds[1]);
         log("TEST sending disco on party 1\n");            
         break;

    case 6:                                  
         showResultCode();                
         outxml->putMessageID(disconnectcommand);
         outxml->putParameter(conxidparamname, conxIds[0]);
         log("TEST sending disco on party 0\n");            
         break;

    case 7:                                 // Disco server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
  }

  if  (isThereAnOutboundMessage)                             
  {
      outxml->putParameter(transidparamname, ++transID);
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



int halfConnectLoop(const unsigned int serverconnect) // (~)
{
  // Emulate very simple scenario in negative test in which the only media is
  // a reserve connection (half-connect) followed immediately by a disco.
  // Intended to be run with multiple instances of the test running.

  static int digitsiters, isserverconnect, port=1000;
  static int conxIds[2],  conferenceID, testIterations;
  static int NUMITERATIONS, DISCODELAY;

  if (testIterations == 0)
  {
      sprintf(getbuf(),"\nBegin half connect loop on %s\n", queueName); log();
      setGlobalsF();
                                            // If config parameters are for this test ...
      if (config->iparamA == ID_HALFCONNECTLOOP)
      {
          NUMITERATIONS = config->iparamB;  // ... get parameters from config
          DISCODELAY    = config->iparamC; 
      }
      else
      {                                     // Otherwise use default parameters
          NUMITERATIONS = HCL_NUMITERATIONS;
          DISCODELAY    = HCL_DISCODELAY; 
      }

      sprintf(getbuf(),"configured iterations: %d; disco delay ms: %d\n\n", NUMITERATIONS, DISCODELAY);
      log();
      mmsSleep(2);
      srand(time(0));
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
         sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
         if (!clientID) clientID = getMessageClientID();

         outxml->putMessageID(connectcommand);
         outxml->putParameter(portparamname, 0);
         outxml->putParameter(ipparamname, " "); 
         log("TEST sending party A half connect\n");
         break;
                                             
    case 2:                                // Send party A full disco
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[0] = getExpectedConnectionID())) return -1; 
         if (DISCODELAY > 0) 
             mmsSleep(MMS_N_MS(DISCODELAY));  
           
         outxml->putMessageID(disconnectcommand); 
         outxml->putParameter(conxidparamname, conxIds[0]); 
         log("TEST sending party A full disconnect\n");
         break;
                                            // Get party A disco result
    case 3:                                 // Disco from server 
         if  (-1 == checkResultCode()) return -1;

         if  (testIterations < NUMITERATIONS) 
         {
              sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
              outxml->putMessageID(connectcommand);
              outxml->putParameter(portparamname, 0);
              outxml->putParameter(ipparamname, " "); 
              log("TEST sending party A half connect\n");
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

       printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result; 
}

