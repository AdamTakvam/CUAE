{literal}
<script type="text/javascript" src="js/user_autocomplete.js"></script>
<script type="text/javascript">
<!--
function SetAllCheckBoxes(FormName, FieldName, CheckValue)
{
	if(!document.forms[FormName])
		return;
	var objCheckBoxes = document.forms[FormName].elements[FieldName];
	if(!objCheckBoxes)
		return;
	var countCheckBoxes = objCheckBoxes.length;
	if(!countCheckBoxes)
		objCheckBoxes.checked = CheckValue;
	else
		// set the check value for all check boxes
		for(var i = 0; i < countCheckBoxes; i++)
            if(!objCheckBoxes[i].disabled)
                objCheckBoxes[i].checked = CheckValue;
}
// -->
</script>
{/literal}

<form action="{$SCRIPT_NAME}" method="post" name="useraddform">
<input type="hidden" name="id" value="{$id}" />
Add user by username: 
<input type="text" name="username" style="width:15em;" onkeyup="acCallback.field = this; acCallback.formname = document.useraddform; getUser(this,event);" autocomplete="off" value="" /> 
<ul id="fancyDropdown" style="left: 15.5em" ></ul>
<input type="submit" name="add_user" value="Add User" />
<input type="button"  onclick="window.open('group_add_users.php?id={$id}','add_users','width=600,height=600,resizable=yes,scrollbars=yes');" value="Add Several Users" />
</form>

<p>{include file="page_nav.tpl"}</p>

<form action="{$SCRIPT_NAME}" method="post" name="user_manage_form">
<input type="hidden" name="id" value="{$id}" />
<table>
<col style="width: 32px;" />
<col />
<col style="width: 15%" />
<tr>
    <th>Select</th>
    <th>User</th>
    <th>Level</th>
</tr>
{capture assign="buttons"}
<tr>
    <td colspan="3">
        <input type="button" onclick="SetAllCheckBoxes('user_manage_form','user_ids[]',true);" value="Select All" />
        <input type="button" onclick="SetAllCheckBoxes('user_manage_form','user_ids[]',false);" value="Select None" />
        <input type="submit" name="make_admin" value="Make Administrator" />
        <input type="submit" name="make_user" value="Make Normal User" />
        <input type="submit" name="remove_users" value="Remove Users" />
    </td>
</tr>
{/capture}

{$buttons}
{foreach from=$users item=user}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td><input type="checkbox" name="user_ids[]" value="{$user.as_users_id}" {if $user.as_users_id eq $admin_id}disabled="disabled"{/if} /></td>
    <td>{$user.username|escape:"html"}</td>
    <td>{$user.level_display}</td>
</tr>
{foreachelse}
<tr>
    <td colspan="3">There are no users in this group.</td>
</tr>
{/foreach}
{$buttons}

</table>
</form>

<p>{include file="page_nav.tpl"}</p>

