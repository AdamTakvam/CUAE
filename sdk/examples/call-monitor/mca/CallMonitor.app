<Application name="CallMonitor" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="CallMonitor">
    <outline>
      <treenode type="evh" id="632605770032726811" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632605770032726808" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632605770032726807" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726849" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632605770032726846" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632605770032726845" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632798333970156816" actid="632606843862474809" />
          <ref id="632798333970156827" actid="632609223498596665" />
          <ref id="632798333970156875" actid="632617966013889956" />
          <ref id="632798333970156892" actid="632605817629675036" />
          <ref id="632798333970156918" actid="632605881876499391" />
          <ref id="632798333970156921" actid="632605881876499394" />
          <ref id="632798333970156942" actid="632608290328114522" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726854" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632605770032726851" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632605770032726850" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632798333970156817" actid="632606843862474809" />
          <ref id="632798333970156828" actid="632609223498596665" />
          <ref id="632798333970156876" actid="632617966013889956" />
          <ref id="632798333970156893" actid="632605817629675036" />
          <ref id="632798333970156919" actid="632605881876499391" />
          <ref id="632798333970156922" actid="632605881876499394" />
          <ref id="632798333970156943" actid="632608290328114522" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605817629675007" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632605817629675004" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632605817629675003" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632798333970156900" actid="632605817629675060" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605817629675012" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632605817629675009" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632605817629675008" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632798333970156901" actid="632605817629675060" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605881876499780" level="2" text="Metreos.Providers.PCapService.ActiveCallList_Complete: OnActiveCallList_Complete">
        <node type="function" name="OnActiveCallList_Complete" id="632605881876499777" path="Metreos.StockTools" />
        <node type="event" name="ActiveCallList_Complete" id="632605881876499776" path="Metreos.Providers.PCapService.ActiveCallList_Complete" />
        <references>
          <ref id="632798333970156933" actid="632605881876499786" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605881876499785" level="2" text="Metreos.Providers.PCapService.ActiveCallList_Failed: OnActiveCallList_Failed">
        <node type="function" name="OnActiveCallList_Failed" id="632605881876499782" path="Metreos.StockTools" />
        <node type="event" name="ActiveCallList_Failed" id="632605881876499781" path="Metreos.Providers.PCapService.ActiveCallList_Failed" />
        <references>
          <ref id="632798333970156934" actid="632605881876499786" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632606225558163519" level="2" text="Metreos.Providers.PCapService.MonitorCall_Complete: OnMonitorCall_Complete">
        <node type="function" name="OnMonitorCall_Complete" id="632606225558163516" path="Metreos.StockTools" />
        <node type="event" name="MonitorCall_Complete" id="632606225558163515" path="Metreos.Providers.PCapService.MonitorCall_Complete" />
        <references>
          <ref id="632798333970156803" actid="632606225558163525" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632606225558163524" level="2" text="Metreos.Providers.PCapService.MonitorCall_Failed: OnMonitorCall_Failed">
        <node type="function" name="OnMonitorCall_Failed" id="632606225558163521" path="Metreos.StockTools" />
        <calls>
          <ref actid="632617966013889952" />
        </calls>
        <node type="event" name="MonitorCall_Failed" id="632606225558163520" path="Metreos.Providers.PCapService.MonitorCall_Failed" />
        <references>
          <ref id="632798333970156804" actid="632606225558163525" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726823" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632605770032726820" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632605770032726819" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726828" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632605770032726825" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632605770032726824" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726833" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632605770032726830" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632605770032726829" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726838" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632605770032726835" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632605770032726834" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726843" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632605770032726840" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632605770032726839" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632605817629675035" level="1" text="RequestAuthentication">
        <node type="function" name="RequestAuthentication" id="632605817629675032" path="Metreos.StockTools" />
        <calls>
          <ref actid="632605817629675050" />
          <ref actid="632605817629675031" />
          <ref actid="632605817629675063" />
          <ref actid="632605881876499380" />
          <ref actid="632605881876499404" />
        </calls>
      </treenode>
      <treenode type="fun" id="632605881876499425" level="1" text="GetActiveCalls">
        <node type="function" name="GetActiveCalls" id="632605881876499422" path="Metreos.StockTools" />
        <calls>
          <ref actid="632605881876499421" />
          <ref actid="632606843862474814" />
          <ref actid="632605881876500335" />
          <ref actid="632608290328114521" />
        </calls>
      </treenode>
      <treenode type="fun" id="632605817629675024" level="1" text="Exit">
        <node type="function" name="Exit" id="632605817629675021" path="Metreos.StockTools" />
        <calls>
          <ref actid="632605817629675049" />
          <ref actid="632605817629675020" />
          <ref actid="632606843862474824" />
          <ref actid="632606843862474830" />
          <ref actid="632605881876499384" />
          <ref actid="632606843862474813" />
          <ref actid="632609223498596669" />
          <ref actid="632617966013889959" />
          <ref actid="632606225558163532" />
          <ref actid="632605881876499401" />
          <ref actid="632608290328114526" />
        </calls>
      </treenode>
      <treenode type="fun" id="632609939149282112" level="1" text="GetSidFromIp">
        <node type="function" name="GetSidFromIp" id="632609939149282109" path="Metreos.StockTools" />
        <calls>
          <ref actid="632609939149282108" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_originalTo" id="632798333970156673" vid="632605881876499427">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_callId" id="632798333970156675" vid="632605770032726814">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_mmsID" id="632798333970156677" vid="632606735182334555">
        <Properties type="UInt">g_mmsID</Properties>
      </treenode>
      <treenode text="g_mediaServerIP" id="632798333970156679" vid="632606518029439009">
        <Properties type="String">g_mediaServerIP</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632798333970156681" vid="632605770032726816">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632798333970156683" vid="632605770032726860">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_displayName" id="632798333970156685" vid="632605770032726858">
        <Properties type="String" initWith="DisplayName">g_displayName</Properties>
      </treenode>
      <treenode text="g_failureThreshold" id="632798333970156687" vid="632605817629675039">
        <Properties type="UInt" initWith="OperationFailureThresh">g_failureThreshold</Properties>
      </treenode>
      <treenode text="g_listFailureCounter" id="632798333970156689" vid="632605817629675045">
        <Properties type="UInt" defaultInitWith="0">g_listFailureCounter</Properties>
      </treenode>
      <treenode text="g_monitorFailureCounter" id="632798333970156691" vid="632617944194869416">
        <Properties type="UInt" defaultInitWith="0">g_monitorFailureCounter</Properties>
      </treenode>
      <treenode text="g_passCode" id="632798333970156693" vid="632605881876499375">
        <Properties type="String" initWith="PassCode">g_passCode</Properties>
      </treenode>
      <treenode text="g_maxFailedLogins" id="632798333970156695" vid="632605881876499389">
        <Properties type="UInt" initWith="LoginFailureThresh">g_maxFailedLogins</Properties>
      </treenode>
      <treenode text="g_failedLogins" id="632798333970156697" vid="632605881876499381">
        <Properties type="UInt" defaultInitWith="0">g_failedLogins</Properties>
      </treenode>
      <treenode text="g_monitoredDN" id="632798333970156699" vid="632605881876500340">
        <Properties type="ArrayList" initWith="MonitoredDN">g_monitoredDN</Properties>
      </treenode>
      <treenode text="g_monitorCallId" id="632798333970156701" vid="632606225558163513">
        <Properties type="UInt" defaultInitWith="0">g_monitorCallId</Properties>
      </treenode>
      <treenode text="g_monitorConnectionId1" id="632798333970156703" vid="632606225558163539">
        <Properties type="String">g_monitorConnectionId1</Properties>
      </treenode>
      <treenode text="g_monitorConnectionId2" id="632798333970156705" vid="632606225558163541">
        <Properties type="String">g_monitorConnectionId2</Properties>
      </treenode>
      <treenode text="g_monitorConn1Created" id="632798333970156707" vid="632606735182334296">
        <Properties type="Bool" defaultInitWith="false">g_monitorConn1Created</Properties>
      </treenode>
      <treenode text="g_monitorConn2Created" id="632798333970156709" vid="632606735182334298">
        <Properties type="Bool" defaultInitWith="false">g_monitorConn2Created</Properties>
      </treenode>
      <treenode text="g_isCallerAnswered" id="632798333970156711" vid="632605817629675054">
        <Properties type="Bool" defaultInitWith="false">g_isCallerAnswered</Properties>
      </treenode>
      <treenode text="g_isCallMonitored" id="632798333970156713" vid="632605817629675056">
        <Properties type="Bool" defaultInitWith="false">g_isCallMonitored</Properties>
      </treenode>
      <treenode text="g_fromNumber" id="632798333970156715" vid="632609914127331545">
        <Properties type="String">g_fromNumber</Properties>
      </treenode>
      <treenode text="g_DB_Name" id="632798333970156717" vid="632609939149282097">
        <Properties type="String" initWith="DatabaseName">g_DB_Name</Properties>
      </treenode>
      <treenode text="g_DB_Username" id="632798333970156719" vid="632609939149282099">
        <Properties type="String" initWith="Username">g_DB_Username</Properties>
      </treenode>
      <treenode text="g_DB_Password" id="632798333970156721" vid="632609939149282101">
        <Properties type="String" initWith="Password">g_DB_Password</Properties>
      </treenode>
      <treenode text="g_DB_Server" id="632798333970156723" vid="632609939149282103">
        <Properties type="String" initWith="Server">g_DB_Server</Properties>
      </treenode>
      <treenode text="g_DB_Port" id="632798333970156725" vid="632609939149282105">
        <Properties type="String" initWith="Port">g_DB_Port</Properties>
      </treenode>
      <treenode text="g_monitoredIp" id="632798333970156727" vid="632609939149282125">
        <Properties type="String">g_monitoredIp</Properties>
      </treenode>
      <treenode text="AuthFailedAudio" id="632798333970156729" vid="632611925512542955">
        <Properties type="String" initWith="AuthFailedAudio">AuthFailedAudio</Properties>
      </treenode>
      <treenode text="RequestAuthAudio" id="632798333970156731" vid="632611925512542957">
        <Properties type="String" initWith="RequestAuthAudio">RequestAuthAudio</Properties>
      </treenode>
      <treenode text="GoodByeAudio" id="632798333970156733" vid="632611925512542959">
        <Properties type="String" initWith="GoodByeAudio">GoodByeAudio</Properties>
      </treenode>
      <treenode text="NoActiveCallsAudio" id="632798333970156735" vid="632611925512542961">
        <Properties type="String" initWith="NoActiveCallsAudio">NoActiveCallsAudio</Properties>
      </treenode>
      <treenode text="NoDeviceAssocAudio" id="632798333970156737" vid="632611925512542963">
        <Properties type="String" initWith="NoDeviceAssocAudio">NoDeviceAssocAudio</Properties>
      </treenode>
      <treenode text="MonitorsFailedAudio" id="632798333970156739" vid="632617944194869418">
        <Properties type="String" initWith="MonitorsFailedAudio">MonitorsFailedAudio</Properties>
      </treenode>
      <treenode text="PoundSignAudio" id="632798333970156741" vid="632611925512542965">
        <Properties type="String" initWith="PoundSignAudio">PoundSignAudio</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632605770032726810" treenode="632605770032726811" appnode="632605770032726808" handlerfor="632605770032726807">
    <node type="Start" id="632605770032726810" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="159">
      <linkto id="632605881876499430" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605770032726818" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="364" y="158">
      <linkto id="632605770032726844" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605817629675058" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="variable">g_displayName</ap>
        <ap name="WaitForMedia" type="csharp">true</ap>
        <ap name="Conference" type="literal">false</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="MmsId">g_mmsID</rd>
        <rd field="ConnectionId">g_connectionId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: Answering call with callId: " + callId</log>
        <log condition="default" on="true" level="Warning" type="csharp">"OnIncomingCall: Failed to answer call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632605770032726844" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="366" y="305">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: CallMonitor script exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632605770032726862" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="956" y="157">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605817629675049" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="756.8258" y="288" mx="794" my="304">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632605770032726862" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnIncomingCall: calling Exit function</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675050" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="566.551758" y="143" mx="631" my="159">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632605817629675049" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605770032726862" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnIncomingCall: Invoking RequestAuthentication</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675058" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="159">
      <linkto id="632605817629675050" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">fromNumber</ap>
        <rd field="ResultData">g_isCallerAnswered</rd>
        <rd field="ResultData2">g_fromNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632605881876499430" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="186" y="159">
      <linkto id="632605770032726818" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605881876499431" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_originalTo, string originalTo, LogWriter log)
{
	// Checking that the originalTo number is available.
	if (originalTo == null || originalTo.Equals(string.Empty))
	{
		log.Write(TraceLevel.Warning, "IncomingCall.OriginalTo is either null or empty.");
		return IApp.VALUE_FAILURE;
	}

	g_originalTo = originalTo;	
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632605881876499431" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="188" y="305">
      <linkto id="632605770032726844" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: Rejecting incoming call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Variable" id="632605770032726812" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632605881876499429" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.CallControl.IncomingCall">originalTo</Properties>
    </node>
    <node type="Variable" id="632609914127331544" name="fromNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">fromNumber</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632605770032726848" treenode="632605770032726849" appnode="632605770032726846" handlerfor="632605770032726845">
    <node type="Start" id="632605770032726848" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="58" y="252">
      <linkto id="632605817629675016" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605817629675016" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="146" y="253">
      <linkto id="632605817629675017" type="Labeled" style="Bezier" ortho="true" label="auth_req" />
      <linkto id="632606843862474818" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <linkto id="632606843862474819" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnPlay_Complete: UserData is: " + userData</log>
      </Properties>
    </node>
    <node type="Label" id="632605817629675017" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="279" y="251" />
    <node type="Label" id="632605817629675018" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="378" y="156">
      <linkto id="632605817629675025" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605817629675019" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="880" y="153">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605817629675020" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="672.825867" y="308" mx="710" my="324">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632605817629675019" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling Exit function
</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675025" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="536" y="156">
      <linkto id="632605817629675027" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632605817629675031" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605817629675063" type="Labeled" style="Bezier" label="eod" />
      <linkto id="632605817629675063" type="Labeled" style="Bezier" label="maxdigits" />
      <linkto id="632605817629675027" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnPlay_Complete: Termination condition: " + termCond</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675027" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="389" y="325">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605817629675031" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="477.551758" y="308" mx="542" my="324">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632605817629675027" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605817629675020" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling RequestAuthentication(Action = play) function
</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675063" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="642.551758" y="138" mx="707" my="154">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632605817629675019" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605817629675020" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">digits</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling RequestAuthentication(Action = digits) function</log>
      </Properties>
    </node>
    <node type="Label" id="632606843862474818" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="271" y="405" />
    <node type="Action" id="632606843862474819" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="148" y="402">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632606843862474820" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="385" y="596">
      <linkto id="632606843862474822" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632606843862474821" text="termCond&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="474" y="82" />
    <node type="Action" id="632606843862474822" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="485" y="596">
      <linkto id="632606843862474823" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632606843862474824" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632606843862474823" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="638" y="597">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606843862474824" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="451.825867" y="706" mx="489" my="722">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632606843862474823" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Variable" id="632605817629675002" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632605817629675026" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632605770032726853" treenode="632605770032726854" appnode="632605770032726851" handlerfor="632605770032726850">
    <node type="Start" id="632605770032726853" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="163">
      <linkto id="632606843862474826" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632606225558163508" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="182" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606843862474826" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="116" y="164">
      <linkto id="632606225558163508" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632606843862474830" type="Labeled" style="Bezier" label="auth_req" />
      <linkto id="632606843862474830" type="Labeled" style="Bezier" label="exit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnPlay_Failed: The value of userData is: " + userData
</log>
        <log condition="default" on="true" level="Warning" type="literal">OnPlay_Failed: Default branch taken on UserData switch - Play action missing proper UserData value?</log>
      </Properties>
    </node>
    <node type="Action" id="632606843862474830" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="203.825851" y="152" mx="241" my="168">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632606225558163508" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Variable" id="632606843862474825" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632605817629675006" treenode="632605817629675007" appnode="632605817629675004" handlerfor="632605817629675003">
    <node type="Start" id="632605817629675006" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="57" y="244">
      <linkto id="632605881876499372" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605881876499372" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="244">
      <linkto id="632605881876499374" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData is: " + userData
</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499374" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="311" y="245">
      <linkto id="632605881876499378" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632605881876499403" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632605881876499404" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605881876499403" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: Termination Condition is: " + termCond</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499378" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="511" y="246">
      <linkto id="632605881876499379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string digits, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "OnGatherDigits_Complete: cleaning up received digits string.");

	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);
	return IApp.VALUE_SUCCESS;	
}
</Properties>
    </node>
    <node type="Action" id="632605881876499379" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="665" y="247">
      <linkto id="632605881876499380" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605881876499421" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">digits</ap>
        <ap name="Value2" type="variable">g_passCode</ap>
        <log condition="default" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: User input does not match PassCode setting.</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499380" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="606.551758" y="407" mx="671" my="423">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632605881876499383" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605881876499384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">auth_fail</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Calling RequestAuthentication(Action = auth_fail)</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499383" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="942" y="423">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605881876499384" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="776.8258" y="492" mx="814" my="508">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632605881876499383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Calling Exit function</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499403" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="310" y="125">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605881876499404" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="249.551758" y="356" mx="314" my="372">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632605881876499405" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605881876499407" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Calling RequestAuthentication(Action = play)</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499405" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="440" y="372">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632605881876499406" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="811.4707" y="628">
      <linkto id="632605881876499384" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632605881876499407" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="311.4707" y="497" />
    <node type="Action" id="632605881876499421" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="899.3779" y="231" mx="943" my="247">
      <items count="1">
        <item text="GetActiveCalls" />
      </items>
      <linkto id="632605881876499383" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">GetActiveCalls</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Calling GetActiveCalls function</log>
      </Properties>
    </node>
    <node type="Comment" id="632608290328114535" text="Check if user input matches defined passcode." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="368" y="32" />
    <node type="Variable" id="632605817629675078" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632605817629675079" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632605881876499377" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632605817629675011" treenode="632605817629675012" appnode="632605817629675009" handlerfor="632605817629675008">
    <node type="Start" id="632605817629675011" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="75" y="224">
      <linkto id="632606225558163509" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632606225558163509" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="285" y="224">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632608290328114534" text="Needs error handling." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="297" y="114" />
  </canvas>
  <canvas type="Function" name="OnActiveCallList_Complete" startnode="632605881876499779" treenode="632605881876499780" appnode="632605881876499777" handlerfor="632605881876499776">
    <node type="Start" id="632605881876499779" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="50" y="270">
      <linkto id="632605881876500334" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605881876500334" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="147" y="270">
      <linkto id="632605881876500339" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">g_listFailureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632605881876500339" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="256" y="270">
      <linkto id="632606735182334550" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632606843862474807" type="Labeled" style="Bezier" ortho="true" label="NoAssoc" />
      <linkto id="632606843862474814" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632609223498596663" type="Labeled" style="Bezier" ortho="true" label="NoActiveDevices" />
      <Properties language="csharp">
