<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Name</th>
            <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
            <td></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Device Name</th>
            <td><input type="text" maxlength="15" name="dn" value="{$dn|escape:"html"}" /></td>
            <td>Device identifier as understood by Unified Communications Manager</td>
        </tr>
        {foreach from=$configs item=config}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>{$config.display_name|escape:"html"}</th><td>{$config.fields}</td><td>{$config.description|escape:"html"}</td>
        </tr>
        {/foreach}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Add To Group</th>
            <td>
                <select name="add_to_group">
                    <option value="">(None)</option>
                {foreach from=$groups item=group}
                    <option value="{$group.mce_component_groups_id}" {if $add_to_group == $group.mce_component_groups_id}selected="selected"{/if}>{$group.name|escape:"html"}</option>
                {/foreach}
                </select>
            </td>
            <td>Select a call route group for this route point</td>
        </tr>
    </table>
    <input type="hidden" name="cluster_id" value="{$cluster_id}" />
    <input type="submit" name="submit" class="submit" value="Create CTI Route Point" />
</form>