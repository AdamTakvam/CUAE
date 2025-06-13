/*
 *  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
 *
 *  This work is subject to U.S. and international copyright laws and
 *  treaties. No part of this work may be used, practiced, performed,
 *  copied, distributed, revised, modified, translated, abridged, condensed,
 *  expanded, collected, compiled, linked, recast, transformed or adapted
 *  without the prior written consent of Cisco Systems, Inc. Any use or 
 *  exploitation of this work without authorization could subject the 
 *  perpetrator to criminal and civil liability.
 */

#define DEBUG_CONSOLE_OUTPUT

#include "platform.h"
#include "debug_tools.h"
#include "test.h"

#if 0
#ifdef SCCP_SAPP
// Required by Microsoft C++ for pre-compiled headers
// Define a blank include file for all other platforms
#include <stdafx.h>
//#include <winsock2.h>
#endif /* SCCP_SAPP */
#endif

#ifdef AUDIO_ENABLE
#include <test_audio.h>
#endif

#ifdef CELL_ENABLE
#include <test_cell.h>
#endif

#ifdef HANDOFF_ENABLE
#include <test_bluetooth.h>
#endif

#ifdef HANDOFF_ENABLE
#include <test_handoff.h>
#endif

#ifdef SAPP_PLATFORM_WIN32
#include <conio.h>
#endif

#ifdef SCCP_PLATFORM_POCKET_PC
#include <winsock.h>
#endif

#ifdef SAPP_PLATFORM_UNIX
#ifndef SAPP_PLATFORM_UNIX_WIN  // used when compiling under windows
#include <arpa/inet.h>
#include <string.h>
#include <unistd.h>
#endif
#endif /* SAPP_PLATFORM_UNIX */

#if 0
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

typedef int sccp_state; 
typedef int cli_menu_state; 
#endif

static int sccptest_exit_status = 0;
static int sccptest_xfer_flag   = 0;

//extern gapi_cmaddr_t sapp_cmaddr;

#ifdef SCCP_SAPP
extern "C"
{
#include "sapp.h"
#include "timer.h"
#include "gapi.h"
#include "ssapi.h"

//extern gapi_cmaddr_t sapp_cmaddr;
static int sccp_test_addr_count = 0;
static unsigned long  sccp_test_addrs[5];
static unsigned short sccp_test_ports[5];

void sccp_test_get_cm_addr1 (char *addrstr, char *portstr, unsigned long *addr, unsigned short *port)
{
    /*
     * Keep the address and port in network order.
     */
    *port = htons((unsigned short)(atol(portstr)));
    *addr = inet_addr(addrstr);
}

int sccptest_get_new_call_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}

};
//#include "sccptest.h"
#endif /* #ifdef SCCP_SAPP */


// DEBUG: Remove when we add the enable/disable to the menu system
#define MENU_TITLE(x)       {strcpy(call_state, x);}
#define SCCP_MENU_ADD(x) snprintf(cmd_line, sizeof(cmd_line), "%010d", x);    menu_add_command_ex(sccp_menu, cmd_line, #x, sccp_menu_func, 0, x, (long)&sccp_menu_state, (long)&sccp_menu_event_data, (long)&sccp_menu_callback_data);



// Private function declarations
long sccp_callback(sccp_state *state, SCCP_STACK_EVENT event, sccp_event_data *event_data, void *callback_data);
void print_menu(void);
int menu_func(const char *cmd_line, unsigned long menu_id, long param1, long param2, long param3, long param4);
int sccp_menu_func(const char *cmd_line, unsigned long menu_id, long param1, long param2, long param3, long param4);
long init_routine(void);
void shutdown_routine(void);
static void sccptest_print_menu_str (char *str);

#define SCCPTEST_DEFAULT_LINE 0
#define SCCPTEST_DEFAULT_CALL 0


// Global variable declarations
char            local_mac[17];
char            call_state[255];
long            active_line = SCCPTEST_DEFAULT_LINE;
int             active_call = SCCPTEST_DEFAULT_CALL;
cli_menu_state  *menu;
cli_menu_state  *sccp_menu;
void            *sccp_menu_state;
void            *sccp_menu_event_data;
void            *sccp_menu_callback_data;
sccp_state      *client;

typedef enum sccptest_states_e_ {
    SCCPTEST_S_MIN = -1,
    SCCPTEST_S_CLOSED,
    SCCPTEST_S_OPENED,
    SCCPTEST_S_QUITTING,
    SCCPTEST_S_MAX
} sccptest_states_e;

static  sccptest_states_e sccptest_state = SCCPTEST_S_CLOSED;

#ifdef SCCP_SAPP

#define SCCPTEST_MAX_LINES 1 //2
#define SCCPTEST_MAX_CALLS 2 //2

typedef struct sccptest_call_t_ {
    int call_id;
    int state;
} sccptest_call_t;

typedef struct sccptest_line_t_ {
    sccptest_call_t calls[SCCPTEST_MAX_CALLS];
} sccptest_line_t;

static sccptest_line_t sccptest_lines[SCCPTEST_MAX_LINES];

typedef enum sccptest_menu_states_e_ {
    SCCPTEST_MENU_STATE_MIN = -1,
    SCCPTEST_MENU_STATE_CLOSED,
	SCCPTEST_MENU_STATE_OPENING,
    SCCPTEST_MENU_STATE_OPENED,
    SCCPTEST_MENU_STATE_INCOMING,
    SCCPTEST_MENU_STATE_OUTGOING,
//    SCCPTEST_MENU_STATE_OFFHOOK,
    SCCPTEST_MENU_STATE_ONHOOK,
//    SCCPTEST_MENU_STATE_ALERTING,
//    SCCPTEST_MENU_STATE_CONNECTED,
    SCCPTEST_MENU_STATE_ACTIVE,
    SCCPTEST_MENU_STATE_HOLDING,
    SCCPTEST_MENU_STATE_MAX
} sccptest_menu_states_e;

static char *sccptest_menu_states[] = {
    "CLOSED",
    "OPENING",
	"OPENED",
//    "OFFHOOK",
    "INCOMING",
    "OUTGOING",
    "ONHOOK",
//    "ALERTING",
//    "CONNECTED",
    "ACTIVE",
    "HOLDING"
};

typedef enum sccptest_commands_e_ { 
    SCCPTEST_COMMAND_MIN = -1,
    SCCPTEST_COMMAND_QUIT,
    SCCPTEST_COMMAND_OPEN,
    SCCPTEST_COMMAND_RESET,
    SCCPTEST_COMMAND_SHOW,
    SCCPTEST_COMMAND_LINE,
    SCCPTEST_COMMAND_OFFHOOK,
    SCCPTEST_COMMAND_ONHOOK,
    SCCPTEST_COMMAND_OVERLAP,
    SCCPTEST_COMMAND_ENBLOC,
    SCCPTEST_COMMAND_USER_DATA,
    SCCPTEST_COMMAND_FEATURE,
    SCCPTEST_COMMAND_MAX,
} sccptest_commands_e;

