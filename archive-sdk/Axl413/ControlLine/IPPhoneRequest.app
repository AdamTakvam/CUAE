<Application name="IPPhoneRequest" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IPPhoneRequest">
    <outline>
      <treenode type="evh" id="632892518002371259" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632892518002371256" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632892518002371255" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SetLines/Start</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632892525355101340" level="2" text="Metreos.Providers.Http.GotRequest: ListLines">
        <node type="function" name="ListLines" id="632892525355101337" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632892525355101336" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SetLines/ListLines</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632892525355101449" level="2" text="Metreos.Providers.Http.GotRequest: Operation">
        <node type="function" name="Operation" id="632892525355101446" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632892525355101445" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/SetLines/Operation</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632892525355101332" level="1" text="ListLinesWorker">
        <node type="function" name="ListLinesWorker" id="632892525355101329" path="Metreos.StockTools" />
        <calls>
          <ref actid="632892525355101328" />
          <ref actid="632892525355101349" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="CcmAdministrator" id="632900491472100027" vid="632892518002371284">
        <Properties type="String" initWith="CcmAdmin">CcmAdministrator</Properties>
      </treenode>
      <treenode text="CcmPassword" id="632900491472100029" vid="632892518002371286">
        <Properties type="String" initWith="CcmPassword">CcmPassword</Properties>
      </treenode>
      <treenode text="CcmPublisherIP" id="632900491472100031" vid="632892518002371308">
        <Properties type="String" initWith="CcmPubIp">CcmPublisherIP</Properties>
      </treenode>
      <treenode text="g_getPhoneResponse" id="632900491472100033" vid="632892525355101293">
        <Properties type="Metreos.Types.AxlSoap413.GetPhoneResponse">g_getPhoneResponse</Properties>
      </treenode>
      <treenode text="g_numLines" id="632900491472100035" vid="632892525355101295">
        <Properties type="Int">g_numLines</Properties>
      </treenode>
      <treenode text="g_device" id="632900491472100037" vid="632892525355101333">
        <Properties type="String">g_device</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632892518002371258" treenode="632892518002371259" appnode="632892518002371256" handlerfor="632892518002371255">
    <node type="Start" id="632892518002371258" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="440">
      <linkto id="632892518002371264" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632892518002371262" text="This application can easily make manipulations to any of the three common line settings:&#xD;&#xA;&#xD;&#xA;Call Forward All &#xD;&#xA;DoNotDisturb (FowardtoVoicemail)&#xD;&#xA;RingSetting&#xD;&#xA;&#xD;&#xA;By making use of a query parameter in the service URL, we can allow a CCM administrator&#xD;&#xA;to tailor down the overall purpose of the application." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="154" y="120" />
    <node type="Action" id="632892518002371263" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="640" y="440">
      <linkto id="632892525355101317" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632892525355101328" type="Labeled" style="Bezier" label="cfa" />
      <linkto id="632892525355101328" type="Labeled" style="Bezier" label="dnd" />
      <linkto id="632892525355101328" type="Labeled" style="Bezier" label="ring" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">type</ap>
      </Properties>
    </node>
    <node type="Action" id="632892518002371264" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="176" y="440">
      <linkto id="632892518002371267" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">query["type"]</ap>
        <ap name="Value2" type="csharp">query["device"]</ap>
        <rd field="ResultData">type</rd>
        <rd field="ResultData2">g_device</rd>
      </Properties>
    </node>
    <node type="Action" id="632892518002371267" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="313" y="440">
      <linkto id="632892525355101292" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632892525355101298" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_device</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
        <rd field="GetPhoneResponse">g_getPhoneResponse</rd>
      </Properties>
    </node>
    <node type="Comment" id="632892525355101290" text="GetPhone allows us to pull every&#xD;&#xA;line belonging to this phone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="458" y="644" />
    <node type="Action" id="632892525355101292" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="480" y="440">
      <linkto id="632892525355101305" type="Labeled" style="Bezier" ortho="true" label="0" />
      <linkto id="632892518002371263" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap413.GetPhoneResponse g_getPhoneResponse, ref int g_numLines)
	{
		g_numLines = 0;

		// if we can find no line information, then this device must have
            // no lines, and so is not useful for this demo.
		if(g_getPhoneResponse.Response.@return.device.lines != null &amp;&amp;
               g_getPhoneResponse.Response.@return.device.lines.Items != null)
            {
                 g_numLines = g_getPhoneResponse.Response.@return.device.lines.Items.Length;
            }

		return g_numLines.ToString();
	}</Properties>
    </node>
    <node type="Comment" id="632892525355101297" text="Determine how many lines&#xD;&#xA;are on the device" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="424" y="320" />
    <node type="Label" id="632892525355101298" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="312" y="576" />
    <node type="Label" id="632892525355101299" text="e" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="328" y="808">
      <linkto id="632892525355101300" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632892525355101300" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="456" y="808">
      <linkto id="632892525355101304" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Service Unavailable</ap>
        <ap name="Text" type="literal">Unable to process your request at this time.

