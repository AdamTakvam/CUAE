//
// mmswin.cpp 
// tray GUI for media server control
//
#include "StdAfx.h"
#include "mmswin.h"
#include "mmswincommon.h"



int startMediaServer(G* g)
{
  if  (g->app.state != STATE_STOPPED) return -1;
  if  (FindWindow(NULL, MMSCONTITLE))
  {    MessageBox(g->hWnd,"Media engine console is active", g->szTitle, MB_ICONEXCLAMATION); 
       return -1;
  }
                                            // Get MMS path from registry
  char cmdline[MAX_PATH], workingdir[MAX_PATH];
  getMediaServerCommandLine(cmdline, workingdir);       
  g->app.state = STATE_STARTING;
  g->app.showState = SW_SHOW;

  memset(&g->sui, 0, sizeof(STARTUPINFO));
  g->sui.cb  = sizeof(STARTUPINFO);
  g->sui.dwFlags = STARTF_USEPOSITION | STARTF_USESHOWWINDOW;
  g->sui.dwX = g->workarea.right  - 700;
  g->sui.dwY = g->workarea.bottom - 500;
  g->sui.wShowWindow = SW_SHOWNORMAL;
       
  changeTrayIcon(g, IDI_YELLOW);            // Give it 10 seconds   
  SetTimer(g->hWnd, TIMER_STARTSERVER, 10 * 1000, NULL);     

  int result = CreateProcess
              (NULL,        // appName,
               cmdline,     // path to exe
               NULL,        // lpProcessAttrs
               NULL,        // lpThreadAttrs
               FALSE,       // bInheritHandles
               0,           // dwCreationFlags e.g. CREATE_NO_WINDOW
               NULL,        // lpEnvironment
               workingdir,  // lpCurrentDirectory
               &g->sui,     // STARTUPINFO
               &g->pi);     // Returned process info
  
  return result;
}



int stopMediaServer(G* g)
{
  if (g->app.state != STATE_STARTED) return -1;
  g->app.state      = STATE_STOPPING;
  g->app.showState  = SW_LIMBO;
  changeTrayIcon(g, IDI_YELLOW);
  SetTimer(g->hWnd, TIMER_STARTSERVER, 10 * 1000, NULL);

  PostThreadMessage(g->app.serverThreadHandle, WM_NOTIFY, NOTIFY_STOP, 0);

  return 0;
}



int APIENTRY WinMain(HINSTANCE hinst, HINSTANCE hPrev, LPSTR cmdline, int nShow)
{                                  
  G g;                                       
  MSG msg;
  if  (!initApp(g, hinst)) return 0;         
                                             
  setTrayIcon(g.hWnd, g.wcex.hIconSm, g.app.uIcon, 
              WM_MMSWIN_TRAYMSG, MMSWIN_TRAYTIP, ICON_ADD);

  while(GetMessage(&msg, NULL, 0, 0))       // Message pump
  {     TranslateMessage(&msg);
        DispatchMessage (&msg);
  }
 
  return msg.wParam;
}