public static string Execute(ref uint g_monitorCallId, ArrayList g_monitoredDN, Hashtable activeCalls, string g_originalTo, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "OnActiveCallList_Complete: Looking for suitable call to monitor.");

	// Make sure DNs are defined
	bool success = false;
	if (g_monitoredDN== null)
		log.Write(TraceLevel.Warning, "OnActiveCallList_Complete: Monitored DNs not defined.");
	else if (g_monitoredDN.Count == 0)
		log.Write(TraceLevel.Warning, "OnActiveCallList_Complete: Monitored DNs is empty.");	
	else
		success = true;

	if (!success)
		return "NoAssoc";
	else
		success = false;

	// This ArrayList will contain UInt32 CallIds for calls that are currently active on the set of devices that 
	// corresponds to the DID
	ArrayList matchedCallIdList = new ArrayList();

	// Here we iterate through all the DNs and check to see if the activeCalls hash
	// has a corresponding entry. If so, add the UInt32 callId to the matchedCallIdList.
	foreach (string dn in g_monitoredDN)
	{
		// Trim any whitespace from device name
		string trimmedDN = dn.Trim();

		log.Write(TraceLevel.Verbose, "OnActiveCallList_Complete: Looking for active calls for device: " + trimmedDN);
		if (activeCalls.ContainsKey(trimmedDN))
		{
			uint tempCallId = Convert.ToUInt32(activeCalls[trimmedDN]);
			log.Write(TraceLevel.Verbose, "OnActiveCallList_Complete: Found active call with callId: " + tempCallId);
			matchedCallIdList.Add(tempCallId);
		}
	}

	// If there are currently no active calls on any of the devices that correspond to the dialed DID, we
	// return the "NoActiveDevices" result value.
	if (matchedCallIdList.Count == 0)
	{
		log.Write(TraceLevel.Verbose, "OnActiveCallList_Complete: Did not match any active calls to devices in DID group.");
		return "NoActiveDevices";
	}

			
	// Pseudo-random generator used to pick one of the matched active calls
	System.Random numGen = new System.Random();
	int random = numGen.Next(matchedCallIdList.Count);

	try
	{
		g_monitorCallId = Convert.ToUInt32(matchedCallIdList[random]);
		log.Write(TraceLevel.Verbose, "OnActiveCallList_Complete: The value of g_monitorCallId is: " + g_monitorCallId);
	}
	catch
	{
		log.Write(TraceLevel.Warning, "OnActiveCallList_Complete: Could not cast monitor callId to UInt32.");
		return IApp.VALUE_FAILURE;
	}

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632606225558163525" name="MonitorCall" class="MaxAsyncActionNode" group="" path="Metreos.Providers.PCapService" x="837" y="257" mx="908" my="273">
      <items count="2">
        <item text="OnMonitorCall_Complete" treenode="632606225558163519" />
        <item text="OnMonitorCall_Failed" treenode="632606225558163524" />
      </items>
      <linkto id="632606518029439006" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632617966013889952" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Media Server Port1" type="variable">mediaPort1</ap>
        <ap name="Media Server Port2" type="variable">mediaPort2</ap>
        <ap name="Call Identifier" type="variable">g_monitorCallId</ap>
        <ap name="Media Server Ip Address" type="variable">g_mediaServerIP</ap>
        <ap name="UserData" type="literal">none</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnActiveCallList_Complete: Invoking MonitorCall action for callId: " + g_monitorCallId + ", MMS IP: " + g_mediaServerIP + ", MMS Port1: " + mediaPort1 + ", MMS Port2: " + mediaPort2</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnActiveCallList_Complete: MonitorCall action failed provisonally, invoking OnMonitorCall_Failed</log>
      </Properties>
    </node>
    <node type="Action" id="632606225558163545" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1213" y="271">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606518029439006" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1069" y="272">
      <linkto id="632606225558163545" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="SoundToneOnJoin" type="csharp">false</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <rd field="ConferenceId">g_conferenceId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnActiveCallList_Complete: Creating conference, populating it with connectionId: " + g_connectionId</log>
        <log condition="success" on="true" level="Verbose" type="csharp">"OnActiveCallList_Complete: Created conference with conferenceId: " + g_conferenceId
