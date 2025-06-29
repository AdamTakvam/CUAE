//
// MmsMqAppAdapter.cpp 
//
// App server msmq protocol adapter
//
// Command parameter parsers
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMqAppAdapter.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

#define MAX_EXT_LENGTH 16
static char* VOX = "vox", *WAV = "wav", *TTS = "tts";


int MmsMqAppAdapter::stripServerID(const int id, MmsServerCmdHeader& commandHeader)
{
  // Strip the server ID from a connection ID or conference ID, and place  
  // the extracted server ID into the supplied parameter map command header.

  const static unsigned int mask = 0xff000000;
  const int tentativeServerID = (id & mask) >> 24;

  if  (tentativeServerID > commandHeader.serverID)
       commandHeader.serverID = tentativeServerID;

  return id & ~mask;
}
  


int MmsMqAppAdapter::insertServerID(const int id, const int serverID)
{
  // Insert server ID to the high byte of supplied ID 
  return id | ((serverID & 0xff) << 24);
} 




int MmsMqAppAdapter::insertServerIdExcludeZero(const int id, const int serverID)
{
  // Insert server ID to the high byte of supplied ID 
  return (serverID < 1)?  id: id | ((serverID & 0xff) << 24);
}



int MmsMqAppAdapter::insertServerID(char* flatmap)
{
  // When we were not passed serverID as part of a connectionID or conferenceID,
  // we'll need to retrieve it and append it to the returned connection ID. This
  // is the case for a half connect command, since no connection ID is specified.

  if (!flatmap) return 0;
  int serverID = 0;
  const int clientID = (int) getFlatmapClientHandle(flatmap);
  if (clientID) serverID = this->getServerIdFromClientId(clientID);

  if (serverID)
  {   // Insert server ID to the high byte of connection ID 
      int conxID = getFlatmapConnectionID(flatmap);
      conxID = this->insertServerID(conxID, serverID);
      setFlatmapConnectionID(flatmap, conxID);
      setFlatmapServerID(flatmap, serverID);
  } 

  return serverID;  
}



