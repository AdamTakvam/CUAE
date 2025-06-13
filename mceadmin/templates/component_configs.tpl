{capture name=button_set}
    <p>
        {if not $_no_update}<input class="submit" type="submit" name="update" value="Apply" />{/if}
        {$buttons}
        <input class="submit" type="submit" name="cancel" value="Done" />
    </p>
{/capture}

<form action="{$SCRIPT_NAME}" method="post">
<input type="hidden" name="id" value="{$id}" />

    {$smarty.capture.button_set}

    <table class="componentConfig">
        <col class="inputLabels" />
        <col class="inputFields" />
        <col class="inputDescriptions" />
        {$additional_configs}
        {foreach from=$_configs item=_config}
        <tr class="{cycle values='oddRow,evenRow'}">
            <th>{$_config.display_name}</th>
            <td>{$_config.fields}</td>
            <td>
                {$_config.description|escape:"html"}
                {if $_config.meta_description neq ''}
                <span class="configRange">{$_config.meta_description}</span>
                {/if}
            </td>
        </tr>
        {foreachelse}
        <tr class="{cycle values='oddRow,evenRow'}">
            <td colspan="3">There are no configuration items</td>
        </tr>
        {/foreach}
    </table>

    {$smarty.capture.button_set}

</form>