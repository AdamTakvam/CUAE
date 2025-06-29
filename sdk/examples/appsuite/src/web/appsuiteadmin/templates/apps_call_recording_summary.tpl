<p>{include file="page_nav.tpl"}</p>

<table class="statTable">
    <tr>
        {if $admin_access || $group_admin_access}<th>User</th>{/if}
        <th>Origin Number</th>
        <th>Destination Number</th>
        <th>Start</th>
        <th>Duration</th>
        <th>Recordings</th>
    </tr>
    {foreach from=$calls item=call}
    <tr class="{cycle values='rowOne,rowTwo'}">
        {if $admin_access ||$group_admin_access}
        <td>
            {if $call.as_users_id}
            <a href="/appsuiteadmin/user.php?id={$call.as_users_id}">{$call.name|escape:"html"}</a>
            {/if}
        </td>
        {/if}
        <td>{$call.origin_number|escape:"html"}</td>
        <td>{$call.destination_number|escape:"html"}</td>
        <td>{$call.start|date_format:"%D %I:%M:%S %p"}</td>
        <td>{$call.duration}</td>
        <td><a href="call_recordings.php?id={$call.as_call_records_id}">{$call.recording_count} Recording(s)</a></td>
    </tr>
    {/foreach}
</table>