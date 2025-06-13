<Application name="Agent" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Agent">
    <outline>
      <treenode type="evh" id="632520256836409879" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632520256836409876" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632520256836409875" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632520504464737708" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632520504464737705" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632520504464737704" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632767396136244624" actid="632520504464737719" />
          <ref id="632767396136244631" actid="632521226229070555" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632520504464737713" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632520504464737710" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632520504464737709" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632767396136244625" actid="632520504464737719" />
          <ref id="632767396136244632" actid="632521226229070555" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632520504464737718" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632520504464737715" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632520504464737714" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632767396136244626" actid="632520504464737719" />
          <ref id="632767396136244633" actid="632521226229070555" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527325857790928" level="2" text="Metreos.Events.RemoteAgent.RequestConference: OnRequestConference">
        <node type="function" name="OnRequestConference" id="632527325857790925" path="Metreos.StockTools" />
        <calls>
          <ref actid="632537870868175575" />
        </calls>
        <node type="event" name="RequestConference" id="632527325857790924" path="Metreos.Events.RemoteAgent.RequestConference" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560196784" level="2" text="Metreos.Events.RemoteAgent.RecordingEvent: OnRecordingEvent">
        <node type="function" name="OnRecordingEvent" id="632527335560196781" path="Metreos.StockTools" />
        <node type="event" name="RecordingEvent" id="632527335560196780" path="Metreos.Events.RemoteAgent.RecordingEvent" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632537870868175546" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632537870868175543" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632537870868175542" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632767396136244762" actid="632537870868175552" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632537870868175551" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632537870868175548" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632537870868175547" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632767396136244763" actid="632537870868175552" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555905720469712" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632555905720469709" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632555905720469708" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555905720469717" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632555905720469714" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632555905720469713" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632585988688885982" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632585988688885979" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632585988688885978" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632585988688885989" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632585988688885986" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632585988688885985" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="db_Name" id="632767396136244560" vid="632521226229070625">
        <Properties type="String" initWith="DbName">db_Name</Properties>
      </treenode>
      <treenode text="db_Server" id="632767396136244562" vid="632521226229070627">
        <Properties type="String" initWith="DbServer">db_Server</Properties>
      </treenode>
      <treenode text="db_Port" id="632767396136244564" vid="632521226229070629">
        <Properties type="UInt" initWith="DbPort">db_Port</Properties>
      </treenode>
      <treenode text="db_Username" id="632767396136244566" vid="632521226229070631">
        <Properties type="String" initWith="DbUsername">db_Username</Properties>
      </treenode>
      <treenode text="db_Password" id="632767396136244568" vid="632521226229070633">
        <Properties type="String" initWith="DbPassword">db_Password</Properties>
      </treenode>
      <treenode text="db_ConnectionName" id="632767396136244570" vid="632521226229070863">
        <Properties type="String" initWith="DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632767396136244572" vid="632525439932447150">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_userId" id="632767396136244574" vid="632520350806070164">
        <Properties type="UInt">g_userId</Properties>
      </treenode>
      <treenode text="g_remoteAgentId" id="632767396136244576" vid="632520504464737698">
        <Properties type="UInt">g_remoteAgentId</Properties>
      </treenode>
      <treenode text="g_userLevel" id="632767396136244578" vid="632520504464737700">
        <Properties type="UInt">g_userLevel</Properties>
      </treenode>
      <treenode text="g_externalNumber" id="632767396136244580" vid="632520504464737696">
        <Properties type="String">g_externalNumber</Properties>
      </treenode>
      <treenode text="g_outgoingCallId" id="632767396136244582" vid="632520504464737702">
        <Properties type="String">g_outgoingCallId</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632767396136244584" vid="632520504464737725">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632767396136244586" vid="632521283639875552">
        <Properties type="String" defaultInitWith="0">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="632767396136244588" vid="632521845222003111">
        <Properties type="UInt">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_agentDN" id="632767396136244590" vid="632525439932447148">
        <Properties type="String" defaultInitWith="NONE">g_agentDN</Properties>
      </treenode>
      <treenode text="g_agentRecordId" id="632767396136244592" vid="632525439932447152">
        <Properties type="UInt">g_agentRecordId</Properties>
      </treenode>
      <treenode text="g_incomingConnectionId" id="632767396136244594" vid="632534030134646090">
        <Properties type="String">g_incomingConnectionId</Properties>
      </treenode>
      <treenode text="g_outgoingConnectionId" id="632767396136244596" vid="632534030134646092">
        <Properties type="String">g_outgoingConnectionId</Properties>
      </treenode>
      <treenode text="g_recordingConnectionId" id="632767396136244598" vid="632538254415137638">
        <Properties type="String" defaultInitWith="0">g_recordingConnectionId</Properties>
      </treenode>
      <treenode text="g_recording" id="632767396136244600" vid="632537870868175536">
        <Properties type="Bool" defaultInitWith="false">g_recording</Properties>
      </treenode>
      <treenode text="g_recordingId" id="632767396136244602" vid="632537870868175878">
        <Properties type="UInt" defaultInitWith="0">g_recordingId</Properties>
      </treenode>
      <treenode text="g_isPeerToPeer" id="632767396136244604" vid="632520992361736930">
        <Properties type="Bool" defaultInitWith="false">g_isPeerToPeer</Properties>
      </treenode>
      <treenode text="g_callerConnected" id="632767396136244606" vid="632536728080857258">
        <Properties type="Bool" defaultInitWith="false">g_callerConnected</Properties>
      </treenode>
      <treenode text="g_calleeConnected" id="632767396136244608" vid="632536728080857260">
        <Properties type="Bool" defaultInitWith="false">g_calleeConnected</Properties>
      </treenode>
      <treenode text="g_calleeDialed" id="632767396136244610" vid="632536728080857262">
        <Properties type="Bool" defaultInitWith="false">g_calleeDialed</Properties>
      </treenode>
      <treenode text="g_mediaServerIP" id="632767396136244612" vid="632538282337813080">
        <Properties type="String">g_mediaServerIP</Properties>
      </treenode>
      <treenode text="g_exit" id="632767396136244614" vid="632538282337813090">
        <Properties type="Bool" defaultInitWith="false">g_exit</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632767396136244616" vid="632767263316804037">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632520256836409878" treenode="632520256836409879" appnode="632520256836409876" handlerfor="632520256836409875">
    <node type="Start" id="632520256836409878" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="275">
      <linkto id="632521226229070784" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520256836409880" name="AcceptCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="1102" y="396">
      <linkto id="632520504464737719" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632537870868175497" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <log condition="default" on="true" level="Warning" type="csharp">"OnIncomingCall: could not 'Accept' call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632520350806070157" name="GetDeviceByDn" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="673" y="277">
      <linkto id="632520504464737695" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632585988688885962" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DirectoryNumber" type="variable">to</ap>
        <rd field="DeviceName">deviceName</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: GetDeviceByDn for: " + to</log>
        <log condition="default" on="true" level="Warning" type="csharp">"OnIncomingCall: could not obtain device for destination number: " + to
