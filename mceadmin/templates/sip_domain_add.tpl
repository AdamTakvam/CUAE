<form action="{$SCRIPT_NAME}" method="post">
<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescriptions" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Domain Name</th>
        <td><input type="text" name="domain_name" value="{$domain_name|escape:"html"}" /></td>
        <td>Resolvable SIP domain name</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Primary Registrar</th>
        <td><input type="text" name="primary_registrar" value="{$primary_registrar}" /></td>
        <td>Address of the primary registrar</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Secondary Registrar</th>
        <td><input type="text" name="secondary_registrar" value="{$secondary_registrar}" /></td>
        <td>
            Address of the secondary registrar
            <span class="metaDescription">Optional</span>
        </td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Outbound Proxy</th>
        <td><input type="text" name="outbound_proxy" value="{$outbound_proxy}" /></td>
        <td>
            Address of the outbound proxy
            <span class="metaDescription">Optional</span>
        </td>
    </tr>
</table>
<input type="hidden" name="type" value="{$type}" />
<input type="submit" name="submit" class="submit" value="Create SIP Domain" />
</form>