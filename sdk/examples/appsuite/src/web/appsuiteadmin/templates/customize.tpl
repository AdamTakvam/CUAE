{include file="system_mgmt_nav.tpl" selected="customize"}

<h3>Custom Logo</h3>

{if $custom_logo}
<form action="{$SCRIPT_NAME}" method="post" enctype="multipart/form-data">
    Your current logo:
    <br />
    <img src="./images/{$custom_logo}" alt="Custom Logo" />
    <br />
    <input type="submit" name="logo_remove" value="Remove Custom Logo" />
</form>
{else}
    <p>You may change the logo displayed on the Application Suite Administrator with one of your own.</p>
{/if}

<form action="{$SCRIPT_NAME}" method="post" enctype="multipart/form-data">
    <strong>Upload Custom Logo</strong> (GIF, JPG, BMP, or PNG): <input type="file" name="new_logo" /> <input type="submit" name="logo_submit" value="Upload" />
</form>


<h3>Expose Application Settings</h3>

<p>
You can control which application settings are exposed to users in the administrator.  For instance, you would disable exposing any settings for applications that are currently not installed.
</p>

<form action="{$SCRIPT_NAME}" method="post">
<table style="width: 25em;">
<col style="width: 2em;" />
<col />
<tr>
    <th>Expose</th>
    <th>Application</th>
</tr>
{foreach from=$application_list item=application}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td style="text-align:center"><input type="checkbox" name="applications[{$application.as_applications_id}]" value="1" {if $application.installed}checked="checked"{/if} /></td>
    <td>{$application.name}</td>
</tr>
{/foreach}
</table>
<input type="submit" name="app_settings_submit" value="Submit" />
</form>