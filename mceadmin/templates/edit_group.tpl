<script language="javascript" src="./js/selectbox.js"></script>

<h3>Group Properties</h3>

<form action="{$SCRIPT_NAME}" method="post">
    <col class="inputLabels" />
    <col class="inputFields" />
    <table>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Name</th>
            <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Type</th>
            <td>
                <input type="hidden" name="type" value="{$type}" />
                {$type_display}
            </td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Description</th>
            <td><textarea name="description" rows="3" cols="50">{$description|escape:"html"}</textarea></td>
        </tr>
        {if not $is_call_route_group}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Failover Group</th>
            <td>
            <select name="failover_group_id">
                <option value="0">None</option>
            {foreach from=$failovers item=failover}
                <option value="{$failover.mce_component_groups_id}" {if $failover_group_id == $failover.mce_component_groups_id}selected="selected"{/if} >{$failover.name|escape:"html"}</option>
            {/foreach}
            </select>
            </td>
        </tr>
        {/if}
    </table>
    <input type="hidden" name="alarm_group_id" value="{if $alarm_group_id > 0}{$alarm_group_id}{else}{$default_alarm_group}{/if}" />
    <input type="hidden" name="id" value="{$id}" />

{if $id > 0}
    <input type="submit" name="update" value="Apply" />
    <input type="submit" name="delete" value="Delete Group" {if $default}disabled="disabled"{/if}>
    <input type="submit" name="done" value="Done" />
{else}
    <input type="submit" name="create" value="Create Group" />
    <input type="submit" name="done" value="Cancel" />
{/if}
</form>


{if $id > 0}


    {if $members}
    <h3>Members</h3>
    
    <form action="{$SCRIPT_NAME}" method="post">
        <p>
            <select name="member_ids[]" size="5" multiple="multiple">
            {foreach from=$members item=member}
                <option value="{$member.mce_components_id}">{if $member.display_name}{$member.display_name|escape:"html"}{else}{$member.name|escape:"html"}{/if}</option>
            {/foreach}
            </select>
        </p>
        <p>
        {if $is_call_route_group}    
            <input type="button" name="move_up" onclick="javascript:moveOptionUp(this.form['member_ids[]']);" value="Move Up" />
            <input type="button" name="move_down" onclick="javascript:moveOptionDown(this.form['member_ids[]']);" value="Move Down" />
            <input type="submit" name="reorder" onclick="javascript:selectAllOptions(this.form['member_ids[]']); return true;" value="Commit New Order" />
        {/if}
            <input type="submit" name="remove" value="Remove" />
            <input type="hidden" name="id" value="{$id}" />
            <input type="hidden" name="type" value="{$type}" />
        </p>
    </form>
    {/if}

    
    {if $nonmembers}
    <h3>Add Member</h3>
    
    <form method="post" action="{$SCRIPT_NAME}">
        <select name="add_id">
            <option value="0" selected="selected"></option>
        {foreach from=$nonmembers item=nonmember}
            <option value="{$nonmember.mce_components_id}">{$nonmember.name|escape:"html"}</option>
        {/foreach}
        </select>
        <br />
        <input type="hidden" name="id" value="{$id}" />
        <input type="hidden" name="type" value="{$type}" />
        <input type="submit" class="submit" name="add" value="Add Member" />
    </form>
    {/if}

    
{/if}