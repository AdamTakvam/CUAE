<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <tr class="rowOne">
            <th>Name</th>
            <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
        </tr>
        <tr class="rowTwo">
            <th>Address</th>
            <td><input type="text" name="ip_address" value="{$ip_address}" /></td>
        </tr>
    </table>
    <input type="hidden" name="cluster_id" value="{$cluster_id}" />
    <input type="hidden" name="new" value="new" />
    <input type="submit" name="add" class="submit" value="Add CTI Manager" />
    <input type="submit" name="done" class="submit" value="Go Back" />
</form>