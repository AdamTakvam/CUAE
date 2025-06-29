{if $is_slave_server}
<p class="error">
This is a backup replication server.  Changes to accounts and applications should not be made here, or it will lose
synchronization with the main server.  Please go to the <a href="http://{$master_server}/appsuiteadmin/">main server</a>.
</p>
{/if}

{if $group_admin_access}
<div class="mainSection">
    <h3>Management</h3>
    <ul>
        {if $admin_access}
        <li><a href="system_mgmt.php">System Management</a></li>
        {/if}
        <li><a href="account_mgmt.php">Account Management</a></li>
        <li><a href="group_mgmt.php">Group Management</a></li>
        {if $admin_access}
        <li><a href="replication.php">Replication Setup</a></li>
        <li><a href="backup.php">Settings &amp; Records Backup</a></li>
        <li><a href="restore.php">Settings &amp; Records Restore</a></li>
        {/if}
    </ul>
</div>

<div class="mainSection">
    <h3>Reports</h3>
    <ul>
        <li><a href="./reports/account_summary.php">Account Summaries</a></li>
        <li><a href="./reports/call_stats.php">Call Statistics</a></li>
        <li><a href="./reports/security.php">Security &amp; Access Control Reports</a></li>
    </ul>
</div>

<div class="mainSection" style="clear: left;">
    <h3>Application Suite</h3>
    <ul>
        <li><a href="./apps/scheduled_conference.php">ScheduledConference</a></li>
        <li><a href="./apps/intercom_admin.php">Intercom</a></li>
        <li><a href="./apps/call_recording_summary.php">RapidRecord</a></li>
        <li><a href="./apps/remote_agent.php">Remote Agent</a></li>
        {if not $admin_access}
        <li><a href="./apps/active_relay.php">ActiveRelay</a></li>
        {/if}
    </ul>
</div>
{/if}

{if !$admin_access && !$group_admin_access}
<div class="mainSection">
    <h3>Application Suite</h3>
    <ul>
        {if $app_sc_exposed}<li><a href="./apps/scheduled_conference.php">ScheduledConference</a></li>{/if}
        {if $app_i_exposed}<li><a href="./apps/intercom.php">Intercom</a></li>{/if}
        {if $app_rr_exposed}<li><a href="./apps/call_recording_summary.php">RapidRecord</a></li>{/if}
        {if $app_ar_exposed}<li><a href="./apps/active_relay.php">ActiveRelay</a></li>{/if}
    </ul>
</div>


<div class="mainSection">
    <h3>Account Management</h3>
    <ul>
        <li><a href="user.php?id={$user_id}">Edit Your Profile</a></li>
    </ul>
</div>
{/if}

<div class="clear"></div>
