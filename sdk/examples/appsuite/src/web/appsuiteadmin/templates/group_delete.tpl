<form action="{$SCRIPT_NAME}" method="post">
{if $user_count > 0}
<p>
There are currently {$user_count} user(s) in the group &quot;{$name|escape:"html"}&quot;.  What would you like to do with these users?
</p>
<ul style="list-style-type:none;">
    <li><input type="radio" name="my_action" value="move" checked="checked" /> Delete the group and move the users to group
    <select name="new_group">
    {foreach from=$other_groups item=group}
        <option value="{$group.as_user_groups_id}">{$group.name|escape:"html"}</option>
    {/foreach}
    </select>
    </li>
    <li><input type="radio" name="my_action" value="delete" /> Delete the group and leave the users alone</li>
</ul>
{else}
<p>Are you sure you want to delete the group &quot;{$name|escape:"html"}&quot;?</p>
{/if}

<input type="submit" name="delete_confirm" value="Delete Group" /> 
<input type="submit" name="delete_cancel" value="Cancel" />
<input type="hidden" name="id" value="{$id}" />
</p>
</form>