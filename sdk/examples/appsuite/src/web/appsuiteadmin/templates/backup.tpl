<table>
	<tr>
		<th>Date</th>
		<th>Name</th>
		<th>Status</th>
		<th colspan="2">&nbsp;</th>
	</tr>
	{foreach from=$backups item=backup}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td>{$backup.backup_date|date_format:"%D %I:%M:%S %p"}</td>
		<td>{$backup.name|escape:"html"}</td>
		<td>{$backup.status_display}</td>
		<td>[ <a href="backup_download.php?id={$backup.as_backups_id}">Download</a> ]</td>
		<td>[ <a href="backup_delete.php?id={$backup.as_backups_id}">Delete</a> ]</td>
	</tr>
	{/foreach}
</table>
<p><a href="backup_perform.php" class="button">Perform a Backup</a></p>