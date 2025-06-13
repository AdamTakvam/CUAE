<h3>Telephony Servers</h3>

<table>
    <tr>
        <th>Name</th>
        <th>Type</th>
    </tr>
{foreach from=$components item=telephony}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><a href="edit_ipt.php?id={$telephony.mce_components_id}&amp;type={$telephony.type}">{$telephony.name|escape:"html"}</a></td>
        <td>{$telephony.display_type}</td>
    </tr>
{/foreach}
{foreach from=$call_managers item=telephony}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><a href="edit_call_manager.php?id={$telephony.mce_call_manager_clusters_id}">{$telephony.name|escape:"html"}</a></td>
        <td>Unified Communications Manager {$telephony.version} Cluster</td>
    </tr>
{/foreach}
{foreach from=$sip_domains item=telephony}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><a href="sip_domain_edit.php?id={$telephony.mce_sip_domains_id}">{$telephony.domain_name|escape:"html"}</a></td>
        <td>{$telephony.display_type}</td>
    </tr>
{/foreach}

</table>

<h3>Add a Telephony Server</h3>

<form action="{$SCRIPT_NAME}" method="post">
<select name="type">
{foreach from=$telephony_server_types key=type_id item=type_name}
    <option value="{$type_id}">{$type_name}</option>
{/foreach}
</select>
<input type="submit" name="add_server" class="submit" value="Add Server" />
</form>

<h3>Call Route Groups</h3>
<form method="post" action="{$SCRIPT_NAME}">
    <select name="group_id">
{foreach from=$groups item=group}
        <option value="{$group.mce_component_groups_id}">{$group.name|escape:"html"}</option>
{/foreach}
    </select>
    <input type="submit" name="edit_group" value="Edit Group" />
</form>

<form method="post" action="{$SCRIPT_NAME}">
    <select name="group_type">
{foreach from=$cr_group_types key=type item=typename}
        <option value="{$type}">{$typename}</option>
{/foreach}  
    </select>
    <input type="submit" name="create_group" value="Create New Group" />
</form>