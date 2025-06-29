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
            if(!objCheckBoxes[i].disabled)
                objCheckBoxes[i].checked = CheckValue;}
// -->
</script>
{/literal}


<form action="{$SCRIPT_NAME}" method="post">
    <input type="submit" name="create" value="Create User" />
    {if $admin_access}<input type="submit" name="import" value="Import Users" />{/if}
</form>

<hr />


<form action="{$SCRIPT_NAME}" method="post" name="search">
    <p>
    <strong>Search for users</strong> by
        <select name="search[field]">
            <option value="username">Username</option>
            <option value="account_code" {if $search.field == "account_code"}selected="selected"{/if}>Account Code</option>
            <option value="first_name" {if $search.field == "first_name"}selected="selected"{/if}>First Name</option>
            <option value="last_name" {if $search.field == "last_name"}selected="selected"{/if}>Last Name</option>
            <option value="email" {if $search.field == "email"}selected="selected"{/if}>E-mail</option>
        </select>
    for
        <input type="text" name="search[text]" value="{$search.text|escape:"html"}" />
    that are in group
        <select name="search[group]">
            {if $admin_access}<option value="">(Any)</option>{/if}
            {foreach from=$groups item=group}
            <option value="{$group.as_user_groups_id}" {if $search.group == $group.as_user_groups_id}selected="selected"{/if}>{$group.name|escape:"html"}</option>
            {/foreach}
        </select>
    and are
        {html_options options=$statuses selected=$search.status name="search[status]"}
        <select name="search[user_type]">
            <option value="">(Any Type)</option>
            <option value="1" {if $search.user_type == 1}selected="selected"{/if}>Normal Users</option>
            <option value="64" {if $search.user_type == 64}selected="selected"{/if}>Administrators</option>
        </select>
     </p>
     <p>
     <strong>Sort by</strong>
        {html_options options=$sort_types selected=$search.sort name="search[sort]"}
        <select name="search[sort_order]">
            <option value="ASC" {if not $search.sort_order eq "DESC"}selected="selected"{/if}>Ascending</option>
            <option value="DESC" {if $search.sort_order eq "DESC"}selected="selected"{/if}>Descending</option>
        </select>
     </p>
        <input type="hidden" name="search[mode]" value="1" />
        <input type="submit" value="Search" />
</form>

<p>{include file="page_nav.tpl"}</p>

<form action="{$SCRIPT_NAME}" method="post" name="user_manage_form">
<p><strong>*</strong> - group administrator</p>
<table>
    <col />
    <col />
    <col />
    <col />
    <col />
    <col style="width: 3em;" />
    <col />
    <col />
    <col />
    <tr>
        <th>&nbsp;</th>
        <th>Name</th>
        <th>Username</th>
        <th>E-mail</th>
        <th>Account Code</th>
        <th>LDAP</th>
        <th>Group</th>
        <th>Status</th>
        <th>&nbsp;</th>
    </tr>

{capture assign="buttons"}
    <tr style="background-color: #ddd;">
        <td colspan="9">
        <input type="button" onclick="SetAllCheckBoxes('user_manage_form','user_ids[]',true);" value="Select All" />
        <input type="button" onclick="SetAllCheckBoxes('user_manage_form','user_ids[]',false);" value="Select None" />
        <input type="submit" name="delete" value="Delete Users" />
        </td>
    </tr>
{/capture}

    {$buttons}
    {foreach from=$users item=user}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><input type="checkbox" name="user_ids[]" value="{$user.as_users_id}" {if $user.as_users_id == $admin_id}disabled="disabled"{/if} /></td>
        <td>{if $user.group_admin}<strong>*</strong>{else}&nbsp;{/if} {$user.last_name|escape:"html"}, {$user.first_name|escape:"html"}</td>
        <td>{$user.username|escape:"html"}</td>
        <td>{$user.email|escape:"html"}</td>
        <td>{$user.account_code}</td>
        <td style="text-align:center">{if $user.ldap_synched}<img src="./images/checkmark.gif" alt="checkmark" />{else}&nbsp;{/if}</td>
        <td>{$user.name|escape:"html"}</td>
        <td>{$user.display_status}</td>
        <td style="text-align:center"><input type="submit" name="edit_user[{$user.as_users_id}]" value="Edit" /></td>
    </tr>
    {foreachelse}
    <tr>
        <td colspan="9">No users were found</td>
    </tr>
    {/foreach}
    {$buttons}
</table>
</form>

<p>{include file="page_nav.tpl"}</p>
