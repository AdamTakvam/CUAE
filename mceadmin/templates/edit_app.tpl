{capture name=buttons}
    {if $enabled}
    <input class="submit" type="submit" name="disable" value="Disable Application" {if !$_app_server_on}disabled="disabled"{/if} />
    {else}
    <input class="submit" type="submit" name="enable" value="Enable Application" {if !$_app_server_on}disabled="disabled"{/if} />
    <input class="submit" type="submit" name="uninstall" value="Uninstall Application" {if !$_app_server_on}disabled="disabled"{/if} />
    {/if}
{/capture}

<div class="componentData">
{include file="component_metadata.tpl"}
</div>


{include file="component_configs.tpl" buttons=$smarty.capture.buttons}

<h3>Scripts</h3>
<table class="appScripts">
    <tr>
        <th>Name</th>
        <th>Event Type</th>
    </tr>
    {section name=y loop=$scripts}
    <tr class="{cycle values='oddRow,evenRow'}">
        <td>{$scripts[y].name|escape:"html"}</td>
        <td>{$scripts[y].event_type|escape:"html"}</td?
    </tr>
    {/section}
</table>


<h3>Partitions</h3>


    <table class="appPartitions">
        <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Actions</th>
        </tr>
        {section name=x loop=$partitions}
        <tr class="{cycle values='oddRow,evenRow'}">
            <td>{$partitions[x].name|escape:"html"}</td>
            <td>{$partitions[x].description|escape:"html"}</td>
            <td>
                <a href="edit_app_partition.php?id={$partitions[x].id}" class="button">Edit</a>
                {if $partitions[x].name neq "Default"}
                <a href="delete_app_partition.php?id={$partitions[x].id}" class="button">Delete</a>
                {/if}
            </td>
        </tr>
        {/section}
    </table>

<form action="add_app_partition.php" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="create_partition" class="submit" value="Create Partition" />
</form>


<h3>Update Application</h3>
{if $enabled}
    <p>To update this application to a new version, disable the application first.</p>
{else}
    <p>
        Upload the new application MCA file to update the application.
        {if !$_app_server_on}
        <strong>The application server is not currently running.</strong>  Any applications you have uploaded cannot be updated until you restart the server.
        {/if}
    </p>

    <form method="post" action="install_app.php" enctype="multipart/form-data">
        <input type="hidden" name="application_id" value="{$id}" />
        <input type="file" name="mca_file" /> <input class="submit" type="submit" name="update" value="Upload File" />
    </form>
{/if}