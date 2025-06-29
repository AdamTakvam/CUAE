{if $error}

	<p class="error">
	<strong>ERROR!  This backup has failed for the following reason:</strong>
	<br />
	{$error}
	</p>
	<form action="{$SCRIPT_NAME}" method="post">
		<input type="submit" name="done" value="Done" />
	</form>

{else}
	
	{if $step == 0}
		<p>
		You are about to create a backup of settings and records database of the application suite.
		<br />
		If you are sure you want to do this, press the &quot;Perform Backup&quot; button.
		</p>
		<form action="{$SCRIPT_NAME}" method="post">
			<input type="hidden" name="step" value="1" />
			<input type="submit" name="submit" value="Perform Backup" />
			<input type="submit" name="done" value="Cancel" />
		</form>
	{/if}
	
	{if $step == 1}
		<p>
		Backup completed successfully!
		</p>
		<form action="{$SCRIPT_NAME}" method="post">
			<input type="submit" name="done" value="Done" />
		</form>
	{/if}

{/if}