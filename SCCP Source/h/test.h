/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */
#ifndef _TEST_H_
#define _TEST_H_


#ifndef SCCP_SAPP

#define AUDIO_ENABLE
#define CELL_ENABLE
#define BLUETOOTH_ENABLE
#define HANDOFF_ENABLE

#endif

#include <stdio.h>
#include <stdlib.h>
#ifdef SCCP_PLATFORM_WINDOWS
#include <conio.h>
#include <winsock2.h>
#endif
#ifdef SCCP_PLATFORM_POCKET_PC
#include <winsock.h>
#endif

#include <debug_tools.h>
#include <sccp_ice_events.h>
#include <cli_menu.h>
#include <sccp_ice.h>
#ifndef SCCP_SAPP
#include <generic_timers.h>
#include <strong_random.h>
#endif

enum 
{
	TEST_COMMAND_QUIT,
	TEST_COMMAND_CALL_BY_DIGIT,
	TEST_COMMAND_CALL_BY_DIALSTRING,
	TEST_COMMAND_HANGUP,
	TEST_COMMAND_ANSWER,
	TEST_COMMAND_OFFHOOK,
	TEST_COMMAND_SEND_DEVICE_DATA,
	TEST_COMMAND_SWITCH_LINE,
	TEST_COMMAND_REGISTER,
	TEST_COMMAND_UNREGISTER,
	TEST_COMMAND_INVALID
};


extern cli_menu_state		*menu;
extern cli_menu_state		*sccp_menu;
extern void					*sccp_menu_state;
extern void					*sccp_menu_event_data;
extern void					*sccp_menu_callback_data;
#ifndef SCCP_SAPP
extern strong_random_state	*rand_state;
#endif

#endif