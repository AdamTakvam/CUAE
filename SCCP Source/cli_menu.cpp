/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */
#include <cli_menu.h>
#include <stdio.h>
#include <string.h>





#define MENU_MAGIC                  (0x05e1ec70)
#define MAX_MENU_ITEMS              (256)
#define MENU_DESCRIPTION_COLUMN     (30)
#define MENU_CHECK_STATE_MAGIC(x)   if (state == NULL || state->magic != MENU_MAGIC) {return(x);}





// Structure definitions
struct menu_item_entry_command
{
    unsigned char           enabled;
    long                    priority;
    char                    description[MAX_MENU_DESCRIPTION_LENGTH];
    long                    param1;
    long                    param2;
    long                    param3;
    long                    param4;
    unsigned long           menu_id;
    menu_callback           callback;
    menu_item_entry_command *next;
};


struct menu_item_entry
{
    char                    command[MAX_MENU_COMMAND_LENGTH];
    menu_item_entry_command *cmd_list;
    MENU_ITEM_TYPE          type;
    menu_item_entry         *next;
    unsigned char           hidden;
    unsigned char           enabled;
};


struct menu_state
{
    unsigned long           magic;
    unsigned long           menu_id;                        // Serial number for menu items
    menu_item_entry         *menu_items;
};





// Private function prototypes
static menu_item_entry *
menu_find_item_by_command(cli_menu_state *state, const char *command);

static menu_item_entry_command *
menu_find_executable_by_item(menu_item_entry *entry);

static int
menu_attach_item(cli_menu_state *state, menu_item_entry *item);

static int
menu_attach_command_to_item(menu_item_entry *item, menu_item_entry_command *command);





cli_menu_state *
menu_init(void)
{
    menu_state      *state;

    state = new menu_state;
    if (state == NULL)
    {
        return(NULL);
    }

    memset(state, 0x0, sizeof(menu_state));
    state->magic    = MENU_MAGIC;
    state->menu_id  = 1;

    return(state);
}


int
menu_destroy(cli_menu_state *state)
{
    menu_item_entry         *iterator;
    menu_item_entry         *iterator_temp;
    menu_item_entry_command *cmd;

    if (state == NULL)
    {
        return(0);
    }

    iterator            = state->menu_items;
    while (iterator != NULL)
    {
        if (iterator->cmd_list != NULL)
        {
            cmd = iterator->cmd_list;
            iterator->cmd_list = cmd->next;
            delete cmd;
        }
        else
        {
            iterator_temp = iterator;
            iterator = iterator->next;
            delete iterator_temp;
        }
    }

    memset(state, 0x0, sizeof(menu_state));
    delete state;

    return(0);
}


static menu_item_entry *
menu_find_item_by_command(cli_menu_state *state,
                          const char *command)
{
    menu_item_entry         *iterator;

    MENU_CHECK_STATE_MAGIC(NULL);

    iterator            = state->menu_items;
    while (iterator != NULL)
    {
        if (strncmp(command, iterator->command, sizeof(iterator->command)) == 0)
        {
            return(iterator);
        }
        iterator        = iterator->next;
    }

    return(NULL);
}


static menu_item_entry_command *
menu_find_executable_by_item(menu_item_entry *entry)
{
    menu_item_entry_command *iterator;

    if (entry == NULL)
    {
        return(NULL);
    }

    iterator = entry->cmd_list;
    // TODO: Put good logic code in here to determine which command
    // we really want to execute
    while (iterator != NULL && iterator->enabled == 0)
    {
        iterator = iterator->next;
    }

    return(iterator);
}


static int
menu_attach_item(cli_menu_state *state,
                 menu_item_entry *item)
{
    menu_item_entry         *iterator;
    menu_item_entry         *last;

    MENU_CHECK_STATE_MAGIC(-1);

    last                    = NULL;
    iterator                = state->menu_items;
    while (iterator != NULL)
    {
        last                = iterator;
        iterator            = iterator->next;
    }

    if (last == NULL)
    {
        state->menu_items   = item;
    }
    else
    {
        last->next          = item;
    }

    item->next              = NULL;

    return(0);
}


