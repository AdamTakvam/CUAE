{capture name=button_set}
    <p>
        <input class="submit" type="submit" name="update" value="Apply" />
        <input type="hidden" name="type" value="{$type}" />
        <input class="submit" type="submit" name="uninstall" value="Uninstall {$type_display}" />
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
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Name</th>
            <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
            <td></td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Description</th>
            <td><textarea name="description">{$_metadata.description|escape:"html"}</textarea></td>
            <td></td>
        </tr>
        <tr class="rowOne">
            <th>Address</th>
            {* HACK HACK HACK HACK - this should change if any other configs are added to H.323 Gateways *}
            <td>{$_configs[0].fields}</td>
            <td><em style="color:red;">This change will not take effect until the Application Server has been restarted</em></td>
        </tr>
    </table>

    {$smarty.capture.button_set}

</form>