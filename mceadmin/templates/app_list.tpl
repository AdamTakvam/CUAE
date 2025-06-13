
{include file="component_list.tpl"}

<h3>Install An Application</h3>

<p>
    Upload the application MCA file to install the application.
    {if !$_app_server_on}
    <strong>The application server is not currently running.</strong>  Any applications you have uploaded cannot be installed until you restart the server.
    {/if}
</p>

<form method="post" action="install_app.php" enctype="multipart/form-data">
    <input type="file" name="mca_file" /> <input class="submit" type="submit" name="install" value="Upload File" />
</form>