int MmsMqAppAdapter::buildConnectionParameters(MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{
  // Parse XML for <field name="connectionAttribute">name value</field> entries. 
  // For any such attribute present, convert and mask the specified attribute 
  // into the remote or local attributes doubleword. When done, if remote and/or
  // local attributes are present, write a MMSP_REMOTE_CONX_ATTRIBUTES and/or
  // MMSP_LOCAL_CONX_ATTRIBUTES parameter to the media server parameter map.

  unsigned int remoteConxAttrs=0, localConxAttrs=0, altCoder=0, isError=0;              

  char* searchstart = xmlmsg->firstparam();
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ATTRIBUTE];

  while(1)                                  // Find each connection attribute
  {                                         // supplied in the XML message
    char* bufpos  = xmlmsg->getvaluefor(paramname, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 
    isError       = 0;

    int  attrno = xmlmsg->getConnectionAttribute();
    if  (attrno == -1) isError = TRUE;      // Bad attribute name
    if  (isError) break;
    
    char* szValue  = xmlmsg->attrValue();
    int   valuelen = ACE_OS::strlen(szValue);

    int   nValue=0, isLocal=0, coderType=0, framesize=0, isAsciiZeros=0, dfd=0; 
    static char* G711ULAW = "g711ulaw", *G711ALAW = "g711alaw";
    static int codermin = sizeof("g711u")-1;   
                                            
    switch(attrno)                           // Translate XML attribute value
    {                                        // to media server attribute value
      // String or numeric representations of coder type may be used
      // Metreos app server does not use string form; however our test client may.

      case MmsAppMessageX::CODER_TYPE_LOCAL: // "coderTypeLocal xxxxxxxxxx"
           isLocal = TRUE;                   // Fall thru ...     
      case MmsAppMessageX::CODER_TYPE_REMOTE:// "coderTypeRemote xxxxxxxxxx"
                                             // Check if numeric coder type 
           if  (this->editNumericCoderType(szValue, coderType, altCoder, isLocal) == -1)
                isError = TRUE;
           else
           if  (coderType > 0);              // Coder type is numeric and OK?
           else
           if  (valuelen < codermin)         // Coder specified as string?
                isError = TRUE;              
           else                              
           if  (memcmp(szValue,G711ULAW,codermin) == 0)
                coderType = MMS_CODER_G711ULAW64K;
           else
           if  (memcmp(szValue,G711ALAW,codermin) == 0)
                coderType = MMS_CODER_G711ALAW64K;
           else isError = TRUE;
            
           if  (isError) break; 
                
           if  (isLocal)
                localConxAttrs  |= coderType;
           else remoteConxAttrs |= coderType;
           break;

                     
      // Framesize specification is codec-specific. Since parameters can appear
      // in any order, we cannot assume we know what the codec is at this point
      // so we may not know here whether the specified framesize is valid for 
      // the coder. We do know the universe of frame sizes valid for all HMP
      // coders, so we can edit to that granularity.

      case MmsAppMessageX::FRAMESIZE_LOCAL: // "framesizeLocal nn"
           isLocal = TRUE;                  // Fall thru ...     
      case MmsAppMessageX::FRAMESIZE_REMOTE:// "framesizeRemote nn"
      case MmsAppMessageX::FRAMESIZE:       // "framesize nn"
     
           nValue = ACE_OS::atoi(szValue);

           switch(nValue)
           { case 10: framesize = MMS_CODER_G711_FRAMESIZE_10; break;
             case 20: framesize = MMS_CODER_FRAMESIZE_20;      break;
             case 30: framesize = MMS_CODER_FRAMESIZE_30;      break;
             case 40: framesize = MMS_CODER_G729_FRAMESIZE_40; break;
             case 60: framesize = MMS_CODER_G723_FRAMESIZE_60; break;
             default: isError = TRUE;
           }
           
           if  (isError) break;                 
           
           if  (isLocal)
                localConxAttrs  |= framesize;
           else remoteConxAttrs |= framesize;
           break;


      case MmsAppMessageX::VADENABLE_LOCAL: // "vadEnableLocal n"
           isLocal = TRUE;                  // Fall thru ...     
      case MmsAppMessageX::VADENABLE_REMOTE:// "vadEnableRemote n"
      case MmsAppMessageX::VADENABLE:       // "vadEnable n"

           switch(nValue = ACE_OS::atoi(szValue))
           { case 1:                        // 1 means enable
                  break; 
                   
             case 0:                        
                  isAsciiZeros = this->isZeros(xmlmsg->attrValue());
                  if  (valuelen == 0)       
                       nValue = 1;          // Empty means enable
                  else                      
                  if  (!isAsciiZeros)       // Ensure '0' was specified, 
                       isError = TRUE;      // not some non-digit string
                  break;

             default:  isError = TRUE;
           }
           
           if  (isError)     break;       
           if  (nValue != 1) break;               
           
           if  (isLocal)
                localConxAttrs  |= MMS_CODER_VAD_ENABLE;
           else remoteConxAttrs |= MMS_CODER_VAD_ENABLE;
           break;

     
      case MmsAppMessageX::DATAFLOW_DIRECTION:       

           for(dfd=0; dfd < MmsAppMessageX::DATAFLOW_DIRECTION_COUNT; dfd++)
               if  (ACE_OS::strcmp(szValue, MmsAppMessageX::dataflowDirectionNames[dfd]) == 0)
                    break;

           switch(dfd)
           { case MmsAppMessageX::IP_RECEIVEONLY:   dfd = MMS_DIRECTION_IPRO; break;
             case MmsAppMessageX::IP_SENDONLY:      dfd = MMS_DIRECTION_IPSO; break;
             case MmsAppMessageX::MULTICAST_SERVER: dfd = MMS_DIRECTION_MCS;  break;
             case MmsAppMessageX::MULTICAST_CLIENT: dfd = MMS_DIRECTION_MCC;  break;
             case MmsAppMessageX::IP_BIDIRECTIONAL:
             default: dfd = MMS_DIRECTION_IPBI; 
           }
          
           MMS_DATAFLOW_DIRECTION_PUT(remoteConxAttrs, dfd);
 
           break;
       
    }    // switch(attrno)
 
    if  (isError) break;

  } // while(1)


  if  (isError) return MMS_ERROR_PARAMETER_VALUE;

  if  (remoteConxAttrs)                     // If any remote attrs, write param
       map.insert(MMSP_REMOTE_CONX_ATTRIBUTES, (int)remoteConxAttrs);

  if  (localConxAttrs)                      // If any local attrs, write param
       map.insert(MMSP_LOCAL_CONX_ATTRIBUTES,  (int)localConxAttrs);

  if  (altCoder)                            // If alternate coder, write param
       map.insert(MMSP_ALTERNATE_CODER,  (int)altCoder);

  return 0;
}



int MmsMqAppAdapter::buildTerminationConditionParameters(MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{
  // Parse XML for <field name="terminationCondition">name value</field> entries. 
  // Build termination conditions map as a map of termination condition maps. 
  // Embed termination conditions map into the media server parameter map.

  MmsFlatMapWriter termcondsMap;

  char* searchstart = xmlmsg->firstparam();
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];

  while(1)                                  // Find each termination condition
  {                                         // supplied in the XML message
    char* bufpos  = xmlmsg->getvaluefor(paramname, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 

    int  tcno = xmlmsg->getTerminationCondition();
    if  (tcno == -1)  
    {    MMSLOG((LM_INFO,"%s bad term cond = '%s'\n",taskName,xmlmsg->paramValue()));
         continue;
    }
    
    MmsFlatMapWriter condMap(200); 

    char* pval = xmlmsg->termcondValue();
    MmsParameterValue val(pval);            // Parse the parameter value parts

    int  nvalue = val.ival(0),  nvalu2 = 0;
    int  tcType = 0, tcLen = 0, tcVal1 = 0, tcVal2 = 0;
    int  dsecs  = val.dsecs(nvalue);        // Dialogic wants time in tenths
                                            // Translate XML term condition
    switch(tcno)                            // to media server term condition
    { case MmsAppMessageX::DIGIT:       tcType = DX_DIGTYPE;  tcLen = *pval;  break;
      case MmsAppMessageX::MAXTIME:     tcType = DX_MAXTIME;  tcLen = dsecs;  break;
      case MmsAppMessageX::MAXDIGITS:   tcType = DX_MAXDTMF;  tcLen = nvalue; break;
      case MmsAppMessageX::DIGITDELAY:  tcType = DX_IDDTIME;  tcLen = dsecs;  break;                
      case MmsAppMessageX::DIGITLIST:   tcType = DX_DIGMASK;                  break; 
      case MmsAppMessageX::DIGITPATTERN:tcType = DX_METREOS_DIGPATTERN;       break;
      case MmsAppMessageX::NONSILENCE:  tcType = DX_MAXNOSIL; tcLen = dsecs;  break;
      case MmsAppMessageX::SILENCE: 
           // Silence termination parameter value can be in two parts n:m
           // where n is initial silence length, m is subsequent silence length; 
           // or simply in one part, where the length indicates silence anywhere
           tcType = DX_MAXSIL; 
           nvalu2 = val.ival(1);            // Is there a second parameter part?
           tcLen  = nvalu2? val.dsecs(nvalu2): dsecs;
           tcVal2 = nvalu2? dsecs: 0;       // If so, tp_data is initial silence
    } 
  
    if  (tcType == 0) continue;

    condMap.insert(MMSP_TERMINATION_CONDITION_TYPE,   tcType); 
    condMap.insert(MMSP_TERMINATION_CONDITION_LENGTH, tcLen);   
    condMap.insert(MMSP_TERMINATION_CONDITION_DATA1,  tcVal1); 
    condMap.insert(MMSP_TERMINATION_CONDITION_DATA2,  tcVal2);

    switch(tcType)
    {
      // Format and write character string termination parameters. 
      // Currently these are limited to lists of 1 to 15 digits
      case DX_DIGMASK:
      case DX_METREOS_DIGPATTERN:
         { char* digitlist = xmlmsg->termcondValue();    
           int   listlen   = ACE_OS::strlen(digitlist); 
           if (listlen < 1 || listlen > MAXDIGITPATTERNSIZE) break;
           condMap.insert(MMSP_DIGITLIST, MmsFlatMap::STRING, listlen+1, digitlist);
         }
    }
      
    const int condmaplen = condMap.length();

    char* p = termcondsMap.format           // Embed termcond into termconds
             (MMSP_TERMINATION_CONDITION, MmsFlatMap::FLATMAP, condmaplen); 

    condMap.marshal(p);
  } // while(1)


  if  (termcondsMap.size())                 // Embed termconds map into main map
  {
       const int len = termcondsMap.length();
       char* p = map.format(MMSP_TERMINATION_CONDITIONS, MmsFlatMap::FLATMAP, len);
       termcondsMap.marshal(p);
  }

  return termcondsMap.size();   
}



int MmsMqAppAdapter::buildFileListParameters
( MmsFlatMapWriter& map, MmsAppMessage* xmlmsg, MmsLocaleParams& ld, int command)
{
  // Parse XML for <field name="filename--">name</field> entries. Build filelist map
  // as a map of filespec maps. Embed filelist map into main map.

  MmsFlatMapWriter filelistMap;
  char* searchstart = xmlmsg->firstparam();
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME];

  while(1)
  {                                         // Find <field name="filename ...
    char* bufpos  = xmlmsg->findparam(paramname, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 
                                            // Isolate file path part  
    const int mapPathlen = xmlmsg->isolateParameterValue(bufpos); 
    char* filenamepart = NULL, *extensionpart = NULL;
    int   extensionlen = 0;
                                            // Isolate file extension     
    char* p = xmlmsg->paramValue(), *q = p + mapPathlen, *path = p; 
    if (ACE_OS::strlen(p) > 2)
    {
        while((q > p) && (*q != '.') && (*q != '\\') && (*q != '/')) q--; 
        extensionpart = ((*q == '.') && (q > p) && ACE_OS::strlen(q) > 1)? 
                           q + 1: NULL;
    }  

    if (extensionpart)                      // Isolate file name.ext
    {                                       // (strip path info)
        while((q > p)  && (*q != '\\') && (*q != '/')) q--; 
        filenamepart = (q > p)? q++: p;
        extensionlen = ACE_OS::strlen(extensionpart);
    }
                                            
    MmsFlatMapWriter filespecMap(256); 

    // Note that record path may be a (possibly empty) path with no filename.
    // If a record, and a filename was not supplied, we need to format space
    // in the file path buffer to return the recorded file name; otherwise
    // if filename was supplied, or this is a play command, copy path as is.

    if  (command == COMMANDTYPE_RECORD)      
                                             
         if  (filenamepart && *filenamepart)// Record filename specified 
              filespecMap.insert(MMSP_FILENAME, MmsFlatMap::STRING, 
                                 mapPathlen+1, xmlmsg->paramValue());
                                            // Not specified: assign a name
         else this->assignFilename(filespecMap);                                                      
    else                                    // Not record, insert play file path
    {    filespecMap.insert(MMSP_FILENAME, MmsFlatMap::STRING, 
                            mapPathlen+1, xmlmsg->paramValue());
                                            // If TTS string, flag as such
         if (this->isTtsRequest(path, extensionpart, extensionlen, ld))
             filespecMap.insert(MMSP_FILENAME_IS_TTSTEXT, TRUE);                                            
    }

                                            // Embed filespec map to filelist map 
    p = filelistMap.format(MMSP_FILESPEC, MmsFlatMap::FLATMAP, filespecMap.length()); 
    filespecMap.marshal(p);
  } // while(1)

                                            // If no filespec specified ...
  if ((filelistMap.size() == 0) && (command == COMMANDTYPE_RECORD))
  {                                         // ... allocate one, in order
      MmsFlatMapWriter filespecMap(256);    // to return the record filename

      this->assignFilename(filespecMap);    // ... assign name part now (MMS-20)
                                            // ... and embed in filelist
      char* p = filelistMap.format(MMSP_FILESPEC, 
                MmsFlatMap::FLATMAP, filespecMap.length()); 
      filespecMap.marshal(p);
  }


  if  (filelistMap.size() > 0)              // Embed filelist map into main map
  {    char* p = map.format(MMSP_FILELIST, MmsFlatMap::FLATMAP, filelistMap.length());
       filelistMap.marshal(p);
  }

  return filelistMap.size();
}



int MmsMqAppAdapter::buildGrammarListParameters(MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{
  // Parse XML for <field name="grammarname--">name</field> entries. Build filelist map
  // as a map of filespec maps. Embed filelist map into main map.

  MmsFlatMapWriter filelistMap;
  char* searchstart = xmlmsg->firstparam();
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::GRAMMAR_NAME];

  while(1)
  {                                         // Find <field name="grammarname ...
    char* bufpos  = xmlmsg->findparam(paramname, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 
                                            // Isolate file path part  
    const int mapPathlen = xmlmsg->isolateParameterValue(bufpos); 
    char* filenamepart = NULL, *extensionpart = NULL;
    int   extensionlen = 0;
                                            // Isolate file extension     
    char* p = xmlmsg->paramValue(), *q = p + mapPathlen, *path = p; 
    if (ACE_OS::strlen(p) > 2)
    {
        while((q > p) && (*q != '.') && (*q != '\\') && (*q != '/')) q--; 
        extensionpart = ((*q == '.') && (q > p) && ACE_OS::strlen(q) > 1)? 
                           q + 1: NULL;
    }  

    if (extensionpart)                      // Isolate file name.ext
    {                                       // (strip path info)
        while((q > p)  && (*q != '\\') && (*q != '/')) q--; 
        filenamepart = (q > p)? q++: p;
        extensionlen = ACE_OS::strlen(extensionpart);
    }
                                            
    MmsFlatMapWriter filespecMap(256); 

    filespecMap.insert(MMSP_GRAMMARNAME, MmsFlatMap::STRING, mapPathlen+1, xmlmsg->paramValue());

                                            // Embed filespec map to filelist map 
    p = filelistMap.format(MMSP_GRAMMARSPEC, MmsFlatMap::FLATMAP, filespecMap.length()); 
    filespecMap.marshal(p);
  } // while(1)

  if (filelistMap.size() > 0)              // Embed filelist map into main map
  {   char* p = map.format(MMSP_GRAMMARLIST, MmsFlatMap::FLATMAP, filelistMap.length());
      filelistMap.marshal(p);
  }

  return filelistMap.size();
}


#undef  MMS_TESTING_LOCALE_PARAMETERS
#ifndef MMS_TESTING_LOCALE_PARAMETERS



int MmsMqAppAdapter::buildLocaleDirectoryParameters
(MmsFlatMapWriter& map, MmsAppMessage* xmlmsg, MmsLocaleParams& ld)
{
  // Parse XML for <field name="appName">value</field> and <field name="locale">value</field>
  // entries. For any such parameter present, write a MMSP_APP_NAME or MMSP_LOCALE 
  // parameter to the media server parameter map.

  char* searchstart = xmlmsg->firstparam();
  const static char* appname = MmsAppMessageX::paramnames[MmsAppMessageX::APP_NAME];
  const static char* locale  = MmsAppMessageX::paramnames[MmsAppMessageX::LOCALE];
  const static char* defaultLocale = "en-US";
  const static int   defaultLocaleLength = strlen(defaultLocale) + 1;
  int   appnameFound = 0, paramErrors = 0, result = 0;

  if  (xmlmsg->getvaluefor(appname)) 
  {                                         // Insert appName parameter   
       char* paramval = xmlmsg->paramValue(); 
       if (!paramval || !*paramval)
            paramErrors++;                       
       else
       {    map.insert(MMSP_APP_NAME, MmsFlatMap::STRING, xmlmsg->paramLength()+1, paramval); 
            ld.setAppName(paramval);
            appnameFound = TRUE;
       }
  }
  else // Default app name config is primarily for MMS debugging -- it is not published
  if  (*config->serverParams.defaultAppName)
  {
       char* paramval = config->serverParams.defaultAppName;
       map.insert(MMSP_APP_NAME, MmsFlatMap::STRING, strlen(paramval)+1, paramval);
       ld.setAppName(paramval);
       appnameFound = TRUE; 
  }


  if  (xmlmsg->getvaluefor(locale)) 
  {                                         // Insert locale parameter
       char* paramval = xmlmsg->paramValue();

       if (!paramval || !*paramval)
       {    map.insert(MMSP_LOCALE, MmsFlatMap::STRING, defaultLocaleLength, defaultLocale); 
            ld.setLocale((char*)defaultLocale);
       }                       
       else 
       {    map.insert(MMSP_LOCALE, MmsFlatMap::STRING, xmlmsg->paramLength()+1, paramval); 
            ld.setLocale(paramval);
       }        
  }
  else
  if  (*config->serverParams.defaultLocale) // We use a default locale if not specified
  {
       char* paramval = config->serverParams.defaultLocale;
       map.insert(MMSP_LOCALE, MmsFlatMap::STRING, strlen(paramval)+1, paramval);
       ld.setLocale(paramval);
       appnameFound = TRUE; 
  }                                         // Default default locale directory
  else 
  {    map.insert(MMSP_LOCALE, MmsFlatMap::STRING, defaultLocaleLength, defaultLocale); 
       ld.setLocale((char*)defaultLocale);
  }
  
  if  (config->serverParams.disregardLocaleDirectories)
       result = 0;
  else
  if  (paramErrors)
       result = MMS_ERROR_PARAMETER_VALUE;
  else
  if  (!appnameFound) 
       result = MMS_ERROR_TOO_FEW_PARAMETERS;

  return result;
}



#else // #ifndef MMS_TESTING_LOCALE_PARAMETERS


int MmsMqAppAdapter::buildLocaleDirectoryParameters
(MmsFlatMapWriter& map, MmsAppMessage* xmlmsg, MmsLocaleParams& ld)
{
  // Compile this method in place of the offical method above, when we want to 
  // test possibilities of various application name and locale parameters 
  // arriving from client (app server) XML.

  char* testAppName = "unknownAppName", *testLocale = "xx";
 
  map.insert(MMSP_APP_NAME, MmsFlatMap::STRING, strlen(testAppName)+1, testAppName); 
  ld.setAppName(testAppName);      

  map.insert(MMSP_LOCALE, MmsFlatMap::STRING, strlen(testLocale)+1, testLocale); 
  ld.setLocale(testLocale);    

  return 0;
}


#endif // #ifndef MMS_TESTING_LOCALE_PARAMETERS




int MmsMqAppAdapter::isTtsRequest(char* path, char* ext, const int extlen, MmsLocaleParams& ld)
{
  // Indicate if "path" string is in actuality a string to be rendered to speech

  int isTtsRequest = FALSE;

  // Are we using file system to differentiate wav paths from tts strings?
  if  (this->config->media.ttsIsPathStrategy == 0)                            
       isTtsRequest = !this->isExistingMediaFile(path, ld); 
       
  else // When not using file system, identify TTS strings as those with no          
       // apparent file extension. defined as a dot followed by 1-16 characters.
  if  (ext == NULL || extlen == 0 || extlen > MAX_EXT_LENGTH)
       isTtsRequest = TRUE;

  return isTtsRequest;
}



void MmsMqAppAdapter::assignFilename(MmsFlatMapWriter& filespecMap)
{
  // Assign an 8-character record filename when none was supplied
  // Indicate via MMSP_FILENAME_IS_ASSIGNED that this is the case

  char buf[MMS_RECORD_FILENAME_BUFFERSIZE];
  Mms::createFilename(buf);

  filespecMap.insert(MMSP_FILENAME, MmsFlatMap::STRING, 
          MMS_RECORD_FILENAME_BUFFERSIZE, buf);

  filespecMap.insert(MMSP_FILENAME_IS_ASSIGNED, TRUE);
  filespecMap.insert(MMSP_FILENAME_IS_TTSTEXT,  FALSE);
}



void MmsMqAppAdapter::insertFilename(MmsAppMessage* outxml, char* flatmap)
{
  // Insert namne of recording file into xml destined for return to client

  MmsFlatMapReader map(flatmap);            
  char* pfilelistmap = NULL;   
                
  map.find(MMSP_FILELIST, &pfilelistmap);

  if  (pfilelistmap) 
  {
       MmsFlatMapReader filelistMap(pfilelistmap);   
     
       char* pfilespecmap = NULL;
       filelistMap.find(MMSP_FILESPEC, &pfilespecmap);

       if  (pfilespecmap)  
       {
            MmsFlatMapReader filespecMap(pfilespecmap);

            char* pathstring = NULL;
            filespecMap.find(MMSP_FILENAME, &pathstring);

            if  (pathstring)  
                 outxml->putParameter(MmsAppMessageX::FILE_NAME, pathstring); 
       }       
  } 
}



int MmsMqAppAdapter::buildAudioFileAttributeParameters
( MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{
  // Parse XML for <field name="audioFileAttribute">name value</field> entries. 
  // For any such attribute present, convert and mask the specified attribute 
  // into a media server play/record attributes doubleword. When done, if  
  // attributes are present, write a MMSP_PLAY_RECORD_ATTRIBUTES parameter 
  // to the media server parameter map.

  unsigned int playrecattrs = 0, isError = 0;
  char* searchstart = xmlmsg->firstparam();
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_FILE_ATTRIBUTE];

  while(1)                                  // Find each audio file attribute
  {                                         // supplied in the XML message
    char* bufpos  = xmlmsg->getvaluefor(paramname, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 
    isError       = 0;

    int  attrno = xmlmsg->getAudioFileAttribute();
    if  (attrno == -1) isError = TRUE;      // Bad attribute name
    if  (isError) break;
    
    static char* ULAW = "ulaw", *ALAW = "alaw", *ADPCM = "adpcm", *PCM = "pcm";
    char* szValue = xmlmsg->attrValue();
    int   nValue  = 0;
                                            
    switch(attrno)                          // Translate XML attribute value
    {                                       // to media server attribute value
      case MmsAppMessageX::FORMAT:          // "format xxx"
                                    
           if  (ACE_OS::strcmp(szValue,VOX) == 0)
                playrecattrs |= MMS_FILETYPE_VOX;
           else
           if  (ACE_OS::strcmp(szValue,WAV) == 0)
                playrecattrs |= MMS_FILETYPE_WAV;
           else isError = TRUE;
           break;

      case MmsAppMessageX::ENCODING:        // "encoding xxxxx"
                            
           if  (ACE_OS::strcmp(szValue,ULAW)  == 0)
                playrecattrs |= MMS_FORMAT_MULAW;
           else
           if  (ACE_OS::strcmp(szValue,ALAW)  == 0)
                playrecattrs |= MMS_FORMAT_ALAW;
           else
           if  (ACE_OS::strcmp(szValue,ADPCM) == 0)
                playrecattrs |= MMS_FORMAT_ADPCM;
           else
           if  (ACE_OS::strcmp(szValue,PCM)   == 0)
                playrecattrs |= MMS_FORMAT_PCM;
           else isError = TRUE;           
           break;
                     
      case MmsAppMessageX::BITRATE:         // "bitrate nn"

           switch(nValue = ACE_OS::atoi(szValue))
           { case 6:  playrecattrs |=  MMS_RATE_KHZ_6;  break;
             case 8:  playrecattrs |=  MMS_RATE_KHZ_8;  break;
             case 11: playrecattrs |=  MMS_RATE_KHZ_11; break;
             default: isError = TRUE;
           }
           break;

      case MmsAppMessageX::SAMPLESIZE:         // "samplesize nn"
           switch(nValue = ACE_OS::atoi(szValue))
           { case 4:  playrecattrs |=  MMS_SAMPLESIZE_BIT_4;  break;
             case 8:  playrecattrs |=  MMS_SAMPLESIZE_BIT_8;  break;
             case 16: playrecattrs |=  MMS_SAMPLESIZE_BIT_16; break;
             default: isError = TRUE;
           }
           break;
    }    
 
    if  (isError) break;
  }     // while(1)

  if  (isError) return MMS_ERROR_PARAMETER_VALUE;

  if  (playrecattrs)
       map.insert(MMSP_PLAY_RECORD_ATTRIBUTES, (int)playrecattrs);

  return 0;
}



int MmsMqAppAdapter::buildAudioToneAttributeParameters
( MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{
  // Parse XML for <field name="audioToneAttribute">name value</field> entries. 
  // If attributes are present, write one or two MMSP_FREQUENCY_AMPLITUDE  
  // parameters to the media server parameter map.

  unsigned int f1 = 0, f2 = 0, a1 = 0, a2 = 0, d = 0, isError = 0;
  unsigned int is1 = 0, isF1 = 0, isF2 = 0, is2 = 0, isA1 = 0, isA2 = 0, isD = 0;
  char* searchstart = xmlmsg->firstparam();
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::AUDIO_TONE_ATTRIBUTE];

  while(1)                                  // Find each audio tone attribute
  {                                         // supplied in the XML message
    char* bufpos  = xmlmsg->getvaluefor(paramname, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 
    isError       = 0;

    int  attrno = xmlmsg->getAudioToneAttribute();
    if  (attrno == -1) isError = TRUE;      // Bad attribute name
    if  (isError) break;
    
    char* szValue = xmlmsg->attrValue();
    int   nValue  = ACE_OS::atoi(szValue);
                                            
    switch(attrno)                          // Translate XML attribute value
    {                                       // to media server attribute value
      case MmsAppMessageX::FREQUENCY1:              // "frequency1 nnnn"
                      
           isF1 = TRUE;              
           f1   = nValue;                  
           break;

      case MmsAppMessageX::AMPLITUDE1:              // "amplitude1 nnnn"
               
           isA1 = TRUE;            
           a1   = nValue;           
           break;   

      case MmsAppMessageX::FREQUENCY2:              // "frequency2 nnnn"
                      
           isF2 = TRUE;              
           f2   = nValue;                  
           break;

      case MmsAppMessageX::AMPLITUDE2:              // "amplitude2 nnnn"
               
           isA2 = TRUE;            
           a2   = nValue;           
           break;
                     
      case MmsAppMessageX::DURATION:                // "duration nn" (in ms)

           isD = TRUE;                       
           if  (nValue < (-1))                    
                isError = TRUE;
           else                             // If client specifies 0 or -1
           if  (nValue < 1)                 // ... set infinite duration for hmp
                d = (-1);                   // ... else round to 10ms units for hmp
           else d = (nValue / 10) + ((nValue % 10) >= 5);
           break;
    }    
 
    if  (isError) break;
  }     // while(1)

  if  (isError) return MMS_ERROR_PARAMETER_VALUE;

  if  (isF1 || isA1) is1 = TRUE; 
  if  (isF2 || isA2) is2 = TRUE; 
                                            // If 1st tone component not specified
  if (!is1) is1 = TRUE;                     // ... default the first component
  
  if  (is1)                                 // If 1st tone component specified ...
  {                                         // ... write it to parameter map
      MmsFlatMapWriter faMap(128); 
      if (f1 == 0) f1 = this->config->media.defaultToneFrequency;
      faMap.insert(MMSP_FREQUENCY, (int)f1);
      faMap.insert(MMSP_AMPLITUDE, (int)a1);
      char* buf = map.format(MMSP_FREQUENCY_AMPLITUDE, MmsFlatMap::FLATMAP, faMap.length()); 
      faMap.marshal(buf);
  }

  if  (is2)
  {                                         // If 2nd tone component specified ...
      MmsFlatMapWriter faMap(128);          // ... write it to parameter map
      if (f2 == 0) f2 = this->config->media.defaultToneFrequency;
      faMap.insert(MMSP_FREQUENCY, (int)f2);
      faMap.insert(MMSP_AMPLITUDE, (int)a2);
      char* buf = map.format(MMSP_FREQUENCY_AMPLITUDE, MmsFlatMap::FLATMAP, faMap.length()); 
      faMap.marshal(buf);
  }
                                            
  if (isD)                                  // If tone duration specified ...
      map.insert(MMSP_DURATION, (int)d);    // ... write it to parameter map

  return 0;
}

  

int MmsMqAppAdapter::buildCallStateParameters(MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{
  char* bufpos  = xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::CALL_STATE]);
  if   (bufpos == NULL) return MMS_ERROR_TOO_FEW_PARAMETERS;
             
  // Retrieve call state name and value, e.g. "silence 30000"

  if  (-1 == xmlmsg->findNameValuePairDelimiters()) return MMS_ERROR_PARAMETER_VALUE;  

  for(int i=0; i < MmsAppMessageX::CALL_STATE_COUNT; i++)     
      if  (stricmp(xmlmsg->paramValue(), MmsAppMessageX::callStateNames[i]) == 0) break;

  if (i >= MmsAppMessageX::CALL_STATE_COUNT) return MMS_ERROR_PARAMETER_VALUE; 
  const int callStateID = i+1;

  const char* szValue = xmlmsg->attrValue();
  const int valuelen  = szValue? ACE_OS::strlen(szValue): 0;
  const int duration  = valuelen > 0? atoi(szValue): 0;

  map.insert(MMSP_CALL_STATE, callStateID);
  map.insert(MMSP_CALL_STATE_DURATION, duration);

  return 0;
}



int MmsMqAppAdapter::buildAdjustPlayParameters
( MmsFlatMapWriter& map, MmsAppMessage* xmlmsg, const int isRequired)
{
  // adjustPlay command permits adjustment of volume and/or speed of playback. 
  // Its parameters correspond closely to those of the HMP voice API's dx_adjsv. 
  // volume: if specified, must be between 10 and +10.
  // speed:  ditto.
  // adjustmentType: "abs", "rel", or "tog". 
  //               corresponding to SV_ABSPOS, SV_RELCURPOS, SV_TOGGLE, of dx_adjsv
  // toggleType:   0, 1, 2, or 3, corresponding to, and the same as, dx_adjsv's  
  //               SV_TOGORIGIN, SV_CURORIGIN, SV_CURLASTMOD, SV_RESETORIG

  char* volbufpos  = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::VOLUME]);
  char* spdbufpos  = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::SPEED]);

  if (volbufpos == NULL && spdbufpos == NULL) 
      return isRequired? MMS_ERROR_TOO_FEW_PARAMETERS: 0;

  int volume = 0, speed = 0;

  if (volbufpos)
      if (-1 == this->editVolumeSpeedParameter(xmlmsg, volbufpos, &volume))
          return MMS_ERROR_PARAMETER_VALUE;

  if (spdbufpos)
      if (-1 == this->editVolumeSpeedParameter(xmlmsg, spdbufpos, &speed))
          return MMS_ERROR_PARAMETER_VALUE;
    
  int adjustmentType = MMS_ADJTYPE_NONE;

  if (xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::ADJUST_TYPE]))
  {
      char* pval = xmlmsg->paramValue();

      if (stricmp(pval, MMSCLIENT_ADJTYPE_ABSOLUTE) == 0) 
          adjustmentType = MMS_ADJTYPE_ABSOLUTE; 
      else
      if (stricmp(pval, MMSCLIENT_ADJTYPE_RELATIVE) == 0) 
          adjustmentType = MMS_ADJTYPE_RELATIVE; 
      else
      if (stricmp(pval, MMSCLIENT_ADJTYPE_TOGGLE)   == 0) 
          adjustmentType = MMS_ADJTYPE_TOGGLE; 
  }
      
  int toggleType = MMS_TOGTYPE_ORG_CUR;

  if (adjustmentType == MMS_ADJTYPE_TOGGLE)
  {
      if (xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::TOGGLE_TYPE]))
      {   
          const int paramValue = atoi(xmlmsg->paramValue());

          switch(paramValue)
          { case MMS_TOGTYPE_ORG_CUR:
            case MMS_TOGTYPE_TO_CURORG:
            case MMS_TOGTYPE_LASTMOD:
            case MMS_TOGTYPE_RESETORG:
                 toggleType = paramValue;
                 break;
            default: return MMS_ERROR_PARAMETER_VALUE;
          }
      }
  }

  if (volbufpos)
      map.insert(MMSP_VOLUME, volume);

  if (spdbufpos)
      map.insert(MMSP_SPEED, speed);

  if (adjustmentType != MMS_ADJTYPE_NONE)
      map.insert(MMSP_ADJUSTMENT_TYPE, adjustmentType);

  if (adjustmentType == MMS_ADJTYPE_TOGGLE)
      map.insert(MMSP_TOGGLE_TYPE, toggleType);

  return 0;
}



