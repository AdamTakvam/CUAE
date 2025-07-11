<p>
The following report presents a summary of account usage, organized by user's last name.
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
        This report will have to be generated by calculating from <strong>{$count}</strong> call records which could take a significant amount of time.  Are you sure you want to view this report?
        </p>
        <p>
        <input type="hidden" name="s_fvalues" value="{$s_fvalues|escape:"html"}" />
        <input type="submit" name="confirm_show" value="Yes" />
        <input type="submit" name="reject_show" value="No" />
        </p>
    </div>
{/if}

</fieldset>
</form>

{if $show_okay}
<p>{include file="page_nav.tpl"}</p>

<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="s_fvalues" value="{$s_fvalues|escape:"html"}" />
    <p><input type="submit" name="download" value="Download Report to a CSV File" /></p>
</form>

<form action="{$SCRIPT_NAME}" method="post">
<table class="statTable">
	<tr>
		<th>Account</th>
		<th>Status</th>
		<th>Total Call Duration</th>
		<th>Placed<br />Calls</th>
		<th>Successful<br />Calls</th>
		<th>Avg. Call Length</th>
		<th>Failed<br />Logins</th>
		<th>Last Login</th>
		<th>Details</th>
	</tr>
	{foreach from=$accounts item=account}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td><a href="/appsuiteadmin/user.php?id={$account.user_id}">{$account.name|escape:"html"}</a></td>
		<td>{$account.status_display}</td>
		<td>{$account.total_call_duration}</td>
		<td>{$account.placed_calls}</td>
		<td>{$account.successfull_calls}</td>
		<td>{$account.avg_call_length}</td>
		<td>{$account.failed_logins}</td>
		<td>{if $account.last_used neq '0000-00-00 00:00:00'}{$account.last_used|date_format:"%D %I:%M:%S %p"}{else}N/A{/if}</td>
		<td><input type="submit" name="view_detail[{$account.user_id}]" value="View Details" /></td>
	</tr>
	{/foreach}
</table>
</form>
{/if}