<p>
{if $oid > 0}
	Below are graphs of the recent values of {$set_name} over various intervals.  
	<br />
	To view graphs of all measurable metrics over a set interval, click on the graph for that interval.
{elseif !empty($interval)}
	Below are graphs of all measurable metrics over the {$set_name|lower}.  
	<br />
	To view graphs of a certain metrics over various recent intervals, click on the graph for that metric.
{/if}
</p>

{if $oid > 0}
	<p><strong>Currently In Use</strong>: {$oid_value}</p>
{/if}

{foreach from=$graphs item=graph key=index}
{if $oid > 0}

	<h3>{$graph.name}</h3>
	<a href="stats_view.php?range={$index}" /><img src="/stats/{$graph.filename}" alt="Graph of {$set_name} for {$graph.name}" border="0" /></a>
	
{elseif !empty($interval)}

	<h3>{$graph.name}</h3>
	<p>Currently In Use: {$graph.value}</p>
	<a href="stats_view.php?oid={$index}" /><img src="/stats/{$graph.filename}" alt="Graph of {$graph.name} for {$set_name}" border="0" /></a>

{/if}
{/foreach}
