<h3>Media Engines</h3>

<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col />
    <col />
    <col />
    <col style="width: 15em;" />
    <tr>
        <th>Name</th>
        <th>Address</th>
        <th>Status</th>
        <th>&nbsp;</th>
    </tr>
{foreach from=$mservers item=mserver}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>{$mserver.name|escape:"html"}</td>
        <td>{$mserver.ip_address}</td>
        <td>{$mserver.status}</td>
        <td>
            <input type="submit" name="edit[{$mserver.id}]" value="Edit" />
            <input type="submit" name="remove[{$mserver.id}]" value="Remove" />
            {if not $mserver.enabled}
                <input type="submit" name="enable[{$mserver.id}]" value="Enable" />
            {else}
                <input type="submit" name="disable[{$mserver.id}]" value="Disable" />
            {/if}
        </td>
    </tr>
{foreachelse}
    <tr>
        <td colspan="3">There are no media engines listed.</td>
    </tr>
{/foreach}
</table>
<input type="submit" name="add" value="Add a Media Engine" />
<input type="submit" name="refresh" value="Refresh Media Engine List" />
</form>

<h3>Media Resource Groups</h3>

<form method="post" action="{$SCRIPT_NAME}">
    <select name="group_id">
    {foreach from=$groups item=group}
        <option value="{$group.mce_component_groups_id}">{$group.name|escape:"html"}</option>
    {/foreach}
    </select>
    <input type="submit" name="edit_group" value="Edit Group" />
    <input type="submit" name="create_group" value="Create New Group" />
</form>