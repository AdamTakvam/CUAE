<Application name="CSS" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="CSS">
    <outline>
      <treenode type="evh" id="633068643791883336" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="633068643791883333" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="633068643791883332" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SalesforceDemo/web.css</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="633068643791883335" treenode="633068643791883336" appnode="633068643791883333" handlerfor="633068643791883332">
    <node type="Start" id="633068643791883335" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="66" y="306">
      <linkto id="633068643791883337" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="633068643791883337" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="218.90625" y="308">
      <linkto id="633068643791883338" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref string body, string hostname)
{
body = 
@"html, body
{
	border:0;
	margin:0;
	left:0;
	right:0;
	top:0;
	bottom:0;
	font-family:Arial;
	background-color:#f0f0f0;
}

.calltext
{
}

.username
{
	font-weight:bold;
}

.customername
{
	font-weight:bold;
}

.directional
{
}

#activecalls
{
	position:absolute;
	left:250px;
	top:202px;
	width:655px;
}

div.normalrow
{
	text-align:center;
	font-size:2em;
	width:650px;
	position:absolute;
	display:block;
	margin:10px 0 0 0;
	background-color:#b0b0b0;
	border-color:black;
	border-width:2px;
	border-style:solid;
	color:black;
}


#activecallstitle
{
	padding-left:10px;
	background-color:#2f6681;
	margin:10px 0 10px 0;
	border-color:white;
	border-width:4px;
	border-style:solid;
	font-size:2em;
	color:White;
}


.functionbar
{
	background-color:Yellow;
	background-repeat:repeat-x;
	border-width:2px 2px 2px 0;
	border-style:solid;
	border-color:Black;
}


.whisperbutton, .listenbutton, .bargebutton, .messagebutton, .helpbutton, .intercom, .messagebroadcast
{
	position:relative;
	font-size:1.2em;
	border-width:2px;
	border-style:solid;
	border-color:Black;
	padding:3px;
	margin:2px;
	cursor:pointer;
	background-color:#b0b0b0;
}


span.whisperbutton:hover, span.listenbutton:hover, span.bargebutton:hover, span.messagebutton:hover, span.intercom:hover, span.messagebroadcast:hover
{
	background-color:black;
	color:white;
}


#headerbar
{
	padding-left:10px;
	top:35px;
	display:block;
	position:relative;
	background-color:#2f6681;
	color:White;
	font-size:2.5em;
	border-color:White;
	border-style:solid;
	border-width:6px 0;
}


#subheaderbar
{
	font-size:.8em;
	margin-top:10px;
}


#controlbox
{
	position:absolute;
	top:213px;
	width:220px;
}


#controlboxtitle
{
	position:relative;
	padding-left:10px;
	margin-bottom:20px;
	background-color:#2f6681;
	border-color:white;
	border-width:4px;
	border-style:solid;
	font-size:2em;
	color:White;
}

#responsebox
{
	position:absolute;
	top:165px;
	left:250px;
	font-size:1em;
	visibility:hidden;
	border-style:dotted;
	border-width:2px;
	border-color:White;
	background-color:#2f6681;
	padding:3px;
	color:White;
}


#responseboxtitle
{
	display:inline;
}

#responseboxtext
{
	
}

#login
{
	position:relative;
	display:block;
	left:100px;
	top:75px;
	padding-left:10px;
	border-width:0 0 0 17px;
	border-color:#2f6681;
	border-style:solid;
}

label
{
	color:#2f6681;
	font-weight:bold;
	width:100px;
}

input
{
	display:block;
	left:100px;
}

#editbox
{
	position:absolute;
	width:220px;
	height:220px;
	visibility:hidden;
	border-color:Black;
	border-style:solid;
	border-width:2px;
	background-color:#cecece;
	padding:4px;
}

#messagetext
{
	position:relative;
	width:100%;
	height:180px;
	margin:0;
	padding:0;
	background-color:White;
	color:#2f6681;
	border-color:#555555;
	border-style:solid;
	border-width:1px;
	overflow:auto;
}

#editbox input
{
	margin:auto;
	display:inline;
}
";

return "success";

}
</Properties>
    </node>
    <node type="Action" id="633068643791883338" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="409.90625" y="310">
      <linkto id="633068643791883558" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">body</ap>
        <ap name="Content-Type" type="literal">text/css</ap>
      </Properties>
    </node>
    <node type="Action" id="633068643791883558" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="562" y="311">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="633068643791883343" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="633068643791883344" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">body</Properties>
    </node>
    <node type="Variable" id="633068985643791472" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="hostname" refType="reference" name="Metreos.Providers.Http.GotRequest">hostname</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>