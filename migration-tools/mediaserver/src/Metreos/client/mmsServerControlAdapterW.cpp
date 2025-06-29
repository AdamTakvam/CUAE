//
// MmsServerControlAdapterW.cpp -- mms Windows GUI adapter   
// 
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsServerControlAdapterW.h"
#include "X:\\mediaserver\\win\\mmswincommon.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


void MmsServerControlAdapterW::onStartAdapter()
{
  // We bump the adapter listener thread priority down a notch in order
  // that the process which starts and stops the media server service can
  // identify the thread to which to send thread messages. This is a low
  // activity thread and the lower priority has no negative impact.
  SetConsoleTitle(MMSCONTITLE);
  m_hConsole    = FindWindow(NULL,MMSCONTITLE);
  m_hHMP        = FindWindow(NULL,MMSHMPTITLE);
  m_hThisThread = GetCurrentThreadId();
  writeThreadID();
  SetThreadPriority((HANDLE)m_hThisThread, THREAD_PRIORITY_BELOW_NORMAL);
  
  int count=0;

  while(1)                                   
  {                                         // Install a queue
     int  result =  PostThreadMessage(m_hThisThread, 0, 0, 0); 
     if  (result || count++ > 10) break;
     mmsSleep(1);
  }

  MSG msg;
  GetMessage(&msg, NULL, 0, 0xffff);        // Clear bogus message

  onHeartbeat();                            // Ack gui process
}



void MmsServerControlAdapterW::onHeartbeat() 
{
  if  (!m_hGui && this->getMmsWin())        // First time register our thread#                          
       PostMessage(m_hGui, WM_FROMSERVER, NOTIFY_START, GetCurrentThreadId());

  while(GetQueueStatus(QS_ALLEVENTS))       // Get messages each time including
        this->getGuiMessage();              // first time, in case no gui (401)
}


void MmsServerControlAdapterW::onServerPush(MmsMsg* msg) 
{
}

int MmsServerControlAdapterW::onShutdown()
{
  if  (m_hConsole) ShowWindow(m_hConsole, SW_SHOWNORMAL);
  //  (m_hHMP)     ShowWindow(m_hHMP,     SW_SHOWMINIMIZED);

  PostMessage(m_hGui, WM_FROMSERVER, NOTIFY_STOP, 0); 
  return 0;
}



int MmsServerControlAdapterW::getGuiMessage()
{
  MSG msg;
  GetMessage(&msg, NULL, 0, 0xffff);
  if  (msg.message <= 0) return 0;

  switch(msg.message) 
  {  
   case WM_NOTIFY:
        switch(msg.wParam)
        {
          case NOTIFY_HIDE:
               MMSLOG((LM_INFO,"%s HIDE received\n",taskName)); 
               if  (m_hConsole) ShowWindow(m_hConsole, SW_HIDE);
               //  (m_hHMP) ShowWindow(m_hHMP, SW_HIDE);
               PostMessage(m_hGui, WM_FROMSERVER, NOTIFY_HIDE, 0); 
               break;

          case NOTIFY_SHOW:
               MMSLOG((LM_INFO,"%s SHOW received\n",taskName));  
               if  (m_hConsole) ShowWindow(m_hConsole, SW_SHOWNORMAL);
               //  (m_hHMP)     ShowWindow(m_hHMP,     SW_SHOW);
               PostMessage(m_hGui, WM_FROMSERVER, NOTIFY_SHOW, 0); 
               break;

          case NOTIFY_STOP:
               MMSLOG((LM_INFO,"%s STOP received\n",taskName)); 
               if  (m_hConsole) ShowWindow(m_hConsole, SW_SHOWNORMAL);
               PostMessage(m_hGui, WM_FROMSERVER, NOTIFY_STOP, 0); 
               this->postServerMessage(MMSM_SERVERCONTROL, MMS_SERVERCTRL_SHUTDOWN);
               break;

          case NOTIFY_CONFIG:
               this->postServerMessage(MMSM_SERVERCONTROL, MMS_SERVERCTRL_REFRESHCONFIG);
               break;

          case NOTIFY_CYCLE:
               this->postServerMessage(MMSM_SERVERCONTROL, MMS_SERVERCTRL_CYCLELOG);
               break;  
        }
        break;

   case WM_MMS_PING:
        // Pinger's thread ID is in lParam 617
        PostThreadMessage(msg.lParam, WM_MMS_PINGBACK, msg.wParam, m_hThisThread);
        break;                          
  }

  return 1;
}



void MmsServerControlAdapterW::writeThreadID()
{
  // Write adapter thread ID to registry, for use by external processes  
  // which may wish to send messages to the media server
  HKEY  hkey;
  DWORD dwResult, dwThreadID = GetCurrentThreadId(); 
                                             
  dwResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, MMS_REGKEY, 0, KEY_ALL_ACCESS, &hkey);
  dwResult = RegSetValueEx(hkey, REGKEY_THREADID, 0, REG_DWORD, (BYTE*)&dwThreadID, 4);
  RegCloseKey(hkey);
}



HWND MmsServerControlAdapterW::getMmsWin()  // Find the GUI process
{
  if  (!m_hGui || !IsWindow(m_hGui))
       EnumWindows(findMmsWin, (LPARAM)this);

  return m_hGui;
}



BOOL CALLBACK MmsServerControlAdapterW::findMmsWin(HWND hwnd, LPARAM lParam)
{
  TCHAR buf[MAX_PATH];
  GetClassName(hwnd, buf, MAX_PATH);

  int  isMmsWin = ACE_OS::strcmp(MMSWIN_WINCLASS, buf) == 0;
  if  (isMmsWin)
     ((MmsServerControlAdapterW*)lParam)->m_hGui = hwnd;

  return !isMmsWin;                         
}



int MmsServerControlAdapterW::preparseCommand(void* protocolData)
{
  return 0;
}



void MmsServerControlAdapterW::setCommandHeader(MmsServerCmdHeader* cmdHdr, int cmd)
{
  cmdHdr->command  = cmd;
  cmdHdr->sender   = this;
}

