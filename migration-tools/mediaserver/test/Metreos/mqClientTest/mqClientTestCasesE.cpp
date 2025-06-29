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
extern int  interCommandSleepMs;  
extern int  isShowHeartbeatContent; 
extern int  heartbeatInterval;
extern int  isNoShowInboundXml;
extern int  isShowProvisionalContent;
extern int  isPassthruProvisional; 
extern char queueName[32];
extern char machineName[128];
extern char returnIP[32];

#define ID_TESTDIGITSOVERMANY 53441         // iparamA
#define DOMC_NUMITERATIONS    2             // iparamB
#define DOMC_NUMCONNECTS      3             // iparamC
#define DOMC_NUMDIGITS        4             // iparamD
#define DOMC_MAX_CONNECTIONS 64 

#define ID_VOICETUNNELLOOP    53442         // iparamA
#define VTL_NUMITERATIONS     2             // iparamB
#define VTL_NUMDIGITS         10            // iparamC

#define ID_SCHED_CONFERENCE   53443         // iparamA
#define SCS_NUMITERATIONS     1             // iparamB


const char* digits[12] = {"1", "2", "3", "4", "5", "6", "7","8", "9", "0", "#", "*"};

int randomNumberInRange(int min, int max)
{
  double f01 = ((double)rand() / (double)(RAND_MAX+1));
  double mf  = f01 * (max - min);
  int result = (int)mf + min;
  return result;
}



