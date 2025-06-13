<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Cisco Unified Application Environment :: License Details for {$file}</title>
    <link rel="stylesheet" href="{$smarty.const.WEB_PATH}/style.css" type="text/css" />
</head>

<body>

<h3>{$file}</h3>

<table>
	<caption>License Data</caption>
	<col style="width: 35%" />
	<col />
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>Hostname</th>
		<td>{$hostname}</td>
	</tr>
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>MAC Address</th>
		<td>{$mac}</td>
	</tr>
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>Feature</th>
		<td>{$feature}</td>
	</tr>	
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>Version</th>
		<td>Up to {$version}</td>
	</tr>	
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>Expiration</th>
		<td>{$expires}</td>
	</tr>	
</table>

<hr />

{foreach from=$resources item=data}
<br />
<table>
	<caption>{$data.name}</caption>
	<col style="width: 35%" />
	<col />
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>Amount Licensed</th>
		<td>{$data.value}</td>
	</tr>
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>Version</th>
		<td>Up to {$data.version}</td>
	</tr>	
	<tr class="{cycle values='evenRow,oddRow'}">
		<th>Expiration</th>
		<td>{$data.expires}</td>
	</tr>	
</table>
{/foreach}

</body>
</html>