//
// MmsSessionC.cpp
//
// Session operation command parameter support
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#ifdef MMS_WINPLATFORM
#include <minmax.h>
#endif
#include "mmsSession.h"
#include "mmsParameterMap.h"
#include "mmsMediaEvent.h"
#include "mmsSessionManager.h"
#include "mmsCommandTypes.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsReporter.h"
#include "mmsTts.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsSession::Op::preparePlaylist()   
{
  // Pre-parses playlist parameters in the operation parameter map. The file 
  // list exists in an embedded MMSP_FILELIST map. Each file specification is
  // a MMSP_FILESPEC map within the MMSP_FILELIST map. For any filespec in the 
  // playlist which is a TTS text sample, we copy the text to the operation's
  // tts string array. Returns the number of files and TTS strings in playlist.

  char* filelistFlatmap = 0;
  int  result = parameterMap.find(MMSP_FILELIST, &filelistFlatmap);
  if  (!filelistFlatmap || !ttsData) return 0;
  int  numfiles = 0;

  MmsFlatMapReader filelistMap(filelistFlatmap);
  
  while(1)
  {                                         // Get next occurrence of 
    char* filespecFlatmap = 0;              // MMSP_FILESPEC within map
    result = filelistMap.find(MMSP_FILESPEC, &filespecFlatmap, NULL, numfiles+1);
    if (!filespecFlatmap) break;
                                            // Extract file path or TTS text
    MmsFlatMapReader filespecMap(filespecFlatmap);
    int  isTtsText = 0, playspeclength = 0;
    char* playspec = 0, *ptts = 0; 

    playspeclength = filespecMap.find(MMSP_FILENAME, &playspec);
    if (!playspec || !(*playspec)) continue;                                                                    
                                        
    result = filespecMap.find(MMSP_FILENAME_IS_TTSTEXT, &ptts);
    if (ptts) isTtsText = *((int*)ptts);
                                             
    if (isTtsText && this->ttsData)         // Accumulate TTS text strings:
    {                                       // We clone the strings here, 
        char*  s = new char[playspeclength];// this memory being freed at
        memcpy(s, playspec, playspeclength);// session command end, when
        this->ttsData->ttsStrings->add(s);  // session->ttsData is deleted.

        if (Config()->diagnostics.flags & MMS_DIAG_LOG_FILELIST)
            MMSLOG((LM_DEBUG,"%s TTS: %s\n", logkey, s));
    }                        
       
    numfiles++;
  }

  return numfiles;
}



int MmsSession::Op::openPlaylist    
( MmsDeviceVoice* deviceVoice, MMSPLAYFILEINFO* firstFileInfo, const int isRecord)
{
  // Extracts file playlist parameters from parameter map. The file list
  // exists in an embedded MMSP_FILELIST map. Each file specification exists
  // a MMSP_FILESPEC map within the MMSP_FILELIST map. From files identified,
  // builds I/O transfer table for play or record & opens all files in playlist.
  // Returns number of files in playlist, or -1 if parameter error

  // We'll still need to modify this for URL filespecs, probably by inclusion
  // of additional entries in the MMSP_FILESPEC map.

  deviceVoice->iottReset();                 // Clear i/o transfer table

  char* filelistFlatmap = 0;
  int result = this->parameterMap.find(MMSP_FILELIST, &filelistFlatmap);
  if (!filelistFlatmap) return 0;

  int numfiles = 0, ttsFilesSeen = 0, filetype = this->getFileTypeParameter();

  MmsFlatMapReader filelistMap(filelistFlatmap);
  
  while(1)
  {       
    // Get next occurrence of MMSP_FILESPEC within map
    MmsPlaylistParams pl(isRecord);   
    pl.ldata.set(firstFileInfo->ldata);     // Set locale directory info

    result = filelistMap.find(MMSP_FILESPEC, &pl.filespecFlatmap, NULL, numfiles+1);
    if  (!pl.filespecFlatmap) break;

                                            // Extract file path
    MmsFlatMapReader filespecMap(pl.filespecFlatmap);

    pl.pathlength = filespecMap.find(MMSP_FILENAME, &pl.subpath);
    if (!pl.subpath) continue;    
                          
    if  (*pl.subpath == '\0')               // If client did not specify name ...
    {
         if  (pl.pathlength < (MMS_RECORDFILENAMESIZE + 5)) 
              continue;                     // ... invent a filename
         else MmsSession::createFilename(pl.subpath, filetype);
    }
    else                                    // Otherwise use name specified:
    {                                       // Check if adapter invented name
         if  (MmsSession::wasFilenameAssigned(pl.subpath, filespecMap))
         {                                  // If so, use name, insert filetype
              MmsSession::createFilename(pl.subpath, filetype, TRUE);
              putMapHeader(setFlag, MmsServerCmdHeader::IS_RESULT_STRING_EXPECTED);
         }
                                            // Extract offset & segment length         
         char* poffset=NULL, *plength=NULL, *ptts=NULL;                                          
         result = filespecMap.find(MMSP_FILEOFFSET, &poffset);
         result = filespecMap.find(MMSP_FILELENGTH, &plength);
         if (poffset) pl.fileoffset    = *((int*)poffset);
         if (plength) pl.segmentlength = *((int*)plength);
                                             
         result = filespecMap.find(MMSP_FILENAME_IS_TTSTEXT, &ptts);
         if (ptts) pl.isTtsText = *((int*)ptts);
                                             
         if (pl.isTtsText)                  // If "path" is a TTS text string ...
             pl.subpath = this->ttsData?   // ... get path to its .wav file
                ttsData->ttsFiles->getAt(ttsFilesSeen++): NULL; 
    }

    pl.outpath = firstFileInfo? firstFileInfo->fullpath: NULL;
     
    if (-1 == session->addToPlaylist(pl, deviceVoice))
        return -1;                                           

    numfiles++;

    if  (numfiles == 1 && firstFileInfo)    // Caller may need file path
    {    firstFileInfo->path = pl.subpath;  // to read or write a descriptor
         firstFileInfo->pathlength = pl.pathlength;
    } 
  }

  return numfiles;
}



