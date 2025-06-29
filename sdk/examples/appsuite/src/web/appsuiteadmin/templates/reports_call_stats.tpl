<p>
The following report presents some overall call statistics.
</p>

<fieldset>
<legend>Report Filters</legend>
<form action="{$SCRIPT_NAME}" method="post">
{foreach from=$filters item=filter key=filter_name}
<label class="filterLabel">{$filter_name}</label>{eval var=$filter->RenderUI()}
<br />
{/foreach}

<br />
<label class="filterLabel">Sort by</label> 
    <select name="sort_by">
        <option value="">(none)</option>
        {foreach from=$sort item=sort_val key=sort_name}
        <option value="{$sort_val}"{if $sort_val eq $sort_by} selected="selected"{/if}>{$sort_name}</option>
        {/foreach}
    </select>
    <select name="sort_order">
        <option value="ASC"{if $sort_order eq "ASC"} selected="selected"{/if}>ascending</option>
        <option value="DESC"{if $sort_order eq "DESC"} selected="selected"{/if}>descending</option>
    </select>
    
<br />
<input type="hidden" name="show" value="1" />
<input type="submit" name="filter" value="Filter" />

{if $show && not $show_okay}
    <div style="background-color:#ffe; text-align: center; padding: 1em;">
        <p>
        This filtered report will yield <strong>{$count}</strong> records.  Are you sure you want to generate this report?
        </p>
        <p>
        <input type="hidden" name="s_fvalues" value="{$s_fvalues|escape:"html"}" />
        <input type="submit" name="confirm_show" value="Yes" />
        <input type="submit" name="reject_show" value="No" />
        </p>
    </div>
{/if}

</form>
</fieldset>

{if $show_okay}
<p>{include file="page_nav.tpl"}</p>

<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="s_fvalues" value="{$s_fvalues|escape:"html"}" />
    <p><input type="submit" name="download" value="Download Report to a CSV File" /></p>
</form>

<table class="statTable">
	<tr>
		<th>Account</th>
        <th>Find Me Details</th>
		<th>Origin Number</th>
		<th>Destination Number</th>
		<th>Application</th>
		<th>Partition</th>
        <th>Script</th>
		<th>Start</th>
		<th>Duration</th>
		<th>End Reason</th>
	</tr>
	{foreach from=$calls item=call}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td>
			{if $call.as_users_id}
			<a href="/appsuiteadmin/user.php?id={$call.as_users_id}">{$call.name|escape:"html"}</a>
            {else}
            -
			{/if}
		</td>
        <td>
            {if $call.findme_count > 0}
            <a href="/appsuiteadmin/reports/findme_records.php?id={$call.as_call_records_id}">View</a>
            {else}
            -
            {/if}
        </td>
		<td>{$call.origin_number|escape:"html"}</td>
		<td>{$call.destination_number|escape:"html"}</td>
		<td>{$call.application_name|escape:"html"}</td>
		<td>{$call.partition_name|escape:"html"}</td>
		<td>{$call.script_name|escape:"html"}</td>
		<td>{$call.start|date_format:"%D %I:%M:%S %p"}</td>
		<td>{$call.duration}</td>
		<td>{$call.end_reason_display}</td>
	</tr>
	{/foreach}
</table>
{/if}