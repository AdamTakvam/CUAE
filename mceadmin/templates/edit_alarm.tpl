{capture name=buttons}
	<input type="hidden" name="type" value="{$type}" />
	<input class="submit" type="submit" name="uninstall" value="Uninstall {$describe_type}" />
{/capture}
<div class="componentData">
	
{include file="component_metadata.tpl"}

</div>

{include file="component_configs.tpl" buttons=$smarty.capture.buttons}