int MmsSession::addToPlaylist(MmsPlaylistParams& pl, MmsDeviceVoice* deviceVoice) 
{
  if (!pl.subpath) return -1;
  const int isPlay = !pl.isRecordFile && !pl.isTtsText;

  char fullpath[MAXPATHLEN];                // Build full path to playfile 
  pl.fullpath = fullpath;

  this->buildPlayfileFullPath
     (fullpath, pl.subpath, pl.ldata, pl.isRecordFile, pl.isTtsText); 
                                            // If play action and file does not
  if (isPlay && _access(fullpath,0) == -1)  // exist, it may have been recorded  
      this->buildPlayfileFullPath           // by CUME; try audio root directory
         (fullpath, pl.subpath, pl.ldata, TRUE, FALSE); 

  if (pl.outpath) strcpy(pl.outpath, fullpath);  

  if (config->diagnostics.flags & MMS_DIAG_LOG_FILELIST)
      MMSLOG((LM_DEBUG,"%s %s\n", objname, fullpath));

  // Lock all audio files during open in order not to conflict with 
  // periodic policing of the audio directories. This should be sufficient 
  // as open files will be ignored during policing.
  ACE_Guard<ACE_Thread_Mutex> x(sessionMgr->audiolock);

  // If record action and file exists, delete the file
  if (pl.isRecordFile && _access(fullpath, 0) != -1)
      MmsSession::removeRecordFile(fullpath);

  return pl.isRecordFile?                   // Add file to playrec list
         deviceVoice->openfileRecord(fullpath):
         deviceVoice->openfilePlay  (fullpath, pl.fileoffset, pl.segmentlength);
}



int MmsSession::Op::getTerminationConditionParameters
( MmsDeviceVoice* deviceVoice, MMS_DV_TPT_LIST& tptlist)
{
  // Extracts termination condition parameters from parameter map. The ter-
  // mination conditions list exists in an embedded MMSP_TERMINATION_CONDITIONS
  // map. Each termination condition exists in a MMSP_TERMINATION_CONDITION map
  // within the MMSP_TERMINATION_CONDITIONS map. For each such condition
  // identified, register the termination condition with the voice device..
  // Returns number of conditions in list, or -1 if condition type error

  // We expect termination conditions as received from protocol conversion, 
  // to be specified as HMP constants. This is less than ideal from an adapter 
  // perspective, as adapter code must know about HMP, but at this point it 
  // is not worth the development effort to generalize the many termination 
  // conditions, with their various unique options.

  char* termcondsFlatmap = NULL;
  int  result = parameterMap.find(MMSP_TERMINATION_CONDITIONS, &termcondsFlatmap);
  if  (termcondsFlatmap == NULL) return 0;
  int  numconds = 0;

  MmsFlatMapReader condsMap(termcondsFlatmap);

  while(1)
  {                                         // Get next occurrence of 
    char* condFlatmap = NULL;               // MMSP_TERMINATION_CONDITION
    result = condsMap.find(MMSP_TERMINATION_CONDITION, &condFlatmap, NULL, numconds+1);
    if  (condFlatmap == NULL) break;
                                            
    MmsFlatMapReader condMap(condFlatmap);  // Extract termination condition
    char* ptype = NULL, *plength = NULL, *pflags = NULL, *pdata = NULL;
    int   type=0, length=0, flags=0, data=0;

    result = condMap.find(MMSP_TERMINATION_CONDITION_TYPE,   &ptype);
    result = condMap.find(MMSP_TERMINATION_CONDITION_LENGTH, &plength);
    result = condMap.find(MMSP_TERMINATION_CONDITION_DATA1,  &pflags);
    result = condMap.find(MMSP_TERMINATION_CONDITION_DATA2,  &pdata);
    if  (!ptype || !plength) continue;

    type   = *((int*)ptype);
    length = *((int*)plength);
    if  (pflags) flags = *((int*)pflags);
    if  (pdata)  data  = *((int*)pdata);
    int  isInternalCondition = FALSE;

    switch(type)
    {
      case DX_DIGMASK:
         { // DX_DIGMASK has a MMSP_DIGITLIST which must be translated 
           // JDL, 05/24/06, we also need digitlist info to check preexist DX_DIGMASK.
           if  (this->getDigitListParameters(deviceVoice, condMap) == 0) continue;

           unsigned short digitmask = 0;       
           session->getDigitMaskParameters(deviceVoice, condMap, digitmask);
           length = digitmask;
           break;
         }

     case DX_METREOS_DIGPATTERN:
           // DX_METREOS_DIGPATTERN has a MMSP_DIGITLIST which we save off.
           // This is an internal media server monitored term condition, and as
           // such is not registered with the voice device, but with the session 
           
           if  (this->getDigitListParameters(deviceVoice, condMap) == 0) continue;

           isInternalCondition = TRUE;
           this->flags |= MMS_OP_FLAG_DIGPATTERN_TERM_SET;  

           if  (Config()->diagnostics.flags & MMS_DIAG_LOG_TERMCONDITIONS)
                MMSLOG((LM_INFO,"%s op %d start digit pattern monitor\n", logkey, opid));         
           break;
    }

    result = isInternalCondition? 0:
         deviceVoice->setTerminationCondition(tptlist, type, length, flags, data);
    if  (result == -1) return -1;           // Type not recognized

    numconds++;
  }

  return numconds;
}



