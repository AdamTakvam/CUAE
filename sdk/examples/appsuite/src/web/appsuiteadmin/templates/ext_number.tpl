<form action="{$SCRIPT_NAME}" method="post">
<input type="hidden" name="from" value="{$from}" />
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Name (Optional)</th>
        <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
        <td>Example: "My Cell Phone"</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Phone Number</th>
        <td><input type="text" name="phone_number" value="{$phone_number|escape:"html"}" /></td>
        <td>
            {if not $number_description}
                No dashes, periods, or hyphens - Example: "8885554444" or "4444"
            {else}
                {$number_description|escape:"html"}
            {/if}
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Delay Call Time</th>
        <td><input type="text" name="delay_call_time" value="{$delay_call_time}" /></td>
        <td>Number of seconds to delay a call to this device</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Call Attempt Timeout</th>
        <td><input type="text" name="call_attempt_timeout" value="{$call_attempt_timeout}" /></td>
        <td>Number of seconds before ending a call attempt to this device; 0 for no timeout</td>
    </tr>
    {if $ar_exposed}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Enable ActiveRelay</th>
        <td>
        	<input type="checkbox" name="ar_enabled" {if $ar_enabled}checked="checked"{/if} />
        </td>
        <td>Enable ActiveRelay for this number</td>
    </tr>
    {/if}
</table>

<h3>Time of Day Restrictions</h3>

<p>
Time of day restrictions allow you to define when this number can be contacted during the weekday.  
{if $is_corporate}<strong>Voice Mail Box numbers are not allowed to have time of day restrictions.</strong>{/if}
</p>

<p>
<input type="checkbox" value="1" name="timeofday_enabled" {if $timeofday_enabled}checked="checked"{/if} {if $is_corporate}disabled="disabled"{/if} onclick="javscript:this.form.sat_enabled.disabled=this.form.sun_enabled.disabled=!this.checked" /> Enable for weekdays as well as: 
<input type="checkbox" value="1" name="sat_enabled" {if $timeofday_enabled}{if $timeofday_weekend & 1}checked="checked"{/if}{else}disabled="disabled"{/if} /> Saturday 
<input type="checkbox" value="2" name="sun_enabled" {if $timeofday_enabled}{if $timeofday_weekend & 2}checked="checked"{/if}{else}disabled="disabled"{/if}/> Sunday
</p>

<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Start</th>
        <td>{html_select_time display_seconds=false minute_interval=15 field_array=td_start use_24_hours=false prefix="" time=$timeofday_start}</td>
        <td>Start time of interval during which the device can be contacted</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>End</th>
        <td>{html_select_time display_seconds=false minute_interval=15 field_array=td_end use_24_hours=false prefix="" time=$timeofday_end}</td>
        <td>End time of interval during which the device can be contacted</td>
    </tr>     
</table>


<input type="hidden" name="user_id" value="{$user_id}" />
{if $id > 0}
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="submit" value="Apply Number Settings" />
	<input type="submit" name="cancel" value="Done" />
{else}
    <input type="submit" name="submit" value="Create Number" />
	<input type="submit" name="cancel" value="Cancel" />
{/if}
</form>
