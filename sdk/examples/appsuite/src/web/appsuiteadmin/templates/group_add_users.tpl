{if !$done}

{literal}
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
			objCheckBoxes[i].checked = CheckValue;
}
// -->
</script>
{/literal}

<form action="{$SCRIPT_NAME}" method="post" name="search">
    Search for users by
        <select name="search[field]">
            <option value="username">Username</option>
            <option value="account_code" {if $search.field == "account_code"}selected="selected"{/if}>Account Code</option>
            <option value="first_name" {if $search.field == "first_name"}selected="selected"{/if}>First Name</option>
            <option value="last_name" {if $search.field == "last_name"}selected="selected"{/if}>Last Name</option>
            <option value="email" {if $search.field == "email"}selected="selected"{/if}>E-mail</option>
        </select>
    for
        <input type="text" name="search[text]" value="{$search.text}" />

    <input type="hidden" name="search[mode]" value="1" />
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" value="Search" />
</form>

{if $users}
<form action="{$SCRIPT_NAME}" method="post" name="user_list">
    <input type="hidden" name="id" value="{$id}" />
    <table>
        <tr>
            <th></th>
            <th>Username</th>
            <th>Name</th>
        </tr>
        <tr style="background-color: #ddd;">
            <td colspan="3">
            <input type="button" onclick="SetAllCheckBoxes('user_list','user_ids[]',true);" value="Select All" />
            <input type="button" onclick="SetAllCheckBoxes('user_list','user_ids[]',false);" value="Select None" />
            </td>
        </tr>
        {foreach from=$users item=user}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td><input type="checkbox" name="user_ids[]" value="{$user.as_users_id}" /></td>
            <td>{$user.username|escape:"html"}</td>
            <td>{$user.last_name|escape:"html"}, {$user.first_name|escape:"html"}</td>
        </tr>
    {/foreach}
    </table>
    <input type="submit" name="add" value="Add Users" />
</form>
{else}
    <input type="button" onclick="window.opener.location = 'group_edit.php?id={$id}'; window.close();" value="Close" />
{/if}

{else}
<p style="text-align:center">
    <input type="button" onclick="window.opener.location = 'group_edit.php?id={$id}'; window.close();" value="Close" />
</p>
{/if}