</log>
      </Properties>
    </node>
    <node type="Comment" id="632520350806070158" text="GetDeviceByDn will&#xD;&#xA;fail if there is more&#xD;&#xA;than once device that&#xD;&#xA;uses the given DN" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="525" y="32" />
    <node type="Comment" id="632520350806070162" text="GetUserByDeviceMac&#xD;&#xA;returns the first row it&#xD;&#xA;retrieves from the DB,&#xD;&#xA;so if more than one user&#xD;&#xA;is associated with a device,&#xD;&#xA;we might be getting the &#xD;&#xA;wrong user" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="782" y="32" />
    <node type="Action" id="632520504464737695" name="GetRemoteAgentByDevice" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="821" y="276">
      <linkto id="632521845222003110" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632585988688885962" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="variable">deviceName</ap>
        <rd field="RemoteAgentId">g_remoteAgentId</rd>
        <rd field="UserId">g_userId</rd>
        <rd field="UserLevel">g_userLevel</rd>
        <rd field="ExternalNumber">g_externalNumber</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: GetRemoteAgentByDevice for device: " + deviceName</log>
      </Properties>
    </node>
    <node type="Action" id="632520504464737719" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="1041" y="518" mx="1107" my="534">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632520504464737708" />
        <item text="OnMakeCall_Failed" treenode="632520504464737713" />
        <item text="OnRemoteHangup" treenode="632520504464737718" />
      </items>
      <linkto id="632521845222003117" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632536728080857265" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_externalNumber</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="DisplayName" type="variable">from</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outgoingCallId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: MakeCall to: " + g_externalNumber
</log>
      </Properties>
    </node>
    <node type="Action" id="632520504464737723" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="393" y="275">
      <linkto id="632525439932447155" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632520350806070157" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
        <log condition="default" on="true" level="Error" type="literal">OnIncomingCall: destination (To) number could not be obtained from IncomingCall event.</log>
