<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632834840330576922" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632834840330576919" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632834840330576918" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/walk</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_intercommanddelay" id="632834840330577704" vid="632834840330576931">
        <Properties type="String">g_intercommanddelay</Properties>
      </treenode>
      <treenode text="g_user" id="632834840330577706" vid="632834840330576974">
        <Properties type="String" initWith="user">g_user</Properties>
      </treenode>
      <treenode text="g_pass" id="632834840330577708" vid="632834840330576976">
        <Properties type="String" initWith="pass">g_pass</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632834840330576921" treenode="632834840330576922" appnode="632834840330576919" handlerfor="632834840330576918">
    <node type="Start" id="632834840330576921" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="344">
      <linkto id="632834840330577202" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632834840330577202" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="241" y="344">
      <linkto id="632834840330577203" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string host, string routingGuid, string cookie, ref string http)
{
	string command="";
	string phoneIp = "";
	string phoneName = "";
	if(cookie != "NONE")
	{
		string[] old = cookie.Split(new char[] {'|'}); 
		command = old[0];
		phoneIp = old[1];
		phoneName = old[2];
	}

	http = "&lt;html&gt;&lt;body&gt;&lt;form action='http://" + host + "/performwalk' method='POST'&gt;";
	http += "&lt;label&gt;Command&lt;/label&gt;&lt;br/&gt;&lt;textarea cols='20' rows='20' name='command'&gt;" + command + "&lt;/textarea&gt;&lt;br/&gt;";
	http += "&lt;label&gt;Phone IP&lt;/label&gt;&lt;br/&gt;&lt;input type='text' name='phoneIP' value='" + phoneIp + "' /&gt;&lt;br/&gt;";
	http += "&lt;label&gt;Phone Name&lt;/label&gt;&lt;br/&gt;&lt;input type='text' name='phoneName' value='" + phoneName + "' /&gt;&lt;br/&gt;";
	http += "&lt;input type='Submit' /&gt;";
	http += "&lt;/form&gt;&lt;br/&gt;&lt;br/&gt;&lt;br/&gt;";
	http += "&lt;h3&gt;Reference&lt;/h3&gt;";
	http += @"&lt;ul&gt;
&lt;li&gt;Key:Line1 to Key:Line34&lt;/li&gt;
&lt;li&gt;Key:KeyPad0 to Key:KeyPad9&lt;/li&gt;
&lt;li&gt;Key:Soft1 to Key:Soft5&lt;/li&gt;
&lt;li&gt;Key:KeyPadStar&lt;/li&gt;
&lt;li&gt;Key:KeyPadPound&lt;/li&gt;
&lt;li&gt;Key:VolDwn&lt;/li&gt;
&lt;li&gt;Key:VolUp&lt;/li&gt;
&lt;li&gt;Key:Headset&lt;/li&gt;
&lt;li&gt;Key:Speaker&lt;/li&gt;
&lt;li&gt;Key:Mute&lt;/li&gt;
&lt;li&gt;Key:Info&lt;/li&gt;
&lt;li&gt;Key:Messages&lt;/li&gt;
&lt;li&gt;Key:Services&lt;/li&gt;
&lt;li&gt;Key:Directories&lt;/li&gt;
&lt;li&gt;Key:Settings&lt;/li&gt;
&lt;li&gt;Key:NavUp&lt;/li&gt;
&lt;li&gt;Key:NavDwn&lt;/li&gt;
&lt;li&gt;Key:AppMenu&lt;/li&gt;
&lt;li&gt;Key:Hold&lt;/li&gt;
&lt;/ul&gt;
SoftKey:n
Where
n = one of the following softkey names:
&lt;ul&gt;
&lt;li&gt;Back&lt;/li&gt;
&lt;li&gt;Cancel&lt;/li&gt;
&lt;li&gt;Exit&lt;/li&gt;
&lt;li&gt;Next&lt;/li&gt;
&lt;li&gt;Search&lt;/li&gt;
&lt;li&gt;Select&lt;/li&gt;
&lt;li&gt;Submit&lt;/li&gt;
&lt;li&gt;Update&lt;/li&gt;
&lt;li&gt;Dial&lt;/li&gt;
&lt;li&gt;EditDial&lt;/li&gt;
&lt;li&gt;&amp;lt;&amp;lt;&lt;/li&gt;
&lt;li&gt;1-9&lt;/li&gt;
&lt;/ul&gt;
&lt;/body&gt;&lt;/html&gt;";

	return "true";
}
</Properties>
    </node>
    <node type="Action" id="632834840330577203" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="504" y="344">
      <linkto id="632834840330577204" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">http</ap>
        <ap name="Content-Type" type="literal">text/html</ap>
      </Properties>
    </node>
    <node type="Action" id="632834840330577204" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="644" y="342">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632834840330576933" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632834840330576936" name="phoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">phoneIp</Properties>
    </node>
    <node type="Variable" id="632834840330576940" name="results" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">results</Properties>
    </node>
    <node type="Variable" id="632834840330576942" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632834840330577140" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632834840330577142" name="remoteIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteIP</Properties>
    </node>
    <node type="Variable" id="632834840330577143" name="cookie" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="phoneWalkerCookie" defaultInitWith="NONE" refType="reference">cookie</Properties>
    </node>
    <node type="Variable" id="632834840330577602" name="http" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">http</Properties>
    </node>
    <node type="Variable" id="632834840330577721" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>