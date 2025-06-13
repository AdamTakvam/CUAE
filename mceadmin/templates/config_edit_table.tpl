<p>
{$description}
<span class="metaDescription">{$meta_description}</span>
</p>

<p>
You must press the &quot;Apply&quot; button after editing the table to commit the changes
to the configuration.
</p>

<form action="{$SCRIPT_NAME}" method="post">
    <table>
        {counter name="x" start=-1 print=false}
        {foreach from=$values item=row}
        {counter name="x" assign="x"}
        <tr class="{cycle values='rowOne,rowTwo'}">

            {counter name="y" start=-1 print=false}
            {foreach from=$row item=cell}
            {counter name="y" assign="y"}
            <td><input type="text" name="values[{$x}][{$y}]" value="{$cell|escape:"html"}" /></td>
            {/foreach}
            <td><input type="submit" name="delete_row[{$x}]" value="Delete Row" /></td>

        </tr>
        {/foreach}
        <tr class="{cycle values='rowOne,rowTwo'}">
            {counter name="y" start=-1 print=false}
            {foreach from=$values[0] item=dummy}
            {counter name="y" assign="y"}
            <td><input type="submit" name="delete_col[{$y}]" value="Delete Column" /></td>
            {/foreach}
            <td>&nbsp;</td>
        </tr>
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="part_id" value="{$part_id}" />
    <p>
    <input type="submit" name="add_row" value="Add Row" />
    <input type="submit" name="add_col" value="Add Column" />
    </p>
    <p>
    <input type="submit" name="update" value="Apply" />
    <input type="button" value="Close" onclick="window.close();" />
    </p>
</form>