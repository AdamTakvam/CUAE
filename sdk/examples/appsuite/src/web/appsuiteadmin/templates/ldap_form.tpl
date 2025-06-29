<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Hostname</th>
        <td><input type="text" name="hostname" value="{$server.hostname|escape:"html"}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Port</th>
        <td><input type="text" name="port" value="{$server.port}" size="5" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Secure Connection</th>
        <td><input type="checkbox" name="secure_connect" value="1" {if $server.secure_connect}checked="checked"{/if}/></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Search Base DN</th>
        <td><input type="text" name="base_dn" value="{$server.base_dn|escape:"html"}" style="width:40em;" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>User DN</th>
        <td><input type="text" name="user_dn" value="{$server.user_dn|escape:"html"}" style="width:40em;" /></td>
    </tr>
    {if $mode == 'edit'}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Password</th>
        <td><input type="button" onclick="window.open('ldap_password.php?id={$id}','password','width=600,height=400,resizable=yes,scrollbars=yes')"  value="Change Password" /></td>
    </tr>
    {else}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Password</th>
        <td><input type="password" name="password" value="{$server.password}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Verify Password</th>
        <td><input type="password" name="password_verify" value="{$server.password_verify}" /></td>
    </tr>
    {/if}
</table>

<input type="hidden" name="id" value="{$id}" />
<input type="submit" name="submit" value="Submit" />
<input type="submit" name="test" value="Test Settings" />
<input type="submit" name="goback" value="Go Back" />
</form>