Please try again later.</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101302" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="760" y="808">
      <linkto id="632892525355101303" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101303" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="888" y="808">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632892525355101304" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="600" y="808">
      <linkto id="632892525355101302" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Label" id="632892525355101305" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="480" y="568" />
    <node type="Label" id="632892525355101306" text="n" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="328" y="880">
      <linkto id="632892525355101307" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632892525355101307" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="456" y="880">
      <linkto id="632892525355101310" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Configuration Error</ap>
        <ap name="Text" type="literal">This device has no lines.</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101308" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="760" y="880">
      <linkto id="632892525355101309" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101309" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="888" y="880">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632892525355101310" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="600" y="880">
      <linkto id="632892525355101308" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101317" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="776" y="552">
      <linkto id="632892525355101319" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Line Operations</ap>
        <ap name="Prompt" type="literal">Make a selection</ap>
        <rd field="ResultData">options</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101319" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="920" y="552">
      <linkto id="632892525355101322" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Call Forward All</ap>
        <ap name="URL" type="csharp">"http://" + host + "/SetLines/ListLines?type=cfa&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">options</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101322" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1056" y="552">
      <linkto id="632892525355101324" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Do Not Disturb</ap>
        <ap name="URL" type="csharp">"http://" + host + "/SetLines/ListLines?type=dnd&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">options</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101324" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1200" y="552">
      <linkto id="632892525355101326" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Ring Setting</ap>
        <ap name="URL" type="csharp">"http://" + host + "/SetLines/ListLines?type=ring&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">options</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101326" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1360" y="552">
      <linkto id="632892525355101327" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">options.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101327" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1496" y="552">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632892525355101328" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="774.0872" y="288" mx="821" my="304">
      <items count="1">
        <item text="ListLinesWorker" />
      </items>
      <linkto id="632892525355101335" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="host" type="variable">host</ap>
        <ap name="routingGuid" type="variable">routingGuid</ap>
        <ap name="query" type="variable">query</ap>
        <ap name="FunctionName" type="literal">ListLinesWorker</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101335" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1000" y="304">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632892518002371260" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632892518002371261" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632892518002371265" name="type" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">type</Properties>
    </node>
    <node type="Variable" id="632892525355101301" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632892525355101318" name="options" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">options</Properties>
    </node>
    <node type="Variable" id="632892525355101320" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632892525355101321" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ListLines" startnode="632892525355101339" treenode="632892525355101340" appnode="632892525355101337" handlerfor="632892525355101336">
    <node type="Start" id="632892525355101339" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="440">
      <linkto id="632892525355101349" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632892525355101349" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="216" y="424" mx="263" my="440">
      <items count="1">
        <item text="ListLinesWorker" />
      </items>
      <linkto id="632892525355101351" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="host" type="variable">host</ap>
        <ap name="routingGuid" type="variable">routingGuid</ap>
        <ap name="query" type="variable">query</ap>
        <ap name="FunctionName" type="literal">ListLinesWorker</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101351" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="464" y="440">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632892525355101341" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632892525355101342" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632892525355101343" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632892525355101344" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="routingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Operation" startnode="632892525355101448" treenode="632892525355101449" appnode="632892525355101446" handlerfor="632892525355101445">
    <node type="Start" id="632892525355101448" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="109" y="317">
      <linkto id="632892525355101451" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632892525355101451" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="259" y="319">
      <linkto id="632892525355101453" type="Labeled" style="Bezier" ortho="true" label="dnd" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">query["type"]</ap>
      </Properties>
    </node>
    <node type="Comment" id="632892525355101452" text="CFA - users are accustomed to a toggle operation.  So we get the line, and see if it's forwarded.  If it is, undo it.  If it's not, prompt for new CFA number.&#xD;&#xA;&#xD;&#xA;DND - a toggle operation makes sense for DND (if ForwardToVoicemail is checked, uncheck it and vice versa)&#xD;&#xA;&#xD;&#xA;RingSetting - a toggle operation probably doesn't make sense since it is there is one-beep, no noise, ring, and system default.  &#xD;&#xA;You could however make it configurable how it operates to satifsy the whims of more customers without going back to the app.&#xD;&#xA;&#xD;&#xA;&#xD;&#xA;For any operation, we modify the line text label if we have moved off of the default&#xD;&#xA;&#xD;&#xA;TODO DEFINE CFA AND RING" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="136" y="72" />
    <node type="Action" id="632892525355101453" name="GetLine" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="480" y="320">
      <linkto id="632892525355101456" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">((Metreos.AxlSoap413.XLine) g_getPhoneResponse.Response.@return.device.lines.Items[int.Parse(query["index"])]).Item.uuid</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
        <rd field="GetLineResponse">getLineResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101455" name="UpdateLine" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="920" y="232">
      <linkto id="632892525355101475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">((Metreos.AxlSoap413.XLine) g_getPhoneResponse.Response.@return.device.lines.Items[int.Parse(query["index"])]).Item.uuid</ap>
        <ap name="CallForwardAll" type="variable">forward</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101456" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="624" y="320">
      <linkto id="632892525355101458" type="Labeled" style="Bezier" ortho="true" label="dnd" />
      <linkto id="632892525355101459" type="Labeled" style="Bezier" ortho="true" label="no_dnd" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetLineResponse getLineResponse)
{
	if(getLineResponse.Response.@return.directoryNumber.callForwardAll != null &amp;&amp;
		getLineResponse.Response.@return.directoryNumber.callForwardAll.forwardToVoiceMailSpecified &amp;&amp;
getLineResponse.Response.@return.directoryNumber.callForwardAll.forwardToVoiceMail)
	{
		return "dnd";
	}
	else
	{
		return "no_dnd";
	}
}
</Properties>
    </node>
    <node type="Comment" id="632892525355101457" text="Check if DND is set" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="560" y="248" />
    <node type="Action" id="632892525355101458" name="CreateForward" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="768" y="232">
      <linkto id="632892525355101455" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ForwardToVoiceMail" type="csharp">false</ap>
        <rd field="Forward">forward</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101459" name="CreateForward" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="768" y="424">
      <linkto id="632892525355101466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ForwardToVoiceMail" type="csharp">true</ap>
        <rd field="Forward">forward</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101461" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1776" y="328">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632892525355101463" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1656" y="328">
      <linkto id="632892525355101461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101464" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1432" y="424">
      <linkto id="632892525355101470" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">DND On</ap>
        <ap name="Prompt" type="literal">Make a selection</ap>
        <ap name="Text" type="literal">DND set on line</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101466" name="UpdateLine" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="920" y="424">
      <linkto id="632892525355101477" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">((Metreos.AxlSoap413.XLine) g_getPhoneResponse.Response.@return.device.lines.Items[int.Parse(query["index"])]).Item.uuid</ap>
        <ap name="CallForwardAll" type="variable">forward</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101468" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1424" y="232">
      <linkto id="632892525355101470" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">DND Off</ap>
        <ap name="Prompt" type="literal">Make a selection</ap>
        <ap name="Text" type="literal">Line restored to default</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101470" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1544" y="328">
      <linkto id="632892525355101463" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Comment" id="632892525355101472" text="Changing the line label is usually a requirement of an app like this,&#xD;&#xA;so that the admin can see the changes at a glance.  However, &#xD;&#xA;it does incur 2 more AXL writes.  For a very large deployment, maybe &#xD;&#xA;one would want to make this configurable off, and address the issue&#xD;&#xA;of checking a client's line status a different way. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1112" y="72" />
    <node type="Action" id="632892525355101473" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1136" y="232">
      <linkto id="632892525355101483" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_device</ap>
        <ap name="Lines" type="variable">lines</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101475" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1024" y="232">
      <linkto id="632892525355101473" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetPhoneResponse g_getPhoneResponse, Metreos.Types.AxlSoap413.Lines lines, Metreos.Types.Http.QueryParamCollection query)
{
	// Checking for any nulls leading up to the lines structure
	
	// Initialize the line data structure
	Metreos.AxlSoap413.XLine[] lineData = new Metreos.AxlSoap413.XLine[g_getPhoneResponse.Response.@return.device.lines.Items.Length];

	Array.Copy(g_getPhoneResponse.Response.@return.device.lines.Items,
			lineData, lineData.Length);

	int index = int.Parse(query["index"]);

	string existingLabel = ((Metreos.AxlSoap413.XLine) g_getPhoneResponse.Response.@return.device.lines.Items[index]).label;

	existingLabel = existingLabel.Replace("DND - ", String.Empty);

   	lineData[index].label  = existingLabel;

	

	lines.Data = lineData;

	return "success";
}

