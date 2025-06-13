<script type="text/javascript" src="./js/setallcheckboxes.js"></script>

<!-- List & Manage Device Pools -->

<h3>Device Pool Management</h3>

<form action="{$SCRIPT_NAME}" method="post" name="device_list_form" />
    
    <p>
    <strong>Search for devices</strong> by username for
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
            <td colspan="2">
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
            <th>Username</th>
            <th>Status</th>
        </tr>
        {$buttons}
        {foreach from=$devices item=device}
        <tr class="{cycle values='evenRow,oddRow'}">
            <td><input type="checkbox" name="select[]" value="{$device.mce_ietf_sip_devices_id}" /></td>
            <td>{$device.username|escape:"html"}</td>
            <td>{$device.status_display}</td>
        </tr>
        {foreachelse}
        <tr>
            <td colspan="3">No devices were found.</td>
        </tr>
        {/foreach}
        {$buttons}
    </table>
    
    <p>{include file="page_nav.tpl"}</p>

    <input type="hidden" name="p" value="{$p}" />
    <input type="hidden" name="id" value="{$id}" />
</form>


<!-- Add Devices -->

<h3>Add To Device Pool</h3>

<form action="{$_SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <col class="inputDescriptions" />
        <tr class="{cycle values='evenRow,oddRow'}">
            <th>Username</th>
            <td><input type="text" name="add_username" value="{$add_username|escape:"html"}" /></td>
            <td></td>
        </tr>
        <tr class="{cycle values='evenRow,oddRow'}">
            <th>Password</th>
            <td><input type="password" name="add_password" /></td>
            <td></td>
        </tr>
        <tr class="{cycle values='evenRow,oddRow'}">
            <th>Verify Password</th>
            <td><input type="password" name="add_password_verify" /></td>
            <td></td>
        </tr>
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="add_device" value="Add Device" />
</form>
