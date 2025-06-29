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
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632283491418437674" treenode="632485420083424337" appnode="632283491418437672" handlerfor="632283491418437671">
    <node type="Start" id="632283491418437674" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="352">
      <linkto id="632283491418437677" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283491418437677" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="194" y="353">
      <linkto id="632283491418437678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Weather</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437678" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="371" y="353">
      <linkto id="632283491418437682" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Find a weather station</ap>
        <ap name="URL" type="csharp">host + "/Weather/ChooseState?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437682" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="550" y="354">
      <linkto id="632283491418437683" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="csharp">menu.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632283491418437683" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="740" y="355">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632283491418437684" text="Making menu with one choice: Find a weather station.&#xD;&#xA;Only one choice makes this menu a bit pointless, but &#xD;&#xA;more options could easily be added later." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="504" y="220" />
    <node type="Comment" id="632283491418437686" text="Adding metreosSessionId=[routingGuid] &#xD;&#xA;to keep phone in session" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="243" y="222" />
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
  </canvas>
  <canvas type="Function" name="ChooseCity" activetab="true" startnode="632283491418437710" treenode="632485420083424338" appnode="632283491418437708" handlerfor="632283491418437707">
    <node type="Loop" id="632283491418437731" name="Loop" text="loop (expr)" cx="120" cy="120" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="499" y="301" mx="559" my="361">
      <linkto id="632283491418437732" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632283491418437716" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">stations.Count &gt; 100 ? 100 : stations.Count</Properties>
    </node>
    <node type="Start" id="632283491418437710" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="361">
      <linkto id="632503579925120941" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283491418437716" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="743" y="360">
      <linkto id="632283491418437733" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437720" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="219" y="361">
      <linkto id="632283491418437721" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Listing of stations</ap>
        <ap name="Prompt" type="literal">Choose Station</ap>
        <rd field="ResultData">menu</rd>
        <log condition="entry" on="true" level="Info" type="literal">Creating station list</log>
      </Properties>
    </node>
    <node type="Action" id="632283491418437721" name="GetAllStations" class="MaxActionNode" group="" path="Metreos.Native.Weather" x="362" y="362">
      <linkto id="632283491418437723" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632283491418437731" port="1" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="State" type="csharp">queryParams["state"]</ap>
        <rd field="ResultData">stations</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437723" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="362" y="467">
      <linkto id="632283491418437725" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Weather</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="literal">State could not be found</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437725" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="364" y="567">
      <linkto id="632283491418437726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437726" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="364" y="656">
      <linkto id="632283491418437727" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">errorText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437727" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="365" y="756">
      <linkto id="632283491418437728" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">errorText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437728" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="365" y="854">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632283491418437732" name="AddMenuItem" container="632283491418437731" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="559" y="361">
      <linkto id="632283491418437731" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="csharp">(stations[loopIndex] as string[])[1]</ap>
        <ap name="URL" type="csharp">host + "/Weather/DisplayStation?station=" + (stations[loopIndex] as string[])[0] + "&amp;state=" + queryParams["state"] + "&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437733" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="869.4121" y="359">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632283491418437749" text="GetAllStations returns a listing of station friendly names &#xD;&#xA;and abbreviations.&#xD;&#xA;&#xD;&#xA;Using these string values, construct a menu for the &#xD;&#xA;user to select from.   The friendly name is used for display, &#xD;&#xA;and the station id is used for the station query parameter in&#xD;&#xA;the URL field of each menu item" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="408.4121" y="175" />
    <node type="Action" id="632503579925120941" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="119.173843" y="360">
      <linkto id="632503579925120943" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632283491418437720" type="Labeled" style="Bezier" ortho="true" label="default" />
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
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632503579925120943" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="120.173828" y="451">
      <linkto id="632503579925120944" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Invalid Request</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="literal">State not specified</ap>
        <rd field="ResultData">menu</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"State is: " + (queryParams["state"] == null ? "N/A" : queryParams["state"]) + ", City is: " + (queryParams["station"] == null ? "N/A" : queryParams["station"]) </log>
      </Properties>
    </node>
    <node type="Action" id="632503579925120944" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="120.173828" y="550">
      <linkto id="632503579925120945" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120945" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="121.173828" y="649">
      <linkto id="632503579925120946" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120946" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="120.173828" y="749">
      <linkto id="632503579925120942" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
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
      <linkto id="632284533238906505" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283491418437703" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="449" y="303">
      <linkto id="632283491418437704" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437704" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="571" y="304">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Comment" id="632283491418437748" text="GetStates returns a listing of state abbreviations.&#xD;&#xA;&#xD;&#xA;Using these string values, construct a menu for the &#xD;&#xA;user to select from." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="492.4707" y="132" />
    <node type="Action" id="632284533238906505" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="202" y="305">
      <linkto id="632284533238906508" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Choose state</ap>
        <ap name="Prompt" type="literal">Enter state abbreviation</ap>
        <ap name="URL" type="csharp">host + "/Weather/ChooseCity?metreosSessionId="  + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632284533238906508" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="311" y="303">
      <linkto id="632283491418437703" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
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
    <node type="Variable" id="632283491418437702" name="states" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.ArrayList" refType="reference">states</Properties>
    </node>
    <node type="Variable" id="632284533238906506" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DisplayStation" startnode="632283491418437737" treenode="632485420083424340" appnode="632283491418437735" handlerfor="632283491418437734">
    <node type="Start" id="632283491418437737" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="343">
      <linkto id="632503579925120931" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283491418437742" name="GetWeatherStatus" class="MaxActionNode" group="" path="Metreos.Native.Weather" x="216" y="343">
      <linkto id="632283491418437744" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="State" type="csharp">queryParams["state"]</ap>
        <ap name="Station" type="csharp">queryParams["station"]</ap>
        <rd field="ResultData">weatherString</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437744" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="371" y="343">
      <linkto id="632283491418437746" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Weather Status</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="variable">weatherString</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437746" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="501" y="344">
      <linkto id="632283491418437747" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">MainMenu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632283491418437747" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="637" y="346">
      <linkto id="632283491418437752" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Comment" id="632283491418437750" text="With a state and station chosen,&#xD;&#xA;use GetWeatherStatus to format a &#xD;&#xA;weather string for us, which we pass &#xD;&#xA;back to the phone in a &#xD;&#xA;CiscoIPPhoneText object." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="469" y="149" />
    <node type="Action" id="632283491418437752" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="770" y="345">
      <linkto id="632283491418437753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">weatherText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632283491418437753" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="898.4707" y="345">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632503579925120931" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="115.000008" y="344.000031">
      <linkto id="632283491418437742" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632503579925120933" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams)
{
	string state = queryParams["state"];
	string station = queryParams["station"];

	if(state == null || state == String.Empty) return "failure";
	if(station == null || station == String.Empty) return "failure";
	
	return "success";
}

</Properties>
    </node>
    <node type="Action" id="632503579925120932" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="117" y="832">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632503579925120933" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="116" y="435">
      <linkto id="632503579925120934" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Title" type="literal">Invalid Request</ap>
        <ap name="Prompt" type="literal">Choose option</ap>
        <ap name="Text" type="literal">State or City not specified</ap>
        <rd field="ResultData">weatherText</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"State is: " + (queryParams["state"] == null ? "N/A" : queryParams["state"]) + ", City is: " + (queryParams["station"] == null ? "N/A" : queryParams["station"]) </log>
      </Properties>
    </node>
    <node type="Action" id="632503579925120934" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="116" y="534">
      <linkto id="632503579925120935" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/Weather/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120935" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="117" y="633">
      <linkto id="632503579925120939" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">weatherText</rd>
      </Properties>
    </node>
    <node type="Action" id="632503579925120939" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="116" y="733">
      <linkto id="632503579925120932" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">weatherText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
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
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632283491418437757" treenode="632485420083424341" appnode="632283491418437755" handlerfor="632283491418437754">
    <node type="Start" id="632283491418437757" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="258" y="271">
      <linkto id="632283491418437759" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632283491418437759" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="618" y="271">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>