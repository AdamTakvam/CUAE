<form method="post" action="{$SCRIPT_NAME}">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Name</th>
        <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
        <td></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Description</th>
        <td><textarea name="description">{$description|escape:"html"}</textarea></td>
        <td></td>
    </tr>
    {section name=x loop=$configs}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>{$configs[x].display_name|escape:"html"}</th>
        <td>{$configs[x].fields}</td>
        <td>{$configs[x].description|escape:"html"}</td>
    </tr>
    {/section}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Add to Group</th>
        <td>
            <select name="add_to_group">
            {foreach from=$groups item=group}
                <option value="{$group.mce_component_groups_id}">{$group.name|escape:"html"}</option>
            {/foreach}
            </select>
        </td>
        <td></td>
    </tr>
</table>
<input type="hidden" name="type" value="{$type}" />
<input type="submit" name="add" value="Add {$type_display}" />
<input type="submit" name="cancel" value="Cancel" />
</form>
