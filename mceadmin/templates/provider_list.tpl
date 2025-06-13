
{include file="component_list.tpl"}

<h3>Install A Provider</h3>

<p>
To install a provider, you will need to upload the provider assembly (DLL) or assembly package (MCP).
{if !$_app_server_on}
  <strong>The application server is currently not running.</strong>  Please restart the application server before installing a provider.
{/if}
</p>

<form method="post" action="provider_install.php" enctype="multipart/form-data">
    <input type="file" name="providerpackage" {if !$_app_server_on}disabled="disabled"{/if}/> <input class="submit" type="submit" name="upload" value="Upload File" {if !$_app_server_on}disabled="disabled"{/if}/>
</form>