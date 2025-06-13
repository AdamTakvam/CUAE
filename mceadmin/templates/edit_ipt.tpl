{capture name=buttons}
    <input type="hidden" name="type" value="{$type}" />
    <input class="submit" type="submit" name="uninstall" value="Uninstall {$type_display}" />
{/capture}    

{capture name=configs}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Name</th>
        <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
        <td></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Description</th>
        <td><textarea name="description">{$_metadata.description|escape:"html"}</textarea></td>
        <td></td>
    </tr>
{/capture}

{include file="component_configs.tpl" buttons=$smarty.capture.buttons additional_configs=$smarty.capture.configs}