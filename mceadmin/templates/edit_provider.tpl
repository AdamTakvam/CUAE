{capture name=buttons}
    {if $enabled}
    <input class="submit" type="submit" name="disable" value="Disable Provider" />
    {else}
    <input class="submit" type="submit" name="enable" value="Enable Provider" />
    <input class="submit" type="submit" name="uninstall" value="Uninstall Provider" {if !$_app_server_on}disabled="disabled"{/if} />
    {/if}
{/capture}

<div class="componentData">
{include file="component_metadata.tpl"}
</div>

{include file="component_configs.tpl" buttons=$smarty.capture.buttons}

{if $extensions}
<h3>Extensions</h3>

{foreach from=$extensions item=ext}
<div class="providerExtensions">
    <h4>{$ext.name|escape:"html"}</h4>
    <form action="{$SCRIPT_NAME}" method="post">
        <p>
        {$ext.description|escape:"html"}
        <br />
        Status: {if $ext.wait_for_completion != 0}Busy{else}Ready{/if}
        </p>
        {if $ext.params}
        <table>
            <col class="inputLabels" />
            <col class="inputFields" />
            <col class="inputDescription" />
            <caption>Parameters</caption>
            {foreach from=$ext.params item=param}
            <tr class="{cycle values='rowOne,rowTwo'}">
                <td>{$param.name|escape:"html"}</td>
                {if $_app_server_on}<td>{$param.field|escape:"html"}</td>{/if}
                <td>{$param.description|escape:"html"}</td>
            </tr>
            {/foreach}
        </table>
        {/if}
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="extension_id" value="{$ext.id}" />
    <input type="submit" name="invoke_ext" value="Invoke Extension" {if !$_app_server_on || $ext.wait_for_completion != 0}disabled="disabled"{/if} />
    </form>
</div>
{/foreach}
{/if}