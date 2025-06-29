<form action="{$SCRIPT_NAME}" method="post" autocomplete="off">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr>
        <th>New PIN</th>
        <td><input type="password" name="pin" value="" /></td>
    </tr>
    <tr>
        <th>Verify PIN</th>
        <td><input type="password" name="pin_verify" value="" /></td>
    </tr>
    <input type="hidden" name="id" value="{$id}" />
</table>
    <input type="submit" name="update" value="Change PIN" />
    <input type="button" onclick="window.close();" value="Close" />
</form>