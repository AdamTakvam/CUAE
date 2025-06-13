<table style="width: auto">
    <col style="wdith: 15em;" />
    <col />
    <tr>
        <th>Metric</th>
        <th>Currently In Use</th>
    </tr>
    {foreach from=$oids item=data key=oid}
    <tr class="{cycle values='oddRow,evenRow'}">
        <td>{$data.name}</td>
        <td style="text-align:center">{$data.active|string_format:"%u"}</td>
    </tr>
    {/foreach}
</table>

<p>
<form method="post" action="stats_view.php">
    View graphs of 
    <select name="oid">
    {foreach from=$oids item=data key=oid}
		<option value="{$oid}">{$data.name}</option>
    {/foreach}
    </select>
    over intervals up to a year
    <input type="submit" name="submit" value="View" />
</form>
</p>

<p>
<form method="post" action="stats_view.php">
    View graphs of all metrics over the last 
    <select name="range">
        <option value="{$smarty.const.STATS_INTERVAL_HOUR}">hour</option>
        <option value="{$smarty.const.STATS_INTERVAL_6HOURS}">6 hours</option>		
        <option value="{$smarty.const.STATS_INTERVAL_12HOURS}">12 hours</option>
        <option value="{$smarty.const.STATS_INTERVAL_DAY}">day</option>
        <option value="{$smarty.const.STATS_INTERVAL_WEEK}">week</option>
        <option value="{$smarty.const.STATS_INTERVAL_MONTH}">month</option>
        <option value="{$smarty.const.STATS_INTERVAL_YEAR}">year</option>
    </select>
    <input type="submit" name="submit" value="View" />
</form>
</p>
