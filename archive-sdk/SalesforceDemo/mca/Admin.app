<Application name="Admin" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Admin">
    <outline>
      <treenode type="evh" id="633004250508109179" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633004250508109176" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633004250508109175" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/Admin</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633066398551719577" level="2" text="Metreos.Providers.Http.GotRequest: ProcessLogin">
        <node type="function" name="ProcessLogin" id="633066398551719574" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633066398551719573" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="633068211791407089" level="2" text="Metreos.Providers.Http.GotRequest: GetState">
        <node type="function" name="GetState" id="633068211791407086" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633068211791407085" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/GetState</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633071510444337664" level="2" text="Metreos.Providers.Http.GotRequest: Whisper">
        <node type="function" name="Whisper" id="633071510444337661" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633071510444337660" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/Whisper</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="633071665896166472" level="2" text="Metreos.Providers.Http.GotRequest: SendUniMessage">
        <node type="function" name="SendUniMessage" id="633071665896166469" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633071665896166468" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/SendUnicastMessage</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="633066398551719437" level="1" text="ComposeGrid">
        <node type="function" name="ComposeGrid" id="633066398551719434" path="Metreos.StockTools" />
        <calls>
          <ref actid="633068211791407092" />
        </calls>
      </treenode>
      <treenode type="fun" id="633066398551719552" level="1" text="PromptLogin">
        <node type="function" name="PromptLogin" id="633066398551719549" path="Metreos.StockTools" />
        <calls>
          <ref actid="633066398551719548" />
        </calls>
      </treenode>
      <treenode type="fun" id="633066398551719557" level="1" text="GetClientType">
        <node type="function" name="GetClientType" id="633066398551719554" path="Metreos.StockTools" />
        <calls>
          <ref actid="633068211791407090" />
          <ref actid="633066398551719553" />
        </calls>
      </treenode>
      <treenode type="fun" id="633071665896165386" level="1" text="ClearCurrentOperation">
        <node type="function" name="ClearCurrentOperation" id="633071665896165383" path="Metreos.StockTools" />
        <calls>
          <ref actid="633071665896165382" />
          <ref actid="633071665896166521" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_activeCalls" id="633086135845415628" vid="633066398551719438">
        <Properties type="DataTable">g_activeCalls</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="633086135845415630" vid="633066398551719567">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_host" id="633086135845415632" vid="633066398551719580">
        <Properties type="String">g_host</Properties>
      </treenode>
      <treenode text="g_users" id="633086135845415634" vid="633066398551719583">
        <Properties type="Hashtable" initWith="Users">g_users</Properties>
      </treenode>
      <treenode text="g_userDevicename" id="633086135845415636" vid="633066398551719645">
        <Properties type="String">g_userDevicename</Properties>
      </treenode>
      <treenode text="g_username" id="633086135845415638" vid="633066398551719647">
        <Properties type="String">g_username</Properties>
      </treenode>
      <treenode text="g_phoneUser" id="633086135845415640" vid="633071510444337705">
        <Properties type="String" initWith="PhoneUsername">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="633086135845415642" vid="633071510444337707">
        <Properties type="String" initWith="PhonePassword">g_phonePass</Properties>
      </treenode>
      <treenode text="g_currentOperation" id="633086135845415644" vid="633071665896165380">
        <Properties type="String" defaultInitWith="NONE">g_currentOperation</Properties>
      </treenode>
      <treenode text="g_currentOperationCallId" id="633086135845415646" vid="633071665896165404">
        <Properties type="String">g_currentOperationCallId</Properties>
      </treenode>
      <treenode text="g_currentOperationIp" id="633086135845415648" vid="633071665896165407">
        <Properties type="String">g_currentOperationIp</Properties>
      </treenode>
      <treenode text="g_customerInfos" id="633086135845415650" vid="633082104606024667">
        <Properties type="Hashtable">g_customerInfos</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="633004250508109178" treenode="633004250508109179" appnode="633004250508109176" handlerfor="633004250508109175">
    <node type="Start" id="633004250508109178" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="168" y="448">
      <linkto id="633066398551719569" type="Basic" style="Vector" />
    </node>
    <node type="Comment" id="633066398551719432" text="&#xD;&#xA;CREATE TABLE activecalls&#xD;&#xA;(&#xD;&#xA;  id INT unsigned NOT NULL auto_increment,&#xD;&#xA;  devicename VARCHAR(25),&#xD;&#xA;  to_number VARCHAR(25) NOT NULL DEFAULT '',&#xD;&#xA;  from_number VARCHAR(25) NOT NULL DEFAULT '',&#xD;&#xA;	active TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',&#xD;&#xA;  direction tinyint(1) unsigned NOT NULL default '0', /* 0=inbound, 1=outbound */&#xD;&#xA;	state tinyint(1) unsigned NOT NULL default '0', /* 0=hold, 1=active */&#xD;&#xA;	PRIMARY KEY(id)&#xD;&#xA;);" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="225" y="42" />
    <node type="Action" id="633066398551719548" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="359.942383" y="429" mx="398" my="445">
      <items count="1">
        <item text="PromptLogin" />
      </items>
      <linkto id="633068211791407102" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="phoneModelHeader" type="variable">phoneModelHeader</ap>
        <ap name="FunctionName" type="literal">PromptLogin</ap>
      </Properties>
    </node>
    <node type="Action" id="633066398551719569" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="262" y="448">
      <linkto id="633066398551719548" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">routingGuid</ap>
        <ap name="Value2" type="variable">host</ap>
        <ap name="Value3" type="literal">NONE</ap>
        <rd field="ResultData">g_routingGuid</rd>
        <rd field="ResultData2">g_host</rd>
        <rd field="ResultData3">g_currentOperation</rd>
      </Properties>
    </node>
    <node type="Action" id="633068211791407102" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="583" y="446">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633004250508109180" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="633004250508109181" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="633004250508109184" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="633010212573437960" name="phoneModelHeader" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="x-CiscoIPPhoneModelName" defaultInitWith="NONE" refType="reference">phoneModelHeader</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ProcessLogin" activetab="true" startnode="633066398551719576" treenode="633066398551719577" appnode="633066398551719574" handlerfor="633066398551719573">
    <node type="Start" id="633066398551719576" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="104" y="456">
      <linkto id="633066398551719649" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633066398551719649" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="280" y="456">
      <linkto id="633066398551719652" type="Labeled" style="Vector" label="failure" />
      <linkto id="633068211791407099" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.FormCollection form, ref string g_userDevicename, ref string g_username, Hashtable g_users)
{
	string submittedUsername = form["username"];
	string submittedPassword = form["password"];

	if(submittedUsername != null)
	{
		if(g_users.Contains(submittedUsername))
		{
			g_userDevicename = g_users[submittedUsername] as string;
			g_username = submittedUsername;
		}
	}

	return g_users.Contains(submittedUsername) ? "success" : "failure";
}
</Properties>
    </node>
    <node type="Comment" id="633066398551719651" text="Authenticate user, lookup&#xD;&#xA;username in user&lt;&gt;device table" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="192" y="368" />
    <node type="Action" id="633066398551719652" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="448" y="600">
      <linkto id="633066398551719653" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Location" type="csharp">"http://" + g_host + "/SalesforceDemo/Admin"</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">303</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="633066398551719653" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="600">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633068211791407099" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="439" y="265">
      <linkto id="633068643791883318" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref string body, string g_routingGuid)
{
	body = 
@"&lt;!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""&gt;
&lt;html xmlns='http://www.w3.org/1999/xhtml'&gt;
	&lt;head&gt;
&lt;script type='text/javascript'&gt;

var username;
var password;
var phoneIP;
var appServerIP;
var number;
var req = false;
var state = 'dialing';

var routingGuid = '" + g_routingGuid + @"';
var req;
var currentRowOver = '';
var exDom;

var pixelsBetweenRows = 20;

var whisperButtonType = 'whisperbutton';
var listenButtonType = 'listenbutton';
var bargeButtonType = 'bargebutton';
var messageButtonType = 'messagebutton';
var broadcastButtonType = 'broadcastbutton';

var rowoutTimers = {};
var rowoutTimerMs = 1000;

var testing = false;

var successActionMessage = 'Command successfully executed';

// setTimeout delegates
function rowoutTimeoutDelegate(method, row, functionbar, userId)
{
	return (function()
		{
			method(row, functionbar, userId);
		});
}

var BrowserDetect = {
	init: function () {
		this.browser = this.searchString(this.dataBrowser) || 'An unknown browser';
		this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| 'an unknown version';
		this.OS = this.searchString(this.dataOS) || 'an unknown OS';
	},
	searchString: function (data) {
		for (var i=0;i&lt;data.length;i++)	{
			var dataString = data[i].string;
			var dataProp = data[i].prop;
			this.versionSearchString = data[i].versionSearch || data[i].identity;
			if (dataString) {
				if (dataString.indexOf(data[i].subString) != -1)
					return data[i].identity;
			}
			else if (dataProp)
				return data[i].identity;
		}p
	},
	searchVersion: function (dataString) {
		var index = dataString.indexOf(this.versionSearchString);
		if (index == -1) return;
		return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
	},
	dataBrowser: [
		{ 	string: navigator.userAgent,
			subString: 'OmniWeb',
			versionSearch: 'OmniWeb/',
			identity: 'OmniWeb'
		},
		{
			string: navigator.vendor,
			subString: 'Apple',
			identity: 'Safari'
		},
		{
			prop: window.opera,
			identity: 'Opera'
		},
		{
			string: navigator.vendor,
			subString: 'iCab',
			identity: 'iCab'
		},
		{
			string: navigator.vendor,
			subString: 'KDE',
			identity: 'Konqueror'
		},
		{
			string: navigator.userAgent,
			subString: 'Firefox',
			identity: 'Firefox'
		},
		{
			string: navigator.vendor,
			subString: 'Camino',
			identity: 'Camino'
		},
		{		// for newer Netscapes (6+)
			string: navigator.userAgent,
			subString: 'Netscape',
			identity: 'Netscape'
		},
		{
			string: navigator.userAgent,
			subString: 'MSIE',
			identity: 'Explorer',
			versionSearch: 'MSIE'
		},
		{
			string: navigator.userAgent,
			subString: 'Gecko',
			identity: 'Mozilla',
			versionSearch: 'rv'
		},
		{ 		// for older Netscapes (4-)
			string: navigator.userAgent,
			subString: 'Mozilla',
			identity: 'Netscape',
			versionSearch: 'Mozilla'
		}
	],
	dataOS : [
		{
			string: navigator.platform,
			subString: 'Win',
			identity: 'Windows'
		},
		{
			string: navigator.platform,
			subString: 'Mac',
			identity: 'Mac'
		},
		{
			string: navigator.platform,
			subString: 'Linux',
			identity: 'Linux'
		}
	]

};
BrowserDetect.init();


function attachEventListener(target, eventType, functionRef,
   capture)
{
 	if (typeof target.addEventListener != 'undefined')
 	{
   		target.addEventListener(eventType, functionRef, capture);
 	}
	 else if (typeof target.attachEvent != 'undefined')
 	{
   		target.attachEvent('on' + eventType, functionRef);
 	}
 	else
 	{
   		eventType = 'on' + eventType;

	   	if (typeof target[eventType] == 'function')
   		{
     			var oldListener = target[eventType];

	     		target[eventType] = function()
     			{
       				oldListener();

       				return functionRef();
     			};
   		}
   		else
   		{
     			target[eventType] = functionRef;
   		}
 	}
}


function retrieveUpdate() {
    // branch for native XMLHttpRequest object
    if(window.XMLHttpRequest &amp;&amp; !(window.ActiveXObject)) {
    	try {
			req = new XMLHttpRequest();
        } catch(e) {
			req = false;
        }
    // branch for IE/Windows ActiveX version
    } else if(window.ActiveXObject) {
       	try {
        	req = new ActiveXObject('Msxml2.XMLHTTP');
      	} catch(e) {
        	try {
          		req = new ActiveXObject('Microsoft.XMLHTTP');
        	} catch(e) {
          		req = false;
        	}
		}
    }
	if(req) {
		req.onreadystatechange = processUpdateChange;
		var link = 'http://' + document.location.host + '/SalesforceDemo/GetState?metreosSessionId=' + routingGuid;
		req.open('GET', link, true);
		req.send('');
	}
}


function callActionRequest(link, text) {
    // branch for native XMLHttpRequest object
    if(window.XMLHttpRequest &amp;&amp; !(window.ActiveXObject)) {
    	try {
			req = new XMLHttpRequest();
        } catch(e) {
			req = false;
        }
    // branch for IE/Windows ActiveX version
    } else if(window.ActiveXObject) {
       	try {
        	req = new ActiveXObject('Msxml2.XMLHTTP');
      	} catch(e) {
        	try {
          		req = new ActiveXObject('Microsoft.XMLHTTP');
        	} catch(e) {
          		req = false;
        	}
		}
    }
	if(req) {
		req.onreadystatechange = processActionChange;
		req.open('GET', link, true);
		var content = '';
		if(text)
		{
			req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');	
			content = 'text=' + text;
			alert(content);
		}
		req.send(content);
	}
}




function load()
{
	if(!testing)
	{
		retrieveUpdate();
	}
	else
	{
		syncTable(getExampleDom());
		
				// show buttons
		var testDiv = document.createElement('div');
		testDiv.style.width = '150px';
		testDiv.style.height = '300px';
		testDiv.style.top = '500px';
		testDiv.style.left = '800px';
		testDiv.style.visibility = 'visible';
		testDiv.style.position = 'absolute';
		
		var adder = document.createElement('div');
		var adderText = document.createTextNode('Add');
		adder.appendChild(adderText);
		attachEventListener(adder, 'click', add, true);

		testDiv.appendChild(adder);
		
		var remover = document.createElement('div');
		var removerText = document.createTextNode('Remove');
		remover.appendChild(removerText);
		attachEventListener(remover, 'click', remove, true);

		
		testDiv.appendChild(remover);
		
		var body = document.getElementsByTagName('body')[0];

		body.appendChild(testDiv);
	}
}


function getEventTarget(event, idSubstring)
{
 	var targetElement = null;

 	if (typeof event.target != 'undefined')
 	{
   		targetElement = event.target;
 	}
 	else
 	{
   		targetElement = event.srcElement;
 	}

 	while (targetElement.nodeType == 3 &amp;&amp; targetElement.parentNode != null)
 	{
   		targetElement = targetElement.parentNode;
	}


	while(true)
	{

		var found = false;
		var idAttr = targetElement.getAttribute('id');
		if(idAttr)
		{
			found = idAttr.indexOf(idSubstring) &gt; -1;
			
		}

		if(!found)
		{
			targetElement = targetElement.parentNode;
		}
		else
		{
			break;
		}
	
 	} 


 	return targetElement;
}



function processUpdateChange()
{
	if(req.readyState == 4)
	{
		if(req.status == 200)
		{
			if(req.responseXML)
			{
				var dom = req.responseXML;
				syncTable(dom);
			}
		}
		else
		{
			alert(req.status);
		}

		setTimeout('retrieveUpdate()', 4000);
	}
}


function processActionChange()
{

	if(req.readyState == 4)
	{
		if(req.status == 200)
		{
			if(req.responseXML)
			{
				var dom = req.responseXML;
				
				processActionResponse(dom);
			}
		}
		else
		{
			alert(req.status);
		}
	}
}


function processActionResponse(dom)
{
	var dataNode = dom.lastChild;
	var resultNode = dataNode.childNodes.length == 0 ? false : dataNode.childNodes[0];

	var responseType = resultNode.getAttribute('type');
	var userId = resultNode.getAttribute('id');

	if(responseType == whisperButtonType)
	{
		var code = resultNode.getAttribute('code');
		var errorMessage = resultNode.childNodes.length == 1 ? resultNode.childNodes[0].nodeValue : '';
		processWhisperResponse(userId, code, errorMessage);
	}
	else if(responseType == listenButtonType)
	{
		var code = resultNode.getAttribute('code');
		var errorMessage = resultNode.childNodes.length == 1 ? resultNode.childNodes[0].nodeValue : '';
		processWhisperResponse(userId, code, errorMessage);
	}
	else if(responseType == bargeButtonType)
	{
		var code = resultNode.getAttribute('code');
		var errorMessage = resultNode.childNodes.length == 1 ? resultNode.childNodes[0].nodeValue : '';
		processWhisperResponse(userId, code, errorMessage);
	}
	else if(responseType == messageButtonType)
	{
		var code = resultNode.getAttribute('code');
		var errorMessage = resultNode.childNodes.length == 1 ? resultNode.childNodes[0].nodeValue : '';
		processMessageResponse(userId, code, errorMessage);
	}
}


function processWhisperResponse(userId, code, errorMessage)
{
	var responsebox =document.getElementById('responsebox');
	var responseboxtext = document.getElementById('responseboxtext');

	if(code == '0') // success
	{
		responsebox.style.visibility = 'visible';
		if(responseboxtext.childNodes.length == 1)
		{
			responseboxtext.childNodes[0].nodeValue = successActionMessage;
		}
		else
		{
			var textNode = document.createTextNode(successActionMessage);
			responseboxtext.appendChild(textNode);
		}
		
		responseboxtext.nodeValue = successActionMessage;
	}
	else
	{
		// error!
		responsebox.style.visibility = 'visible';
		if(responseboxtext.childNodes.length == 1)
		{
			responseboxtext.childNodes[0].nodeValue = errorMessage;
		}
		else
		{
			var textNode = document.createTextNode(errorMessage);
			responseboxtext.appendChild(textNode);
		}

	}
}


function processMessageResponse(userId, code, errorMessage)
{
	var responsebox =document.getElementById('responsebox');
	var responseboxtext = document.getElementById('responseboxtext');

	if(code == '0') // success
	{
		responsebox.style.visibility = 'visible';
		if(responseboxtext.childNodes.length == 1)
		{
			responseboxtext.childNodes[0].nodeValue = successActionMessage;
		}
		else
		{
			var textNode = document.createTextNode(successActionMessage);
			responseboxtext.appendChild(textNode);
		}

		
		responseboxtext.nodeValue = successActionMessage;
	}
	else
	{
		// error!
		responsebox.style.visibility = 'visible';
		if(responseboxtext.childNodes.length == 1)
		{
			responseboxtext.childNodes[0].nodeValue = errorMessage;
		}
		else
		{
			var textNode = document.createTextNode(errorMessage);
			responseboxtext.appendChild(textNode);
		}

	}
}

function getExampleDom()
{
	exampleDom = '&lt;data&gt;&lt;allusers&gt;';
	exampleDom = exampleDom + ""&lt;user id='1' status='active' username='User1' direction='inbound' customer='Dave Jones' customerNumber='512-555-1234' time='00:01:15' misc='http://' + document.location.host + '/SalesforceDemo/Misc' /&gt;"";

	exampleDom = exampleDom + '&lt;/allusers&gt;&lt;/data&gt;'; 

	// code for IE
	if (window.ActiveXObject)
  	{
  		var doc=new ActiveXObject('Microsoft.XMLDOM');
  		doc.async='false';
  		doc.loadXML(exampleDom);
  	}
	// code for Mozilla, Firefox, Opera, etc.
	else
  	{
  		var parser=new DOMParser();
  		var doc=parser.parseFromString(exampleDom,'text/xml');
  	}

	// documentElement always represents the root node
	var x=doc.documentElement;

	return x;
}


function syncTable(dom)
{
	var dataNode = dom.lastChild;
	var allUsersNode = dataNode.childNodes.length == 0 ? false : dataNode.childNodes[0];

	// testing logic with FF -- can be removed 
	if(allUsersNode.nodeName == 'user') allUsersNode = allUsersNode.parentNode;

	if(allUsersNode)
	{
		var table = document.getElementById('activecalls');
		
		var rowsFound = new Array();

		for(i = 0; i &lt; allUsersNode.childNodes.length; i++)
		{
			if(allUsersNode.childNodes[i].nodeType == 1)
			{
			var userElement = allUsersNode.childNodes[i];
			var userId = userElement.getAttribute('id');
			var callStatus = userElement.getAttribute('status');
			var userName = userElement.getAttribute('username');
			var direction = userElement.getAttribute('direction');
			var customerName = userElement.getAttribute('customer');
			var customerNumber = userElement.getAttribute('customerNumber');
			var duration = userElement.getAttribute('time');
			var misc = userElement.getAttribute('misc');

			if(userId &amp;&amp; userId != '')
			{
				rowsFound[rowsFound.length] = userId;

				var statusText = '?';
				var usernameText = '?';
				var directionalText = 'called';
				var customerNameText = '?';
				var customerPhoneText = '?';
				var durationText = '?';
				var miscText = '?';
			
				// read in only set values

				if(callStatus == 'active')
				{
					statusText = 'ON';
				}
				else if(callStatus == 'inactive')
				{
					statusText = 'HOLD';
				}

				if(userName &amp;&amp; userName != '')
				{
					usernameText = userName;
				}

				if(direction == 'inbound')
				{
					directionalText = 'called by';
				}
				else if(direction == 'outbound')
				{
					directionalText = 'called'
				}

				if(customerName &amp;&amp; customerName != '')
				{
					customerNameText = customerName;
				}

				if(customerNumber &amp;&amp; customerNumber != '')
				{
					customerPhoneText = customerNumber;
				}

				if(duration &amp;&amp; duration != '')
				{
					durationText = duration;
				}

				if(misc &amp;&amp; misc != '')
				{
					miscText = misc;
				}

				// durationText not dealt with

				// find if this row already exists
				var matchingRow = document.getElementById('user' + userId);

				if(matchingRow)
				{
					SyncValues(table, matchingRow, statusText, usernameText, directionalText, customerNameText, customerPhoneText, durationText, miscText);
				}
				else
				{
					 AddRow(table, userId, statusText, usernameText, directionalText, customerNameText, customerPhoneText, durationText, miscText);
				}
			}
			}
		}


		// clean up any completely gone calls
		var rows = table.childNodes;

		var toDelete = new Array();

		if(rows &amp;&amp; rows.length &gt; 0)
		{
			for(i = 0; i &lt; rows.length; i++)
			{
				if(rows[i].nodeType == 1)
				{
				var userId = rows[i].getAttribute('id');

				if(userId != 'activecallstitle') // kindly skip the header div
				{
				var found = false;
				for(j = 0; j &lt; rowsFound.length; j++)
				{
					var rowUserId = 'user' + rowsFound[j];

					if(rowUserId == userId)
					{
						found = true;
						break;
					}
				}
	
				if(!found)
				{
					toDelete[toDelete.length] = rows[i];
				}
				}
				}
			}
		}

		for(i = 0; i &lt; toDelete.length; i++)
		{
			RemoveRow(toDelete[i]);
		}
	}
}

function BuildCallElement(statusText, usernameText, directionalText, customerNameText, customerPhoneText)
{
	var callTextWrapper = document.createElement('span');
	var userNameWrapper = document.createElement('span');
	var customerNameWrapper = document.createElement('span');
	var calledByWrapper = document.createElement('span');

	var userNameWrapperText = document.createTextNode(usernameText);
	var customerNameWrapperText = document.createTextNode(customerNameText);
	var calledByWrapperText = document.createTextNode(' ' + directionalText + ' ');

	userNameWrapper.appendChild(userNameWrapperText);
	customerNameWrapper.appendChild(customerNameWrapperText);
	calledByWrapper.appendChild(calledByWrapperText);

	callTextWrapper.appendChild(userNameWrapper);
	callTextWrapper.appendChild(calledByWrapper);
	callTextWrapper.appendChild(customerNameWrapper);
	

	callTextWrapper.className = 'calltext';
	userNameWrapper.className = 'username';
	customerNameWrapper.className = 'customername';
	calledByWrapper.className = 'directional';

	return callTextWrapper;
}

function BuildDurationElement(durationText, userId)
{
	var durationWrapper = document.createElement('span');
	var durationWrapperText = document.createTextNode(BuildDurationString(durationText));
	durationWrapper.appendChild(durationWrapperText);
	durationWrapper.className = 'duration';
	durationWrapper.setAttribute('id', 'duration' + userId);
	return durationWrapper;
}


function BuildDurationString(durationText)
{
	return ' (' + durationText + ') ';
}

function findPos(obj)
{
	var curleft = curtop = 0;
	if (obj.offsetParent) {
		curleft = obj.offsetLeft
		curtop = obj.offsetTop
		while (obj = obj.offsetParent)
	       	{
			curleft += obj.offsetLeft
			curtop += obj.offsetTop
		}
	}
	return [curleft,curtop];
}

function checkMouseEnter (element, evt)
{
	if (element.contains &amp;&amp; evt.fromElement) 
	{
		return !element.contains(evt.fromElement);
	}
	else if (evt.relatedTarget)
       	{
		return !containsDOM(element, evt.relatedTarget);
	}
}


function checkMouseLeave (element, evt)
{
	if (element.contains &amp;&amp; evt.toElement)
       	{
		return !element.contains(evt.toElement);
	}
	else if (evt.relatedTarget) 
	{
		return !containsDOM(element, evt.relatedTarget);
	}
}



function containsDOM (container, containee)
{
	var isParent = false;
	do 
	{
		if ((isParent = container == containee))
		break;
		containee = containee.parentNode;
	}
	while (containee != null);
	
	return isParent;
}




function rowmouseover(evt)
{
	var row = getEventTarget(evt, 'user');

	if(checkMouseEnter(row, evt))
	{
		var userId = row.userId;

		row.style.backgroundColor = 'yellow';
		row.style.borderWidth = '2px 0 2px 2px';

		var functionBar = document.getElementById('functionbar' + userId);
		if(functionBar)
		{
			functionBar.style.visibility = 'visible';
			functionBar.style.backgroundColor = 'yellow';
		}

		if(rowoutTimers[userId])
		{
			clearTimeout(rowoutTimers[userId]);
			delete rowoutTimers[userId];
		}
	}
}

function rowmouseout(evt)
{
	var row = getEventTarget(evt, 'user');
	
	if(checkMouseLeave(row, evt))
	{
		var userId = row.userId;

		row.style.backgroundColor = '#b0b0b0';
	
		var functionBar = document.getElementById('functionbar' + userId);
		functionBar.style.backgroundColor = '#b0b0b0';

		// and what should be delayed 
		rowoutTimers[userId] = setTimeout(rowoutTimeoutDelegate(mouseoutTimoutHandler, row, functionBar, userId), rowoutTimerMs);
	}
}


function functionbarmouseover(evt)
{
	var functionbar = getEventTarget(evt, 'functionbar')
		
	if(checkMouseEnter(functionbar, evt))
	{
		var userId = functionbar.userId;

		functionbar.style.visibility = 'visible';
		functionbar.style.backgroundColor = 'yellow';

		var rowbar = document.getElementById('user' + userId);
		rowbar.style.backgroundColor = 'yellow';
		rowbar.style.borderWidth = '2px 0 2px 2px';

		if(rowoutTimers[userId])
		{
			clearTimeout(rowoutTimers[userId]);
			delete rowoutTimers[userId];
		}
	}
}


function functionbarmouseout(evt)
{
	var functionbar = getEventTarget(evt, 'functionbar');
	
	if(checkMouseLeave(functionbar, evt))
	{
		var userId = functionbar.userId;

		// process immediate execute
	
		var rowbar = document.getElementById('user' + userId);
		rowbar.style.backgroundColor = '#b0b0b0';
		functionbar.style.backgroundColor = '#b0b0b0';
	

		// and what should be delayed
		rowoutTimers[userId] = setTimeout(rowoutTimeoutDelegate(mouseoutTimoutHandler, rowbar, functionbar, userId), rowoutTimerMs);
		
	}
}


function mouseoutTimoutHandler(row, functionBar, userId)
{
	row.style.borderWidth = '2px';
	functionBar.style.visibility = 'hidden';

	// remove timer
	delete rowoutTimers[userId];
}




function findLastRow(rowHolder)
{
	for(i = rowHolder.childNodes.length; i &gt; 0; i--)
	{
		if(rowHolder.childNodes[i - 1].nodeType == 1)
		{
			return rowHolder.childNodes[i - 1];
		}
	}

	return null;
}





function AddRow(rowHolder, userId, statusText, usernameText, directionalText, customerNameText, customerPhoneText, durationTextValue, miscTextValue)
{
	// get rowheader
	var newRow = document.createElement('div');
	newRow.className = 'normalrow';
	newRow.setAttribute('id', 'user' + userId);

	// figure out top pixel position if not first row
	if(rowHolder.getElementsByTagName('div').length &gt; 1) // don't forget that there is the title div
	{
		var lastRow = findLastRow(rowHolder);

		var rowHeight = lastRow.offsetHeight;
		var topLeftPos = findPos(lastRow);
		var rowBottom = rowHeight + topLeftPos[1]; 

		var newRowTop = rowBottom + pixelsBetweenRows - 230; // offset for css stlye on activecalls div... this could certainly be more intelligent. findPos finds absolute pos

		newRow.style.top = newRowTop + 'px';	
	}

	var call = document.createElement('div');
	call.className = 'call';
	call.appendChild(BuildCallElement(statusText, usernameText, directionalText, customerNameText, customerPhoneText));
	call.appendChild(BuildDurationElement(durationTextValue, userId));
	newRow.appendChild(call);

	// add onmouseout, onmountover
	newRow.userId = userId;

	attachEventListener(newRow, 'mouseover', rowmouseover, true);
	attachEventListener(newRow, 'mouseout', rowmouseout, true);

	rowHolder.appendChild(newRow);

	// after adding the row, build function bar

	var functionBar = document.createElement('div');
	functionBar.className = 'functionbar';
	functionBar.setAttribute('id', 'functionbar' + userId);
	functionBar.userId = userId;
	attachEventListener(functionBar, 'mouseover', functionbarmouseover, false);
	attachEventListener(functionBar, 'mouseout', functionbarmouseout, false);



	var whisperButton = buildButton(whisperButtonType, 'Whisper', userId);
	var listenButton = buildButton(listenButtonType, 'Listen', userId);
	var bargeButton = buildButton(bargeButtonType, 'Barge', userId);
	var messageButton = buildButton(messageButtonType, 'Message', userId);

	functionBar.appendChild(whisperButton);
	functionBar.appendChild(listenButton);
	functionBar.appendChild(bargeButton);
	functionBar.appendChild(messageButton);

	// get right-top position, and height of the newRow, and put this functionBar to the right
	var rowHeight = newRow.offsetHeight;
	var topLeftPos = findPos(newRow);
	var rowRight = topLeftPos[0] + newRow.offsetWidth;

	var body = document.getElementsByTagName('body')[0];
	functionBar.style.position = 'absolute';
	functionBar.style.visibility = 'hidden';
	functionBar.style.left = (rowRight - 2) + 'px';  // -2 makes up for the border we wil collapse on mouseover/mouseout events
	functionBar.style.top = topLeftPos[1] + 'px' ;
	functionBar.style.height = (rowHeight - 4) + 'px'; // if you change CSS of functionbar border width, padding, or margin, must compensate here
	body.appendChild(functionBar);
}

function callactionclick(evt)
{
	var callActionButton = getEventTarget(evt, 'callaction');

	if (!evt) evt = window.event;
	var relTarg = evt.relatedTarget || evt.fromElement;

	if(relTarg != callActionButton)
	{
		var buttonType = callActionButton.buttonType;
		
		makeActionRequest(buttonType, callActionButton);
	}
}

function makeActionRequest(buttonType, button)
{
	var link;
	var content;
	if(buttonType == whisperButtonType)
	{
		link = 'http://' + document.location.host + '/SalesforceDemo/Whisper?metreosSessionId=' + routingGuid + '&amp;type=' + buttonType + '&amp;id=' + button.userId;
	}	
	else if(buttonType == listenButtonType)
	{
	}
	else if(buttonType == bargeButtonType)
	{
	}
	else if(buttonType == messageButtonType)
	{
	    // show edit box
	    
		var editbox = document.getElementById('editbox');
		editbox.userId = button.userId;
		editbox.actionType = messageButtonType;
		editbox.style.visibility = 'visible';
		
		var pos = findPos(button);
		editbox.style.top = pos[1] + 'px';
		editbox.style.left = pos[0] + 'px';
		editbox.style.zIndex = '100';
		
		// we don't set link -- this operation is a two stage operation--first show form, then submit
	}
	else if( buttonType == broadcastButtonType)
	{
		// show edit box
		var editbox = document.getElementById('editbox');
		editbox.userId = button.userId;
		editbox.actionType = broadcastButtonType;
		editbox.style.visibility = 'visible';
		
		var pos = findPos(button);
		editbox.style.top = pos[1] + 'px';
		editbox.style.left = pos[0] + 'px';
		editbox.style.zIndex = '100';
		
		// we don't set link -- this operation is a two stage operation--first show form, then submit
	}
	if(link)
	{
		callActionRequest(link, content);
	}
}

function messageSubmit(sender)
{
    var editbox = document.getElementById('editbox');
    var messagetext = document.getElementById('messagetext');
		
	var messageType = editbox.actionType;
    var link = 'http://' + document.location.host + '/SalesforceDemo/SendUnicastMessage?metreosSessionId=' + routingGuid + '&amp;type=' + messageType + '&amp;id=' + editbox.userId;
    var content = messagetext.value;
    callActionRequest(link, content);
    
    editbox.style.visibility = 'hidden';
}

function messageCancel(sender)
{
    var editbox = document.getElementById('editbox');
	
	editbox.style.visibility = 'hidden';
}



function buildButton(classname, buttontext, userId)
{
	var button = document.createElement('span');
	button.setAttribute('id', 'callaction' + buttontext);
	button.buttonType = classname;
	button.userId = userId;
	button.className = classname;
	var buttonText = document.createTextNode(buttontext);
	button.appendChild(buttonText);

	attachEventListener(button, 'click', callactionclick, true);

	return button;
}


function SyncValues(table, matchingRow, statusText, usernameText, directionalText, customerNameText, customerPhoneText, durationText, miscText)
{

	var currentDurationSpan = matchingRow.childNodes[0].childNodes[1]; // row, call div, duration span

	var testString = BuildDurationString(durationText);
	if(currentDurationSpan.childNodes[0].nodeValue != testString)
	{
		currentDurationSpan.childNodes[0].nodeValue = testString;
	}

/*	var callNode = matchingRow.childNodes[0];
	var callNodeText = callNode.childNodes[0];

	var durationNode = matchingRow.childNodes[1];
	var durationNodeText = durationNode.childNodes[0];


	if(durationNodeText.nodeValue != durationText)
	{
		durationNodeText.nodeValue = durationText;
	}

	
	if(miscNodeText.nodeValue != miscText)
	{
		miscNodeText.nodeValue = miscText;
	}
		
*/
	
}


function RemoveRow(childNode)
{
	childNode.parentNode.removeChild(childNode);
	
	// child userid
	var currentUser = childNode.userId;
	// remove functionbar
	var functionBar = document.getElementById('functionbar' + currentUser);
	functionBar.parentNode.removeChild(functionBar);	
	
	// check to see if the editbox is showing for this user
	var editbox = document.getElementById('editbox');
	if(editbox.userId == currentUser)
	{
		editbox.style.visibility = 'hidden';
	}
}


function add()
{
	var doc = null;
	if(!exDom)
	{
		// code for IE
		if (window.ActiveXObject)
  		{
  			doc=new ActiveXObject('Microsoft.XMLDOM');
  			doc.async='false';
  			doc.loadXML(exampleDom);
  		}
		// code for Mozilla, Firefox, Opera, etc.
		else
  		{
  			var parser=new DOMParser();
  			doc=parser.parseFromString(exampleDom,'text/xml');
  		}

		// documentElement always represents the root node
		exDom=doc.documentElement;
	}

	// add some more stuff to existing dom

	var allUsersNode = exDom.childNodes.length == 0 ? false : exDom.childNodes[0];

	if(allUsersNode)
	{
		var table = document.getElementById('activecalls');
		
		var rowsFound = new Array();

		var maxInt = -1;
		for(i = 0; i &lt; allUsersNode.childNodes.length; i++)
		{
			var userElement = allUsersNode.childNodes[i];
			var userId = userElement.getAttribute('id');
			if(parseInt(userId) &gt; maxInt)
			{
				maxInt = parseInt(userId);
			}
		}


		// add a new row

		 AddRow(table, (maxInt + 1).toString(), 'my status', 'Billy Bob', 'called', 'His Highness', '512-3333-222', '00:00:12', 'belH');
		 
		 // update dom
		 var newUser = doc.createElement('user');
		 newUser.setAttribute('id', (maxInt + 1).toString());
		 newUser.setAttribute('status', 'active');
		 newUser.setAttribute('username', 'Billy Bob');
		 newUser.setAttribute('direction', 'inbound');
		 newUser.setAttribute('customer', 'His Highness');
		 newUser.setAttribute('customerNumber', '512-3333-222');
		 newUser.setAttribute('time', '00:01:15');
		 newUser.setAttribute('misc', 'belh');
		 
		 allUsersNode.appendChild(newUser);	
	}
}

function remove()
{
	var table = document.getElementById('activecalls');
	RemoveRow(table.childNodes[2]);
		
}

		&lt;/script&gt;
		&lt;link rel='stylesheet' type='text/css' href='web.css'&gt;
	&lt;/head&gt;
	&lt;body onload='load()'&gt;
		&lt;div id='headerbar'&gt;
			&lt;span&gt;Cisco Unified Application Environment SDK Reference Application&lt;/span&gt;
			&lt;div id='subheaderbar'&gt;
				&lt;span&gt;In-house Sales Manager Portal&lt;/span&gt;
			&lt;/div&gt;
		&lt;/div&gt;
		&lt;div id='activecalls'&gt;
			&lt;div id='activecallstitle'&gt;In-house Rep Call Panel&lt;/div&gt;
		&lt;/div&gt;
		&lt;!--&lt;div id='testbox' style='display:block;position:absolute;width:100px;height 50px;top:400px' onclick='add()'&gt;Add&lt;/div&gt;--&gt;
		&lt;div id='controlbox'&gt;
			&lt;div id='controlboxtitle'&gt;Controls&lt;/div&gt;
			&lt;div class='helpbutton' onclick='makeActionRequest(messageButtonType, this)'&gt;&lt;span&gt;Broadcast&lt;/span&gt;&lt;/div&gt;
		&lt;/div&gt;
		&lt;div id='responsebox'&gt;
			&lt;div id='responseboxtitle'&gt;&lt;/div&gt;
			&lt;span id='responseboxtext'&gt;&lt;/span&gt;
		&lt;/div&gt;
		&lt;div id='editbox'&gt;
			&lt;div&gt;
				&lt;textarea id='messagetext'&gt;&lt;/textarea&gt; &lt;input type='submit' name='Send' value='Send' onclick='messageSubmit(this)' /&gt;
				&lt;input type='button' name='Cancel' value='Cancel' onclick='messageCancel(this)' /&gt;
			&lt;/div&gt;
		&lt;/div&gt;
	&lt;/body&gt;
&lt;/html&gt;";

return "success";

}
</Properties>
    </node>
    <node type="Action" id="633068211791407101" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="699" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="633068211791407103" text="There is a big webpage lurking in this custom code..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="256" y="200" />
    <node type="Action" id="633068643791883318" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="580" y="265">
      <linkto id="633068211791407101" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Location" type="csharp">"http://" + g_host + "/SalesforceDemo/Admin"</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Variable" id="633066398551719578" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="633066398551719582" name="form" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference" name="Metreos.Providers.Http.GotRequest">form</Properties>
    </node>
    <node type="Variable" id="633068643791883317" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="GetState" startnode="633068211791407088" treenode="633068211791407089" appnode="633068211791407086" handlerfor="633068211791407085">
    <node type="Start" id="633068211791407088" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="118" y="462">
      <linkto id="633068211791407090" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633068211791407090" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="297.577148" y="444" mx="340" my="460">
      <items count="1">
        <item text="GetClientType" />
      </items>
      <linkto id="633068211791407092" type="Labeled" style="Vector" label="Browser" />
      <linkto id="633068211791407095" type="Labeled" style="Vector" label="Phone" />
      <Properties final="false" type="appControl" log="On">
        <ap name="phoneModelHeader" type="variable">phoneModelHeader</ap>
        <ap name="FunctionName" type="literal">GetClientType</ap>
      </Properties>
    </node>
    <node type="Action" id="633068211791407092" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="460.948242" y="323" mx="502" my="339">
      <items count="1">
        <item text="ComposeGrid" />
      </items>
      <linkto id="633068211791407094" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">ComposeGrid</ap>
      </Properties>
    </node>
    <node type="Action" id="633068211791407094" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="732" y="340">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633068211791407095" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="567" y="663">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="633068211791407096" text="Todo" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="541" y="582" />
    <node type="Variable" id="633068211791407091" name="phoneModelHeader" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="x-CiscoIPPhoneModelName" defaultInitWith="NONE" refType="reference">phoneModelHeader</Properties>
    </node>
    <node type="Variable" id="633068211791407093" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Whisper" startnode="633071510444337663" treenode="633071510444337664" appnode="633071510444337661" handlerfor="633071510444337660">
    <node type="Start" id="633071510444337663" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="344">
      <linkto id="633071510444337672" type="Basic" style="Vector" />
    </node>
    <node type="Comment" id="633071510444337665" text="Need ID of call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="256" y="208" />
    <node type="Action" id="633071510444337666" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1768" y="176">
      <linkto id="633071510444337704" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPRx:" + userDeviceIp</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337668" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="504" y="344">
      <linkto id="633071510444337680" type="Labeled" style="Vector" label="Success" />
      <linkto id="633071665896165351" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">String.Format("SELECT devicename FROM activecalls WHERE id = {0}", id);</ap>
        <ap name="Name" type="literal">activecalls</ap>
        <rd field="ResultSet">findDeviceNameResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337672" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="216" y="344">
      <linkto id="633071510444337675" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">queryParams["id"]</ap>
        <ap name="Value2" type="csharp">queryParams["type"]</ap>
        <rd field="ResultData">id</rd>
        <rd field="ResultData2">type</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337675" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="336" y="344">
      <linkto id="633071510444337720" type="Labeled" style="Vector" label="default" />
      <linkto id="633071665896165382" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">id != null &amp;&amp; id != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444337680" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="728" y="344">
      <linkto id="633071510444337686" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">findDeviceNameResults.Rows.Count == 0 ? String.Empty : findDeviceNameResults.Rows[0][0] as string</ap>
        <rd field="ResultData">deviceName</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337683" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="976" y="344">
      <linkto id="633071510444337687" type="Labeled" style="Vector" label="Success" />
      <linkto id="633071665896165359" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">deviceIpResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337686" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="848" y="344">
      <linkto id="633071510444337683" type="Labeled" style="Bezier" label="true" />
      <linkto id="633071665896165355" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">deviceName != null &amp;&amp; deviceName != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444337687" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1136" y="344">
      <linkto id="633071510444337690" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">deviceIpResults.Rows.Count == 0 ? String.Empty : deviceIpResults.Rows[0]["IP"] as string</ap>
        <rd field="ResultData">deviceIp</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337690" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1240" y="344">
      <linkto id="633071510444337696" type="Labeled" style="Vector" label="true" />
      <linkto id="633071665896165363" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">deviceIp != null &amp;&amp; deviceIp != String.Empty</ap>
      </Properties>
    </node>
    <node type="Comment" id="633071510444337691" text="Check that devicename from the database is populated" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="560" y="224" />
    <node type="Comment" id="633071510444337692" text="Check that deviceIP from the database is populated" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="976" y="216" />
    <node type="Comment" id="633071510444337694" text="If one sends RTPRx: without port, the phone will chose an available port.&#xD;&#xA;&#xD;&#xA;&lt;CiscoIPPhoneResponse&gt;&#xD;&#xA;&lt;ResponseItem URL=&quot;RTPRx:10.10.10.10&quot; Data=&quot;24684&quot; Status=&quot;0&quot;/&gt;&#xD;&#xA;&lt;/CiscoIPPhoneResponse&gt;&#xD;&#xA;&#xD;&#xA;We now have IP address of both user phone, and receiver phone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1808" y="32" />
    <node type="Action" id="633071510444337696" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="1360" y="344">
      <linkto id="633071510444337697" type="Labeled" style="Vector" label="Success" />
      <linkto id="633071665896165367" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">g_userDevicename</ap>
        <rd field="ResultData">userIpResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337697" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1520" y="344">
      <linkto id="633071510444337698" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">userIpResults.Rows.Count == 0 ? String.Empty : userIpResults.Rows[0]["IP"] as string</ap>
        <rd field="ResultData">userDeviceIp</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337698" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1632" y="344">
      <linkto id="633071510444337666" type="Labeled" style="Vector" label="commented_out" />
      <linkto id="633071665896165388" type="Labeled" style="Vector" label="default" />
      <linkto id="633071510444337870" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">userDeviceIp != null &amp;&amp; userDeviceIp != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444337704" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1944" y="176">
      <linkto id="633071510444337709" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">deviceIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
        <rd field="ResultData">executeResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337709" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="2080" y="176">
      <linkto id="633071510444337711" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.CiscoIpPhone.Response executeResponse, ref int chosenPort, LogWriter log)
{
	// executeResponse does not currently parameter the response XML.
	
	if(executeResponse == null)
		log.Write(TraceLevel.Error, "executeResponse == null");

	string responseXml = executeResponse.ToString();
	
	XmlDocument doc = new XmlDocument();
	
	try
	{
		doc.Load(responseXml);
	}
	catch(Exception e)	
	{
		log.Write(TraceLevel.Error, "Unable to load the XML response returned by the phone after SendExecute command {0}", e);
		return "failure";
	}
	
	XmlNode node = null;
	try
	{
		node = doc.SelectSingleNode("ResponseItem");
	}
	catch(Exception e)
	{
		log.Write(TraceLevel.Error, "Unable to perform XPath operation on Response XML of IP Phone.  {0}", e);
		return "failure";
	}

	if(node != null)
	{
		XmlAttribute attr = node.Attributes["Data"];
		if(attr != null)
		{
			try
			{
				chosenPort = int.Parse(attr.Value);
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, "Unable to parse {0} as an integer", attr.Value);
				return "failure";
			}
		}
		else
		{
			log.Write(TraceLevel.Error, "Unable to retrieve the 'Data' attribute from the response XML");
			return "failure";
		}
	}
	else
	{
		log.Write(TraceLevel.Error, "XPath operation returned no nodes.");
		return "failure";
	}

	return "success";

}
</Properties>
    </node>
    <node type="Action" id="633071510444337711" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2240" y="344">
      <linkto id="633071510444337712" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPTx:" + deviceIp + ":" + chosenPort</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337712" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2416" y="344">
      <linkto id="633071510444337717" type="Labeled" style="Vector" label="Success" />
      <linkto id="633071665896165375" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">userDeviceIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="633071510444337715" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2944" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633071510444337716" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="2728" y="344">
      <linkto id="633071510444337715" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="variable">body</log>
      </Properties>
    </node>
    <node type="Action" id="633071510444337717" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="2560" y="344">
      <linkto id="633071510444337716" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(int code, string errorMessage, string id, string type, ref string body, ref string g_currentOperation, string userDeviceIp, ref string g_currentOperationIp)
{
	body = String.Format("&lt;data&gt;&lt;result type='{0}' id='{1}' code='{2}'&gt;{3}&lt;/result&gt;&lt;/data&gt;", type, id, code, errorMessage);

	if(code == 0)
	{
		g_currentOperation = "WHISPER";
		g_currentOperationIp = userDeviceIp;
	}

	return "success"; 
}
</Properties>
    </node>
    <node type="Action" id="633071510444337720" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="336" y="456">
      <linkto id="633071510444337721" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">1</ap>
        <ap name="Value2" type="literal">Web client sent malformed message (no ID).</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071510444337721" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="336" y="560" />
    <node type="Label" id="633071510444337724" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2560" y="208">
      <linkto id="633071510444337717" type="Basic" style="Vector" />
    </node>
    <node type="Comment" id="633071510444337869" text="Investigate why Response object null for IP Communicator" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1880" y="128" />
    <node type="Action" id="633071510444337870" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1776" y="344">
      <linkto id="633071510444337871" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref int chosenPort)
{
	System.Random rand = new System.Random();
	chosenPort = rand.Next(20480, 32000);
	if(chosenPort % 2 == 1)
	{
		chosenPort = chosenPort + 1;
	}
	return "success";
}
</Properties>
    </node>
    <node type="Action" id="633071510444337871" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1904" y="344">
      <linkto id="633071510444337872" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPRx:" + userDeviceIp + ":" + chosenPort</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633071510444337872" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2080" y="344">
      <linkto id="633071510444337711" type="Labeled" style="Vector" label="Success" />
      <linkto id="633071665896165371" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">deviceIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
        <rd field="ResultData">executeResults</rd>
      </Properties>
    </node>
    <node type="Comment" id="633071510444337875" text="Because of issue with Response object, instead am choosing port for phone." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1728" y="392" />
    <node type="Action" id="633071665896165351" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="504" y="456">
      <linkto id="633071665896165352" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">2</ap>
        <ap name="Value2" type="literal">Unable to query the database for call</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165352" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="504" y="560" />
    <node type="Action" id="633071665896165355" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="456">
      <linkto id="633071665896165356" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">3</ap>
        <ap name="Value2" type="literal">Unable to query the database for the device name associated with the call</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165356" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="848" y="560" />
    <node type="Action" id="633071665896165359" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="976" y="456">
      <linkto id="633071665896165360" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">4</ap>
        <ap name="Value2" type="csharp">"Unable to query the real-time cache for the devicename " + deviceName</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165360" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="976" y="560" />
    <node type="Action" id="633071665896165363" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1240" y="456">
      <linkto id="633071665896165364" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">5</ap>
        <ap name="Value2" type="csharp">"The real-time cache returned an empty IP address for devicename " + deviceName</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165364" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1240" y="560" />
    <node type="Action" id="633071665896165367" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1360" y="456">
      <linkto id="633071665896165368" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">6</ap>
        <ap name="Value2" type="csharp">"Unable to query the real-time cache for the devicename " + g_userDevicename</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165368" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1360" y="560" />
    <node type="Action" id="633071665896165371" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2080" y="480">
      <linkto id="633071665896165372" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">7</ap>
        <ap name="Value2" type="csharp">"Unable to send Rx command to " + deviceName + ":" + deviceIp</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165372" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2080" y="584" />
    <node type="Action" id="633071665896165375" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2416" y="472">
      <linkto id="633071665896165376" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">8</ap>
        <ap name="Value2" type="csharp">"Unable to send Tx command to " + g_userDevicename + ":" + userDeviceIp</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165376" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="2416" y="576" />
    <node type="Action" id="633071665896165382" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="368" y="208" mx="433" my="224">
      <items count="1">
        <item text="ClearCurrentOperation" />
      </items>
      <linkto id="633071510444337668" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">ClearCurrentOperation</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896165388" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1632" y="456">
      <linkto id="633071665896165389" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">9</ap>
        <ap name="Value2" type="csharp">"The real-time cache returned an empty IP address for user devicename " + g_userDevicename</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896165389" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1632" y="560" />
    <node type="Comment" id="633071665896165436" text="Set WHISPER to currentState" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="2488" y="376" />
    <node type="Comment" id="633071665896165643" text="&lt;data&gt;&lt;result type='{0}' id='{1}' code='{2}'&gt;{3}&lt;/result&gt;&lt;/data&gt;&#xD;&#xA;&#xD;&#xA; type, id, code, errorMessage&#xD;&#xA;&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="2480" y="408" />
    <node type="Variable" id="633071510444337667" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="633071510444337673" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="633071510444337674" name="id" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">id</Properties>
    </node>
    <node type="Variable" id="633071510444337681" name="findDeviceNameResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">findDeviceNameResults</Properties>
    </node>
    <node type="Variable" id="633071510444337682" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="633071510444337684" name="deviceIpResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">deviceIpResults</Properties>
    </node>
    <node type="Variable" id="633071510444337689" name="deviceIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceIp</Properties>
    </node>
    <node type="Variable" id="633071510444337695" name="userDeviceIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userDeviceIp</Properties>
    </node>
    <node type="Variable" id="633071510444337702" name="userIpResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">userIpResults</Properties>
    </node>
    <node type="Variable" id="633071510444337703" name="executeResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Response" refType="reference">executeResults</Properties>
    </node>
    <node type="Variable" id="633071510444337710" name="chosenPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">chosenPort</Properties>
    </node>
    <node type="Variable" id="633071510444337718" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" initWith="" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="633071510444337722" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="633071510444337723" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="633071510444337725" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="633071665896165642" name="type" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">type</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SendUniMessage" startnode="633071665896166471" treenode="633071665896166472" appnode="633071665896166469" handlerfor="633071665896166468">
    <node type="Start" id="633071665896166471" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="432">
      <linkto id="633071665896166479" type="Basic" style="Vector" />
    </node>
    <node type="Comment" id="633071665896166476" text="Need ID of call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="248" y="296" />
    <node type="Action" id="633071665896166478" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="496" y="432">
      <linkto id="633071665896166481" type="Labeled" style="Vector" label="Success" />
      <linkto id="633071665896166507" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">String.Format("SELECT devicename FROM activecalls WHERE id = {0}", id);</ap>
        <ap name="Name" type="literal">activecalls</ap>
        <rd field="ResultSet">findDeviceNameResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896166479" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="208" y="432">
      <linkto id="633071665896166480" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">queryParams["id"]</ap>
        <ap name="Value2" type="csharp">queryParams["type"]</ap>
        <rd field="ResultData">id</rd>
        <rd field="ResultData2">type</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896166480" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="328" y="432">
      <linkto id="633071665896166499" type="Labeled" style="Vector" label="default" />
      <linkto id="633071665896166521" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">id != null &amp;&amp; id != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896166481" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="720" y="432">
      <linkto id="633071665896166483" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">findDeviceNameResults.Rows.Count == 0 ? String.Empty : findDeviceNameResults.Rows[0][0] as string</ap>
        <rd field="ResultData">deviceName</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896166482" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="968" y="432">
      <linkto id="633071665896166484" type="Labeled" style="Vector" label="Success" />
      <linkto id="633071665896166511" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">deviceName</ap>
        <rd field="ResultData">deviceIpResults</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896166483" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="840" y="432">
      <linkto id="633071665896166482" type="Labeled" style="Bezier" label="true" />
      <linkto id="633071665896166509" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">deviceName != null &amp;&amp; deviceName != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896166484" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1128" y="432">
      <linkto id="633071665896166485" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">deviceIpResults.Rows.Count == 0 ? String.Empty : deviceIpResults.Rows[0]["IP"] as string</ap>
        <rd field="ResultData">deviceIp</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896166485" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1232" y="432">
      <linkto id="633071665896166513" type="Labeled" style="Vector" label="default" />
      <linkto id="633071665896166582" type="Labeled" style="Vector" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">deviceIp != null &amp;&amp; deviceIp != String.Empty</ap>
      </Properties>
    </node>
    <node type="Comment" id="633071665896166486" text="Check that devicename from the database is populated" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="624" y="360" />
    <node type="Comment" id="633071665896166487" text="Check that deviceIP from the database is populated" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="968" y="304" />
    <node type="Action" id="633071665896166496" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2224" y="432">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633071665896166497" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="2008" y="432">
      <linkto id="633071665896166496" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">response</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="variable">body</log>
      </Properties>
    </node>
    <node type="Action" id="633071665896166498" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1840" y="432">
      <linkto id="633071665896166497" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(int code, string errorMessage, string id, string type, ref string response, ref string g_currentOperation)
{
	response = String.Format("&lt;data&gt;&lt;result type='{0}' id='{1}' code='{2}'&gt;{3}&lt;/result&gt;&lt;/data&gt;", type, id, code, errorMessage);

	if(code == 0)
	{
		g_currentOperation = "MESSAGE";
	}

	return "success"; 
}
</Properties>
    </node>
    <node type="Action" id="633071665896166499" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="544">
      <linkto id="633071665896166500" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">1</ap>
        <ap name="Value2" type="literal">Web client sent malformed message (no ID).</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896166500" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="328" y="648" />
    <node type="Label" id="633071665896166501" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1840" y="296">
      <linkto id="633071665896166498" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633071665896166505" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1592" y="432">
      <linkto id="633071665896166517" type="Labeled" style="Vector" label="default" />
      <linkto id="633071665896166498" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">text.ToString()</ap>
        <ap name="URL" type="variable">deviceIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896166507" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="544">
      <linkto id="633071665896166508" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">2</ap>
        <ap name="Value2" type="literal">Unable to query the database for call</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896166508" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="496" y="648" />
    <node type="Action" id="633071665896166509" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="840" y="544">
      <linkto id="633071665896166510" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">3</ap>
        <ap name="Value2" type="literal">Unable to query the database for the device name associated with the call</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896166510" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="840" y="648" />
    <node type="Action" id="633071665896166511" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="968" y="544">
      <linkto id="633071665896166512" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">4</ap>
        <ap name="Value2" type="csharp">"Unable to query the real-time cache for the devicename " + deviceName</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896166512" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="968" y="648" />
    <node type="Action" id="633071665896166513" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1232" y="544">
      <linkto id="633071665896166514" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">5</ap>
        <ap name="Value2" type="csharp">"The real-time cache returned an empty IP address for devicename " + deviceName</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896166514" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1232" y="648" />
    <node type="Action" id="633071665896166517" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1592" y="568">
      <linkto id="633071665896166518" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">7</ap>
        <ap name="Value2" type="csharp">"Unable to send text command to " + deviceName + ":" + deviceIp</ap>
        <rd field="ResultData">code</rd>
        <rd field="ResultData2">errorMessage</rd>
        <log condition="exit" on="true" level="Error" type="variable">errorMessage</log>
      </Properties>
    </node>
    <node type="Label" id="633071665896166518" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1592" y="672" />
    <node type="Action" id="633071665896166521" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="360" y="296" mx="425" my="312">
      <items count="1">
        <item text="ClearCurrentOperation" />
      </items>
      <linkto id="633071665896166478" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">ClearCurrentOperation</ap>
      </Properties>
    </node>
    <node type="Comment" id="633071665896166524" text="Set MESSAGE to currentState" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1768" y="464" />
    <node type="Comment" id="633071665896166525" text="&lt;data&gt;&lt;result type='{0}' id='{1}' code='{2}'&gt;{3}&lt;/result&gt;&lt;/data&gt;&#xD;&#xA;&#xD;&#xA; type, id, code, errorMessage&#xD;&#xA;&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1760" y="496" />
    <node type="Action" id="633071665896166582" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1408" y="432">
      <linkto id="633071665896166505" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">"Message from " + g_username</ap>
        <ap name="Text" type="csharp">body["text"]</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Comment" id="633071665896166583" text="There absolutely needs to be a size check on the content typed by the end-user.&#xD;&#xA;The fact that this is not checking is not good. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1360" y="312" />
    <node type="Variable" id="633071665896166473" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="633071665896166474" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.FormCollection" initWith="body" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="633071665896166475" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="633071665896166576" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="633071665896166577" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="633071665896166578" name="findDeviceNameResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">findDeviceNameResults</Properties>
    </node>
    <node type="Variable" id="633071665896166579" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="633071665896166580" name="deviceIpResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">deviceIpResults</Properties>
    </node>
    <node type="Variable" id="633071665896166581" name="deviceIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceIp</Properties>
    </node>
    <node type="Variable" id="633071665896166584" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="633071665896166585" name="id" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">id</Properties>
    </node>
    <node type="Variable" id="633071665896166586" name="type" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">type</Properties>
    </node>
    <node type="Variable" id="633086135845415849" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">response</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ComposeGrid" startnode="633066398551719436" treenode="633066398551719437" appnode="633066398551719434" handlerfor="633071665896166468">
    <node type="Loop" id="633082104606024666" name="Loop" text="loop (expr)" cx="522" cy="289" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="478" y="601" mx="739" my="746">
      <linkto id="633082104606024670" fromport="1" type="Basic" style="Vector" />
      <linkto id="633066398551719443" fromport="3" type="Labeled" style="Vector" label="default" />
      <Properties iteratorType="int" type="csharp">g_activeCalls.Rows.Count</Properties>
    </node>
    <node type="Start" id="633066398551719436" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="93" y="347">
      <linkto id="633068985643791564" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633066398551719440" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="271" y="348">
      <linkto id="633068211791407002" type="Labeled" style="Vector" label="default" />
      <linkto id="633082104606024678" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="literal">select id, devicename, to_number, from_number, active, direction, state from activecalls</ap>
        <ap name="Name" type="literal">activecalls</ap>
        <rd field="ResultSet">g_activeCalls</rd>
      </Properties>
    </node>
    <node type="Comment" id="633066398551719441" text="&#xD;&#xA;CREATE TABLE activecalls&#xD;&#xA;(&#xD;&#xA;  id INT unsigned NOT NULL auto_increment,&#xD;&#xA;  devicename VARCHAR(25),&#xD;&#xA;  to_number VARCHAR(25) NOT NULL DEFAULT '',&#xD;&#xA;  from_number VARCHAR(25) NOT NULL DEFAULT '',&#xD;&#xA;	active TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',&#xD;&#xA;  direction tinyint(1) unsigned NOT NULL default '0', /* 0=inbound, 1=outbound */&#xD;&#xA;	state tinyint(1) unsigned NOT NULL default '0', /* 0=hold, 1=active */&#xD;&#xA;	PRIMARY KEY(id)&#xD;&#xA;);" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="32" />
    <node type="Action" id="633066398551719443" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1027.4707" y="339">
      <linkto id="633068211791407000" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable g_activeCalls, Hashtable g_users, ref string body, LogWriter log, Hashtable g_customerInfos)
{
	System.Text.StringBuilder builder = new System.Text.StringBuilder();
	builder.Append("&lt;?xml version='1.0' encoding='ISO-8859-1'?&gt;");
	builder.Append("&lt;data&gt;");
	builder.Append("&lt;allusers&gt;");

	foreach(DataRow row in g_activeCalls.Rows)
	{
		// pull raw data out
		string id = Convert.ToString(row["id"]);
		string status = Convert.ToInt32(row["state"]) == 0 ? "inactive" : "active";
		string deviceName = row["devicename"] as string;
		string toNumber = row["to_number"] as string;
		string fromNumber = row["from_number"] as string;
		string direction = Convert.ToInt32(row["direction"]) == 0 ? "inbound" : "outbound";
		DateTime active = Convert.ToDateTime(row["active"]);

		// correlate useful info for web interface
		// determine username, based on devicename

		bool found = false;
		string username = String.Empty;
		foreach(DictionaryEntry entry in g_users)
		{
			if(String.Compare(deviceName, entry.Value as string, true) == 0)
			{
				found = true;
				username = entry.Key as string;
				break;
			}
		}

		if(!found)
		{
			log.Write(TraceLevel.Warning, "Could not find user associated with '{0}' in the Users configuration item", deviceName);
		}

		// determine customer number
		string customerNumber = String.Empty;
		if(direction == "inbound")
		{
			customerNumber = fromNumber;
		}
		else
		{
			customerNumber = toNumber;
		}

		string customerName = String.Empty;

		// determine customer display name
		if(customerNumber != null &amp;&amp; g_customerInfos.Contains(customerNumber))
		{
			customerName = g_customerInfos[customerNumber] as string;
		}

	
		// determine misc link
		string misc = String.Empty;

		// build time string;
		string formattedTime = String.Empty;
		TimeSpan time = DateTime.Now.Subtract(active);
		string hours = time.Hours.ToString();
		string minutes = time.Minutes.ToString();
		string seconds = time.Seconds.ToString();

		if(hours.Length == 1)
		{
			hours = "0" + hours;
		}
		if(minutes.Length == 1)
		{
			minutes = "0" + minutes;
		}
		if(seconds.Length == 1)
		{
			seconds = "0" + seconds;
		}

		formattedTime = String.Format("{0}:{1}:{2}", hours, minutes, seconds);
		
		builder.AppendFormat("&lt;user id='{0}' status='{1}' username='{2}' direction='{3}' customer='{4}' customerNumber='{5}' time='{6}' misc='{7}' /&gt;", 
			id, status, username, direction, customerName, customerNumber, formattedTime, misc);
		
	}

	builder.Append("&lt;/allusers&gt;");
	builder.Append("&lt;/data&gt;");

	body = builder.ToString();

	return "success";
}
</Properties>
    </node>
    <node type="Comment" id="633068211791406999" text="&lt;data&gt;&#xD;&#xA;    &lt;allusers&gt;&#xD;&#xA;        &lt;user id='1' status='active' username='User1' direction='inbound' &#xD;&#xA;customer='Dave Jones' customerNumber='512-555-1234' &#xD;&#xA;time='00:01:15' misc='http://appserver_ip:8000/SalesforceDemo/Misc' /&gt;&#xD;&#xA;    &lt;/allusers&gt;&#xD;&#xA;&lt;/data&gt;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="502" y="149" />
    <node type="Action" id="633068211791407000" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1192.4707" y="340">
      <linkto id="633068211791407006" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Cache-Control" type="literal">no-cache</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"ResponseBody XML: " + body</log>
      </Properties>
    </node>
    <node type="Action" id="633068211791407002" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="270" y="534">
      <linkto id="633068211791407004" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">500</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="exit" on="true" level="Info" type="literal">Failure to read database!</log>
      </Properties>
    </node>
    <node type="Action" id="633068211791407004" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="273" y="738">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633068211791407006" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1336.4707" y="340">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="633068211791407007" text="=&gt;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="460" y="185" />
    <node type="Comment" id="633068211791407008" text="SF Database for customer number to customer name correlation" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="54" y="229" />
    <node type="Action" id="633068985643791564" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="170" y="348">
      <linkto id="633066398551719440" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable g_activeCalls)
{
	g_activeCalls.Clear();// work-around for a bug

	return ""; 
}
</Properties>
    </node>
    <node type="Action" id="633082104606024665" name="CustomerLookup" container="633082104606024666" class="MaxActionNode" group="" path="Metreos.Providers.SalesforceDemo" x="798" y="732">
      <linkto id="633082104606024675" type="Labeled" style="Bezier" label="Success" />
      <linkto id="633082104606024676" type="Labeled" style="Vector" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="PhoneNumber" type="variable">phone</ap>
        <rd field="FirstName">first</rd>
        <rd field="LastName">last</rd>
        <rd field="AccountName">account</rd>
      </Properties>
    </node>
    <node type="Action" id="633082104606024670" name="Assign" container="633082104606024666" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="589" y="732">
      <linkto id="633082104606024671" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">Convert.ToInt32(g_activeCalls.Rows[loopIndex]["direction"]) == 0 ?  Convert.ToString(g_activeCalls.Rows[loopIndex]["from_number"]) : Convert.ToString(g_activeCalls.Rows[loopIndex]["to_number"]) </ap>
        <rd field="ResultData">phone</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Customer Number: " + (Convert.ToInt32(g_activeCalls.Rows[loopIndex]["direction"]) == 0 ?  Convert.ToString(g_activeCalls.Rows[loopIndex]["from_number"]) : Convert.ToString(g_activeCalls.Rows[loopIndex]["to_number"]))</log>
      </Properties>
    </node>
    <node type="Action" id="633082104606024671" name="If" container="633082104606024666" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="690" y="732">
      <linkto id="633082104606024666" port="2" type="Labeled" style="Vector" label="true" />
      <linkto id="633082104606024665" type="Labeled" style="Vector" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_customerInfos.Contains(phone == null ? String.Empty : phone)</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Customer Infos Contains " + phone + "? " + (g_customerInfos.Contains(phone == null ? String.Empty : phone))</log>
      </Properties>
    </node>
    <node type="Action" id="633082104606024675" name="CustomCode" container="633082104606024666" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="894" y="732">
      <linkto id="633082104606024666" port="3" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable g_customerInfos, string phone, string account, string first, string last)
{
	g_customerInfos[phone] = String.Format("{0} {1} from {2}", first, last, account);

	return "";
}
</Properties>
    </node>
    <node type="Action" id="633082104606024676" name="CustomCode" container="633082104606024666" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="749" y="796">
      <linkto id="633082104606024666" port="4" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Hashtable g_customerInfos, string phone)
{
	g_customerInfos[phone] = null;

	return "";
}
</Properties>
    </node>
    <node type="Action" id="633082104606024678" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="406" y="343">
      <linkto id="633082104606024666" port="1" type="Labeled" style="Vector" label="true" />
      <linkto id="633066398551719443" type="Labeled" style="Vector" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_activeCalls.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Comment" id="633082104606024679" text="If already have customer info, &#xD;&#xA;don't bother doing Customer lookup" container="633082104606024666" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="596" y="654" />
    <node type="Variable" id="633068211791407001" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="633068211791407005" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="633082104606024669" name="phone" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">phone</Properties>
    </node>
    <node type="Variable" id="633082104606024672" name="account" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">account</Properties>
    </node>
    <node type="Variable" id="633082104606024673" name="first" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">first</Properties>
    </node>
    <node type="Variable" id="633082104606024674" name="last" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">last</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PromptLogin" startnode="633066398551719551" treenode="633066398551719552" appnode="633066398551719549" handlerfor="633071665896166468">
    <node type="Start" id="633066398551719551" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="431">
      <linkto id="633066398551719553" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633066398551719553" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="221.577148" y="415" mx="264" my="431">
      <items count="1">
        <item text="GetClientType" />
      </items>
      <linkto id="633066398551719562" type="Labeled" style="Vector" label="Browser" />
      <linkto id="633066398551719563" type="Labeled" style="Vector" label="Phone" />
      <Properties final="false" type="appControl" log="On">
        <ap name="phoneModelHeader" type="variable">phoneModelHeader</ap>
        <ap name="FunctionName" type="literal">GetClientType</ap>
      </Properties>
    </node>
    <node type="Action" id="633066398551719562" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="600" y="224">
      <linkto id="633066398551719564" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref string body, string g_routingGuid, string g_host)
{
		body = String.Format(
@"&lt;!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""&gt;
&lt;html xmlns='http://www.w3.org/1999/xhtml'&gt;
	&lt;head&gt;
		&lt;link rel='stylesheet' type='text/css' href='web.css'&gt; 
	&lt;/head&gt;

	&lt;body&gt; 
		&lt;div id='headerbar'&gt;
			&lt;span&gt;Cisco Unified Application Environment SDK Reference Application&lt;/span&gt;	
			&lt;div id='subheaderbar'&gt;
				&lt;span&gt;In-house Sales Manager Portal&lt;/span&gt;	
			&lt;/div&gt;
		&lt;/div&gt;


&lt;form id='login' method='post' action='http://{0}/SalesforceDemo/ProcessLogin?metreosSessionId={1}'&gt;
&lt;label for='username'&gt;Username&lt;/label&gt;&lt;input type='text' name='username' /&gt;&lt;br /&gt;
&lt;label for='password'&gt;Password&lt;/label&gt;&lt;input type='password' name='password' /&gt;&lt;br /&gt;
&lt;input type='submit' name='submit' /&gt;
&lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;", g_host, g_routingGuid);

return String.Empty;

}
</Properties>
    </node>
    <node type="Action" id="633066398551719563" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="610" y="610">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633066398551719564" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="800" y="224">
      <linkto id="633066398551719572" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Cache-Control" type="literal">no-cache</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="633066398551719572" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="920" y="224">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="633068211791407097" text="Todo" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="587" y="541" />
    <node type="Variable" id="633066398551719558" name="phoneModelHeader" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="phoneModelHeader" refType="reference">phoneModelHeader</Properties>
    </node>
    <node type="Variable" id="633066398551719565" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="633066398551719571" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="GetClientType" startnode="633066398551719556" treenode="633066398551719557" appnode="633066398551719554" handlerfor="633071665896166468">
    <node type="Start" id="633066398551719556" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="163" y="413">
      <linkto id="633066398551719560" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633066398551719560" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="405" y="412">
      <Properties final="true" type="appControl" log="On">
        <ap name="Type" type="csharp">phoneModelHeader != "NONE" ? "Phone" : "Browser"</ap>
        <ap name="ReturnValue" type="csharp">phoneModelHeader != "NONE" ? "Phone" : "Browser"</ap>
      </Properties>
    </node>
    <node type="Variable" id="633066398551719559" name="phoneModelHeader" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="phoneModelHeader" refType="reference">phoneModelHeader</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ClearCurrentOperation" startnode="633071665896165385" treenode="633071665896165386" appnode="633071665896165383" handlerfor="633071665896166468">
    <node type="Start" id="633071665896165385" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="88" y="360">
      <linkto id="633071665896165387" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633071665896165387" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="256" y="360">
      <linkto id="633071665896165394" type="Labeled" style="Vector" label="WHISPER" />
      <linkto id="633071665896165398" type="Labeled" style="Bezier" label="LISTEN" />
      <linkto id="633071665896165409" type="Labeled" style="Vector" label="BARGE" />
      <linkto id="633071665896165430" type="Labeled" style="Vector" label="NONE" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_currentOperation</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896165394" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="512" y="360">
      <linkto id="633071665896165396" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPTx:Stop</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896165396" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="720" y="360">
      <linkto id="633071665896165425" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_currentOperationIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896165397" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1040" y="200">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="633071665896165398" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="512" y="488">
      <linkto id="633071665896165399" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPRx:Stop</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896165399" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="720" y="488">
      <linkto id="633071665896165406" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_currentOperationIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896165406" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="864" y="488">
      <linkto id="633071665896165428" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_currentOperationCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896165409" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="512" y="584">
      <linkto id="633071665896165410" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="literal">RTPRx:Stop</ap>
        <ap name="URL2" type="literal">RTPTx:Stop</ap>
        <rd field="ResultData">execute</rd>
      </Properties>
    </node>
    <node type="Action" id="633071665896165410" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="720" y="584">
      <linkto id="633071665896165412" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="variable">g_currentOperationIp</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="633071665896165412" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="864" y="584">
      <linkto id="633071665896165426" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_currentOperationCallId</ap>
      </Properties>
    </node>
    <node type="Label" id="633071665896165425" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="904" y="360" />
    <node type="Label" id="633071665896165426" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1056" y="584" />
    <node type="Label" id="633071665896165428" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1056" y="488" />
    <node type="Label" id="633071665896165430" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="480" y="160" />
    <node type="Action" id="633071665896165432" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="904" y="200">
      <linkto id="633071665896165397" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">NONE</ap>
        <rd field="ResultData">g_currentOperation</rd>
      </Properties>
    </node>
    <node type="Label" id="633071665896165433" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="800" y="200">
      <linkto id="633071665896165432" type="Basic" style="Vector" />
    </node>
    <node type="Variable" id="633071665896165395" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>