<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <col class="inputDescription" />
        <tr class="rowOne">
            <th>Name</th>
            <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
            <td>&nbsp;</td>
        </tr>
        <tr class="rowTwo">
            <th>Address</th>
            <td><input type="text" name="ip_address" value="{$ip_address}" /></td>
            <td><span class="sideNotice">This change will not take effect until the Application Server has been restarted</span></td>
        </tr>
    </table>
    <input type="hidden" name="cluster_id" value="{$cluster_id}" />
    <input type="hidden" name="edit" value="{$id}" />
    <input type="submit" name="update" class="submit" value="Apply" />
    <input type="submit" name="delete" class="submit" value="Delete Subscriber" />
    <input type="submit" name="done" class="submit" value="Done" />
</form>