//
// MmsAppMessage.cpp  
//
// Represents an incoming or outgoing application XML message body 
// and the methods required to operate on same
//
#include "StdAfx.h"
#include "stdlib.h"
#pragma warning(disable:4786)
#include "mmsAppMessage.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


                                            // Ctor 1: incoming message
MmsAppMessage::MmsAppMessage(char* utf8message)     
{ 
  init();
  this->ubuflen = this->utf8ToAnsi(utf8message);
}


                                            // Ctor 2: outgoing message
MmsAppMessage::MmsAppMessage()         
{ 
  init();
  beginReturnMessage();
}



int MmsAppMessage::ansiToUtf8(char* a)
{ 
  const unsigned int len = strlen(a)+1;

  MultiByteToWideChar(CP_ACP,  0, a, len, wbuf, len*2); 

  WideCharToMultiByte(CP_UTF8, 0, wbuf, len*2, ubuf, len, NULL, NULL);

  return len; 
}



int MmsAppMessage::utf8ToAnsi(char* u)
{ 
  const unsigned int len = _mbslen((unsigned char*)u);

  MultiByteToWideChar(CP_UTF8, 0, u, len, wbuf, len*2); 

  WideCharToMultiByte(CP_ACP,  0, wbuf, len*2, ubuf, len, NULL, NULL);

  return len; 
}



char* MmsAppMessage::put(char* c)         // Append ansi string to ubuf
{
  // XML string length is always expected to be (writepointer - buffer)
  // The XML string is terminated at each write, and the terminator is 
  // then overwritten on a subsequent write.

  const int len = strlen(c);
  memcpy(pu, c, len);
  ubuflen = (pu += len) - ubuf;
  *pu = '\0';                                
  return pu;
}
       


char* MmsAppMessage::getvaluefor(const char* name, char* searchstart)               
{ 
  // Get value of named parameter into valbuf and return buffer position 

  memset(valbuf,0,sizeof(valbuf));
  char*  valpos = findparam(name, searchstart);
  if    (valpos)  isolateParameterValue(valpos);
  return valpos;                            
}



int MmsAppMessage::isolateParameterValue(char* xmlbufpos)
{
  // Copy value from message buffer into value buffer returning length

  char* valend = strstr(xmlbufpos, FIELD_END_TAG);
  if  (!valend) return 0;
  memset(valbuf,0,sizeof(valbuf));
  this->paramlength = valend - xmlbufpos;
  const int maxlen  = MMS_MAXXMLPARAMSIZE - 1;
  if (paramlength > maxlen) paramlength = maxlen;
  memcpy(valbuf, xmlbufpos, this->paramlength);
  return this->paramlength;
}

    
                                       
char* MmsAppMessage::findparam(const char* name, char* searchstart)               
{ 
  // Return pointer to value of a named parameter

  char* searchstring = makeFieldName(name);
  if   (searchstart == NULL) searchstart = pparams;

  char* stringlocation  = strstr(searchstart, searchstring);
  if   (stringlocation == NULL) return NULL;

  char*  paramlocation  = stringlocation + strlen(searchstring);
  return paramlocation;
}


                                             
char* MmsAppMessage::makeFieldName(const char* name)   
{
  // Construct an xml named parameter

  char*  p = tagbuf;
  memcpy(p, FIELDNAME_FRAGMENT, sizeof(FIELDNAME_FRAGMENT));
  p  += (sizeof(FIELDNAME_FRAGMENT) - 1);
  const int len = strlen(name);
  memcpy(p, name, len);
  p  += len;                                // Note this copy includes nullterm
  memcpy(p, FIELDNAME_END_FRAGMENT, sizeof(FIELDNAME_END_FRAGMENT));
  return tagbuf;
}


                                             
int MmsAppMessage::makeNameValuePair        // Construct name/value pair
(const char* name, const int value, char* buf, int* buflen)   
{
  char c[16]; wsprintf(c,"%d",value);
  return this->makeNameValuePair(name, c, buf, buflen);
}


                                             
int MmsAppMessage::makeNameValuePair        // Construct name/value pair
(const char* name, const char* value, char* buf, int* buflen)   
{
  const int len1 = name?  strlen(name):  0;
  const int len2 = value? strlen(value): 0;
  const int len0 = 2 + len1 + len2;
  if((len0 == 2) || !*buflen) return -1;
  if (*buflen < len0)
  {   *buflen = len0;
       return 0;
  }

  const int stringlen = len0-1;
  memset(buf, ' ', len0); 

  if  (len1) memcpy(buf, name, len1);
  if  (len2) memcpy(buf+len1+1, value, len2);

  *(buf+stringlen) = '\0'; 

  return stringlen;                                
}



