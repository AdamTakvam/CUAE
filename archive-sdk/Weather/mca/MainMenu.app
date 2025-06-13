<Application name="MainMenu" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="MainMenu">
    <outline>
      <treenode type="evh" id="632485420083424337" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632283491418437672" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283491418437671" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Weather/MainMenu</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632485420083424338" level="1" text="Metreos.Providers.Http.GotRequest: ChooseCity">
        <node type="function" name="ChooseCity" id="632283491418437708" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283491418437707" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Weather/ChooseCity</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632485420083424339" level="1" text="Metreos.Providers.Http.GotRequest: ChooseState">
        <node type="function" name="ChooseState" id="632283491418437688" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283491418437687" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Weather/ChooseState</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632485420083424340" level="1" text="Metreos.Providers.Http.GotRequest: DisplayStation">
        <node type="function" name="DisplayStation" id="632283491418437735" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632283491418437734" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Weather/DisplayStation</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632485420083424341" level="1" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632283491418437755" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632283491418437754" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_devicename" id="632974561273896688" vid="632786194365827167">
        <Properties type="String">g_devicename</Properties>
      </treenode>
      <treenode text="g_proxy" id="632974561273896780" vid="632974561273896779">
        <Properties type="String" defaultInitWith="NONE" initWith="httpproxyURL">g_proxy</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632283491418437674" treenode="632485420083424337" appnode="632283491418437672" handlerfor="632283491418437671">
    <node type="Start" id="632283491418437674" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="358">
      <linkto id="632786194365827046" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283491418437677" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="546" y="358">
      <linkto id="632785714975213965" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Weather</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437678" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="721" y="358">
      <linkto id="632283491418437682" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Find a weather station</ap>
        <ap name="URL" type="csharp">host + "/Weather/ChooseState?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437682" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="848" y="358">
      <linkto id="632283491418437683" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="csharp">menu.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632283491418437683" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1012" y="358">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632283491418437684" text="Making menu with one choice: Find a weather station.&#xD;&#xA;Only one choice makes this menu a bit pointless, but &#xD;&#xA;more options could easily be added later." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="805" y="226" />
    <node type="Comment" id="632283491418437686" text="Adding metreosSessionId=[routingGuid] &#xD;&#xA;to keep phone in session" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="536" y="228" />
    <node type="Comment" id="632785714975213961" text="Check for cookie presence, &#xD;&#xA;and attempt to parse preference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="325" y="262" />
    <node type="Action" id="632785714975213962" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="418" y="358">
      <linkto id="632283491418437677" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable result, ref bool foundPreference, ref string state, ref string station, LogWriter log, ref string g_devicename, Metreos.Types.Http.QueryParamCollection queryParams)
{

	g_devicename = queryParams["device"];

	log.Write(TraceLevel.Info, "Device=" + g_devicename);

	foundPreference = false;

	if(result != null &amp;&amp; result.Rows.Count == 1)
	{
		try
		{
			state = result.Rows[0]["state"] as string;
			station = result.Rows[0]["station"] as string;

			if(state != null &amp;&amp; state != String.Empty &amp;&amp; station != null &amp;&amp; station != String.Empty)
			{
				foundPreference = true;
			}
		}
		catch { }
	}

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632785714975213965" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="619" y="357">
      <linkto id="632785714975213966" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632283491418437678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">foundPreference</ap>
      </Properties>
    </node>
    <node type="Action" id="632785714975213966" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="675" y="272">
      <linkto id="632283491418437678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Favorite station</ap>
        <ap name="URL" type="csharp">host + "/Weather/DisplayStation?state=" + state + "&amp;station=" + station + "&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632786194365827044" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="260" y="358">
      <linkto id="632785714975213962" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT state, station FROM LastChoice WHERE devicename = '" + queryParams["device"] + "'"</ap>
        <ap name="Name" type="literal">preferences</ap>
        <rd field="ResultSet">result</rd>
      </Properties>
    </node>
    <node type="Action" id="632786194365827046" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="124" y="358">
      <linkto id="632786194365827044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">queryParams["device"] != null &amp;&amp; queryParams["device"] != String.Empty</ap>
      </Properties>
    </node>
    <node type="Variable" id="632283491418437676" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632283491418437679" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632283491418437680" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632283491418437681" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632785714975213963" name="foundPreference" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="false" refType="reference">foundPreference</Properties>
    </node>
    <node type="Variable" id="632785714975213967" name="state" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">state</Properties>
    </node>
    <node type="Variable" id="632785714975213968" name="station" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">station</Properties>
    </node>
    <node type="Variable" id="632786194365827045" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632786194365827047" name="result" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">result</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ChooseCity" startnode="632283491418437710" treenode="632485420083424338" appnode="632283491418437708" handlerfor="632283491418437707">
    <node type="Loop" id="632283491418437731" name="Loop" text="loop (expr)" cx="120" cy="120" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="499" y="301" mx="559" my="361">
      <linkto id="632283491418437732" fromport="1" type="Basic" style="Bevel" ortho="true" />
      <linkto id="632283491418437716" fromport="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">stations.Count &gt; 100 ? 100 : stations.Count</Properties>
    </node>
    <node type="Start" id="632283491418437710" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="361">
      <linkto id="632503579925120941" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632283491418437716" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="743" y="360">
      <linkto id="632283491418437733" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437720" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="219" y="361">
      <linkto id="632283491418437721" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Listing of stations</ap>
        <ap name="Prompt" type="literal">Choose Station</ap>
        <rd field="ResultData">menu</rd>
        <log condition="entry" on="true" level="Info" type="literal">Creating station list</log>
      </Properties>
    </node>
    <node type="Action" id="632283491418437721" name="GetAllStations" class="MaxActionNode" group="" path="Metreos.Native.Weather" x="362" y="362">
      <linkto id="632283491418437723" type="Labeled" style="Bevel" ortho="true" label="failure" />
      <linkto id="632283491418437731" port="1" type="Labeled" style="Bevel" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="State" type="csharp">queryParams["state"]</ap>
        <rd field="ResultData">stations</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437723" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="362" y="467">
      <linkto id="632283491418437725" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Weather</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="literal">State could not be found</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437725" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="364" y="567">
      <linkto id="632283491418437726" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437726" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="364" y="656">
      <linkto id="632283491418437727" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437727" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="365" y="756">
      <linkto id="632283491418437728" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">errorText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437728" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="365" y="854">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632283491418437732" name="AddMenuItem" container="632283491418437731" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="559" y="361">
      <linkto id="632283491418437731" port="3" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(stations[loopIndex] as string[])[1]</ap>
        <ap name="URL" type="csharp">host + "/Weather/DisplayStation?station=" + (stations[loopIndex] as string[])[0] + "&amp;state=" + queryParams["state"] + "&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437733" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="869.4121" y="359">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632283491418437749" text="GetAllStations returns a listing of station friendly names &#xD;&#xA;and abbreviations.&#xD;&#xA;&#xD;&#xA;Using these string values, construct a menu for the &#xD;&#xA;user to select from.   The friendly name is used for display, &#xD;&#xA;and the station id is used for the station query parameter in&#xD;&#xA;the URL field of each menu item" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="408.4121" y="175" />
    <node type="Action" id="632503579925120941" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="119.173843" y="360">
      <linkto id="632503579925120943" type="Labeled" style="Bevel" ortho="true" label="failure" />
      <linkto id="632283491418437720" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams)
{
	string state = queryParams["state"];

	if(state == null || state == String.Empty) return "failure";
	
	return "success";
}

