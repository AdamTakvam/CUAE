<form action="{$SCRIPT_NAME}" method="post">
    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Domain Name</th>
            <td><input type="text" name="sip_domain_data[domain_name]" value="{$sip_domain_data.domain_name|escape:"html"}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Primary Registrar</th>
            <td><input type="text" name="sip_domain_data[primary_registrar]" value="{$sip_domain_data.primary_registrar}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Secondary Registrar</th>
            <td><input type="text" name="sip_domain_data[secondary_registrar]" value="{$sip_domain_data.secondary_registrar}" /></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Outbound Proxy</th>
            <td><input type="text" name="sip_domain_data[outbound_proxy]" value="{$sip_domain_data.outbound_proxy}" /></td>
        </tr>
    </table>
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="update" class="submit" value="Apply" />
    <input type="submit" name="uninstall" value="Uninstall {$type_display}" />
</form>

<h3>Devices</h3>

<div style="border:1px #ddd solid; padding: 5px; margin-top: 5px;">
<h4>{$dp_type_display}s</h4>

<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <table>
        <col />
        <col style="width:20em;" />
        <tr>
            <th>Name</th>
            <th>Action</th>
        </tr>
        {foreach from=$sip_device_pools item=device_pool}
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

<form action="{if $type eq 1}sip_device_pool_add.php{else}ietf_sip_device_pool_add.php{/if}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="create_device_pool" class="submit" value="Create {$dp_type_display}" />
</form>
</div>