</log>
      </Properties>
    </node>
    <node type="Action" id="632606518029439256" name="ReserveConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="405" y="385">
      <linkto id="632606735182334552" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MmsId" type="variable">g_mmsID</ap>
        <rd field="ConnectionId">g_monitorConnectionId1</rd>
        <rd field="MediaRxPort">mediaPort1</rd>
        <rd field="MediaRxIP">g_mediaServerIP</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnActiveCallList_Complete: Creating a MMS connection for the first RTP stream.</log>
        <log condition="success" on="true" level="Verbose" type="csharp">"OnActiveCallList_Complete: Created connection: g_monitorConnectionId1: " + g_monitorConnectionId1 + " RxIP: " + g_mediaServerIP + " RxPort: " + mediaPort1
</log>
      </Properties>
    </node>
    <node type="Action" id="632606518029439257" name="ReserveConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="639" y="387">
      <linkto id="632606735182334557" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="MmsId" type="variable">g_mmsID</ap>
        <rd field="ConnectionId">g_monitorConnectionId2</rd>
        <rd field="MediaRxPort">mediaPort2</rd>
        <rd field="MediaRxIP">g_mediaServerIP</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnActiveCallList_Complete: Creating a MMS connection for the second RTP stream.</log>
        <log condition="success" on="true" level="Verbose" type="csharp">"OnActiveCallList_Complete: Created connection: g_monitorConnectionId2: " + g_monitorConnectionId2 + " RxIP: " + g_mediaServerIP + " RxPort: " + mediaPort2</log>
      </Properties>
    </node>
    <node type="Action" id="632606735182334550" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="404" y="271">
      <linkto id="632606518029439256" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632606735182334551" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_monitorConn1Created</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnActiveCallList_Complete: Checking if we need to create a MMS connection for the first RTP stream.</log>
      </Properties>
    </node>
    <node type="Action" id="632606735182334551" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="639" y="272">
      <linkto id="632606518029439257" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632606225558163525" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_monitorConn2Created</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnActiveCallList_Complete: Checking if we need to create a MMS connection for the second RTP stream.</log>
      </Properties>
    </node>
    <node type="Action" id="632606735182334552" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="519" y="386">
      <linkto id="632606735182334551" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_monitorConn1Created</rd>
      </Properties>
    </node>
    <node type="Action" id="632606735182334557" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="776" y="387">
      <linkto id="632606225558163525" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_monitorConn2Created</rd>
      </Properties>
    </node>
    <node type="Label" id="632606843862474807" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="110" y="132" />
    <node type="Label" id="632606843862474808" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="524" y="611">
      <linkto id="632606843862474809" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632606843862474809" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="587" y="596" mx="640" my="612">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632606843862474812" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632606843862474813" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">NoDeviceAssocAudio</ap>
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632606843862474812" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="792" y="614">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606843862474813" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="757.825867" y="747" mx="795" my="763">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632606843862474812" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632606843862474814" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="215.5459" y="366" mx="259" my="382">
      <items count="1">
        <item text="GetActiveCalls" />
      </items>
      <linkto id="632606843862474815" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">GetActiveCalls</ap>
      </Properties>
    </node>
    <node type="Action" id="632606843862474815" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="255" y="513">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632606843862475106" text="Need to do something about GetActiveCalls - must have way to differentiate b/w &#xD;&#xA;true failure and NoActiveCalls" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="352" y="192" />
    <node type="Comment" id="632608290328114533" text="In the CustomCode action, we take measures to validate the activeCalls hash and match it up against a validated list of devices in the DID group. Code is commented.&#xD;&#xA;If there are no devices associated with the current DID, we play an announcement and exit when it completes. &#xD;&#xA;If the connections for the RTP streams have not been connected, create them. Next, attempt to monitor call. &#xD;&#xA;If attempt fails, invoke GetActiveCalls again. Otherwise, add caller to conference and exit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="165" y="32" />
    <node type="Label" id="632609223498596663" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="430" y="140" />
    <node type="Label" id="632609223498596664" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="47" y="603">
      <linkto id="632609223498596665" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632609223498596665" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="113" y="588" mx="166" my="604">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632609223498596668" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632609223498596669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">NoActiveCallsAudio</ap>
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632609223498596668" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="341" y="606">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632609223498596669" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="307.825836" y="750" mx="345" my="766">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632609223498596668" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632617966013889952" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1008.29297" y="369" mx="1070" my="385">
      <items count="1">
        <item text="OnMonitorCall_Failed" treenode="632606225558163524" />
      </items>
      <linkto id="632606225558163545" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">OnMonitorCall_Failed</ap>
      </Properties>
    </node>
    <node type="Variable" id="632605881876500332" name="activeCalls" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Hashtable" initWith="Active Calls" refType="reference" name="Metreos.Providers.PCapService.ActiveCallList_Complete">activeCalls</Properties>
    </node>
    <node type="Variable" id="632606225558163536" name="mediaPort1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mediaPort1</Properties>
    </node>
    <node type="Variable" id="632606225558163538" name="mediaPort2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mediaPort2</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnActiveCallList_Failed" startnode="632605881876499784" treenode="632605881876499785" appnode="632605881876499782" handlerfor="632605881876499781">
    <node type="Start" id="632605881876499784" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="227">
      <linkto id="632605881876500337" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605881876500335" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="239.5459" y="212" mx="283" my="228">
      <items count="1">
        <item text="GetActiveCalls" />
      </items>
      <linkto id="632605881876500336" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">GetActiveCalls</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnActiveCallList_Failed: Invoking GetActiveCalls function</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876500336" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="421" y="229">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605881876500337" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="164.048828" y="227">
      <linkto id="632605881876500335" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_listFailureCounter + 1</ap>
        <rd field="ResultData">g_listFailureCounter</rd>
      </Properties>
    </node>
    <node type="Comment" id="632606735182335571" text="If the list of active calls could not be obtained, the GetActiveCalls function is invoked again.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="39" y="156" />
  </canvas>
  <canvas type="Function" name="OnMonitorCall_Complete" activetab="true" startnode="632606225558163518" treenode="632606225558163519" appnode="632606225558163516" handlerfor="632606225558163515">
    <node type="Start" id="632606225558163518" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="140">
      <linkto id="632606518029439008" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632606225558163531" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="791" y="581">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606225558163546" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="347" y="140">
      <linkto id="632609939149282108" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">phoneIp</ap>
        <rd field="ResultData">g_isCallMonitored</rd>
        <rd field="ResultData2">g_monitoredIp</rd>
      </Properties>
    </node>
    <node type="Action" id="632606518029439008" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="123" y="140">
      <linkto id="632606518029439011" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_monitorConnectionId1</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="MediaTxIP" type="variable">g_mediaServerIP</ap>
        <ap name="MediaTxPort" type="variable">port1</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnMonitorCall_Complete: g_monitorConnectionId1: " + g_monitorConnectionId1 + " IP: " + g_mediaServerIP + " Port: " + port1</log>
      </Properties>
    </node>
    <node type="Action" id="632606518029439011" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="240" y="140">
      <linkto id="632606225558163546" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_monitorConnectionId2</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="MediaTxIP" type="variable">g_mediaServerIP</ap>
        <ap name="MediaTxPort" type="variable">port2</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnMonitorCall_Complete: g_monitorConnectionId2: " + g_monitorConnectionId2 + " IP: " + g_mediaServerIP + " Port: " + port2
