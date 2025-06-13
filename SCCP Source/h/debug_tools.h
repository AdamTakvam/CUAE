/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */
#ifndef _DEBUG_TOOLS_H_
#define _DEBUG_TOOLS_H_

#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC
#include <crtdbg.h>
#include <stdio.h>
#include <conio.h>

#ifndef DEBUG_CONSOLE_OUTPUT
#define printf  debug_printf
#endif

#define INIT_DEBUG          initialize_debug_system();
#define SET_OUTPUT_FILE(x)  open_output_file(x);

void
initialize_debug_system(void);

void
open_output_file(char *filename);

extern int
debug_printf(const char *format, ...);


#else	// _DEBUG not defined
#include <stdio.h>

#define INIT_DEBUG
#define SET_OUTPUT_FILE(x)

#endif

#endif