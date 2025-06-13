<Application name="script1" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632967146751243867" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632967146751243864" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632967146751243863" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ControlRinger/Main</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632968429478798606" level="2" text="Metreos.Providers.Http.GotRequest: ProcessRingSet">
        <node type="function" name="ProcessRingSet" id="632968429478798603" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632968429478798602" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ControlRinger/ProcessRingSet</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_controlledPhone" id="632968429478798539" vid="632967146751243877">
        <Properties type="Metreos.Types.AxlSoap413.GetPhoneResponse">g_controlledPhone</Properties>
      </treenode>
      <treenode text="g_ccmIP" id="632968429478798541" vid="632967146751243896">
        <Properties type="String" initWith="ccmIP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_ccmUser" id="632968429478798543" vid="632967146751243898">
        <Properties type="String" initWith="ccmUser">g_ccmUser</Properties>
      </treenode>
      <treenode text="g_ccmPass" id="632968429478798545" vid="632967146751243900">
        <Properties type="String" initWith="ccmPass">g_ccmPass</Properties>
      </treenode>
      <treenode text="g_controlledLines" id="632968429478798547" vid="632967146751243927">
        <Properties type="ArrayList">g_controlledLines</Properties>
      </treenode>
      <treenode text="g_controlledPhoneName" id="632968429478798614" vid="632968429478798613">
        <Properties type="String">g_controlledPhoneName</Properties>
      </treenode>
      <treenode text="g_thisPhoneName" id="632968429478798616" vid="632968429478798615">
        <Properties type="String">g_thisPhoneName</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632967146751243866" treenode="632967146751243867" appnode="632967146751243864" handlerfor="632967146751243863">
    <node type="Loop" id="632967146751243906" name="Loop" text="loop (expr)" cx="337" cy="212" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="695" y="272" mx="864" my="378">
      <linkto id="632967146751243909" fromport="1" type="Basic" style="Vector" />
      <linkto id="632968429478798575" fromport="3" type="Labeled" style="Vector" label="default" />
      <Properties iteratorType="enum" type="csharp">g_controlledPhone.Response.@return.device.lines.Items</Properties>
    </node>
    <node type="Loop" id="632968429478798578" name="Loop" text="loop (expr)" cx="378" cy="573" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1240" y="16" mx="1429" my="302">
      <linkto id="632968429478798590" fromport="1" type="Basic" style="Vector" />
      <linkto id="632968429478798584" fromport="3" type="Labeled" style="Vector" label="default" />
      <Properties iteratorType="int" type="csharp">g_controlledLines.Count</Properties>
    </node>
    <node type="Start" id="632967146751243866" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="385">
      <linkto id="632967146751243870" type="Basic" style="Vector" />
    </node>
    <node type="Action" id="632967146751243870" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="232" y="384">
      <annot id="632967146751243873" x="308" y="390" text="Check 'device' and&#xD;&#xA;check 'controlled' &#xD;&#xA;query parameter presence" />
      <linkto id="632967146751243876" type="Labeled" style="Vector" label="success" />
      <linkto id="632967146751243915" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection query, ref string errorMsg, ref string g_thisPhoneName, ref string g_controlledPhoneName)
{
	bool failure = false;
	if(query["device"] == null || query["device"] == String.Empty)
	{
		errorMsg = "The 'device' parameter is not set on the service.";
		failure = true;
	}

	if(query["controlled"] == null || query["controlled"] == String.Empty)
	{
		errorMsg = "The 'controlled' parameter is not set on the service.";
		failure = true;
	}

	if(failure)
	{
		return "failure";
	}
	else
	{
		g_thisPhoneName = query["device"];
		g_controlledPhoneName = query["controlled"];
		return "success";
	}
}
</Properties>
    </node>
    <node type="Action" id="632967146751243876" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="408" y="384">
      <linkto id="632967146751243908" type="Labeled" style="Vector" label="success" />
      <linkto id="632967146751243916" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="csharp">query["controlled"]</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetPhoneResponse">g_controlledPhone</rd>
      </Properties>
    </node>
    <node type="Action" id="632967146751243908" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="568" y="384">
      <linkto id="632967146751243906" port="1" type="Labeled" style="Vector" label="haslines" />
      <linkto id="632967146751243921" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetPhoneResponse g_controlledPhone)
{
	if(g_controlledPhone != null &amp;&amp;
		g_controlledPhone.Response.@return != null &amp;&amp;
		g_controlledPhone.Response.@return.device != null &amp;&amp;
		g_controlledPhone.Response.@return.device.lines != null &amp;&amp;
		g_controlledPhone.Response.@return.device.lines.Items != null &amp;&amp;
		g_controlledPhone.Response.@return.device.lines.Items.Length &gt; 0)
	{
		return "haslines";
	}
	else
	{
		return "nolines";
	}
}
</Properties>
    </node>
    <node type="Action" id="632967146751243910" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="232" y="600">
      <linkto id="632967146751243912" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632967146751243912" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="232" y="712">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632967146751243913" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="408" y="616">
      <linkto id="632967146751243918" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632967146751243915" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="232" y="480">
      <linkto id="632967146751243910" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">App Failure</ap>
        <ap name="Prompt" type="literal">Choose an option</ap>
        <ap name="Text" type="variable">errorMsg</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632967146751243916" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="408" y="496">
      <linkto id="632967146751243913" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">App Failure</ap>
        <ap name="Prompt" type="literal">Choose an option</ap>
        <ap name="Text" type="literal">Unable to use AXL to retrieve the controlled phone</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632967146751243918" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="408" y="712">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632967146751243920" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="568" y="616">
      <linkto id="632967146751243922" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632967146751243921" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="568" y="496">
      <linkto id="632967146751243920" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Done</ap>
        <ap name="Prompt" type="literal">Choose an option</ap>
        <ap name="Text" type="literal">No lines are configured on the controlled device</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632967146751243922" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="568" y="712">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632967146751243930" name="CustomCode" container="632967146751243906" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="922" y="380">
      <linkto id="632967146751243906" port="3" type="Labeled" style="Vector" label="success" />
      <linkto id="632968429478798525" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetLineResponse line, ArrayList g_controlledLines, IEnumerator loopEnum, LogWriter log)
{
	Metreos.AxlSoap413.XLine xLine = loopEnum.Current as Metreos.AxlSoap413.XLine;
	string index = xLine.index;
	string uuid = xLine.uuid;
	string label = xLine.label;
	string ringSetting = xLine.ringSetting.ToString();
	string consecutiveRingSetting = xLine.consecutiveRingSetting.ToString();

	string pattern = null;

	if(line != null &amp;&amp;
		line.Response.@return != null &amp;&amp;
		line.Response.@return.directoryNumber != null)
	{
		pattern = line.Response.@return.directoryNumber.pattern;

		g_controlledLines.Add(new string[] {uuid, pattern, label, index, ringSetting, consecutiveRingSetting});

		log.Write(TraceLevel.Info, "Uuid: {0}, Pattern: {1}, Label: {2}, Index: {3}, RingSetting: {4}, ConsecRingSetting: {5}", uuid, pattern, label, index, ringSetting, consecutiveRingSetting);
 
		return "success";
	}
	else
	{
		return "failure";
	}
}
</Properties>
    </node>
    <node type="Action" id="632968429478798524" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="864" y="699">
      <linkto id="632968429478798526" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798525" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="864" y="579">
      <linkto id="632968429478798524" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Done</ap>
        <ap name="Prompt" type="literal">Choose an option</ap>
        <ap name="Text" type="literal">Unable to use AXL to retrieve the controlled phone's lines</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798526" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="864" y="795">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632967146751243909" name="GetLine" container="632967146751243906" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="793" y="379">
      <linkto id="632967146751243930" type="Labeled" style="Vector" label="success" />
      <linkto id="632968429478798525" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">(loopEnum.Current as Metreos.AxlSoap413.XLine).Item.uuid</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
        <rd field="GetLineResponse">line</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798575" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1132.14063" y="374">
      <linkto id="632968429478798578" port="1" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Toggle Ring Setting</ap>
        <ap name="Prompt" type="literal">Choose Line</ap>
        <rd field="ResultData">menuXml</rd>
      </Properties>
    </node>
    <node type="Comment" id="632968429478798576" text="XRingSetting:&#xD;&#xA;&#xD;&#xA;/// &lt;remarks/&gt;&#xD;&#xA;		[System.Xml.Serialization.XmlEnumAttribute(&quot;Use System Default&quot;)]&#xD;&#xA;		UseSystemDefault,&#xD;&#xA;    &#xD;&#xA;		/// &lt;remarks/&gt;&#xD;&#xA;		Disable,&#xD;&#xA;    &#xD;&#xA;		/// &lt;remarks/&gt;&#xD;&#xA;		[System.Xml.Serialization.XmlEnumAttribute(&quot;Flash Only&quot;)]&#xD;&#xA;		FlashOnly,&#xD;&#xA;    &#xD;&#xA;		/// &lt;remarks/&gt;&#xD;&#xA;		[System.Xml.Serialization.XmlEnumAttribute(&quot;Ring Once&quot;)]&#xD;&#xA;		RingOnce,&#xD;&#xA;    &#xD;&#xA;		/// &lt;remarks/&gt;&#xD;&#xA;		Ring,&#xD;&#xA;    &#xD;&#xA;		/// &lt;remarks/&gt;&#xD;&#xA;		[System.Xml.Serialization.XmlEnumAttribute(&quot;Beep Only&quot;)]&#xD;&#xA;		BeepOnly," class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1728" y="408" />
    <node type="Action" id="632968429478798579" name="Switch" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1416" y="352">
      <linkto id="632968429478798580" type="Labeled" style="Vector" label="UseSystemDefault" />
      <linkto id="632968429478798592" type="Labeled" style="Vector" label="Disable" />
      <linkto id="632968429478798594" type="Labeled" style="Vector" label="FlashOnly" />
      <linkto id="632968429478798596" type="Labeled" style="Vector" label="RingOnce" />
      <linkto id="632968429478798598" type="Labeled" style="Vector" label="Ring" />
      <linkto id="632968429478798600" type="Labeled" style="Vector" label="BeepOnly" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">(g_controlledLines[loopIndex] as string[])[4]</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798580" name="AddMenuItem" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1552" y="72">
      <linkto id="632968429478798578" port="3" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">displayLineName + " (" + "On" + ")"</ap>
        <ap name="URL" type="csharp">"http://" + host + "/ControlRinger/ProcessRingSet?metreosSessionId=" + routingGuid + "&amp;arrayIndex=" + loopIndex + "&amp;currentState=On"</ap>
        <rd field="ResultData">menuXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798582" name="Assign" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1384" y="288">
      <linkto id="632968429478798579" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">(currentLabel == null || currentLabel == String.Empty) ? currentPattern : currentLabel</ap>
        <rd field="ResultData">displayLineName</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798584" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1800" y="312">
      <linkto id="632968429478798587" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menuXml.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798587" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="2128" y="312">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632968429478798590" name="Assign" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1312" y="352">
      <linkto id="632968429478798582" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">(g_controlledLines[loopIndex] as string[])[1]</ap>
        <ap name="Value2" type="csharp">(g_controlledLines[loopIndex] as string[])[2]</ap>
        <rd field="ResultData">currentPattern</rd>
        <rd field="ResultData2">currentLabel</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798592" name="AddMenuItem" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1544" y="216">
      <linkto id="632968429478798578" port="3" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">displayLineName + " (" + "Off" + ")"</ap>
        <ap name="URL" type="csharp">"http://" + host + "/ControlRinger/ProcessRingSet?metreosSessionId=" + routingGuid + "&amp;arrayIndex=" + loopIndex + "&amp;currentState=Off"</ap>
        <rd field="ResultData">menuXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798594" name="AddMenuItem" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1544" y="312">
      <linkto id="632968429478798578" port="3" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">displayLineName + " (" + "Off" + ")"</ap>
        <ap name="URL" type="csharp">"http://" + host + "/ControlRinger/ProcessRingSet?metreosSessionId=" + routingGuid + "&amp;arrayIndex=" + loopIndex + "&amp;currentState=Off"</ap>
        <rd field="ResultData">menuXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798596" name="AddMenuItem" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1544" y="392">
      <linkto id="632968429478798578" port="3" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">displayLineName + " (" + "On" + ")"</ap>
        <ap name="URL" type="csharp">"http://" + host + "/ControlRinger/ProcessRingSet?metreosSessionId=" + routingGuid + "&amp;arrayIndex=" + loopIndex + "&amp;currentState=On"</ap>
        <rd field="ResultData">menuXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798598" name="AddMenuItem" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1536" y="456">
      <linkto id="632968429478798578" port="3" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">displayLineName + " (" + "On" + ")"</ap>
        <ap name="URL" type="csharp">"http://" + host + "/ControlRinger/ProcessRingSet?metreosSessionId=" + routingGuid + "&amp;arrayIndex=" + loopIndex + "&amp;currentState=On"</ap>
        <rd field="ResultData">menuXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798600" name="AddMenuItem" container="632968429478798578" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1536" y="544">
      <linkto id="632968429478798578" port="3" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">displayLineName + " (" + "Off" + ")"</ap>
        <ap name="URL" type="csharp">"http://" + host + "/ControlRinger/ProcessRingSet?metreosSessionId=" + routingGuid + "&amp;arrayIndex=" + loopIndex + "&amp;currentState=Off"</ap>
        <rd field="ResultData">menuXml</rd>
      </Properties>
    </node>
    <node type="Variable" id="632967146751243868" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632967146751243869" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632967146751243871" name="errorMsg" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMsg</Properties>
    </node>
    <node type="Variable" id="632967146751243911" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632967146751243926" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632967146751243929" name="line" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetLineResponse" refType="reference">line</Properties>
    </node>
    <node type="Variable" id="632968429478798577" name="menuXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" initWith="" refType="reference">menuXml</Properties>
    </node>
    <node type="Variable" id="632968429478798583" name="displayLineName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">displayLineName</Properties>
    </node>
    <node type="Variable" id="632968429478798585" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632968429478798588" name="currentLabel" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">currentLabel</Properties>
    </node>
    <node type="Variable" id="632968429478798589" name="currentPattern" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">currentPattern</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ProcessRingSet" activetab="true" startnode="632968429478798605" treenode="632968429478798606" appnode="632968429478798603" handlerfor="632968429478798602">
    <node type="Start" id="632968429478798605" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="304">
      <linkto id="632968429478798612" type="Basic" style="Vector" />
    </node>
    <node type="Comment" id="632968429478798608" text="currentState&#xD;&#xA;arrayIndex" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="128" y="120" />
    <node type="Action" id="632968429478798610" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="360" y="304">
      <linkto id="632968429478798619" type="Labeled" style="Vector" label="success" />
      <linkto id="632968429478798620" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_controlledPhoneName</ap>
        <ap name="Lines" type="variable">lines</ap>
        <ap name="CallManagerIP" type="variable">g_ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmPass</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798612" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="200" y="304">
      <linkto id="632968429478798610" type="Labeled" style="Vector" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.Lines lines, Metreos.Types.Http.QueryParamCollection query, Metreos.Types.AxlSoap413.GetPhoneResponse g_controlledPhone)
{
		
	lines.Data = new Metreos.AxlSoap413.XLine[g_controlledPhone.Response.@return.device.lines.Items.Length];
	g_controlledPhone.Response.@return.device.lines.Items.CopyTo(lines.Data, 0);
	
	int index = Convert.ToInt32(query["arrayIndex"]); 

	string currentState = query["currentState"].ToLower();

	if(currentState == "on")
	{
		// turn it off	
		lines.Data[index].ringSetting = Metreos.AxlSoap413.XRingSetting.Disable;
		lines.Data[index].consecutiveRingSetting = Metreos.AxlSoap413.XRingSetting.Disable;	
	}
	else
	{
		lines.Data[index].ringSetting = Metreos.AxlSoap413.XRingSetting.UseSystemDefault;
		lines.Data[index].consecutiveRingSetting = Metreos.AxlSoap413.XRingSetting.Ring;	
	}

	return "success";
	 
}
</Properties>
    </node>
    <node type="Action" id="632968429478798618" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="504" y="448">
      <linkto id="632968429478798622" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632968429478798619" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="504" y="304">
      <linkto id="632968429478798618" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">"App Done"</ap>
        <ap name="Text" type="csharp">"Changed state of line to " + ((query["currentState"].ToLower() == "on") ? "Off" : "On")</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798620" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="360" y="448">
      <linkto id="632968429478798618" type="Labeled" style="Vector" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="csharp">"App Failure"</ap>
        <ap name="Text" type="csharp">"Unable to update the controlled phone using AXL"</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632968429478798622" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="608" y="576">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632968429478798607" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632968429478798611" name="lines" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.Lines" initWith="" refType="reference">lines</Properties>
    </node>
    <node type="Variable" id="632968429478798617" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632968429478798623" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>