</Properties>
    </node>
    <node type="Action" id="632503579925120942" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="121.173828" y="848">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632503579925120943" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="120.173828" y="451">
      <linkto id="632503579925120944" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Invalid Request</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="literal">State not specified</ap>
        <rd field="ResultData">menu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"State is: " + (queryParams["state"] == null ? "N/A" : queryParams["state"]) + ", City is: " + (queryParams["station"] == null ? "N/A" : queryParams["station"]) </log>
      </Properties>
    </node>
    <node type="Action" id="632503579925120944" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="120.173828" y="550">
      <linkto id="632503579925120945" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120945" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="121.173828" y="649">
      <linkto id="632503579925120946" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120946" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="120.173828" y="749">
      <linkto id="632503579925120942" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Variable" id="632283491418437712" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632283491418437713" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632283491418437714" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632283491418437715" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632283491418437718" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632283491418437722" name="stations" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.ArrayList" refType="reference">stations</Properties>
    </node>
    <node type="Variable" id="632283491418437724" name="errorText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">errorText</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ChooseState" startnode="632283491418437690" treenode="632485420083424339" appnode="632283491418437688" handlerfor="632283491418437687">
    <node type="Start" id="632283491418437690" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="306">
      <linkto id="632284533238906505" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632283491418437703" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="559" y="302">
      <linkto id="632283491418437704" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437704" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="681" y="303">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632283491418437748" text="GetStates returns a listing of state abbreviations.&#xD;&#xA;&#xD;&#xA;Using these string values, construct a menu for the &#xD;&#xA;user to select from." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="492.4707" y="132" />
    <node type="Action" id="632284533238906505" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="202" y="305">
      <linkto id="632284533238906508" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Choose state</ap>
        <ap name="Prompt" type="literal">Enter state abbreviation</ap>
        <ap name="URL" type="csharp">host + "/Weather/ChooseCity?metreosSessionId="  + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632284533238906508" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="311" y="303">
      <linkto id="632283491418437703" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">State</ap>
        <ap name="QueryStringParam" type="literal">state</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Variable" id="632283491418437696" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632283491418437698" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632283491418437699" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632283491418437700" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632284533238906506" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DisplayStation" activetab="true" startnode="632283491418437737" treenode="632485420083424340" appnode="632283491418437735" handlerfor="632283491418437734">
    <node type="Start" id="632283491418437737" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="343">
      <linkto id="632503579925120931" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632283491418437742" name="GetWeatherStatus" class="MaxActionNode" group="" path="Metreos.Native.Weather" x="248" y="343">
      <linkto id="632283491418437744" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="State" type="csharp">queryParams["state"]</ap>
        <ap name="Station" type="csharp">queryParams["station"]</ap>
        <ap name="Proxy" type="variable">g_proxy</ap>
        <rd field="ResultData">weatherString</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437744" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="371" y="343">
      <linkto id="632785794464280089" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Weather Status</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="variable">weatherString</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437746" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="594" y="342">
      <linkto id="632283491418437747" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">2</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid + "&amp;device=" + g_devicename</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437747" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="725" y="343">
      <linkto id="632283491418437752" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Comment" id="632283491418437750" text="With a state and station chosen,&#xD;&#xA;use GetWeatherStatus to format a &#xD;&#xA;weather string for us, which we pass &#xD;&#xA;back to the phone in a &#xD;&#xA;CiscoIPPhoneText object." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="469" y="149" />
    <node type="Action" id="632283491418437752" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="835" y="343">
      <linkto id="632786194365827169" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">weatherText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437753" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1197.4707" y="343">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632503579925120931" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="115.000015" y="344.000031">
      <linkto id="632283491418437742" type="Labeled" style="Bevel" ortho="true" label="default" />
      <linkto id="632503579925120933" type="Labeled" style="Bevel" ortho="true" label="failure" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref string state, ref string station)
{
	state = queryParams["state"];
	station = queryParams["station"];

	if(state == null || state == String.Empty) return "failure";
	if(station == null || station == String.Empty) return "failure";
	
	return "success";
}