int voiceTunnelLoop(const unsigned int serverconnect)
{
  static int digitsiters, isserverconnect, port=1000;
  static int conxIds[2],  conferenceID, testIterations;
  static int NUMITERATIONS, NUMDIGITS, NUMCONNECTS;

  #define PLAYFILE_ENTER_ACCTCODE "thankyou.vox"
  #define PLAYFILE_ENTER_PIN      "thankyou.vox"
  #define PLAYFILE_ENTER_DN       "thankyou.vox"
  #define PLAYFILE_ENTERING_CONF  "thankyou.vox"

  #define DIGITS_ACCTCODE "1#"
  #define DIGITS_PIN      "2#"
  #define DIGITS_DIALED   "5124585554#"

  #define HALFCONNECTDELAYMSECS 250
  #define MINDIGDELAYMSECS 1000
  #define MAXDIGDELAYMSECS 4000
  #define MININTERDIGDELAYMSECS  250
  #define MAXINTERDIGDELAYMSECS 1000
  #define MINHOSTCONFCONNECTDELAYMSECS  500
  #define MAXHOSTCONFCONNECTDELAYMSECS 1500
  #define MINCONFDELAYMSECS 2000
  #define MAXCONFDELAYMSECS 5000

  
  if (testIterations == 0)
  {
      sprintf(getbuf(),"\nBegin voice tunnel loop on %s\n", queueName); log();
                                            // If config parameters are for this test ...
      if (config->iparamA == ID_VOICETUNNELLOOP)
      {
          NUMITERATIONS = config->iparamB;  // ... get parameters from config
          NUMDIGITS     = config->iparamC; 
      }
      else
      {                                     // Otherwise use default parameters
          NUMITERATIONS = VTL_NUMITERATIONS;
          NUMDIGITS     = VTL_NUMDIGITS; 
      }

      sprintf(getbuf(),"Configured iterations: %d; digits: %d\n\n", NUMITERATIONS, NUMDIGITS);
      log();
      mmsSleep(3);
      srand(time(0));
  }

  char *p = NULL, *termcondname;
  int   n = 0, result = 1, i = 0, isThereAnOutboundMessage = TRUE;
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
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         log("TEST sending party A half connect\n");
         break;

    case 2:                                 // Send full connect
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if (n = HALFCONNECTDELAYMSECS) mmsSleep(MMS_N_MS(HALFCONNECTDELAYMSECS));

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending party A final connect\n");              
         break;

    case 3:                                 // Play "Enter account code"
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTER_ACCTCODE);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1"); 
         log("TEST sending play (enter acct)\n");
         isPassthruProvisional = TRUE;      // Route provisional thru state machine
         break;

    case 4:                                  // Get play provisional, send sendDigits
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
                                            // We may want to loop this into n commands
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], DIGITS_ACCTCODE);  
         log("TEST sending acct digits\n");         
         isPassthruProvisional = FALSE; 
         break;

    case 5:                                  // Get sendDigits result
         if (-1 == checkResultCode(1)) return -1;
         isThereAnOutboundMessage = FALSE;
         break;  

    case 6:                                  // Get play termination, send getDigits
         if (-1 == checkResultCode(1)) return -1;   
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST sending acct getDigits\n");          
         isPassthruProvisional = FALSE; 
         break;                           
                                            // get getDigits termination
    case 7:                                 // Play "Enter PIN"
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTER_PIN);
                                            // max digits termination 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1"); 
         log("TEST sending play (enter PIN)\n");
         isPassthruProvisional = TRUE;       
         break;

    case 8:                                  // Get play provisional, send sendDigits
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);    
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
                                            // We may want to loop this into n commands
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], DIGITS_PIN);  
         log("TEST sending DN digits\n");         
         isPassthruProvisional = FALSE; 
         break;

    case 9:                                  // Get sendDigits result
         if (-1 == checkResultCode(1)) return -1;
         isThereAnOutboundMessage = FALSE;
         break; 

    case 10:                                 // Get play termination, send getDigits
         if (-1 == checkResultCode(1)) return -1;   

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST sending PIN getDigits\n");          
         isPassthruProvisional = FALSE; 
         break;              
                                            // Get getDigits termination
    case 11:                                // Play "Dial number" 
         if (-1 == checkResultCode()) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTER_DN);
                                            // max digits termination 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");
         log("TEST sending play (enter DN)\n");
         isPassthruProvisional = TRUE;       
         break;
  
    case 12:                                 // Get play provisional, send sendDigits
         if (-1 == checkResultCode(1)) return -1;  
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);   
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
                                            // We may want to loop this into n commands
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], DIGITS_DIALED);  
         log("TEST sending DN digits\n");         
         isPassthruProvisional = FALSE; 
         break;

    case 13:                                 // Get sendDigits result
         if (-1 == checkResultCode(1)) return -1;
         isThereAnOutboundMessage = FALSE;
         break; 
                                             
    case 14:                                 // Get play termination, send getDigits
         if (-1 == checkResultCode(1)) return -1;   

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST sending DN getDigits\n");          
         isPassthruProvisional = FALSE; 
         break;
  
    case 15:                                // Get getDigits result, play conference message 
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTERING_CONF);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");
         log("TEST sending play (enter conf)\n");
         isPassthruProvisional = FALSE;       
         break;
                                            // Get play result
    case 16:                                // Send party B half connect
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         log("TEST sending party B half connect\n");
         break;
                                            // Get party B half connect result
    case 17:                                // Send party A conference connect
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if (n = HALFCONNECTDELAYMSECS) mmsSleep(MMS_N_MS(HALFCONNECTDELAYMSECS));

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         log("TEST sending party A conference connect\n");                         
         break;
                                            // Get party A conf connect result
    case 18:                                // Send party B final connect to conf
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conferenceID = getExpectedConferenceID())) return -1;

         n = randomNumberInRange(MINHOSTCONFCONNECTDELAYMSECS, MAXHOSTCONFCONNECTDELAYMSECS);
         sprintf(getbuf(),"TEST pause %d ms\n", n); log();
         mmsSleep(MMS_N_MS(n));

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         log("TEST sending party B conference connect\n");         
         break;
                                            // Get party B conf connect result
    case 19:                                // Wait a bit and disco party B
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conferenceID = getExpectedConferenceID())) return -1;

         n = randomNumberInRange(MINCONFDELAYMSECS, MAXCONFDELAYMSECS);
         sprintf(getbuf(),"TEST pause %d ms\n", n); log();
         mmsSleep(MMS_N_MS(n));
                                           // Send an IP disconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         log("TEST sending party B full disconnect\n");
         break;
                                            // Get party B disco result
    case 20:                                // Send party A full disco
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         log("TEST sending party A full disconnect\n");
         break;
                                            // Get party A disco result
    case 21:                                // Disco from server 
         if  (-1 == checkResultCode()) return -1;

         if  (testIterations < NUMITERATIONS) 
         {
              sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
              outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
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
 


int voiceTunnelNoConference(const unsigned int serverconnect)
{
  static int digitsiters, isserverconnect, port=1000;
  static int conxIds[2],  testIterations;
  static int NUMITERATIONS, NUMDIGITS, NUMCONNECTS;

  #define PLAYFILE_ENTER_ACCTCODE "thankyou.vox"
  #define PLAYFILE_ENTER_PIN      "thankyou.vox"
  #define PLAYFILE_ENTER_DN       "thankyou.vox"
  #define PLAYFILE_ENTERING_CONF  "thankyou.vox"

  #define DIGITS_ACCTCODE "1#"
  #define DIGITS_PIN      "2#"
  #define DIGITS_DIALED   "5124585554#"

  #define HALFCONNECTDELAYMSECS 250
  #define MINDIGDELAYMSECS 1000
  #define MAXDIGDELAYMSECS 4000
  #define MININTERDIGDELAYMSECS  250
  #define MAXINTERDIGDELAYMSECS 1000
  #define MINHOSTCONFCONNECTDELAYMSECS  500
  #define MAXHOSTCONFCONNECTDELAYMSECS 1500
  #define MINCONFDELAYMSECS 2000
  #define MAXCONFDELAYMSECS 5000

  
  if (testIterations == 0)
  {
      sprintf(getbuf(),"\nBegin voice tunnel (no conf) on %s\n", queueName); log();
                                            // If config parameters are for this test ...
      if (config->iparamA == ID_VOICETUNNELLOOP)
      {
          NUMITERATIONS = config->iparamB;  // ... get parameters from config
          NUMDIGITS     = config->iparamC; 
      }
      else
      {                                     // Otherwise use default parameters
          NUMITERATIONS = VTL_NUMITERATIONS;
          NUMDIGITS     = VTL_NUMDIGITS; 
      }

      sprintf(getbuf(),"Configured iterations: %d; digits: %d\n\n", NUMITERATIONS, NUMDIGITS);
      log();
      mmsSleep(3);
      srand(time(0));
  }

  char *p = NULL, *termcondname;
  int   n = 0, result = 1, i = 0, isThereAnOutboundMessage = TRUE;
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
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         log("TEST sending party A half connect\n");
         break;

    case 2:                                 // Send full connect
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if (n = HALFCONNECTDELAYMSECS) mmsSleep(MMS_N_MS(HALFCONNECTDELAYMSECS));

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending party A final connect\n");              
         break;

    case 3:                                 // Play "Enter account code"
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTER_ACCTCODE);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1"); 
         log("TEST sending play (enter acct)\n");
         isPassthruProvisional = TRUE;      // Route provisional thru state machine
         break;

    case 4:                                  // Get play provisional, send sendDigits
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
                                            // We may want to loop this into n commands
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], DIGITS_ACCTCODE);  
         log("TEST sending acct digits\n");         
         isPassthruProvisional = FALSE; 
         break;

    case 5:                                  // Get sendDigits result
         if (-1 == checkResultCode(1)) return -1;
         isThereAnOutboundMessage = FALSE;
         break;  

    case 6:                                  // Get play termination, send getDigits
         if (-1 == checkResultCode(1)) return -1;   
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST sending acct getDigits\n");          
         isPassthruProvisional = FALSE; 
         break;                           
                                            // get getDigits termination
    case 7:                                 // Play "Enter PIN"
         if  (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTER_PIN);
                                            // max digits termination 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1"); 
         log("TEST sending play (enter PIN)\n");
         isPassthruProvisional = TRUE;       
         break;

    case 8:                                  // Get play provisional, send sendDigits
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);    
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
                                            // We may want to loop this into n commands
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], DIGITS_PIN);  
         log("TEST sending DN digits\n");         
         isPassthruProvisional = FALSE; 
         break;

    case 9:                                  // Get sendDigits result
         if (-1 == checkResultCode(1)) return -1;
         isThereAnOutboundMessage = FALSE;
         break; 

    case 10:                                 // Get play termination, send getDigits
         if (-1 == checkResultCode(1)) return -1;   

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST sending PIN getDigits\n");          
         isPassthruProvisional = FALSE; 
         break;              
                                            // Get getDigits termination
    case 11:                                // Play "Dial number" 
         if (-1 == checkResultCode()) return -1;

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTER_DN);
                                            // max digits termination 
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");
         log("TEST sending play (enter DN)\n");
         isPassthruProvisional = TRUE;       
         break;
  
    case 12:                                 // Get play provisional, send sendDigits
         if (-1 == checkResultCode(1)) return -1;  
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);   
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
                                            // We may want to loop this into n commands
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], DIGITS_DIALED);  
         log("TEST sending DN digits\n");         
         isPassthruProvisional = FALSE; 
         break;

    case 13:                                 // Get sendDigits result
         if (-1 == checkResultCode(1)) return -1;
         isThereAnOutboundMessage = FALSE;
         break; 
                                             
    case 14:                                 // Get play termination, send getDigits
         if (-1 == checkResultCode(1)) return -1;   

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST sending DN getDigits\n");          
         isPassthruProvisional = FALSE; 
         break;
  
    case 15:                                // Get getDigits result, play conference message 
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], PLAYFILE_ENTERING_CONF);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1");
         log("TEST sending play (enter conf)\n");
         isPassthruProvisional = FALSE;       
         break;
                                            // Get play result
    case 16:                                // Send party B half connect
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         log("TEST sending party B half connect\n");
         break;
                                            // Get party B half connect result
    case 17:                                // Send party B final connect
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;

         n = randomNumberInRange(MINHOSTCONFCONNECTDELAYMSECS, MAXHOSTCONFCONNECTDELAYMSECS);
         sprintf(getbuf(),"TEST pause %d ms\n", n); log();
         mmsSleep(MMS_N_MS(n));

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST sending party B final connect\n");         
         break;
                                            // Get party B conf connect result
    case 18:                                // Wait a bit and disco party B
         if (-1 == checkResultCode()) return -1;

         n = randomNumberInRange(MINCONFDELAYMSECS, MAXCONFDELAYMSECS);
         sprintf(getbuf(),"TEST pause %d ms\n", n); log();
         mmsSleep(MMS_N_MS(n));
                                           // Send an IP disconnect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         log("TEST sending party B full disconnect\n");
         break;
                                            // Get party B disco result
    case 19:                                // Send party A full disco
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         log("TEST sending party A full disconnect\n");
         break;
                                            // Get party A disco result
    case 20:                                // Disco from server 
         if  (-1 == checkResultCode()) return -1;

         if  (testIterations < NUMITERATIONS) 
         {
              sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
              outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
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



int digitsOverManyConnections(const unsigned int serverconnect)
{
  // Fast and furiously send a configurable number of digits to a configurable number 
  // of connections, a configurable number of times.

  // This is a different configuration than previous tests, in that state may
  // remain the same over a batch of commands, and state may revert to some prior
  // state in order to implement looping of the test.

  static int iters, digitsiters, isserverconnect, received, port=1000;
  static int conxIds[DOMC_MAX_CONNECTIONS];
  static int sendDigitResults, getDigitResults, testIterations;
  static int NUMITERATIONS, NUMDIGITS, NUMCONNECTS;

  if  (iters++ == 0) 
  {    log("\nBegin digits over many connections test\n");
                                            // If config parameters are for this test ...
       if (config->iparamA == ID_TESTDIGITSOVERMANY)
       {
           NUMITERATIONS = config->iparamB; // ... get parameters from config
           NUMCONNECTS   = config->iparamC; 
           NUMDIGITS     = config->iparamD; 
       }
       else
       {                                    // Otherwise use default parameters
           NUMITERATIONS = DOMC_NUMITERATIONS;
           NUMCONNECTS   = DOMC_NUMCONNECTS;  
           NUMDIGITS     = DOMC_NUMDIGITS; 
       }

       if (NUMCONNECTS > DOMC_MAX_CONNECTIONS)
       {   sprintf(getbuf(),"Too many connections requested: %d\n", NUMCONNECTS); log();
           NUMCONNECTS = DOMC_MAX_CONNECTIONS;
       }

       sprintf(getbuf(),"Configured iterations: %d; connections: %d; digits: %d\n\n",
               NUMITERATIONS, NUMCONNECTS, NUMDIGITS);
       log();
       mmsSleep(3);
  }

  char *p = NULL, *c;
  int   n = 0, result = 1, i = 0, j = 0, isThereAnOutboundMessage = 0; // , *m;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage; 


  switch(state)
  {
    case 0:
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1); 
         isNoShowInboundXml = 1;
         isThereAnOutboundMessage = 1; // in simpler tests we pause between each operation
         interCommandSleepMs = 0;      // so we can see what's happening. for this test we
         state = 1;                    // must disable that delay in order to keep things
         break;                        // moving at production speeds

    case 1:
                  
         sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
                      
         for(; i < NUMCONNECTS; i++)   // Send n connect commands    
         {             
             delete outxml;
             outxml = new MmsAppMessage;
              
             if (!clientID) clientID = getMessageClientID();

             outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
             outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port += 2);
             outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
             outxml->terminateReturnMessage(clientID);
             putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());

             sprintf(getbuf(),"TEST sending session %d connect\n", i+1); log();
         }

         isPassthruProvisional = TRUE;
         i = received = 0;
         state = 2;
         break;

    case 2:
         // State remains at 2 until we collect result for all n connect commands

         if ((4 == showResultCode()) || (1 > (n = getExpectedConnectionID())))          
         {   sprintf(getbuf(),"\n\n--- MMS-assigned connection ID zero at index %d\n\n", received); log();
             sprintf(getbuf(), "%s\n", appxml->getNarrowMessage()); log(); // show xml
             return -1;
         }     
            
         conxIds[received++] = n;
         showReturnedPortAndIP();

         if  (received < NUMCONNECTS)
              break;      // state remains 2
         else state = 3;  // fall thru ... 
     
    case 3:
         // m = conxIds;
         // sprintf(getbuf(),"\n\n%d %d %d %d %d %d %d %d %d %d %d %d %d %d %d %d\n\n",
         // m[0],m[1],m[2],m[3],m[4],m[0],m[6],m[7],m[8],m[9],m[10],m[11],m[12],m[13],m[14],m[15]);
         // log();  
                                      
         for(i=0; i < NUMCONNECTS; i++)        // Send n getDigits commands 
         {             
             delete outxml;
             outxml = new MmsAppMessage; 
             if (0 == conxIds[i]) { sprintf(getbuf(),"\n\n---- connection ID zero at index %d\n\n", i); log(); }         

             outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);
             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[i]);
             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                    "digit #"); 
             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                    "maxdigits 11"); 
             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                    "maxtime 10000");   // 10 seconds, change from 3 secs for stress test.
              
             sprintf(getbuf(),"TEST sending get digits on conx %d\n",i+1); log();     

             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
             outxml->terminateReturnMessage(clientID);
             // sprintf(getbuf(),"\n\n%s\n\n", outxml->getNarrowMessage()); log();  // show xml
             putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());             
         }

         mmsSleep(MmsTime(0, 250 + (NUMCONNECTS * 50)));  // give them time to  listen
         i = received = 0;   
         state = 4;   
         break;

    case 4:            
         // State remains at 4 until we collect all getDigit provisional results
         if  (-1 == checkResultCode(1)) return -1;

         if  (++received < NUMCONNECTS)
              break;
         else 
         {    isPassthruProvisional = FALSE;
              i = 0;
              state = 5;  // fall thru ... 
         } 
     

    case 5:  // On each connection, send a few digits one at a time. terminated by pound 

         for(; i < NUMCONNECTS; i++)
         { 
             j = 0;
             for(; j < NUMDIGITS; j++)
             {
                delete outxml;
                outxml = new MmsAppMessage;
      
                outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);
                outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[i]);

                c = (j == NUMDIGITS-1)? "#": (char*)digits[j]; 
                outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], c);

                outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
                outxml->terminateReturnMessage(clientID);
            
                sprintf(getbuf(),"TEST sending digit %s on conx %d\n", c, i+1); log();
                putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());  
             }
         }

         sendDigitResults = getDigitResults = received = 0;
         state = 6;
         break;


    case 6:  // Get result for each of the above sendDigits commands
             // State remains at 6 until we collect all the results
             // We will likely collect some receiveDigits results as well before then
         if  (-1 == checkResultCode()) return -1;

         if  (command == MmsAppMessageX::MMSMSG_GETDIGITS)  getDigitResults++;
         else
         if  (command == MmsAppMessageX::MMSMSG_SENDDIGITS) sendDigitResults++;
         else
         {    sprintf(getbuf(),"Expected GETDIGITS or SENDDIGITS found '%s'\n", appxml->commandName());
              log();
              return -1;
         }

         if  (sendDigitResults == (NUMCONNECTS * NUMDIGITS)) 
         {
              state = 7;                    // We may also have collected all of
                                            // the getDigits results by now as well
              if  (getDigitResults < NUMCONNECTS)            
                   break;
              else;// fall thru, all results collected                 
         }
         else break;


    case 7:   // Get result for each of the receiveDigits commands (which terminated
              // on pound sign) State remains at 7 until we collect all results
              // We will likely have collected some or all of these above

         if  (getDigitResults < NUMCONNECTS)
         {     
             if  (-1 == checkResultCode()) return -1;

             if  (command == MmsAppMessageX::MMSMSG_GETDIGITS)  getDigitResults++;    
             else
             {    sprintf(getbuf(),"Expected GETDIGITS found '%s'\n", appxml->commandName());
                  log();
                  return -1;
             }
         }

         if  (getDigitResults < NUMCONNECTS)
              break;
         else state = 8;    // fall thru ...


    case 8:                 // Send n disconnects 
                                           
         for(; i < NUMCONNECTS; i++)
         {
             delete outxml;
             outxml = new MmsAppMessage;
              
             outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[i]); 
             outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
             outxml->terminateReturnMessage(clientID);
             putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
             sprintf(getbuf(),"TEST sending session %d disconnect\n", i+1); log();
         }

         i = 0;
         state = 9;
         break;


    case 9:   // Get result for each of the disconnect commands 
              // State remains at 9 until we collect all results
         if  (-1 == checkResultCode()) return -1;

         if  (++received < NUMCONNECTS)
              break;         // get next result
         else                // if more iterations ...
         if  (++digitsiters < NUMITERATIONS)
         {                   // ... send a new batch of connect commands  
             sprintf(getbuf(),"\n\n>> iteration %d ...\n", ++testIterations); log();

             for(; i < NUMCONNECTS; i++)
             {             
                 if (outxml) delete outxml;
                 outxml = new MmsAppMessage;
              
                 outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
                 outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], port += 2);
                 outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

                 outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
                 outxml->terminateReturnMessage(clientID);
                 putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
                
                 sprintf(getbuf(),"TEST sending session %d connect\n", i+1); log();
             }

             i = received = 0;
             isPassthruProvisional = TRUE;
             state = 2;      // ... and loop back to begin another test iteration
             break;
         }              
         else state = 10;    // done: fall thru ...
 
    case 10:                 // Disconnect from media server
         interCommandSleepMs = USE_DEFAULT_INTERCOMMAND_DELAY;
         if (serverconnect & SERVER_DISCO) sendServerDisconnect(outxml);  
         isThereAnOutboundMessage = 1;          
         result = -1;
         break;

  } // switch(state)



  if  (isThereAnOutboundMessage)
  {
       outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
       outxml->terminateReturnMessage(clientID);
       // sprintf(getbuf,"\n\n%s\n\n", outxml->getNarrowMessage()); log();
                                            // Send the outbound message
       putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  }

  if  (appxml) delete appxml;
  appxml = NULL;

  return result;
}



