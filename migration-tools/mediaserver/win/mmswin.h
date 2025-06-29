//
// mmswin.h 
// tray GUI for media server control
//
#if !defined(_MMSWIN_H)
#define _MMSWIN_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#define WIN32_LEAN_AND_MEAN
#include "resource.h"
#include <shellapi.h>

#define MMSWIN_TRAYTIP "Cisco Unified Media Engine"
#define ICON_ADD    (1)
#define ICON_REMOVE (2)
#define ICON_CHANGE (3)
#define MONWIN_DEFAULT_WIDTH  240
#define MONWIN_DEFAULT_HEIGHT  60

#define STATE_STOPPED     0
#define STATE_STARTING    1
#define STATE_STARTED     2
#define STATE_STOPPING    3

#define TIMER_STARTSERVER 1
#define TIMER_BLINK       2

#define BLINK_INIT        1
#define BLINK_BLINK       2

struct  BLINKSTRUCT 
{int    times;
 int    interval; 
 int    blinks;
 int    colorNormal;
 int    colorBlink;
 void load(int t, int i, int n, int b) 
 { times = t; interval = i; colorNormal = n, colorBlink = b; blinks = 0; 
 }
};

struct GSCRATCH                     
{ int      n;
  HWND     hwnd;
  RECT     rect;
  TCHAR    buf[MAX_PATH + MAX_PATH]; 	
};
 
struct APPCONFIG                             
{  
  SIZE   monWinSize;                        // Main window size 
  LPARAM serverThreadHandle;
  UINT   uIcon;
  int    state;
  int    showState;
};
 
struct G
{ 
  HINSTANCE   hInst;                        // App instance
  HWND        hWnd;                         // Main window
  HWND        hList;                        // Monitor listbox
  HMENU       hContext;                     // Context menu
  HACCEL      hAccelTbl;                    // Main window accelerators
  RECT        rect;                         // Monitor window rect
  RECT        workarea;                     // Current desktop work area
  WNDCLASSEX  wcex;                         // Monitor window class info 
  APPCONFIG   app;                          // App configuration params
  GSCRATCH    s;                            // Scratch space 
  BLINKSTRUCT bs;                           // Blink state
  STARTUPINFO sui;   
  TCHAR       szWindowClass[MAX_PATH];      // Monitor window class name
  TCHAR       szTitle      [MAX_PATH];      // Monitor window title 
  PROCESS_INFORMATION pi;
};


LRESULT CALLBACK wndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK trayMessageHandler(HWND, WPARAM, LPARAM);
BOOL    CALLBACK findPrevInstance  (HWND, LPARAM);

int  startMediaServer(G*);
int  stopMediaServer (G*);
int  getMediaServerCommandLine(char* cmdline, char* workingdir);
BOOL initApp(G&, HINSTANCE);
void cleanup(G*);
BOOL createContextMenu(G&);
BOOL InitInstance (G&);
ATOM registerMainClass(G&);
void setTrayIcon(HWND, HICON, UINT, UINT, LPCSTR, int);
void changeTrayIcon(G* g, UINT newIcon);
void setInitialAppConfig(G& g);
void blinkTrayIcon(G* g, const int action=BLINK_BLINK, const int numx=0, const int n=0);



#endif