static char *sccptest_commands[] = 
{
    "q",
    "o",
    "r",
    "s",
    "l",
    "k",
    "h",
    "c",
    "d",
    "u",
    "f"
};  

typedef struct sccptest_options_t_ {
    MENU_ITEM_OPTION option1;
    MENU_ITEM_OPTION option2;
} sccptest_options_t;

static sccptest_options_t sccptest_menu_closed[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* RESET    */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL}, 
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}, 
    /* OFFHOOK  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}, 
    /* ONHOOK   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}, 
    /* OVERLAP  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}, 
    /* ENBLOC   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}, 
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}
};

static sccptest_options_t sccptest_menu_opening[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* RESET    */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OFFHOOK  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ONHOOK   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OVERLAP  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ENBLOC   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}
};

static sccptest_options_t sccptest_menu_opened[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* RESET    */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OFFHOOK  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ONHOOK   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_REVEAL},
    /* OVERLAP  */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* ENBLOC   */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL}
};

static sccptest_options_t sccptest_menu_onhook[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* RESET    */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OFFHOOK  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ONHOOK   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_REVEAL}, 
    /* OVERLAP  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ENBLOC   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL}
};

static sccptest_options_t sccptest_menu_incoming[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* RESET    */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OFFHOOK  */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* ONHOOK   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_REVEAL}, 
    /* OVERLAP  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ENBLOC   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}
};

static sccptest_options_t sccptest_menu_outgoing[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* RESET    */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OFFHOOK  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ONHOOK   */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL}, 
    /* OVERLAP  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ENBLOC   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
};

static sccptest_options_t sccptest_menu_active[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* RESET    */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OFFHOOK  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ONHOOK   */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL}, 
    /* OVERLAP  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ENBLOC   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL}
};

static sccptest_options_t sccptest_menu_holding[] = 
{
    /* QUIT     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* OPEN     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* RESET    */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* SHOW     */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL},
    /* LINE     */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE}, 
    /* OFFHOOK  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ONHOOK   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* OVERLAP  */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* ENBLOC   */ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* USER_DATA*/ {MENU_ITEM_OPTION_DISABLE, MENU_ITEM_OPTION_HIDE},
    /* FEATURE  */ {MENU_ITEM_OPTION_ENABLE,  MENU_ITEM_OPTION_REVEAL}
};

static sccptest_options_t *sccptest_option_table[] = 
{
    sccptest_menu_closed,
	sccptest_menu_opening,
    sccptest_menu_opened,
    sccptest_menu_incoming,
    sccptest_menu_outgoing,
    sccptest_menu_onhook,
    sccptest_menu_active,
    sccptest_menu_holding
};

typedef struct sccptest_menu_table_t_ {
    cli_menu_state *menu;
    sccptest_options_t **options;
} sccptest_menu_table_t;

static sccptest_menu_table_t sccptest_menu_table = 
{
    menu, sccptest_option_table
};

sccptest_call_t *sccptest_get_call (int line, int call_id)
{
    int i, j;

    for (i = line; i < SCCPTEST_MAX_LINES; i++) {
        for (j = 0; j < SCCPTEST_MAX_CALLS; j++) {
            if (sccptest_lines[i].calls[j].call_id == call_id) {
                return (&(sccptest_lines[i].calls[j]));
            }
        }
    }

    return (NULL);
}

void sccptest_free_call (int line, int call_id)
{
    sccptest_call_t *call;

    call = sccptest_get_call(line, call_id);
    if (call != NULL) {
        call->call_id = 0;
    }
}
void sccptest_set_menu_state (sccptest_menu_states_e state, int line, int call_id)
{
    sccptest_options_t *options;
    int i;
    sccptest_call_t *call;

    call = sccptest_get_call(line, call_id);
    if (call == NULL) {
        return;
    }

    call->state = state;

#if 1
    if (call_id != active_call) {
        return;
    }

    options = sccptest_option_table[state];

    for (i = SCCPTEST_COMMAND_QUIT; i < SCCPTEST_COMMAND_MAX; i++) {
        menu_item_configure(menu, sccptest_commands[i], options->option1);
        menu_item_configure(menu, sccptest_commands[i], options->option2);

        options++;
    }
#endif

    //print_menu();
}

void sccptest_set_menu_state_all (sccptest_menu_states_e state)
{
    int i, j;

    for (i = 0; i < SCCPTEST_MAX_LINES; i++) {
        for (j = 0; j < SCCPTEST_MAX_CALLS; j++) {
            sccptest_lines[i].calls[j].state = state;
        }
    }

    sccptest_set_menu_state(state, 0, 0);
}

#endif

#ifdef SCCP_SAPP
#ifdef __cplusplus
extern "C" {
#endif
#endif /*SCCP_SAPP */

void sccptest_callback(int event, void *data, void *callback_data)
{
    sccp_callback(client, (SCCP_STACK_EVENT)event, (sccp_event_data *)data, callback_data);
}
#ifdef SCCP_SAPP
#ifdef __cplusplus
}
#endif
#endif /* SCCP_SAPP */


