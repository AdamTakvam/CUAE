{literal}
<script type="text/javascript">
<!--
function disable_buttons(id, dontdisable)
{
    try_disable_button(id, dontdisable, 'enable');
    try_disable_button(id, dontdisable, 'disable');
    try_disable_button(id, dontdisable, 'restart');
    try_disable_button(id, dontdisable, 'stop');
    try_disable_button(id, dontdisable, 'start');
    try_disable_button(id, dontdisable, 'kill');
}

function try_disable_button(id, dontdisable, button)
{
    if (dontdisable != button)
    {
        try
        {
            var name = button + '[' + id + ']';
            document.forms[0].elements[name].disabled = true;
            return;
        }
        catch (e)
        {
            return;
        }
    }
}
// -->
</script>
{/literal}

<form action="{$SCRIPT_NAME}" method="post">
<table>
<tr>
    <th>Service Name</th>
    <th>Description</th>
    <th>Enabled</th>
    <th>Status</th>
    <th>Actions</th>
</tr>
<tr style="background-color:#ccc">
    <td colspan="4">&nbsp;</td>
    <td><input type="submit" name="refresh" value="Refresh Service Status" /></td>
</tr>
{foreach from=$services item=service}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td>{$service.name}</td>
    <td>{$service.description}</td>
    <td>{if $service.enabled}Yes{else}No{/if}</td>
    <td>{$service.status}</td>
    <td>
        {if $service.status == "Unknown"}
            
        {elseif $service.enabled}
            <input type="submit" name="disable[{$service.id}]" value="Disable" onclick="disable_buttons({$service.id},'disable')" />
            {if $service.status == "Running"}
            <input type="submit" name="restart[{$service.id}]" value="Restart" onclick="disable_buttons({$service.id},'restart')" />
            <input type="submit" name="stop[{$service.id}]" value="Stop" onclick="disable_buttons({$service.id},'stop')" />
            {elseif $service.status == "Stopped"}
            <input type="submit" name="start[{$service.id}]" value="Start" onclick="disable_buttons({$service.id},'start')" />
            {/if}
            {if $service.status != "Stopped"}
            <input type="submit" name="kill[{$service.id}]" value="Kill" onclick="disable_buttons({$service.id},'kill')" />
            {/if}
        {else}
            <input type="submit" name="enable[{$service.id}]" value="Enable" onclick="disable_buttons({$service.id},'enable')" />
            {if $service.status == "Running"}
            <input type="submit" name="stop[{$service.id}]" value="Stop" onclick="disable_buttons({$service.id},'stop')" />
            {/if}
        {/if}
    </td>
</tr>
{/foreach}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td>Management Console</td>
    <td>Web server that hosts the management console</td>
    <td>Yes</td>
    <td>Running</td>
    <td>
        <input type="submit" name="restart_console" value="Restart" />        
    </td>
</tr>
<tr style="background-color:#ccc">
    <td colspan="4">&nbsp;</td>
    <td><input type="submit" name="refresh" value="Refresh Service Status" /></td>
</tr>
</table>
</form>