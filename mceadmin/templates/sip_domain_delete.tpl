<p>
Are you <strong>sure</strong> you want to delete the SIP Domain {$sd_data.domain_name|escape:"html"}?
</p>
<form action="{$SCRIPT_NAME}" method="post">
    <input type="hidden" name="id" value="{$id}" />
    <input type="submit" name="delete_yes" value="Yes" />
    <input type="submit" name="delete_no" value="No" />
</form>