#ifndef _WINDOWS
int
main(int argc, char **argv)
{
    long                err = 0;
    char                cmd_line[MAX_MENU_COMMAND_LENGTH];
#ifdef SCCP_SAPP
    int sapp_rc;
    int i;
#endif /* #ifdef SCCP_SAPP */


    INIT_DEBUG;
    SET_OUTPUT_FILE("message.txt");

    //rand_state = NULL;
    //seed_strong_random(NULL);

#ifdef SCCP_SAPP
    if ((argc < 4) ||
        ((argc % 2) == 1)) {
        printf("Usage: sccp_test <MAC Address> <Call Manager IP Address> <Port> [<Call Manager IP Address> <Port>]\n");
        return(1);
    }
#else
    if (argc != 3)
    {
        printf("Usage: sccp_test {Call Manager IP Address} {MAC Address}\n");
        return(1);
    }
#endif /* #ifdef SCCP_SAPP */
    active_line = SCCPTEST_DEFAULT_LINE;
    active_call = SCCPTEST_DEFAULT_CALL;
#ifdef SCCP_SAPP
    strncpy(local_mac, argv[1], sizeof(local_mac) - 1);
#else
    strncpy(local_mac, argv[2], sizeof(local_mac) - 1);
#endif
    local_mac[sizeof(local_mac) - 1] = '\0';

#ifdef SCCP_SAPP
    memset(&sccptest_lines, 0, sizeof(sccptest_lines));
    timer_event_system_init();
    sapp_rc = sapp_init();
    if (sapp_rc != 0) {
        printf("sapp init error: rc= %d\n", sapp_rc);
        cmd_line[0] = getchar();
        return (3);
    }

    printf("\n");
    for (i = 0; i < 5; i++) {
        unsigned char *p;
        
        if ((argc - (i * 2 + 2)) > 1) {
            sccp_test_addr_count++;
            sccp_test_get_cm_addr1(argv[i * 2 + 2],
                                   argv[i * 2 + 2 + 1],
                                   &(sccp_test_addrs[i]),
                                   &(sccp_test_ports[i]));

            printf("CM[%d]: addr:port= 0x%08lx:%04x\n",
                   i, sccp_test_addrs[i], sccp_test_ports[i]);

            p = (unsigned char *)(&(sccp_test_addrs[i]));

            printf("CM[%d] %d.%d.%d.%d:%d\n",
                   i, p[0], p[1], p[2], p[3], ntohs(sccp_test_ports[i]));
        }
    }
    printf("\n");
#else
    // Initialize timer subsystem
    generic_timer_init(500);
#endif

    sccp_menu       = menu_init();
    SCCP_MENU_ADD(SCCP_STACK_EVENT_OFFHOOK);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CALL_SETUP);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CALL_SETUP_ACK);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CALL_ALERTING);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CALL_PROCEEDING);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CALL_CONNECTED);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CALL_RELEASE);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_FEATURE);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_MEDIA_RECEIVE_START);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_MEDIA_RECEIVE_STOP);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_MEDIA_TRANSMIT_START);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_MEDIA_TRANSMIT_STOP);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CM_CONNECTED);
    SCCP_MENU_ADD(SCCP_STACK_EVENT_CM_DISCONNECTED);


    menu            = menu_init();
    
    menu_add_command_ex(menu, "z",          "test",                         menu_func, 0, 'z', 0, 0, 0);
    menu_item_configure(menu, "z",          MENU_ITEM_OPTION_HIDE);
    
    menu_add_command_ex(menu, "q",          "Quit",                         menu_func, 0, TEST_COMMAND_QUIT,                0, 0, 0);
    menu_add_command_ex(menu, "Q",          "Quit",                         menu_func, 0, TEST_COMMAND_QUIT,                0, 0, 0);
#ifdef SCCP_SAPP
    menu_add_command_ex(menu, "o",          "Open session",                  menu_func, 0, 'o', 0, 0, 0);
    menu_add_command_ex(menu, "r",          "Reset session",                 menu_func, 0, 'r', 0, 0, 0);
    menu_add_command_ex(menu, "s",          "Show session",                  menu_func, 0, 's', 0, 0, 0);
#endif
    menu_add_command_ex(menu, "l",          "Switch line",                  menu_func, 0, TEST_COMMAND_SWITCH_LINE,         0, 0, 0);
    menu_add_command_ex(menu, "k",          "Go offhook",                   menu_func, 0, TEST_COMMAND_OFFHOOK,             0, 0, 0);
    menu_add_command_ex(menu, "h",          "Hangup",                       menu_func, 0, TEST_COMMAND_HANGUP,              0, 0, 0);
    menu_add_command_ex(menu, "u",          "Send Device->User data",       menu_func, 0, TEST_COMMAND_SEND_DEVICE_DATA,    0, 0, 0);
    menu_add_command_ex(menu, "c",          "Call via individual digits",   menu_func, 0, TEST_COMMAND_CALL_BY_DIGIT,       0, 0, 0);
    menu_add_command_ex(menu, "d",          "Call via complete dialstring", menu_func, 0, TEST_COMMAND_CALL_BY_DIALSTRING,  0, 0, 0);
#ifdef SCCP_SAPP
    menu_add_command_ex(menu, "f",          "Feature",                      menu_func, 0, 'f', 0, 0, 0);
#endif

#ifndef SCCP_SAPP
    menu_add_command_ex(menu, "a",          "Answer",                       menu_func, 0, TEST_COMMAND_ANSWER,              0, 0, 0);
    menu_add_command_ex(menu, "register",   "Register with CallManager",    menu_func, 0, TEST_COMMAND_REGISTER,            0, 0, 0);
    menu_add_command_ex(menu, "unregister", "Unregister with CallManager",  menu_func, 0, TEST_COMMAND_UNREGISTER,          0, 0, 0);
    menu_item_configure(menu, "a",          MENU_ITEM_OPTION_DISABLE);
#endif
    menu_item_configure(menu, "Q",          MENU_ITEM_OPTION_HIDE);
    menu_item_configure(menu, "c",          MENU_ITEM_OPTION_DISABLE);
    menu_item_configure(menu, "d",          MENU_ITEM_OPTION_DISABLE);
    menu_item_configure(menu, "c",          MENU_ITEM_OPTION_HIDE);
    menu_item_configure(menu, "d",          MENU_ITEM_OPTION_HIDE);
    menu_item_configure(menu, "u",          MENU_ITEM_OPTION_DISABLE);

#ifdef SCCP_SAPP
    sccptest_set_menu_state_all(SCCPTEST_MENU_STATE_CLOSED);
#endif

    err = init_routine();
    if (err != 0)
    {
        return(err);
    }

#ifndef SCCP_SAPP
    client = sccp_client_create();
    sccp_client_init(client, sccp_callback, NULL);

    err = sccp_client_cm_add(client, argv[1], 2000, 1);
    if (err != 0)
    {
        printf("Error adding CM to CM registration list, aborting\n");
        shutdown_routine();

        sccp_client_destroy(client);
        menu_destroy(menu);
        menu_destroy(sccp_menu);

        return(3);
    }
    err = sccp_client_register(client);
    if (err != 0)
    {
        printf("Unable to connect to Call Manager, aborting.\n");

        shutdown_routine();

        sccp_client_destroy(client);
        menu_destroy(menu);
        menu_destroy(sccp_menu);

        return(2);
    }
#endif /* #ifndef SCCP_SAPP */

    MENU_TITLE("Idle");
    // Provide CLI control menu
    do
    {
        print_menu();

        gets(cmd_line);
        if (cmd_line[0] == '\0') {
            ;//print_menu();
        }
        else {
            menu_process_command_line(menu, cmd_line);
        }

        cmd_line[0] = '\0';
    } while (sccptest_exit_status == 0);
//    } while (1);
//    } while (cmd_line[0] != 'q' && cmd_line[0] != 'Q');

#ifndef SCCP_SAPP
    sccp_client_unregister(client);
    shutdown_routine();

//    sccp_client_destroy(client);

    menu_destroy(menu);
    menu_destroy(sccp_menu);

#endif
    return(err);
}
#endif

#ifdef SCCP_SAPP
#define sccp_num_lines_get(client)             sapp_get_num_lines()
#define sccp_line_name_get(client, line)       sapp_get_line_name(line)
#define sccp_line_idle_check(client, new_line) (1)

/*
 * This function is a wrapper for sapp. sccptest calls this function to call sapp
 * which then calls the sccp callbacks.
 */
