<script type="text/javascript" src="/appsuiteadmin/js/user_autocomplete.js"></script>

<p>
Please note that the scheduled time that you enter for a conference will be according
to your timezone.
</p>
{if !$conf_dn}
<p class="warning">
There is currently no number set as the conference dial number.  Please get an administrator to
set the conference dial number.
</p>
{/if}

<form action="{$SCRIPT_NAME}" method="post" name="conferenceForm">
<table>
	<col class="inputLabels" />
	<col class="inputFields" />
{if $group_admin_access || $admin_access}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Host Username</th>
	{if $id > 0}
			<td>{$username|escape:"html"}</td>
	{else}
    <td>
        <input type="text" name="username" style="width:15em;" onkeyup="acCallback.field = this; acCallback.formname = document.conferenceForm; getUser(this,event,true);" autocomplete="off" value="{$username|escape:"html"}" /> 
        <ul id="fancyDropdown" style="left: 15.25em" ></ul>
    </td>
	{/if}
	</tr>
{/if} 
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Scheduled Date</th>
		<td>
			{html_select_date field_array="datetime" prefix="" end_year=+5 time=$datetime}
		</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Scheduled Time</th>
		<td>
			{html_select_time field_array="datetime" prefix="" minute_interval=5 use_24_hours=false display_seconds=false time=$datetime}
		</td>
	</tr>
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Duration</th>
		<td>
			<input type="text" name="duration[hours]" value="{$duration.hours}" size="3" /> hr(s) 
			<input type="text" name="duration[minutes]" value="{$duration.minutes}" size="2" maxlength="2" /> min(s) 
		</td>
	</tr>	
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Expected Participants</th>
		<td><input type="text" name="num_participants" value="{$num_participants}" /></td>
	</tr>
	{if $id > 0}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Conference DN</th>
		<td>{$conf_dn|escape:"html"}</td>
	</tr>	
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Host Conference PIN</th>
		<td>{$host_conf_id}</td>
	</tr>	
	<tr class="{cycle values='rowOne,rowTwo'}">
		<th>Participant Conference PIN</th>
		<td>{$participant_conf_id}</td>
	</tr>
	{/if}
</table>
{if $id > 0}
	<input type="hidden" name="host_conf_id" value="{$host_conf_id}" />
	<input type="hidden" name="participant_conf_id" value="{$participant_conf_id}" />
	<input type="hidden" name="id" value="{$id}" />
	<input type="hidden" name="user_id" value="{$user_id}" />
	<input type="submit" name="submit" value="Apply Conference Settings" />
	<input type="submit" name="delete" value="Unschedule Conference" />
	<input type="submit" name="cancel" value="Done" />
{else}
	<input type="submit" name="submit" value="Create Conference" />
	<input type="submit" name="cancel" value="Cancel" />
{/if}
</form>

{if $id > 0}
<h3>Notify Participants</h3>

    {if $smtp_set}
    <p>
    List the email addresses of the expected conference participants below to send
    a notification of this conference.  The addresses can be separated by commas or
    be put on different lines.  The host will also be notified.
    </p>
    
    <form action="{$SCRIPT_NAME}" method="post">
    
    <p>
    <strong>Participant Emails</strong>:
    <br/>
    <textarea name="emails" rows="10" cols="50">{$emails|escape:"html"}</textarea>
    </p>
    
    <p>
    <strong>Additional Details</strong>:
    <br/>
    <textarea name="details" rows="10" cols="50">{$details|escape:"html"}</textarea>
    </p>
    
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="submit_notify" value="Send Notifications" />
    </form>
    {else}
    <p>
    The ability to e-mail participants about this conference is disabled.
    <br />
    Please ask your administrator to configure the SMTP server settings to enable this feature.
    </p>
    {/if}
{/if}
