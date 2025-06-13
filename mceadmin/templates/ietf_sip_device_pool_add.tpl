<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Name</th>
            <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
            <td></td>
        </tr>
        {foreach from=$configs item=config}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>{$config.display_name|escape:"html"}</th><td>{$config.fields}</td><td>{$config.description|escape:"html"}</td>
        </tr>
        {/foreach}
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="submit" class="submit" value="Create IETF SIP Device Pool" />
</form>