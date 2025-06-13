/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */
#define DEBUG_CONSOLE_OUTPUT
#include "platform.h"
#define _CRTDBG_MAP_ALLOC
#include "debug_tools.h"


//#define SAPP_PLATFORM_WIN32
#ifdef _DEBUG
#ifdef SAPP_PLATFORM_WIN32
#include <windows.h>
#include <winbase.h>
#endif
#include <stdio.h>
#include <stdarg.h>

//#define _CRTDBG_MAP_ALLOC

static FILE *debug_output = NULL;
static long output_destination = 0;

void
initialize_debug_system(void)
{
#ifdef SAPP_PLATFORM_WIN32
    _HFILE report_file;
    OFSTRUCT open_params;

    report_file = (void *)OpenFile("debug_rpt.txt", &open_params, OF_DELETE);
    report_file = (void *)OpenFile("debug_rpt.txt", &open_params, OF_CREATE|OF_READWRITE);

//    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF|_CRTDBG_DELAY_FREE_MEM_DF|_CRTDBG_LEAK_CHECK_DF);
//    _CrtSetDbgFlag(_CRTDBG_CHECK_ALWAYS_DF|_CRTDBG_ALLOC_MEM_DF|_CRTDBG_DELAY_FREE_MEM_DF|_CRTDBG_LEAK_CHECK_DF);
    //_CrtSetDbgFlag(_CRTDBG_CHECK_ALWAYS_DF|_CRTDBG_ALLOC_MEM_DF|_CRTDBG_DELAY_FREE_MEM_DF|_CRTDBG_LEAK_CHECK_DF|_CRTDBG_CHECK_CRT_DF);    
    _CrtSetDbgFlag(_CRTDBG_CHECK_ALWAYS_DF|_CRTDBG_ALLOC_MEM_DF|_CRTDBG_DELAY_FREE_MEM_DF|_CRTDBG_LEAK_CHECK_DF);    
    _CrtSetReportMode(_CRT_WARN, _CRTDBG_MODE_FILE | _CRTDBG_MODE_DEBUG);
    //_CrtSetReportFile(_CRT_WARN, report_file);
    _CrtSetReportFile(_CRT_WARN, _CRTDBG_FILE_STDOUT);
    _CrtSetReportMode(_CRT_ERROR, _CRTDBG_MODE_FILE | _CRTDBG_MODE_DEBUG);
    //_CrtSetReportFile(_CRT_ERROR, report_file);
    _CrtSetReportFile(_CRT_ERROR, _CRTDBG_FILE_STDOUT);
    _CrtSetReportMode(_CRT_ASSERT, _CRTDBG_MODE_FILE | _CRTDBG_MODE_DEBUG);
    //_CrtSetReportFile(_CRT_ASSERT, report_file);
    _CrtSetReportFile(_CRT_ASSERT, _CRTDBG_FILE_STDOUT);
    _RPTF0(_CRT_WARN, "Starting program\n");
#endif    
}


void
open_output_file(char *filename)
{
    if (debug_output != NULL)
    {
        fclose(debug_output);
    }

    debug_output = fopen(filename, "w+");
    rewind(debug_output);
}


int
debug_printf(const char *format, ...)
{
    long output_count;
    va_list list;

    va_start(list, format);
    if (debug_output == NULL)
    {
        output_count = vprintf(format, list);
    }
    else
    {
        output_count = vfprintf(debug_output, format, list);
        fflush(debug_output);
    }

    return(output_count);
}

#endif