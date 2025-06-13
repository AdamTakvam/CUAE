<Application name="IncomingCall" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IncomingCall">
    <outline>
      <treenode type="evh" id="632436358155658491" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632436358155658488" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632436358155658487" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632436358155658569" level="2" text="Metreos.Providers.JTapi.JTapiCallEstablished: OnJTapiCallEstablished">
        <node type="function" name="OnJTapiCallEstablished" id="632436358155658566" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallEstablished" id="632436358155658565" path="Metreos.Providers.JTapi.JTapiCallEstablished" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632436358155658574" level="2" text="Metreos.Providers.JTapi.JTapiHangup: OnJTapiHangup">
        <node type="function" name="OnJTapiHangup" id="632436358155658571" path="Metreos.StockTools" />
        <node type="event" name="JTapiHangup" id="632436358155658570" path="Metreos.Providers.JTapi.JTapiHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632436648769301191" level="2" text="Metreos.Events.cBarge.IntegrityCheck: OnIntegrityCheck">
        <node type="function" name="OnIntegrityCheck" id="632436648769301188" path="Metreos.StockTools" />
        <node type="event" name="IntegrityCheck" id="632436648769301187" path="Metreos.Events.cBarge.IntegrityCheck" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632436648769301196" level="2" text="Metreos.Events.cBarge.IntegrityCheckFailed: OnIntegrityCheckFailed">
        <node type="function" name="OnIntegrityCheckFailed" id="632436648769301193" path="Metreos.StockTools" />
        <node type="event" name="IntegrityCheckFailed" id="632436648769301192" path="Metreos.Events.cBarge.IntegrityCheckFailed" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632441376825748089" level="2" text="Metreos.Providers.JTapi.JTapiCallActive: OnJTapiCallActive">
        <node type="function" name="OnJTapiCallActive" id="632441376825748086" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallActive" id="632441376825748085" path="Metreos.Providers.JTapi.JTapiCallActive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632441376825748094" level="2" text="Metreos.Providers.JTapi.JTapiCallInactive: OnJTapiCallInactive">
        <node type="function" name="OnJTapiCallInactive" id="632441376825748091" path="Metreos.StockTools" />
        <node type="event" name="JTapiCallInactive" id="632441376825748090" path="Metreos.Providers.JTapi.JTapiCallInactive" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632442536910120088" level="1" text="DoIntegrityCheck">
        <node type="function" name="DoIntegrityCheck" id="632442536910120085" path="Metreos.StockTools" />
        <calls>
          <ref actid="632442536910120084" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632442637281040587" vid="632436358155658554">
        <Properties type="Long">g_callId</Properties>
      </treenode>
      <treenode text="g_to" id="632442637281040589" vid="632436358155658556">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_originalTo" id="632442637281040591" vid="632436358155658558">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_from" id="632442637281040593" vid="632436358155658560">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632442637281040595" vid="632436358155658562">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_dialPointDN" id="632442637281040597" vid="632436358155658611">
        <Properties type="String" initWith="DialPointDN">g_dialPointDN</Properties>
      </treenode>
      <treenode text="g_timeStamp" id="632442637281040599" vid="632436358155658616">
        <Properties type="DateTime">g_timeStamp</Properties>
      </treenode>
      <treenode text="g_callRecordsTable" id="632442637281040601" vid="632436358155658619">
        <Properties type="DataTable">g_callRecordsTable</Properties>
      </treenode>
      <treenode text="g_callMode" id="632442637281040603" vid="632436630697700431">
        <Properties type="String" defaultInitWith="new">g_callMode</Properties>
      </treenode>
      <treenode text="g_mainCallId" id="632442637281040605" vid="632436648769301575">
        <Properties type="Long">g_mainCallId</Properties>
      </treenode>
      <treenode text="g_bargeRoutingGuid" id="632442637281040607" vid="632436648769301577">
        <Properties type="String" defaultInitWith="EMPTY">g_bargeRoutingGuid</Properties>
      </treenode>
      <treenode text="g_gotCallEstablished" id="632442637281040609" vid="632439264744317306">
        <Properties type="Bool" defaultInitWith="false">g_gotCallEstablished</Properties>
      </treenode>
      <treenode text="g_deviceName" id="632442637281040611" vid="632442536910120082">
        <Properties type="String">g_deviceName</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" activetab="true" startnode="632436358155658490" treenode="632436358155658491" appnode="632436358155658488" handlerfor="632436358155658487">
    <node type="Start" id="632436358155658490" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="58" y="343">
      <linkto id="632436358155658552" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632436358155658552" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="143" y="343">
      <linkto id="632436630697700426" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiIncomingCall: Function entry</log>
	public static string Execute(ref string from, ref DataTable g_callRecordsTable)
	{
		if ((from == null) || (from == string.Empty))
			from = "UNAVAILABLE";
		

		g_callRecordsTable = new DataTable();
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632436358155658553" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="428" y="342">
      <linkto id="632436358155658564" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">callId</ap>
        <ap name="Value2" type="variable">to</ap>
        <ap name="Value3" type="variable">originalTo</ap>
        <ap name="Value4" type="variable">from</ap>
        <rd field="ResultData">g_callId</rd>
        <rd field="ResultData2">g_to</rd>
        <rd field="ResultData3">g_originalTo</rd>
        <rd field="ResultData4">g_from</rd>
      </Properties>
    </node>
    <node type="Action" id="632436358155658564" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="523" y="342">
      <linkto id="632436648769301438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">routingGuid</ap>
        <ap name="Value2" type="variable">deviceName</ap>
        <rd field="ResultData">g_routingGuid</rd>
        <rd field="ResultData2">g_deviceName</rd>
      </Properties>
    </node>
    <node type="Action" id="632436630697700426" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="243" y="342">
      <linkto id="632436630697700427" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632436630697700428" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632436648769301437" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">from</ap>
        <ap name="Value2" type="variable">g_dialPointDN</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiIncomingCall: checking to see if this call came from designated bridge DN. from: " + from + " to: " + to + " originalTo: " + originalTo</log>
      </Properties>
    </node>
    <node type="Action" id="632436630697700427" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="344" y="343">
      <linkto id="632436358155658553" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnJTapiIncomingCall: New call detected</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632436630697700428" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="244" y="457">
      <linkto id="632436630697700429" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnJTapiIncomingCall: "Compare" action returned "default", bailing!</ap>
        <ap name="LogLevel" type="literal">Error</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632436630697700429" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243" y="559">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632436630697700430" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="681" y="221">
      <linkto id="632436648769301573" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">bridge</ap>
        <ap name="Value2" type="variable">bargeRoutingGuid</ap>
        <ap name="Value3" type="variable">mainCallId</ap>
        <rd field="ResultData">g_callMode</rd>
        <rd field="ResultData2">g_bargeRoutingGuid</rd>
        <rd field="ResultData3">g_mainCallId</rd>
        <log condition="entry" on="true" level="Info" type="literal">Entering "Bridge Call" mode</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301437" name="GetCallRecords" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="245" y="220">
      <linkto id="632436648769301444" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632437099120595638" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">to</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <rd field="CallRecordsTable">callRecordsTable</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiIncomingCall: retrieving call records for lineId: " + to</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301438" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="625" y="342">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632436648769301440" name="JTapiAnswerCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="583" y="222">
      <linkto id="632436630697700430" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632439114879933208" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiIncomingCall: Calling JTapiAnswerCall(" + callId.ToString() + ")"</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301444" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="366" y="222">
      <linkto id="632436648769301445" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632437099120595637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((callRecordsTable != null) &amp;&amp; (callRecordsTable.Rows.Count &gt; 0))</ap>
      </Properties>
    </node>
    <node type="Action" id="632436648769301445" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="472" y="221">
      <linkto id="632436648769301440" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632437099120595640" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref long mainCallId, ref string bargeRoutingGuid, DataTable callRecordsTable)
	{
		try
		{
			DataRow row = callRecordsTable.Rows[0];
			mainCallId = Convert.ToInt64(row[CBargeCallRecords.CallId]);
			bargeRoutingGuid = Convert.ToString(row[CBargeCallRecords.BargeRoutingGuid]);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Label" id="632436648769301573" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="776" y="222" />
    <node type="Label" id="632436648769301574" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="428" y="449">
      <linkto id="632436358155658553" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632437099120595635" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="419" y="601">
      <linkto id="632437099120595642" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632437099120595636" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="365" y="57" />
    <node type="Action" id="632437099120595637" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="366" y="131">
      <linkto id="632437099120595636" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnJTapiIncomingCall: list of active call records empty, rejecting call</ap>
        <ap name="LogLevel" type="literal">Error</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632437099120595638" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="244" y="131">
      <linkto id="632437099120595639" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnJTapiIncomingCall: GetCallRecords action did not succeed, rejecting call</ap>
        <ap name="LogLevel" type="literal">Error</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Label" id="632437099120595639" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="244" y="56" />
    <node type="Action" id="632437099120595640" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="472" y="129">
      <linkto id="632437099120595641" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnJTapiIncomingCall: could not retrieve information from call record, rejecting call</ap>
        <ap name="LogLevel" type="literal">Error</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Label" id="632437099120595641" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="470" y="59" />
    <node type="Action" id="632437099120595642" name="JTapiRejectCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="526" y="602">
      <linkto id="632437099120595643" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiIncomingCall: Rejecting call with callId: " + callId.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632437099120595643" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="648" y="603">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632439114879933208" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="581" y="131">
      <linkto id="632439114879933209" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnJTapiIncomingCall: JTapiAnswerCall action took default path</ap>
        <ap name="LogLevel" type="literal">Error</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632439114879933209" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="580" y="32">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632436358155658496" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632436358155658497" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632436358155658498" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632436358155658499" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference">originalTo</Properties>
    </node>
    <node type="Variable" id="632436358155658500" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference">from</Properties>
    </node>
    <node type="Variable" id="632436648769301439" name="callRecordsTable" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">callRecordsTable</Properties>
    </node>
    <node type="Variable" id="632436648769301442" name="bargeRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">bargeRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632436648769301448" name="mainCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" refType="reference">mainCallId</Properties>
    </node>
    <node type="Variable" id="632441376825748084" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference">deviceName</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallEstablished" startnode="632436358155658568" treenode="632436358155658569" appnode="632436358155658566" handlerfor="632436358155658565">
    <node type="Start" id="632436358155658568" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="164">
      <linkto id="632442536910120917" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632436358155658614" name="CreateCallRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="693" y="164">
      <linkto id="632442536910120119" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632442536910120118" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="LineId" type="variable">g_originalTo</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="DeviceName" type="variable">g_deviceName</ap>
        <rd field="Timestamp">g_timeStamp</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiCallEstablished:  New Call mode, creating call record</log>
        <log condition="success" on="true" level="Info" type="literal">OnJTapiCallEstablished: call record created</log>
        <log condition="default" on="true" level="Info" type="literal">OnJTapiCallEstablished: could not create call record</log>
      </Properties>
    </node>
    <node type="Action" id="632436630697700433" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="397" y="162">
      <linkto id="632436648769301207" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632436648769301579" type="Labeled" style="Bezier" ortho="true" label="bridge" />
      <linkto id="632442536910120084" type="Labeled" style="Bezier" ortho="true" label="new" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_callMode</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiCallEstablished: switching on g_callMode</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301207" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="397" y="67">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiCallEstablished: could not determine call type, exiting</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301579" name="JTapiConference" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="397" y="275">
      <linkto id="632436648769301580" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632436648769301206" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="VolatileCallId" type="variable">g_callId</ap>
        <ap name="CallId" type="variable">g_mainCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiCallEstablished: Attempting to conference callId: " + g_mainCallId.ToString() + " with volatile call id: " + g_callId.ToString()</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301580" name="Forward" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="540" y="275">
      <Properties final="true" type="appControl">
        <ap name="ToGuid" type="variable">g_bargeRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiCallEstablished: forwarding all events to barging app at routing guid: " + g_bargeRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301206" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="396" y="470">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiCallEstablished: JTapiConference didn not succeed, exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632439264744317305" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="250" y="162">
      <linkto id="632439264744317310" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632439980862420626" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_gotCallEstablished</ap>
        <log condition="entry" on="true" level="Info" type="literal">JTAPICallEstablished: Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632439264744317310" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="162">
      <linkto id="632436630697700433" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_gotCallEstablished</rd>
      </Properties>
    </node>
    <node type="Action" id="632439980862420626" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="250" y="283">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632442536910120084" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="476.942383" y="147" mx="527" my="163">
      <items count="1">
        <item text="DoIntegrityCheck" />
      </items>
      <linkto id="632436358155658614" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="FunctionName" type="literal">DoIntegrityCheck</ap>
      </Properties>
    </node>
    <node type="Action" id="632442536910120118" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="815" y="164">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJtapiCallEstablished: exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910120119" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="693" y="275">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiCallEstablished: exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910120917" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="127" y="164">
      <linkto id="632439264744317305" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="csharp">"OnJTapiCallEstablished: event received for device: " + deviceName</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Variable" id="632441376825748083" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" defaultInitWith="EMPTY" refType="reference">deviceName</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632436358155658573" treenode="632436358155658574" appnode="632436358155658571" handlerfor="632436358155658570">
    <node type="Start" id="632436358155658573" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="271">
      <linkto id="632436648769301209" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632436648769301209" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="120" y="271">
      <linkto id="632436648769301210" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632436648769301211" type="Labeled" style="Bezier" ortho="true" label="bridge" />
      <linkto id="632442536910120279" type="Labeled" style="Bezier" ortho="true" label="new" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">g_callMode</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: checking call mode</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301210" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="119" y="159">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: could not determine call type, bailing!</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301211" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="120" y="381">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: Hangup received in "bridge" mode, exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632437099120595634" name="RemoveCallRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="433" y="269">
      <linkto id="632439264744318191" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_to</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiHangup: Received hangup for new call, removing call record"</log>
      </Properties>
    </node>
    <node type="Action" id="632437143407008631" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="524" y="387">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632439264744318190" name="GetCallRecords" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="315" y="268">
      <linkto id="632437099120595634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_originalTo</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <rd field="CallRecordsTable">g_callRecordsTable</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: Retrieving call records</log>
      </Properties>
    </node>
    <node type="Action" id="632439264744318191" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="523" y="269">
      <linkto id="632437143407008631" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632439710441213793" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((g_callRecordsTable != null) &amp;&amp; (g_callRecordsTable.Rows.Count &gt; 0))</ap>
      </Properties>
    </node>
    <node type="Action" id="632439710441213793" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="615.90625" y="270">
      <linkto id="632439710441213796" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632437143407008631" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: checking if we have a valid routing guid for barge script</log>
	public static string Execute(ref string bargeRoutingGuid, DataTable g_callRecordsTable)
	{
		try
		{
			DataRow row = g_callRecordsTable.Rows[0];
			bargeRoutingGuid = Convert.ToString(row[CBargeCallRecords.BargeRoutingGuid]);
			if ((bargeRoutingGuid != null) &amp;&amp; (bargeRoutingGuid != string.Empty))
				return IApp.VALUE_SUCCESS;
			return IApp.VALUE_FAILURE;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Action" id="632439710441213796" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="717" y="271">
      <linkto id="632439710441213797" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <ap name="Cause" type="variable">cause</ap>
        <ap name="EventName" type="literal">Metreos.Providers.JTapi.JTapiHangup</ap>
        <ap name="ToGuid" type="variable">bargeRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiHangup: Sending JTapiHangup event to barge script at guid: " + bargeRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441213797" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="826.82605" y="269">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632442536910120279" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="220.768066" y="270">
      <linkto id="632439264744318190" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632442536910120280" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">cause</ap>
        <ap name="Value2" type="literal">NORMAL</ap>
      </Properties>
    </node>
    <node type="Action" id="632442536910120280" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="220.768066" y="380">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiHangup: exiting function, cause was: " + cause</log>
      </Properties>
    </node>
    <node type="Variable" id="632439710441213795" name="bargeRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">bargeRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632441376825748081" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" defaultInitWith="EMPTY" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632441376825748082" name="cause" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Cause" defaultInitWith="EMPTY" refType="reference">cause</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnIntegrityCheck" startnode="632436648769301190" treenode="632436648769301191" appnode="632436648769301188" handlerfor="632436648769301187">
    <node type="Start" id="632436648769301190" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="272">
      <linkto id="632436648769301199" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632436648769301199" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="137" y="273">
      <linkto id="632436648769301200" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632436648769301202" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_to</ap>
        <ap name="Value2" type="variable">lineId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIntegrityCheck: comparing received lineId: " + lineId + " to own lineId: " + g_to</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301200" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="254" y="273">
      <linkto id="632436648769301201" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="fromRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="EventName" type="literal">Metreos.Events.cBarge.IntegrityCheckFailed</ap>
        <ap name="ToGuid" type="variable">fromRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIntegrityCheck: lineIds were mismatched, sending IntegrityCheckFailed event to sender w/ routing guid: " + fromRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301201" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="369" y="274">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632436648769301202" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="385">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIntegrityCheck: lineIds were not mismatched, ignoring request</log>
      </Properties>
    </node>
    <node type="Variable" id="632436648769301197" name="lineId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="lineId" refType="reference">lineId</Properties>
    </node>
    <node type="Variable" id="632436648769301198" name="fromRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="fromRoutingGuid" refType="reference">fromRoutingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnIntegrityCheckFailed" startnode="632436648769301195" treenode="632436648769301196" appnode="632436648769301193" handlerfor="632436648769301192">
    <node type="Start" id="632436648769301195" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="324">
      <linkto id="632436648769301204" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632436648769301204" name="RemoveCallRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="156" y="324">
      <linkto id="632436648769301205" type="Labeled" style="Bezier" label="success" />
      <linkto id="632436648769301205" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_to</ap>
        <ap name="RoutingGuid" type="variable">fromRoutingGuid</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">"OnIntegrityCheckFailed: received response from script: " + fromRoutingGuid</log>
        <log condition="success" on="true" level="Info" type="csharp">"OnIntegrityCheckFailed: deleted call record with lineId: " + g_to + " and routing guid: " + fromRoutingGuid</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnIntegrityCheckFailed: failed to delete call record with lineId: " + g_to + " and routing guid: " + fromRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632436648769301205" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="370" y="322">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Variable" id="632436648769301203" name="fromRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="fromRoutingGuid" refType="reference">fromRoutingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallActive" startnode="632441376825748088" treenode="632441376825748089" appnode="632441376825748086" handlerfor="632441376825748085">
    <node type="Start" id="632441376825748088" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="128">
      <linkto id="632441376825748095" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632441376825748095" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="129">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632441376825748097" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" defaultInitWith="EMPTY" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632441376825748098" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallInactive" startnode="632441376825748093" treenode="632441376825748094" appnode="632441376825748091" handlerfor="632441376825748090">
    <node type="Start" id="632441376825748093" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="76" y="212">
      <linkto id="632441376825748096" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632441376825748096" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="205" y="214">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632441376825748099" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632441376825748100" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Long" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DoIntegrityCheck" startnode="632442536910120087" treenode="632442536910120088" appnode="632442536910120085" handlerfor="632441376825748090">
    <node type="Loop" id="632442536910120089" name="Loop" text="loop (expr)" cx="493.000031" cy="270" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="431.132172" y="164" mx="678" my="299">
      <linkto id="632442536910120090" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632442536910120102" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_callRecordsTable.Rows.Count;</Properties>
    </node>
    <node type="Start" id="632442536910120087" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="116" y="300">
      <linkto id="632442536910120099" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632442536910120090" name="CustomCode" container="632442536910120089" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="519.0742" y="211">
      <linkto id="632442536910120092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632442536910120097" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
	public static string Execute(DataTable g_callRecordsTable, ref string foreignLineId, ref string foreignRoutingGuid, int loopIndex)
	{
		try
		{
			DataRow row = g_callRecordsTable.Rows[loopIndex];
			foreignLineId = Convert.ToString(row[CBargeCallRecords.LineId]);
			foreignRoutingGuid = Convert.ToString(row[CBargeCallRecords.RoutingGuid]);
			return IApp.VALUE_SUCCESS;
		}
		catch
		{
			return IApp.VALUE_FAILURE;
		}
	}
</Properties>
    </node>
    <node type="Label" id="632442536910120091" text="E" container="632442536910120089" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="839.1322" y="299">
      <linkto id="632442536910120089" port="3" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632442536910120092" text="E" container="632442536910120089" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="521.1322" y="308" />
    <node type="Action" id="632442536910120093" name="SendEvent" container="632442536910120089" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="717.1322" y="210">
      <linkto id="632442536910120094" type="Labeled" style="Bezier" label="failure" />
      <linkto id="632442536910120095" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632442536910120094" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl">
        <ap name="lineId" type="variable">foreignLineId</ap>
        <ap name="fromRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="EventName" type="literal">Metreos.Events.cBarge.IntegrityCheck</ap>
        <ap name="ToGuid" type="variable">foreignRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"DoIntegrityCheck: sending IntegrityCheck event to routingGuid: " + foreignRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910120094" name="RemoveCallRecord" container="632442536910120089" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="715.1322" y="354">
      <linkto id="632442536910120096" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">foreignLineId</ap>
        <ap name="RoutingGuid" type="variable">foreignRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"DoIntegrityCheck: Removing invalid call record with lineId: " + foreignLineId + " and guid: " + foreignRoutingGuid</log>
      </Properties>
    </node>
    <node type="Label" id="632442536910120095" text="E" container="632442536910120089" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="831.1322" y="209" />
    <node type="Label" id="632442536910120096" text="E" container="632442536910120089" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="833.1322" y="353" />
    <node type="Action" id="632442536910120097" name="Compare" container="632442536910120089" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="615.1322" y="210">
      <linkto id="632442536910120093" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632442536910120098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">foreignRoutingGuid</ap>
        <ap name="Value2" type="variable">g_routingGuid</ap>
      </Properties>
    </node>
    <node type="Label" id="632442536910120098" text="E" container="632442536910120089" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="616.1322" y="309" />
    <node type="Action" id="632442536910120099" name="GetCallRecords" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="244.132172" y="300">
      <linkto id="632442536910120100" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_originalTo</ap>
        <rd field="CallRecordsTable">g_callRecordsTable</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"DoIntegrityCheck: retrieving call records for lineId: " + g_originalTo</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910120100" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="345.132172" y="300">
      <linkto id="632442536910120089" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632442536910120101" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((g_callRecordsTable != null) &amp;&amp; (g_callRecordsTable.Rows.Count &gt; 0))</ap>
      </Properties>
    </node>
    <node type="Action" id="632442536910120101" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="344.132172" y="404">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">DoIntegrityCheck: No stale records detected, exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632442536910120102" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1052.20642" y="297">
      <Properties final="true" type="appControl">
        <ap name="ReturnValue" type="literal">default</ap>
        <log condition="entry" on="true" level="Info" type="literal">DoIntegrityCheck: exiting function</log>
      </Properties>
    </node>
    <node type="Variable" id="632442536910120116" name="foreignLineId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">foreignLineId</Properties>
    </node>
    <node type="Variable" id="632442536910120117" name="foreignRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">foreignRoutingGuid</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>