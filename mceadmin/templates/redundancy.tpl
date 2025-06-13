<p>
The server must be assigned an ID number which will be unique relative to all other servers in the setup.  
</p>

<form action="{$SCRIPT_NAME}" method="post" name="redundancyForm">

<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Server ID</th>
        <td><input type="text" name="server_id" value="{$server_id}" /></td>
        <td>Positive integer ID that <strong>must be unique</strong> to this appliance relative to all other appliances in the redundancy setup</td>
    </tr>
</table>


<h3>As Master</h3>
<p>
Configure the settings for this appliance to be the master with a stand-by appliance.  Enable the master setup by setting an address, username, and password for the stand-by.  
To disable master setup, simply remove the address of the standby.
</p>
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Address of Stand-by</th>
        <td>
            <input type="text" name="redundancy_standby_ip" value="{$redundancy_standby_ip}"/>
        </td>
        <td>Fully qualified address of the stand-by appliance</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Database Username</th>
        <td><input type="text" name="redundancy_standby_username" value="{$redundancy_standby_username|escape:"html"}" /></td>
        <td>Set username for stand-by access</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Database Password</th>
        <td><input type="password" name="redundancy_standby_password" /></td>
        <td>Set password for stand-by access {if $redundancy_standby_password}(Leave blank for no change){/if}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Verify Password</th>
        <td><input type="password" name="verify_redundancy_standby_password" /></td>
        <td>Enter password again</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Startup Synchronization Timeout</th>
        <td><input type="text" name="redundancy_standby_startup_sync_time" value="{$redundancy_standby_startup_sync_time}" /> seconds</td>
        <td></td>
    </tr>    
</table>

<h3>As Stand-by</h3>
<p>
Configure the settings for this appliance to be the stand-by for a master appliance.  Enable stand-by setup by setting an address, username, and password to access the master.
</p>
{if $config_file_host && not $slave_enabled}
    <p>
    Due to inherent database restrictions, if application suite replication is turned on and the appliance is set as a subscriber, then settings for stand-by setup must be the same.
    In this case, the fields are already filled out and you simply click "Enable".
    </p>
{/if}
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Address of Master</th>
        <td>
            <input type="text" name="redundancy_master_ip" value="{if $redundancy_master_ip}{$redundancy_master_ip}{else}{$config_file_host}{/if}" {if not $slave_enabled && $config_file_host}disabled="disabled"{/if} />
            {if $config_file_host && not $slave_enabled}
                <input type="button" value="Enable" onclick="javascript:document.redundancyForm.redundancy_master_ip.disabled = false;" />
            {/if}
        </td>
        <td>Resolvable address of the master appliance</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Database Username</th>
        <td><input type="text" name="redundancy_master_username" value="{$redundancy_master_username|escape:"html"}" /></td>
        <td>Username to access master database</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Database Password</th>
        <td><input type="password" name="redundancy_master_password" /></td>
        <td>Password to access master database {if $redundancy_master_password}(Leave blank for no change){/if}</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Verify Password</th>
        <td><input type="password" name="verify_redundancy_master_password" /></td>
        <td>Enter password again</td>
    </tr>    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Heartbeat Interval</th>
        <td><input type="text" name="redundancy_master_heartbeat" value="{$redundancy_master_heartbeat}" /> seconds</td>
        <td>Time in between actions of checking the master server</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Max Missed Heartbeats</th>
        <td><input type="text" name="redundancy_master_max_missed_heartbeat" value="{$redundancy_master_max_missed_heartbeat}" /></td>
        <td></td>
    </tr>    
</table>

<p class="notice">
Please note that if you make any changes involving the Server ID or address of the master server, 
then <strong>the application server and all related services, including the database, will shut down and restart.</strong>
This may take up to a minute or two depending on the state of the services.
</p>

<p>
<input type="submit" name="update" value="Apply Settings" />
<input type="submit" name="done" value="Done" />
</p>

</form>