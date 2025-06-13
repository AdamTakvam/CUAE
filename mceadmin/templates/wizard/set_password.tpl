<p>
Because this is the first time you have accessed the management console, you will need to initialize some passwords for the environment.
Please select strong passwords that are not easy to guess and contain a mix of lowercase and uppercase letters, numbers, and/or special characters.
</p>

<form method="post">

<h3>Media Engine Password</h3>

<p>
The media engine password must be set to allow the media engine to accept securely sent media.  This password will be used when adding the media engine resident on this appliance to any environment configuration.
</p>

<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr>
        <th>Enter Password</th>
        <td><input type="password" name="media_password" /></td>
    </tr>
    <tr>
        <th>Verify Password</th>
        <td><input type="password" name="media_password_verify" /></td>
    </tr>
</table>

<h3>Administrator Password</h3>

<p>
You will need to set the password for the main administrator account.  This password will be used when logging in to the management console as the administrator.
</p>

<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr>
        <th>Enter Password</th>
        <td><input type="password" name="administrator_password" /></td>
    </tr>
    <tr>
        <th>Verify Password</th>
        <td><input type="password" name="administrator_password_verify" /></td>
    </tr>
</table>

<p>
<input type="submit" name="confirm" value="Set Passwords" />
</p>

</form>