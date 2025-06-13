<form action="{$SCRIPT_NAME}" method="post">

<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Username</th>
        <td>{$username}</td>
        <td>Name that identifies the user</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>New Password<br /></th>
        <td><input type="password" name="password" /></td>
        <td>If you do not want to change the password, leave this field empty</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Verify New Password</th>
        <td><input type="password" name="password2" /></td>
        <td>If changing the password, re-enter it here to verify it</td>
    </tr>
    {if $id <> 1}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Access Level</th>
        <td>
            <select name="access_level">
            {foreach from=$accesslevels key=value item=description}
                <option value="{$value}" {if $access_level==$value}selected="selected"{/if}>{$description}</option>
            {/foreach}
            </select>
        </td>
        <td>Permission level for the user</td>
    </tr>
    {/if}
</table>
    <input type="hidden" name="current_access_level" value="{$access_level}" />
    <input type="hidden" name="id" value="{$id}" />

    <input type="submit" name="update" value="Apply" class="submit" {if !$can_modify}disabled="disabled"{/if} />
    <input type="submit" name="delete" value="Delete User" class="submit" {if !$can_modify || $id==$user_id}disabled="disabled"{/if} />
    <input type="submit" name="done" value="Go Back" />

</form>