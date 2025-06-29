<form action="{$SCRIPT_NAME}" method="post">
<input type="hidden" name="id" value="{$id}" />
<h3>Backup Metadata</h3>
<pre>
{$metadata|escape:"html"}
</pre>
<p>
Are you <strong>sure</strong> you want to delete the backup done on {$backup_date|date_format:"%D %I:%M:%S %p"}?
</p>
<input type="submit" name="delete_yes" value="Yes" />
<input type="submit" name="delete_no" value="No" />
</form>