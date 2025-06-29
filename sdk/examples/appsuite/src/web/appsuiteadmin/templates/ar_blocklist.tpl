<script type="text/javascript">
{literal}
<!--
function make_editable(id)
{
    field1 = "numbers[" + id + "][type]";
    field2 = "numbers[" + id + "][number]";
    document.listform.elements[field1].disabled=false;
    document.listform.elements[field2].disabled=false;
}

//--> 
{/literal}
</script>

{include file="system_mgmt_nav.tpl" selected="blocklist"}

<p>
    The ActiveRelay Blacklist defines all inbound calling numbers which ActiveRelay will ignore by not ringing Find Me numbers for all users. 
<p>
<p>
    There are two exceptions to this list. 
</p>
<ul>
    <li> Any Find Me number configured as the Voice Mail Box will still be dialed by ActiveRelay 
    <li> Any Find Me number defined in the user's whitelist has precedence over this list.
</ul>

<p>
Only digits and the characters + * # are allowed for numbers in the list.
</p>

<form action="{$_SCRIPT_NAME}" method="post" name="listform">
    
    {foreach from=$numbers item=number}
    {html_options name="numbers[`$number.as_activerelay_filter_numbers_id`][type]" options=$match_types selected=$number.type disabled=disabled class="filterListItem"}
    <input type="text" name="numbers[{$number.as_activerelay_filter_numbers_id}][number]" value="{$number.number}" disabled="disabled" class="filterListItem" /> 
    <input type="button" value="Edit" onclick="javascript:make_editable({$number.as_activerelay_filter_numbers_id})" />
    <input type="submit" name="delete[{$number.as_activerelay_filter_numbers_id}]" value="Delete" />
    <br />
    {/foreach}
    {html_options name="add_type" options=$match_types}
    <input type="text" name="add_number" value="" /> <input type="submit" name="add" value="Add" />
    
    <p>
        <input type="submit" name="update" value="Apply" />
    </p>
    
</form>