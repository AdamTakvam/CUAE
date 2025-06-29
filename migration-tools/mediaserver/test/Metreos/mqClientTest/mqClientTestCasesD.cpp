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
extern int  ipcType;
extern char queueName[32];
extern char machineName[128];
extern char returnIP[32];
extern char buf[192];



int confereeGetDigitsTest(const unsigned int serverconnect) // (J)
{ 
  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conferee get digits test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, thisConxID=0, result=1, waitsecs=0, isThereAnOutboundMessage=1; 
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

         if  (-1 == (conxIds[0] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         printf("TEST sending session/conference connect 2\n");
         break;

    case 3:
         if  (-1 == checkResultCode()) return -1;
         if  (-1== (conxIds[1] = getExpectedConnectionID())) return -1;
         thisConxID=conxIds[1];  showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], thisConxID); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 10000");   // 10 seconds

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxdigits 4"); 

         //txml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
         //     "digit #"); 

         printf("TEST sending get digits on conferee\n");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;

         waitsecs = 5;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"1123");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         isThereAnOutboundMessage = false;
         mmsSleep(1);
           
         break;

    case 4:                                 // Terminations for both sendDigits
         checkResultCode();                 // and getDigits are coming thru, 
         isThereAnOutboundMessage = false;  // so wait for both
         break;
 
    case 5:                                 // Abandon connections & disco server
         if (-1 == checkResultCode()) printf("TEST FAILED\n"); else printf("TEST success\n");
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
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



int playAndGetDigitsTest(const unsigned int serverconnect) // (Z)
{ 
  // A bug was observed where if a play id issued with a digit termination,
  // and the digits are subsequently gathered, and the play terminates on
  // pound sign, and three or more digits are entered, and the first two
  // digits are the same, the first digit is not gathered. For example
  // press 112#, get back 12#, but press 123#, get back 123#.

  // Update: this test passes, but voicetunnel demonstrably skips digits.
  // A contributing factor is that speed of typing digits causes digits
  // to be lost -- this is why with "1123", "123" was observed, since 
  // typing the same digit is usually done faster than otherwise.

  // Update: removing the sleep from behind the first senddigits does not
  // elicit the problem, So speed is not the issue. I'm thinking that 
  // in deviceVoice.clearDigitBuffer, the local buffer gets cleared,
  // then a context switch happens (due to play complete event) before
  // the device digit buffer is cleared via dx_clrdigbuf, and a digit
  // arrives in the interim and is added to the buffer, and finally the
  // dx_clrdigbuf is executed, resulting in the device digit buffer being
  // out of sync with the user digit buffer. Let's add a digit buffer mutex,
  // set that mutex in clearDigitBuffer, and block on that mutex on a
  // receive digits action, so that we cannot receive a digit while the
  // buffer clear operation is in progress.

  // ANOTHER TEST: to test back to back commands hitting media server
  // before it may have a chance to complete each command, comment out
  // the inter-command sleeps in this test

  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin play and get digits test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, thisConxID=0, result=1, waitsecs=0, isThereAnOutboundMessage=1; 
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
         log("TEST sending session/conference connect 1\n");
         break;    

    case 2:                                 // Do a play  
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();
                                            // Play 15-second announcement
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::APP_NAME], TEST_APP_NAME);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::LOCALE],   TEST_LOCALE_DIR);
                
         // to test a very long tts string uncomment these lines, except for the two ruler lines,
         // and comment out the real file names:

          outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], 
         //          1         2         3         4         5         6         7         8
         // 12345678901234567890123456789012345678901234567890123456789012345678901234567890
           "now is the time, for all good men to come to the aid of their party. now is the "  
           "time for all good men to come to the aid of their party. now is the time for all"  
           " good men to come to the aid of their party. now is the time for all good men to"  
           " come to the aid of their party. now is the time for all good men to come to the"  
           " aid of their party. now is the time, for all good men, to come to, the aid of, " 
           "their party. now is the time for all good men to come to the aid of their party."                 
          );
          
         // to test a very long tts string uncomment the above lines, except for the two ruler lines,
         // and comment out the real file names in the two lines following:

         //outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         //outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox"); 
                                          
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1"); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         log("TEST sending play with maxdigits 1 termination\n"); 
         isPassthruProvisional = TRUE;
         break;

    case 3: // Get play provisional return and send all digits
        
         if  (-1 == checkResultCode(1)) return -1; 
         isPassthruProvisional = FALSE;
         interCommandSleepMs = 0; 

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"1");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         // printf("\n\n%s\n\n", outxml->getNarrowMessage());
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         log("TEST sending sendDigits 1\n"); 

         delete outxml;  // mmsSleep(MMS_N_MS(100 * threadPoolSize));
         outxml = new MmsAppMessage;                          
         // mmsSleep(1);                 
       
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"1");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         // printf("\n\n%s\n\n", outxml->getNarrowMessage());
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         log("TEST sending sendDigits 1\n"); 

         delete outxml;
         outxml = new MmsAppMessage;                          
         // mmsSleep(1);         

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"2");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         // printf("\n\n%s\n\n", outxml->getNarrowMessage());
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         log("TEST sending sendDigits 2\n"); 

         delete outxml;
         outxml = new MmsAppMessage;                          
         // mmsSleep(1); 

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"3");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         // printf("\n\n%s\n\n", outxml->getNarrowMessage());
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         log("TEST sending sendDigits 3\n"); 

         delete outxml;
         outxml = new MmsAppMessage;                          
         // mmsSleep(1); 

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         //outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         //outxml->terminateReturnMessage(clientID);
         // printf("\n\n%s\n\n", outxml->getNarrowMessage());
         //putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         log("TEST sending sendDigits #\n"); 
         interCommandSleepMs = USE_DEFAULT_INTERCOMMAND_DELAY;
   
         break;

    case 4:                                 // Check play return, send GetDigits 
         if  (-1 == checkResultCode(1)) return -1; 

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 

         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 

         outxml->putParameter(termcondname, "maxtime 1000");    
         outxml->putParameter(termcondname, "digit #"); 

         log("TEST sending get digits with digit # maxtime 1000 termination\n");          
         break;

    case 5:                                  
         checkResultCode(1);                 // Get sendDigits return 1
         isThereAnOutboundMessage = FALSE;         
         break;

    case 6:                                  
         checkResultCode(1);                 // Get sendDigits return 1
         isThereAnOutboundMessage = FALSE;  
         break;

    case 7:                                  
         checkResultCode(1);                 // Get sendDigits return 2
         isThereAnOutboundMessage = FALSE;  
         break;

    case 8:                                  
         checkResultCode(1);                 // Get sendDigits return 3
         isThereAnOutboundMessage = FALSE;  
         break;

    case 9:                                  
         checkResultCode(1);                 // Get sendDigits return #
         isThereAnOutboundMessage = FALSE;  
         break;  
                                     
 
    case 10:                                // Get GetDigits return, disco server
         checkResultCode(1); 
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
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



