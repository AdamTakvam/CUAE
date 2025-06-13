<form method="post" action="{$SCRIPT_NAME}">
<p>
Are you <strong>sure</strong> you want to delete the user <em>{$username|escape:"html"}</em>?
</p>
<input type="hidden" name="id" value="{$id}" />
<input type="hidden" name="list" value="{$list}" />
<input class="submit" type="submit" name="submit_yes" value="Yes" />
<input class="submit" type="submit" name="submit_no" value="No" />
<form>