int MmsMqAppAdapter::editVolumeSpeedParameter(MmsAppMessage* xmlmsg, char* bufpos, int* paramval)
{
  // Validate volume or speed parameter value, either of which must be in range -10 <= v <= +10
  const int paramlength = xmlmsg->isolateParameterValue(bufpos);
  if (paramlength == 0 || paramval == NULL) return -1;
  *paramval = atoi(bufpos);
  return (*paramval < -10 || *paramval > 10)? -1: 0;
}



int MmsMqAppAdapter::buildConferenceParameters(MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{
  unsigned int flags = 0, isParameters = 0; // Conference attributes                 
  if  (buildConferenceAttributeParameters(map, xmlmsg, &flags))
       isParameters = TRUE;
                                            // Conferee attributes
  if  (buildConfereeAttributeParameters(map, xmlmsg, &flags))
       isParameters = TRUE;

  int   hairpin = 0, hairpinPromote = 0, value = 0;
  char* phairpin = xmlmsg->findparam 
       (MmsAppMessageX::paramnames[MmsAppMessageX::HAIRPIN]);
  char* phairpinPromote = xmlmsg->findparam
       (MmsAppMessageX::paramnames[MmsAppMessageX::HAIRPIN_PROMOTE]);

  if (phairpin)
  {   xmlmsg->isolateParameterValue(phairpin);
      value = ACE_OS::atoi(xmlmsg->paramValue());
      map.insert(MMSP_HAIRPIN, value);
  }

  if (phairpinPromote)
  {   xmlmsg->isolateParameterValue(phairpinPromote);
      value = ACE_OS::atoi(xmlmsg->paramValue());
      map.insert(MMSP_HAIRPIN_PROMOTE, value);
  }

  return flags & EDIT_ERROR? -1: isParameters? 1: 0;
}



int MmsMqAppAdapter::buildConferenceAttributeParameters
( MmsFlatMapWriter& map, MmsAppMessage* xmlmsg, unsigned int* editflags)
{
  // Parse XML for <field name="conferenceAttribute">value</field> entries. 
  // Translate XML attributes to media server attribute bitmap.

  unsigned int attributes = 0; 
                                            // Set defaults from config
  if  (this->config->media.conferenceNotifyOnJoin)
  {    attributes |= MMS_CONFERENCE_SOUNDTONE;
       if  (this->config->media.conferenceNoToneReceiveOnly)
            attributes |= MMS_CONFERENCE_RCVONLY_NOTONE;
  }
               
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ATTRIBUTE];
  char* searchstart = xmlmsg->firstparam();

  while(1)                                  // Find all conference attributes
  {
    char* bufpos  = xmlmsg->getvaluefor(paramname, searchstart);
    if   (bufpos == NULL) break; 
    searchstart   = bufpos; 

    int  attrno = xmlmsg->getConferenceAttribute();
    if  (attrno == -1)  
         if  (editflags) *editflags |= EDIT_ERROR;
         else;
    else switch(attrno)
         { case MmsAppMessageX::SOUND_TONE: 
                attributes |=  MMS_CONFERENCE_SOUNDTONE; 
                break;  
           case MmsAppMessageX::NO_TONE: 
                attributes &= ~MMS_CONFERENCE_SOUNDTONE; 
                attributes |=  MMS_CONFERENCE_RCVONLY_NOTONE; 
                break;   
           case MmsAppMessageX::SOUND_TONE_WHEN_RECEIVE_ONLY:
                attributes &= ~MMS_CONFERENCE_RCVONLY_NOTONE; 
                attributes |=  MMS_CONFERENCE_SOUNDTONE; 
                break;
           case MmsAppMessageX::NO_TONE_WHEN_RECEIVE_ONLY:
                attributes |=  MMS_CONFERENCE_RCVONLY_NOTONE;
                break;
         }
  }

  if  (attributes)   
       map.insert(MMSP_CONFERENCE_ATTRIBUTES, (int)attributes);

  return attributes != 0;                   // Indicate if attributes were present
}



