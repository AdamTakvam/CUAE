<h3>Change Password</h3>

<p>
Specify the password used whenever audio files are deployed to the local media engine resident on this appliance. 
This password must be supplied whenever any application server is configured to use this media engine. 
</p>

<form action="{$SCRIPT_NAME}" method="post">
<table>
<col class="inputLabels" />
<col class="inputFields" />
<col class="inputDescriptions" />
<tr class="{cycle values='rowOne,rowTwo'}">
    <th>New Password</th>
    <td><input type="password" name="new_password" value="" /></td>
    <td>Must be at least 7 characters long</td>
</tr>
<tr class="{cycle values='rowOne,rowTwo'}">
    <th>Verify Password</th>
    <td><input type="password" name="verify_new_password" value="" /></td>
    <td>Please retype the new password</td>
</tr>
</table>
<input type="submit" name="submit" value="Submit" />
</form>

<h3>Media Firmware Addresses</h3>

{if $default_ip && $default_mac}
    <p>
    Specify the default IP and MAC address to which the media firmware will bind.  Please note that changes will take effect after the media engine has been restarted.
    </p>

    <form action="{$SCRIPT_NAME}" method="post">
    <table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Default IP Address</th>
        <td><input type="text" name="default_ip" value="{$default_ip}" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Default MAC Address</th>
        <td><input type="text" name="default_mac" value="{$default_mac}" /></td>
    </tr>
    </table>
    <input type="submit" name="address_submit" value="Submit" />
    </form>
{else}
    <p>There are currently no set values for the media firmware because the firmware has not been activated by the media engine.  To enable these settings, try running the media engine.</p>
{/if}