</Properties>
    </node>
    <node type="Action" id="632892525355101476" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1160" y="424">
      <linkto id="632892525355101481" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_device</ap>
        <ap name="Lines" type="variable">lines</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101477" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1048" y="424">
      <linkto id="632892525355101476" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.AxlSoap413.GetPhoneResponse g_getPhoneResponse, Metreos.Types.AxlSoap413.Lines lines, Metreos.Types.Http.QueryParamCollection query, Metreos.Types.AxlSoap413.GetLineResponse getLineResponse)
{
	// Checking for any nulls leading up to the lines structure
	
	// Initialize the line data structure
	Metreos.AxlSoap413.XLine[] lineData = new Metreos.AxlSoap413.XLine[g_getPhoneResponse.Response.@return.device.lines.Items.Length];

	Array.Copy(g_getPhoneResponse.Response.@return.device.lines.Items,
			lineData, lineData.Length);

	int index = int.Parse(query["index"]);

	string existingLabel = ((Metreos.AxlSoap413.XLine) g_getPhoneResponse.Response.@return.device.lines.Items[index]).label;

		if(existingLabel == null || existingLabel == String.Empty)		
	{
		// If you find no label, then the line number is used for the label by default... we mimic then by taking this behavior into account
		// A side effect of this is that we will set the label to the pattern
		// when the unset the DND
		existingLabel = getLineResponse.Response.@return.directoryNumber.pattern;
	}	

   	lineData[index].label  = "DND - " + existingLabel;

	lines.Data = lineData;

	return "success";
}

