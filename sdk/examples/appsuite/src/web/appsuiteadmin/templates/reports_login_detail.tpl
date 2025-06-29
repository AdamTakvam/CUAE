<p>
The following report presents details regarding a login to the system.
</p>

<p>
	<strong>Account Code</strong>: {$user.account_code}
	<br />
	<strong>Username</strong>: {$user.username|escape:"html"}
	<br />
	<strong>Login Time</strong>: {$auth_timestamp|date_format:"%D %I:%M:%S %p"}
</p>

<p>{include file="page_nav.tpl"}</p>

<table class="statTable">
<tr>
	<th>Start</th>
	<th>Duration</th>
	<th>Origin Number</th>
	<th>Destination Number</th>
	<th>Application</th>
	<th>Partition</th>
	<th>End Reason</th>
</tr>
{foreach from=$calls item=call}
<tr class="{cycle values='rowOne,rowTwo'}">
	<td>{$call.start|date_format:"%D %I:%M:%S %p"}</td>
	<td>{$call.duration}</td>
	<td>{$call.origin_number|escape:"html"}</td>
	<td>{$call.destination_number|escape:"html"}</td>
	<td>{$call.application_name|escape:"html"}</td>
	<td>{$call.partition_name|escape:"html"}</td>
	<td>{$call.end_reason_display}</td>
</tr>
{/foreach}
</table>

<p>
<a href="account_detail.php?id={$user.as_users_id}">Go Back</a>
</p>