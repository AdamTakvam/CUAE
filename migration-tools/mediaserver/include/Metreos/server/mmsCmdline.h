//
// mmsCmdline.h  
//
#ifndef MMS_CMDLINE_H
#define MMS_CMDLINE_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#define XP_DISMISS_PROMPT   "prompt"
#define XP_DISMISS_NOPROMPT "noprompt"

#define XP_START_HMP_NEVER     "fN"          
#define XP_START_HMP_ALWAYS    "fA"           
#define XP_START_HMP_IFSTOP    "fV"          

#define XP_START_NEVER_STOP    "fNX"
#define XP_START_ALWAYS_STOP   "fAX"
#define XP_START_IFSTOP_STOP   "fVX" 

#define XP_SVC_INSTALL         "/sI"
#define XP_SVC_UNINSTALL       "/sD"
#define XP_SVC_RUN             "/sS"   

#define NP_DISMISS_PROMPT       1
#define NP_DISMISS_NOPROMPT     2

#define NP_START_HMP_NEVER      3
#define NP_START_HMP_ALWAYS     4
#define NP_START_HMP_IFSTOP     5

#define NP_START_NEVER_STOP     6
#define NP_START_ALWAYS_STOP    7
#define NP_START_IFSTOP_STOP    8



class MmsCmdline
{
  public:
  MmsCmdline(int argc, char** argv)  
  {
    this->argc   = argc; this->argv = argv;
    this->prompt = TRUE;
    this->startHmpService = NP_START_HMP_NEVER;
    this->stopHmpService  = 0;
  }


  void Parse()
  { 
    if  (this->argc < 2) return;
    char *c, *pVal = NULL;

    for(int i=1; i < argc; i++)                 
    {   c = argv[i];                            
        int nParam = ParseToken(c, &pVal);     

        switch(nParam)
        { 
          case NP_DISMISS_NOPROMPT: 
          case NP_DISMISS_PROMPT:                                                     
               this->prompt = (nParam == NP_DISMISS_PROMPT);     
               break;         

          case NP_START_HMP_NEVER: 
          case NP_START_HMP_ALWAYS:
          case NP_START_HMP_IFSTOP: 
                         
               this->startHmpService = nParam;             
               break;

          case NP_START_NEVER_STOP:
          case NP_START_ALWAYS_STOP:
          case NP_START_IFSTOP_STOP:
                         
               this->startHmpService = nParam - 3; 
               this->stopHmpService  = nParam;           
               break;
  
          default: ACE_OS::printf("MAIN commandline option '%s' not recognized\n", c);                             
        }
    }
  }


  BOOL prompt;
  int  startHmpService;
  int  stopHmpService;

  int  getArgc() { return this->argc; }
  char** getArgv() { return this->argv; }

  protected:

  int ParseToken(char* token, char** pVal)
  { 
    // This will be expanded to include single-character parameter IDs, followed
    // by variable-length text strings, at such time as we define additional
    // command line parameters

    char *p = token;                        // Point at token
    if  (*p++ != '/') return 0;             // Slash begins token 
   
    int nWhichParam = IdentifyRuntimeParameter(p);
 
    return nWhichParam;                     // Return parameter number
  }


  int IdentifyRuntimeParameter(char* c)
  { 
    // This will be generalized to map structures at such time as a sufficient
    // number of command line parameters exists to justify the generalization.
    
    if (stricmp(c, XP_DISMISS_NOPROMPT)  == 0) return NP_DISMISS_NOPROMPT;  
    if (stricmp(c, XP_DISMISS_PROMPT)    == 0) return NP_DISMISS_PROMPT;  

    if (stricmp(c, XP_START_HMP_NEVER)   == 0) return NP_START_HMP_NEVER;  
    if (stricmp(c, XP_START_HMP_ALWAYS)  == 0) return NP_START_HMP_ALWAYS;  
    if (stricmp(c, XP_START_HMP_IFSTOP)  == 0) return NP_START_HMP_IFSTOP;  

    if (stricmp(c, XP_START_NEVER_STOP)  == 0) return NP_START_NEVER_STOP; 
    if (stricmp(c, XP_START_ALWAYS_STOP) == 0) return NP_START_ALWAYS_STOP;  
    if (stricmp(c, XP_START_IFSTOP_STOP) == 0) return NP_START_IFSTOP_STOP;   

    return 0;
  }

  int    argc; 
  char** argv;

}; // class MmsCmdline


#endif

