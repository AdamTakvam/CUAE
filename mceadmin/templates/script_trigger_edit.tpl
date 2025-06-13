
<h3>Trigger Parameters</h3>

<p>
<strong>Event Type</strong>: {$event_type|escape:"html"}
</p>

<form method="post" action="{$SCRIPT_NAME}">
<table>
    <col style="width:150px;" />
    <col />
    <tr>
        <th>Parameter Name</th>
        <th>Values</th>
    </tr>
    {foreach from=$triggers item=trigger key=trig_id}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>
        	<strong>{$trigger.name|escape:"html"}</strong>
        </td>
        <td>
        	{foreach from=$trigger.values item=value key=value_id}        	
        		<input type="text" name="parameters[{$trig_id}][{$value_id}]" value="{$value|escape:"html"}" />
        		<input type="submit" class="submit" name="delete_value[{$value_id}]" value="Delete Value" />
        		<br />
        	{/foreach}
        		<input type="text" name="add_value[{$trig_id}]" value="" /> 
        		<input type="submit" class="submit" name="add_value_submit[{$trig_id}]" value="Add Value" />
        </td>
    </tr>
    <tr style="background-color: #eee;">
    	<td colspan="2">
    		<input type="submit" class="submit" name="delete_parameter[{$trig_id}]" value="Delete Parameter" />
    		<input type="submit" name="update_parameter[{$trig_id}]" value="Apply Parameter Values" />
    	</td>
    </tr>
    {/foreach}
</table>


<h3>Add Trigger Parameter</h3>
<table>
	<col class="inputLabels" />
	<col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
    	<th>Parameter Name</th>
    	<td><input type="text" name="add_parameter[name]" value="" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
    	<th>Initial Value</th>
    	<td><input type="text" name="add_parameter[value]" value="" /></td>
    </tr>
</table>
<input type="submit" class="submit" name="add_parameter_submit" value="Add Parameter"></a>

<p>
	<input type="hidden" name="part_id" value="{$part_id}" />
	<input type="hidden" name="script_id" value="{$script_id}" />
	<input type="submit" class="submit" name="done" value="Done" />
</p>

</form>