int MmsSession::Op::getPlayRecordParameters   
( MmsDeviceVoice::MMS_PLAYRECINFO& playrecinfo, MMSPLAYFILEINFO& fileinfo, const int isPlay)
{
  // Extracts MMSP_PLAY_RECORD_ATTRIBUTES parameter and masks the file
  // format, bit rate, and file type information encoded therein. 
  // Translates these parameters from MMS constants to vox device constants 

  MmsConfig* config = this->Config();
  char* pattrs = NULL;                       
  int   attrs  = 0;

  int  result = parameterMap.find(MMSP_PLAY_RECORD_ATTRIBUTES, &pattrs);
  if  (pattrs) attrs = *((int*)pattrs);
                                              
  int  configDefaultRate     = isPlay?      // First get defaults from config
       config->serverParams.defaultBitRatePlay:
       config->serverParams.defaultBitRateRecord;
  int  configDefaultFormat   = isPlay?
       config->serverParams.defaultFormatPlay:
       config->serverParams.defaultFormatRecord;
  int  configDefaultFiletype = isPlay?
       config->serverParams.defaultFileTypePlay:
       config->serverParams.defaultFileTypeRecord;
  int  configDefaultSamplesize = isPlay?
       config->serverParams.defaultSampleSizePlay:
       config->serverParams.defaultSampleSizeRecord;

  // If an attribute was supplied with the command, we use it; otherwise
  // if a default for the attribute was specified in the config file, we
  // user that; otherwise, if a default for the attribute was specified
  // in the config object at compile time, we use that; otherwise the
  // voice media object will use its default value for the attribute.
  // (If playback, and a descriptor file is present for the playback file,
  // an attribute in the descriptor file will trump any of the above)
                                            // Check parameter overrides:                                     
  if (0 == (playrecinfo.mode = MmsSession::getFormatAttribute(attrs)))
      playrecinfo.mode = MmsSession::getFormatAttribute(configDefaultFormat); 

  if (0 == (playrecinfo.rate = MmsSession::getRateAttribute(attrs)))
      playrecinfo.rate = MmsSession::getRateAttribute(configDefaultRate);  

  if (0 == (playrecinfo.samplesize = MmsSession::getSamplesizeAttribute(attrs)))
      playrecinfo.samplesize = MmsSession::getSamplesizeAttribute(configDefaultSamplesize);  

  if (0 == (playrecinfo.filetype = MmsSession::getFiletypeAttribute(attrs)))
      playrecinfo.filetype = MmsSession::getFiletypeAttribute(configDefaultFiletype);     

  playrecinfo.tone   = (attrs & MMS_PLAYTONE) != 0;

  playrecinfo.nogain = (attrs & MMS_GAINCONTROL_NONE) != 0;

  if (config->media.agcDisableVox)
      playrecinfo.nogain = TRUE;

  if (fileinfo.ldata.isEmpty()) this->getLocaleParameters(fileinfo.ldata);
                                            // Set path to descriptor file
  if (-1 == session->makePropertiesFilePath(&fileinfo)) return -1;

  if (isPlay)
  {   // If playing audio, check if the playfile has a matching descriptor 
      // file containing its attributes. If the file is present, this read  
      // will copy the type, mode, and rate values from the descriptor record 
      // into caller's MMS_PLAYRECINFO, if the file values are valid. Note
      // that descriptor files are created during a record action, so the
      // file path will always be to audio root, and not to a locale directory. 
      MmsAudioFileDescriptor proprec;                      
      proprec.read(playrecinfo, &fileinfo);      
  }
                                            // Engineering diagnostics switches
  if (config->diagnostics.flags & MMS_DIAG_LOG_PLAYREC_ATTRS)
      playrecinfo.flags |= MmsDeviceVoice::MMS_PLAYRECINFO::LOGPLAYREC; 
        
  if (config->diagnostics.flags & MMS_DIAG_LOG_TERMCONDITIONS)
      playrecinfo.flags |= MmsDeviceVoice::MMS_PLAYRECINFO::LOGTPT;

  return 0;        
}



int MmsSession::Op::getFileTypeParameter()
{         
  // Extracts just the filetype info from the parameter map. We need this info
  // early in order to construct a record filename. 
  // Returns MMS_FILETYPE_VOX or MMS_FILETYPE_WAV

  int   returnedFileType = Config()->serverParams.defaultFileTypeRecord;
  char* pattrs = NULL;                       

  int  result = parameterMap.find(MMSP_PLAY_RECORD_ATTRIBUTES, &pattrs);
  if (!pattrs) return returnedFileType;
  int  attrs = *((int*)pattrs);

  if  (attrs & MMS_FILETYPE_VOX) returnedFileType = MMS_FILETYPE_VOX; else
  if  (attrs & MMS_FILETYPE_WAV) returnedFileType = MMS_FILETYPE_WAV; 
 
  return returnedFileType;
}



int MmsSession::Op::getLocaleParameters(MmsLocaleParams& params)
{         
  // Gets appName and locale values from parameter map

  char* appname = NULL, *locale = NULL;
  int result = 0;

  const int lengtha = parameterMap.find(MMSP_APP_NAME, &appname);
  const int lengthl = parameterMap.find(MMSP_LOCALE,   &locale);

  if  (appname && locale)   
       params.set(appname, locale);   
  else result = -1;

  return result;
}



int MmsSession::wasFilenameAssigned(char* filepath, MmsFlatMapReader& filespecMap)
{
  // Determine if an 8-character record filename was assigned at adapter
   
  char* pWasAssigned = 0;
  int   nWasAssigned = 0;

  if (filespecMap.find(MMSP_FILENAME_IS_ASSIGNED, &pWasAssigned))
      nWasAssigned = *((int*)pWasAssigned);
  
  return (nWasAssigned // Also ensure 8 characters, null-terminated, no dot   
      && (filepath[MMS_RECORDFILENAMESIZE] == 0)
      && (memchr(filepath,'.',MMS_RECORDFILENAMESIZE) == NULL) 
      && (strlen(filepath) == MMS_RECORDFILENAMESIZE));
}



int MmsSession::removeRecordFile(char* path)
{
  // Erases a record file and its associated descriptor file
  if (!path || _access(path, 2) == -1) return -1; // File is open

  ACE_OS::unlink(path);
  if (_access(path,0) != -1) return -1;

  char newpath[MAXPATHLEN]; strcpy(newpath, path);

  MmsDirectoryRecursor::PathInfo pathinfo; 
  MmsDirectoryRecursor::parsepath(pathinfo, newpath);

  ACE_OS::strcpy(pathinfo.ext, MMS_RECORD_PROPERTIES_FILE_EXTENSION);    
  ACE_OS::unlink(newpath);                  // Delete the descriptor file  
  if (_access(newpath,0) != -1) return -1;  
  return 0;
}



