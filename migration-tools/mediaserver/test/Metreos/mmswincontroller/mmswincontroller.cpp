// mmswincontroller.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "windows.h"

int _tmain(int argc, _TCHAR* argv[])
{
  int iNumMinutes = 60;

  // Verify that the number of arguments is valid
  if((argc > 2) || (argc < 1)) 
  {
	printf("Invalid number of arguments!\n");
	printf("Usage: mmswincontroller [every_num_minutes]\n");
	printf("Default is to media server every 60 minutes\n");
	return -1;
  }

  // Get number of minutes to wait between restarts
  if(argc > 0)
  {
	iNumMinutes = atoi(argv[1]);
  }

  printf("The media server will be started every %i minutes.\n", iNumMinutes); 

  printf("Please make sure mmswin is running.  If mms is not, hit any key when you are ready!\n");
  char c;
	scanf("%c", &c);
 
  HWND hWnd = FindWindow("mmswin", "Metreos MediaServer");

  if (hWnd == NULL)
  {
    printf("Unable to find mmswin, you need to run it first!\n");
    return -1;
  }

  do 
  {
    // Start mms
    printf("Start mms!\n");
    PostMessage(hWnd, WM_COMMAND, 141L, 0L);
    Sleep(1000*60*iNumMinutes);  
    // Stop mms
    printf("Stop mms!\n");
    PostMessage(hWnd, WM_COMMAND, 141L, 0L);

	//Sleep for 3 minutes
    Sleep(1000*60*3);  
    HWND hWndCmd = FindWindow("ConsoleWindowClass", "Metreos MediaServer Console");
    if (hWndCmd)
    {
      PostMessage(hWndCmd, WM_KEYDOWN, VK_RETURN, 0); 

	  // Sleep 60 seconds
      Sleep(1000*60);  
    }
  } 
  while(1);

	return 0;
}