LRESULT CALLBACK wndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{ 
  G* g = (G*)GetWindowLong(hWnd, GWL_USERDATA);

  switch(msg) 
  {
   case WM_MMSWIN_TRAYMSG:                  // Mouse event over tray icon
        return trayMessageHandler(hWnd, wParam, lParam);

   case WM_FROMSERVER:
        g = g;
        	     
        switch(wParam)
        {  
          case NOTIFY_HIDE:
               g->app.showState = SW_HIDE;
               break;

          case NOTIFY_SHOW:
               g->app.showState = SW_SHOW;
               break;

          case NOTIFY_START:
               g->app.state = STATE_STARTED;
               g->app.serverThreadHandle = lParam;
               changeTrayIcon(g, IDI_GREEN);
               break;

          case NOTIFY_STOP:
               g->app.state = STATE_STOPPED;
               changeTrayIcon(g, IDI_ORANGE);
               break;

          case NOTIFY_QUIT:
               break;
        }

        break;
                              
   case WM_COMMAND:                         
                                             
        switch(LOWORD(wParam))               
        {                                    
         case IDM_EXIT:
              DestroyWindow(hWnd);
              break;

         case IDM_STARTSTOP:
              if  (g->app.state == STATE_STARTED)
                   stopMediaServer(g);
              else if  (g->app.state == STATE_STOPPED)
                   startMediaServer(g);
              break;
         default:
              return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        break;  

   case WM_TIMER: 

        switch(wParam)
        { 
          case TIMER_STARTSERVER:
               switch(g->app.state)         // If app adapter not responding,
               { case STATE_STARTING:       // we don't leave state in limbo
                 case STATE_STOPPING: 
                      g->app.state = STATE_STOPPED; 
                      g->app.showState = SW_SHOW;
                      changeTrayIcon(g, IDI_ORANGE);
                      break;
               }
               KillTimer(hWnd, TIMER_STARTSERVER);
               break;

          case TIMER_BLINK:
               blinkTrayIcon(g);
               break;
        }
        break;

   case WM_MMS_REFRESHCONFIG:
        PostThreadMessage(g->app.serverThreadHandle,WM_NOTIFY,NOTIFY_CONFIG,0);
        blinkTrayIcon(g, BLINK_INIT, 2, 240);
        break;

   case WM_DESTROY:
        cleanup(g);
        PostQuitMessage(0);
        break;

   default:
        return DefWindowProc(hWnd, msg, wParam, lParam);
  }

  return 0;
}



LRESULT CALLBACK trayMessageHandler(HWND hWnd, WPARAM wParam, LPARAM lParam)
{ 
  G* g;
  POINT pt;
  UINT  uId;

  switch(lParam) 
  {
   case WM_RBUTTONUP:
        if  (!(g = (G*)GetWindowLong(hWnd, GWL_USERDATA))) break; 
        createContextMenu(*g);  
        SetForegroundWindow(g->hWnd);
        GetCursorPos(&pt);                  // Show context menu
        uId = TrackPopupMenu(g->hContext, TPM_RETURNCMD, pt.x, pt.y, 0, hWnd, NULL);

        switch(uId)
        { case IDM_EXIT:
               PostMessage(hWnd, WM_CLOSE, 0, 0);
               break;

          case IDM_STARTSTOP:
               if  (g->app.state == STATE_STARTED)
                    stopMediaServer(g);
               else
               if  (g->app.state == STATE_STOPPED)
                    startMediaServer(g);
               break;

          case IDM_SHOWHIDE:   
               if  (g->app.showState == SW_LIMBO) break;
               PostThreadMessage(g->app.serverThreadHandle, WM_NOTIFY,
                   (g->app.showState == SW_HIDE)? NOTIFY_SHOW: NOTIFY_HIDE, 0);
               g->app.showState = SW_LIMBO;
               break;

          case IDM_REFRESHCONFIG:
               PostThreadMessage(g->app.serverThreadHandle,WM_NOTIFY,NOTIFY_CONFIG,0);
               blinkTrayIcon(g, BLINK_INIT, 2, 240);
               break;

          case IDM_CYCLELOG:
               PostThreadMessage(g->app.serverThreadHandle,WM_NOTIFY,NOTIFY_CYCLE,0);
               blinkTrayIcon(g, BLINK_INIT, 2, 240);
               break;
        }

        DestroyMenu(g->hContext); g->hContext = 0;
        return 1;

   case WM_LBUTTONDBLCLK:
        // MessageBox(hWnd, "Double click", "OK", 0);
        return 1;
  }

  return 0;
}



BOOL createContextMenu(G& g)
{
  if  (g.hContext)
       DestroyMenu(g.hContext);

  int  nItem = 0;

  g.hContext = CreatePopupMenu();
  MENUITEMINFO mi;
  memset(&mi, 0, sizeof(MENUITEMINFO));
  int isInProgress = g.app.state != STATE_STARTED && g.app.state != STATE_STOPPED;

  mi.cbSize = sizeof(MENUITEMINFO);         // Start/stop server
  mi.fMask  = MIIM_ID | MIIM_TYPE | MIIM_STATE; 
  mi.wID    = IDM_STARTSTOP;    
  mi.fType  = MFT_STRING;
  mi.dwTypeData = (g.app.state == STATE_STARTED)? 
    _T("&Stop Media Engine"): _T("&Start Media Engine"); 
  mi.fState = isInProgress? MFS_DISABLED: MFS_ENABLED;
  InsertMenuItem(g.hContext, nItem++, TRUE, &mi);

  if  (g.app.state == STATE_STARTED)        
  {    
       mi.wID = IDM_SHOWHIDE;               // Show/hide server console                   
       mi.dwTypeData = (g.app.showState == SW_SHOW)?
         _T("&Hide console"): _T("S&how console"); 
       mi.fState = MFS_ENABLED;
       if  (g.app.showState == SW_LIMBO)
            mi.fState = MFS_DISABLED;
       InsertMenuItem(g.hContext, nItem++, TRUE, &mi); 

       mi.wID = IDM_REFRESHCONFIG;          // Refresh server config 
       mi.dwTypeData = _T("&Refresh configuration");
       mi.fState = MFS_ENABLED;
       InsertMenuItem(g.hContext, nItem++, TRUE, &mi); 

       mi.wID = IDM_CYCLELOG;               // Cycle server log 
       mi.dwTypeData = _T("&Cycle server log");
       mi.fState = MFS_ENABLED;
       InsertMenuItem(g.hContext, nItem++, TRUE, &mi); 
  }

  mi.wID    = 0;                            // Separator                 
  mi.fType  = MFT_SEPARATOR; 
  mi.fState = MFS_ENABLED;               
  InsertMenuItem(g.hContext, nItem++, TRUE, &mi); 
    
  mi.wID    = IDM_EXIT;                     // Exit  
  mi.fType  = MFT_STRING;
  mi.fState = g.app.state == STATE_STOPPED? MFS_ENABLED: MFS_DISABLED;       
  mi.dwTypeData = _T("E&xit");            
  InsertMenuItem(g.hContext, nItem++, TRUE, &mi); 

  return TRUE; 
}



void setTrayIcon(HWND hwnd, HICON hIcon, UINT uId, UINT umsg, LPCSTR szTip, int action)
{ 
  NOTIFYICONDATA info;
  memset(&info,0,sizeof(info));
  info.cbSize  = sizeof(info) ;
  info.hWnd    = hwnd;
  info.uID     = uId;  
  info.uCallbackMessage = umsg; 
  info.hIcon   = hIcon;    
  info.uFlags  = NIF_MESSAGE | NIF_ICON | NIF_TIP; 

  switch(action)
  { case ICON_ADD:
         if (szTip) strncpy(info.szTip, szTip, sizeof(info.szTip)-1); 
         Shell_NotifyIcon(NIM_ADD, &info);
         break;
    case ICON_CHANGE:
         if (szTip) strncpy(info.szTip, szTip, sizeof(info.szTip)-1); 
         Shell_NotifyIcon(NIM_MODIFY, &info);
         break;
    case ICON_REMOVE:
         Shell_NotifyIcon(NIM_DELETE, &info);
  }
}



void changeTrayIcon(G* g, UINT newIcon)
{
  DestroyIcon(g->wcex.hIconSm);
  g->wcex.hIconSm = LoadIcon(g->wcex.hInstance, MAKEINTRESOURCE(newIcon));
  setTrayIcon(g->hWnd, g->wcex.hIconSm, g->app.uIcon, 
              WM_MMSWIN_TRAYMSG, MMSWIN_TRAYTIP, ICON_CHANGE);
}



void blinkTrayIcon(G* g, const int action, const int numtimes, const int interval)
{
  switch(action)
  { case BLINK_INIT:
         g->bs.load(numtimes*2, interval, IDI_GREEN, IDI_YELLOW);
         SetTimer (g->hWnd, TIMER_BLINK, interval, NULL);
         break;
    case BLINK_BLINK:
         if  (++g->bs.blinks  < g->bs.times)
              changeTrayIcon(g, g->bs.blinks & 1? g->bs.colorBlink: g->bs.colorNormal); 
         else
         {    KillTimer(g->hWnd, TIMER_BLINK);
              changeTrayIcon(g, g->bs.colorNormal);      
         }
  }
}



int getMediaServerCommandLine(char* cmdline, char* workingdir)
{
  HKEY  hkey;
  DWORD dwResult, dwDataSize, dwDataType; 
                                            // Get from registry
  dwResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, MMS_REGKEY, 0, KEY_ALL_ACCESS, &hkey);
  dwResult = RegQueryValueEx(hkey, MMS_REGMMSPATH, NULL, NULL, NULL, &dwDataSize);
  if  (dwDataSize)
       dwResult = RegQueryValueEx
      (hkey, MMS_REGMMSPATH, NULL, &dwDataType, (BYTE*)cmdline, &dwDataSize);
  RegCloseKey(hkey);

  if  (dwResult != ERROR_SUCCESS)           // If not in registry use default
       strcpy(cmdline, DEFAULT_MMS_CMDLINE);

  strcpy(workingdir, cmdline);              // Strip off filename/extension
  char* q = workingdir + strlen(workingdir);// to get working directory
  while(q > workingdir && *q != '\\') q--;
  if   (q > workingdir)   *q  = '\0';

  strcat(cmdline, " /fV");                  // Start firmware service option
  return 0;
}



