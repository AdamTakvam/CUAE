{if $_sdk_mode && !$_dev_mode}
<p class="notice">
This server is qualified only for SDK licensing.  SDK licensing allows for development and test use only.
</p>
{/if}

<h3>License Statistics</h3>
<table style="width: auto">
    <caption>
        Cisco Unified Application Server License Mode: {$cuae_license_mode}
        <br />
        Cisco Unified Media Engine License Mode: {$cume_license_mode}
    </caption>
    <col />
    <col style="width: 5em;" />
    <col style="width: 5em;" />
    <col />
    <tr>
        <th>Resource</th>
        <th>Max</th>
        <th>Licensed</th>
        <th>Active</th>
        <th>Stats</th>
    </tr>
    {foreach from=$license_values item=lic_value key=oid}
    <tr class="{cycle values='oddRow,evenRow'}">
        <td>{$lic_value.name}</td>
        <td>{if $lic_value.max != "9999"}{$lic_value.max|string_format:"%u"}{else}Unlimited{/if}</td>
        <td>{$lic_value.total|string_format:"%u"}</td>
        <td>{$lic_value.active|string_format:"%u"}</td>
        <td><a href="stats_view.php?oid={$oid}" class="button" />View Graphs</a></td> 
    </tr>
    {/foreach}
</table>

<h3>License File Management</h3>

<p class="notice">
Please note that any change in licensing will require restarting the media engine.
</p>

<table style="width: auto">
    <tr><th>License File</th><th>Actions</th></tr>
    {foreach from=$license_file_list item=file}
    <tr class="{cycle value='oddRow,evenRow'}">
	    <td>{$file}</td>
	    <td>
	    	<a href="license_details.php?{$file|escape:url}" onclick="window.open('license_details.php?{$file|escape:url}', 'lic_details', 'scrollbars=1,toolbar=0,menubar=0,location=0,height=320,width=400'); return false;" class="button">View Details</a> 
	    	<a href="{$PHP_SELF}?del={$file|escape:url}" class="button">Delete</a>
	    </td>
    </tr>
    {foreachelse}
    <tr><td colspan="2">No license files were found.</td></tr>
    {/foreach}
</table>

<br />

<form method="post" action="{$smarty.server.SCRIPT_NAME}" enctype="multipart/form-data">
    <label>Add License File</label> <input type="file" name="upload_license" /> <input type="submit" name="upload" value="Upload" />
</form>