</log>
      </Properties>
    </node>
    <node type="Comment" id="632608290328114532" text="If the monitor operation succeeded, complete the two half-connects created in OnActiveList_Complete, and add them to the conference." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="145" y="32" />
    <node type="Action" id="632609914127331547" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="481" y="265">
      <linkto id="632609914127331548" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_DB_Name</ap>
        <ap name="Server" type="variable">g_DB_Server</ap>
        <ap name="Port" type="variable">g_DB_Port</ap>
        <ap name="Username" type="variable">g_DB_Username</ap>
        <ap name="Password" type="variable">g_DB_Password</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632609914127331548" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="482" y="367">
      <linkto id="632609939149282115" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632610820853427420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">monitor_call</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632609914127331549" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="482" y="581">
      <linkto id="632610820853427420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632610820853427423" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Command" type="variable">insertCommand</ap>
        <ap name="Name" type="variable">monitor_call</ap>
        <rd field="RowsAffected">rowsEffected</rd>
      </Properties>
    </node>
    <node type="Action" id="632609939149282108" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="441.6172" y="124" mx="483" my="140">
      <items count="1">
        <item text="GetSidFromIp" />
      </items>
      <linkto id="632609914127331547" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632610820853427420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">GetSidFromIp</ap>
        <rd field="Sid">monitoredSid</rd>
      </Properties>
    </node>
    <node type="Action" id="632609939149282115" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="481" y="472">
      <linkto id="632609914127331549" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(uint callType, string callerDN, string calleeDN, string g_originalTo, string g_fromNumber, string monitoredSid, ref string agentDN, ref string customerDN, ref string insertCommand, LogWriter log, string backupPath)
{
      if(System.IO.Directory.Exists(backupPath) == false)
      {
      	try
            {
            	System.IO.Directory.CreateDirectory(backupPath);
            }
            catch
            {
			log.Write(TraceLevel.Error, "Unable to create backup folder {0} for pending records", backupPath);
                  return "Failure";
            }
      }

	switch(callType)
	{
		case 1:		// inbound  (customer to agent)
			agentDN = calleeDN;
			customerDN = callerDN;
			break;
		
		case 2: 		// outbound	(agent to customer)
			agentDN = callerDN;
			customerDN = calleeDN;
			break;

		default:
			break;
	}

	insertCommand = "insert into monitored_calls (mc_government_agent_number, mc_did_number, mc_insurance_agent_number, mc_customer_number, mc_monitored_sid) VALUES ('" + g_fromNumber + "', " + "'" + g_originalTo + "', " + "'" + agentDN + "', " + "'" + customerDN + "', " + "'" + monitoredSid +"')";
	
	log.Write(TraceLevel.Verbose, "Insert Command = {0}", insertCommand);

	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632610820853427420" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="793" y="140">
      <linkto id="632610820853427421" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632606225558163531" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string backupPath, uint callType, string callerDN, string calleeDN, ref string agentDN, ref string customerDN, LogWriter log)
{
      if(System.IO.Directory.Exists(backupPath) == false)
      {
      	try
            {
            	System.IO.Directory.CreateDirectory(backupPath);
            }
            catch
            {
			log.Write(TraceLevel.Error, "Unable to create backup folder {0} for pending records", backupPath);
                  return "Failure";
            }
      }
	
	switch(callType)
	{
		case 1:		// inbound  (customer to agent)
			agentDN = calleeDN;
			customerDN = callerDN;
			break;
		
		case 2: 		// outbound	(agent to customer)
			agentDN = callerDN;
			customerDN = calleeDN;
			break;

		default:
			break;
	}

	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632610820853427421" name="AddRecord" class="MaxActionNode" group="" path="Metreos.Native.CallMonitor" x="894" y="377">
      <linkto id="632606225558163531" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="GovAgentNumber" type="variable">g_fromNumber</ap>
        <ap name="DID" type="variable">g_originalTo</ap>
        <ap name="InsuranceAgentNumber" type="variable">agentDN</ap>
        <ap name="CustomerNumber" type="variable">customerDN</ap>
        <ap name="MonitoredSid" type="variable">monitoredSid</ap>
        <ap name="TemporaryFilePath" type="csharp">System.IO.Path.Combine(backupPath, "pendingrecords.xml")</ap>
      </Properties>
    </node>
    <node type="Action" id="632610820853427423" name="ClearRecordCache" class="MaxActionNode" group="" path="Metreos.Native.CallMonitor" x="647" y="581">
      <linkto id="632606225558163531" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="TemporaryFilePath" type="csharp">System.IO.Path.Combine(backupPath, "pendingrecords.xml")</ap>
        <ap name="DSN" type="variable">dsn</ap>
      </Properties>
    </node>
    <node type="Variable" id="632606518029439003" name="port1" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="RTP Port1" refType="reference" name="Metreos.Providers.PCapService.MonitorCall_Complete">port1</Properties>
    </node>
    <node type="Variable" id="632606518029439007" name="port2" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="RTP Port2" refType="reference" name="Metreos.Providers.PCapService.MonitorCall_Complete">port2</Properties>
    </node>
    <node type="Variable" id="632609914127331540" name="callType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="Call Type" refType="reference" name="Metreos.Providers.PCapService.MonitorCall_Complete">callType</Properties>
    </node>
    <node type="Variable" id="632609914127331541" name="callerDN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Caller DN" refType="reference" name="Metreos.Providers.PCapService.MonitorCall_Complete">callerDN</Properties>
    </node>
    <node type="Variable" id="632609914127331542" name="calleeDN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Callee DN" refType="reference" name="Metreos.Providers.PCapService.MonitorCall_Complete">calleeDN</Properties>
    </node>
    <node type="Variable" id="632609914127331543" name="phoneIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Phone IP" refType="reference" name="Metreos.Providers.PCapService.MonitorCall_Complete">phoneIp</Properties>
    </node>
    <node type="Variable" id="632609939149282107" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632609939149282113" name="agentDN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">agentDN</Properties>
    </node>
    <node type="Variable" id="632609939149282114" name="customerDN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">customerDN</Properties>
    </node>
    <node type="Variable" id="632609939149282116" name="monitor_call" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" defaultInitWith="monitor_call" refType="value">monitor_call</Properties>
    </node>
    <node type="Variable" id="632609939149282117" name="rowsEffected" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">rowsEffected</Properties>
    </node>
    <node type="Variable" id="632609939149282118" name="insertCommand" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">insertCommand</Properties>
    </node>
    <node type="Variable" id="632609939149282119" name="monitoredSid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">monitoredSid</Properties>
    </node>
    <node type="Variable" id="632610820853427422" name="backupPath" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="c:\monitor_call\backup" refType="reference">backupPath</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMonitorCall_Failed" startnode="632606225558163523" treenode="632606225558163524" appnode="632606225558163521" handlerfor="632606225558163520">
    <node type="Start" id="632606225558163523" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="49" y="245">
      <linkto id="632617966013889953" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632606225558163530" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="582" y="248">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632608290328114521" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="396.5459" y="231" mx="440" my="247">
      <items count="1">
        <item text="GetActiveCalls" />
      </items>
      <linkto id="632606225558163530" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">GetActiveCalls</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnMonitorCall_Failed: Invoking GetActiveCalls...</log>
      </Properties>
    </node>
    <node type="Comment" id="632608290328114531" text="If monitoring of the call failed, pick another call to monitor" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="231" y="137" />
    <node type="Action" id="632617966013889953" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140" y="246">
      <linkto id="632617966013889955" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_monitorFailureCounter + 1</ap>
        <rd field="ResultData">g_monitorFailureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632617966013889955" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="255" y="247">
      <linkto id="632608290328114521" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632617966013889956" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_monitorFailureCounter &lt; g_failureThreshold</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnMonitorCall_Failed: Checking whether MonitorCall attempt limit has been reached or not</log>
        <log condition="default" on="true" level="Verbose" type="literal">OnMonitorCall_Failed: CallMonitor attempt limit reached.</log>
      </Properties>
    </node>
    <node type="Action" id="632617966013889956" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="206" y="385" mx="259" my="401">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632617966013889959" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632617966013889960" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">MonitorsFailedAudio</ap>
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnMonitorCall_Failed: playing MonitorsFailedAudio before hanging up and exiting...</log>
      </Properties>
    </node>
    <node type="Action" id="632617966013889959" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="222.825836" y="560" mx="260" my="576">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632617966013889960" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632617966013889960" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="442" y="401">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632605770032726822" treenode="632605770032726823" appnode="632605770032726820" handlerfor="632605770032726819">
    <node type="Start" id="632605770032726822" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="54" y="275">
      <linkto id="632606225558163532" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632606225558163532" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="123.825851" y="259" mx="161" my="275">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632606225558163533" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">onRemoteHangup: Calling Exit function</log>
      </Properties>
    </node>
    <node type="Action" id="632606225558163533" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="285" y="274">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632605770032726827" treenode="632605770032726828" appnode="632605770032726825" handlerfor="632605770032726824">
    <node type="Start" id="632605770032726827" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="270">
      <linkto id="632605809299934190" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605809299934190" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="103" y="270">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632605770032726832" treenode="632605770032726833" appnode="632605770032726830" handlerfor="632605770032726829">
    <node type="Start" id="632605770032726832" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="160">
      <linkto id="632605809299934191" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605809299934191" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="104" y="158">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632605770032726837" treenode="632605770032726838" appnode="632605770032726835" handlerfor="632605770032726834">
    <node type="Start" id="632605770032726837" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="81">
      <linkto id="632605809299934188" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605809299934188" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="110" y="81">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632605770032726842" treenode="632605770032726843" appnode="632605770032726840" handlerfor="632605770032726839">
    <node type="Start" id="632605770032726842" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="119">
      <linkto id="632605809299934189" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605809299934189" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="114" y="119">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestAuthentication" startnode="632605817629675034" treenode="632605817629675035" appnode="632605817629675032" handlerfor="632605770032726839">
    <node type="Start" id="632605817629675034" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="122" y="251">
      <linkto id="632605817629675041" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605817629675036" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="657" y="78" mx="710" my="94">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632605817629675071" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605817629675076" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">RequestAuthAudio</ap>
        <ap name="Prompt2" type="variable">PoundSignAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">auth_req</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"RequestAuthentication: Playing authentication request to connectionId '" + g_connectionId + "' associated with callId: " + g_callId</log>
        <log condition="default" on="true" level="Warning" type="csharp">"RequestAuthentication: Authentication request Play command FAILED. ConnectionId '" + g_connectionId + "' associated with callId: " + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675041" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="206" y="251">
      <linkto id="632605817629675044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605817629675066" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(failureCounter &lt; g_failureThreshold)</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632605817629675042" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="544">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632605817629675044" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="207" y="377">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632605817629675048" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="206" y="157">
      <linkto id="632605817629675041" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">failureCounter + 1</ap>
        <rd field="ResultData">failureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632605817629675053" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243" y="544">
      <linkto id="632605817629675042" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">failureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632605817629675060" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="633" y="343" mx="707" my="359">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632605817629675007" />
        <item text="OnGatherDigits_Failed" treenode="632605817629675012" />
      </items>
      <linkto id="632605817629675077" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605817629675075" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">auth_req</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"RequestAuthentication: GatherDigits to obtain authentication code on connectionId '" + g_connectionId + "' associated with callId: " + g_callId</log>
        <log condition="default" on="true" level="Warning" type="csharp">"RequestAuthentication: GatherDigits to obtain authentication code FAILED on connectionId '" + g_connectionId + "' associated with callId: " + g_callId
</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675066" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="327" y="250">
      <linkto id="632605817629675067" type="Labeled" style="Bezier" ortho="true" label="play" />
      <linkto id="632605817629675068" type="Labeled" style="Bezier" ortho="true" label="digits" />
      <linkto id="632605881876499385" type="Labeled" style="Bezier" ortho="true" label="auth_fail" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Label" id="632605817629675067" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="450" y="124" />
    <node type="Label" id="632605817629675068" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="448" y="250" />
    <node type="Label" id="632605817629675069" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="597" y="94">
      <linkto id="632605817629675036" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632605817629675070" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="206" y="91">
      <linkto id="632605817629675048" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632605817629675071" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="707.4707" y="229" />
    <node type="Label" id="632605817629675072" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="595.4707" y="359">
      <linkto id="632605817629675060" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632605817629675074" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="152.4707" y="544">
      <linkto id="632605817629675053" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632605817629675075" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="703.4707" y="506" />
    <node type="Label" id="632605817629675076" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="828.4707" y="93" />
    <node type="Label" id="632605817629675077" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="837.4707" y="358" />
    <node type="Label" id="632605881876499385" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="451" y="392" />
    <node type="Label" id="632605881876499386" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="590" y="661">
      <linkto id="632605881876499387" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605881876499387" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="667" y="661">
      <linkto id="632605881876499388" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_failedLogins + 1</ap>
        <rd field="ResultData">g_failedLogins</rd>
      </Properties>
    </node>
    <node type="Action" id="632605881876499388" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="777" y="661">
      <linkto id="632605881876499391" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632605881876499394" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_failedLogins &lt; g_maxFailedLogins</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632605881876499391" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="904" y="645.5" mx="957" my="662">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632605881876499397" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605881876499398" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt3" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">AuthFailedAudio</ap>
        <ap name="Prompt2" type="variable">RequestAuthAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">auth_req</ap>
      </Properties>
    </node>
    <node type="Action" id="632605881876499394" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="728" y="819.5" mx="781" my="836">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632605881876499399" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632605881876499401" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">AuthFailedAudio</ap>
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632605881876499397" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1067.41858" y="660" />
    <node type="Label" id="632605881876499398" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="955.4186" y="786" />
    <node type="Label" id="632605881876499399" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="882.4186" y="835" />
    <node type="Action" id="632605881876499401" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="745.244446" y="952" mx="782" my="968">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632605881876499402" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632605881876499402" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="885.4186" y="968">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Comment" id="632608290328114530" text="We check to see if the number of login failures exceeds the defined login threshold. If so, we play an announcement and exit. &#xD;&#xA;Based on the passed in Action parameter, we decide whether we are playing the prompt for the login, or gathering the digits for the login" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="236" y="32" />
    <node type="Variable" id="632605817629675064" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Action" refType="reference">action</Properties>
    </node>
    <node type="Variable" id="632605817629675065" name="failureCounter" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" defaultInitWith="0" refType="reference">failureCounter</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="GetActiveCalls" startnode="632605881876499424" treenode="632605881876499425" appnode="632605881876499422" handlerfor="632605770032726839">
    <node type="Start" id="632605881876499424" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="58" y="240">
      <linkto id="632605881876499977" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605881876499786" name="ActiveCallList" class="MaxAsyncActionNode" group="" path="Metreos.Providers.PCapService" x="266" y="224" mx="343" my="240">
      <items count="2">
        <item text="OnActiveCallList_Complete" treenode="632605881876499780" />
        <item text="OnActiveCallList_Failed" treenode="632605881876499785" />
      </items>
      <linkto id="632605881876500329" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605881876500330" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632605881876499977" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="156" y="239">
      <linkto id="632605881876499786" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632608290328114522" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_listFailureCounter &lt; g_failureThreshold</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632605881876500327" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="156" y="148">
      <linkto id="632605881876499977" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_listFailureCounter + 1</ap>
        <rd field="ResultData">g_listFailureCounter</rd>
      </Properties>
    </node>
    <node type="Label" id="632605881876500328" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="156" y="77">
      <linkto id="632605881876500327" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632605881876500329" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="341" y="374" />
    <node type="Action" id="632605881876500330" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="239">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Comment" id="632605881876500331" text="Need default branch here&#xD;&#xA;that returns failure and more" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="580" y="502" />
    <node type="Action" id="632608290328114522" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="107" y="402" mx="160" my="418">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632608290328114526" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632608290328114525" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">NoActiveCallsAudio</ap>
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">GetActiveCalls: playing 'No Active Calls' announcement.</log>
      </Properties>
    </node>
    <node type="Action" id="632608290328114525" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="290" y="417">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632608290328114526" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="123.825851" y="562" mx="161" my="578">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632608290328114525" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Comment" id="632608290328114527" text="We check to see if the number of failed list/monitor attempts is is equal to the threshold.&#xD;&#xA;If it is,we play the 'no active calls to monitor' prompt, pass in exit userdata so that script exits&#xD;&#xA;when announcement is complete.&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="373" y="64" />
  </canvas>
  <canvas type="Function" name="Exit" startnode="632605817629675023" treenode="632605817629675024" appnode="632605817629675021" handlerfor="632605770032726839">
    <node type="Start" id="632605817629675023" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="205">
      <linkto id="632605817629675059" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632605817629675051" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="227.3529" y="318">
      <linkto id="632606225558163511" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632605817629675059" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="150" y="206">
      <linkto id="632605817629675051" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632606225558163511" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isCallerAnswered</ap>
      </Properties>
    </node>
    <node type="Action" id="632606225558163511" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="304" y="207">
      <linkto id="632606225558163512" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632606735182335572" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isCallMonitored</ap>
      </Properties>
    </node>
    <node type="Action" id="632606225558163512" name="StopMonitorCall" class="MaxActionNode" group="" path="Metreos.Providers.PCapService" x="389" y="321">
      <linkto id="632606735182335572" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Call Identifier" type="variable">g_monitorCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632606225558163534" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="823.82605" y="208">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606225558163547" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="559" y="319">
      <linkto id="632606735182335573" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_monitorConnectionId1</ap>
      </Properties>
    </node>
    <node type="Action" id="632606225558163548" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="733" y="323">
      <linkto id="632606225558163534" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_monitorConnectionId2</ap>
      </Properties>
    </node>
    <node type="Action" id="632606735182335572" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="474" y="207">
      <linkto id="632606225558163547" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632606735182335573" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_monitorConn1Created</ap>
      </Properties>
    </node>
    <node type="Action" id="632606735182335573" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="646" y="208">
      <linkto id="632606225558163548" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632606225558163534" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_monitorConn2Created</ap>
      </Properties>
    </node>
    <node type="Comment" id="632608290328114528" text="Based on booleans, we check to see if the caller was answered, if the call was monitored, if any additional connections were &#xD;&#xA;created, and if so, we take appropriate action." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="256" y="45" />
  </canvas>
  <canvas type="Function" name="GetSidFromIp" startnode="632609939149282111" treenode="632609939149282112" appnode="632609939149282109" handlerfor="632605770032726839">
    <node type="Start" id="632609939149282111" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="54" y="196">
      <linkto id="632609939149282120" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632609939149282120" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="174" y="195">
      <linkto id="632609939149282121" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="literal">ciscodevicelistx</ap>
        <ap name="Server" type="variable">g_DB_Server</ap>
        <ap name="Port" type="variable">g_DB_Port</ap>
        <ap name="Username" type="variable">g_DB_Username</ap>
        <ap name="Password" type="variable">g_DB_Password</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632609939149282121" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="327" y="195">
      <linkto id="632609939149282131" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632610820853427419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">devicelistx</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632609939149282122" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="623" y="197">
      <linkto id="632609939149282132" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632610820853427419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="variable">lookupQuery</ap>
        <ap name="Name" type="variable">devicelistx</ap>
        <rd field="ResultSet">deviceLookupResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632609939149282123" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="622" y="435">
      <Properties final="true" type="appControl" log="On">
        <ap name="Sid" type="variable">deviceName</ap>
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632609939149282131" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="474" y="197">
      <linkto id="632609939149282122" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string g_monitoredIp, ref string lookupQuery)
{
	// Compose query string
	lookupQuery = "select name from device_info where ip = '" + g_monitoredIp + "'";
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632609939149282132" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="623" y="295">
      <linkto id="632609939149282123" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable deviceLookupResult, ref string deviceName)
{
	deviceName = "";

	if (deviceLookupResult.Rows.Count == 0)
		return "Failure";	

	if (deviceLookupResult.Rows[0]["name"] != null)
	{
		deviceName = (string)deviceLookupResult.Rows[0]["name"];
		return "Success";
	}

	return "Failure";
}
</Properties>
    </node>
    <node type="Action" id="632610820853427419" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="623" y="51">
      <Properties final="true" type="appControl" log="On">
        <ap name="Sid" type="literal">Unknown</ap>
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Variable" id="632609939149282124" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632609939149282127" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632609939149282128" name="devicelistx" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="devicelistx" refType="reference">devicelistx</Properties>
    </node>
    <node type="Variable" id="632609939149282129" name="deviceLookupResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">deviceLookupResult</Properties>
    </node>
    <node type="Variable" id="632609939149282130" name="lookupQuery" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">lookupQuery</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>