BOOL initApp(G& g, HINSTANCE hinst)
{ 
  memset(&g, 0, sizeof(G));
  g.hInst = hinst; 
  memcpy(g.szTitle,       MMSWIN_APPTITLE,  sizeof(MMSWIN_APPTITLE));
  memcpy(g.szWindowClass, MMSWIN_WINCLASS,  sizeof(MMSWIN_WINCLASS));
                 
  EnumWindows(findPrevInstance, (LPARAM)&g);
  if  (g.s.hwnd) return FALSE;              // Ensure singleton instance

  if  (0 == registerMainClass(g))  return FALSE;

  setInitialAppConfig(g);
  
  return InitInstance(g);
}



ATOM registerMainClass(G& g)
{ 
  g.wcex.cbSize = sizeof(WNDCLASSEX); 
  g.wcex.style  = CS_HREDRAW | CS_VREDRAW;
  g.wcex.lpfnWndProc  = (WNDPROC)wndProc;
  g.wcex.cbClsExtra   = 0;
  g.wcex.cbWndExtra   = 0;
  g.wcex.hInstance    = g.hInst;
  g.wcex.hIcon        = LoadIcon(g.hInst, MAKEINTRESOURCE(IDI_ORANGE));
  g.wcex.hCursor      = LoadCursor(NULL, IDC_ARROW);
  g.wcex.hbrBackground= (HBRUSH)(COLOR_WINDOW+1);
  g.wcex.lpszMenuName = NULL;  
  g.wcex.lpszClassName= g.szWindowClass;
  g.wcex.hIconSm      = LoadIcon(g.wcex.hInstance, MAKEINTRESOURCE(IDI_ORANGE));
  return RegisterClassEx(&g.wcex);
}



