
{include file="component_configs.tpl"}

{if $extensions}
<h3>Extensions</h3>

{foreach from=$extensions item=ext}
<div class="providerExtensions">
    <h4>{$ext.name}</h4>
    <form action="{$SCRIPT_NAME}" method="post">
        <p>
        {$ext.description}
        </p>
        {if $ext.params}
        <table>
            <col class="inputLabels" />
            <col class="inputFields" />
            <col class="inputDescription" />
            <caption>Parameters</caption>
            {foreach from=$ext.params item=param}
            <tr class="{cycle values='rowOne,rowTwo'}">
                <td>{$param.name}</td>
                {if $_app_server_on}<td>{$param.field}</td>{/if}
                <td>{$param.description}</td>
            </tr>
            {/foreach}
        </table>
        {/if}
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="extension_id" value="{$ext.id}" />
    <input type="submit" name="invoke_ext" value="Invoke Extension" {if !$_app_server_on}disabled="disabled"{/if} />
    </form>
</div>
{/foreach}
{/if}