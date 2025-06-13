<form method="post" action="{$SCRIPT_NAME}">
<table>
	<col class="inputLabels" />
	<col class="inputFields" />
	<col class="inputDescription" />
	{section name=x loop=$configs}
	<tr class="{cycle values='rowOne,rowTwo'}">
	    <th>{$configs[x].display_name}</th>
	    <td>{$configs[x].fields}</td>
	    <td>{$configs[x].description}</td>
	</tr>
	{/section}
</table>
<input type="hidden" name="type" value="{$type}" />
<input type="submit" name="add" value="Add {$type_display}" />
<input type="submit" name="cancel" value="Cancel" />
</form>
