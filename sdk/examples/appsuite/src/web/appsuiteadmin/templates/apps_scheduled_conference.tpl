<p>
The following lists your currently scheduled conferences.
</p>

<p>{include file="page_nav.tpl"}</p>

<form action="{$SCRIPT_NAME}" method="post">
    <table class="statTable">
        <tr>
            {if $group_admin_access || $admin_access}
                <th>Host User</th>
            {/if}
            <th>Scheduled Time</th>
            <th>Duration</th>
            <th>Participants</th>
            <th>Host<br/> Conference PIN</th>
            <th>Participant<br/> Conference PIN</th>
            <th>Status</th>
            <th>Edit</th>
        </tr>
        {foreach from=$conferences item=conference}
        <tr class="{cycle values='rowOne,rowTwo'}">
            {if $group_admin_access || $admin_access}
                <td><a href="/appsuiteadmin/user.php?id={$conference.as_users_id}">{$conference.username|escape:"html"}</a></td>
            {/if}
            <td>{$conference.scheduled_timestamp|date_format:"%D %I:%M:%S %p"}</td>
            <td>{$conference.duration}</td>
            <td>{$conference.num_participants}</td>
            <td>{$conference.host_conf_id}</td>
            <td>{$conference.participant_conf_id}</td>
            <td>{$conference.status_display}</td>
            <td><input type="submit" name="edit[{$conference.as_scheduled_conferences_id}]" value="Edit" {if $conference.expired || $conference.disabled}disabled="disabled"{/if} /></td>
        </tr>
        {/foreach}
    </table>
    <br />
    <input type="submit" name="add" value="Add Conference" />
</form>