int MmsAppMessage::getCommand()             // Identify server command
{
  memset(cmdbuf,0,sizeof(cmdbuf));
  char* p = strstr(ubuf, MESSAGEID_TAG);
  if   (p == NULL) return -1;
  p += (sizeof(MESSAGEID_TAG) - 1);
  char* q = strstr(p, MESSAGEID_END_TAG);
  if   (q == NULL) return -1;
  const int len = q - p;
  memcpy(cmdbuf, p, len);

  for(commandno=0; commandno < MmsAppMessageX::MESSAGE_COUNT; commandno++)
      if  (strcmp(cmdbuf, MmsAppMessageX::messagenames[commandno]) == 0) 
           return commandno;

  commandno = -1;
  return commandno;
}



char* MmsAppMessage::commandName(const int n) 
{ 
  if  (n >= MmsAppMessageX::MESSAGE_COUNT) 
      *cmdbuf = 0; 
  else strcpy(cmdbuf, MmsAppMessageX::messagenames[n]);
  return cmdbuf;
}



int MmsAppMessage::getConnectionAttribute()             
{ 
  // Identify current connection attribute, e.g. "coderTypeRemote g711ulaw"

  if  (-1 == this->findNameValuePairDelimiters()) return -1;               
                                            // Identify attribute name
  for(int i=0; i < MmsAppMessageX::CONNECTION_ATTR_COUNT; i++)     
      if  (stricmp(this->valbuf, MmsAppMessageX::connectionAttrNames[i]) == 0) 
           break;

  return (i < MmsAppMessageX::CONNECTION_ATTR_COUNT)? i: -1;
}



int MmsAppMessage::getTerminationCondition()             
{ 
  // Identify current termination condition, e.g. "maxtime 30000"

  if  (-1 == this->findNameValuePairDelimiters()) return -1;               
                                            // Identify term cond name      
  for(int i=0; i < MmsAppMessageX::TERMCOND_COUNT; i++)     
      if  (stricmp(this->valbuf, MmsAppMessageX::termcondnames[i]) == 0) 
           break;

  return (i < MmsAppMessageX::TERMCOND_COUNT)? i: -1;
}



int MmsAppMessage::getConferenceAttribute()             
{ 
  // Identify current conference attribute, e.g. "soundtone"
                                            
  for(int i=0; i < MmsAppMessageX::CONFERENCE_ATTR_COUNT; i++)     
      if  (stricmp(valbuf, MmsAppMessageX::conferenceAttrNames[i]) == 0) 
           break;

  return (i < MmsAppMessageX::CONFERENCE_ATTR_COUNT)? i: -1;
}



int MmsAppMessage::getConfereeAttribute()             
{ 
  // Identify current conferee attribute, e.g. "receiveonly"
                                             
  for(int i=0; i < MmsAppMessageX::CONFEREE_ATTR_COUNT; i++)     
      if  (stricmp(valbuf, MmsAppMessageX::confereeAtrrNames[i]) == 0) 
           break;

  return (i < MmsAppMessageX::CONFEREE_ATTR_COUNT)?  i: -1;
}



int MmsAppMessage::getConfereeAttributeParam(char** value)            
{                
  // Identify conferee attribute when specified as a paremeter, 
  // e.g. <field name="receiveonly">1</field>, and identify parameter value

  char* attrvalue = NULL, *attrname = NULL;
  int   attrindex = 0;

  for(; attrindex < MmsAppMessageX::CONFEREE_ATTR_COUNT; attrindex++)
  {
      attrname = MmsAppMessageX::confereeAtrrNames[attrindex];
      if  (NULL != (attrvalue = getvaluefor(attrname))) break;
  }

  if  (attrvalue == NULL) return -1;

  if  (value) *value = attrvalue;           // Return pointer to value
  return attrindex;                         // Return attribute name index
}



int MmsAppMessage::getAudioFileAttribute()             
{ 
  // Identify current audio file attribute, e.g. "encoding ulaw"

  if  (-1 == this->findNameValuePairDelimiters()) return -1;               
                                            // Identify attribute name
  for(int i=0; i < MmsAppMessageX::AUDIO_FILE_ATTR_COUNT; i++)     
      if  (stricmp(this->valbuf, MmsAppMessageX::audioFileAttrNames[i]) == 0) 
           break;

  return (i < MmsAppMessageX::AUDIO_FILE_ATTR_COUNT)? i: -1;
}



