<form action="{$SCRIPT_NAME}" method="post" autocomplete="off">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr>
        <th>New Password</th>
        <td><input type="password" name="password" value="{$password}" /></td>
    </tr>
    <tr>
        <th>Verify Password</th>
        <td><input type="password" name="password_verify" value="{$password}" /></td>
    </tr>
    <input type="hidden" name="id" value="{$id}" />
</table>
    <input type="submit" name="update" value="Change Password" />
    <input type="button" onclick="window.close();" value="Close" />
</form>