int digitPatternTestC(const unsigned int serverconnect) 
{ 
  // Tests getdogots digit pattern termination condition concurrent with HMP digit 
  // termination. A bug existed wherein if both a digitpattern term and a digit
  // term were set on the getdigits, and the terminating digit already exists in
  // the digit buffer, the termination was not recognized.

  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin digit pattern test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, thisConxID=0, result=1, waitsecs=0, isThereAnOutboundMessage=1; 
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
         log("TEST sending session/conference connect 1\n");
         break;    

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();
                                           // Play 15-second announcement  
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
                                            // ... with maxdigits 1 termination 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");                                            

         isPassthruProvisional = TRUE;      // Ask to see provisional result  
         log("TEST sending play\n");              
         break;
                                            // play provisional received
    case 3:                                 // Send play to session 1 on session 2
         isPassthruProvisional = FALSE;
         if  (-1 == checkResultCode(1)) return -1;
         log("TEST play provisional received\n");   

         mmsSleep(3);                       // Send digit # after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         log("TEST sending sendDigits #\n");   
         mmsSleep(1);
         break;

    case 4:                                 // Termination for sendDigits 1
         checkResultCode();                  
         isThereAnOutboundMessage = false;   
         break;

    case 5:                                 // Termination for record
         checkResultCode();                 // Send GetDigits             
                                             
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 3000");   // 3 seconds

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digitpattern **"); 

         log("TEST sending get digits\n");  
         break;
                                            // Termination for getDigits 
    case 6:                                 // Abandon connections & disco server
         if (-1 == checkResultCode()) log("\nTEST FAILED\n\n"); else log("TEST success\n");
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
  }


  if  (isThereAnOutboundMessage)
  {
      outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
      outxml->terminateReturnMessage(clientID);

      sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
      putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}



