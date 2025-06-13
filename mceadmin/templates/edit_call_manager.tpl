<!-- Call Manager Cluster Settings -->

<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Name</th>
            <td><input type="text" name="call_manager[name]" value="{$call_manager.name|escape:"html"}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Version</th>
            <td>
            <select name="call_manager[version]">
                {foreach from=$cm_versions item=version}
                <option value="{$version}" {if $call_manager.version == $version}selected="selected"{/if}>{$version}</option>
                {/foreach}
            </select>
            </td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Publisher Address</th>
            <td><input type="text" name="call_manager[publisher_ip_address]" value="{$call_manager.publisher_ip_address}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Publisher Admin Username</th>
            <td><input type="text" name="call_manager[publisher_username]" value="{$call_manager.publisher_username|escape:"html"}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Change Publisher Admin Password</th>
            <td><input type="password" name="call_manager[new_password]" value="" /> <span class="metaDescription">Leave blank to keep old password</span></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Verify Publisher Admin Password</th>
            <td><input type="password" name="call_manager[new_password_verify]" value="" /> <span class="metaDescription">Leave blank to keep old password</span></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>SNMP Community</th>
            <td><input type="text" name="call_manager[snmp_community]" value="{$call_manager.snmp_community|escape:"html"}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Description</th>
            <td><textarea name="call_manager[description]">{$call_manager.description|escape:"html"}</textarea></td>
        </tr>
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="update_cluster" class="submit" value="Apply" />
    <input type="submit" name="uninstall_cluster" value="Uninstall Unified Communications Manager Cluster" />
</form>


<!-- SCCP Subscribers -->

<h3>SCCP Subscribers</h3>

<form action="subscriber.php" method="post">
    <input type="hidden" name="cluster_id" value="{$id}" />
    <table>
        <col />
        <col style="width:200px" />
        <col style="width:200px" />
        <tr>
            <th>Name</th>
            <th colspan="2">Address</th>
        </tr>
        {foreach from=$subscribers item=subscriber}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$subscriber.name|escape:"html"}</td>
            <td>{$subscriber.ip_address}</td>
            <td><input type="submit" name="edit[{$subscriber.mce_call_manager_cluster_subscribers_id}]" class="submit" value="Edit" /></td>
        </tr>
        {/foreach}
    </table>
    <input type="submit" name="new" class="submit" value="Add Subscriber" />
</form>


<!-- CTI Managers -->

<h3>CTI Managers</h3>

<form action="cti_manager.php" method="post">
    <input type="hidden" name="cluster_id" value="{$id}" />
    <table>
        <col />
        <col style="width:200px" />
        <col style="width:200px" />
        <tr>
            <th>Name</th>
            <th colspan="2">Address</th>
        </tr>
        {foreach from=$cti_managers item=manager}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$manager.name|escape:"html"}</td>
            <td>{$manager.ip_address}</td>
            <td><input type="submit" name="edit[{$manager.mce_call_manager_cluster_cti_managers_id}]" class="submit" value="Edit" /></td>
        </tr>
        {/foreach}
    </table>
    <input type="submit" name="new" class="submit" value="Add CTI Manager" />
</form>


<h3>Devices</h3>

{if $subscriber_count + $cti_manager_count == 0}
<p>Please create SCCP subscribers or CTI managers before creating device pools and route points.</p>
{/if}


{if $subscriber_count > 0}

<!-- SCCP Device Pools -->

<div style="border:1px #ddd solid; padding: 5px; margin-top: 5px;">
<h4>SCCP Device Pools</h4>

<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <table>
        <col />
        <col style="width:30%" />
        <tr>
            <th>Name</th>
            <th>Action</th>
        </tr>
        {foreach from=$sccp_device_pools item=device_pool}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$device_pool.name|escape:"html"}</td>
            <td>
                <input type="submit" name="edit_device_pool[{$device_pool.mce_components_id}]" class="submit" value="View Settings" />
                <input type="submit" name="manage_device_pool[{$device_pool.mce_components_id}]" class="submit" value="Manage Devices" />
            </td>
        </tr>
        {/foreach}
    </table>
</form>

<form action="add_sccp_device_pool.php" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="create_device_pool" class="submit" value="Create SCCP Device Pool" />
</form>
</div>
{/if}


{if $cti_manager_count > 0}

<!-- Monitored CTI Device Pools -->

<div style="border:1px #ddd solid; padding: 5px; margin-top: 5px;">
<h4>Monitored CTI Device Pools</h4>

<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <table>
        <col />
        <col style="width:30%" />
        <tr>
            <th>Name</th>
            <th>Action</th>
        </tr>
        {foreach from=$monitored_device_pools item=device_pool}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$device_pool.name|escape:"html"}</td>
            <td>
                <input type="submit" name="edit_device_pool[{$device_pool.mce_components_id}]" class="submit" value="View Settings" />
                <input type="submit" name="manage_device_pool[{$device_pool.mce_components_id}]" class="submit" value="Manage Devices" />
            </td>
        </tr>
        {/foreach}
    </table>
</form>

<form action="monitored_cti_device_pool_add.php" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="create_device_pool" class="submit" value="Create Monitored CTI Device Pool" />
</form>
</div>


<!-- CTI Device Pools -->

<div style="border:1px #ddd solid; padding: 5px; margin-top: 5px;">
<h4>CTI Device Pools</h4>

<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <table>
        <col />
        <col style="width:30%" />
        <tr>
            <th>Name</th>
            <th>Action</th>
        </tr>
        {foreach from=$cti_device_pools item=device_pool}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$device_pool.name|escape:"html"}</td>
            <td>
                <input type="submit" name="edit_device_pool[{$device_pool.mce_components_id}]" class="submit" value="View Settings" />
                <input type="submit" name="manage_device_pool[{$device_pool.mce_components_id}]" class="submit" value="Manage Devices" />
            </td>
        </tr>
        {/foreach}
    </table>
</form>

<form action="add_cti_device_pool.php" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="create_device_pool" class="submit" value="Create CTI Device Pool" />
</form>
</div>


<!-- CTI Route Points -->

<div style="border:1px #ddd solid; padding: 5px; margin-top: 5px;">
<h4>CTI Route Points</h4>

<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <table>
        <col />
        <col />
        <col />
        <col />
        <col style="width:15%" />
        <tr>
            <th>Name</th>
            <th>Device Name</th>
            <th>Directory Number</th>
            <th>Status</th>
            <th>Action</th>
        </tr>
        {foreach from=$cti_route_points item=route_point}
        <tr class="{cycle values='rowOne,rowTwo'}">
            <td>{$route_point.name|escape:"html"}</td>
            <td>{$route_point.device_name|escape:"html"}</td>
            <td>{$route_point.directory_number}</td>
            <td>{$route_point.status_display}</td>
            <td><input type="submit" name="edit_device_pool[{$route_point.mce_components_id}]" class="submit" value="View Settings" /></td>
        </tr>
        {/foreach}
    </table>
</form>

<form action="add_cti_route_point.php" method="post">
    <input type="hidden" name="cluster_id" value="{$id}" />
    <input type="submit" name="create_device_pool" class="submit" value="Create CTI Route Point" />
</form>
</div>


{/if}