int MmsMqAppAdapter::buildConfereeAttributeParameters
( MmsFlatMapWriter& map, MmsAppMessage* xmlmsg, unsigned int* editflags)
{
  // Parse XML for <field name="confereeAttribute">value</field> entries. 
  // Translate XML attributes to media server attribute bitmap.

  unsigned int attributes = 0;                           
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::CONFEREE_ATTRIBUTE];
  char* searchstart = xmlmsg->firstparam();

  while(1)                                  // Find all conferee attributes
  {
    char* bufpos  = xmlmsg->getvaluefor(paramname, searchstart);
    if   (bufpos == NULL) break; 
    searchstart   = bufpos; 

    int  attrno = xmlmsg->getConfereeAttribute();
    if  (attrno == -1)  
         if  (editflags) *editflags |= EDIT_ERROR;
         else;
    else switch(attrno)
         { case MmsAppMessageX::MONITOR: 
                attributes  |= MMS_CONFEREE_MONITOR; 
                break;   
           case MmsAppMessageX::RECEIVE_ONLY:
                attributes  |= MMS_CONFEREE_RECEIVE_ONLY; 
                break;
           case MmsAppMessageX::TARIFF_TONE: 
                attributes  |= MMS_CONFEREE_TARIFF_TONE; 
                break;   
           case MmsAppMessageX::COACH:
                attributes  |= MMS_CONFEREE_COACH; 
                break;
           case MmsAppMessageX::PUPIL:
                attributes  |= MMS_CONFEREE_PUPIL; 
                break;
         }
  }

  if  (attributes)   
       map.insert(MMSP_CONFEREE_ATTRIBUTES, (int)attributes);

  return attributes != 0;                   // Indicate if attributes were present
}



