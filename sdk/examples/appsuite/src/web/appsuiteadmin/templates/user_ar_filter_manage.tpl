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

<p>
{if $type eq 1}
A list of Caller IDs which will cause ActiveRelay to ignore Time-of-Day rules and call enabled Find Me numbers.
{else if $type eq 2} 
A list of Caller IDs which will cause ActiveRelay to not dial any Find Me numbers, except the Find Me number configured as the Voice Mail Box.
{/if}
</p>

<p>
Only digits and the characters + * # are allowed for numbers in the list.
</p>

<form action="{$_SCRIPT_NAME}" method="post" name="listform">
    <input type="hidden" name="id" value="{$id}" />
    <input type="hidden" name="type" value="{$type}" />
    <input type="hidden" name="from" value="{$from}" />
    
    {foreach from=$numbers item=number}
    {html_options name="numbers[`$number.as_activerelay_filter_numbers_id`][type]" options=$match_types selected=$number.type disabled=disabled class="filterListItem"}
    <input type="text" name="numbers[{$number.as_activerelay_filter_numbers_id}][number]" value="{$number.number}" disabled="disabled" class="filterListItem" /> 
    <input type="button" value="Edit" onclick="javascript:make_editable({$number.as_activerelay_filter_numbers_id})" />
    <input type="submit" name="delete[{$number.as_activerelay_filter_numbers_id}]" value="Delete" />
    <br />
    {/foreach}
    
    <p>
    {html_options name="add_type" options=$match_types}
    <input type="text" name="add_number" value="" /> <input type="submit" name="add" value="Add" />
    </p>
    
    <p>
        <input type="submit" name="update" value="Apply" />
        <input type="submit" name="go_back" value="Go Back" />
    </p>
    
</form>