static int
menu_attach_command_to_item(menu_item_entry *item,
                            menu_item_entry_command *command)
{
    menu_item_entry_command     *iterator;
    menu_item_entry_command     *last;

    if (item == NULL || command == NULL)
    {
        return(-1);
    }

    last                = NULL;
    iterator            = item->cmd_list;
    command->next       = NULL;
    while (iterator != NULL && iterator->priority > command->priority)
    {
        last            = iterator;
        iterator        = iterator->next;
    }

    if (last == NULL)
    {
        if (item->cmd_list != NULL)
        {
            // We have an existing command list
            // but we are inserting this new command
            // callback at the front of the list.
            // We connect to old list to the new list
            // element
            command->next = item->cmd_list;
        }
        item->cmd_list  = command;
    }
    else
    {
        last->next      = command;
    }

    return(0);
}


int
menu_add_command_ex(cli_menu_state *state,
                 const char *command,
                 const char *description,
                 menu_callback callback,
                 long priority,
                 long param1,
                 long param2,
                 long param3,
                 long param4)
{
    menu_item_entry_command     *cmd;
    menu_item_entry             *entry;
    int                         return_id = -1;

    MENU_CHECK_STATE_MAGIC(-1);

    cmd                         = new menu_item_entry_command;
    if (cmd == NULL)
    {
        return(-1);
    }

    entry = menu_find_item_by_command(state, command);

    if (entry == NULL)
    {
        entry                       = new menu_item_entry;
        if (entry == NULL)
        {
            delete cmd;
            return(-1);
        }

        memset(entry, 0x0, sizeof(menu_item_entry));

        // Copy the command name to the head of command list
        strncpy(entry->command, command, sizeof(entry->command) - 1);
        entry->command[sizeof(entry->command) - 1] = '\0';
        entry->next                 = NULL;
        entry->type                 = MENU_ITEM_TYPE_COMMAND;
        entry->hidden               = 0;
        entry->enabled              = 1;

        menu_attach_item(state, entry);
    }

    memset(cmd, 0x0, sizeof(menu_item_entry_command));
    // Create the actual cmd block
    cmd->next                   = NULL;
    cmd->priority               = priority;
    cmd->callback               = callback;
    cmd->enabled                = 1;
    cmd->param1                 = param1;
    cmd->param2                 = param2;
    cmd->param3                 = param3;
    cmd->param4                 = param4;
    cmd->menu_id                = state->menu_id++;

    strncpy(cmd->description, description, sizeof(cmd->description) - 1);
    cmd->description[sizeof(cmd->description) - 1] = '\0';
    return_id                   = cmd->menu_id;
    
    menu_attach_command_to_item(entry, cmd);

    return(return_id);
}


int
menu_add_command(cli_menu_state *state,
                 const char *command,
                 const char *description,
                 menu_callback callback)
{
    return(menu_add_command_ex(state, command, description, callback, 0, 0, 0, 0, 0));
}


int
menu_remove_command(cli_menu_state *state,
                    const char *command,
                    const char *description,
                    menu_callback callback,
                    unsigned long menu_id)
{
    menu_item_entry             *entry;
    menu_item_entry_command     *cmd;
    menu_item_entry_command     *last_cmd;
    
    MENU_CHECK_STATE_MAGIC(-1);

    if (menu_id != 0)
    {
        
        entry                   = state->menu_items;
        while (entry != NULL)
        {
            last_cmd            = NULL;
            cmd                 = entry->cmd_list;

            while (cmd != NULL)
            {
                if (cmd->menu_id == menu_id)
                {
                    if (last_cmd != NULL)
                    {
                        // We know that there additional commands in
                        // the command chain.  All we need to do is
                        // relink the chain and deallocate memory for
                        // our command
                        last_cmd->next = cmd->next;
                        delete cmd;
                    }
                    else
                    {
                        // Either we are the only command on the chain
                        // or we are the first command on the chain
                        // Either way, we set the head of the chain to
                        // the next entry past us and we deallocate
                        // our command
                        entry->cmd_list = cmd->next;
                        delete cmd;
                    }

                    return(menu_id);
                }

                last_cmd        = cmd;
                cmd             = cmd->next;
            }
            entry               = entry->next;
        }
    }
    else
    {
        entry = menu_find_item_by_command(state, command);
        if (entry == NULL)
        {
            return(-1);
        }
        // We need to search for the menu item through a match in the signature
        //DEBUG: Todo
    }

    return(0);
}