int MmsMqAppAdapter::buildConfereeAttributeParameter
( MmsFlatMapWriter& map, MmsAppMessage* xmlmsg)
{      
  // Parse XML for <field name="xxxx">value</field> entries, where xxxx
  // is one of our conferee attribute names. The resultant attibute
  // doubleword masks a single attribute, possibly with the reset bit
  // set indicating that the media server should turn the attribute
  // off. The parameter value must be boolean.

  int   attrresult = 0, isAttributeSet = 0, attribute = 0;
  char* pattrval   = NULL;  
                
  int   attrindex  = xmlmsg->getConfereeAttributeParam(&pattrval);
  char* attrvalue  = xmlmsg->paramValue();
                                            // Did we recognize parameter name?
  if   (attrindex == -1 || attrvalue == NULL) 
        attrresult = MMS_ERROR_TOO_FEW_PARAMETERS;    
  else                                      // Ensure parameter value is 0 or 1
  {     isAttributeSet = ACE_OS::atoi(attrvalue);
        if  (isAttributeSet == 1 || (isAttributeSet == 0 && isZeros(attrvalue)));
        else attrresult = MMS_ERROR_PARAMETER_VALUE;
  }    
  
  if  (attrresult) return attrresult;

  switch(attrindex)                         // Mask the indicated attribute 
  {                                         // bit into attribute parameter
    case MmsAppMessageX::RECEIVE_ONLY:attribute |= MMS_CONFEREE_RECEIVE_ONLY; break;
    case MmsAppMessageX::TARIFF_TONE: attribute |= MMS_CONFEREE_TARIFF_TONE;  break;
    case MmsAppMessageX::COACH:       attribute |= MMS_CONFEREE_COACH; break;
    case MmsAppMessageX::PUPIL:       attribute |= MMS_CONFEREE_PUPIL; break;
    default: return MMS_ERROR_PARAMETER_VALUE;
  } 

  if  (isAttributeSet);                     // Mask reset bit if indicated
  else attribute |= MMS_CONFEREE_ATTRIBUTE_OFF;
                                            // Insert the map parameter
  map.insert(MMSP_CONFEREE_ATTRIBUTES, attribute);
  return 0;
}



