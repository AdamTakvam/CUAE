<table>
	<col class="inputLabels" />
	<col />
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Log Message Id</th>
		<td>{$entry.mce_event_log_id}</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Log Message Type</th>
		<td>{$entry.type_display}</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Timestamp</th>
		<td>{$entry.created_timestamp|date_format:"%D %I:%M:%S %p"}</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Status</th>
		<td>
			<strong>
			{if $entry.severity == 0}
				Unknown
			{elseif $entry.severity == 1}
				<span style="color: #c00">ERROR</span>
			{elseif $entry.severity == 2}
				<span style="color: #c20">WARNING</span>
			{elseif $entry.severity == 3}
				<span style="color: #020">OK</span>
			{/if}
			</strong>		
		</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Message Code</th>
		<td>{$entry.message_id}</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Message</th>
		<td>{$entry.message|escape:"html"}</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Details</th>
		<td><pre>{$entry.data|escape:"html"}</pre></td>
	</tr>
</table>

<input type="button" value="Go Back" onclick="history.go(-1);" />