public static string Execute(ref string from, ref string g_routingGuid, string routingGuid, ref string g_agentDN, string to)
{
	if (from == null || from == string.Empty)
		from = "UNAVAILABLE";

	if (to == null || to == string.Empty)
	{
		g_agentDN = "UNAVAILABLE";
		return IApp.VALUE_FAILURE;
	}

	g_agentDN = to;
	g_routingGuid = routingGuid;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632520504464737727" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1318" y="533">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521226229070554" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1101" y="275">
      <linkto id="632521226229070555" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632520256836409880" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isPeerToPeer</ap>
      </Properties>
    </node>
    <node type="Action" id="632521226229070555" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="1291" y="258" mx="1357" my="274">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632520504464737708" />
        <item text="OnMakeCall_Failed" treenode="632520504464737713" />
        <item text="OnRemoteHangup" treenode="632520504464737718" />
      </items>
      <linkto id="632521845222003116" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632536728080857264" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">g_externalNumber</ap>
        <ap name="From" type="variable">from</ap>
        <ap name="DisplayName" type="variable">from</ap>
        <ap name="PeerCallId" type="variable">callId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outgoingCallId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: P2P MakeCall to: " + g_externalNumber</log>
      </Properties>
    </node>
    <node type="Action" id="632521226229070559" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1566.88281" y="272">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521226229070784" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="126.999939" y="276">
      <linkto id="632521226229070785" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720469726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Name</ap>
        <ap name="Server" type="variable">db_Server</ap>
        <ap name="Port" type="variable">db_Port</ap>
        <ap name="Username" type="variable">db_Username</ap>
        <ap name="Password" type="variable">db_Password</ap>
        <ap name="Pooling" type="literal">true</ap>
        <rd field="DSN">DSN</rd>
        <log condition="default" on="true" level="Warning" type="literal">OnIncomingCall: could not format DSN.</log>
      </Properties>
    </node>
    <node type="Action" id="632521226229070785" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="244" y="275">
      <linkto id="632520504464737723" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720469726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">DSN</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="default" on="true" level="Warning" type="literal">OnIncomingCall: failed to open application suite database connection</log>
      </Properties>
    </node>
    <node type="Action" id="632521845222003110" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="986" y="276">
      <linkto id="632521226229070554" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">g_externalNumber</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
        <log condition="entry" on="false" level="Verbose" type="literal">OnIncomingCall: WriteCallRecordStart</log>
      </Properties>
    </node>
    <node type="Action" id="632521845222003113" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="630.3528" y="749">
      <linkto id="632521845222003114" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: hanging up call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632521845222003114" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="746.3535" y="749">
      <linkto id="632521845222003115" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632521845222003115" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="865.3535" y="749">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632521845222003116" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1355.35352" y="424" />
    <node type="Label" id="632521845222003117" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1104.35352" y="680" />
    <node type="Label" id="632521845222003118" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="525.349243" y="749">
      <linkto id="632521845222003113" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521845222003119" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="394" y="479">
      <linkto id="632521845222003120" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">g_agentDN</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632521845222003120" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="393" y="576">
      <linkto id="632521845222003121" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632521845222003121" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="393" y="674">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632525439932447155" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="394" y="377">
      <linkto id="632521845222003119" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Warning" type="csharp">"OnIncomingCall: rejecting call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632536728080857264" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1466.35352" y="273">
      <linkto id="632521226229070559" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">callId</ap>
        <rd field="ResultData">g_calleeDialed</rd>
        <rd field="ResultData2">g_incomingCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632536728080857265" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1211.35352" y="533">
      <linkto id="632520504464737727" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_calleeDialed</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175497" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1005" y="396" />
    <node type="Label" id="632537870868175498" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="269" y="478">
      <linkto id="632521845222003119" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720469726" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="246" y="377">
      <linkto id="632525439932447155" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string from, ref string g_agentDN, string to)
{
	if (from == null || from == string.Empty)
		from = "UNAVAILABLE";

	if (to == null || to == string.Empty)
	{
		g_agentDN = "UNAVAILABLE";
		return IApp.VALUE_FAILURE;
	}

	g_agentDN = to;
	return IApp.VALUE_SUCCESS;
}

</Properties>
    </node>
    <node type="Comment" id="632585988688885670" text="Open connection to application suite&#xD;&#xA;database" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="48" y="200" />
    <node type="Comment" id="632585988688885960" text="Write a call record&#xD;&#xA;for the failed call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="208" y="554" />
    <node type="Comment" id="632585988688885961" text="Check for callerId issues" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="324" y="209" />
    <node type="Action" id="632585988688885962" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="671" y="478">
      <linkto id="632521845222003119" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnIncomingCall: rejecting call with callId: " + callId

</log>
      </Properties>
    </node>
    <node type="Comment" id="632585988688885963" text="Obtain the device name from the directory number, and&#xD;&#xA;then obtain RemoteAgent user information based on the&#xD;&#xA;device name, then write start of call record" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="707" y="185" />
    <node type="Comment" id="632585988688885964" text="If the call is P2P, just make the P2P call to&#xD;&#xA;the external number associated with the &#xD;&#xA;remote agent user" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1191.35352" y="164" />
    <node type="Comment" id="632585988688885965" text="If the call is to be connected in bridged mode,&#xD;&#xA;accept the call, then make the call to the remote&#xD;&#xA;agent's external number" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="833.3535" y="451" />
    <node type="Comment" id="632585988688885966" text="Does this makecall need a reject here?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1392.3" y="391" />
    <node type="Variable" id="632520256836409881" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632520350806070159" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632520350806070160" name="deviceName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceName</Properties>
    </node>
    <node type="Variable" id="632520504464737724" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632521226229070783" name="DSN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">DSN</Properties>
    </node>
    <node type="Variable" id="632525439932447154" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632520504464737707" treenode="632520504464737708" appnode="632520504464737705" handlerfor="632520504464737704">
    <node type="Start" id="632520504464737707" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="192">
      <linkto id="632521226229071023" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520504464737730" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="138" y="343">
      <linkto id="632520504464737731" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632585988688885967" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="default" on="true" level="Warning" type="literal">OnMakeCall_Complete: Conference could not be created. </log>
