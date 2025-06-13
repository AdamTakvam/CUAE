<script type="text/javascript">
<!--
{literal}
var xmlhttp=false;
var getstatus="reboot_status.php";
var finalload="main.php";

function getHTTPObject() {
	  var xmlhttp;
	  /*@cc_on
	  @if (@_jscript_version >= 5)
	    try {
	      xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
	    } catch (e) {
	      try {
	        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	      } catch (E) {
	        xmlhttp = false;
	      }
	    }
	  @else
	  xmlhttp = false;
	  @end @*/
	  if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
	    try {
	      xmlhttp = new XMLHttpRequest();
	    } catch (e) {
	      xmlhttp = false;
	    }
	  }
	  return xmlhttp;
}

function handleHttpResponse()
{
	if (xmlhttp.readyState==4) 
	{
		try
		{
	   		if (xmlhttp.status==200)
	   		{
	   			// Processing code
	   			var xmlDoc = xmlhttp.responseXML;
	   			var status = xmlDoc.getElementsByTagName('status').item(0).firstChild.data;
	   			if (status != '1')
	   			{
                    setTimeout('goHome()',5000);
	   			}
	   		}
	   	}
	   	catch (e)
	   	{
	   	}
	   	setTimeout('detectPage()',2000);
	}
}

function detectPage()
{
    xmlhttp.open("GET", getstatus, true);
    xmlhttp.onreadystatechange = handleHttpResponse
    xmlhttp.send(null);
}

function goHome()
{
    window.location.assign(finalload);
    return;
}

xmlhttp = getHTTPObject();
setTimeout('detectPage()',2000);

{/literal}
-->
</script>

<p>
The management console web server is restarting.  It might require as much as one minute for the web server to successfully restart.  Please be patient.
</p>

<p>
When the web server has successfully restarted, the Management Console will automatically return to the Main Control Panel.
</p>