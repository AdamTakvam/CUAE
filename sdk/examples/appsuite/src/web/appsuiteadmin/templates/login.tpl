<p>
Please log in with a <strong>username</strong> and <strong>password</strong>.  
If you do not have a username and password, please contact your system administrator.
</p>

{if $is_slave_server}
<p class="error">
This is a backup replication server. Changes to accounts and applications should not be made here, or it will lose
synchronization with the main server.  Please log in to the <a href="http://{$master_server}/appsuiteadmin/">main server</a>.
</p>
{/if}

<form action="{$SCRIPT_NAME}" method="post">
<table>
	<col class="inputLabels" />
	<col class="inputFields" />
	<tr>
		<th>Username</th>
		<td><input type="text" name="username" /></td>
	</tr>
	<tr>
		<th>Password</th>
		<td><input type="password" name="password" /></td>
	</tr>
</table>
<input type="submit" name="submit" value="Log In" />
</form>