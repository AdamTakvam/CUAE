<p>
The following report presents a record of all login activity.
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
</form>
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
</fieldset>

{if $show_okay}
<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="s_fvalues" value="{$s_fvalues|escape:"html"}" />
    <p><input type="submit" name="download" value="Download Report to a CSV File" /></p>
</form>

<p>{include file="page_nav.tpl"}</p>

<table class="statTable">
	<tr>
		<th>Account</th>
		<th>Login Type</th>
		<th>Login Information</th>
		<th>Login Time</th>
		<th>Application</th>
		<th>Partition</th>
		<th>Status</th>
	</tr>
	{foreach from=$auths item=auth}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td>
			{if $auth.as_users_id}
			<a href="/appsuiteadmin/user.php?id={$auth.as_users_id}">{$auth.name|escape:"html"}</a>
			{/if}
		</td>
		<td>
			{if $auth.originating_number}
				Phone
			{elseif $auth.source_ip_address}
				Web
			{else}
				Call
			{/if}
		</td>
		<td>
			{if $auth.originating_number}
				Originating Number: {$auth.originating_number|escape:"html"}
				{if $auth.invalid_pin}<br />Entered PIN: {$auth.pin}{/if}
			{elseif $auth.source_ip_address}
				Source IP: {$auth.source_ip_address}
				<br />
				Username: {$auth.username|escape:"html"}
			{else}
				Unknown
			{/if}		
		</td>
		<td>{$auth.auth_timestamp|date_format:"%D %I:%M:%S %p"}</td>
		<td>{$auth.application_name|escape:"html"}</td>
		<td>{$auth.partition_name|escape:"html"}</td>
		<td>{$auth.status_display}</td>
	</tr>
	{/foreach}
</table>
{/if}