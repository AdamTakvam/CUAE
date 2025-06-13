{capture name=configs}
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Name</th>
        <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
        <td></td>
    </tr>
{/capture}

{include file="component_configs.tpl" additional_configs=$smarty.capture.configs}