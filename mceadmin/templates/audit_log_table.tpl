<table>
    <tr>
        <th>Id</th>
        <th>Event Time</th>
        {if $type != 2}<th>Recovered Time</th>{/if}
        <th>Severity</th>
        {if $type != 2}<th>Status</th>{/if}
        <th>Code</th>
        <th>Message</th>
        <th>Details</th>
    </tr>
    {foreach from=$_audit_log item=entry}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><a href="audit_detail.php?id={$entry.mce_event_log_id}&t={$type}">{$entry.mce_event_log_id}</a></td>
        <td>{$entry.created_timestamp|date_format:"%D %I:%M:%S %p"}</td>
        {if $type != 2}
        <td>{if $entry.recovered_timestamp}{$entry.recovered_timestamp|date_format:"%D %I:%M:%S %p"}{else}-{/if}</td>
        {/if}
        <td>
            <strong>
            {if $entry.severity == 0}
                Unknown
            {elseif $entry.severity == 1}
                <span style="color: #900">ERROR</span>
            {elseif $entry.severity == 2}
                <span style="color: #ee6804">WARNING</span>
            {elseif $entry.severity == 3}
                <span style="color: #697e1c">OK</span>
            {/if}
            </strong>
        </td>
        {if $type != 2}<td>{$entry.display_status}</td>{/if}
        <td>{$entry.message_id}</td>
        <td>{$entry.message|escape:"html"}</td>
        <td style="font-size:x-small;"><a href="audit_detail.php?id={$entry.mce_event_log_id}&t={$type}">{$entry.data|truncate:50:"...":true|escape:"html"|nl2br}</a></td>
    </tr>
    {/foreach}
</table>