int MmsSession::getDigitMaskParameters  
( MmsDeviceVoice* deviceVoice, MmsFlatMapReader& map, unsigned short& digitmask)
{
  // Extracts digitlist parameter from specified map, and inserts each digit in
  // that list to the supplied digit mask. Digit masks may be used to qualify
  // the DX_DIGMASK termination condition. Returns number of digits in list.

  char* digits = 0;
  unsigned short mask = 0;
  int strlength  = map.find(MMSP_DIGITLIST, &digits);
  int digitcount = strlength? strlength - 1: 0;

  for(int i = 0; i < digitcount; i++)   
      mask = deviceVoice->addToDigitMask(digits[i], mask);

  digitmask = mask;
  return digitcount;
}



int MmsSession::Op::getDigitListParameters(MmsDeviceVoice* dv,  MmsFlatMapReader& map)
{
  // Extracts digitlist parameter from specified map, and copies the list
  // to the operation object, establishing session->isMonitorDigitPattern.

  memset(this->digitlist, 0, MMS_SIZE_DIGITLIST);
  char* digits = 0;

  map.find(MMSP_DIGITLIST, &digits);

  int len = digits? ACE_OS::strlen(digits): 0;

  if (len) memcpy(this->digitlist, digits, min(len, MMS_SIZE_DIGITLIST-1));

  return len;
}



int MmsSession::Op::getVolumeSpeedParameters(MmsVolumeSpeedEncoder& coder, char* flatmap)
{
  // Extracts volume and speed parameters from either incoming parameter map or from 
  // operation parameter map, to supplied encoder object. 
  // Returns parameter count (0 = none, 1 = volume OR speed, 2 = volume AND speed)
                                            
  if  (flatmap)
  {
       MmsFlatMapReader map(flatmap);
       return this->getVolumeSpeedParameters(coder, map);
  }
  else return this->getVolumeSpeedParameters(coder, this->flatmap());
}



int MmsSession::Op::getVolumeSpeedParameters(MmsVolumeSpeedEncoder& coder, MmsFlatMapReader& map)
{
  // Extracts volume and speed parameters from supplied parameter map to supplied encoder. 
  // Returns parameter count (0 = none, 1 = volume OR speed, 2 = volume AND speed)

  char* pvolume=0, *pspeed=0, *padjtype=0, *ptogtype=0;
  int   length=0, adjtype=0, togtype=0;

  length = map.find(MMSP_VOLUME,&pvolume);
  length = map.find(MMSP_SPEED, &pspeed);
  if (!pvolume && !pspeed) return 0;

  length = map.find(MMSP_ADJUSTMENT_TYPE, &padjtype);
  length = map.find(MMSP_TOGGLE_TYPE, &ptogtype);
  if (padjtype) adjtype = *(int*)padjtype;
  if (ptogtype) togtype = *(int*)ptogtype;   

  if (pvolume)
  {
      coder.isVolumeSet(TRUE);
      coder.volume(*(int*)pvolume);

      if (adjtype) 
      {   coder.vadjtype(adjtype);
          if (togtype) coder.vtogtype(togtype);
      }
  }

  if (pspeed)
  {
      coder.isSpeedSet(TRUE);
      coder.speed(*(int*)pspeed);

      if (adjtype) 
      {   coder.sadjtype(adjtype);
          if (togtype) coder.stogtype(togtype);
      }
  }
      
  return coder.isVolumeSet() + coder.isSpeedSet();
}