int MmsAppMessage::getAudioToneAttribute()             
{ 
  // Identify current audio tone attribute, e.g. "frequency 30"

  if  (-1 == this->findNameValuePairDelimiters()) return -1;               
                                            // Identify attribute name
  for(int i=0; i < MmsAppMessageX::AUDIO_TONE_ATTR_COUNT; i++)     
      if  (stricmp(this->valbuf, MmsAppMessageX::audioToneAttrNames[i]) == 0) 
           break;

  return (i < MmsAppMessageX::AUDIO_TONE_ATTR_COUNT)? i: -1;
}



int MmsAppMessage::findNameValuePairDelimiters()
{
  // Isolate and mark components of a blank-delimited name/value pair

  this->pattrvalue = NULL;                 
  char*  q = this->valbuf;
  while(*q && *q != ' ') q++;               // Find name/value delimiter space
  if   (*q == '\0') return -1;
  *q++ = '\0';                              // Terminate name 
  while(*q == ' ') q++;                     // Skip any extra blanks                      
  this->pattrvalue = q;                     // Point to start of value
  return 0;
}



void MmsAppMessage::markParameters()        // Mark start of params so we can
{                                           // begin searches at that point
  char* p = strstr(ubuf, FIELDNAME_FRAGMENT);
  if   (p)  pparams = p;
}
 


void MmsAppMessage::makelower()             // Convert ansi buffer to lower case
{                                           // for case-insensitive parsing
  for(char* p = ubuf; *p; p++) *p = tolower(*p);                           
}



void MmsAppMessage::beginReturnMessage()    // Seed return message buffer
{                                           // with xml headers
  pu = ubuf;
  put(MMS_XML_HDR);
  put(MMS_MSG_TAG);
}



void MmsAppMessage::putMessageID(int messageno)
{
  char* text = MmsAppMessageX::messagenames[messageno];
  putMessageID(text);
}



void MmsAppMessage::putMessageID(char* name)
{
  put(MESSAGEID_TAG);
  put(name);
  put(MESSAGEID_END_TAG);
}



void MmsAppMessage::putTransactionID(int id)
{   
  putParameter(MMS_TRANSACTIONID_NAME, id);       
}



void MmsAppMessage::putResultCode(int resultcode)
{   
  putParameter(MMS_RESULTCODE_NAME, resultcode);       
}


void MmsAppMessage::putResultCode(int resultcode, int reasoncode)
{   
  putParameter(MMS_RESULTCODE_NAME, resultcode); 
  putReasonCode(reasoncode);      
}


void MmsAppMessage::putReasonCode(int reasoncode)
{   
  putParameter(MMS_REASONCODE_NAME, reasoncode);       
}



void MmsAppMessage::putServerID(int id)  
{   
  char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::SERVER_ID];
  putParameter(paramname, id);       
}



void MmsAppMessage::putClientID(void* id)
{   
  char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID];
  putParameter(paramname, (int)id);       
}



void MmsAppMessage::putUserToken(int token)
{   
  char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::USER_TOKEN];
  putParameter(paramname, token);       
}



void* MmsAppMessage::getClientID()
{                  
  if (this->hqDestination) return this->hqDestination;

  const char* p = this->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID]);

  if (p) return (void*)atoi(this->valbuf);

  return NULL;
}


int MmsAppMessage::getServerID()
{ 
  // Server ID zero indicates client is running a single media server.
  // Ctor intitialized serverid to -1, overwritten in adapter.registerClient, 
  // to the server ID supplied by client, or zero. 

  return this->serverid >= 0? this->serverid:
        (this->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::SERVER_ID]))? 
         atoi(this->valbuf): 0;
}


void MmsAppMessage::putTerminationCondition(char* name)
{   
  putParameter(MMS_TERMINATING_NAME, name);       
}



void MmsAppMessage::putParameter(char* name, char* value)
{          
  put(makeFieldName(name));     
  put(value);                                
  put(FIELD_END_TAG);                        
}



void MmsAppMessage::putParameter(char* name, int value)
{          
  char buf[16]; wsprintf(buf,"%d",value);                             
  putParameter(name, buf);
}



void MmsAppMessage::putParameter(int paramid, char* value)
{
  char* name = paramid < 0 || paramid > MmsAppMessageX::PARAM_COUNT? 
        WTF: MmsAppMessageX::paramnames[paramid];
  putParameter(name, value);
}



