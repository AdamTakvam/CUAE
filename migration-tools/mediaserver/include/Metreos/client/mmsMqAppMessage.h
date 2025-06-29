// 
// mmsMqAppMessage.h
//
// IPC XML message definitions
//
#ifndef MMS_MQ_APPMESSAGE_H
#define MMS_MQ_APPMESSAGE_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include <wtypes.h>
#include <mbstring.h>

#define MMS_MQ_MAX_XMLMESSAGESIZE  2048
#define MMS_MAXXMLTAGSIZE            64
#define MMS_MAXXMLPARAMSIZE          64
#define MAXDIGITPATTERNSIZE          15
#define MMS_OMIT_RESULTCODE    0xabbadaba
#define WTF                    "????"
#define FIELDNAME_FRAGMENT     "<field name=\""
#define FIELDNAME_END_FRAGMENT "\">"
#define FIELD_END_TAG          "</field>"
#define MMS_MSG_TAG            "<message>"
#define MMS_MSG_END_TAG        "</message>"
#define MESSAGEID_TAG          "<messageId>"
#define MESSAGEID_END_TAG      "</messageId>"
#define MMS_RESULTCODE_NAME    "resultCode"
#define MMS_TRANSACTIONID_NAME "transactionId"
#define MMS_RETURNRES_NAME     "resources"
#define MMS_HEARTBEAT_NAME     "heartbeat"
#define MMS_TERMINATING_NAME   "terminationCondition"
#define MMS_XML_HDR "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"

// Example input:
// <?xml version="1.0" encoding="utf-8" ?>
// <message xmlns="http://metreos.com/InternalMessage.xsd">
//    <messageId>somemessagename</messageId>
//    <source>Metreos.Samoa.PAL.Provider.SIP</source>
//    <sourceQueue>.\\Private$\\SomeQueueName</sourceQueue>
//    <field name="nameA">value A</field>
//    <field name="nameB">value B</field>
// </message>
 



class MmsMqAppMessage 
{
  public:
                                             
  MmsMqAppMessage(char* utf8message);       // Incoming message

  MmsMqAppMessage();                        // Outgoing message

  int   ansiToUtf8(char* a);

  int   utf8ToAnsi(char* u);

  char* put(char* c);                       // Append ansi string to ubuf 
        // Isolate value of named parameter, plus return buffer pos
  char* getvaluefor(const char* name, char* searchstart = NULL);               
        // Return buffer pointer to value of a named parameter                                 
  char* findparam(const char* name, char* searchstart = NULL);               
                                            
  char* makeFieldName(const char* name);    // Construct xml named parameter
                                            
  int   isolateParameterValue(char* bufpos);// Extract value from buffer

  int   getCommand();                       // Identify server command

  int   getConnectionAttribute();           // Identify current connect attr
  int   getAudioFileAttribute();            // Identify current playrec attr 
  int   getAudioToneAttribute();            // Identify current playtone attr 
  int   getTerminationCondition();          // Identify current term cond
  int   getConferenceAttribute();           // Identify current conference attr  
  int   getConfereeAttribute();             // Identify current conferee attr
  int   getConfereeAttributeParam(char**);  // Identify conferee attr parameter         

  void  markParameters();                   // Locate & mark start of params 

  void  makelower();                        // Convert ansi buffer to lower case

  void  beginReturnMessage();               // Seed return message buffer
                                            // Terminate xml return message
  void  terminateReturnMessage(const int rc=MMS_OMIT_RESULTCODE);  
  void  terminateReturnMessage(void* id, const int rc=MMS_OMIT_RESULTCODE);           
                                            // Make inbound message outbound
  int   makeTurnaroundMessage(int resultCode, void* clientID=NULL);

  int   backupToEndTag();                   // Move write pointer to end tag

  char* maketag(const char* tagtext, const int isend=FALSE);

  int   remove (const char* tagtext);       // Remove tag from message
  int   remove (const int paramid);
                                            
  void  putMessageID (int messageno);       // Insert server command xml

  void  putMessageID (char* name);          // Insert server command xml

  void  putTransactionID(int id);           // Insert transaction ID xml

  void  putResultCode(int resultcode);      // Insert result code xml

  void  putServerID(int id);                // Insert server ID xml
  void  putClientID(void* id);              // Insert client ID xml
  void* getClientID();   
  int   getServerID();                      // Get id from object or xml
               
  void  putTerminationCondition(char* name);// Insert termination condition xml
                                            // Insert field name= xml
  void  putParameter(char* name,  char* value);
  void  putParameter(char* name,  int   value);
  void  putParameter(int paramid, char* value);
  void  putParameter(int paramid, int   value);
                                            // Format a blank-delimited pair
  int   makeNameValuePair(const char* name, const int value, 
                          char* buf, int* buflen);  
  int   makeNameValuePair(const char* name, const char* value, 
                          char* buf, int* buflen); 

  int   isMessage(const int messageid);

  int   findNameValuePairDelimiters();

  unsigned char* getReturnMessage() { return (unsigned char*)this->ubuf; }
  char* getNarrowMessage()  { return this->ubuf;   } 
  char* paramValue()        { return this->valbuf; }
  char* firstparam()        { return this->pparams;}
  char* termcondValue()     { return this->pattrvalue; }
  char* attrValue()         { return this->pattrvalue; }
  int   paramLength()       { return this->paramlength;}
  int   narrowMsglength()   { return this->ubuflen;    }
  int   commandID()         { return this->commandno;  }
  char* commandName()       { return this->cmdbuf; }
  char* commandName(const int n);
  void* destination()       { return this->hqDestination; } 
  void  destination(void* p){ this->hqDestination = p;    }
  int   serverID()          { return this->serverid;      }   
  void  serverID(int n)     { this->serverid = n;         }

  virtual ~MmsMqAppMessage();

  protected:
                                            
  char   ubuf[MMS_MQ_MAX_XMLMESSAGESIZE];   // Ansi or UTF-8 character buffer         
  WCHAR  wbuf[MMS_MQ_MAX_XMLMESSAGESIZE];   // Unicode character buffer
  char*  pu, *pparams, *ptermcondvalue, *pattrvalue;
  LPWSTR pw;
  int    commandno, termcondno, ubuflen, paramlength, serverid; 
  void*  hqDestination;                             
  char   tagbuf[MMS_MAXXMLTAGSIZE];
  char   valbuf[MMS_MAXXMLPARAMSIZE];
  char   cmdbuf[MMS_MAXXMLPARAMSIZE];

  void init();                              // Ctor initialization

  void textinit();                          // Table initialization
};



#endif