void sccp_sm_event(struct sccp_client_state *client, unsigned long event,
                   struct sccp_event_data *data)
{
    switch (event) {
    case (SCCP_APP_EVENT_CALL_SETUP):
        sapp_setup(data->call_reference, data->line_number,
                   NULL, data->body.setup_data.dialstring,
                   strlen(data->body.setup_data.dialstring), NULL,
                   GAPI_ALERT_INFO_MIN, GAPI_PRIVACY_MIN, sccptest_xfer_flag);
        break;

    case (SCCP_APP_EVENT_FEATURE):
        switch (data->type) {
        case (SCCP_FEATURE_DIGIT):
            sapp_digits(data->call_reference, data->line_number,
                       &(data->body.digit_data), 1);
            break;

        case (SCCP_FEATURE_ANSWER):
            sapp_connect(data->call_reference, data->line_number, NULL);

            break;

        case (SCCP_FEATURE_DEVICE_DATA):
            break;

        case (SCCP_FEATURE_TRANSFER):
            sapp_feature_req(data->call_reference, data->line_number,
                             GAPI_FEATURE_TRANSFER, NULL, GAPI_CAUSE_OK);
            break;

        case (SCCP_FEATURE_HOLD):
            sapp_feature_req(data->call_reference, data->line_number,
                             GAPI_FEATURE_HOLD, NULL, GAPI_CAUSE_OK);
            break;

        case (SCCP_FEATURE_RESUME):
            sapp_feature_req(data->call_reference, data->line_number,
                             GAPI_FEATURE_RESUME, NULL, GAPI_CAUSE_OK);
            break;

        case (SCCP_FEATURE_REDIAL):
            sapp_feature_req(data->call_reference, data->line_number,
                             GAPI_FEATURE_REDIAL, NULL, GAPI_CAUSE_OK);
            break;

        case (SCCP_FEATURE_SPEEDDIAL):
            sapp_feature_req(data->call_reference, data->line_number,
                             GAPI_FEATURE_SPEEDDIAL, NULL, GAPI_CAUSE_OK);
            break;

	default:
	    break;
        }

        break;

    case (SCCP_APP_EVENT_CALL_RELEASE):
        sccptest_call_t *call;
        sccp_event_data data2;

        call = sccptest_get_call(active_line, active_call);
        if (call == NULL) {
            return;
        }

        if (call->state == SCCPTEST_MENU_STATE_OUTGOING) {
            sapp_release_complete(data->call_reference, data->line_number,
                                  GAPI_CAUSE_OK);
    
            data2.line_number    = active_line;
            data2.call_reference = active_call;

            sccptest_callback(SCCP_STACK_EVENT_CALL_RELEASE, &data2, NULL);
        }
        else {
            sapp_release(data->call_reference, data->line_number,
                         GAPI_CAUSE_OK);
        }

        break;

    case (SCCP_APP_EVENT_OFFHOOK): {
        sccptest_call_t *call;

        //sapp_offhook(data->call_reference, data->line_number);
        call = sccptest_get_call(active_line, active_call);
        if (call == NULL) {
            return;
        }
        if (call->state == SCCPTEST_MENU_STATE_INCOMING) {
            sapp_connect(active_call, active_line, NULL);
        }
        else {
            sapp_setup(data->call_reference, data->line_number,
                       NULL, NULL, 0, NULL,
                       GAPI_ALERT_INFO_MIN, GAPI_PRIVACY_MIN, sccptest_xfer_flag);
        }

        break;
    }
    
    case (SCCP_APP_EVENT_SELECT_LINE):
        print_menu();
        break;
    
    default:
        break;
    }
}
#endif

static int sccptest_exit ()
{
    char c;

    printf("Press any key to exit sccptest...");
    c = getchar();

    sccptest_exit_status = 1;

    return (1);
}

