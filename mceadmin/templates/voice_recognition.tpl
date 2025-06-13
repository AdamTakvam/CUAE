
<p>
The only speech recognition server that we currently support is Nuance OSR. Please note that any changes to either list of servers will require you to manually restart the media engine
for the changes to take efffect.
</p>

<form action="{$SCRIPT_NAME}" method="post">
<h3>Speech Recognition Servers</h3>

<table style="width:500px">
<col />
<col />
<col style="width:100px" />
<tr>
    <th>Host</th>
    <th>Port</th>
    <th>&nbsp;</th>
</tr>
{foreach from=$vrservers key=index item=server}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td>{$server.host|escape:"html"}</td>
    <td>{$server.port}</td>
    <td><input type="submit" name="vr_delete[{$index}]" value="Remove" /></td>
</tr>
{/foreach}
<tr style="background-color:#eee;">
    <td><input type="text" name="vr_host_add" value="{$vr_host_add}" size="40"/></td>
    <td><input type="text" name="vr_port_add" value="{if $vr_port_add}{$vr_port_add}{else}4904{/if}" size="6" /></td>
    <td><input type="submit" name="vr_add" value="Add Server" /></td>
</tr>
</table>


<h3>Speech Recognition License Servers</h3>

<table style="width:500px">
<col />
<col />
<col style="width:100px" />
<tr>
    <th>Host</th>
    <th>Port</th>
    <th>&nbsp;</th>
</tr>
{foreach from=$vrlservers key=index item=server}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td>{$server.host|escape:"html"}</td>
    <td>{$server.port}</td>
    <td><input type="submit" name="vrl_delete[{$index}]" value="Remove" /></td>
</tr>
{/foreach}
<tr style="background-color:#eee;">
    <td><input type="text" name="vrl_host_add" value="{$vrl_host_add}" size="40" /></td>
    <td><input type="text" name="vrl_port_add" value="{if $vrl_port_add}{$vrl_port_add}{else}27000{/if}" size="6" /></td>
    <td><input type="submit" name="vrl_add" value="Add Server" /></td>
</tr>
</table>

</form>