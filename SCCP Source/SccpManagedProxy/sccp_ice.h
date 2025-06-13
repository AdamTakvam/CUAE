/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */

/*
  The sccp stack does all its own memory allocation.  This helps sandbox the
  stack from bad pointer use.  All messages from the stack to the user
  application are via messages from allocated heap memory.  All messages
  into the stack are copied into internally generated buffers.  It is the
  responsibility of the user application to clean up any of the memory that
  it uses to send messages to the stack and it is also the responsibility
  of the user application to clean up the memory from messages that the stack
  sends to the user application.

  All interfaces into the sccp stack should be call be value.  Any additional
  functions or entries in the stack should follow the existing memory
  management scheme (sandboxing the memory).


*/
#ifndef _SCCP_ICE_H_
#define _SCCP_ICE_H_


#include <sccp_ice_events.h>


const long SCCP_DEFAULT_SERVER_PORT         = 2000;
const long SCCP_DEFAULT_BUFFER_SIZE         = 4096;

#ifdef SCCP_SAPP
struct sccp_client_state {
    int one;
};
#endif
typedef struct sccp_client_state sccp_state;
typedef long (*sccp_stack_callback)(sccp_state *state, SCCP_STACK_EVENT event, sccp_event_data *event_data, void *user_data);



//**********************************************************************
//
//
//  State management
//
//
//**********************************************************************
sccp_state *
sccp_client_create();

long
sccp_client_init(
				 sccp_state *state,
				 sccp_stack_callback callback,
				 void *user_data);

long
sccp_client_destroy(sccp_state *state);





//**********************************************************************
//
//
//  Network control
//
//
//**********************************************************************
long
sccp_client_register(
					sccp_state *state);

long
sccp_client_unregister(sccp_state *state);


long
sccp_client_cm_add(
				   sccp_state *state,
				   char *server_ip_addr,
				   unsigned short server_port,
				   unsigned long priority);





//**********************************************************************
//
//
//  Public Message control
//
//
//**********************************************************************
void
sccp_sm_event(
			  sccp_state *state,
			  unsigned long event,
			  sccp_event_data *event_data);





//**********************************************************************
//
//
//  Public SCCP Stack Query Tools
//
//
//**********************************************************************
unsigned long
sccp_line_idle_check(
					 sccp_state *state,
					 unsigned long line_number);

unsigned long
sccp_num_lines_get(sccp_state *state);

const char *
sccp_line_name_get(
				   sccp_state *state,
				   unsigned long line_number);

const char *
sccp_last_dialed_number_get(
							sccp_state *state,
							unsigned long line_number);




#endif
