<h3>Edit A Group</h3>

<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col width="75%"/>
    <col />
<tr>
    <th>Group Name</th>
    <th>Actions</th>
</tr>
{foreach from=$groups item=group}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td>{$group.name|escape:"html"}</td>
    <td><a href="group_edit.php?id={$group.as_user_groups_id}" class="button">Edit</a> <a href="group_delete.php?id={$group.as_user_groups_id}" class="button" />Delete</td>
</tr>
{foreachelse}
<tr>
    <td colspan="2">There are currently no user groups</td>
</tr>
{/foreach}
</table>
</form>

{if $admin_access}
<h3>Create A Group</h3>

<form action="{$SCRIPT_NAME}" method="post">
<p>Name of new group: <input type="text" name="group_name" value="" /> <input type="submit" name="create_group" value="Create Group" /></p>
</form>
{/if}