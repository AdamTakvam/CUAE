{include file="system_mgmt_nav.tpl" selected="ldap"}

<p>
LDAP servers can be configured to import users and for user authentication.
</p>

<table>
<tr>
    <th>Hostname</th>
    <th>Port</th>
    <th>Secure</th>
    <th>Search Base DN</th>
    <th>User DN</th>
    <th>Use Password</th>
    <th></th>
</tr>
{foreach from=$ldap_servers item=server}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td>{$server.hostname|escape:"html"}</td>
    <td>{$server.port}</td>
    <td>{if $server.secure_connect}Yes{else}No{/if}</td>
    <td>{$server.base_dn|escape:"html"}</td>
    <td>{$server.user_dn|escape:"html"}</td>
    <td>{if $server.password}Yes{else}No{/if}</td>
    <td>
        <a href="{$SCRIPT_NAME}?test_ldap={$server.as_ldap_servers_id}" class="button">Test</a>
        <a href="ldap_edit.php?id={$server.as_ldap_servers_id}" class="button">Edit</a> 
        <a href="ldap_remove.php?id={$server.as_ldap_servers_id}" class="button">Remove</a>
    </td>
</tr>
{foreachelse}
<tr>
    <td colspan="7">There are no LDAP servers currently set up.</td>
</tr>
{/foreach}
</table>
<form action="ldap_add.php" method="post">
    <input type="submit" name="add_ldap_server" value="Add LDAP Server" />
</form>

<p>
<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" style="width: 200px" />
        <col class="inputDescription" />
        <tr class="{cycle values='rowOne,rowTwo'}">
			<th>Attribute Name</th>
            <th>Attribute Description</th>
        </tr>     
        <tr>
			<td><input type="text" name="ldap_username_attribute" value="{$ldap_username_attribute|escape:"html"}" /></td>
            <td>Attribute to use for the username when importing users from LDAP servers</td>
        </tr>
        <tr>
			<td><input type="text" name="ldap_account_code_attribute" value="{$ldap_account_code_attribute|escape:"html"}" /></td>
            <td>Attribute to use for the account code when importing users from LDAP servers</td>
        </tr>
    </table>
    <input type="submit" name="apply" value="Apply" />
</form>
</p>