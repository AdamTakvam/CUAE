{capture name=buttons}
        <input type="hidden" name="type" value="{$type}" />
        <input class="submit" type="submit" name="uninstall" value="Delete {$type_display}" />
{/capture}

{include file="component_configs.tpl" buttons=$smarty.capture.buttons _no_update=1}