int digitPatternTestX(const unsigned int serverconnect) 
{ 
  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin digit pattern test A\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, thisConxID=0, result=1, waitsecs=0, isThereAnOutboundMessage=1; 
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

         if  (-1 == (conxIds[0] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         printf("TEST sending session/conference connect 2\n");
         break;

    case 3:
         if  (-1 == checkResultCode()) return -1;
         if  (-1== (conxIds[1] = getExpectedConnectionID())) return -1;
         thisConxID=conxIds[1];  showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], thisConxID); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 10000");   // 10 seconds

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digitpattern ###"); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit d"); 

         printf("TEST sending get digits on conferee\n");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());


         delete outxml;
         outxml = new MmsAppMessage;

         waitsecs = 5;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"##0#");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());


         delete outxml;
         outxml = new MmsAppMessage;

         waitsecs = 1;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());


         delete outxml;
         outxml = new MmsAppMessage;

         waitsecs = 1;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         isThereAnOutboundMessage = false;
         mmsSleep(1);
         break;

    case 4:                                 // Termination for getDigits
         checkResultCode();                  
         isThereAnOutboundMessage = false;   
         break;

    case 5:                                 // Termination for sendDigits 1
         checkResultCode();                   
         isThereAnOutboundMessage = false;   
         break;


    case 6:                                 // Termination for sendDigits 2
         checkResultCode();                   
         isThereAnOutboundMessage = false;  
         break;

                                            // Termination for sendDigits 3
    case 7:                                 // Abandon connections & disco server
         if (-1 == checkResultCode()) printf("TEST FAILED\n"); else printf("TEST success\n");
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
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



int digitPatternTestB(const unsigned int serverconnect) 
{ 
  // Preloads digit buffer with pattern required to fire a subsequent
  // GetDigits with DIGITPATTERN termination. Test succeeds if the 
  // GetDigits terminates immediately.

  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin digit pattern test B\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, thisConxID=0, result=1, waitsecs=0, isThereAnOutboundMessage=1; 
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

    case 2:                                 // Do a play to load digit buffer
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
                                            // Play 15-second announcement
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");                                           
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 6"); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         log("TEST sending play with maxdigits 6 term\n");              
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send sendDigits after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"##0###");
         log("TEST sending sendDigits ##0###\n");              
         break;

   case 3:                                  
         checkResultCode();                 // Get sendDigits return
         isThereAnOutboundMessage = FALSE;  
         break;
                                            // Get play return
   case 4:                                  // Send GetDigits w ### pattern termination
         if  (-1 == checkResultCode()) return -1; 

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 20000");    

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digitpattern ###"); 

         log("TEST sending get digits with triple-pound termination\n");  
         isPassthruProvisional = TRUE;      // Route provisional thru state machine
         break;

    case 5:                                 // Both getDigits provisional return and
         checkResultCode(1);                // getDigits termination are coming thru, 
         isThereAnOutboundMessage = FALSE;  // so wait for both
         isPassthruProvisional = FALSE; 
         break;
 
    case 6:                                 // Abandon connection & disco server
         checkResultCode(); 
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
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