int
menu_func(const char *cmd_line, unsigned long menu_id, long param1, long param2, long param3, long param4)
{
    char c = '\0';;
    char buffer[1024];

    switch(param1)
    {
    case TEST_COMMAND_QUIT:
        {
            printf("TEST: Quiting\n");
            if (sccptest_state == SCCPTEST_S_OPENED) {
                sccptest_state = SCCPTEST_S_QUITTING;

                /*
                 * Try to shutdown gracefully if the user quits. We do this
                 * by sending a close command.
                 */
                printf("TEST: Closing SCCP session...\n");
                memset(&sccptest_lines, 0, sizeof(sccptest_lines));
                sapp_resetsession_req(GAPI_CAUSE_CM_RESET);
                sccptest_set_menu_state(SCCPTEST_MENU_STATE_CLOSED, active_line, active_call);
                print_menu();
            }
            else {
                print_menu();

                gapi_cleanup();
                ssapi_cleanup();
                sapp_cleanup();

                shutdown_routine();

                menu_destroy(menu);
                menu_destroy(sccp_menu);

                sccptest_exit();
                exit(0);
            }

            break;
        }
    case TEST_COMMAND_CALL_BY_DIGIT:
        {
            sccp_event_data data;
            data.type           = SCCP_FEATURE_DIGIT;
            data.line_number    = active_line;
#ifdef SCCP_SAPP
            sccptest_call_t *call;

            call = sccptest_get_call(active_line, 0);
            if (call == NULL) {
                printf("no free calls.\n");
                break;
            }

            data.call_reference = sccptest_get_new_call_id();
            active_call = data.call_reference;
            call->call_id = data.call_reference;
            call->state = SCCPTEST_MENU_STATE_OPENED;
#else
            data.call_reference = 0;
#endif
            printf("Dial via single digits\n");
            printf("Press Enter to exit dialing\n");
            
            sccp_sm_event(client, SCCP_APP_EVENT_OFFHOOK, &data);
            do {
                printf("Digit [0-9,A-D,*,#]: ");
                c = getchar();
                if ((c == '\r') || (c == '\n')) {
                    print_menu();
                    break;
                }
                data.body.digit_data = c;

                sccp_sm_event(client, SCCP_APP_EVENT_FEATURE, &data);
                
                printf("\n");
            } while ((c != '\r') && (c != '\n'));
            
            break;
        }
    case TEST_COMMAND_CALL_BY_DIALSTRING:
        {
            sccp_event_data data;
            sccp_setup_data *setup = &data.body.setup_data;
            sccptest_call_t *call;

            memset(setup, 0, sizeof(data.body.setup_data));

            call = sccptest_get_call(active_line, 0);
            if (call == NULL) {
                printf("no free calls.\n");
                break;
            }
            
            if (cmd_line == NULL)
            {
                printf("Number to dial: ");
                
                fgets(setup->dialstring, sizeof(setup->dialstring) - 1, stdin);
            }
            else
            {
                strncpy(data.body.setup_data.dialstring, cmd_line, sizeof(data.body.setup_data.dialstring));
            }
            data.body.setup_data.dialstring[sizeof(data.body.setup_data.dialstring) - 1] = '\0';
            
            if (data.body.setup_data.dialstring[strlen(data.body.setup_data.dialstring) - 1] != '\0' && cmd_line == NULL)
            {
                data.body.setup_data.dialstring[strlen(data.body.setup_data.dialstring) - 1] = '\0';
            }
            
            if (strlen(data.body.setup_data.dialstring) > 0)
            {
                printf("ACTION: Dialing: %s\n", data.body.setup_data.dialstring);
                data.line_number    = active_line;
#ifdef SCCP_SAPP
                data.call_reference = sccptest_get_new_call_id();
                active_call = data.call_reference;
                call->call_id = data.call_reference;
                call->state = SCCPTEST_MENU_STATE_OPENED;
#else
                data.call_reference = 0;
#endif

                sccptest_set_menu_state(SCCPTEST_MENU_STATE_OUTGOING, active_line, active_call);                
                sccp_sm_event(client, SCCP_APP_EVENT_CALL_SETUP, &data);
            }
            else
            {
                printf("No dialstring to dial.\n");
            }
            break;
        }
    case TEST_COMMAND_HANGUP:
        {
#ifdef SCCP_SAPP
            sccp_event_data data;
            data.type           = SCCP_FEATURE_END_CALL;
            data.line_number    = active_line;

            data.call_reference = active_call;

            sccptest_print_menu_str("ACTION: Going onhook\n");
            
            sccp_sm_event(client, SCCP_APP_EVENT_CALL_RELEASE, &data); 

//            sccptest_set_menu_state(SCCPTEST_MENU_STATE_ONHOOK, active_line, active_call);
#endif
            break;
        }
    case TEST_COMMAND_ANSWER:
        {
            sccp_event_data data;
            data.line_number        = active_line;
#ifdef SCCP_SAPP
            data.call_reference = active_call;
#else
            data.call_reference = 0;
#endif
            
            printf("ACTION: Answering call\n");
            
            data.type = SCCP_FEATURE_ANSWER;
            sccp_sm_event(client, SCCP_APP_EVENT_FEATURE, &data);
            break;
        }
    case 'f': {
        sccp_event_data feature;

        feature.line_number    = active_line;
        feature.call_reference = active_call;

        if (cmd_line != NULL) {
            switch (cmd_line[0]) {
            case ('h'):
                /*
                 * Add code to block hold/resume if not in the active state.
                 */
                feature.type = SCCP_FEATURE_HOLD;
//                sccptest_set_menu_state(SCCPTEST_MENU_STATE_HOLDING, active_line, active_call);
                break;

            case ('r'):
                feature.type = SCCP_FEATURE_RESUME;
                sccptest_set_menu_state(SCCPTEST_MENU_STATE_ACTIVE, active_line, active_call);
                break;

            case ('d'):
                feature.type = SCCP_FEATURE_REDIAL;
                sccptest_set_menu_state(SCCPTEST_MENU_STATE_ACTIVE, active_line, active_call);
                break;

            case ('s'):
                feature.type = SCCP_FEATURE_SPEEDDIAL;
                sccptest_set_menu_state(SCCPTEST_MENU_STATE_ACTIVE, active_line, active_call);
                break;

            case ('x'):
                feature.type = SCCP_FEATURE_TRANSFER;
                break;

            default:
                feature.type = SCCP_FEATURE_INVALID;
                break;
            }
            
            printf("ACTION: Feature: %d\n", feature.type);
            
            sccp_sm_event(client, SCCP_APP_EVENT_FEATURE, &feature);
        }
        else {
            printf("Invalid or missing feature.\n");
        }

        break;
    }

    case TEST_COMMAND_SEND_DEVICE_DATA:
        {
            char buffer[512];
            sccp_event_data feature;
            feature.line_number    = active_line;
#ifdef SCCP_SAPP
            feature.call_reference = active_call;
#else
            feature.call_reference = 0;
#endif
            
            printf("Data to send: ");
            fgets(feature.body.user_data.data, sizeof(feature.body.user_data.data) - 1, stdin);
            printf("Application ID: ");
            fgets(buffer, sizeof(buffer) - 1, stdin);
            feature.body.user_data.application_id = strtoul(buffer, NULL, 0);
            printf("Transaction ID: ");
            fgets(buffer, sizeof(buffer) - 1, stdin);
            feature.body.user_data.transaction_id = strtoul(buffer, NULL, 0);

            feature.type    = SCCP_FEATURE_DEVICE_DATA;
            feature.body.user_data.data[sizeof(feature.body.user_data.data) - 1] = '\0';
            // Remove the CR from the data stream
            feature.body.user_data.data[strlen(feature.body.user_data.data)] = '\0';
            feature.body.user_data.data_len = strlen(feature.body.user_data.data);
            printf("ACTION: Sending: %s\n", feature.body.user_data.data);
            
            sccp_sm_event(client, SCCP_APP_EVENT_FEATURE, &feature);
            break;
        }
    case TEST_COMMAND_OFFHOOK:
        {
            sccp_sm_event(client, SCCP_APP_EVENT_OFFHOOK, NULL);
            break;
        }
    case TEST_COMMAND_SWITCH_LINE:
        {
            unsigned long new_line;

            if (cmd_line == NULL)
            {
                printf("Line to change to [1-2]: ");
                
                fgets(buffer, sizeof(buffer) - 1, stdin);
                buffer[sizeof(buffer) - 1] = '\0';
                new_line = strtoul(buffer, NULL, 0);
            }
            else
            {
                new_line = strtoul(cmd_line, NULL, 0);
            }

            if ((new_line != 1) && (new_line != 2)) {
                printf("Invalid line.\n");
                break;
            }

            active_line = new_line;
            print_menu();
#if 0

            //TODO: Change line_idle_check to initialized check
            // or change to a > check on num lines
            if (sccp_line_idle_check(client, new_line))
            {
                sccp_event_data event_data;
                active_line = new_line;
                printf("\nActive line is now: %d\n\n", active_line);
                event_data.body.activate_line = active_line;
                sccp_sm_event(client, SCCP_APP_EVENT_SELECT_LINE, &event_data);
            }
            else
            {
                printf("\nIllegal line number (%d) selected\n\n", new_line);
            }
#endif
            break;
        }
#ifndef SCCP_SAPP
    case TEST_COMMAND_REGISTER:
        {
            long err = 0;

            err = sccp_client_register(client);
            if (err != 0)
            {
                printf("Unable to register with CallManager.  Please try again.\n");
            }
            break;
        }
    case TEST_COMMAND_UNREGISTER:
        {
            sccp_client_unregister(client);
            break;
        }
#endif /* SCCP_SAPP */
#ifdef SCCP_SAPP
    case 'k': {
        if (1) {
            sccp_event_data data;

            printf("ACTION: Answering call\n");

            data.line_number    = active_line;
            data.call_reference = active_call;
            data.type = SCCP_FEATURE_ANSWER;

            sccp_sm_event(client, SCCP_APP_EVENT_FEATURE, &data);
        }
        else {
            printf("ACTION: Going offhook\n");
        }

        break;
    }

    case 'h': {
        printf("ACTION: Going onhook\n");

        sapp_release(sapp_get_conn_id(), sapp_get_line(), GAPI_CAUSE_NORMAL);

        break;
    }
    
    case 'z': {
        int flags = -1;

        if (cmd_line != NULL) {
            flags = atoi(cmd_line);
            printf("flags= %d\n", flags);
            if (flags == 9) {
                flags = -1;
            }
            printf("flags= %d\n", flags);
            sapp_set_flags(flags);
        }

        break;
    }

    case 'o': {
        gapi_cmaddr_t cms[5];
        char lmac[17];
        int i;
        int id = 1;
        int count = 0;
        char command = 's';
        gapi_media_caps_t caps;
        gapi_opensession_values_t values;
        sapp_info_t *sinfo;


        memset(cms, 0, sizeof(*cms) * 5);
        
        for (i = 0; i < 5; i++) {
            cms[i].addr = sccp_test_addrs[i];
            cms[i].port = sccp_test_ports[i];
        }

        caps.count = 1;
        caps.caps[0].payload = GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
        caps.caps[0].milliseconds_per_packet = 20;

//        caps.caps[1].payload = GAPI_PAYLOAD_XV150_MR;
//        caps.caps[1].milliseconds_per_packet = 20;

        memset(&values, 0, sizeof(values));
        values.device_poll_to = 180000;//10000
//        values.device_type = GAPI_DEVICE_IPSTE;
        values.device_type = GAPI_DEVICE_STATION_TELECASTER_MGR;
//        values.cc_mode = GAPI_CC_MODE_PASSTHRU;

        if (cmd_line != NULL) {
            command = cmd_line[0];
        }

        switch (command) {
        case ('m'):
            count = atoi(&(cmd_line[2]));

            for (i = 0; i < 17; i++) {
                lmac[i] = '0';
            }

            for (i = 1; i <= count; i++) {
                int start;

                if (i < 10) {
                    start = 0;
                }
                else if (i < 100) {
                    start = 1;
                }
                else {
                    start = 2;
                }
                itoa(i, &(lmac[11 - start]), 10);

                printf("TEST: Opening SCCP session %d: %s...\n", i, lmac);

                sapp_opensession_req(cms, lmac, NULL,
                                     GAPI_SRST_MODE_DISABLE, NULL, &caps, &values,
                                     GAPI_PROTOCOL_VERSION_PARCHE, sapp_get_info(i));

		        sccptest_set_menu_state_all(SCCPTEST_MENU_STATE_OPENING);
#if 1
                if (i % 10 == 0) {
                    sleep(5);
                }
#endif
            }

            break;

        case ('n'):
            count = atoi(&(cmd_line[2]));

            for (i = 0; i < 17; i++) {
                lmac[i] = '0';
            }

            itoa(count, &(lmac[11]), 10);
     
            printf("TEST: Opening SCCP session %d...\n", count);

            sapp_opensession_req(cms, lmac, NULL,
                                 GAPI_SRST_MODE_DISABLE, NULL, &caps, &values,
                                 GAPI_PROTOCOL_VERSION_PARCHE, sapp_get_info(i));

            sccptest_set_menu_state_all(SCCPTEST_MENU_STATE_OPENING);

            break;

        default:
            if (cmd_line != NULL) {
                id = atoi(cmd_line);
            }

            sinfo = sapp_get_info(id);
            if (sinfo == NULL) {
                printf("TEST: no sapp info.\n");
                break;
            }

#if 0 //test
            local_mac[11] = '\0';
#endif
            printf("TEST: Opening SCCP session %s...\n", local_mac);
            sapp_opensession_req(cms, local_mac, NULL,
                                 GAPI_SRST_MODE_DISABLE, NULL, &caps, &values,
                                 GAPI_PROTOCOL_VERSION_PARCHE, sinfo);

		    sccptest_set_menu_state_all(SCCPTEST_MENU_STATE_OPENING);
            
            break;
        }

        break;
    }

    case 'r': {
        gapi_causes_e cause;

        printf("TEST: Resetting SCCP session...\n");
        memset(&sccptest_lines, 0, sizeof(sccptest_lines));
        if ((cmd_line != NULL) && (cmd_line[0] == '1')) {
            cause = GAPI_CAUSE_CM_RESTART;
        }
        else {
            cause = GAPI_CAUSE_CM_RESET;
        }
        sapp_resetsession_req(cause);
        sccptest_set_menu_state(SCCPTEST_MENU_STATE_CLOSED, active_line, active_call);
        break;
    }

    case 's':
        printf("TEST: Showing SCCP session...\n");
        sapp_showsession();
        break;
#endif /* #ifdef SCCP_SAPP */
    default:
        {
            return(1);
            break;
        }
    }

    return(0);
}


