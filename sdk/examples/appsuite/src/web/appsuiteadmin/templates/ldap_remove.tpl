<form action="{$SCRIPT_NAME}" method="post">
<p>
    If you remove the LDAP server at <em>{$hostname|escape:"html"}</em>, then the {$user_count} user(s) imported from this server can no longer use it for synchronization and external authentication.
    You may perform an action to resolve this.  <span style="color:red;font-weight:bold;">Please note that this action is irreversible.</span> 
</p>

<p>
You have the following options:
</p>

<ul>
    <li style="padding-top:1em;">You may go the <a href="account_mgmt.php">account management page</a> and manually delete these user accounts.</li>

    <li style="padding-top:1em;">
    You may delete all of the user accounts linked to this LDAP server as well as the server itself.
    <br />
    <input type="submit" name="delete_all" value="Delete Users and Server" />
    </li>
    
    {if $servers}
    <li style="padding-top:1em;">
    You may pick another configured LDAP server to associate with the user accounts.
    <br />
    <select name="new_ldap_assoc_id">
        {foreach from=$servers item=server}
        <option value="{$server.as_ldap_servers_id}">{$server.hostname|escape:"html"}</option>
        {/foreach}
    </select>
    <input type="submit" name="change" value="Move Users and Delete {$hostname|escape:"html"}" />
    </li>
    {/if}
</ul>

<input type="hidden" name="id" value="{$id}" />
<input type="submit" name="cancel" value="Cancel" />
</form>