int digitPatternTest(const unsigned int serverconnect) // (K)
{ 
  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin digit pattern test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, thisConxID=0, result=1, waitsecs=0, isThereAnOutboundMessage=1; 
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
         if  (-1 == (conxIds[0] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 2\n");
         break;

    case 3:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[1] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();
                                           // Play 15-second announcement
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
                                            // ... with maxdigits 1 termination 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");                                            

         isPassthruProvisional = TRUE;      // Ask to see provisional result  
         log("TEST sending play\n");              
         break;
                                            // play provisional received
    case 4:                                 // Send play to session 1 on session 2
         isPassthruProvisional = FALSE;
         if  (-1 == checkResultCode(1)) return -1;
         printf("TEST play provisional received\n");   

         mmsSleep(3);                       // Send sendDigits after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         log("TEST sending sendDigits #\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());


         delete outxml;
         outxml = new MmsAppMessage;

         //log("TEST waiting 1 seconds ...\n");                            
         //mmsSleep(1);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         isThereAnOutboundMessage = false;
         mmsSleep(1);
         break;

    case 5:                                 // Termination for sendDigits 1
         checkResultCode();                  
         isThereAnOutboundMessage = false;   
         break;

    case 6:                                 // Termination for sendDigits 2
         checkResultCode();                 // Send GetDigits             
                                            // Send third pound
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 5000");   // 5 seconds

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digitpattern ##"); 

         log("TEST sending get digits on conferee\n");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());


         delete outxml;
         outxml = new MmsAppMessage;

         log("TEST waiting 1 seconds ...\n");                            
         mmsSleep(1);                       // Send sendDigits # after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  
         break;

    case 7:                                 // Termination for getDigits
         checkResultCode();                  
         isThereAnOutboundMessage = false;   
         break;
                                            // Termination for sendDigits  
    case 8:                                 // Abandon connections & disco server
         if (-1 == checkResultCode()) log("TEST FAILED\n"); else log("TEST success\n");
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
  }


  if  (isThereAnOutboundMessage)
  {
      outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
      outxml->terminateReturnMessage(clientID);

      sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
      putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}




int digitListTest(const unsigned int serverconnect)
{
  // connects to server, connects a session, plays an announcement with
  // digitlist termination, sends some digits during playback, disconnects 
  // session, disconnects server

  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin announcment/send digits test\n\n");
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
         showReturnedPortAndIP();
                                            // Play 15-second announcement
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
                                            // ... with digitlist termination 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         // outxml->putParameter(termcondname, "maxdigits 1"); 
         outxml->putParameter(termcondname, "digitlist #*");
                                            // ... and 6-second max
         outxml->putParameter(termcondname, "maxtime 6000");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending play with digitlist term\n");              
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send sendDigits after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"*9");
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



 
int conferenceRecordPromptAndDisco(const unsigned int serverconnect)  // 'L'
{ 
  // Establish 2-party conference. Record the conference to named file. 
  // Play a prompt to party 2. Disco party 2 during prompt.
  // Disco conferees. Cancel the record session.

  static int iters, isserverconnect, recordConxID, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conference record test C\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result=1, thisConxID=0, waitsecs=0, isThereAnOutboundMessage=1;  
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

    case 2:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         thisConxID   = conxIds[0];
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

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
                    
         delete outxml;
         outxml = new MmsAppMessage;
         waitsecs = 2;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                                                                                                                 
         recordConxID = 2;   
                                            // Play audio to party 1
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         printf("TEST sending play to party 2\n");  

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
                    
         delete outxml;
         outxml = new MmsAppMessage;

         waitsecs = 1;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                // Send disco after n seconds

             
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], thisConxID);
         printf("TEST sending conference/session disconnect conx %d\n",  thisConxID);              
         break;

    case 3:                                 // Termination for play
         checkResultCode();                  
         isThereAnOutboundMessage = false;   
         break; 
                                            // Termination for disco
    //case 4:                               // Disco record session
    //     showResultCode();                
    //     outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
    //     outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], recordConxID);
    //     printf("TEST record session disco conx %d (*** should fail ***)\n", recordConxID);              
    //     break;

    case 4:                                 // Disco server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
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