int
menu_disable_command(cli_menu_state *state,
                    const char *command,
                    const char *description,
                    menu_callback callback,
                    unsigned long menu_id)
{
    menu_item_entry             *entry;
    menu_item_entry_command     *cmd;
    
    MENU_CHECK_STATE_MAGIC(-1);

    if (menu_id != 0)
    {
        
        entry                   = state->menu_items;
        while (entry != NULL)
        {
            cmd                 = entry->cmd_list;

            while (cmd != NULL)
            {
                if (cmd->menu_id == menu_id)
                {
                    cmd->enabled = 0;
                    return(menu_id);
                }

                cmd             = cmd->next;
            }
            entry               = entry->next;
        }
    }
    else
    {
        entry = menu_find_item_by_command(state, command);
        if (entry == NULL)
        {
            return(-1);
        }
        // We need to search for the menu item through a match in the signature
        //DEBUG: Todo
    }

    return(0);
}


int
menu_enable_command(cli_menu_state *state,
                    const char *command,
                    const char *description,
                    menu_callback callback,
                    unsigned long menu_id)
{
    menu_item_entry             *entry;
    menu_item_entry_command     *cmd;
    
    MENU_CHECK_STATE_MAGIC(-1);

    if (menu_id != 0)
    {
        
        entry                   = state->menu_items;
        while (entry != NULL)
        {
            cmd                 = entry->cmd_list;

            while (cmd != NULL)
            {
                if (cmd->menu_id == menu_id)
                {
                    cmd->enabled = 1;
                    return(menu_id);
                }

                cmd             = cmd->next;
            }
            entry               = entry->next;
        }
    }
    else
    {
        entry = menu_find_item_by_command(state, command);
        if (entry == NULL)
        {
            return(-1);
        }
        // We need to search for the menu item through a match in the signature
        //DEBUG: Todo
    }

    return(0);
}


int
menu_item_configure(cli_menu_state *state,
                    const char *command,
                    MENU_ITEM_OPTION option)
{
    menu_item_entry             *entry;

    MENU_CHECK_STATE_MAGIC(-1);

    entry = menu_find_item_by_command(state, command);
    if (entry == NULL)
    {
        return(-1);
    }

    switch(option)
    {
    case MENU_ITEM_OPTION_HIDE:
        {
            entry->hidden = 1;
            break;
        }
    case MENU_ITEM_OPTION_REVEAL:
        {
            entry->hidden = 0;
            break;
        }
    case MENU_ITEM_OPTION_ENABLE:
        {
            entry->enabled = 1;
            break;
        }
    case MENU_ITEM_OPTION_DISABLE:
        {
            entry->enabled = 0;
            break;
        }
    case MENU_ITEM_OPTION_INVALID:
    default:
        {
            return(-1);
            break;
        }
    }

    return(0);
}


int
menu_hide_item(cli_menu_state *state,
               const char *command)
{
    menu_item_entry             *entry;

    MENU_CHECK_STATE_MAGIC(-1);

    entry = menu_find_item_by_command(state, command);
    if (entry == NULL)
    {
        return(-1);
    }


    return(0);
}


int
menu_reveal_item(cli_menu_state *state,
                 const char *command)
{
    menu_item_entry             *entry;

    MENU_CHECK_STATE_MAGIC(-1);

    entry = menu_find_item_by_command(state, command);
    if (entry == NULL)
    {
        return(-1);
    }

    entry->hidden = 0;

    return(0);
}


int
menu_remove_command_by_id(cli_menu_state *state,
                          unsigned long menu_id)
{
    return(menu_remove_command(state, NULL, NULL, NULL, menu_id));
}


int
menu_remove_command_by_command(cli_menu_state *state,
                               const char *command,
                               menu_callback callback)
{
    return(menu_remove_command(state, command, NULL, callback, 0));
}


int
menu_disable_command_by_id(cli_menu_state *state,
                          unsigned long menu_id)
{
    return(menu_disable_command(state, NULL, NULL, NULL, menu_id));
}

int
menu_disable_command_by_command(cli_menu_state *state,
                               const char *command,
                               menu_callback callback)
{
    return(menu_disable_command(state, command, NULL, callback, 0));
}

int
menu_enable_command_by_id(cli_menu_state *state,
                          unsigned long menu_id)
{
    return(menu_enable_command(state, NULL, NULL, NULL, menu_id));
}

int
menu_enable_command_by_command(cli_menu_state *state,
                               const char *command,
                               menu_callback callback)
{
    return(menu_enable_command(state, command, NULL, callback, 0));
}




