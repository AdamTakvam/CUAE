<p>
    <strong>Origin Number</strong>: {$record.origin_number|escape:"html"}<br />
    <strong>Destination Number</strong>: {$record.destination_number|escape:"html"}<br />
    <strong>Start Time</strong>: {$record.start|date_format:"%D %I:%M:%S %p"}<br />
    <strong>Duration</strong>: {$record.duration}<br />
</p>

<table class="statTable">
    <tr>
        {if $group_admin_access || $admin_access}<th>User</th>{/if}
        <th>Start</th>
        <th>Duration</th>
        <th>Recording Type</th>
        <th>Download</th>
    </tr>
    {foreach from=$recordings item=recording}
    <tr class="{cycle values='rowOne,rowTwo'}">
        {if  $group_admin_access || $admin_access}<td>{$recording.name|escape:"html"}</td>{/if}
        <td>{$recording.start|date_format:"%D %I:%M:%S %p"}</td>
        <td>{$recording.duration}</td>
        <td>{$recording.type_display}</td>
        <td>
            {if $recording.end neq '0000-00-00 00:00:00'}
            <a href="http://{$global_webhost}/{$global_recording_path}/{$recording.url}">Download</a>
            {else}
            &nbsp;
            {/if}
        </td>
    </tr>
    {/foreach}
</table>