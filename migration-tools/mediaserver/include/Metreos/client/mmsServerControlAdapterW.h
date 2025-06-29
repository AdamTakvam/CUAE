//
// MmsServerControlAdapterW.h -- Windows GUI adapter for Metreos Media Server
// 
#ifndef MMS_SERVERCONTROLADAPTERW_H
#define MMS_SERVERCONTROLADAPTERW_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mmsProtocolAdapter.h"
#define WIN32_LEAN_AND_MEAN
#define REGKEY_THREADID "ThreadID"
#include <windows.h>



class MmsServerControlAdapterW: public MmsIpcAdapter
{
  public:
                                            
  MmsServerControlAdapterW(MmsTask::InitialParams* params): MmsIpcAdapter(params)
  {
    m_hGui = m_hConsole = m_hHMP = 0;
    m_hThisThread = 0;            
  }

  virtual ~MmsServerControlAdapterW() {}

  protected:
  virtual void onStartAdapter();
  virtual void onStopAdapter() { }
  virtual int  preparseCommand(void* protocolData);  
  virtual void setCommandHeader(MmsServerCmdHeader* cmdHdr, int cmd);
  virtual void onData(void*) { }
  virtual void onHeartbeat(); 
  virtual int  onShutdown();
  virtual void onServerPush(MmsMsg* msg);

  void writeThreadID();
  int  getGuiMessage();
  HWND getMmsWin();
  static BOOL CALLBACK findMmsWin(HWND,LPARAM);

  HWND  m_hGui;
  HWND  m_hConsole;
  HWND  m_hHMP;
  DWORD m_hThisThread;
};



#endif