int
menu_dump(cli_menu_state *state)
{
    menu_item_entry             *entry;
    menu_item_entry_command     *cmd;
    long                        i;

    MENU_CHECK_STATE_MAGIC(-1);

    entry = state->menu_items;
    while (entry != NULL)
    {
        printf("Menu Item           : %s\n", entry->command);

        cmd = entry->cmd_list;
        i   = 0;
        while (cmd != NULL)
        {
            if (cmd != entry->cmd_list)
            {
                printf("    ----------\n");
            }
            printf("    Entry           : %d\n", i);
            printf("    ID              : %d\n", cmd->menu_id);
            printf("    Description	    : %s\n", cmd->description);
            printf("    Enabled         : %s\n", (cmd->enabled?"Yes":"No"));
            printf("    Callback address: 0x%x\n", cmd->callback);
            printf("    Priority        : %d\n", cmd->priority);
            cmd = cmd->next;
            i++;
        }
        entry = entry->next;
    }

    return(0);
}


int
menu_print(cli_menu_state *state)
{
    menu_item_entry         *entry;
    long                    len;

    MENU_CHECK_STATE_MAGIC(-1);

    entry = state->menu_items;
    while (entry != NULL)
    {
        if (entry->hidden == 0 && entry->enabled == 1)
        {
            len = (strlen(entry->command) - MENU_DESCRIPTION_COLUMN) < 0?0:1;
            
            printf("%-*s%-*s%s\n", MENU_DESCRIPTION_COLUMN, entry->command, len, "", entry->cmd_list->description);
        }
        entry = entry->next;
    }

    return(0);
}


int
menu_process_command_line(cli_menu_state *state, const char *command_line)
{
    menu_item_entry             *menu_item;
    menu_item_entry_command     *menu_cmd;
    menu_callback               callback;
    const char                  *command;
    const char                  *params;
    unsigned long               i;
    unsigned long               return_val = 1;

    MENU_CHECK_STATE_MAGIC(-1);

    if (command_line == NULL)
    {
        return(-1);
    }

    for (i = 0; i < sizeof(menu_item->command); i++)
    {
        if (command_line[i] == '\0' || command_line[i] == ' ')
        {
            break;
        }
    }
    if (command_line[i] == ' ')
    {
        command = new char[i + 1];
        if (command == NULL)
        {
            return(-1);
        }

        memset((void *)command, 0x0, i + 1);
        memcpy((void *)command, command_line, i);
        params = &command_line[i + 1];
    }
    else
    {
        command = command_line;
        params = NULL;
    }
    menu_item   = menu_find_item_by_command(state, command);
    if (menu_item == NULL || menu_item->enabled == 0)
    {
        return(-1);
    }
    menu_cmd    = menu_find_executable_by_item(menu_item);

    while (return_val != 0 && menu_cmd != NULL)
    {
        callback = menu_cmd->callback;
        
        if (callback != NULL && menu_cmd->enabled == 1)
        {
            return_val = callback(params, menu_cmd->menu_id,
                menu_cmd->param1,
                menu_cmd->param2,
                menu_cmd->param3,
                menu_cmd->param4);
            
        }

        if (return_val != 0)
        {
            menu_cmd = menu_cmd->next;
        }
    }

    if (params != NULL)
    {
        delete [] (char *)(void *)command;
    }

    return(return_val);
}



#ifdef TEST_CLI
int
menu_func(const char *cmd_line, unsigned long menu_id, long param1, long param2, long param3, long param4);

int
main(long argc, char **argv)
{
    void *menu;
    unsigned long id;

    menu = menu_init();
    menu_add_command(menu, "Item1", "Look1", menu_func);
    menu_add_command(menu, "Item2", "Look2", menu_func);
    id = menu_add_command(menu, "Item3", "Look3", menu_func);
    menu_add_command(menu, "Item2", "Look7", menu_func);
    menu_add_command(menu, "Item3", "Look8", menu_func);
    menu_add_command(menu, "Item4", "Look4", menu_func);
    menu_add_command(menu, "Item5", "Look5", menu_func);
    menu_add_command(menu, "Item6", "Look6", menu_func);

    menu_dump(menu);
    printf("\n\n");
    menu_print(menu);
    printf("\n\nRemoving a command\n\n");

    menu_remove_command_by_id(menu, id);
    menu_print(menu);

    menu_process_command_line(menu, "Item3 1 2 3 4 5 6 z x c v");
    return(0);
}


int
menu_func(const char *cmd_line, unsigned long menu_id, long param1, long param2, long param3, long param4)
{
    printf("Got menu func: Menu ID: %d, Cmd Line: %s\n", menu_id, cmd_line);

    return(0);
}
#endif