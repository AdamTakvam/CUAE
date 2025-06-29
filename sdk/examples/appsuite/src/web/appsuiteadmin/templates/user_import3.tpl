{if !$finished}
<p>
The following users have conflicting or invalid usernames or account codes.  Please enter a valid username or account code for these users.
</p>

<form action="{$SCRIPT_NAME}" method="post">
<table>
<tr>
    <th>Username</th>
    <th>Surname</th>
    <th>Given Name</th>
    <th>Email</th>
    <th colspan="2">Account Code</th>
</tr>

{foreach from=$users key=index item=user}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td>
    {if $user.need_username}
        <input type="text" name="new_usernames[{$index}]" value="{$user.uid[0]|escape:"html"}" />
    {else}
        {$user.uid[0]|escape:"html"}
    {/if}
    </td>
    <td>{$user.sn[0]|escape:"html"}</td>
    <td>{$user.givenname[0]|escape:"html"}</td>
    <td>{$user.mail[0]|escape:"html"}</td>
    <td>
    {if $user.need_ac}
        <input type="text" name="new_account_codes[{$index}]" value="{$user.account_code[0]|escape:"html"}" />
    {else}
        {$user.account_code[0]|escape:"html"}
    {/if}
    </td>
    <td><input type="submit" name="remove_import[{$index}]" value="Remove" /></td>
</tr>
{/foreach}

</table>

    <input type="hidden" name="s_import" value="{$s_import|htmlspecialchars}" />
    <input type="hidden" name="as_ldap_servers_id" value="{$as_ldap_servers_id}" />
    <input type="hidden" name="import_group" value="{$import_group}" />
    <input type="submit" name="add_data" value="Submit" />
    <input type="submit" name="cancel" value="Cancel" />
</form>

{else}

<form action="{$SCRIPT_NAME}" method="post">
<p>
Import completed. {$count} users were imported.
</p>
<input type="submit" name="cancel" value="Done" />
</form>
{/if}