void MmsAppMessage::putParameter(int paramid, int value)
{
  char* name = paramid < 0 || paramid > MmsAppMessageX::PARAM_COUNT? 
        WTF: MmsAppMessageX::paramnames[paramid];
  putParameter(name, value);
}

   
                                    
void MmsAppMessage::terminateReturnMessage(const int resultcode, const int reason)             
{ 
  // Complete xml return message and convert to UTF-8 as expected by client

  if  (resultcode != MMS_OMIT_RESULTCODE)   // If result code supplied ...
       putResultCode(resultcode, reason);   // ... append resultcode field
                                             
  put(MMS_MSG_END_TAG);                      

  ansiToUtf8(ubuf);
}


                                    
void MmsAppMessage::terminateReturnMessage
( void* clientID, const int resultcode, const int reason)             
{ 
  // Complete xml return message and convert to UTF-8 as expected by client
  if  (clientID)
       putClientID(clientID);

  if  (resultcode != MMS_OMIT_RESULTCODE)   // If result code supplied ...
       putResultCode(resultcode, reason);   // ... append resultcode field
                                             
  put(MMS_MSG_END_TAG);                      

  ansiToUtf8(ubuf);
}



int MmsAppMessage::makeTurnaroundMessage(int resultCode, int reason, void* clientID)
{
  // Append resultcode to inbound message and make it an outbound message
   
  const int result = this->backupToEndTag();
  if  (result == -1) return result;

  this->terminateReturnMessage(clientID, resultCode, reason);  
      
  return 0;                                  
}


                                            // Construct xml tag in tagbuf
char* MmsAppMessage::maketag(const char* tagtext, const int isend)
{
  const int textlength = strlen(tagtext);
  char* p = tagbuf;
  *p++ = '<';
  if (isend) *p++ = '/';
  strcpy(p, tagtext);
  p += textlength;
  *p++ = '>';
  *p = '\0';
  return tagbuf;
}



int MmsAppMessage::remove(const int paramid)
{
  char* name = paramid < 0 || paramid > MmsAppMessageX::PARAM_COUNT? 
        WTF: MmsAppMessageX::paramnames[paramid];
  return this->remove(name);
}


                                            // Compress xml tag out of message
int MmsAppMessage::remove(const char* tagtext)
{       
  pu  = ubuf + ubuflen;
  const char* tag = makeFieldName(tagtext);
  char* p = strstr(ubuf, tag);              // Find start of tag
  if   (p == NULL)  return 0;  
 
  const char* endtag = FIELD_END_TAG;
  const int   endtaglength = sizeof(FIELD_END_TAG) - 1;
  char* q = strstr(p, endtag);              // Find end tag
  if   (q == NULL)  return 0;

  q  += endtaglength;                       // Point at next char after tag
  const char* endbuf = ubuf + MMS_MAX_XMLMESSAGESIZE;
  if   (q > endbuf) return 0;

  const int remainingMessageLength = pu - q;        
  if  (remainingMessageLength <= 0) return 0;
                                            // Overwrite tag so located with
  memmove(p, q, remainingMessageLength + 1);// balance of msg plus null term 
  pu = p + remainingMessageLength;          // Adjust buffer pointer
  ubuflen = (pu - ubuf);                    // Adjust buffer content length

  const int charactersRemoved = (q - p);
  return charactersRemoved;                         
}



int MmsAppMessage::backupToEndTag()         // Back write pointer to </message> 
{
  char* p  = strstr(ubuf, MMS_MSG_END_TAG); 
  if   (p == NULL) return -1;                                            
  pu  = p;                     
  return 0;                                  
}



int MmsAppMessage::isMessage(const int messageid)
{
  return (messageid < 0 || messageid > MmsAppMessageX::MESSAGE_COUNT)? 0:
          0 == strcmp(this->commandName(), MmsAppMessageX::messagenames[messageid]);
}



MmsAppMessage::~MmsAppMessage()             // Dtor
{
  
}



void MmsAppMessage::init()                  // Ctor object initialization
{
  this->sd = MmsAppMessageX::instance();      
  memset(ubuf,0,sizeof(ubuf));
  memset(wbuf,0,sizeof(wbuf));
  memset(cmdbuf, 0, sizeof(cmdbuf));
  memset(valbuf, 0, sizeof(valbuf));
  memset(tagbuf, 0, sizeof(tagbuf));
  pu = pparams = ubuf; 
  pw = wbuf; 
  hqDestination= NULL;
  pattrvalue   = NULL; 
  paramlength  = ubuflen = commandno = termcondno = 0;
  serverid     = -1;
}






