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

<p>
    {$description|escape:"html"}
</p>

<h3>Configuration</h3>

<form action="{$SCRIPT_NAME}" method="post" name="editPartition">

<table class="componentConfig">
    <col class="inputLabels" />
    <col class="inputFields" />
    <col class="inputDescription" />
    {if $is_default_partition}
        <input type="hidden" name="enabled" value="1" />
    {else}
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Enabled</th>
        <td>
            <input type="radio" name="enabled" value="1" {if $enabled}checked="checked"{/if} /> Yes
            <input type="radio" name="enabled" value="0" {if not $enabled}checked="checked"{/if} /> No
        </td>
        <td>Indicate whether or not the partition is active</td>
    </tr>
    {/if}
    <tr class="{cycle values='oddRow,evenRow'}">
        <th>Reserve Media Early</th>
        <td>
            <input type="radio" name="use_early_media" {if $use_early_media}checked="checked"{/if} value="1" /> Yes
            <input type="radio" name="use_early_media" {if not $use_early_media}checked="checked"{/if} {if in_array($crg_id, $sip_groups)}disabled="disabled"{/if} value="0" /> No
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
                    <option value="{$call_route_groups[x].mce_component_groups_id}" {if $crg_id==$call_route_groups[x].mce_component_groups_id}selected="selected"{/if}>{$call_route_groups[x].name|escape:"html"}</option>
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
                    <option value="{$media_resource_groups[x].mce_component_groups_id}" {if $mrg_id==$media_resource_groups[x].mce_component_groups_id}selected="selected"{/if} >{$media_resource_groups[x].name|escape:"html"}</option>
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
<input type="hidden" name="alarm_group" value="{$ag_id}" />
<input class="submit" type="submit" name="update" value="Apply Configuration" />
<input class="submit" type="submit" name="cancel" value="Done" />
</form>

<h3>Scripts</h3>

<table>
    <tr>
        <th>Name</th>
        <th>Event Type</th>
        <th>&nbsp;</th>
    </tr>
{foreach from=$scripts item=script}
    <tr class="{cycle values='oddRow,evenRow'}">
        <td>{$script->GetName()|escape:"html"}</td>
        <td>{$script->GetEventType()|escape:"html"}</td>
        <td><a class="button" href="script_trigger_edit.php?script_id={$script->GetId()}&amp;part_id={$id}">Edit Trigger Parameters</a></td>
    </tr>
{/foreach}
</table>