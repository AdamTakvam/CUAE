<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col class="inputLabels" />
    <col />
    <col />
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Name</th>
        <td><input type="text" name="add_name" value="{$add_name|escape:"html"}" /></td>
        <td>&nbsp;</td>
    </tr>
    {foreach from=$configs item=config}
    {if $config.name neq "MetreosReserved_ConnectionType"}
        <tr class="{cycle values='oddRow,evenRow'}">
            <th>{$config.display_name|escape:"html"}</th>
            <td>{$config.fields}</td>
            <td>
                {$config.description|escape:"html"}
                {if $config.meta_description neq ''}
                <span class="configRange">{$config.meta_description}</span>
                {/if}
            </td>
        </tr>
    {/if}
    {/foreach}
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Add to Group</th>
        <td>
            <select name="add_to_group">
            {foreach from=$groups item=group}
                <option value="{$group.mce_component_groups_id}">{$group.name|escape:"html"}</option>
            {/foreach}
            </select>
        </td>
        <td>&nbsp;</td>
    </tr>
</table>
<input type="hidden" name="MetreosReserved_ConnectionType" value="IPC" />
<input type="submit" name="add" value="Add" />
<input type="submit" name="cancel" value="Cancel" />
</form>