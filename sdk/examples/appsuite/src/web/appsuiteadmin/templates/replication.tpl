{if $role == 0 && !$selected}
	<p>
	You can set up this server to replicate the application suite database as a subscriber or a publisher.
	If you wish to set up a replication system, please select which role you want for this server.
	</p>
	
	<form method="post" action="{$SCRIPT_NAME}">
	<ul style="list-style-type:none">
		<li><input type="radio" name="role" value="1" />Publisher</li>
		<li><input type="radio" name="role" value="2" />Subscriber</li>
	</ul>
	<input type="submit" name="select" value="Select Role" />
	</form>
{else}

{if $selected}
<p>
<strong>PLEASE NOTE</strong> that if any of the fields are already populated, then this is because redundancy for the MCE environment is also set up on this machine.  Changing the <strong>Server ID</strong>
here will change it for all the databases on the appliance.  
{if $role == 2}Due to database restrictions, if the appliance is already configured in the MCE environment as a standby, then the settings for a subscriber are the same as the settings for MCE standby.{/if}
</p>
{/if}

<form method="post" action="{$SCRIPT_NAME}">
<input type="hidden" name="role" value="{$role}" />
<input type="hidden" name="selected" value="{$selected}" />
<table>
	<col class="inputLabels" />
	<col class="inputFields" />
	<col class="inputDescription" />
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Server ID</th>
		<td><input type="text" name="server_id" value="{$server_id}" /></td>
		<td>Positive integer ID of this server.  <strong>Must be unique</strong> relative to every other server in the publisher/subscriber replication setup.</td>
	</tr>
{if $role == 2}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Host</th>
		<td><input type="text" name="host" value="{$host|escape:"html"}" /></td>
		<td>Name of the publisher server to replicate</td>
	</tr>
{/if}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Username</th>
		<td><input type="text" name="user" value="{$user|escape:"html"}" /></td>
		<td>Username to use for the database replication user</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Password</th>
		<td><input type="password" name="password" /></td>
		<td>Password to use for the database replication user
		{if $password}(Leave blank for no change){/if}</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Verify Password</th>
		<td><input type="password" name="password_verify" /></td>
		<td>Enter password again for verification</td>
	</tr>

</table>
<p class="notice">
	If you commit any changes, please note that the database will temporarily be shut down
	and then restarted.
</p>
<p>
	<input type="submit" name="commit" value="Commit Changes" />
	{if !$selected}
	<input type="submit" name="disable" value="Disable Replication" />
	{/if}
</p>
</form>
{/if}