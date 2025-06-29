<div>

<h3>Uncaught Exception</h3>

<table>
	<col />
	<col />
	<tr>
		<th>Error Code</th>
		<td>{$exception->getCode()|escape:"html"}</td>
	</tr>
	<tr>
		<th>Message</th>
		<td>{$exception->getMessage()|escape:"html"|nl2br}</td>
	</tr>	
	<tr>
		<th>Stacktrace</th>
		<td>{$exception->getTraceAsString()|escape:"html"|nl2br}</td>
	</tr>	
	<tr>
		<th>File</th>
		<td>{$exception->getFile()|escape:"html"}</td>
	</tr>
</table>

</div>