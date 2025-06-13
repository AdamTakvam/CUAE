<form method="post" action="{$SCRIPT_NAME}">
<p>
Are you <strong>sure</strong> you want to uninstall <em>{$component_name|escape:"html"}</em>?
</p>
<input type="hidden" name="type" value="{$type}" />
<input type="hidden" name="id" value="{$id}" />
<input class="submit" type="submit" name="submit_yes" value="Yes" />
<input class="submit" type="submit" name="submit_no" value="No" />
<form>
