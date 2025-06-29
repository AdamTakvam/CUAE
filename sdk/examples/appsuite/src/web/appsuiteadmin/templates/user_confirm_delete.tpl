<form action="{$SCRIPT_NAME}" method="post">
<p>
Are you <strong>sure</strong> you want to delete the user account {$user.username|escape:"html"} for {$user.first_name|escape:"html"} {$user.last_name|escape:"html"}?
</p>
<input type="hidden" name="id" value="{$id}" />
<input type="submit" name="confirm_yes" value="Yes" /> <input type="submit" name="confirm_no" value="No" />
</form>