int scheduledConferenceSim(const unsigned int serverconnect)
{
  // Emulate the scheduled conference max app 
  static int digitcount, isserverconnect, port=1000;
  static int conxIds[2],  conferenceID, testIterations;
  static int NUMITERATIONS, NUMDIGITS, NUMCONNECTS;

  #define SCS_PLAYFILE_ENTER_PIN      "thankyou.vox"
  #define SCS_PLAYFILE_CONF_WELCOME   "thankyou.vox" 
  #define SCS_PLAYFILE_ANNC_NEW_PARTY "thankyou.vox"
  #define SCS_PLAYFILE_SPEAK_NAME     "thankyou.vox"
  #define SCS_PLAYFILE_ANNC_PARTYLEFT "thankyou.vox"

  static char* SCS_DIGITS_PIN[] = { "5","5","2","4","4","#" };

  #define SCS_HALFCONNECTDELAYMSECS 250
  #define SCS_MINDIGDELAYMSECS  500
  #define SCS_MAXDIGDELAYMSECS 1500
  #define SCS_MININTERDIGDELAYMSECS  250
  #define SCS_MAXINTERDIGDELAYMSECS 1000
  #define SCS_MINHOSTCONFCONNECTDELAYMSECS  500
  #define SCS_MAXHOSTCONFCONNECTDELAYMSECS 1500
  #define SCS_MINCONFDELAYMSECS 2000
  #define SCS_MAXCONFDELAYMSECS 5000

  
  if (testIterations == 0)
  {
      sprintf(getbuf(),"\nBegin scheduled conference sim on %s\n", queueName); log();
                                            // If config parameters are for this test ...
      if (config->iparamA == ID_SCHED_CONFERENCE)
      {
          NUMITERATIONS = config->iparamB;  // ... get parameters from config
      }
      else
      {                                     // Otherwise use default parameters
          NUMITERATIONS = SCS_NUMITERATIONS;
      }

      sprintf(getbuf(),"Configured iterations: %d\n\n", NUMITERATIONS);
      log();
      mmsSleep(3);
      srand(time(0));
  }

  char *p = NULL, *termcondname;
  int   n = 0, result = 1, i = 0, isThereAnOutboundMessage = TRUE;
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

    case 1:                                 // Send party A half connect
         sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
         if (!clientID) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         log("TEST 1. sending party A half connect\n");
         break;

    case 2:                                 // Send party A full connect
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[0] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if (n = SCS_HALFCONNECTDELAYMSECS) mmsSleep(MMS_N_MS(SCS_HALFCONNECTDELAYMSECS));

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST 2. sending party A final connect\n");              
         break;

    case 3:                                 // Play "Enter PIN"
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], SCS_PLAYFILE_ENTER_PIN);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1"); 
         log("TEST 3. sending play (enter PIN)\n");
         isPassthruProvisional = TRUE;      // Route provisional thru state machine
         break;

    // One difference seems to be that in the voice tunnel tests, we sent digits prior
    // to issuing a getdigits. Here we are sending the getdigits before sending digits 

    case 4:                                  // Get play provisional, send getDigits
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST 4. sending PIN getDigits\n");          
         isPassthruProvisional = TRUE;      // Ask for GetDigits provisional
         break;  

    case 5:                                 // Get GetDigits provisional, send first digit
         if (-1 == checkResultCode(1)) return -1; 
         digitcount = 0;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         mmsSleep(1);                       // Pause to permit partial play 
         log("TEST 5. sending PIN digit 1\n");         
         isPassthruProvisional = FALSE; 
         break;
                         
    case 6:                                 // Get SendDigits result, send 2nd digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         mmsSleep(MMS_N_MS(n));
         sprintf(getbuf(),"TEST 6. pause %d ms before send PIN digit 2\n", n); log();           
         break;

    case 7:                                 // Get SendDigits result, send 3rd digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 7. pause %d ms before send PIN digit 3\n", n); log();
         mmsSleep(MMS_N_MS(n));
         break;

    case 8:                                 // Get SendDigits result, send 4th digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 8. pause %d ms before send PIN digit 4\n", n); log();
         mmsSleep(MMS_N_MS(n));
         break;
        
    case 9:                                 // Get SendDigits result, send 5th digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 9. pause %d ms before send PIN digit 5\n", n); log(); 
         mmsSleep(MMS_N_MS(n));      
         break;

    case 10:                                 // Get SendDigits result, send 6th digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 10. pause %d ms before send PIN digit 6\n", n); log();   
         mmsSleep(MMS_N_MS(n));          
         break;

    case 11:                                // Get SendDigits result
         if (-1 == checkResultCode()) return -1;
         isThereAnOutboundMessage = FALSE;                       
         break;

    case 12:                                // Get get getDigits termination, send party A conf connect
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], 0); 
         log("TEST 12. sending party A conference connect\n");                         
         break;

    // This is oddball -- should play this announcement before party added to conference

    case 13:                                 // Get connect result, play "welcome" to end
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conferenceID = getExpectedConferenceID())) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], SCS_PLAYFILE_CONF_WELCOME);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 10000"); 
         log("TEST 13. sending play (welcome)\n");
         break;

    case 14:                                 // Get play term, send party B half connect
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
         log("TEST 14. sending party B half connect\n");
         break;

    case 15:                                 // Send party B full connect
         if (-1 == checkResultCode()) return -1;
         if (-1 == (conxIds[1] = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();
         if (n = SCS_HALFCONNECTDELAYMSECS) mmsSleep(MMS_N_MS(SCS_HALFCONNECTDELAYMSECS));

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1002);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 
         log("TEST 15. sending party B final connect\n");              
         break;

    case 16:                                 // Play "Enter PIN"
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], SCS_PLAYFILE_ENTER_PIN);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxdigits 1"); 
         log("TEST 16. sending play (enter PIN)\n");
         // Have to wait for play to complete, or GetDigits will find session busy
         break;

    case 17:                                 // Get play result, send getDigits
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_GETDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 15000");  
         log("TEST 17. sending PIN getDigits\n");          
         isPassthruProvisional = TRUE;      // Ask for GetDigits provisional
         break;  

    case 18:                                // Get GetDigits provisional, send first digit
         if (-1 == checkResultCode(1)) return -1; 
         digitcount = 0;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         mmsSleep(1);                       // Pause to permit partial play 
         log("TEST 18. sending PIN digit 1\n");         
         isPassthruProvisional = FALSE; 
         break;
                         
    case 19:                                // Get SendDigits result, send 2nd digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         mmsSleep(MMS_N_MS(n));
         sprintf(getbuf(),"TEST 19. pause %d ms before send PIN digit 2\n", n); log();           
         break;

    case 20:                                // Get SendDigits result, send 3rd digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 20. pause %d ms before send PIN digit 3\n", n); log();
         mmsSleep(MMS_N_MS(n));
         break;

    case 21:                                // Get SendDigits result, send 4th digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 21. pause %d ms before send PIN digit 4\n", n); log();
         mmsSleep(MMS_N_MS(n));
         break;
        
    case 22:                                // Get SendDigits result, send 5th digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]);
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 22. pause %d ms before send PIN digit 5\n", n); log();
         mmsSleep(MMS_N_MS(n));       
         break;

    case 23:                                // Get SendDigits result, send 6th digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], SCS_DIGITS_PIN[digitcount++]); 
         n = randomNumberInRange(SCS_MININTERDIGDELAYMSECS, SCS_MAXINTERDIGDELAYMSECS);
         sprintf(getbuf(),"TEST 23. pause %d ms before send PIN digit 6\n", n); log();  
         mmsSleep(MMS_N_MS(n));                 
         break;

    case 24:                                // Get SendDigits result
         if (-1 == checkResultCode()) return -1;
         isThereAnOutboundMessage = FALSE;                       
         break;

    case 25:                                // Get getDigits termination, send play "speak name"
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], SCS_PLAYFILE_SPEAK_NAME);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 10000"); 
         log("TEST 25. sending play (speak name)\n");
         break;
     
    case 26:                                // Get play result, send record
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_RECORD]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "digit #"); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxtime 10000");  
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION],
                "maxsil 9900");  
         log("TEST 26. sending record\n");          
         isPassthruProvisional = TRUE;      // Ask for GetDigits provisional
         break;  

    case 27:                                // Get record provisional, pause, send term digit
         if (-1 == checkResultCode(1)) return -1; 
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_SENDDIGITS]);  
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::DIGITS], "#"); 
         mmsSleep(2);                       // Pause to permit record 
         log("TEST 27. sending record pound digit\n");         
         isPassthruProvisional = FALSE; 
         break;

    case 28:                                // Get SendDigits result
         if (-1 == checkResultCode()) return -1;
         isThereAnOutboundMessage = FALSE;                       
         break;

    case 29:                                // Get record termination, send party B conf connect
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         log("TEST 29. sending party B conference connect\n");                         
         break;

     // This is oddball -- should play this announcement before party added to conference

    case 30:                                 // Get connect result, play "welcome" to end
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], SCS_PLAYFILE_CONF_WELCOME);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 10000"); 
         log("TEST 30. sending play (welcome)\n");
         break;
                                            // Get play result
    case 31:                                // Wait a bit and disco party B
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[1]); 
         n = randomNumberInRange(SCS_MINCONFDELAYMSECS, SCS_MINCONFDELAYMSECS);
         sprintf(getbuf(),"TEST 31. pause %d ms before disco party B\n", n); log();
         mmsSleep(MMS_N_MS(n));
         break;

    case 32:                                 // Get dicso result, play "party left" 
         if (-1 == checkResultCode()) return -1;
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_PLAY]);
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID], conferenceID); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME], SCS_PLAYFILE_ANNC_PARTYLEFT);
         termcondname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION]; 
         outxml->putParameter(termcondname, "maxtime 10000"); 
         log("TEST 32. sending play (party has left)\n");
         break;
                                             
    case 33:                                // Get play result, send party A full disco
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]); 
         outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID], conxIds[0]); 
         log("TEST 33. sending party A full disconnect\n");
         break;
                                            // Get party A disco result
    case 34:                                // Disco from server 
         if  (-1 == checkResultCode()) return -1;

         if  (testIterations < NUMITERATIONS) 
         {
              sprintf(getbuf(), "\n\n>> iteration %d ...\n", ++testIterations); log();
              outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
              outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 
              mmsSleep(1);
              log("TEST 1. sending party A half connect\n");
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
 
int simpleLBRTest(const unsigned int serverconnect)
{
  static int iters, isserverconnect;
  if  (iters++ == 0) printf("\nBegin simple LBR test\n\n");
  char* p = NULL;
  int   n = 0, result = 1;
  if  (outxml) delete outxml;
  outxml = new MmsAppMessage;
  static bool bLocalCoderMissing;
  static bool bRemoteCoderMissing;
  static bool bSendOnly;
  static bool bReceiveOnly;
  static int coderType;
  static int frameSize;
  static char coderparam[32];
  static char frameparam[32];

  switch(state)
  {
    case 0:                                 // Send initial connect
         isserverconnect = (serverconnect & SERVER_CONNECT) != 0;
         if  (isserverconnect) sendServerConnect(outxml);
         else return 0 == (state = 1); 
         break;

    case 1:                                 // Send an IP connect
         // allow user to select a subtest.
         bLocalCoderMissing = false;
         bRemoteCoderMissing = false;
         bSendOnly = false;
         bReceiveOnly = false;
         coderType = 1;
         frameSize = 30;

         do{
           char c=0; 
           printf("\n\nPlease select an unit test:\n"); 
           printf("(a) Use G.711 ulaw, frame size 10.\n");
           printf("(b) Use G.711 ulaw, frame size 20.\n");
           printf("(c) Use G.711 ulaw, frame size 30.\n");
           printf("(d) Use G.711 alaw, frame size 10.\n");
           printf("(e) Use G.711 alaw, frame size 20.\n");
           printf("(f) Use G.711 alaw, frame size 30.\n");  
           printf("(g) Use G.711 alaw, frame size 60, invalid frame size.\n");  
           printf("(h) Use G.723, frame size 30.\n");
           printf("(i) Use G.723, frame size 60.\n");
           printf("(j) Use G.723, frame size 10, invalid frame size.\n");  
           printf("(k) Use G.729, frame size 20.\n");
           printf("(l) Use G.729, frame size 30.\n");
           printf("(m) Use G.729, frame size 40.\n");
           printf("(n) Use G.729, frame size 10, invalid frame size.\n");  
           printf("(o) Use G.723 if available or G.711 ulaw, frame size 30.\n");
           printf("(p) Use G.723 if available or G.711 alaw, frame size 30.\n");
           printf("(q) Use G.723 if available or G.711 alaw but pass in G.723 only frame size 60.\n");
           printf("(r) Use G.729 if available or G.711 ulaw, frame size 30.\n");
           printf("(s) Use G.729 if available or G.711 alaw, frame size 30.\n");
           printf("(t) Use G.729 if available or G.711 alaw but pass in G.729 only frame size 40.\n");
           printf("(u) Missing local coder type.\n");
           printf("(v) Missing remote coder type.\n");
           printf("(w) Send only\n");
           printf("(x) Receive only\n");
           while(!c) {
             c=_getch();
           }
             
           switch(c)
           {
             case 'a':      // G.711 ulaw, frame size 10
               coderType = 1;
               frameSize = 10;
               break;

             case 'b':      // G.711 ulaw, frame size 20
               coderType = 1;
               frameSize = 20;
               break;

             case 'c':      // G.711 ulaw, frame size 30
               coderType = 1;
               frameSize = 30;
               break;

             case 'd':      // G.711 alaw, frame size 10
               coderType = 2;
               frameSize = 10;
               break;

             case 'e':      // G.711 alaw, frame size 20
               coderType = 2;
               frameSize = 20;
               break;

             case 'f':      // G.711 alaw, frame size 30
               coderType = 2;
               frameSize = 30;
               break;

             case 'g':      // G.711 alaw, invalid frame size 60
               coderType = 2;
               frameSize = 60;
               break;

             case 'h':      // Use G.723, frame size 30
               coderType = 4;
               frameSize = 30;
               break;

             case 'i':      // Use G.723, frame size 60
               coderType = 4;
               frameSize = 60;
               break;

             case 'j':      // Use G.723, invalid frame size 10
               coderType = 4;
               frameSize = 10;
               break;

             case 'k':      // Use G.729, frame size 20
               coderType = 8;
               frameSize = 20;
               break;

             case 'l':      // Use G.729, frame size 30
               coderType = 8;
               frameSize = 30;
               break;

             case 'm':      // Use G.729, frame size 40
               coderType = 8;
               frameSize = 40;
               break;

             case 'n':      // Use G.729, invalid frame size 10
               coderType = 8;
               frameSize = 10;
               break;

             case 'o':      // G.723 if available or G.711 ulaw, frame size 30
               coderType = 5;
               frameSize = 30;
               break;

             case 'p':      // G.723 if available or G.711 alaw, frame size 30
               coderType = 6;
               frameSize = 30;
               break;

             case 'q':      // G.723 if available or G.711 alaw but pass in G.723 only frame size 60
               coderType = 6;
               frameSize = 60;
               break;

             case 'r':      // G.729 if available or G.711 ulaw, frame size 30
               coderType = 9;
               frameSize = 30;
               break;

             case 's':      // G.729 if available or G.711 alaw, frame size 30
               coderType = 10;
               frameSize = 30;
               break;

             case 't':      // G.729 if available or G.711 alaw but pass in G.723 only frame size 40
               coderType = 10;
               frameSize = 40;
               break;

             case 'u':      // Missing local coder type
               bLocalCoderMissing = true;
               break;

             case 'v':      // Missing remote coder type
               bRemoteCoderMissing = true;
               break;

             case 'w':      // Send only
               bSendOnly = true;
               break;

             case 'x':      // Receive only
               bReceiveOnly = true;
               break;

             default:
               printf("\n\nInvalid selection, execute default unit test!!!\n");
               break; 
           }
         } while(0); 

         if  (isserverconnect) clientID = getMessageClientID();
         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 0);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], " "); 

         if (bSendOnly)
           putConnectParam(outxml, "dataflowDirection ipSendOnly");
         else if (bReceiveOnly)
           putConnectParam(outxml, "dataflowDirection ipReceiveOnly");

         if (bLocalCoderMissing)
         {
           sprintf(coderparam, "coderTypeRemote %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeRemote %d", frameSize);
           putConnectParam(outxml, frameparam);
         }
         else if (bRemoteCoderMissing)
         {
           sprintf(coderparam, "coderTypeLocal %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeLocal %d", frameSize);
           putConnectParam(outxml, frameparam);
         }
         else 
         {
           sprintf(coderparam, "coderTypeRemote %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeRemote %d", frameSize);
           putConnectParam(outxml, frameparam);
           sprintf(coderparam, "coderTypeLocal %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeLocal %d", frameSize);
           putConnectParam(outxml, frameparam);
         }

         log("TEST sending session half connect with LBR parameters\n");
         break;

    case 2:
         if  (-1 == checkResultCode()) return -1;
         if  (-1 == (n = getExpectedConnectionID())) return -1;
         showReturnedPortAndIP();

         outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CONNECTION_ID], n); 
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::PORT], 1000);
         outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::IP_ADDRESS], "127.0.0.1"); 

         if (bSendOnly)
           putConnectParam(outxml, "dataflowDirection ipSendOnly");
         else if (bReceiveOnly)
           putConnectParam(outxml, "dataflowDirection ipReceiveOnly");

         if (bLocalCoderMissing)
         {
           sprintf(coderparam, "coderTypeRemote %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeRemote %d", frameSize);
           putConnectParam(outxml, frameparam);
         }
         else if (bRemoteCoderMissing)
         {
           sprintf(coderparam, "coderTypeLocal %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeLocal %d", frameSize);
           putConnectParam(outxml, frameparam);
         }
         else 
         {
           sprintf(coderparam, "coderTypeRemote %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeRemote %d", frameSize);
           putConnectParam(outxml, frameparam);
           sprintf(coderparam, "coderTypeLocal %d", coderType);
           putConnectParam(outxml, coderparam);
           sprintf(frameparam, "frameSizeLocal %d", frameSize);
           putConnectParam(outxml, frameparam);
         }

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

