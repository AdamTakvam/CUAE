<p>
Are you <strong>sure</strong> you want to delete the {$call_manager.name|escape:"html"} Unified Communications Manager Cluster?
</p>
<form action="{$SCRIPT_NAME}" method="post">
	<input type="hidden" name="id" value="{$id}" />
	<input type="submit" name="delete_yes" value="Yes" />
	<input type="submit" name="delete_no" value="No" />
</form>
