<p>
<form action="{$SCRIPT_NAME}" method="post">
<label class="filterLabel">Sort by</label> 
    <select name="sort_by">
        <option value="">(none)</option>
        {foreach from=$sort item=sort_val key=sort_name}
        <option value="{$sort_val}"{if $sort_val eq $sort_by} selected="selected"{/if}>{$sort_name}</option>
        {/foreach}
    </select>
    <select name="sort_order">
        <option value="ASC"{if $sort_order eq "ASC"} selected="selected"{/if}>ascending</option>
        <option value="DESC"{if $sort_order eq "DESC"} selected="selected"{/if}>descending</option>
    </select>
    <input type="submit" name="sort" value="Sort" />
</form>
</p>

<p>{include file="page_nav.tpl"}</p>

<table>
	<tr>
		<th>Name</th>
		<th>Device</th>
		<th>Directory Number</th>
		<th>Find Me Number</th>
		<th>&nbsp;</th>
	</tr>
{foreach from=$ra_users item=user}
	<tr class="{cycle values='rowOne,rowTwo'}">
		<td>{$user.name|escape:"html"}</td>
		<td>{$user.device|escape:"html"}</td>
		<td>{$user.directory_number|escape:"html"}</td>
		<td>{$user.external_number|escape:"html"}</td>
		<td><a href="/appsuiteadmin/user.php?id={$user.user_id}" class="button">Edit</a></td>	
	</tr>
{/foreach}
</table>