{literal}
<script type="text/javascript">
<!--
function SetAllCheckBoxes(FormName, FieldName, CheckValue)
{
	if(!document.forms[FormName])
		return;
	var objCheckBoxes = document.forms[FormName].elements[FieldName];
	if(!objCheckBoxes)
		return;
	var countCheckBoxes = objCheckBoxes.length;
	if(!countCheckBoxes)
		objCheckBoxes.checked = CheckValue;
	else
		// set the check value for all check boxes
		for(var i = 0; i < countCheckBoxes; i++)
			objCheckBoxes[i].checked = CheckValue;
}
// -->
</script>
{/literal}

{capture assign=pageNavigation}
<p>
	<strong>Page</strong>: 
	{if $page_logic->current > 1}
		<a href="{$SCRIPT_NAME}?p={$page_logic->previous}&amp;f={$current_path|escape:"url"}" class="pageNumber">Previous</a>
	{/if}
	{foreach from=$page_logic->page_numbers item=page_number}
		{if $page_logic->current != $page_number}
			<a href="{$SCRIPT_NAME}?p={$page_number}&amp;f={$current_path|escape:"url"}" class="pageNumber">{$page_number}</a>
		{else}
			{$page_number}
		{/if}
	{/foreach}
	{if $page_logic->current < $page_logic->last}
		<a href="{$SCRIPT_NAME}?p={$page_logic->next}&amp;f={$current_path|escape:"url"}" class="pageNumber">Next</a>
	{/if}
</p>
{/capture}

<p>
To view a log or open a directory, click on the file name.  
To create an archive of the logs, check the box next to each file you want to archive and click on 
the &quot;Archive Selected Logs&quot; button.
</p>

{$pageNavigation}

<form action="{$SCRIPT_NAME}" method="post" name="log_form">

<table>
	<col style="width:4em; text-align:center;" />
	<col />
    <col style="width:10em;" />
	<col style="width:15em;" />
	<tr>
		<th>Select</th>
		<th>File Name</th>
        <th>Size (Bytes)</th>
		<th>Last Modified</th>
	</tr>
	<tr>
		<td><input type="checkbox" onclick="SetAllCheckBoxes('log_form','logs[]',this.checked);" /></td>
		<td colspan="3">Select/Unselect All</td>
	</tr>
    {foreach from=$files item=file}
    	<tr class="{cycle values='rowOne,rowTwo'}">
    		<td>{if $file.name != 'PARENT DIRECTORY'}<input type="checkbox" name="logs[]" value="{$file.file_path}" />{/if}</td>
    		<td>
    		{if $file.dir}
    			[ DIR ] <a href="{$SCRIPT_NAME}?f={$file.file_path|escape:"url"}">{$file.name}</a>
    		{else}
    			<a href="view_log.php?{$file.file_path|escape:"url"}" onclick="window.open('view_log.php?{$file.file_path|escape:"url"}','','resizable=yes,scrollbars=yes');return false;">{$file.name}</a>
    		{/if}
    		</td>
            <td>
            {if $file.dir}
                &nbsp;
            {else}
                {$file.file_size}
            {/if}
            </td>
    		<td>
    			{$file.timestamp|date_format:"%D %I:%M:%S %p"}
    		</td>
    	</tr>
    {/foreach}
</table>

{$pageNavigation}

<input type="hidden" name="p" value="{$page_number}" />
<input type="hidden" name="f" value="{$current_path|escape:"url"}" />
<input type="submit" name="submit_archive" value="Archive Selected Logs" />
<input type="submit" name="submit_delete" value="Delete Selected Logs" />
</form>
