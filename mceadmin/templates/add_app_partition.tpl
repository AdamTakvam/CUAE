<script language="Javascript">
<!--
sipGroups = new Array(-1{foreach from=$sip_groups item=group_id},{$group_id}{/foreach})
{literal}

function setEarlyMediaForGroup(groupId)
{
    var isSipGroup = false
    for (x = 0; x < sipGroups.length; ++x)
    {
        if (sipGroups[x] == groupId)
        {
            isSipGroup = true;
            break;
        }
    }

    if (isSipGroup)
    {
        document.editPartition.use_early_media[0].checked=true
        document.editPartition.use_early_media[1].disabled=true
    }
    else
    {
        document.editPartition.use_early_media[1].disabled=false
    }
}

{/literal}
-->
</script>

<form action="{$SCRIPT_NAME}" method="post" name="editPartition">

<table>
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="oddRow">
        <th>Name</th>
        <td><input type="text" name="name" value="{$name|escape:"html"}" /></td>
    </tr>
    <tr class="evenRow">
        <th>Description</th>
        <td><textarea name="description">{$description|escape:"html"}</textarea></td>
    </tr>
</table>


<h3>Configuration</h3>

<table class="appPartitionConfigs">
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Enabled</th>
        <td>
            <input type="radio" name="enabled" {if $enabled}checked="checked"{/if} value="1" /> Yes
            <input type="radio" name="enabled" {if not $set_enabled || not $enabled}checked="checked"{/if} value="0" /> No
        </td>
        <td>Indicate whether or not the partition is active</td>
    </tr>
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Reserve Media Early</th>
        <td>
            <input type="radio" name="use_early_media" {if $use_early_media}checked="checked"{/if} value="1" /> Yes
            <input type="radio" name="use_early_media" {if not $set_uem || not $use_early_media}checked="checked"{/if} value="0" /> No
        </td>
        <td>Reserve media ports early to reduce setup time</td>
    </tr>
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Locale</th>
        <td>
            <select name="locale" {if empty($locale_list)}disabled="disabled"{/if}>
                {foreach from=$locale_list item=locale_item}
                <option value="{$locale_item}" {if $locale == $locale_item}selected="selected"{/if}>{$locale_item}</option>
                {/foreach}
            </select>
        </td>
        <td>The default locale for this partition</td>
    </tr>
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Preferred Codec</th>
        <td>
            <select name="preferred_codec">
                {foreach from=$codec_list item=codec}
                <option value="{$codec}" {if $preferred_codec == $codec}selected="selected"{/if}>{$codec}</option>
                {/foreach}
            </select>
        </td>
        <td>Preferred media resource codec</td>
    </tr>
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Call Route Group</th>
        <td>
            <select name="call_route_group" {if not $_dev_mode}onchange="javascript:setEarlyMediaForGroup(this.form.call_route_group.value);return true;"{/if}>
                <option value="0">None</option>
                {section name=x loop=$call_route_groups}
                <option value="{$call_route_groups[x].mce_component_groups_id}" {if $call_route_group == $call_route_groups[x].mce_component_groups_id}selected="selected"{/if}>{$call_route_groups[x].name|escape:"html"}</option>
                {/section}
            </select>
        </td>
        <td>Associate partition with a call route group</td>
    </tr>
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Media Resource Group</th>
        <td>
            <select name="media_resource_group">
                <option value="0">None</option>
                {section name=x loop=$media_resource_groups}
                <option value="{$media_resource_groups[x].mce_component_groups_id}" {if $media_resource_group == $media_resource_groups[x].mce_component_groups_id}selected="selected"{/if}>{$media_resource_groups[x].name|escape:"html"}</option>
                {/section}
            </select>
        </td>
        <td>Associate partition with a media resource group</td>
    </tr>
    {foreach from=$configs item=config}
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>{$config.display_name|escape:"html"}</th>
        <td>{$config.fields}</td>
        <td>
            {$config.description|escape:"html"}
            {if $config.meta_description neq ''}
            <span class="configRange">{$config.meta_description}</span>
            {/if}
        </td>
    </tr>
    {/foreach}
</table>

<input type="hidden" name="id" value="{$id}" />
<input type="hidden" name="alarm_group" value="{$alarm_group}" />
<input class="submit" type="submit" name="create" value="Create Partition" />
<input class="submit" type="submit" name="cancel" value="Cancel" />

</form>