int MmsMqAppAdapter::isExistingMediaFile(char* filename, MmsLocaleParams& ld) 
{
  const int strlength = filename == NULL? 0: strlen(filename);
  if (strlength < 1 || strlength > MAXPATHLEN) return 0; 
  char fullpath[MAXPATHLEN+1]; memset(fullpath,0,MAXPATHLEN+1);
  const int isIgnoreLocale = config->serverParams.disregardLocaleDirectories;
  int  result = 0;

  if (filename && *filename == '$')   // mapped drive
  {
      if (config->getNumDriveMappings() > 0)
      {
          std::string s = std::string(filename);
          config->getDriveMappingFullPath(s);
          ACE_OS::strncpy(fullpath, s.c_str(), MAXPATHLEN);
          result = (_access(fullpath, 0) != -1);
      }    
  }
  else  // First look in localized directory if we are using them
  { 
    if (0 == this->getMediaFullpath(fullpath, filename, 
        config->serverParams.audioBasePath, ld, isIgnoreLocale))
        result = (_access(fullpath, 0) != -1);
        
        // Next look in audio root directory -- play file may be output of   
        // a previous record action, in which case it exists in audio root
    if(!result && !isIgnoreLocale)
        if (0 == this->getMediaFullpath(fullpath, filename, 
                 config->serverParams.audioBasePath, ld, TRUE))
            result = (_access(fullpath, 0) != -1);
  }

  return result;
}