void
print_menu (void)
{
    int i, j;
    unsigned char *p;
    gapi_cmaddr_t *cmaddr = sapp_get_cmaddr_by_state2(SAPP_CM_S_REGISTERED);
#if 0
    sccptest_options_t *options;
    int i;
    sccptest_call_t *call;
#endif

    printf("\n------------=={ MENU }==------------\n");

    printf("sccptest state= %d\n", sccptest_state);

    p = (unsigned char *)(&(cmaddr->addr));
    printf("cmaddr= [%d:%d:%d:%d:%d]\n",
           p[0], p[1], p[2], p[3], htons((short)(cmaddr->port)));

    printf("call_state= %s\n", call_state);
    printf("active line/call_id= %d/%d\n", (int)active_line, active_call);
    printf("line  call  state\n");
    for (i = 0; i < SCCPTEST_MAX_LINES; i++) {
        for (j = 0; j < SCCPTEST_MAX_CALLS; j++) {
            printf("%-4d  %-4d  %-10s\n",
                   i, sccptest_lines[i].calls[j].call_id,
                   sccptest_menu_states[sccptest_lines[i].calls[j].state]);
        }
    }
    printf("\n");

#if 0
    call = sccptest_get_call(active_line, active_call);
    if (call == NULL) {
        return;
    }

    if (call_id != active_call) {
        return;
    }

    options = sccptest_option_table[call->state];

    for (i = SCCPTEST_COMMAND_QUIT; i < SCCPTEST_COMMAND_MAX; i++) {
        menu_item_configure(menu, sccptest_commands[i], options->option1);
        menu_item_configure(menu, sccptest_commands[i], options->option2);

        options++;
    }
#endif

    menu_print(menu);
    printf("\n\n");
}

