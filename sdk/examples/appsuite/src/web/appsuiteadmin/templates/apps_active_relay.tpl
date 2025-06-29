<h3>Find Me Numbers</h3>
<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col />
    <col />
    <col />
    <col style="width: 10em;" />
    <tr>
        <th>Name</th>
        <th>Phone Number</th>
        <th>Enabled</th>
        <th>&nbsp;</th>
    </tr>
    {foreach from=$numbers item=number}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>{$number.name|escape:"html"}</td>
        <td>{$number.phone_number|escape:"html"}</td>
        <td>{if $number.ar_enabled}Enabled{else}Disabled{/if}</td>
        <td>
            <input type="submit" name="edit_number[{$number.as_external_numbers_id}]" value="Edit" />
            <input type="submit" name="delete_number[{$number.as_external_numbers_id}]" value="Delete" />
        </td>
    </tr>
    {/foreach}
</table>
<input type="hidden" name="id" value="{$id}" />
{if not $reached_numbers_limit}
    <input type="submit" name="add_number" value="Add Number" />
{/if}
</form>

<h3>Settings</h3>

<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Voice Mail Box</th>
        <td>
        <select name="corporate_number_id">
            <option value="0">None</option>
        {foreach from=$numbers item=number}
            <option value="{$number.as_external_numbers_id}" {if $number.as_external_numbers_id == $corporate_number_id}selected="selected"{/if}>{if $number.name}{$number.name|escape:"html"}{else}{$number.phone_number|escape:"html"}{/if}</option>
        {/foreach}
        </select>
        </td>
        <td>Number which has voice mail capabilities</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>ActiveRelay Transfer Number</th>
        <td>
            <input type="text" name="ar_transfer_number" value="{$ar_transfer_number|escape:"html"}" />
        </td>
        <td>Number that the caller is transferred to by ActiveRelay Transfer</td>
    </tr>
</table>
<input type="hidden" name="id" value="{$id}" />
<input type="submit" name="update" value="Apply" />
</form>


<h3>Single Reach Numbers</h3>
<form action="{$SCRIPT_NAME}" method="post">
<input type="hidden" name="id" value="{$id}" />
<table style="width:25em;">
    <col />
    <col />
    <tr>
        <th colspan="2">Number</th>
    </tr>
    {foreach from=$sr_numbers item=number}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>{$number.number|escape:"html"}</td>
        <td><input type="submit" name="delete_sr[{$number.as_single_reach_numbers_id}]" value="Delete" /></td>
    </tr>
    {/foreach}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td><input type="text" name="new_sr" value="" /></td>
        <td><input type="submit" name="add_sr" value="Add" /></td>
    </tr>
</table>    
</form>


<h3>ActiveRelay Filters</h3>
<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    
    <input type="submit" name="whitelist" value="Manage Whitelist" style="width:15em; margin-right:1em;" />
    A list of Caller IDs which will cause ActiveRelay to always activate.
    <br />
    <input type="submit" name="blacklist" value="Manage Blacklist" style="width:15em; margin-right:1em;" />
    A list of Caller IDs which will cause ActiveRelay to not activate.
</form>