BOOL InitInstance(G& g)                     // Initialize main window
{                                               
  SystemParametersInfo(SPI_GETWORKAREA, 0, &g.workarea, 0);
  CopyRect(&g.rect, &g.workarea);            
  g.rect.left = g.workarea.right  - g.app.monWinSize.cx;
  g.rect.top  = g.workarea.bottom - g.app.monWinSize.cy;
                                             
  DWORD dwExStyle = WS_EX_CONTROLPARENT | WS_EX_PALETTEWINDOW; 
  DWORD dwStyle   = WS_DLGFRAME | WS_POPUP; // | WS_VISIBLE;
                                            // Create hidden window
  g.hWnd = CreateWindowEx(dwExStyle, g.szWindowClass, g.szTitle, dwStyle,
           g.rect.left, g.rect.top, g.app.monWinSize.cx, g.app.monWinSize.cy,
           NULL, NULL, g.hInst, NULL);
  if  (!g.hWnd) return FALSE;
                                            // Store "globals" with window 
  SetWindowLong(g.hWnd, GWL_USERDATA, (LONG)&g);
  
  ShowWindow  (g.hWnd, SW_HIDE);              
  UpdateWindow(g.hWnd);
  return TRUE;
}



BOOL CALLBACK findPrevInstance(HWND hwnd, LPARAM lParam)
{ 
  // Returns handle of any existing instance of our app in g.s.hwnd
  G* g = (G*)lParam;
  g->s.hwnd = NULL;                         // Default to no prior instance
  GetClassName(hwnd, g->s.buf, MAX_PATH);   // Compare window class with ours
  BOOL isPrevInstance = (strcmp(g->szWindowClass, g->s.buf) == 0);
  if  (isPrevInstance)
       g->s.hwnd = hwnd;                    // Indicate prior instance found
  return !isPrevInstance;                   // Keep enumerating or not    
}



void setInitialAppConfig(G& g)
{ 
  //hAccelTbl = LoadAccelerators(g.hInst, MAKEINTRESOURCE(IDC_MMSWIN));
  g.app.monWinSize.cx = MONWIN_DEFAULT_WIDTH;
  g.app.monWinSize.cy = MONWIN_DEFAULT_HEIGHT;
  g.app.state = STATE_STOPPED;
  g.app.showState = SW_SHOW;
  g.app.uIcon = IDI_ORANGE;
}



void cleanup(G* g)
{  
  setTrayIcon(g->hWnd, g->wcex.hIconSm, g->app.uIcon, 
              WM_MMSWIN_TRAYMSG, NULL, ICON_REMOVE);

  if (g->hContext) DestroyMenu(g->hContext);
}