</Properties>
    </node>
    <node type="Action" id="632503579925120932" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="117" y="832">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632503579925120933" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="116" y="435">
      <linkto id="632503579925120934" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Invalid Request</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="literal">State or City not specified</ap>
        <rd field="ResultData">weatherText</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"State is: " + (queryParams["state"] == null ? "N/A" : queryParams["state"]) + ", City is: " + (queryParams["station"] == null ? "N/A" : queryParams["station"]) </log>
      </Properties>
    </node>
    <node type="Action" id="632503579925120934" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="116" y="534">
      <linkto id="632503579925120935" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120935" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="117" y="633">
      <linkto id="632503579925120939" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120939" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="116" y="733">
      <linkto id="632503579925120932" type="Labeled" style="Bevel" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">weatherText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Comment" id="632785714975213960" text="Set a cookie on the IP phone,&#xD;&#xA;which will be used a preference the &#xD;&#xA;next time the application is used" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="690.9414" y="250" />
    <node type="Action" id="632785794464280089" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="474.9414" y="342">
      <linkto id="632283491418437746" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Refresh</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/DisplayStation?state=" + state + "&amp;station=" + station + "&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632786194365827050" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="935.601563" y="449">
      <linkto id="632786194365827052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT state, station FROM LastChoice WHERE devicename = '" + g_devicename + "'"</ap>
        <ap name="Name" type="literal">preferences</ap>
        <rd field="ResultSet">result</rd>
        <log condition="entry" on="true" level="Info" type="literal">Found preference</log>
      </Properties>
    </node>
    <node type="Action" id="632786194365827052" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1027.58789" y="495">
      <linkto id="632786194365827054" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632786194365827171" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">result.Rows.Count == 1</ap>
      </Properties>
    </node>
    <node type="Action" id="632786194365827054" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1054.61523" y="632">
      <linkto id="632283491418437753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("INSERT into LastChoice (state, station, devicename) VALUES ('{0}', '{1}', '{2}')", state, station, g_devicename);</ap>
        <ap name="Name" type="literal">preferences</ap>
      </Properties>
    </node>
    <node type="Action" id="632786194365827169" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="948.5879" y="343">
      <linkto id="632786194365827050" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632283491418437753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_devicename != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632786194365827171" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="1090.61523" y="426">
      <linkto id="632283491418437753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="csharp">String.Format("update LastChoice set state='{0}', station='{1}' where devicename='{2}'", state, station, g_devicename);</ap>
        <ap name="Name" type="literal">preferences</ap>
      </Properties>
    </node>
    <node type="Variable" id="632283491418437739" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632283491418437740" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632283491418437741" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632283491418437743" name="weatherString" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">weatherString</Properties>
    </node>
    <node type="Variable" id="632283491418437745" name="weatherText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">weatherText</Properties>
    </node>
    <node type="Variable" id="632283491418437760" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632785714975213969" name="state" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" defaultInitWith="NONE" refType="reference">state</Properties>
    </node>
    <node type="Variable" id="632785714975213970" name="station" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" defaultInitWith="NONE" refType="reference">station</Properties>
    </node>
    <node type="Variable" id="632786194365827053" name="result" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">result</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632283491418437757" treenode="632485420083424341" appnode="632283491418437755" handlerfor="632283491418437754">
    <node type="Start" id="632283491418437757" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="258" y="271">
      <linkto id="632283491418437759" type="Basic" style="Bevel" ortho="true" />
    </node>
    <node type="Action" id="632283491418437759" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="618" y="271">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>