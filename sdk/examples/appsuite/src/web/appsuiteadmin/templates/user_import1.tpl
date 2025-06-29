{if $step eq 1}

    <h3>Select an LDAP Server</h3>
 
    <form action="{$SCRIPT_NAME}" method="post"> 
    {if not empty($servers)}
        <select name="as_ldap_servers_id">
        {foreach from=$servers item=server}
            <option value="{$server.as_ldap_servers_id}">{$server.hostname|escape:"html"}</option>
        {/foreach}
        </select>
        <p>
        <input type="hidden" name="step" value="1" />
        <input type="submit" name="submit_step_1" value="Select" /> <input type="submit" name="cancel" value="Cancel" />
        </p>

    {else}
    <p>
    <strong>There are no LDAP servers configured.</strong>  
    Please go to the <a href="system_mgmt.php">System Management page</a> and add an LDAP server.
    </p>
    {/if}
    </form>

{elseif $step eq 2}

    <h3>Search Criteria</h3>
    <p>
    You can import users from the LDAP server based on the search criteria
    you enter below.  You may use an asterisk (*) as a wildcard (i.e. John Sm*).
    </p>
    <p>
    The free-formed filter field allows one to define any filter they wish, either in 
    conjunction with the other attributes or stand-alone.  Use this feature with caution.
    </p>
    
    <form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <tr>
            <th>Username</th>
            <td><input type="text" name="uid_search" value="" style="width:25em;" /></td>
        </tr>
        <tr>
            <th>Surname</th>
            <td><input type="text" name="sn_search" value="" style="width:25em;" /></td>
        </tr>
        <tr>
            <th>Given Name</th>
            <td><input type="text" name="gn_search" value="" style="width:25em;" /></td>
        </tr>
        <tr>
            <th>Free-Formed Filter</th>
            <td><input type="text" name="freeform" value="" style="width:25em;" /></td>
        </tr>
    </table>
    <input type="hidden" name="as_ldap_servers_id" value="{$as_ldap_servers_id}" />
    <input type="hidden" name="step" value="2" />
    <input type="checkbox" name="call_manager_search" value="1" checked="checked" /> Search for CallManager users
    <br />
    <input type="submit" name="submit_step_2" value="Search" /> <input type="submit" name="cancel" value="Cancel" />
    </form>

{/if}