<script type="text/javascript">
{literal}
<!--
function make_editable(id)
{
    field1 = "blacklist[" + id + "][type]";
    field2 = "blacklist[" + id + "][number]";
    document.blacklistForm.elements[field1].disabled=false;
    document.blacklistForm.elements[field2].disabled=false;
}

//--> 
{/literal}
</script>

{include file="system_mgmt_nav.tpl" selected="findme"}

<form action="{$SCRIPT_NAME}" method="post">
<h3>Find Me Number Settings</h3>

<table>
    <col class="inputLabels" />
    <col class="inputFields" style="width: 200px" />
    <col class="inputDescription" />
    {foreach from=$configs item=config}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>{$config.name|replace:"_":" "|capitalize|escape:"html"}</th>
        <td><input type="text" name="{$config.name}" value="{$config.value|escape:"html"}" /></td>
        <td>{$config.description|escape:"html"}</td>
    </tr>
    {/foreach}
</table>
<p><input type="submit" name="update" value="Apply" /></p>
</form>
    
<h3>Validation Regular Expressions</h3>

<p>
You may configure one or more regular expressions to use for validating Find Me Numbers entered in by the user.  
Please use the format defined by POSIX 1003.2 for extended regular expressions.
If there are no regular expressions defined, than all entries for Find Me Numbers are considered valid.
</p>

<form action="{$SCRIPT_NAME}" method="post">

{if $valid_regexs}
    <p>
        <select name="del_regex">
        {foreach from=$valid_regexs item=regex key=val}
            <option value="{$val}">{$regex|escape:"html"}</option>
        {/foreach}
        </select>
        <input type="submit" name="delete" value="Delete" />
    </p>
{/if}

<p>
You may test your regular expression against a string before adding it by entering in the test string and pressing the "Test" button.
</p>
<p>
    <label for="add_regex" class="settingLabel">Regular Expression</label><input type="text" name="add_regex" value="{$tested.add_regex|escape:"html"}" /> <input type="submit" name="add" value="Add" />
    <br />
    <label for="add_regex_test" class="settingLabel">Test With String</label><input type="text" name="add_regex_test" value="{$tested.add_regex_test|escape:"html"}" /> <input type="submit" name="test" value="Test" /> 
    {if $tested}
        <span class="response">{$tested.result}</span>
    {/if}
</p>

</form>

<p>    
    <strong>Examples</strong>:
    {literal}
    <ul>
        <li><span class="regex">1234</span> - any string containing "1234"</li>
        <li><span class="regex">^1234$</span> - literal "1234"</li>
        <li><span class="regex">^[0-9]+$</span> - any numerical string</li>
        <li><span class="regex">[0-9]+$</span> - any string ending in a series of numerical digits</li>
        <li><span class="regex">[0-9]+</span> - any string with a series of numerical digits contained in it</li>
        <li><span class="regex">^[+0-9]+$</span> - any string composed of digits and plus signs (+)</li>
        <li><span class="regex">^[0-9]{5}$</span> - any numerical string with five digits</li>
        <li><span class="regex">^[0-9]{3,10}$</span> - any numerical string with three to ten digits</li>
    </ul>
    {/literal}
    For more complex examples, please consult a POSIX 1003.2 regular expression reference.
</p>

<h3>Block List</h3>

<p>
Users will not be able to add Find Me numbers that match any definition in the block list.
Also, any existing Find Me numbers which match a definition on the block list will be disabled.
Only digits and the characters + * # are allowed for numbers in the list.
</p>

<form action="{$_SCRIPT_NAME}" method="post" name="blacklistForm">
    
    {foreach from=$blacklist item=number key=index}
    {html_options name="blacklist[`$index`][type]" options=$match_types selected=$number.type disabled=disabled class="filterListItem"}
    <input type="text" name="blacklist[{$index}][number]" value="{$number.number}" disabled="disabled" class="filterListItem" /> 
    <input type="button" value="Edit" onclick="javascript:make_editable({$index})" />
    <input type="submit" name="delete_blacklist[{$index}]" value="Delete" />
    <br />
    {/foreach}
    {html_options name="add_blacklist_type" options=$match_types}
    <input type="text" name="add_blacklist_number" value="" /> <input type="submit" name="add_blacklist" value="Add" />
    
    <p>
        <input type="submit" name="update_blacklist" value="Apply" />
    </p>
    
</form>