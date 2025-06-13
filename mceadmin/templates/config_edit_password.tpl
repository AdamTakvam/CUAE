<p>
{$description}
<span class="metaDescription">{$meta_description}</span>
</p>

<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <tr class="rowTwo">
            <th>New Password</th>
            <td><input type="password" name="values[password]" /></td>
        </tr>
        <tr class="rowOne">
            <th>Verify New Password</th>
            <td><input type="password" name="values[password_verify]" /></td>
        </tr>
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="part_id" value="{$part_id}" />
    <input type="submit" name="update" value="Apply" />
    <input type="button" value="Close" onclick="window.close();" />
</form>