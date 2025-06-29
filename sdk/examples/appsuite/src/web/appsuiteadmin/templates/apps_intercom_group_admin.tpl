<script type="text/javascript" src="/appsuiteadmin/js/user_autocomplete.js"></script>


<h3>Configuration</h3>

<form action="{$SCRIPT_NAME}" method="post">
<table>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Name</th>
		<td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Enabled</th>
		<td><input type="checkbox" name="is_enabled" {if $is_enabled}checked="checked"{/if} /></td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Talkback Enabled</th>
		<td><input type="checkbox" name="is_talkback_enabled" {if $is_talkback_enabled}checked="checked"{/if} /></td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Private Group</th>
		<td><input type="checkbox" name="is_private" {if $is_private}checked="checked"{/if} /></td>
	</tr>
</table>
{if $id > 0}
	<input type="hidden" name="id" value="{$id}" />
	<input type="submit" name="submit" value="Apply Configuration" />
	<input type="submit" name="delete" value="Delete Group" />
	<input type="submit" name="cancel" value="Done" />
{else}
	<input type="submit" name="submit" value="Create Group" />
	<input type="submit" name="cancel" value="Cancel" />
{/if}
</form>

{if $id > 0}
<h3>Members</h3>

<form action="{$SCRIPT_NAME}" method="post">
<table>
	<tr>
		<th>Username</th>
		<th>Name</th>
		<th>&nbsp;</th>
	</tr>
	{foreach from=$members item=member}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td><a href="/appsuiteadmin/user.php?id={$member.as_users_id}">{$member.username|escape:"html"}</a></td>
		<td>{$member.last_name|escape:"html"}, {$member.first_name|escape:"html"}</td>
		<td><input type="submit" name="delete_member[{$member.as_users_id}]" value="Remove From Group" /></td>
	</tr>
	{/foreach}
</table>
<input type="hidden" name="id" value="{$id}" />
</form>

<form action="{$SCRIPT_NAME}" method="post" name="addUserForm">
<input type="hidden" name="id" value="{$id}" />
<strong>Add User by Username</strong>: 
    <input type="text" name="add_user" style="width:15em;" onkeyup="acCallback.field = this; acCallback.formname = document.addUserForm; getUser(this,event,true);" autocomplete="off" value="" /> 
    <ul id="fancyDropdown" style="left: 16.75em" ></ul>
    <input type="submit" name="add_user_submit" value="Add" />
</form>
{/if}