<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632968429478798630" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632968429478798627" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632968429478798626" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Whisper/Main</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632968429478798674" level="2" text="Metreos.Providers.Http.GotRequest: Start">
        <node type="function" name="Start" id="632968429478798671" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632968429478798670" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/Whisper/Start</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_phoneUser" id="632969275065461720" vid="632968429478798641">
        <Properties type="String" initWith="PhoneUsername">g_phoneUser</Properties>
      </treenode>
      <treenode text="g_phonePass" id="632969275065461722" vid="632968429478798643">
        <Properties type="String" initWith="PhonePassword">g_phonePass</Properties>
      </treenode>
      <treenode text="g_phoneList" id="632969275065461724" vid="632968429478798645">
        <Properties type="ArrayList" initWith="PageList">g_phoneList</Properties>
      </treenode>
      <treenode text="g_phoneDevicename" id="632969275065461726" vid="632968429478798647">
        <Properties type="String">g_phoneDevicename</Properties>
      </treenode>
      <treenode text="g_associateServices" id="632969275065461728" vid="632969275065461706">
        <Properties type="Bool" initWith="AssociateService">g_associateServices</Properties>
      </treenode>
      <treenode text="g_delayService" id="632969275065461779" vid="632969275065461778">
        <Properties type="Bool" initWith="AssociateService">g_delayService</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632968429478798629" treenode="632968429478798630" appnode="632968429478798627" handlerfor="632968429478798626">
    <node type="Loop" id="632968429478798649" name="Loop" text="loop (var)" cx="241.589233" cy="213" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="688" y="232" mx="809" my="338">
      <linkto id="632968429478798665" fromport="1" type="Basic" style="Vector" />
      <linkto id="632968429478798668" fromport="3" type="Labeled" style="Vector" label="default" />
      <Properties iteratorType="enum" type="variable">g_phoneList</Properties>
    </node>
    <node type="Start" id="632968429478798629" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="336">
      <linkto id="632968429478798640" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632968429478798640" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="248" y="336">
      <linkto id="632968429478798650" type="Labeled" style="Vector" label="success" />
      <linkto id="632968429478798658" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection query, ref string g_phoneDevicename, ref string errorMsg, LogWriter log)
{
	g_phoneDevicename = query["device"];

	if(g_phoneDevicename != null &amp;&amp; g_phoneDevicename != String.Empty)
	{
		return "success";
	}
	else
	{
		errorMsg = "No 'device' query parameter defined";
		log.Write(TraceLevel.Error, errorMsg);
		return "failure";
	}
}
</Properties>
    </node>
    <node type="Action" id="632968429478798650" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="424" y="336">
      <linkto id="632968429478798655" type="Labeled" style="Vector" label="true" />
      <linkto id="632968429478798664" type="Labeled" style="Vector" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_phoneList.Count == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798655" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="424" y="448">
      <linkto id="632968429478798656" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Config Error</ap>
        <ap name="Text" type="literal">No devices configured in page list</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798656" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="424" y="552">
      <linkto id="632968429478798657" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798657" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="424" y="664">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798658" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="248" y="456">
      <linkto id="632968429478798659" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Config Error</ap>
        <ap name="Text" type="variable">errorMsg</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798659" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="248" y="560">
      <linkto id="632968429478798660" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798660" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="248" y="672">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798664" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="576" y="336">
      <linkto id="632968429478798649" port="1" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Page List</ap>
        <ap name="Prompt" type="literal">Choose device</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798665" name="AddMenuItem" container="632968429478798649" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="816" y="336">
      <linkto id="632968429478798649" port="3" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">loopEnum.Current as string</ap>
        <ap name="URL" type="csharp">"http://" + host + "/Whisper/Start?metreosSessionId=" + routingGuid + "&amp;pageto=" + loopEnum.Current as string</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798668" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1056" y="336">
      <linkto id="632968429478798669" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798669" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1176" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632968429478798639" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632968429478798651" name="errorMsg" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMsg</Properties>
    </node>
    <node type="Variable" id="632968429478798652" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632968429478798653" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632968429478798654" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632968429478798666" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632968429478798667" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Start" activetab="true" startnode="632968429478798673" treenode="632968429478798674" appnode="632968429478798671" handlerfor="632968429478798670">
    <node type="Start" id="632968429478798673" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="80" y="280">
      <linkto id="632968429478798677" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632968429478798676" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="672" y="416">
      <linkto id="632968429478798699" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">g_associateServices &amp;&amp; !g_delayService ? ("http://" + host + "/Whisper/Display?homeDevice=" + g_phoneDevicename + "&amp;pageto=" + query["pageto"]) : null</ap>
        <ap name="URL2" type="csharp">"RTPRx:" + remoteIP + ":" + chosenPort</ap>
        <rd field="ResultData">execute</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"RTPRx:" + remoteIP + ":" + chosenPort</log>
      </Properties>
    </node>
    <node type="Action" id="632968429478798677" name="QueryByDevice" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="216" y="280">
      <linkto id="632968429478798679" type="Labeled" style="Vector" label="default" />
      <linkto id="632968429478798688" type="Labeled" style="Vector" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="csharp">query["pageto"]</ap>
        <rd field="ResultData">deviceResult</rd>
        <rd field="Count">resultCount</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798679" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="216" y="400">
      <linkto id="632968429478798684" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">App Error</ap>
        <ap name="Text" type="literal">Unable to use real-time cache</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798684" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="216" y="520">
      <linkto id="632968429478798685" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798685" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="216" y="624">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798688" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="384" y="280">
      <linkto id="632968429478798689" type="Labeled" style="Vector" label="equal" />
      <linkto id="632968429478798697" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">resultCount</ap>
        <ap name="Value2" type="csharp">0</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798689" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="384" y="408">
      <linkto id="632968429478798690" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Config Error</ap>
        <ap name="Text" type="literal">Could not find IP address of phone in real-time cache</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798690" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="384" y="528">
      <linkto id="632968429478798691" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798691" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="384" y="632">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798697" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="528" y="280">
      <linkto id="632968429478798702" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(ref int chosenPort)
{
	// playin a little safe within advertised range
	chosenPort = new System.Random(System.Environment.TickCount).Next(20482, 32000);

	if(chosenPort % 2 == 1)
	{
		chosenPort++;
	}

	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632968429478798699" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="792" y="416">
      <linkto id="632968429478799310" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="csharp">deviceResult.IP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798700" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="672" y="560">
      <linkto id="632968429478798704" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"RTPTx:" + deviceResult.IP + ":" + chosenPort</ap>
        <rd field="ResultData">execute2</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"RTPTx:" + deviceResult.IP + ":" + chosenPort</log>
      </Properties>
    </node>
    <node type="Action" id="632968429478798702" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="664" y="280">
      <linkto id="632968429478798703" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Paging...</ap>
        <ap name="Text" type="literal">Page</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798703" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="792" y="280">
      <linkto id="632968429478798676" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798704" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="800" y="560">
      <linkto id="632969275065461780" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute2.ToString()</ap>
        <ap name="URL" type="csharp">remoteIP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798707" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="944" y="560">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478799310" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="542" y="557.0911">
      <linkto id="632968429478798700" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632969275065461780" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="936" y="408.091125">
      <linkto id="632968429478798707" type="Labeled" style="Vector" label="false" />
      <linkto id="632969275065461781" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_associateServices &amp;&amp; g_delayService</ap>
      </Properties>
    </node>
    <node type="Action" id="632969275065461781" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1009" y="413">
      <linkto id="632969275065461782" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"http://" + host + "/Whisper/Display?homeDevice=" + g_phoneDevicename + "&amp;pageto=" + query["pageto"]</ap>
        <rd field="ResultData">execute</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"RTPRx:" + remoteIP + ":" + chosenPort</log>
      </Properties>
    </node>
    <node type="Action" id="632969275065461782" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1129" y="413">
      <linkto id="632968429478798707" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">execute.ToString()</ap>
        <ap name="URL" type="csharp">deviceResult.IP</ap>
        <ap name="Username" type="variable">g_phoneUser</ap>
        <ap name="Password" type="variable">g_phonePass</ap>
      </Properties>
    </node>
    <node type="Variable" id="632968429478798675" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632968429478798678" name="deviceResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoDeviceList.Device" refType="reference">deviceResult</Properties>
    </node>
    <node type="Variable" id="632968429478798680" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632968429478798681" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632968429478798682" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632968429478798683" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632968429478798686" name="resultCount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">resultCount</Properties>
    </node>
    <node type="Variable" id="632968429478798695" name="execute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute</Properties>
    </node>
    <node type="Variable" id="632968429478798696" name="remoteIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteIP</Properties>
    </node>
    <node type="Variable" id="632968429478798698" name="chosenPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">chosenPort</Properties>
    </node>
    <node type="Variable" id="632968429478798706" name="execute2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">execute2</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>