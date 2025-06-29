{literal}
<script type="text/javascript">
<!--
function SetAllCheckBoxes(FormName, FieldName, CheckValue)
{
    if(!document.forms[FormName])
        return;
    var objCheckBoxes = document.forms[FormName].elements[FieldName];
    if(!objCheckBoxes)
        return;
    var countCheckBoxes = objCheckBoxes.length;
    if(!countCheckBoxes)
        objCheckBoxes.checked = CheckValue;
    else
        // set the check value for all check boxes
        for(var i = 0; i < countCheckBoxes; i++)
            objCheckBoxes[i].checked = CheckValue;
}
// -->
</script>
{/literal}

<p>
Searched for users with:
{if !$uid_search & !$sn_search & !$gn_search}no filter criteria{/if}
{if $uid_search}<br />Username of <strong>{$uid_search|escape:"html"}</strong>{/if}
{if $sn_search}<br />Surname of <strong>{$sn_search|escape:"html"}</strong>{/if}
{if $gn_search}<br />Given Name of <strong>{$gn_search|escape:"html"}</strong>{/if}
<br />
Found <strong>{$count}</strong> users.  Users <strong>{$first+1}-{$last+1}</strong> displayed.
<br />
<a class="button" href="user_import1.php?as_ldap_servers_id={$as_ldap_servers_id}&step=2">Try Another Search</a>
</p>

<p>
Please select which users you want to import from the search results.
</p>

<form action="{$SCRIPT_NAME}" method="post" name="user_select_form">
<table>
<tr>
    <th>Select</th>
    <th>Username</th>
    <th>Surname</th>
    <th>Given Name</th>
    <th>Email</th>
    <th>Account Code</th>
</tr>

{capture assign="form_buttons"}
<tr style="background-color: #ddd;">
    <td colspan="6">
        <input type="button" value="Select All" onclick="SetAllCheckBoxes('user_select_form','iuser_index[]',true);" />
        <input type="button" value="Select None" onclick="SetAllCheckBoxes('user_select_form','iuser_index[]',false);" />
    </td>
</tr>
{/capture}

{$form_buttons}

{foreach from=$users key=index item=user}
<tr class="{cycle values='rowOne,rowTwo'}">
    <td><input type="checkbox" name="iuser_index[]" value="{$index}" {if $selected[$index]}checked="checked"{/if}/></td>
    <td>{$user.uid[0]|escape:"html"}</td>
    <td>{$user.sn[0]|escape:"html"}</td>
    <td>{$user.givenname[0]|escape:"html"}</td>
    <td>{$user.mail[0]|escape:"html"}</td>
    <td>{$user.account_code[0]|escape:"html"}</td>
</tr>
{/foreach}

{$form_buttons}

<tr>
    <input type="hidden" name="old_first" value="{$first}" />
    <input type="hidden" name="old_last" value="{$last}" />
    <td colspan="3">{if $first > 0}<input type="submit" name="previous" value="Previous Page" />{/if}</td>
    <td colspan="3" style="text-align:right;">{if $last < $count-1}<input type="submit" name="next" value="Next Page" />{/if}</td>
</tr>

</table>

<p>
You may also select a user group into which you will import all of these users:
<br />
<select name="import_group">
    <option></option>
    {foreach from=$groups item=group}
    <option value="{$group.as_user_groups_id}" {if $import_group eq $group.as_user_groups_id}selected="selected"{/if}>{$group.name|escape:"html"}</option>
    {/foreach}
</select>
</p>

    <input type="hidden" name="s_data" value="{$s_data|htmlspecialchars}" />
    <input type="hidden" name="s_selected" value="{$s_selected|htmlspecialchars}" />
    <input type="hidden" name="as_ldap_servers_id" value="{$as_ldap_servers_id}" />
    <input type="hidden" name="uid_search" value="{$uid_search|escape:"html"}" />
    <input type="hidden" name="sn_search" value="{$sn_search|escape:"html"}" />
    <input type="hidden" name="gn_search" value="{$gn_search|escape:"html"}" />
    <input type="hidden" name="p" value="{if $p > 0}{$p}{else}1{/if}" />

<p>
    <input type="submit" name="import" value="Import Selected" />
    <input type="submit" name="import_all" value="Import All" />
    <input type="submit" name="cancel" value="Cancel" />
</p>

</form>