int MmsMqAppAdapter::editNumericCoderType
(char* coder, int& codertype, unsigned int& altcoder, const int isLocal)
{
  // Determine if valid numeric coder type was specified. Return -1 if error;
  // codertype is returned zero if coder is not numeric or out of range
  // codertype is returned as a valid MMS_CODER_XXXXX value if coder is numeric
  // and is one of the MMS_CODERTYPE_XXXX app server coder enum values or is a 
  // valid combination of two such values.
  // altcoder is returned as a MMS_CODER_XXXX value if a valid combination of
  // coder types was specified; zero otherwise 
  
  codertype = altcoder = 0;
  if (coder == NULL) return -1;
  const int ncoder = ACE_OS::atoi(coder);
                 
  switch(ncoder)
  {
    case 0:
         break;

    case MMS_CODERTYPE_G711ULAW: 
         codertype = MMS_CODER_G711ULAW64K; 
         break;

    case MMS_CODERTYPE_G711ALAW: 
         codertype = MMS_CODER_G711ALAW64K; 
         break;

    case MMS_CODERTYPE_G723: 
    case MMS_CODERTYPE_G723 | MMS_CODERTYPE_G711ULAW:
    case MMS_CODERTYPE_G723 | MMS_CODERTYPE_G711ALAW:

         switch(this->config->media.g723kbps_n)
         { case MmsConfig::MMS_G723_63: 
                codertype = MMS_CODER_G7231_6_3K; // G.723 6.3 kbps
                break;      
           default: 
                codertype = MMS_CODER_G7231_5_3K; // G.723 5.3 kbps
         }      

         // If client specified a G.711 coder along with a low bitrate
         // coder, this means we should attempt to acquire a low bitrate
         // coder, and if one is not available, revert to a G.711 coder.
         altcoder = isLocal? 0:
                ncoder & MMS_CODER_G711ULAW64K? MMS_CODER_G711ULAW64K:
                ncoder & MMS_CODER_G711ALAW64K? MMS_CODER_G711ALAW64K: 0;                       
         break;

    case MMS_CODERTYPE_G729: 
    case MMS_CODERTYPE_G729 | MMS_CODER_G711ULAW64K:
    case MMS_CODERTYPE_G729 | MMS_CODER_G711ALAW64K:

         switch(this->config->media.g729type_n)
         { case MmsConfig::MMS_G729_AB: 
                codertype = MMS_CODER_G729ANNEXAB; // G.729ab
                break;      
           default: 
                codertype = MMS_CODER_G729ANNEXA;  // G.729a
         }      
        
         altcoder = isLocal? 0:
                ncoder & MMS_CODER_G711ULAW64K? MMS_CODER_G711ULAW64K:
                ncoder & MMS_CODER_G711ALAW64K? MMS_CODER_G711ALAW64K: 0;                       
         break;
  }

  return 0;
}
