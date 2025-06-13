<table>
    <col />
    <col style="width: 5em;" />
    <col style="width: 15em;" />
    <tr>
        <th>Name</th>
        <th>Version</th>
        <th>Status</th>
    </tr>
{section name=x loop=$components}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><a href="edit_component.php?id={$components[x].mce_components_id}&amp;type={$components[x].type}">{$components[x].display_name|escape:"html"}</a></td>
        <td>{$components[x].version|escape:"html"}</td>
        <td>{$components[x].display_status}</td>
    </tr>
{/section}
</table>