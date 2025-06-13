<p>
Are you <strong>sure</strong> you want to delete the partition {$name|escape:"html"}?
</p>
<form method="post" action="{$SCRIPT_NAME}">
	<input type="hidden" name="id" value="{$id}" />
	<input type="hidden" name="app_id" value="{$app_id}" />
	<input type="submit" name="submit_yes" value="Yes" class="submit" />
	<input type="submit" name="submit_no" value="No" class="submit" />
</form>