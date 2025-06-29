<p>
The following report tracks all the Find Me Number records associated with a regular call records.
</p>

<p>
    <strong>Account</strong>: {$name|escape:"html"}
    <br />
    <strong>Origin Number</strong>: {$call_record.origin_number|escape:"html"}
    <br />
    <strong>Destination Number</strong>: {$call_record.destination_number|escape:"html"}
    <br />
    <strong>Application</strong>: {$call_record.application_name|escape:"html"}
    <br />
    <strong>Partition</strong>: {$call_record.partition_name|escape:"html"}
    <br />
    <strong>Script</strong>: {$call_record.script_name|escape:"html"}
    <br />
    <strong>Start</strong>: {$call_record.start|date_format:"%D %I:%M:%S %p"}
    <br />
    <strong>Duration</strong>: {$call_record.duration}
    <br />
    <strong>End Reason</strong>: {$call_record.end_reason_display}
</p>

<table class="statTable">
    <tr>
        <th>From</th>
        <th>To</th>
        <th>Type</th>
        <th>End Reason</th>
    </tr>
    {foreach from=$findme_records item=record}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>{$record.calling_number|escape:"html"}</td>
        <td>{$record.called_number|escape:"html"}</td>
        <td>{$record.type_display}</td>
        <td>{$record.end_reason_display}</td>
    </tr>
    {/foreach}
</table>

<p>
<input type="button" value="Go Back" onclick="javascript: history.go(-1)" />
</p>