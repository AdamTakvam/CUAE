<form action="{$SCRIPT_NAME}" method="post">

    <table>
        <col class="inputLabels" />
        <col class="inputFields" />
        <col class="inputDescription" />
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Username</th>
            <td><input type="text" name="username" value="{$username}" /></td>
            <td>Valid characters are alphabetic, numeric, and the characters <em>@.-_</em> and must start with a letter</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Password</th>
            <td><input type="password" name="password" /></td>
            <td>&nbsp;</td>
        </tr>
        <tr class="{cycle values='rowOne,rowTwo'}">
            <th>Verify Password</th>
            <td><input type="password" name="password2" /></td>
            <td>Re-enter the password to verify it</td>
        </tr>
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
    </table>

    <input type="submit" name="add" value="Add User" />
    <input type="submit" name="done" value="Go Back" />

</form>