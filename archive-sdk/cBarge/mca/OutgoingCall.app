<Application name="OutgoingCall" trigger="Metreos.Providers.JTapi.JTapiMakeCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OutgoingCall">
    <outline>
      <treenode type="evh" id="632436358155658491" level="1" text="Metreos.Providers.JTapi.JTapiMakeCall (trigger): OnJTapiMakeCall">
        <node type="function" name="OnJTapiMakeCall" id="632436358155658488" path="Metreos.StockTools" />
        <node type="event" name="JTapiMakeCall" id="632436358155658487" path="Metreos.Providers.JTapi.JTapiMakeCall" trigger="true" />
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
    </outline>
    <variables>
      <treenode text="g_callId" id="632442536910120304" vid="632436358155658554">
        <Properties type="Long">g_callId</Properties>
      </treenode>
      <treenode text="g_to" id="632442536910120306" vid="632436358155658556">
        <Properties type="String">g_to</Properties>
      </treenode>
      <treenode text="g_from" id="632442536910120308" vid="632436358155658560">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632442536910120310" vid="632436358155658562">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_timeStamp" id="632442536910120312" vid="632436358155658616">
        <Properties type="DateTime">g_timeStamp</Properties>
      </treenode>
      <treenode text="g_callRecordsTable" id="632442536910120314" vid="632436358155658619">
        <Properties type="DataTable">g_callRecordsTable</Properties>
      </treenode>
      <treenode text="g_gotCallEstablished" id="632442536910120316" vid="632439264744317393">
        <Properties type="Bool" defaultInitWith="false">g_gotCallEstablished</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiMakeCall" startnode="632436358155658490" treenode="632436358155658491" appnode="632436358155658488" handlerfor="632436358155658487">
    <node type="Start" id="632436358155658490" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="58" y="343">
      <linkto id="632439224396371108" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632436358155658553" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="311" y="344">
      <linkto id="632436648769301438" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="variable">callId</ap>
        <ap name="Value2" type="variable">to</ap>
        <ap name="Value3" type="variable">from</ap>
        <ap name="Value4" type="variable">routingGuid</ap>
        <rd field="ResultData">g_callId</rd>
        <rd field="ResultData2">g_to</rd>
        <rd field="ResultData3">g_from</rd>
        <rd field="ResultData4">g_routingGuid</rd>
      </Properties>
    </node>
    <node type="Action" id="632436630697700427" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="227" y="345">
      <linkto id="632436358155658553" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">OnJTapiMakeCall: New call detected</ap>
        <ap name="LogLevel" type="literal">Info</ap>
        <ap name="ApplicationName" type="literal">cBarge</ap>
      </Properties>
    </node>
    <node type="Action" id="632436648769301438" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="434" y="345">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632439224396371108" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="137" y="343">
      <linkto id="632436630697700427" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(ref DataTable g_callRecordsTable)
	{
		g_callRecordsTable = new DataTable();
		return IApp.VALUE_SUCCESS;
	}
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
    <node type="Variable" id="632436358155658500" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiCallEstablished" activetab="true" startnode="632436358155658568" treenode="632436358155658569" appnode="632436358155658566" handlerfor="632436358155658565">
    <node type="Loop" id="632436358155658623" name="Loop" text="loop (expr)" cx="493" cy="270" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="555" y="32" mx="802" my="167">
      <linkto id="632436358155658626" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632436358155659207" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_callRecordsTable.Rows.Count;</Properties>
    </node>
    <node type="Start" id="632436358155658568" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="167">
      <linkto id="632439264744317385" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632436358155658614" name="CreateCallRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="288" y="168">
      <linkto id="632436358155658618" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="LineId" type="variable">g_from</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="DeviceName" type="variable">deviceName</ap>
        <rd field="Timestamp">g_timeStamp</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiCallEstablished:  New Call mode, creating call record for lineId: " + g_from</log>
      </Properties>
    </node>
    <node type="Action" id="632436358155658618" name="GetCallRecords" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="396" y="169">
      <linkto id="632436358155658622" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_from</ap>
        <rd field="CallRecordsTable">g_callRecordsTable</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiCallEstablished: retrieving call records for lineId: " + g_from</log>
      </Properties>
    </node>
    <node type="Action" id="632436358155658622" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="479" y="168">
      <linkto id="632436358155658623" port="1" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632436358155659206" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((g_callRecordsTable != null) &amp;&amp; (g_callRecordsTable.Rows.Count &gt; 0))</ap>
      </Properties>
    </node>
    <node type="Action" id="632436358155658626" name="CustomCode" container="632436358155658623" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="643.942" y="80">
      <linkto id="632436358155658628" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632436358155659208" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Warning" type="literal">OnJTapiCallEstablished: the number of call records retrieved was 0, should be at least one. Conferencing third party may not be possible.</log>
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
    <node type="Label" id="632436358155658627" text="E" container="632436358155658623" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1000" y="162">
      <linkto id="632436358155658623" port="3" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632436358155658628" text="E" container="632436358155658623" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="645" y="178" />
    <node type="Action" id="632436358155658631" name="SendEvent" container="632436358155658623" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="841" y="80">
      <linkto id="632436358155658632" type="Labeled" style="Bezier" label="failure" />
      <linkto id="632436358155658633" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632436358155658632" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl">
        <ap name="lineId" type="variable">foreignLineId</ap>
        <ap name="fromRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="EventName" type="literal">Metreos.Events.cBarge.IntegrityCheck</ap>
        <ap name="ToGuid" type="variable">foreignRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiCallEstablished: sending IntegrityCheck event to routingGuid: " + foreignRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632436358155658632" name="RemoveCallRecord" container="632436358155658623" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="839" y="224">
      <linkto id="632436358155659204" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">foreignLineId</ap>
        <ap name="RoutingGuid" type="variable">foreignRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJtapiCallEstablished: Removing invalid call record with lineId: " + foreignLineId + " and guid: " + foreignRoutingGuid</log>
      </Properties>
    </node>
    <node type="Label" id="632436358155658633" text="E" container="632436358155658623" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="955" y="79" />
    <node type="Label" id="632436358155659204" text="E" container="632436358155658623" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="957" y="223" />
    <node type="Action" id="632436358155659206" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="481" y="267">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Warning" type="literal">OnJTapiCallEstablished: No stale records detected, exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632436358155659207" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1139.07422" y="167">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiCallEstablished: exiting function</log>
      </Properties>
    </node>
    <node type="Action" id="632436358155659208" name="Compare" container="632436358155658623" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="739" y="80">
      <linkto id="632436358155658631" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632436358155659209" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">foreignRoutingGuid</ap>
        <ap name="Value2" type="variable">g_routingGuid</ap>
      </Properties>
    </node>
    <node type="Label" id="632436358155659209" text="E" container="632436358155658623" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="740" y="179" />
    <node type="Action" id="632439264744317385" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="114.4707" y="167">
      <linkto id="632439264744317388" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632439980862420713" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="variable">g_gotCallEstablished</ap>
      </Properties>
    </node>
    <node type="Action" id="632439264744317388" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="202.4707" y="168">
      <linkto id="632436358155658614" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_gotCallEstablished</rd>
      </Properties>
    </node>
    <node type="Action" id="632439980862420713" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="113" y="279">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632436358155658629" name="foreignRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">foreignRoutingGuid</Properties>
    </node>
    <node type="Variable" id="632436358155658630" name="foreignLineId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">foreignLineId</Properties>
    </node>
    <node type="Variable" id="632442536910120366" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="DeviceName" refType="reference">deviceName</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnJTapiHangup" startnode="632436358155658573" treenode="632436358155658574" appnode="632436358155658571" handlerfor="632436358155658570">
    <node type="Start" id="632436358155658573" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="271">
      <linkto id="632439710441214574" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632437099120595634" name="RemoveCallRecord" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="265" y="270">
      <linkto id="632439710441214575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_from</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiHangup: Received hangup for new call, removing call record"</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441214573" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="375.132141" y="386">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: exiting script</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441214574" name="GetCallRecords" class="MaxActionNode" group="" path="Metreos.Native.CBarge" x="124.132156" y="270">
      <linkto id="632437099120595634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="LineId" type="variable">g_from</ap>
        <rd field="CallRecordsTable">g_callRecordsTable</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: Retrieving call records</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441214575" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="373.132141" y="269">
      <linkto id="632439710441214573" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632439710441214576" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">((g_callRecordsTable != null) &amp;&amp; (g_callRecordsTable.Rows.Count &gt; 0))</ap>
      </Properties>
    </node>
    <node type="Action" id="632439710441214576" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="467.0384" y="269">
      <linkto id="632439710441214577" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632439710441214573" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnJTapiHangup: checking if we have a valid routing guid for barge script</log>
	public static string Execute(ref string bargeRoutingGuid, DataTable g_callRecordsTable)
	{
		try
		{
			DataRow row = g_callRecordsTable.Rows[0];
			bargeRoutingGuid = Convert.ToString(row[CBargeCallRecords.BargeRoutingGuid]);
			if (bargeRoutingGuid != null)
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
    <node type="Action" id="632439710441214577" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="569.132141" y="268">
      <linkto id="632439710441214578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="CallId" type="variable">g_callId</ap>
        <ap name="EventName" type="literal">Metreos.Providers.JTapi.JTapiHangup</ap>
        <ap name="ToGuid" type="variable">bargeRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnJTapiHangup: Sending JTapiHangup event to barge script at guid: " + bargeRoutingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632439710441214578" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="677.9582" y="268">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632439710441214586" name="bargeRoutingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">bargeRoutingGuid</Properties>
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
        <ap name="Value1" type="variable">g_from</ap>
        <ap name="Value2" type="variable">lineId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIntegrityCheck: comparing received lineId: " + lineId + " to own lineId: " + g_from</log>
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
        <ap name="LineId" type="variable">g_from</ap>
        <ap name="RoutingGuid" type="variable">fromRoutingGuid</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">"OnIntegrityCheckFailed: received response from script: " + fromRoutingGuid</log>
        <log condition="success" on="true" level="Info" type="csharp">"OnIntegrityCheckFailed: deleted call record with lineId: " + g_from + " and routing guid: " + fromRoutingGuid</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnIntegrityCheckFailed: failed to delete call record with lineId: " + g_from + " and routing guid: " + fromRoutingGuid</log>
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
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>