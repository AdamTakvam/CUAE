<p>
The application suite restore allows you to use a previously made backup file to bring the application suite back to
the state it was in at the time of the backup.  You may select from the currently stored backups or you may upload 
an external backup file.
</p>
<form action="{$SCRIPT_NAME}" method="post">
	<strong>Select a Backup:</strong> 
	<select name="backup_id">
		<option></option>
		{foreach from=$backups item=backup}
		<option value="{$backup.as_backups_id}">{$backup.backup_date|date_format:"%D %I:%M:%S %p"}</option>
		{/foreach}
	</select>
	<input type="submit" name="select" value="Restore From Backup" />
</form>
<form action="{$SCRIPT_NAME}" method="post" enctype="multipart/form-data">
	<strong>Upload Restore File:</strong> 
	<input type="file" name="restore_file" />
	<input type="submit" name="upload" value="Upload File" />
</form>