int MmsSession::getRecordFileExpiration()
{
  // Days until expiration of a recording can be specified in a MMSP_EXPIRES 
  // parameter. If not present, we use the configged recordFileExpirationDays.

  static const int maxExpirationDays = 9999;
  int   expirationDays = 0;
  char* pExpires = NULL;

  parameterMap.find(MMSP_EXPIRES, &pExpires);
  if  (pExpires == NULL) 
       expirationDays = config->media.recordFileExpirationDays;
  else expirationDays = *((int*)pExpires);
      
  if  (expirationDays < 0) 
       expirationDays = 0; 
  else                                   
  if  (expirationDays > maxExpirationDays) 
       expirationDays = maxExpirationDays; 
  
  return expirationDays;
}


                                         
int MmsSession::setDefaultConnectAttributes
( unsigned int& remoteattrs, unsigned int& localattrs, const int isHalfConnect) 
{
  // If client did not specify a particular connection attribute, mask the
  // installation default for that attribute into the attributes dword

  if  (this->isReconnect())                  
       return this->setDefaultReconnectAttributes(remoteattrs, localattrs);

  int  configRemoteCoderMms = 0, configLocalCoderMms = 0;

  int  clientRemoteCoder     = MMS_GET_CODER_BITS(remoteattrs);
  int  clientRemoteFramesize = MMS_GET_FRAMESIZE_BITS(remoteattrs);
  int  clientRemoteVadEnable = remoteattrs & MMS_CODER_VAD_ENABLE;

  int  clientLocalCoder      = MMS_GET_CODER_BITS(localattrs);
  int  clientLocalFramesize  = MMS_GET_FRAMESIZE_BITS(localattrs);
  int  clientLocalVadEnable  = localattrs  & MMS_CODER_VAD_ENABLE;

  const char* configRemoteCoder     = config->media.remoteCoderType;
  const int   configRemoteFramesize = config->media.remoteCoderFramesize;
  const int   configRemoteVadEnable = config->media.remoteCoderVadEnable;

  const char* configLocalCoder      = config->media.localCoderType;
  const int   configLocalFramesize  = config->media.localCoderFramesize;
  const int   configLocalVadEnable  = config->media.localCoderVadEnable;

  if (!isHalfConnect && (flags & MMS_SESSION_FLAG_HALF_CONNECTED))
  { 
      // When a half connect was previously completed, and a particular
      // attribute was not specified at full connect time, default to
      // the media attribute specified or defaulted at half connect time
      if  (!clientRemoteCoder)
      {    clientRemoteCoder = MMS_GET_CODER_BITS(remoteIpAttrs);
           remoteattrs |= clientRemoteCoder;
      }
      if (!clientRemoteFramesize) 
      {    clientRemoteFramesize = MMS_GET_FRAMESIZE_BITS(remoteIpAttrs);
           remoteattrs |= clientRemoteFramesize;
      }
      if (!clientRemoteVadEnable) 
      {    clientRemoteVadEnable = remoteIpAttrs & MMS_CODER_VAD_ENABLE;
           remoteattrs |= clientRemoteVadEnable;
      }

      if  (!clientLocalCoder)
      {    clientLocalCoder = MMS_GET_CODER_BITS(localIpAttrs);
           localattrs |= clientLocalCoder;
      }
      if (!clientLocalFramesize) 
      {    clientLocalFramesize  = MMS_GET_FRAMESIZE_BITS(localIpAttrs);
           localattrs |= clientLocalFramesize;
      }
      if (!clientLocalVadEnable) 
      {    clientLocalVadEnable  = localIpAttrs & MMS_CODER_VAD_ENABLE;
           localattrs |= clientLocalVadEnable;
      }

      if (this->isCoderReserved() && !isLowBitrateCoder(remoteattrs, localattrs)) 
      {   // LBR coder specified at half connect was just overridden by non-LBR.
          // Since we reserved the LBR resource at half connect, we now unreserve it.
          MMSLOG((LM_DEBUG,"%s session %d LBR coder reservation canceled\n",objname,ordinal));
          this->reserveLowBitrateCoderEx(FALSE);
      }  
  }


  if  (clientRemoteCoder)
  {
       // when remote coder specified but not framesize, use default framesize
       if (!clientRemoteFramesize)
            remoteattrs |= this->getDefaultFramesizeForSpecifiedCoder(clientRemoteCoder);
  }
  else  
  if  (!clientRemoteCoder && configRemoteCoder)
  {
       // when no remote coder specified, use configured default coder
       configRemoteCoderMms = coderStringToMms(configRemoteCoder);
       if  (configRemoteCoderMms)
            remoteattrs |= configRemoteCoderMms;
  } 
  
  if  (configRemoteCoderMms)
  {    // when we use the default coder, also use the default framesize
       unsigned int framesizebits;

       if  (isValidFramesizeForSpecifiedCoder
              (configRemoteCoderMms, configRemoteFramesize))
            framesizebits = framesizeToMms(configRemoteFramesize);
       else framesizebits = getDefaultFramesizeForSpecifiedCoder(configRemoteCoderMms);

       remoteattrs |= framesizebits;
  }
  else
  if (!clientRemoteFramesize)
  {      
      // when client specifies coder but not framesize, use the media
      // server (also CallManager) default framesize for that coder                                    
      remoteattrs |= this->getDefaultFramesizeForSpecifiedCoder
          (MMS_GET_CODER_BITS(remoteattrs));
  }
        
  if  (!clientRemoteVadEnable && configRemoteVadEnable) 
       remoteattrs |= MMS_CODER_VAD_ENABLE;
          

  // Local attributes, when not explicitly specified,  
  // will assume the values of the corresponding remote attributes

  if  (clientLocalCoder)
  {    // when local coder specified but not framesize, use default framesize
       if (!clientLocalFramesize)
            localattrs |= this->getDefaultFramesizeForSpecifiedCoder(clientRemoteCoder);
  }    // when local coder not specified, use remote coder
  else localattrs |= MMS_GET_CODER_BITS(remoteattrs);

 
  if  (!clientLocalFramesize)
       localattrs |= MMS_GET_FRAMESIZE_BITS(remoteattrs);
  
  if  (!clientLocalVadEnable)
  {
       if  (configLocalVadEnable)
            localattrs |= MMS_CODER_VAD_ENABLE;
       else localattrs |= clientRemoteVadEnable;
  } 
         
  this->localIpAttrs  = localattrs;         // Save formatted IP info for use
  this->remoteIpAttrs = remoteattrs;        // on subsequent connect/reconnect

  return 0;
}



int MmsSession::setDefaultReconnectAttributes
( unsigned int& remoteattrs, unsigned int& localattrs) 
{
  // When we are reconnecting (changing port and/or IP of an existing
  // connection, and possibly other connection attributes as well), 
  // we use existing IP device settings as defaults.

  const int clientRemoteCoder       = MMS_GET_CODER_BITS(remoteattrs);
  const int clientRemoteFramesize   = MMS_GET_FRAMESIZE_BITS(remoteattrs);
  const int clientRemoteDataflowDir = MMS_DATAFLOW_DIRECTION_GET(remoteattrs);

  const int clientLocalCoder        = MMS_GET_CODER_BITS(localattrs);
  const int clientLocalFramesize    = MMS_GET_FRAMESIZE_BITS(localattrs);
  const int clientLocalDataflowDir  = MMS_DATAFLOW_DIRECTION_GET(localattrs);


  if  (!clientRemoteCoder)
  {
       const int currentCoder = MMS_GET_CODER_BITS(this->remoteIpAttrs);   
       MMS_CLEAR_CODER_BITS(remoteattrs);   
       remoteattrs |= currentCoder;
  } 

  if  (!clientLocalCoder)
  {
       const int currentCoder = MMS_GET_CODER_BITS(this->localIpAttrs);   
       MMS_CLEAR_CODER_BITS(localattrs);   
       localattrs |= currentCoder;
  } 

  if  (!clientRemoteFramesize)
  {
       const int currentFramesize = MMS_GET_FRAMESIZE_BITS(this->remoteIpAttrs);   
       MMS_CLEAR_FRAMESIZE_BITS(remoteattrs);   
       remoteattrs |= currentFramesize;
  } 

  if  (!clientLocalFramesize)
  {
       const int currentFramesize = MMS_GET_FRAMESIZE_BITS(this->localIpAttrs);   
       MMS_CLEAR_FRAMESIZE_BITS(localattrs);   
       localattrs |= currentFramesize;
  } 

  if  (!clientRemoteDataflowDir)
  {
       const int currentDataflow = MMS_DATAFLOW_DIRECTION_GET(this->remoteIpAttrs);   
       MMS_DATAFLOW_DIRECTION_CLEAR(remoteattrs);   
       remoteattrs |= currentDataflow;
  } 

  if  (!clientLocalDataflowDir)
  {
       const int currentDataflow = MMS_DATAFLOW_DIRECTION_GET(this->localIpAttrs);   
       MMS_DATAFLOW_DIRECTION_CLEAR(localattrs);   
       localattrs |= currentDataflow;
  } 


  // VAD will not change on reconnect

  const int remoteVadEnabled = (this->remoteIpAttrs & MMS_CODER_VAD_ENABLE) != 0;
  if  (remoteVadEnabled)
       remoteattrs |= MMS_CODER_VAD_ENABLE;

  const int localVadEnabled  = (this->localIpAttrs  & MMS_CODER_VAD_ENABLE) != 0;
  if  (localVadEnabled)
       localattrs  |= MMS_CODER_VAD_ENABLE;
 
  return 0;
}



