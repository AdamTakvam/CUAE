<Application name="BargeCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="BargeCall">
    <outline>
      <treenode type="evh" id="632435773465789775" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632435773465789772" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632435773465789771" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632437143407008660" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632437143407008657" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632437143407008656" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632442637281040405" actid="632437143407008671" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632437143407008665" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632437143407008662" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632437143407008661" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632442637281040406" actid="632437143407008671" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632437143407008670" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632437143407008667" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632437143407008666" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632442637281040407" actid="632437143407008671" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632437167973019250" level="2" text="Metreos.Events.cBarge.AddConferee: OnAddConferee">
        <node type="function" name="OnAddConferee" id="632437167973019247" path="Metreos.StockTools" />
        <node type="event" name="AddConferee" id="632437167973019246" path="Metreos.Events.cBarge.AddConferee" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632438060957014572" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632438060957014569" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632438060957014568" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632437280962334927" level="1" text="HandlePendingConferees">
        <node type="function" name="HandlePendingConferees" id="632437280962334924" path="Metreos.StockTools" />
        <calls>
          <ref actid="632437280962334942" />
          <ref actid="632438964144403280" />
          <ref actid="632438964144403301" />
          <ref actid="632437280962334923" />
          <ref actid="632439052207895517" />
          <ref actid="632439264744318057" />
        </calls>
      </treenode>
      <treenode type="fun" id="632437280962334962" level="1" text="AddCallToConference">
        <node type="function" name="AddCallToConference" id="632437280962334959" path="Metreos.StockTools" />
        <calls>
          <ref actid="632438177259880439" />
          <ref actid="632437280962334958" />
        </calls>
      </treenode>
      <treenode type="fun" id="632439052207894848" level="1" text="HangupCallers">
        <node type="function" name="HangupCallers" id="632439052207894845" path="Metreos.StockTools" />
        <calls>
          <ref actid="632439052207895518" />
          <ref actid="632439264744318058" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632442637281040359" vid="632435773465789780">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_from" id="632442637281040361" vid="632435773465789782">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_originalTo" id="632442637281040363" vid="632435773465789784">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632442637281040365" vid="632436648769301344">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_dialPointDN" id="632442637281040367" vid="632437155992942172">
        <Properties type="String" initWith="DialPointDN">g_dialPointDN</Properties>
      </treenode>
      <treenode text="g_bridgeCallId" id="632442637281040369" vid="632437167973019183">
        <Properties type="String">g_bridgeCallId</Properties>
      </treenode>
      <treenode text="g_callerMap" id="632442637281040371" vid="632437280962334690">
        <Properties type="Hashtable">g_callerMap</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632442637281040373" vid="632437280962334693">
        <Properties type="String" defaultInitWith="0">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_bridgeConnId" id="632442637281040375" vid="632437280962334705">
        <Properties type="String" defaultInitWith="0">g_bridgeConnId</Properties>
      </treenode>
      <treenode text="g_callerIdUnavailable" id="632442637281040377" vid="632437280962334918">
        <Properties type="Bool" defaultInitWith="false">g_callerIdUnavailable</Properties>
      </treenode>
      <treenode text="g_pendingConferees" id="632442637281040379" vid="632437280962334928">
        <Properties type="Hashtable">g_pendingConferees</Properties>
      </treenode>
      <treenode text="g_bridgeAttempting" id="632442637281040381" vid="632437280962334936">
        <Properties type="Bool" defaultInitWith="false">g_bridgeAttempting</Properties>
      </treenode>
      <treenode text="g_bridgeEstablished" id="632442637281040383" vid="632437280962334938">
        <Properties type="Bool" defaultInitWith="false">g_bridgeEstablished</Properties>
      </treenode>
      <treenode text="g_firstCallerConnId" id="632442637281040385" vid="632438366127631008">
        <Properties type="String" defaultInitWith="0">g_firstCallerConnId</Properties>
      </treenode>
      <treenode text="g_firstCallerAnswered" id="632442637281040387" vid="632438964144403277">
        <Properties type="Bool" defaultInitWith="false">g_firstCallerAnswered</Properties>
      </treenode>
      <treenode text="g_mainRoutingGuid" id="632442637281040389" vid="632438964144403309">
        <Properties type="String">g_mainRoutingGuid</Properties>
      </treenode>
      <treenode text="g_mainCallId" id="632442637281040391" vid="632439710441214798">
        <Properties type="Long">g_mainCallId</Properties>
      </treenode>
      <treenode text="g_deviceToBargeMap" id="632442637281040393" vid="632441683792023430">
        <Properties type="Hashtable" initWith="DeviceBargeMap">g_deviceToBargeMap</Properties>
      </treenode>
      <treenode text="g_handlingDeviceName" id="632442637281040395" vid="632441745576777178">
        <Properties type="String">g_handlingDeviceName</Properties>
      </treenode>
      <treenode text="g_bargeLineNumber" id="632442637281040397" vid="632441745576777181">
        <Properties type="String">g_bargeLineNumber</Properties>
      </treenode>
      <treenode text="g_mainTimeStamp" id="632442637281040399" vid="632442536910119920">
        <Properties type="DateTime">g_mainTimeStamp</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632435773465789774" treenode="632435773465789775" appnode="632435773465789772" handlerfor="632435773465789771">
    <node type="Start" id="632435773465789774" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="235">
      <linkto id="632441683792023432" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632435773465789796" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="235" y="237">
      <linkto id="632435773465789797" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string from, ref string originalTo, ref Hashtable g_callerMap, ref Hashtable g_pendingConferees, ref bool g_callerIdUnavailable)
	{
		if ((from == null) || (from == string.Empty))
		{
			from = "UNAVAILABLE";
			g_callerIdUnavailable = true;
		}

		originalTo = originalTo.Substring(1);
		g_callerMap = new Hashtable();
		g_pendingConferees = new Hashtable();
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632435773465789797" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="326" y="237">
      <linkto id="632437143407008655" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">callId</ap>
        <ap name="Value2" type="variable">originalTo</ap>
        <ap name="Value3" type="variable">from</ap>
        <ap name="Value4" type="variable">routingGuid</ap>
        <rd field="ResultData">g_callId</rd>
        <rd field="ResultData2">g_originalTo</rd>
        <rd field="ResultData3">g_from</rd>
        <rd field="ResultData4">g_routingGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632437143407008655" name="GetCallRecords" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="428" y="238">
      <linkto id="632437155992942345" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632439052207894835" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">originalTo</ap>
        <rd field="CallRecordsTable">callRecordsTable</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: retrieving call records for callId: " + callId.ToString() + " and lineId: " + originalTo</log>
        <log condition="success" on="true" level="Info" type="literal">OnIncomingCall: Call records retrieved</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: could not obtain call records</log>
      </Properties>
    </node>
    <node type="Action" id="632437143407008671" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="1404" y="218" mx="1470" my="234">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632437143407008660" />
        <item text="OnMakeCall_Failed" treenode="632437143407008665" />
        <item text="OnRemoteHangup" treenode="632437143407008670" />
      </items>
      <linkto id="632437167973019178" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403284" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="To" type="variable">g_bargeLineNumber</ap>
        <ap name="From" type="variable">g_dialPointDN</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <ap name="userData" type="literal">none</ap>
        <rd field="CallId">g_bridgeCallId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: placing bridging call to: " + g_bargeLineNumber + " from: " + g_dialPointDN</log>
      </Properties>
    </node>
    <node type="Action" id="632437143407008675" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="636" y="237">
      <linkto id="632441745576777180" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632439052207894835" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: parsing data rows</log>
        <log condition="default" on="true" level="Warning" type="literal">OnIncomingCall: could not parse call record</log>
	public static string Execute(ref string mainRoutingGuid, ref string bargeRoutingGuid, DataTable callRecordsTable, ref long g_mainCallId, ref string g_handlingDeviceName, ref DateTime g_mainTimeStamp)
	{
		try
		{
			DataRow row = callRecordsTable.Rows[0];
			mainRoutingGuid = Convert.ToString(row[CBargeCallRecords.RoutingGuid]);
			g_mainCallId = Convert.ToInt64(row[CBargeCallRecords.CallId]);
			if (row.IsNull(CBargeCallRecords.BargeRoutingGuid))
				bargeRoutingGuid = null;
			else
				bargeRoutingGuid = Convert.ToString(row[CBargeCallRecords.BargeRoutingGuid]);

			if (row.IsNull(CBargeCallRecords.DeviceName))
				g_handlingDeviceName = null;
			else
			{
				g_handlingDeviceName = Convert.ToString(row[CBargeCallRecords.DeviceName]);
				g_mainTimeStamp = Convert.ToDateTime(row[CBargeCallRecords.TimeStamp]);
			}

			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632437155992942345" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="530" y="238">
      <linkto id="632437143407008675" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632439052207894835" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((callRecordsTable != null) &amp;&amp; (callRecordsTable.Rows.Count &gt; 0))</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: checking that call records table is not empty</log>
        <log condition="default" on="true" level="Warning" type="literal">OnIncomingCall: Call records table was either null or empty</log>
      </Properties>
    </node>
    <node type="Action" id="632437167973019174" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="898" y="234">
      <linkto id="632438366127631005" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632439710441215148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(bargeRoutingGuid == null)</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: checking if barge script routing guid is defined</log>
      </Properties>
    </node>
    <node type="Action" id="632437167973019178" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1622.21289" y="233">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632437167973019179" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="898.2129" y="459">
      <linkto id="632437167973019182" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632439710441215150" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="from" type="variable">from</ap>
        <ap name="callId" type="variable">callId</ap>
        <ap name="fromRoutingGuid" type="variable">routingGuid</ap>
        <ap name="connectionId" type="variable">g_firstCallerConnId</ap>
        <ap name="EventName" type="literal">Metreos.Events.cBarge.AddConferee</ap>
        <ap name="ToGuid" type="variable">bargeRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Sending 'AddConferee' event to script: " + bargeRoutingGuid</log>
        <log condition="default" on="true" level="Error" type="literal">OnIncomingCall: SendEvent action DID NOT SUCCEED! Registering self as handler, creating conference bridge</log>
      </Properties>
    </node>
    <node type="Action" id="632437167973019182" name="Forward" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1017.21289" y="458">
      <Properties final="true" type="appControl">
        <ap name="ToGuid" type="variable">bargeRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Forwarding all future events to script: " + bargeRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632437280962334940" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1357.68359" y="233">
      <linkto id="632437143407008671" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">true</ap>
        <ap name="Value3" type="variable">mainRoutingGuid</ap>
        <rd field="ResultData">g_bridgeAttempting</rd>
        <rd field="ResultData2">g_firstCallerAnswered</rd>
        <rd field="ResultData3">g_mainRoutingGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632437280962334941" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1468.68359" y="494">
      <linkto id="632437280962334942" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">false</ap>
        <rd field="ResultData">g_bridgeAttempting</rd>
        <rd field="ResultData2">g_bridgeEstablished</rd>
      </Properties>
    </node>
    <node type="Action" id="632437280962334942" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1555.68359" y="479" mx="1628" my="495">
      <items count="1">
        <item text="HandlePendingConferees" />
      </items>
      <linkto id="632442536910119926" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">deny</ap>
        <ap name="FunctionName" type="literal">HandlePendingConferees</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Failed: calling HandlePendingConferees(deny)</log>
      </Properties>
    </node>
    <node type="Action" id="632438366127631005" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="1001.04816" y="233">
      <linkto id="632439052207894842" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403279" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="ConnectionId">g_firstCallerConnId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Attempting to answer barging call with callId: " + callId</log>
        <log condition="success" on="true" level="Info" type="literal">OnIncomingCall: Call answered</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: Call could not be answered</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403279" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1159" y="345">
      <linkto id="632438964144403280" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_firstCallerAnswered</rd>
      </Properties>
    </node>
    <node type="Action" id="632438964144403280" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1090.68359" y="524" mx="1163" my="540">
      <items count="1">
        <item text="HandlePendingConferees" />
      </items>
      <linkto id="632438964144403282" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">deny</ap>
        <ap name="FunctionName" type="literal">HandlePendingConferees</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: calling HandlePendingConferees(deny)</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403282" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1160" y="696">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632438964144403283" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="1255" y="346">
      <linkto id="632438964144403279" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Hanging up call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403284" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="1469" y="387">
      <linkto id="632437280962334941" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Hanging up call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207894835" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="607" y="434">
      <linkto id="632439052207894837" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Rejecting call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207894837" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="610" y="553">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632439052207894842" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1141.90625" y="231">
      <linkto id="632441745576777183" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: adding conferee to callId-&gt;connectionId map</log>
	public static string Execute(ref Hashtable g_callerMap, string g_firstCallerConnId, string g_callId)
	{
		try
		{
			g_callerMap.Add(g_callId, g_firstCallerConnId);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632439710441215148" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="898.048157" y="342">
      <linkto id="632437167973019179" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403279" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="ConnectionId">g_firstCallerConnId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: Attempting to answer barging call with callId: " + callId</log>
        <log condition="success" on="true" level="Info" type="literal">OnIncomingCall: Call answered</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: Call could not be answered</log>
      </Properties>
    </node>
    <node type="Label" id="632439710441215150" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="898" y="570" />
    <node type="Label" id="632439710441215151" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1142" y="133">
      <linkto id="632439052207894842" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632441683792023432" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="133" y="237">
      <linkto id="632435773465789796" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632441683792023433" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(g_deviceToBargeMap != null)</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: verifying that g_deviceToBargeMap is not null</log>
      </Properties>
    </node>
    <node type="Action" id="632441683792023433" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="133" y="351">
      <linkto id="632441683792023656" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnIncomingCall: g_deviceToBargeMap is null! Please configure the "deviceBargeMap" application property in mceadmin</ap>
        <ap name="LogLevel" type="literal">Error</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632441683792023656" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="471">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632441745576777180" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="759" y="237">
      <linkto id="632437167973019174" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632439052207894835" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: The value of g_handlingDeviceName is: " + ((g_handlingDeviceName == null) ? "NULL" : g_handlingDeviceName)</log>
        <log condition="success" on="true" level="Info" type="csharp">"OnIncomingCall: found record for line: " + originalTo + " on device: " + g_handlingDeviceName + " with barge number: " + g_bargeLineNumber</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnIncomingCall: could not obtain device information for line: " + originalTo</log>
	public static string Execute(string g_handlingDeviceName, ref string g_bargeLineNumber, Hashtable g_deviceToBargeMap)
	{
		if (g_handlingDeviceName == null)
			return IApp.VALUE_FAILURE;

		g_bargeLineNumber = g_deviceToBargeMap[g_handlingDeviceName] as string;
		if (g_bargeLineNumber == null)
			return IApp.VALUE_FAILURE;

		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632441745576777183" name="BindBargeToRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="1254.25586" y="232">
      <linkto id="632437280962334940" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403283" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">originalTo</ap>
        <ap name="DeviceName" type="variable">g_handlingDeviceName</ap>
        <ap name="RoutingGuid" type="variable">mainRoutingGuid</ap>
        <ap name="Timestamp" type="variable">g_mainTimeStamp</ap>
        <ap name="BargeRoutingGuid" type="variable">routingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: no previous barge handler exists, registering self with call record - lineId: " + originalTo + " routingGuid: " + routingGuid</log>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: BindBargeToRecord action failed</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910119926" name="BindBargeToRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="1624.69849" y="384">
      <linkto id="632437167973019178" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_originalTo</ap>
        <ap name="DeviceName" type="variable">g_handlingDeviceName</ap>
        <ap name="RoutingGuid" type="variable">g_mainRoutingGuid</ap>
        <ap name="Timestamp" type="variable">g_mainTimeStamp</ap>
        <ap name="BargeRoutingGuid" type="csharp">null</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: BindBargeToRecord removing self as handler for call on line: " + g_originalTo + " on device: " + g_handlingDeviceName</log>
      </Properties>
    </node>
    <node type="Variable" id="632435773465789776" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632435773465789777" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632435773465789778" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632435773465789779" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632435773465789809" name="callRecordsTable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">callRecordsTable</Properties>
    </node>
    <node type="Variable" id="632437155992942170" name="mainRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mainRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632437155992942171" name="bargeRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">bargeRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632437498520371049" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference">originalTo</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632437143407008659" treenode="632437143407008660" appnode="632437143407008657" handlerfor="632437143407008656">
    <node type="Start" id="632437143407008659" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="60" y="251">
      <linkto id="632437280962334704" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632437280962334703" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="239" y="251">
      <linkto id="632438955420427425" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">conferenceId</ap>
        <ap name="Value2" type="variable">bridgeConnId</ap>
        <ap name="Value3" type="literal">true</ap>
        <rd field="ResultData">g_conferenceId</rd>
        <rd field="ResultData2">g_bridgeConnId</rd>
        <rd field="ResultData3">g_bridgeEstablished</rd>
      </Properties>
    </node>
    <node type="Action" id="632437280962334704" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="150" y="252">
      <linkto id="632437280962334703" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632438964144403303" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(conferenceId != "0")</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Complete: CallId is: " + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632437280962334713" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="477" y="251">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632438955420427425" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="352.735046" y="252">
      <linkto id="632438955420427426" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632437280962334713" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_firstCallerConnId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnAddConferee: attempting to add party with connectionId: " + g_firstCallerConnId + " to conference with Id: " + g_conferenceId</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnAddConferee: could not add party with connectionId: " + g_firstCallerConnId + " to conference with Id: " + g_conferenceId</log>
      </Properties>
    </node>
    <node type="Action" id="632438955420427426" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="355" y="471">
      <linkto id="632438964144403304" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnMakeCall_Complete: JoinConference action took default path</ap>
        <ap name="LogLevel" type="literal">Warning</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632438964144403301" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="208" y="559" mx="281" my="575">
      <items count="1">
        <item text="HandlePendingConferees" />
      </items>
      <linkto id="632442536910119924" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">deny</ap>
        <ap name="FunctionName" type="literal">HandlePendingConferees</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Complete: calling HandlePendingConferees(deny)</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403303" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="151" y="364">
      <linkto id="632438964144403304" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnMakeCall_Complete: ConferenceId is 0, unwinding...</ap>
        <ap name="LogLevel" type="literal">Warning</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632438964144403304" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="152.352875" y="469">
      <linkto id="632438964144403306" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Complete: hanging up caller with callId: " + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403306" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="151.352875" y="573">
      <linkto id="632438964144403301" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_bridgeCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Complete: hanging up bridge call with callId: " + g_bridgeCallId</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403311" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="543" y="575">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632442536910119924" name="BindBargeToRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="423.442749" y="575">
      <linkto id="632438964144403311" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_originalTo</ap>
        <ap name="DeviceName" type="variable">g_handlingDeviceName</ap>
        <ap name="RoutingGuid" type="variable">g_mainRoutingGuid</ap>
        <ap name="Timestamp" type="variable">g_mainTimeStamp</ap>
        <ap name="BargeRoutingGuid" type="csharp">null</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Complete: BindBargeToRecord removing self as handler for call on line: " + g_originalTo + " on device: " + g_handlingDeviceName</log>
      </Properties>
    </node>
    <node type="Variable" id="632437280962334701" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632437280962334702" name="bridgeConnId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference">bridgeConnId</Properties>
    </node>
    <node type="Variable" id="632437280962334707" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632437143407008664" treenode="632437143407008665" appnode="632437143407008662" handlerfor="632437143407008661">
    <node type="Start" id="632437143407008664" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="286">
      <linkto id="632438964144403275" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632437280962334920" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="223" y="284">
      <linkto id="632437280962334930" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632438964144403273" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_callerIdUnavailable</ap>
      </Properties>
    </node>
    <node type="Action" id="632437280962334923" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="372.210938" y="512" mx="445" my="528">
      <items count="1">
        <item text="HandlePendingConferees" />
      </items>
      <linkto id="632442536910119928" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">deny</ap>
        <ap name="FunctionName" type="literal">HandlePendingConferees</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Failed: calling HandlePendingConferees(deny)</log>
      </Properties>
    </node>
    <node type="Action" id="632437280962334930" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="223" y="400">
      <linkto id="632438964144403274" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632438964144403273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(g_originalTo != g_from)</ap>
      </Properties>
    </node>
    <node type="Action" id="632437280962334932" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="674.884155" y="528">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632438964144403273" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="441" y="284">
      <linkto id="632437280962334923" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Failed: Conference bridge call failed, callerId not available, thus hanging up call with callId: " + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403274" name="BlindTransfer" class="MaxActionNode" group="" path="Metreos.CallControl" x="222" y="528">
      <linkto id="632437280962334923" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="To" type="variable">g_from</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Failed: Attempting to forward call with callId: " + g_callId + " to #: " + g_from</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403275" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="114" y="285">
      <linkto id="632437280962334920" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">false</ap>
        <rd field="ResultData">g_bridgeAttempting</rd>
        <rd field="ResultData2">g_bridgeEstablished</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnMakeCall_Failed: Setting g_bridgeAttempting and g_bridgeEstablished to "false"</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910119928" name="BindBargeToRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="566.4427" y="528">
      <linkto id="632437280962334932" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_originalTo</ap>
        <ap name="DeviceName" type="variable">g_handlingDeviceName</ap>
        <ap name="RoutingGuid" type="variable">g_mainRoutingGuid</ap>
        <ap name="Timestamp" type="variable">g_mainTimeStamp</ap>
        <ap name="BargeRoutingGuid" type="csharp">null</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Complete: BindBargeToRecord removing self as handler for call on line: " + g_originalTo + " on device: " + g_handlingDeviceName</log>
      </Properties>
    </node>
    <node type="Variable" id="632437280962334917" name="callIdToRedirect" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="userData" refType="reference">callIdToRedirect</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632437143407008669" treenode="632437143407008670" appnode="632437143407008667" handlerfor="632437143407008666">
    <node type="Start" id="632437143407008669" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="221">
      <linkto id="632439052207894839" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632439052207894839" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="128" y="220">
      <linkto id="632439052207894864" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632439052207895517" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_bridgeCallId</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRemoteHangup: checking if hangup came from bridge call</log>
        <log condition="default" on="true" level="Info" type="literal">OnRemoteHangup: hang up not from bridge call</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207894864" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="245" y="221">
      <linkto id="632439052207895191" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"OnRemoteHangup: hangup detected from callId: " + callId</log>
	public static string Execute(ref Hashtable g_callerMap, string callId)
	{
		try
		{
			g_callerMap.Remove(callId);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632439052207895191" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="357" y="220">
      <linkto id="632439052207895513" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632439052207895516" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((g_callerMap.Count == 0) &amp;&amp; (g_pendingConferees.Count == 0))</ap>
      </Properties>
    </node>
    <node type="Action" id="632439052207895192" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="559" y="220">
      <linkto id="632439052207895514" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_bridgeCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRemoteHangup: hanging up bridge call with callId: " + g_bridgeCallId</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207895513" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="468" y="221">
      <linkto id="632439052207895192" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632439052207895515" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">(g_bridgeEstablished || g_bridgeAttempting)</ap>
      </Properties>
    </node>
    <node type="Action" id="632439052207895514" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="667" y="220">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632439052207895515" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="468" y="330">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632439052207895516" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="356" y="330">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnRemoteHangup: exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207895517" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="60.2109375" y="318" mx="133" my="334">
      <items count="1">
        <item text="HandlePendingConferees" />
      </items>
      <linkto id="632439052207895518" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">deny</ap>
        <ap name="FunctionName" type="literal">HandlePendingConferees</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRemoteHangup: Hangup from conference bridge, calling HandlePendingConferees(deny)</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207895518" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="90.23828" y="453" mx="134" my="469">
      <items count="1">
        <item text="HangupCallers" />
      </items>
      <linkto id="632439052207895521" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">HangupCallers</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnRemoteHangup: calling HangupCallers</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207895521" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="130" y="616">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnRemoteHangup: exiting script</log>
      </Properties>
    </node>
    <node type="Variable" id="632439052207894838" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAddConferee" startnode="632437167973019249" treenode="632437167973019250" appnode="632437167973019247" handlerfor="632437167973019246">
    <node type="Start" id="632437167973019249" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="166">
      <linkto id="632437280962334689" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632437280962334689" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="119" y="166">
      <linkto id="632438964144403268" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="csharp">"OnAddConferee: Received request to add caller from: " + from + "  to current conference. CallId: " + callId</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632437280962334699" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="534" y="168">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632438177259880439" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="312.893555" y="152" mx="376" my="168">
      <items count="1">
        <item text="AddCallToConference" />
      </items>
      <linkto id="632437280962334699" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callId" type="variable">callId</ap>
        <ap name="connectionId" type="variable">connectionId</ap>
        <ap name="FunctionName" type="literal">AddCallToConference</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnAddConferee: Calling AddCallToConference, passing in callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403268" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="243" y="167">
      <linkto id="632438177259880439" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632438964144403269" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_bridgeEstablished</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAddConferee: checking if conference bridge is established</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403269" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="239" y="439">
      <linkto id="632438964144403270" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632438964144403272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_bridgeAttempting</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnAddConferee: checking if conference bridge is pending...</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403270" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="382" y="440">
      <linkto id="632438964144403271" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403272" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"OnAddConferee: Adding caller with callId: " + callId + " to pending conferees list."</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnAddConferee: failed to add caller with callId: " + callId + " to pending conferees table"</log>
	public static string Execute(Hashtable g_pendingConferees, string callId, string conferenceId)
	{
		try
		{
			g_pendingConferees[callId] = conferenceId;
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632438964144403271" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="531" y="440">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnAddConferee: exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403272" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="381" y="570">
      <linkto id="632438964144403271" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnAddConferee: Rejecting caller with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Variable" id="632437280962334686" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632437280962334687" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="from" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632437280962334688" name="fromRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="fromRoutingGuid" refType="reference">fromRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632439895733261293" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="connectionId" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632438060957014571" treenode="632438060957014572" appnode="632438060957014569" handlerfor="632438060957014568">
    <node type="Start" id="632438060957014571" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="358">
      <linkto id="632439710441214800" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632439264744318057" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="418" y="339" mx="491" my="355">
      <items count="1">
        <item text="HandlePendingConferees" />
      </items>
      <linkto id="632439264744318058" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="action" type="literal">deny</ap>
        <ap name="FunctionName" type="literal">HandlePendingConferees</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: Hangup from conference bridge, calling HandlePendingConferees(deny)</log>
      </Properties>
    </node>
    <node type="Action" id="632439264744318058" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="588.027344" y="339" mx="632" my="355">
      <items count="1">
        <item text="HangupCallers" />
      </items>
      <linkto id="632439264744318059" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">HangupCallers</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: calling HangupCallers</log>
      </Properties>
    </node>
    <node type="Action" id="632439264744318059" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="742.789063" y="356">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441214800" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="135" y="359">
      <linkto id="632439710441214801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632442536910119932" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_mainCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiHangup: barge script received hangup for callId: " + callId.ToString() + " and script is handling callId " + g_mainCallId.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441214801" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="137" y="495">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: barge script ignoring hangup</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441214802" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="359" y="357">
      <linkto id="632439264744318057" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_bridgeCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiHangup: hanging up bridge call with callId: " + g_bridgeCallId</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910119932" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="244" y="358">
      <linkto id="632439710441214802" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632439710441214801" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">cause</ap>
        <ap name="Value2" type="literal">NORMAL</ap>
      </Properties>
    </node>
    <node type="Variable" id="632439710441214797" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632442536910119931" name="cause" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Cause" refType="reference">cause</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HandlePendingConferees" startnode="632437280962334926" treenode="632437280962334927" appnode="632437280962334924" handlerfor="632438060957014568">
    <node type="Loop" id="632437280962334935" name="Loop" text="loop (var)" cx="391.464844" cy="269" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="490" y="44" mx="686" my="178">
      <linkto id="632437280962334954" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632438240932137974" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="dictEnum" type="variable">g_pendingConferees</Properties>
    </node>
    <node type="Loop" id="632438240932137971" name="Loop" text="loop (var)" cx="392" cy="262" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="493" y="433" mx="689" my="564">
      <linkto id="632438240932137972" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632438240932137974" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="dictEnum" type="variable">g_pendingConferees</Properties>
    </node>
    <node type="Start" id="632437280962334926" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="298">
      <linkto id="632437280962334951" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632437280962334944" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="300" y="143" />
    <node type="Label" id="632437280962334945" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="298" y="465" />
    <node type="Label" id="632437280962334946" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="413" y="178">
      <linkto id="632437280962334935" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632437280962334947" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="413" y="564">
      <linkto id="632438240932137971" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632437280962334949" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="223.000015" y="299">
      <linkto id="632438240932137969" type="Labeled" style="Bezier" ortho="true" label="accept" />
      <linkto id="632438177259880440" type="Labeled" style="Bezier" ortho="true" label="deny" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">action</ap>
        <log condition="entry" on="true" level="Info" type="literal">HandlePendingConferees: determining action to take</log>
      </Properties>
    </node>
    <node type="Action" id="632437280962334951" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="123" y="299">
      <linkto id="632437280962334949" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632437280962334952" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((g_pendingConferees != null) &amp;&amp; (g_pendingConferees.Count &gt; 0))</ap>
      </Properties>
    </node>
    <node type="Action" id="632437280962334952" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="407">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">HandlePendingConferees: Nothing to do, exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632437280962334954" name="CustomCode" container="632437280962334935" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="604" y="179">
      <linkto id="632437280962334958" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403287" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string tempCallId, ref string tempConnId, IDictionaryEnumerator loopDictEnum)
	{
		try
		{
			tempCallId = loopDictEnum.Key as string;
			tempConnId = loopDictEnum.Value as string;

			if (tempCallId != null &amp;&amp; tempConnId != null)
			{
				return IApp.VALUE_SUCCESS;
			}
			else
				return IApp.VALUE_FAILURE;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}

</Properties>
    </node>
    <node type="Action" id="632437280962334958" name="CallFunction" container="632437280962334935" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="683.893555" y="163" mx="747" my="179">
      <items count="1">
        <item text="AddCallToConference" />
      </items>
      <linkto id="632437280962334935" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="callId" type="variable">tempCallId</ap>
        <ap name="connectionId" type="variable">tempConnId</ap>
        <ap name="FunctionName" type="literal">AddCallToConference</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"HandlePendingConferees: Calling AddCallToConference(callId = " + tempCallId + ")"</log>
      </Properties>
    </node>
    <node type="Action" id="632437280962334965" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1177.0293" y="359">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">HandlePendingConferees: exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632438177259880440" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="297" y="376">
      <linkto id="632437280962334945" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">HandlePendingConferees: Denying pending callers</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632438240932137969" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="301" y="216">
      <linkto id="632437280962334944" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">HandlePendingConferees: Accepting pending callers</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632438240932137972" name="CustomCode" container="632438240932137971" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="582.464844" y="563">
      <linkto id="632438240932137973" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403300" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref string tempCallId, IDictionaryEnumerator loopDictEnum)
	{
		try
		{
			tempCallId = loopDictEnum.Value as string;
			if (tempCallId != null)
			{
				return IApp.VALUE_SUCCESS;
			}
			else
				return IApp.VALUE_FAILURE;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632438240932137973" name="RejectCall" container="632438240932137971" class="MaxActionNode" group="" path="Metreos.CallControl" x="738.464844" y="563">
      <linkto id="632438240932137971" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">tempCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"HandlePendingConferees: Rejecting call with callId: " + tempCallId</log>
      </Properties>
    </node>
    <node type="Action" id="632438240932137974" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1012.46484" y="359">
      <linkto id="632437280962334965" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">HandlePendingConferees: Clearing list of pending conferees</log>
	public static string Execute(ref Hashtable g_pendingConferees)
	{
		g_pendingConferees.Clear();
		return IApp.VALUE_SUCCESS;	
	}
</Properties>
    </node>
    <node type="Label" id="632438964144403286" text="E" container="632437280962334935" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="852" y="72">
      <linkto id="632437280962334935" port="3" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632438964144403287" text="E" container="632437280962334935" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="605" y="266" />
    <node type="Label" id="632438964144403299" text="F" container="632438240932137971" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="843" y="495">
      <linkto id="632438240932137971" port="3" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632438964144403300" text="F" container="632438240932137971" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="583" y="649" />
    <node type="Variable" id="632437280962334933" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="action" refType="reference">action</Properties>
    </node>
    <node type="Variable" id="632437280962334953" name="tempCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">tempCallId</Properties>
    </node>
    <node type="Variable" id="632437280962334955" name="removeList" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">removeList</Properties>
    </node>
    <node type="Variable" id="632437280962334956" name="tempConnId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">tempConnId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="AddCallToConference" startnode="632437280962334961" treenode="632437280962334962" appnode="632437280962334959" handlerfor="632438060957014568">
    <node type="Start" id="632437280962334961" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="372">
      <linkto id="632438177259880432" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632438177259880431" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="474.7975" y="373.5">
      <linkto id="632438964144403295" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632438964144403296" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnAddConferee: adding conferee to callId-&gt;connectionId map</log>
        <log condition="default" on="true" level="Info" type="csharp">"AddCallToConference: Could not add call with callId to callId-&gt;connectionId map"</log>
	public static string Execute(ref Hashtable g_callerMap, string connId, string callId)
	{
		try
		{
			g_callerMap.Add(callId, connId);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632438177259880432" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="320.555359" y="372.5">
      <linkto id="632438177259880431" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632438964144403293" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">connId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnAddConferee: attempting to add party with connectionId: " + connId + " to conference with Id: " + g_conferenceId</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnAddConferee: could not add party with connectionId: " + connId + " to conference with Id: " + g_conferenceId</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403288" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="335" y="126">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">failure</ap>
        <log condition="entry" on="true" level="Info" type="literal">AddCallToConference: Exiting function, returning failure</log>
      </Properties>
    </node>
    <node type="Label" id="632438964144403289" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="225" y="126">
      <linkto id="632438964144403288" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632438964144403290" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="224" y="207">
      <linkto id="632438964144403291" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632438964144403291" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="334" y="209">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">success</ap>
        <log condition="entry" on="true" level="Info" type="literal">AddCallToConference: Exiting function, returning success</log>
      </Properties>
    </node>
    <node type="Action" id="632438964144403293" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="322" y="475">
      <linkto id="632438964144403294" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"AddCallToConference: hanging up call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Label" id="632438964144403294" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="322" y="570" />
    <node type="Label" id="632438964144403295" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="475" y="486" />
    <node type="Label" id="632438964144403296" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="597" y="374" />
    <node type="Variable" id="632438060957014068" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632438343497630509" name="connId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="connectionId" refType="reference">connId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="HangupCallers" startnode="632439052207894847" treenode="632439052207894848" appnode="632439052207894845" handlerfor="632438060957014568">
    <node type="Loop" id="632439052207894853" name="Loop" text="loop (var)" cx="447" cy="388" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="288" y="209" mx="512" my="403">
      <linkto id="632439052207894854" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632439052207894861" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="dictEnum" type="variable">g_callerMap</Properties>
    </node>
    <node type="Start" id="632439052207894847" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="403">
      <linkto id="632439052207894851" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632439052207894851" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="119" y="403">
      <linkto id="632439052207895519" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">HangupCallers: function entry</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632439052207894861" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="858.9414" y="403">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">HangupCallers: exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207895519" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="211" y="404">
      <linkto id="632439052207894853" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632439052207895520" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">g_callerMap.Count &gt; 0</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"HangupCallers:caller map count = " + g_callerMap.Count</log>
      </Properties>
    </node>
    <node type="Action" id="632439052207894854" name="Assign" container="632439052207894853" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="372" y="403">
      <linkto id="632439052207894856" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="csharp">(loopDictEnum.Key as string)</ap>
        <rd field="ResultData">callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632439052207894856" name="If" container="632439052207894853" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="478" y="403">
      <linkto id="632439052207894858" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632439052207894857" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((callId != null) &amp;&amp; (callId != string.Empty))</ap>
      </Properties>
    </node>
    <node type="Action" id="632439052207894857" name="Hangup" container="632439052207894853" class="MaxActionNode" group="" path="Metreos.CallControl" x="600" y="404">
      <linkto id="632439052207894860" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632439052207894853" port="3" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="success" on="true" level="Info" type="csharp">"HangupCallers: Hangup succesful for callId: " + callId</log>
        <log condition="default" on="true" level="Info" type="csharp">"HangupCallers: Hangup action took default path for callId: " + callId</log>
      </Properties>
    </node>
    <node type="Label" id="632439052207894858" text="E" container="632439052207894853" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="480" y="503" />
    <node type="Label" id="632439052207894859" text="E" container="632439052207894853" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="681" y="341">
      <linkto id="632439052207894853" port="3" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632439052207894860" text="E" container="632439052207894853" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="601" y="501" />
    <node type="Action" id="632439052207895520" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="211" y="504">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">HangupCallers: nothing to do, exiting</log>
      </Properties>
    </node>
    <node type="Variable" id="632439052207894855" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>