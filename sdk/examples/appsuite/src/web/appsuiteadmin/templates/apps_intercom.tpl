<p>
On this page, you can manage your intercom group memberships.
</p>

<h3>Intercom Groups Memberships</h3>

<form action="{$SCRIPT_NAME}" method="post">
<table>
	<tr>
		<th>Name</th>
		<th>Type</th>
		<th>&nbsp;</th>
	</tr>
	{foreach from=$groups item=group}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td>{$group.name|escape:"html"}</td>
		<td>{if $group.is_private}Private{else}Public{/if}</td>
		<td><input type="submit" name="leave[{$group.as_intercom_groups_id}]" value="Leave Group" /></td>
	</tr>
	{/foreach}
</table>
</form>

<h3>Join A Group</h3>

<form action="{$SCRIPT_NAME}" method="post">
	<select name="join_group_id">
		<option value="0"></option>
	{foreach from=$publics item=public}
		<option value="{$public.as_intercom_groups_id}">{$public.name|escape:"html"}</option>
	{/foreach}	</select>
	<input type="submit" name="join" value="Join Group" />
</form>