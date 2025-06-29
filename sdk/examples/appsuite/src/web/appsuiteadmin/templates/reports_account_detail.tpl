<p>
The following report presents authentication details for a specific account.
</p>

<p>
<strong>Account Code</strong>: {$user.account_code}
<br />
<strong>Username</strong>: {$user.username|escape:"html"}
<br />
<strong>Status</strong>: {$display_status}
</p>

<p>{include file="page_nav.tpl"}</p>

<form action="{$SCRIPT_NAME}" method="post">
<table class="statTable">
    <tr>
        <th>Login Time</th>
        <th>Status</th>
        <th>Originating Number</th>
        <th>Entered PIN</th>
        <th>Application</th>
        <th>Partition</th>
        <th>Calls</th>
        <th>&nbsp;</th>
    </tr>
    {foreach from=$auths item=auth}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>{$auth.auth_timestamp|date_format:"%D %I:%M:%S %p"}</td>
        <td>{$auth.status_display}</td>
        <td>{$auth.originating_number|escape:"html"}</td>
        <td>{if $auth.invalid_pin}{$auth.pin}{/if}</td>
        <td>{$auth.application_name|escape:"html"}</td>
        <td>{$auth.partition_name|escape:"html"}</td>
        <td>{$auth.call_count}</td>
        <td>
            {if $auth.call_count != 0}<input type="submit" name="view_detail[{$auth.as_auth_records_id}]" value="View Details" />{/if}
        </td>
    </tr>
    {/foreach}
</table>
</form>

<p>
<input type="button" value="Go Back" onclick="javascript: history.go(-1)" />
</p>