int MmsSession::isRemoteConnectionAttributeChange(const unsigned int newRemoteattrs,
  const unsigned int currentRemoteAttrs, const int isLog) 
{
  // Determine if attributes of the remote connection are changing by comparing
  // incoming connection attributes to current connection attributes.

  int changes = 0;
  const int clientRemoteCoder       = MMS_GET_CODER_BITS(newRemoteattrs);
  const int clientRemoteFramesize   = MMS_GET_FRAMESIZE_BITS(newRemoteattrs);
  const int clientRemoteDataflowDir = MMS_DATAFLOW_DIRECTION_GET(newRemoteattrs);

  if  (clientRemoteCoder)
  {
       const int currentCoder = MMS_GET_CODER_BITS(currentRemoteAttrs); 
  
       if (clientRemoteCoder != currentCoder)  
       {
           changes++;
           if (isLog) MMSLOG((LM_DEBUG,"%s coder change %d to %d\n",
               objname, currentCoder, clientRemoteCoder));
       }
  } 

  if  (clientRemoteFramesize)
  {
       const int currentFramesize = MMS_GET_FRAMESIZE_BITS(currentRemoteAttrs); 
  
       if (clientRemoteFramesize != currentFramesize)  
       {
           changes++;
           if (isLog) MMSLOG((LM_DEBUG,"%s framesize change %d to %d\n",
               objname,currentFramesize,clientRemoteFramesize));
       }
  } 

  if  (clientRemoteDataflowDir)
  {
       const int currentDataflow = MMS_DATAFLOW_DIRECTION_GET(currentRemoteAttrs); 
  
       if (clientRemoteDataflowDir != currentDataflow)  
       {
           changes++;
           if (isLog) MMSLOG((LM_DEBUG,"%s DFD change %d to %d\n",
               objname,currentDataflow,clientRemoteDataflowDir));
       }
  } 

  return changes;
}



int MmsSession::verifyCoderAvailabilityEx
( const int reserveIfAvailable, const int isLog, char* map)
{
  // Verifies that requested coder is available. 
  // Returns new LBR coder available count if available, -1 otherwise.
  // If caller asks to reserve the resource, we update the count here, in order
  // that the verifying of availability and updating of count is done atomically.
  // Updated for OEM licensing enforcement
   
  if (!isLowBitrateCoder(this->remoteIpAttrs, this->localIpAttrs)) 
      return 1;                             // Coder OK since not lo-bitrate
  int result = 0, isLoBitrateResourcesExhausted = 0;;

  Mms::lbrCountLock->acquire();              
                                            // LICX calculate provisional G729 in use
  const int g729Available = MmsAs::g729(MmsAs::RESX_GET);
  const int provisionalAvailable = g729Available - 1;
  
  if  (g729Available < 1)                   // Out of G729 licenses?
       isLoBitrateResourcesExhausted = TRUE;// LICX G729OUT 1 of 2    
  else
  if  (reserveIfAvailable)                  // Reserve a G729 license
  {                                         // (adjusts available count/sets flags)  
       result = this->reserveLowBitrateCoderEx(TRUE, TRUE);
       switch(result)
       {
          case  0: result = provisionalAvailable; break;
          case -1: isLoBitrateResourcesExhausted = TRUE; // LICX G729OUT 2 of 2 
       }        
  }
  else result = provisionalAvailable;

  if (isLoBitrateResourcesExhausted)
  {
       if (map) setFlatmapRescode(map, MMS_REASON_CODER_NOT_AVAILABLE);    
       this->raiseLowBitrateExhaustedAlarm();
       result = -1;
  }

  Mms::lbrCountLock->release();
 
  return result; 
}



int MmsSession::reserveLowBitrateCoderEx(const int isReserve, const int isModify)
{
  // Reserve or un-reserve a low bit rate coder. We do this by updating the LBR 
  // availability count and setting a flag in the session that the resource is 
  // reserved or not. If caller is updating count, isModify is passed FALSE.
  // Modified for licensing enforcement -- now uses MmsAs global license counts. 
   
  int result = 0, modifyResult = 0;

  switch(isReserve)
  {
    case FALSE:                             // Cancel reservation

         if  (!this->isCoderReserved())
              result = -1;
         else
         {    if (isModify)                 // LICX G729+ (1 of 2)                                            
                  Mms::modifyLbrAvailableCount(+1);   

              this->flags &= ~MMS_SESSION_FLAG_G729_RESOURCE_SPENT;
              this->flags &= ~MMS_SESSION_FLAG_IS_CODER_RESERVED;
         }
         break;

    default:                                // Make reservation

         if  (this->isCoderReserved())
              result = -1;
         else
         {    if (isModify)                 // LICX G729- (1 of 2)                                            
                  modifyResult = Mms::modifyLbrAvailableCount(-1);  

              if (modifyResult < 0)         // Out of G729 licenses
                  result = -1; 
              else 
              {   this->flags |= MMS_SESSION_FLAG_G729_RESOURCE_SPENT;
                  this->flags |= MMS_SESSION_FLAG_IS_CODER_RESERVED;
              }
         }
         break;
  }

  return result;
}



