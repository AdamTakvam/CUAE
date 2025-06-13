<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Name</th>
        <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Version</th>
        <td>
            <select name="version">
                {foreach from=$cm_versions item=cm_version}
                <option value="{$cm_version}" {if $cm_version == $version}selected="selected"{/if}>{$cm_version}</option>
                {/foreach}
            </select>
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Publisher Address</th>
        <td><input type="text" name="publisher_ip_address" value="{$publisher_ip_address}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Publisher Admin Username</th>
        <td><input type="text" name="publisher_username" value="{if $publisher_username}{$publisher_username|escape:"html"}{else}Administrator{/if}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Publisher Admin Password</th>
        <td><input type="password" name="publisher_password" value="" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Retype Publisher Admin Password</th>
        <td><input type="password" name="publisher_password_verify" value="" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>SNMP Community</th>
        <td><input type="text" name="snmp_community" value="{$snmp_community|escape:"html"}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Description</th>
        <td><textarea name="description">{$description|escape:"html"}</textarea></td>
    </tr>
</table>
<input type="submit" name="submit" class="submit" value="Create Unified Communications Manager Cluster" />
</form>