int twoPlaysWithGetDigitPlusDelay(const unsigned int serverconnect)
{
  static int iters, isserverconnect, conxID;
  if  (iters++ == 0) printf("\nBegin announcment/send digits test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result = 1, waitsecs=0, isThereAnOutboundMessage = 1;
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
                                            // Play 15-second announcement
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"goodbye.vox");
                                            // ... with digit termination conds
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");
                                            // ... and 6-second max
         // outxml->putParameter(termcondname, "maxtime 6000");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending play\n");              
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send sendDigits after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         printf("TEST sending sendDigits\n");              
         break;
                                            
    case 3:                                 // Get result of play or send digits
         if  (-1 == checkResultCode()) return -1;
         isThereAnOutboundMessage = 0;
         break;

    case 4:
         if  (-1 == checkResultCode()) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 10000");   // 10 seconds

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxdigits 4"); 

         //outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
         //       "digitdelay 2000"); 

         printf("TEST sending get digits\n");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;

         waitsecs = 2;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"1234");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         isThereAnOutboundMessage = false;
         mmsSleep(1);
           
         break;


    case 5:                                 
         if  (-1 == checkResultCode()) return -1;
         isThereAnOutboundMessage = 0;
         break;

    case 6:                                 
         if  (-1 == checkResultCode()) return -1;
                                            // Play 15-second announcement
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
                                            // ... with digit termination conds
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");
         //                                   // ... and 10-second max
         //outxml->putParameter(termcondname, "maxtime 10000");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("TEST sending play\n");              
         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(2);                       // Send sendDigits after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         printf("TEST sending sendDigits\n");              
         break;
                                            
    case 7:                                 // Get result of play or send digits
         checkResultCode();
         n = getExpectedConnectionID();
         isThereAnOutboundMessage = 0;
         break;

    case 8:                                 // Get result of play or send digits
         checkResultCode();
         n = getExpectedConnectionID();
                                            // Send an IP disconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], n); 
         printf("TEST sending session disconnect\n");              
         break;

    case 9:                                 // Disconnect from media server
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



int cancelDigitPattern(const unsigned int serverconnect) // 'N'
{ 
  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin cancel digit pattern test\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, thisConxID=0, result=1, waitsecs=0, isThereAnOutboundMessage=1; 
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

         if  (-1 == (conxIds[0] = getExpectedConnectionID()))   return -1;
         showReturnedPortAndIP();

         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID);

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS], "127.0.0.1");
         log("TEST sending session/conference connect 2\n");
         break;

    case 3:
         if  (-1 == checkResultCode()) return -1;
         if  (-1== (conxIds[1] = getExpectedConnectionID())) return -1;
         thisConxID=conxIds[1];  showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], thisConxID); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 10000");   // 10 seconds

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digitpattern ###"); 

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit d"); 

         log("TEST sending get digits on conferee\n");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());


         delete outxml;
         outxml = new MmsAppMessage;

         log("TEST waiting 5 seconds ...\n");                            
         mmsSleep(5);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"##0#");
         printf("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());


         delete outxml;
         outxml = new MmsAppMessage;

         log("TEST waiting 1 seconds ...\n");                            
         mmsSleep(1);                // Send sendDigits after n seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS],"#");
         log("TEST sending sendDigits\n");   

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
         

         delete outxml;
         outxml = new MmsAppMessage;
                                            
         mmsSleep(3);                       // Send stop after 3 seconds

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], thisConxID); 
         log("TEST sending stopMediaOperation\n");                         
         break;

    case 4:                                 // Termination for getDigits
         checkResultCode();                  
         isThereAnOutboundMessage = 0;   
         break;

    case 5:                                 // Termination for sendDigits 1
         checkResultCode();                   
         isThereAnOutboundMessage = 0;   
         break;


    case 6:                                 // Termination for sendDigits 2
         checkResultCode();                   
         isThereAnOutboundMessage = 0;  
         break;

                                            // Termination for sendDigits 3
    case 7:                                 // Abandon connections & disco server
         if (-1 == checkResultCode()) printf("TEST FAILED\n"); else printf("TEST success\n");
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);            
         result = -1;
  }


  if  (isThereAnOutboundMessage)
  {
      outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
      outxml->terminateReturnMessage(clientID);

      sprintf(getbuf(), "\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
      putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}

    

int monitorCallState(const unsigned int serverconnect)
{   
  // This test connects two sessions, sends a monitorCallState command on the
  // first session, then sends a play on the second session, playing to the
  // first session. Termination is either via monitorCallState command timeout,
  // meaning that the call state was not observed within the specified interval,
  // or an OK return, in which case the call state transition was observed,

  // This test should be run four times, with silence less than, and greater 
  // than, the wait time; and the same for nonsilence
  const int maxWaitTimeMs = 10000;
  char* desiredSilenceMsParam = "silence 3000";

  static int iters, isserverconnect, conxIDs[4];
  if  (iters++ == 0) printf("\nBegin call state test\n\n");
  char* p = NULL, *paramname = NULL, *fn = NULL;
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
         printf("TEST sending session 1 connect\n");
         break;

    case 2:                                 // Send a second IP connect
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIDs[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         
         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);

         // Use local IP and port returned from the first session connect
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], returnPort);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], returnIP); 
         printf("TEST sending session 2 connect to session 1 local IP/port %s %d\n",returnIP, returnPort);
         break;

    case 3:                                 // Send monitorCallState on session 1
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIDs[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         isPassthruProvisional = TRUE;      // Ask to see provisional result  

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_MONITOR_CALL_STATE]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIDs[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::COMMAND_TIMEOUT], maxWaitTimeMs); 
         paramname = MmsAppMessageX::paramnames[MmsAppMessageX::CALL_STATE];         
         outxml->putParameter(paramname, desiredSilenceMsParam);
         
         printf("TEST sending monitorCallState\n", fn);              
         break;
                                            // monitorCallState provisional received
    case 4:                                 // Send play to session 1 on session 2
         isPassthruProvisional = FALSE;
         if  (-1 == checkResultCode(1)) return -1;
         printf("TEST monitorCallState provisional received\n");   
                                     
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIDs[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox");                
         printf("TEST sending session 2 play to session 1\n", fn);              
         break;

    case 5:                                 // monitorCallState final received
         checkResultCode();
         isThereAnOutboundMessage = 0;   
         break;

    case 6:                                 // Play termination received
         checkResultCode();                 // Disco all sessions and from server 
         sendServerDisconnect(outxml);            
         result = -1;
         break;
  }

  if (isThereAnOutboundMessage)
  {                                           
      outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
      outxml->terminateReturnMessage(clientID);

      printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                              // Send the outbound message
      putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

      if  (appxml) delete appxml;
      appxml = NULL;
  }

  state++;
  return result;
}  


