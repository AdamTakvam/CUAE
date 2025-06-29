{if $step == 1}
	<p>
	The selected backup has the following properties:
	</p>
	
<pre>
{$metadata|escape:"html"}
</pre>
	
	<p>
	The restore process will bring the application suite settings &amp; records
	back to the state defined by the selected backup file.  This means you will lose your current settings.
	<em>During the restore, we will have to temporarily turn off all related services, including the
	application server.</em>
	</p>
	<p>  
	<strong>Are you sure you want to proceed with this restore?</strong>
	</p>
	
	<form action="{$SCRIPT_NAME}" method="post">
		<input type="hidden" name="s_restore" value="{$s_restore}" />
		<input type="hidden" name="step" value="2" />
		<input type="submit" name="restore_yes" value="Yes" />
		<input type="submit" name="restore_no" value="No" />
	</form>
{/if}

{if $step == 2}
	<p>
	Shutting down services and restoring the application suite's state.  Please wait.
	</p>
{/if}

{if $step == 3}
	<p>
	{if $restore_good}
	Application suite restore has completed.  All enabled services have been restarted.
	{else}
	Application suite restore has failed!  All enabled services have been restarted.
	{/if}
	</p>
	
	<form action="restore.php" method="post">
		<input type="submit" name="_goto" value="Done" />
	</form>
{/if}