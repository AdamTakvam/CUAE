<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Metreos Application Suite Administration :: {$_title|capitalize|escape:"html"}</title>
    <link rel="stylesheet" href="/appsuiteadmin/style.css" type="text/css" />
    {$_head_extra}
</head>

<body>

<div id="box">

<h1>
    <img src="/appsuiteadmin/images/metreos_logo.png" alt="Metreos" width="313" height="68" />
	<br/>
	Remote Agent
</h1>

{if $_navigation}
    <div id="navigation">
    	<a href="/appsuiteadmin/remoteagent/logout.php" class="button">Logout</a>
    </div>

    <div id="breadcrumbs">
    	{$_breadcrumbs}
    </div>
{/if}

{if $_title}
    <h2 id="pagetitle">{$_title|capitalize|escape:"html"}</h2>
{/if}

{if $_response_message}
    <div class="response">{$_response_message|nl2br}</div>
{/if}

{if $_error_message}
    <div class="error">
        The following error(s) occurred:
        <br />
        {$_error_message|nl2br}
    </div>
{/if}

    <div id="main">
    {include file="$_content_template"}
    </div>

</div>

<p id="copyright">Copyright &copy; Metreos Corporation.  All rights reserved.</p>

</body>
</html>