int confereeStopMediaAndReplay(const unsigned int serverconnect)
{
  // This test sequence establishes a conference, plays a file to session 0 
  // conferee, waits 3 seconds, stops media, plays another file, 

  static int iters, isserverconnect, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin conferee stop media/replay test A\n\n");
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
                                            
         mmsSleep(3);                       // Send stop media after 3 seconds         

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_STOPMEDIA]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         printf("TEST sending stopMediaOperation\n");          
         break;

    case 5: 
         checkResultCode();
         isThereAnOutboundMessage = FALSE;
         break;

    case 6: 
         checkResultCode();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"stoprec.vox");
         printf("TEST sending another play\n");
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

       printf("\n\n%s\n\n", outxml->getNarrowMessage());
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  state++;
  return result;
}


   
int remoteAgentTestA(const unsigned int serverconnect) 
{ 
  // 

  static int iters, isserverconnect, recordConxID, conferenceID, conxIds[4];
  if  (iters++ == 0) printf("\nBegin remote agent test A\n\n");
  char* p = NULL, *termcondname = NULL;
  int   n = 0, result=1, thisConxID=0, waitsecs=0, isThereAnOutboundMessage=1;  
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

    case 2:                                 // Record conference
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (conxIds[0]   = getExpectedConnectionID())) return -1;
         if  (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         thisConxID   = conxIds[0];
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

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
                    
         delete outxml;
         outxml = new MmsAppMessage;
         waitsecs = 2;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                                                                                                                 
         recordConxID = 2;   
                                            // Play audio to party 1
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], thisConxID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME],"thankyou.vox"); 
         printf("TEST sending play to party 2\n");  

         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
         outxml->terminateReturnMessage(clientID);
         printf("\n\n%s\n\n", outxml->getNarrowMessage());

         putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
                    
         delete outxml;
         outxml = new MmsAppMessage;

         waitsecs = 1;                  
         printf("TEST waiting %d seconds ...\n", waitsecs);                            
         mmsSleep(waitsecs);                // Send disco after n seconds

             
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], thisConxID);
         printf("TEST sending conference/session disconnect conx %d\n",  thisConxID);              
         break;

    case 3:                                 // Termination for play
         checkResultCode();                  
         isThereAnOutboundMessage = false;   
         break; 
                                            // Termination for disco
    case 4:                                 // Disco record session
         showResultCode();                
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], recordConxID);
         printf("TEST record session disco conx %d (*** should fail ***)\n", recordConxID);              
         break;

    case 5:                                 // Disco server
         showResultCode(); 
         if  (serverconnect & SERVER_DISCO)
              sendServerDisconnect(outxml);            
         result = -1;
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
   



