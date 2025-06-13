/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */
#ifndef _CLI_MENU_H_
#define _CLI_MENU_H_

enum MENU_ITEM_TYPE
{
	MENU_ITEM_TYPE_COMMAND,
	MENU_ITEM_TYPE_SUBMENU,
	MENU_ITEM_TYPE_INVALID
};


enum MENU_ITEM_OPTION
{
	MENU_ITEM_OPTION_HIDE,
	MENU_ITEM_OPTION_REVEAL,
	MENU_ITEM_OPTION_ENABLE,
	MENU_ITEM_OPTION_DISABLE,
	MENU_ITEM_OPTION_INVALID
};


#define MAX_MENU_COMMAND_LENGTH		(256)
#define MAX_MENU_DESCRIPTION_LENGTH	(256)
typedef struct menu_state cli_menu_state;


// Function pointer prototypes
typedef int (*menu_callback)(const char *cmd_line, unsigned long menu_id, long param1, long param2, long param3, long param4);


cli_menu_state *
menu_init(void);

int
menu_destroy(cli_menu_state *state);

int
menu_add_command_ex(cli_menu_state *state,
					const char *command,
					const char *description,
					menu_callback callback,
					long priority,
					long param1,
					long param2,
					long param3,
					long param4);

int
menu_add_command(cli_menu_state *state,
				 const char *command,
				 const char *description,
				 menu_callback callback);

int
menu_remove_command(cli_menu_state *state,
					const char *command,
					const char *description,
					menu_callback callback,
					unsigned long menu_id);

int
menu_remove_command_by_id(cli_menu_state *state,
						  unsigned long menu_id);

int
menu_remove_command_by_command(cli_menu_state *state,
							   const char *command,
							   menu_callback callback);

int
menu_disable_command_by_id(cli_menu_state *state,
						   unsigned long menu_id);

int
menu_disable_command_by_command(cli_menu_state *state,
								const char *command,
								menu_callback callback);

int
menu_enable_command_by_id(cli_menu_state *state,
						  unsigned long menu_id);

int
menu_enable_command_by_command(cli_menu_state *state,
							   const char *command,
							   menu_callback callback);

int
menu_item_configure(cli_menu_state *state,
					const char *command,
					MENU_ITEM_OPTION option);

int
menu_dump(cli_menu_state *state);

int
menu_print(cli_menu_state *state);

int
menu_process_command_line(cli_menu_state *state, const char *command_line);


#endif