int MmsSession::isLowBitrateCoder(const unsigned int rattrs, const unsigned int lattrs)
{
  return isLowBitrateCoder(rattrs) || isLowBitrateCoder(lattrs);
}



int MmsSession::isLowBitrateCoder(const unsigned int attrs)
{
  const unsigned int coderbits = MMS_GET_CODER_BITS(attrs);

  switch(coderbits)
  {
    case MMS_CODER_G711ULAW64K: 
    case MMS_CODER_G711ALAW64K: 
    case 0:
         return false;
  }

  return true;
}



int MmsSession::raiseLowBitrateExhaustedAlarm()
{
  // Fire an alarm indicating LBR ports exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "low-bitrate RTP");

  MMSLOG((LM_ERROR,"%s %s\n", objname, alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_G729_ETC, 
     MMS_STAT_CATEGORY_G729, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return -1;
}



unsigned int MmsSession::getDefaultFramesizeForSpecifiedCoder(const unsigned int coderbits)
{
  switch(coderbits)
  {
    case MMS_CODER_G711ULAW64K: 
    case MMS_CODER_G711ALAW64K: 
         return MMS_CODER_FRAMESIZE_20;

    case MMS_CODER_G7231_5_3K:
    case MMS_CODER_G7231_6_3K:    
         return MMS_CODER_FRAMESIZE_30;

    case MMS_CODER_G729:
    case MMS_CODER_G729ANNEXA:
    case MMS_CODER_G729ANNEXB:
    case MMS_CODER_G729ANNEXAB:
         return MMS_CODER_FRAMESIZE_20;
  }

  return 0;
}



int MmsSession::isValidFramesizeForSpecifiedCoder
( const unsigned int coderbits, const int framesizevalue)
{
  switch(coderbits)
  {
    case MMS_CODER_G711ULAW64K: 
    case MMS_CODER_G711ALAW64K: 
         switch(framesizevalue)
         {
           case 10: case 20: case 30: return true;
         }

    case MMS_CODER_G7231_5_3K:
    case MMS_CODER_G7231_6_3K:    
         switch(framesizevalue)
         {
           case 30: case 60: return true;
         }

    case MMS_CODER_G729:
    case MMS_CODER_G729ANNEXA:
    case MMS_CODER_G729ANNEXB:
    case MMS_CODER_G729ANNEXAB:
         switch(framesizevalue)
         {
           case 20: case 30: case 40: return true;
         }
  }

  return FALSE;
}



int MmsSession::coderStringToMms(const char* szcoder)
{
  // Converts a config file string identifying a particular codec
  // to a mms coder ID bitstring value  

  // We compare only the applicable part of the coder string
  static const char* g711u = "g711u", *g711a = "g711a";
  static const char* g723  = "g723",  *g729  = "g729";
  static const int lenG711 = strlen(g711u), lenG723 = strlen(g723);
  static const int lenG729 = strlen(g729);

  int  codertype = 0;
  char coder[64], *p = coder;              
  ACE_OS::strncpy(coder, szcoder, sizeof(coder)-1);
  while(*p) {*p = tolower(*p); p++;}
    
  if ((ACE_OS::strncmp(coder, g711u, lenG711)) == 0)
       codertype = MMS_CODER_G711ULAW64K;
  else
  if ((ACE_OS::strncmp(coder, g711a, lenG711)) == 0)
       codertype = MMS_CODER_G711ALAW64K;
  else
  if ((ACE_OS::strncmp(coder, g723, lenG723))  == 0)      
       switch(this->config->media.g723kbps_n)
       { case MmsConfig::MMS_G723_63: 
              codertype = MMS_CODER_G7231_6_3K;   
              break;      
         default: 
              codertype = MMS_CODER_G7231_5_3K;   
       }   
  else
  if ((ACE_OS::strncmp(coder, g729, lenG729)) == 0)      
       switch(this->config->media.g729type_n)
       { case MmsConfig::MMS_G729_AB: 
              codertype = MMS_CODER_G729ANNEXAB;  
              break;      
         default: 
              codertype = MMS_CODER_G729ANNEXA;   
       }    

  return codertype;
}



int MmsSession::framesizeToMms(const int framesizevalue)
{
  // Converts a config file integer indicating coder framesize
  // to a mms framesize ID bitstring value  
   
  int  mmsFramesize = 0; 
  switch(framesizevalue) 
  { case 10: mmsFramesize = MMS_CODER_G711_FRAMESIZE_10; break;
    case 20: mmsFramesize = MMS_CODER_FRAMESIZE_20;      break;
    case 30: mmsFramesize = MMS_CODER_FRAMESIZE_30;      break;
    case 40: mmsFramesize = MMS_CODER_G729_FRAMESIZE_40; break;
    case 60: mmsFramesize = MMS_CODER_G723_FRAMESIZE_60; break;
  }

  return mmsFramesize;
}



int MmsSession::getFormatAttribute(const int attrs)
{
  int  format = 0;                           
  if  (attrs & MMS_FORMAT_ADPCM)  format = MmsDeviceVoice::ADPCM; else
  if  (attrs & MMS_FORMAT_PCM)    format = MmsDeviceVoice::PCM;   else
  if  (attrs & MMS_FORMAT_ALAW)   format = MmsDeviceVoice::ALAW;  else
  if  (attrs & MMS_FORMAT_MULAW)  format = MmsDeviceVoice::MULAW;  
  return format;
}



int MmsSession::getRateAttribute(const int attrs)
{  
  int  rate = 0;
  if  (attrs & MMS_RATE_KHZ_8)  
       rate  = MmsDeviceVoice::RATE_8KHZ; 
  else
  if  (attrs & MMS_RATE_KHZ_6)  
       rate  = MmsDeviceVoice::RATE_6KHZ; 
  else
  if  (attrs & MMS_RATE_KHZ_11) 
       rate  = MmsDeviceVoice::RATE_11KHZ; 

  return rate;
}


int MmsSession::getSamplesizeAttribute(const int attrs)
{  
  int  size = 0;
  if  (attrs & MMS_SAMPLESIZE_BIT_4)  
      size  = MmsDeviceVoice::SIZE_4BIT; 
  else
  if  (attrs & MMS_SAMPLESIZE_BIT_8)  
       size  = MmsDeviceVoice::SIZE_8BIT; 
  else
  if  (attrs & MMS_SAMPLESIZE_BIT_16) 
       size  = MmsDeviceVoice::SIZE_16BIT; 

  return size;
}


int MmsSession::getFiletypeAttribute(const int attrs)
{
  int  filetype = 0;
  if  (attrs & MMS_FILETYPE_VOX) filetype = MmsDeviceVoice::VOX; else
  if  (attrs & MMS_FILETYPE_WAV) filetype = MmsDeviceVoice::WAV;
  return filetype;
}



void MmsSession::createFilename
( char* namebuf, const int filetype, const int isNamepartPresent)
{
  // Construct unique filename for a record file, format a1b2c3d4.wav

  if  (isNamepartPresent)
       memset(namebuf + MMS_RECORDFILENAMESIZE, 0, 1 + 3 + 1);
  else 
  {    memset(namebuf, 0, MMS_RECORDFILENAMESIZE + 1 + 3 + 1);
       unsigned long tc = Mms::getTickCount();
       ACE_OS::sprintf(namebuf,"%08X", tc); 
  }
  
  char* ft;      
 
  switch(filetype)
  { case MMS_OMIT_EXTENSION: ft = NULL;   break;
    case MMS_FILETYPE_VOX:   ft = ".vox"; break;
    default: ft = ".wav";
  }
   
  if (ft) ACE_OS::strcat(namebuf, ft);
}



int MmsSession::makePropertiesFilePath(MMSPLAYFILEINFO* info)
{
  // Make a properties file path. When MMS records audio it writes a
  // matching properties file containing the codec and bitrate used
  // to record the file. When playback is requested on the file, MMS
  // uses this information to set the playback parameters.
                                     
  if (-1 == this->buildPlayfileFullPath
     (info->propfilepath, info->path, info->ldata, TRUE, info->isTtsText))
      return -1;

  info->pathlength = ACE_OS::strlen(info->propfilepath);
  char* p  = info->propfilepath + info->pathlength;
  while(p  > info->propfilepath  && *p != '.') p--;
  if   (p == info->propfilepath) return -1;

  ACE_OS::strcpy(p, MMS_RECORD_PROPERTIES_FILE_EXTENSION);
  return 0;
}



int MmsSession::buildPlayfileFullPath
( char* fullpath, char* subpath, MmsLocaleParams& localeData, const int isRecord, const int isTts)
{
   return this->buildPlayfileFullPath
    (fullpath, subpath, localeData.appname, localeData.locale, isRecord, isTts);
}



int MmsSession::buildPlayfileFullPath(char* fullpath, char* subpath, 
  char* appname, char* locale, const int isRecord, const int isTts) 
{  
  // Build the full path string to this souund file
  if (!fullpath || !subpath) return -1;

  if (subpath[0] == '$')                    // Mapped drive indicator is $
  {
      if (config->getNumDriveMappings() > 0)
      {
          std::string s = std::string(subpath);
          config->getDriveMappingFullPath(s);
          ACE_OS::strcpy(fullpath, s.c_str());
          return 0;
      }
  }

  const int isUsersFullPathToAudioFile = subpath[1] == ':';
        
  // Unless client passed in a complete path to the sound file, begin with
  // the audio base path e.g. C:\Program Files\Cisco\UAE\MediaServer\Audio                                    
  const char* basepath = isUsersFullPathToAudioFile? "":   
        config->serverParams.audioBasePath;        

  ACE_OS::strcpy(fullpath, basepath);

  if (*fullpath) ensureTrailingSlash(fullpath);    

  // Add locale subdirectories to the file path. We skip this part if we are
  // configured to not use locales, if client supplied a full path to the file,
  // if locale parameters were missing, if this is a record action (locale is
  // not necessary and the locale directory may not exist, these are written
  // to the audio base directory), or if this is a TTS request (TTS wavs are
  // temporary and as such are likewise written to the audio base directory).
  if (isTts || isRecord);
  else
  if (isUsersFullPathToAudioFile);
  else
  if (config->serverParams.disregardLocaleDirectories);
  else
  if (appname == 0 || locale == 0 || *appname == 0 || *locale == 0);
  else
  {   // Add locale subdirectories to path
      char separator[2] = { ACE_DIRECTORY_SEPARATOR_CHAR_A, '\0' };
      ACE_OS::strcat(fullpath, appname); 
      ACE_OS::strcat(fullpath, separator);
      ACE_OS::strcat(fullpath, locale); 
      ACE_OS::strcat(fullpath, separator);
  }

  if (*subpath == ACE_DIRECTORY_SEPARATOR_CHAR_A) ++subpath;

  // Finally copy in the remainder of the filespec, which will be either just
  // the file name and extension under normal circumstances, or the complete  
  // path including filename/extension if client passed in a full path.
  ACE_OS::strcat(fullpath, subpath);
  return 0;
}



int MmsSession::setTerminationIf(MmsDeviceVoice* deviceVoice, 
  MMS_DV_TPT_LIST& tptlist, DV_TPT& cond) 
{   
  return (NULL == findTerminationCondition(tptlist, cond.tp_termno))?
          deviceVoice->setTerminationCondition(tptlist, cond.tp_termno, 
                         cond.tp_length, cond.tp_flags, cond.tp_data):
          0; 
}



DV_TPT* MmsSession::findTerminationCondition(MMS_DV_TPT_LIST& tptlist, int condition)
{   
  DV_TPT* p;                                
  for(p = tptlist.head; p; p = p->tp_nextp) // Walk termination condition list
      if (p->tp_termno == condition) break;

  return p;
}