</Properties>
    </node>
    <node type="Action" id="632892525355101481" name="DoDeviceReset" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1304" y="424">
      <linkto id="632892525355101464" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_device</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
        <ap name="IsHardReset" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101483" name="DoDeviceReset" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1288" y="232">
      <linkto id="632892525355101468" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">g_device</ap>
        <ap name="CallManagerIP" type="variable">CcmPublisherIP</ap>
        <ap name="AdminUsername" type="variable">CcmAdministrator</ap>
        <ap name="AdminPassword" type="variable">CcmPassword</ap>
        <ap name="IsHardReset" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Variable" id="632892525355101450" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632892525355101454" name="getLineResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetLineResponse" refType="reference">getLineResponse</Properties>
    </node>
    <node type="Variable" id="632892525355101460" name="forward" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">forward</Properties>
    </node>
    <node type="Variable" id="632892525355101465" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632892525355101471" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632892525355101474" name="lines" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.Lines" refType="reference">lines</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ListLinesWorker" startnode="632892525355101331" treenode="632892525355101332" appnode="632892525355101329" handlerfor="632892525355101445">
    <node type="Loop" id="632892525355101352" name="Loop" text="loop (var)" cx="268" cy="283" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="400" y="312" mx="534" my="454">
      <linkto id="632892525355101354" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632892525355101363" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">g_numLines</Properties>
    </node>
    <node type="Start" id="632892525355101331" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="456">
      <linkto id="632892525355101356" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632892525355101354" name="AddMenuItem" container="632892525355101352" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="528" y="456">
      <linkto id="632892525355101352" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">" Line"</ap>
        <ap name="URL" type="csharp">"http://" + host + "/SetLines/Operation?type=" + query["type"] + "&amp;metreosSessionId=" + routingGuid + "&amp;index=" + count</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101355" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="264" y="272">
      <linkto id="632892525355101352" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Call Forward</ap>
        <ap name="Prompt" type="literal">Select a line</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101356" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="136" y="456">
      <linkto id="632892525355101355" type="Labeled" style="Bezier" ortho="true" label="cfa" />
      <linkto id="632892525355101359" type="Labeled" style="Bezier" ortho="true" label="ring" />
      <linkto id="632892525355101357" type="Labeled" style="Bezier" ortho="true" label="dnd" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">query["type"]</ap>
      </Properties>
    </node>
    <node type="Action" id="632892525355101357" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="264" y="456">
      <linkto id="632892525355101352" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Do Not Disturb</ap>
        <ap name="Prompt" type="literal">Select a line</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101359" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="264" y="616">
      <linkto id="632892525355101352" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Alter Ring Setting</ap>
        <ap name="Prompt" type="literal">Select a line</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632892525355101362" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848" y="456">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632892525355101363" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="760" y="456">
      <linkto id="632892525355101362" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Variable" id="632892525355101345" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632892525355101346" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632892525355101347" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">query</Properties>
    </node>
    <node type="Variable" id="632892525355101348" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="routingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632892525355101353" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632892525355101361" name="count" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">count</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>