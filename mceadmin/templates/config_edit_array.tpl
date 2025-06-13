<p>
{$description}
<span class="metaDescription">{$meta_description}</span>
</p>

<p>
You must press the the &quot;Apply&quot; button after adding values to commit the changes
to the configuration.
</p>

<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <tr>
            <th>Value</th>
            <th>&nbsp;</th>
        </tr>
        {counter name="x" start=-1 print=false}
        {foreach from=$values item=value}
        {counter name="x" assign="x"}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td><input type="text" name="values[{$x}][value]" value="{$value.value|escape:"html"}" /></td>
            <td><input type="submit" name="delete[{$x}]" value="Delete" /></td>
        </tr>
        {/foreach}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td><input type="text" name="add_value" value="" /></td>
            <td><input type="submit" name="add" value="Add" /></td>
        </tr>
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="part_id" value="{$part_id}" />
    <input type="submit" name="done" value="Apply" />
    <input type="button" value="Close" onclick="window.close();" />
</form>