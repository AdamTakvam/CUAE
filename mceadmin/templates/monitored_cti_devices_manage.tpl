<script type="text/javascript" src="./js/setallcheckboxes.js"></script>


<!-- Add Devices -->

<h3>Add Device</h3>

<form action="{$_SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <col class="inputDescriptions" />
        <tr class="{cycle values='evenRow,oddRow'}">
            <th>Device Name</th>
            <td><input type="text" name="add_device_name" value="{$add_device_name|escape:"html"}" /></td>
            <td></td>
        </tr>
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="type" value="{$type}" />
    <input type="submit" name="add_one_device" value="Add" />
</form>


<!-- List & Manage Device Pools -->

<h3>Management</h3>

<form action="{$SCRIPT_NAME}" method="post" name="device_list_form" />

    <p>
    <strong>Search for devices</strong> by 
        <select name="search[field]">
            <option value="device_name" {if $search.field == "device_name"}selected="selected"{/if}>Device Name</option>
            <option value="directory_number" {if $search.field == "directory_number"}selected="selected"{/if}>Directory Number</option>
        </select>
    for
        <input type="text" name="search[text]" value="{$search.text|escape:"html"}" />
        <input type="hidden" name="search[mode]" value="{$search.mode}" />
    that are
        {html_options name="search[status]" options=$statuses selected=$search.status}
        <input type="submit" name="sub_search" value="Search" />
    </p>
    
    <p>{include file="page_nav.tpl"}</p>
    
    <table>
        <col style="width:20px;" />
        <col />
        <col />
        <col />
    {capture assign="buttons"}
        <tr style="background-color: #ddd;">
            <td colspan="3">
                <input type="button" onclick="SetAllCheckBoxes('device_list_form','select[]',true);" value="Select All" />
                <input type="button" onclick="SetAllCheckBoxes('device_list_form','select[]',false);" value="Select None" />
                <input type="submit" name="delete" value="Delete Devices" />
            </td>
            <td>
                <input type="submit" name="refresh" value="Refresh" />
            </td>
        </tr>
    {/capture}
        <tr>
            <th>&nbsp;</th>
            <th>Device Name</th>
            <th>Directory Number</th>
            <th>Status</th>
        </tr>
        {$buttons}
        {foreach from=$devices item=device}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td><input type="checkbox" name="select[]" value="{$device.mce_call_manager_devices_id}" /></td>
            <td>{$device.device_name|escape:"html"}</td>
            <td>{$device.directory_number}</td>
            <td>{$device.status_display}</td>
        </tr>
        {foreachelse}
        <tr>
            <td colspan="4">No devices were found.</td>
        </tr>
        {/foreach}
        {$buttons}
    </table>
    
    <p>{include file="page_nav.tpl"}</p>

    <input type="hidden" name="p" value="{$p}" />
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="type" value="{$type}" />
</form>