public static string Execute(string conferenceId, ref string g_conferenceId, LogWriter log)
{
	if (conferenceId == null || conferenceId == string.Empty || conferenceId == "0")
	{
		log.Write(TraceLevel.Error, "Failed to create a conference.");
		return IApp.VALUE_FAILURE;
	}

	g_conferenceId = conferenceId;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632520504464737731" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="365" y="342">
      <linkto id="632521845222003123" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632525439932447146" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="MmsId" type="variable">mmsId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="MediaRxIP">g_mediaServerIP</rd>
        <log condition="default" on="true" level="Warning" type="csharp">"OnMakeCall_Complete: failed to answer incoming call with callId: " + g_incomingCallId</log>
      </Properties>
    </node>
    <node type="Action" id="632520504464737732" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="684" y="191">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521226229071023" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="137" y="191">
      <linkto id="632520504464737730" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632525439932447146" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isPeerToPeer</ap>
      </Properties>
    </node>
    <node type="Action" id="632521845222003123" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="365" y="443">
      <linkto id="632521845222003124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnMakeCall_Complete: hanging up outbound connected call with callId: " + g_outgoingCallId</log>
      </Properties>
    </node>
    <node type="Action" id="632521845222003124" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="365.36" y="542">
      <linkto id="632521845222003125" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632521845222003125" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="364.359924" y="648">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632525439932447146" name="WriteAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="365" y="190">
      <linkto id="632534030134646075" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632550392566728982" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentDN" type="variable">g_agentDN</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="IsRecorded" type="literal">false</ap>
        <rd field="AgentRecordId">g_agentRecordId</rd>
        <log condition="default" on="true" level="Verbose" type="csharp">"Could not write agent record for DN: " + g_agentDN</log>
      </Properties>
    </node>
    <node type="Action" id="632534030134646075" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="522" y="190">
      <linkto id="632520504464737732" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">true</ap>
        <rd field="ResultData">g_callerConnected</rd>
        <rd field="ResultData2">g_calleeConnected</rd>
      </Properties>
    </node>
    <node type="Action" id="632550392566728982" name="RetrieveAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="596" y="341">
      <linkto id="632550392566728983" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632585988688885970" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentDN" type="variable">g_agentDN</ap>
        <rd field="AgentRecordId">g_agentRecordId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnMakeCall_Complete: attempting to retrieve agent record for DN: " + g_agentDN</log>
        <log condition="default" on="true" level="Verbose" type="csharp">"OnMakeCall_Complete: Could not retrieve agent record for DN: " + g_agentDN
</log>
      </Properties>
    </node>
    <node type="Action" id="632550392566728983" name="RemoveAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="745" y="342">
      <linkto id="632550392566728984" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632585988688885970" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentRecordId" type="variable">g_agentRecordId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnMakeCall_Complete: Attempting to remove record with ID: " + g_agentRecordId</log>
        <log condition="default" on="true" level="Verbose" type="csharp">"OnMakeCall_Complete: Could not remove record with ID: " + g_agentRecordId
</log>
      </Properties>
    </node>
    <node type="Action" id="632550392566728984" name="WriteAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="888" y="343">
      <linkto id="632550392566728985" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632585988688885970" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentDN" type="variable">g_agentDN</ap>
        <ap name="RoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="IsRecorded" type="literal">false</ap>
        <rd field="AgentRecordId">g_agentRecordId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnMakeCall_Complete: attempting to write new agent record for DN: " + g_agentDN</log>
        <log condition="default" on="true" level="Verbose" type="csharp">"OnMakeCall_Complete: could not write new agent record for DN: " + g_agentDN
</log>
      </Properties>
    </node>
    <node type="Label" id="632550392566728985" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1010.50525" y="343" />
    <node type="Label" id="632550392566728986" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="521.505249" y="110">
      <linkto id="632534030134646075" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632585988688885667" text="If the call was P2P, move right on to&#xD;&#xA;writing the call record for the agent, &#xD;&#xA;otherwise, execute CustomCode to check&#xD;&#xA;that conference was created correctly&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="81" />
    <node type="Comment" id="632585988688885669" text="If the conference was not created&#xD;&#xA;properly, hang up both parties, complete&#xD;&#xA;call record, and exit. Otherwise, answer&#xD;&#xA;incoming call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="484" />
    <node type="Action" id="632585988688885967" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="138" y="442">
      <linkto id="632521845222003123" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Complete: Hanging up incoming call with callId: " + g_incomingCallId
</log>
      </Properties>
    </node>
    <node type="Comment" id="632585988688885968" text="Write call state record" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="309" y="131" />
    <node type="Comment" id="632585988688885969" text="If the WriteAgentRecord action fails,&#xD;&#xA;assume that it is because a record already&#xD;&#xA;exists.The record should be a singleton, &#xD;&#xA;so we assume that it just wasn't removed properly.&#xD;&#xA;We retrieve the current record, remove it, and then&#xD;&#xA;attempt to write it again. " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="765" y="466" />
    <node type="Action" id="632585988688885970" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="743" y="444">
      <linkto id="632585988688885971" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632585988688885974" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isPeerToPeer</ap>
      </Properties>
    </node>
    <node type="Action" id="632585988688885971" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="608" y="541">
      <linkto id="632521845222003124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnMakeCall_Complete: Hanging up incoming call with callId: " + g_incomingCallId
</log>
      </Properties>
    </node>
    <node type="Label" id="632585988688885973" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="46" y="443">
      <linkto id="632585988688885967" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632585988688885974" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="599" y="444" />
    <node type="Comment" id="632585988688885975" text="We should consider moving some of this code back to OnIncomingCall, that way we can&#xD;&#xA;let the call ring out properly, as expected&#xD;&#xA;by IPCC, vs hanging it up here" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="573.761353" y="256" />
    <node type="Variable" id="632520504464737728" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" defaultInitWith="0" refType="reference" name="Metreos.CallControl.MakeCall_Complete">conferenceId</Properties>
    </node>
    <node type="Variable" id="632767263316804039" name="mmsId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="MmsId" defaultInitWith="0" refType="reference" name="Metreos.CallControl.MakeCall_Complete">mmsId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632520504464737712" treenode="632520504464737713" appnode="632520504464737710" handlerfor="632520504464737709">
    <node type="Start" id="632520504464737712" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="289">
      <linkto id="632521845222003134" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521226229071024" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="241" y="289">
      <linkto id="632521226229071025" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632521845222003130" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isPeerToPeer</ap>
      </Properties>
    </node>
    <node type="Action" id="632521226229071025" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="242" y="475">
      <linkto id="632521845222003130" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnMakeCall_Failed: hanging up connected incoming call with callId: " + g_incomingCallId
</log>
      </Properties>
    </node>
    <node type="Action" id="632521226229071026" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="499" y="474">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632521845222003130" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="371.36" y="475">
      <linkto id="632521226229071026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="variable">endReason</ap>
      </Properties>
    </node>
    <node type="Action" id="632521845222003134" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="133" y="289">
      <linkto id="632521226229071024" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="exit" on="true" level="Info" type="csharp">"OnMakeCall_Failed: Outbound call failed. Provided EndReason is: " + endReason</log>
public static string Execute(ref string endReason)
{
	// Metreos.Interfaces.ICallControl should provide string constants that
	// can be used here.
	switch (endReason)
	{
		case "Busy" : 
		case "Normal" : 
		case "Ringout" :
		case "Unreachable" :
		case "InternalError" : break;
		default : endReason = "Unknown"; break;
	}

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632585988688885976" text="Translate provided end reason into&#xD;&#xA;an Application Suite end reason" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="44" y="209" />
    <node type="Comment" id="632585988688885977" text="If the call was P2P, there's nothing to hang up. Otherwise, we hang up the caller.&#xD;&#xA;Then, we write call record stop with the translated end reason, and we exit the script." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="328" y="314" />
    <node type="Variable" id="632521845222003132" name="endReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" defaultInitWith="Unknown" refType="reference" name="Metreos.CallControl.MakeCall_Failed">endReason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632520504464737717" treenode="632520504464737718" appnode="632520504464737715" handlerfor="632520504464737714">
    <node type="Start" id="632520504464737717" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="51" y="214">
      <linkto id="632536728080857257" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632521226229071027" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="986.9999" y="402">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632536728080857256" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="155" y="338">
      <linkto id="632537870868175504" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632537870868175503" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632536728080857257" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="154" y="215">
      <linkto id="632536728080857256" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632537870868175509" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isPeerToPeer</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175499" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="422" y="401">
      <linkto id="632537870868175500" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632537870868175508" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_callerConnected &amp;&amp; g_calleeConnected</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175500" name="RemoveAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="559" y="402">
      <linkto id="632537870868175505" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentRecordId" type="variable">g_agentRecordId</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175502" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="284" y="459">
      <linkto id="632537870868175499" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175503" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="285" y="337">
      <linkto id="632537870868175499" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175504" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="156" y="460">
      <linkto id="632537870868175502" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632537870868175506" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_calleeConnected || g_calleeDialed</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175505" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="734" y="402">
      <linkto id="632538282337813088" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">Normal</ap>
      </Properties>
    </node>
    <node type="Label" id="632537870868175506" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="155" y="557" />
    <node type="Label" id="632537870868175507" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="734" y="498">
      <linkto id="632537870868175505" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632537870868175508" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="422" y="497" />
    <node type="Label" id="632537870868175509" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="290" y="214" />
    <node type="Label" id="632537870868175510" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="421" y="301">
      <linkto id="632537870868175499" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632537870868175563" text="need code to stop recording, write recording record, etc" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="595" y="309" />
    <node type="Action" id="632538282337813088" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="858.9419" y="401">
      <linkto id="632521226229071027" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632538282337813089" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Action" id="632538282337813089" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="859.9419" y="507">
      <linkto id="632538282337813092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_exit</rd>
      </Properties>
    </node>
    <node type="Action" id="632538282337813092" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="986.9419" y="508">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632555905720469724" text="Potential handler sequencing issue w/ remote hangup" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="813" y="556" />
    <node type="Variable" id="632536728080857255" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRequestConference" startnode="632527325857790927" treenode="632527325857790928" appnode="632527325857790925" handlerfor="632527325857790924">
    <node type="Start" id="632527325857790927" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="65" y="312">
      <linkto id="632538282337813099" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527325857790930" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="980" y="499">
      <linkto id="632527325857790932" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Success" type="variable">success</ap>
        <ap name="ConferenceId" type="csharp">(g_conferenceId == null) ? string.Empty : g_conferenceId</ap>
        <ap name="ResponseMessage" type="variable">responseMessage</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="EventName" type="literal">Metreos.Events.RemoteAgent.RequestConferenceResponse</ap>
        <ap name="ToGuid" type="variable">requestingGuid</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnRequestConference: Sending response to script with routingGuid: " + requestingGuid</log>
      </Properties>
    </node>
    <node type="Action" id="632527325857790932" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1093" y="499">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632534030134646076" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="293" y="311">
      <linkto id="632534030134646079" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632537870868175518" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_callerConnected &amp;&amp; g_calleeConnected
</ap>
      </Properties>
    </node>
    <node type="Action" id="632534030134646077" name="BridgeCalls" class="MaxActionNode" group="" path="Metreos.CallControl" x="544" y="309">
      <linkto id="632537870868175513" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632537870868175558" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_outgoingConnectionId</rd>
        <rd field="PeerConnectionId">g_incomingConnectionId</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
        <rd field="MediaRxIP">g_mediaServerIP</rd>
      </Properties>
    </node>
    <node type="Action" id="632534030134646079" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="428" y="309">
      <linkto id="632534030134646077" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632537870868175520" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isPeerToPeer</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175511" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="745" y="308">
      <linkto id="632537870868175512" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632537870868175513" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_conferenceId == null || g_conferenceId == string.Empty || g_conferenceId == "0"</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnRequestConference: the value of g_conferenceId is: " + g_conferenceId</log>
      </Properties>
    </node>
    <node type="Action" id="632537870868175512" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="867" y="308">
      <linkto id="632537870868175885" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">Success</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632537870868175513" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="746" y="500">
      <linkto id="632537870868175885" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">NOCONFERENCE</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnRequestConference: BridgeCalls returned default.</log>
      </Properties>
    </node>
    <node type="Comment" id="632537870868175516" text="If conference is not created, but the individual connections are, what needs to happen?&#xD;&#xA;Also, need to specify ability of conference to coach" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="197" y="101" />
    <node type="Action" id="632537870868175518" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="294" y="412">
      <linkto id="632537870868175523" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">Failure</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175520" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="427" y="413" />
    <node type="Label" id="632537870868175521" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="745" y="208">
      <linkto id="632537870868175511" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632537870868175522" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="745" y="603">
      <linkto id="632537870868175513" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632537870868175523" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="295" y="543" />
    <node type="Action" id="632537870868175558" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="658" y="308">
      <linkto id="632537870868175511" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_isPeerToPeer</rd>
        <log condition="entry" on="true" level="Verbose" type="literal">OnRequestConference: BridgeCalls succeeded</log>
      </Properties>
    </node>
    <node type="Action" id="632537870868175885" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="868" y="500">
      <linkto id="632527325857790930" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632537870868175887" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_routingGuid</ap>
        <ap name="Value2" type="variable">requestingGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175887" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="867" y="605">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="variable">responseMessage</ap>
      </Properties>
    </node>
    <node type="Action" id="632538282337813099" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="166" y="312">
      <linkto id="632534030134646076" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632538282337813100" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632538282337813100" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="166" y="415">
      <linkto id="632538282337813102" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">CALL_ENDING</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632538282337813101" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1055.4873" y="311">
      <linkto id="632537870868175885" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632538282337813102" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="165" y="539" />
    <node type="Variable" id="632527325857790929" name="requestingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="SenderRoutingGuid" refType="reference">requestingGuid</Properties>
    </node>
    <node type="Variable" id="632527325857790931" name="success" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="" defaultInitWith="true" refType="reference">success</Properties>
    </node>
    <node type="Variable" id="632537870868175886" name="responseMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">responseMessage</Properties>
    </node>
    <node type="Variable" id="632538425211819646" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TimerId" defaultInitWith="0" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordingEvent" startnode="632527335560196783" treenode="632527335560196784" appnode="632527335560196781" handlerfor="632527335560196780">
    <node type="Start" id="632527335560196783" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="312">
      <linkto id="632538282337813103" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632537870868175526" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="299" y="659">
      <linkto id="632537870868175528" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Success" type="variable">success</ap>
        <ap name="ConferenceId" type="csharp">(g_conferenceId == null) ? string.Empty : g_conferenceId</ap>
        <ap name="ResponseMessage" type="variable">responseMessage</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="EventName" type="literal">Metreos.Events.RemoteAgent.RecordingEventResponse</ap>
        <ap name="ToGuid" type="variable">requestingGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175528" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="409" y="659">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175529" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="224" y="313">
      <linkto id="632537870868175532" type="Labeled" style="Bezier" ortho="true" label="begin" />
      <linkto id="632537870868175533" type="Labeled" style="Bezier" ortho="true" label="end" />
      <linkto id="632537870868175534" type="Labeled" style="Bezier" ortho="true" label="status" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">actionType</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175530" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="685" y="94">
      <linkto id="632537870868175559" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632537870868175572" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Label" id="632537870868175532" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="339" y="190" />
    <node type="Label" id="632537870868175533" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="338" y="312" />
    <node type="Label" id="632537870868175534" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="341" y="435" />
    <node type="Label" id="632537870868175535" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="590" y="94">
      <linkto id="632537870868175530" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632537870868175538" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="649" y="833">
      <linkto id="632537870868175870" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632537870868175871" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175540" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="667" y="557">
      <linkto id="632537870868175556" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632537870868175579" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Action" id="632537870868175552" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1053" y="79" mx="1113" my="95">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632537870868175546" />
        <item text="OnRecord_Failed" treenode="632537870868175551" />
      </items>
      <linkto id="632537870868175877" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720469722" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CommandTimeout" type="literal">0</ap>
        <ap name="Expires" type="literal">0</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ConnectionId">g_recordingConnectionId</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175555" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="573" y="556">
      <linkto id="632537870868175540" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632537870868175556" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="801" y="557">
      <linkto id="632537870868175577" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632537870868175581" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_recordingConnectionId</ap>
      </Properties>
    </node>
    <node type="Label" id="632537870868175557" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="560" y="833">
      <linkto id="632537870868175538" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632537870868175559" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="881" y="94">
      <linkto id="632537870868175552" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632537870868175575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isPeerToPeer</ap>
      </Properties>
    </node>
    <node type="Comment" id="632537870868175562" text="Expiration?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="988" y="32" />
    <node type="Action" id="632537870868175567" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1368" y="93">
      <linkto id="632593879973703850" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">RECORDING</ap>
        <ap name="Value3" type="literal">true</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
        <rd field="ResultData3">g_recording</rd>
      </Properties>
    </node>
    <node type="Comment" id="632537870868175569" text="Legal values of responseMessage:&#xD;&#xA;RECORDING&#xD;&#xA;NOT_RECORDING&#xD;&#xA;STOPMEDIAFAIL&#xD;&#xA;NOCONFERENCE&#xD;&#xA;CALL_ENDING" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="129" y="37" />
    <node type="Label" id="632537870868175570" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="193" y="660">
      <linkto id="632537870868175526" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632537870868175571" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1587" y="93" />
    <node type="Action" id="632537870868175572" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="685" y="190">
      <linkto id="632537870868175574" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">RECORDING</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Comment" id="632537870868175573" text="if p2p, need to req conf" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="809" y="32" />
    <node type="Label" id="632537870868175574" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="685" y="288" />
    <node type="Action" id="632537870868175575" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="819.626" y="170" mx="885" my="186">
      <items count="1">
        <item text="OnRequestConference" treenode="632527325857790928" />
      </items>
      <linkto id="632537870868175552" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632537870868175888" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SenderRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="FunctionName" type="literal">OnRequestConference</ap>
      </Properties>
    </node>
    <node type="Comment" id="632537870868175576" text="What about conferenceId?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="738" y="500" />
    <node type="Action" id="632537870868175577" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="940" y="555">
      <linkto id="632537870868175578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">NOT_RECORDING</ap>
        <ap name="Value3" type="literal">false</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
        <rd field="ResultData3">g_recording</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175578" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1041" y="555" />
    <node type="Action" id="632537870868175579" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="668" y="657">
      <linkto id="632537870868175580" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">NOT_RECORDING</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175580" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="668" y="753" />
    <node type="Action" id="632537870868175581" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="800" y="656">
      <linkto id="632537870868175583" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">STOPMEDIAFAIL</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175583" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="800" y="754" />
    <node type="Action" id="632537870868175870" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="763" y="834">
      <linkto id="632537870868175873" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">RECORDING</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632537870868175871" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="648" y="936">
      <linkto id="632537870868175874" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="literal">NOT_RECORDING</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175873" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="859" y="835" />
    <node type="Label" id="632537870868175874" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="648" y="1043.5" />
    <node type="Comment" id="632537870868175876" text="here or in async handler? recordingstop?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="935.3828" y="513" />
    <node type="Action" id="632537870868175877" name="WriteRecordingStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1254.38281" y="94">
      <linkto id="632537870868175567" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="MediaType" type="literal">Wav</ap>
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="UsersId" type="variable">g_userId</ap>
        <rd field="RecordingsId">g_recordingId</rd>
      </Properties>
    </node>
    <node type="Action" id="632537870868175888" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="883" y="305">
      <linkto id="632537870868175889" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">NOCONFERENCE</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632537870868175889" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="883" y="409" />
    <node type="Action" id="632538282337813103" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="109" y="313">
      <linkto id="632537870868175529" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632538282337813106" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632538282337813106" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="109" y="411">
      <linkto id="632538282337813108" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">CALL_ENDING</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632538282337813108" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="109" y="508" />
    <node type="Action" id="632555905720469722" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1111" y="237">
      <linkto id="632555905720469723" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">NOT_RECORDING</ap>
        <rd field="ResultData">success</rd>
        <rd field="ResultData2">responseMessage</rd>
      </Properties>
    </node>
    <node type="Label" id="632555905720469723" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1112" y="343" />
    <node type="Action" id="632593879973703850" name="UpdateAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="1479" y="93">
      <linkto id="632537870868175571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentRecordId" type="variable">g_agentRecordId</ap>
        <ap name="IsRecorded" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Variable" id="632537870868175524" name="requestingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="SenderRoutingGuid" refType="reference">requestingGuid</Properties>
    </node>
    <node type="Variable" id="632537870868175525" name="actionType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ActionType" refType="reference">actionType</Properties>
    </node>
    <node type="Variable" id="632537870868175527" name="success" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" defaultInitWith="true" refType="reference">success</Properties>
    </node>
    <node type="Variable" id="632537870868175568" name="responseMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">responseMessage</Properties>
    </node>
    <node type="Variable" id="632538425211819645" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TimerId" defaultInitWith="0" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632537870868175545" treenode="632537870868175546" appnode="632537870868175543" handlerfor="632537870868175542">
    <node type="Start" id="632537870868175545" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="165">
      <linkto id="632538282337813082" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632537870868175565" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="865" y="162">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632537870868175880" name="WriteRecordingStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="356" y="165">
      <linkto id="632537870868175881" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_recordingId</ap>
        <ap name="Filepath" type="variable">mediaFileURL</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRecord_Complete: WriteRecordingStop with Filepath '" + mediaFileURL + "' for RecordingID: " + g_recordingId</log>
      </Properties>
    </node>
    <node type="Action" id="632537870868175881" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="487" y="164">
      <linkto id="632538282337813093" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">0</ap>
        <rd field="ResultData">g_recording</rd>
        <rd field="ResultData2">g_recordingId</rd>
      </Properties>
    </node>
    <node type="Action" id="632538282337813082" name="MoveMediaToAppSever" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="208" y="165">
      <linkto id="632537870868175880" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632537870868175880" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="MediaFilename" type="variable">filename</ap>
        <ap name="MediaServerIp" type="variable">g_mediaServerIP</ap>
        <rd field="MediafileUrl">mediaFileURL</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRecord_Complete: moving file '" + filename + "' to media server '" + g_mediaServerIP + "' and associating it with user: " + g_userId</log>
        <log condition="default" on="true" level="Warning" type="literal">OnRecord_Complete: Could not move media to media server. </log>
      </Properties>
    </node>
    <node type="Action" id="632538282337813093" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="590" y="164">
      <linkto id="632538282337813094" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632593879973703851" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632538282337813094" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="591" y="278">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632585988688885992" text="Move the recording to the proper directory on the associated media server, &#xD;&#xA;and close the recording record" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="44" y="84" />
    <node type="Comment" id="632585988688885993" text="Set values used to determine recording state to their initial values.&#xD;&#xA;Check to see if we're supposed to end the script.&#xD;&#xA;This would occur if RemoteHangup gets handled before this handler&#xD;&#xA;executes." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="484" y="75" />
    <node type="Action" id="632593879973703851" name="UpdateAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="714.410156" y="163">
      <linkto id="632537870868175565" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentRecordId" type="variable">g_agentRecordId</ap>
        <ap name="IsRecorded" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Variable" id="632537870868175875" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" defaultInitWith="none" refType="reference" name="Metreos.MediaControl.Record_Complete">filename</Properties>
    </node>
    <node type="Variable" id="632538282337813083" name="mediaFileURL" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mediaFileURL</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632537870868175550" treenode="632537870868175551" appnode="632537870868175548" handlerfor="632537870868175547">
    <node type="Start" id="632537870868175550" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="153">
      <linkto id="632538282337813084" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632537870868175564" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="640" y="152">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632538282337813084" name="WriteRecordingStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="157.733063" y="153">
      <linkto id="632538282337813085" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="RecordingsId" type="variable">g_recordingId</ap>
        <ap name="Filepath" type="csharp">null</ap>
      </Properties>
    </node>
    <node type="Action" id="632538282337813085" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="272.733063" y="153">
      <linkto id="632538282337813095" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="literal">0</ap>
        <rd field="ResultData">g_recording</rd>
        <rd field="ResultData2">g_recordingId</rd>
      </Properties>
    </node>
    <node type="Action" id="632538282337813095" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="382.942078" y="152">
      <linkto id="632538282337813096" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632593879973703853" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632538282337813096" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="381.942078" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632585988688885994" text="Set values used to determine recording state to their initial values.&#xD;&#xA;Check to see if we're supposed to end the script.&#xD;&#xA;This would occur if RemoteHangup gets handled before this handler&#xD;&#xA;executes." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="371" y="50" />
    <node type="Comment" id="632585988688885995" text="Close the recording record. Not a whole lot else to do." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="57" y="87" />
    <node type="Action" id="632593879973703853" name="UpdateAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="494.410156" y="153">
      <linkto id="632537870868175564" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentRecordId" type="variable">g_agentRecordId</ap>
        <ap name="IsRecorded" type="variable">g_recording</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632555905720469711" treenode="632555905720469712" appnode="632555905720469709" handlerfor="632555905720469708">
    <node type="Start" id="632555905720469711" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632555905720469718" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720469718" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="109" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632555905720469716" treenode="632555905720469717" appnode="632555905720469714" handlerfor="632555905720469713">
    <node type="Start" id="632555905720469716" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="196">
      <linkto id="632555905720469719" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720469719" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="126" y="196">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632585988688885981" treenode="632585988688885982" appnode="632585988688885979" handlerfor="632585988688885978">
    <node type="Start" id="632585988688885981" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="187">
      <linkto id="632585988688885983" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632585988688885983" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="187">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632585988688885984" text="This event is here just so that DigitPresses during the Agent call do not generate warnings in the log about unhandled GotDigits" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="107" />
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632585988688885988" treenode="632585988688885989" appnode="632585988688885986" handlerfor="632585988688885985">
    <node type="Start" id="632585988688885988" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="175">
      <linkto id="632585988688885990" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632585988688885990" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="175">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632585988688885991" text="Avoiding 'Unhandled StopTx event' messages in the log" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="249" />
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>