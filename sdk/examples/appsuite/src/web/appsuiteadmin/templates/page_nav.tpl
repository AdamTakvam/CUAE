    <strong>Page</strong>:
    {if $page_logic->current > 1}
        [ <a href="{$SCRIPT_NAME}?p={$page_logic->previous}{$page_logic->add_query}">Previous</a> ]
    {/if}
    {foreach from=$page_logic->page_numbers item=page_number}
        {if $page_logic->current != $page_number}
            [<a href="{$SCRIPT_NAME}?p={$page_number}{$page_logic->add_query}">{$page_number}</a>]
        {else}
            [{$page_number}]
        {/if}
    {/foreach}
    {if $page_logic->current < $page_logic->last}
        [ <a href="{$SCRIPT_NAME}?p={$page_logic->next}{$page_logic->add_query}">Next</a> ]
    {/if}