static void sccptest_print_menu_str (char *str)
{
    //print_menu();
    printf("\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
    printf("%s", str);
    printf(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n\n");
}

long sccp_callback(sccp_state *state, SCCP_STACK_EVENT event, sccp_event_data *event_data, void *callback_data)
{
    char cmd_line[MAX_MENU_COMMAND_LENGTH];

    snprintf(cmd_line, sizeof(cmd_line), "%010d", event);
    sccp_menu_state         = state;
    sccp_menu_event_data    = event_data;
    sccp_menu_callback_data = callback_data;
    menu_process_command_line(sccp_menu, cmd_line);

    return(0);
}

int
sccp_menu_func(const char *cmd_line, unsigned long menu_id, long param1, long param2, long param3, long param4)
{
    sccp_state *state           = *(sccp_state **)param2;
    sccp_event_data *event_data = *(sccp_event_data **)param3;

    switch(param1)
    {
    case SCCP_STACK_EVENT_OFFHOOK:
        {
            MENU_TITLE("Offhook");

            print_menu();
            break;
        }
    case SCCP_STACK_EVENT_CALL_SETUP:
        {
            MENU_TITLE("Ringing (Incoming)");
            sccptest_set_menu_state(SCCPTEST_MENU_STATE_INCOMING, active_line, active_call);
//            menu_item_configure(menu, "a", MENU_ITEM_OPTION_ENABLE);

            print_menu();
            break;
        }
    case SCCP_STACK_EVENT_CALL_SETUP_ACK:
    case SCCP_STACK_EVENT_CALL_PROCEEDING:
    case SCCP_STACK_EVENT_CALL_ALERTING:
        {
#ifdef SCCP_SAPP
            sccptest_set_menu_state(SCCPTEST_MENU_STATE_ACTIVE, active_line, active_call);
#else
            menu_item_configure(menu, "a", MENU_ITEM_OPTION_DISABLE);
#endif
            switch(param1)
            {
            case SCCP_STACK_EVENT_CALL_SETUP_ACK:
                MENU_TITLE("Call Setup Ack");break;
            case SCCP_STACK_EVENT_CALL_ALERTING:
                MENU_TITLE("Ringing");break;
            case SCCP_STACK_EVENT_CALL_PROCEEDING:
                MENU_TITLE("Proceeding");break;
            };

            print_menu();
            break;
        }
    case SCCP_STACK_EVENT_CALL_CONNECTED:
        {
#ifdef SCCP_SAPP
            sccptest_set_menu_state(SCCPTEST_MENU_STATE_ACTIVE, active_line, active_call);
#else
            menu_item_configure(menu, "a", MENU_ITEM_OPTION_DISABLE);
#endif

            MENU_TITLE("Connected");
            print_menu();
            break;
        }
    case SCCP_STACK_EVENT_CALL_RELEASE:
        {
            MENU_TITLE("Idle");
#ifdef SCCP_SAPP
            sccp_event_data *data = (sccp_event_data *)event_data;

            if (data != NULL) {
                sccptest_set_menu_state(SCCPTEST_MENU_STATE_OPENED, data->line_number,
                                        data->call_reference);
                sccptest_free_call(data->line_number, data->call_reference);
            }
#else
            menu_item_configure(menu, "a", MENU_ITEM_OPTION_DISABLE);
#endif

#ifdef SCCP_SAPP
            active_call = SCCPTEST_DEFAULT_CALL;
#endif

            print_menu();
            break;
        }
    case SCCP_STACK_EVENT_FEATURE: {
        sccp_event_data *feature = (sccp_event_data *)event_data;
        switch(feature->type) {
        case SCCP_FEATURE_DIGIT: {
                printf("STATUS: Got DTMF Digit: 0x%d\n", feature->body.digit_data);
                break;
        }

        case SCCP_FEATURE_STARTTONE: {
            int tone = feature->body.tone_data;

            print_menu();
            if ((tone == GAPI_TONE_BUSY) || (tone == GAPI_TONE_REORDER)) {
                sccptest_print_menu_str("STATUS: Tone: Call Failed. Hangup.\n");
            }
            else {
                sccptest_print_menu_str("STATUS: Tone.\n");
            }

            break;
        }

        case SCCP_FEATURE_HOLD: {
            sccptest_set_menu_state(SCCPTEST_MENU_STATE_HOLDING,
                                    feature->line_number, feature->call_reference);

            print_menu();

            sccptest_print_menu_str("STATUS: Holding\n");

            break;
        }

        default: {
                printf("STATUS: Got feature in callback. (Unhandled)\n");
                break;
            }
        }

        break;
    }
    case SCCP_STACK_EVENT_MEDIA_RECEIVE_START:
        {
            if (event_data == NULL)
            {
                break;
            }
            sccp_media_info                 *media = &event_data->body.media_info;

            memset(media, 0x0, sizeof(sccp_media_info));
            media->port = 0;
            
            sccp_sm_event(state, SCCP_APP_EVENT_MEDIA_RECEIVE_START_ACK, event_data);
            delete event_data;

            break;
        }
    case SCCP_STACK_EVENT_MEDIA_RECEIVE_STOP:
        {
            sccp_sm_event(state, SCCP_APP_EVENT_MEDIA_RECEIVE_STOP_ACK, NULL);

            break;
        }
    case SCCP_STACK_EVENT_MEDIA_TRANSMIT_START:
        {
            if (event_data == NULL)
            {
                break;
            }
//            sccp_media_info                 *media = &event_data->body.media_info;

            sccp_sm_event(state, SCCP_APP_EVENT_MEDIA_TRANSMIT_START_ACK, NULL);
            delete event_data;

            break;
        }
    case SCCP_STACK_EVENT_MEDIA_TRANSMIT_STOP:
        {
            sccp_sm_event(state, SCCP_APP_EVENT_MEDIA_TRANSMIT_STOP_ACK, NULL);

            break;
        }
    case SCCP_STACK_EVENT_CM_CONNECTED:
        {
#ifdef SCCP_SAPP
            int i, j;
#else
            unsigned long       i, j;
#endif
            const char *name;

            printf("Connected to CM\n");
#ifdef SCCP_SAPP
            sccptest_set_menu_state_all(SCCPTEST_MENU_STATE_OPENED);
            sccptest_state = SCCPTEST_S_OPENED;
#else
            menu_item_configure(menu, "c", MENU_ITEM_OPTION_ENABLE);
            menu_item_configure(menu, "d", MENU_ITEM_OPTION_ENABLE);
#endif
            printf("Number of lines available: %d\n", sccp_num_lines_get(client));
            for (i = 0, j = 1; j < SCCP_MAX_LINES && i < sccp_num_lines_get(client); j++)
            {
                name = sccp_line_name_get(client, j);
                if (name != NULL)
                {
                    i++;
                    printf("  - Line[%d]: %s\n", j, name);
                }
            }

            print_menu();

            break;
        }
    case SCCP_STACK_EVENT_CM_DISCONNECTED:
        {
            //char c;

            printf("Disconnected from CM\n");
#ifdef SCCP_SAPP
            sccptest_set_menu_state_all(SCCPTEST_MENU_STATE_CLOSED);
            if (sccptest_state == SCCPTEST_S_QUITTING) {
                print_menu();

                /* Need to post an event so that the stack can fully
                 * clean up.
                 */
                gapi_cleanup();
                ssapi_cleanup();
                sapp_cleanup();

                shutdown_routine();

                menu_destroy(menu);
                menu_destroy(sccp_menu);
                //_CrtMemDumpAllObjectsSince(NULL);
                //c = getchar();

                sccptest_exit();
                exit(0);
            }
            else {
                sccptest_state = SCCPTEST_S_CLOSED;
            }
#else
            menu_item_configure(menu, "c", MENU_ITEM_OPTION_DISABLE);
            menu_item_configure(menu, "d", MENU_ITEM_OPTION_DISABLE);
#endif

            print_menu();

            break;
        }
    default:
        {
            return(1);
            break;
        }
    }

    return(0);
}


long
init_routine(void)
{
#ifdef _WIN32
    WSADATA             sock_data;
    unsigned short      lVersion;

    // Initialize windows socket system
    printf("\nInitializing Windows socket system\n");
    lVersion = MAKEWORD(2,2);
    WSAStartup(lVersion, &sock_data);
    if (LOBYTE(sock_data.wVersion) != 2 ||
        HIBYTE(sock_data.wVersion) != 2 ) {
        WSACleanup();
        return(1); 
    }
    printf("Windows socket system initialized\n");
#endif
#ifdef BLUETOOTH_ENABLE
    local_audio_channels    = 1;
    cell_device_id          = 0;
    voip_device_id          = 0;
#endif
#ifdef AUDIO_ENABLE
    init_audio_system();
#endif
#ifdef CELL_ENABLE
    init_cell_system();
#endif
#ifdef BLUETOOTH_ENABLE
    cell_device_id          = 1;
    init_bluetooth_system();
#endif
#ifdef HANDOFF_ENABLE
    init_handoff_system();
#endif

    return(0);
}


void
shutdown_routine(void)
{
#ifdef SCCP_SAPP0
    /*
     * Try to shutdown gracefully if the user quits. We do this
     * by sending a close command.
     */
    menu_process_command_line(menu, "e");

    sleep(2);
#endif

#ifdef HANDOFF_ENABLE
    shutdown_handoff_system();
#endif
#ifdef BLUETOOTH_ENABLE
    shutdown_bluetooth_system();
#endif
#ifdef CELL_ENABLE
    shutdown_cell_system();
#endif
#ifdef AUDIO_ENABLE
    shutdown_audio_system();
#endif
#ifdef _WIN32
    WSACleanup();
#endif

#ifdef SCCP_SAPP0
    gapi_cleanup();
    ssapi_cleanup();
    sapp_cleanup();
#endif
}

#ifdef SCCP_SAPP
#ifdef __cplusplus
extern "C" {
#endif
#endif /*SCCP_SAPP */

void sccptest_closesession_res (void)
{
    sccptest_callback(SCCP_STACK_EVENT_CM_DISCONNECTED, NULL, NULL);
}

void sccptest_sessionstatus (gapi_status_e status)
{
    switch (status) {
    case (GAPI_STATUS_CM_REGISTER_COMPLETE):
        sccptest_callback(SCCP_STACK_EVENT_CM_CONNECTED, NULL, NULL);
        break;

    case (GAPI_STATUS_RESET_COMPLETE):
        sccptest_callback(SCCP_STACK_EVENT_CM_DISCONNECTED, NULL, NULL);
        break;

    default:
	break;
    }
}

void sccptest_setup (int conn_id, int line, gapi_conninfo_t *conninfo)
{
    sccptest_call_t *call;

    call = sccptest_get_call(line - 1, 0);
    if (call == NULL) {
        sapp_release_complete(conn_id, line, GAPI_CAUSE_OK);
        return;
    }

    active_call = conn_id;
    active_line = line - 1;

    call->call_id = conn_id;

    sccptest_callback(SCCP_STACK_EVENT_CALL_SETUP, NULL, NULL);
}

void sccptest_offhook (int conn_id, int line, int flag)
{
    sccptest_call_t *call;
    char *cmd_line = "d";

    call = sccptest_get_call(line - 1, conn_id);
    if (call != NULL) {
        printf("TEST: ignoring offhook.\n");
        return;
    }

#if 0
    active_call = conn_id;
    active_line = line - 1;

    //call->call_id = conn_id;
#endif
    sccptest_xfer_flag = conn_id;

//    sccptest_callback(TEST_COMMAND_CALL_BY_DIALSTRING, NULL, NULL);
//    cmd_line[0] = TEST_COMMAND_CALL_BY_DIALSTRING;
//    cmd_line[1] = '\0';

    menu_item_configure(menu, "d", MENU_ITEM_OPTION_ENABLE);
    menu_process_command_line(menu, cmd_line);
}

void sccptest_setup_ack (int conn_id, int line)
{
    sccptest_callback(SCCP_STACK_EVENT_CALL_SETUP_ACK, NULL, NULL);
}

void sccptest_proceeding (int conn_id, int line)
{
    sccptest_callback(SCCP_STACK_EVENT_CALL_PROCEEDING, NULL, NULL);
}

void sccptest_alerting (int conn_id, int line)
{
    sccptest_callback(SCCP_STACK_EVENT_CALL_ALERTING, NULL, NULL);
}

void sccptest_connect (int conn_id, int line)
{
    sccptest_callback(SCCP_STACK_EVENT_CALL_CONNECTED, NULL, NULL);

    sapp_connect_ack(conn_id, line);
}

void sccptest_connect_ack (int conn_id, int line)
{
    sccptest_callback(SCCP_STACK_EVENT_CALL_CONNECTED, NULL, NULL);
}

void sccptest_release (int conn_id, int line)
{
    sccp_event_data data;
    
    data.line_number    = line - 1;
    data.call_reference = conn_id;

    sccptest_callback(SCCP_STACK_EVENT_CALL_RELEASE, &data, NULL);

    sapp_release_complete(conn_id, line, GAPI_CAUSE_OK);
}

void sccptest_release_complete (int conn_id, int line)
{
    sccp_event_data data;
    
    data.line_number    = line - 1;
    data.call_reference = conn_id;

    sccptest_callback(SCCP_STACK_EVENT_CALL_RELEASE, &data, NULL);
}

void sccptest_starttone (int conn_id, int line, int tone)
{
    sccp_event_data data;
    
    data.type = SCCP_FEATURE_STARTTONE;
    data.line_number    = line - 1;
    data.call_reference = conn_id;
    data.body.tone_data = tone;

    sccptest_callback(SCCP_STACK_EVENT_FEATURE, &data, NULL);
}

void sccptest_feature_req (int conn_id, int line, int feature)
{
    sccp_event_data data;
    
    data.line_number    = line - 1;
    data.call_reference = conn_id;
    if (feature == GAPI_FEATURE_HOLD) { 
        data.type           = SCCP_FEATURE_HOLD;
    }
    else {
        return;
    }

    sccptest_callback(SCCP_STACK_EVENT_FEATURE, &data, NULL);
}

#ifdef SCCP_SAPP
#ifdef __cplusplus
}
#endif
#endif /* SCCP_SAPP */
