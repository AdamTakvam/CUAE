<Application name="AddCallToConference" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="AddCallToConference">
    <outline>
      <treenode type="evh" id="632341428970781877" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632116075241171830" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632116075241171829" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341428970781886" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632116852951182364" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632116852951182363" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632767222799198905" actid="632477082348579895" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341428970781909" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632120106495874930" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632120106495874929" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632767222799198903" actid="632477082348579895" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341428970781911" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632120106495874934" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632120106495874933" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632767222799198904" actid="632477082348579895" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341428970781913" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632128058030746773" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632128058030746772" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632767222799198732" actid="632472703757845832" />
          <ref id="632767222799198735" actid="632472703757845833" />
          <ref id="632767222799198738" actid="632472703757845834" />
          <ref id="632767222799198819" actid="632128157350202435" />
          <ref id="632767222799198826" actid="632129001672656620" />
          <ref id="632767222799198829" actid="632129001672656629" />
          <ref id="632767222799198832" actid="632129001672656632" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341428970781923" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632128058030746777" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632128058030746776" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632767222799198733" actid="632472703757845832" />
          <ref id="632767222799198736" actid="632472703757845833" />
          <ref id="632767222799198739" actid="632472703757845834" />
          <ref id="632767222799198820" actid="632128157350202435" />
          <ref id="632767222799198827" actid="632129001672656620" />
          <ref id="632767222799198830" actid="632129001672656629" />
          <ref id="632767222799198833" actid="632129001672656632" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341428970781933" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632120356930982891" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632120356930982890" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632767222799198727" actid="632472703757845828" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632341428970781935" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632120356930982895" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632120356930982894" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632767222799198728" actid="632472703757845828" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632477847742812582" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632477847742812579" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632477847742812578" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632477847742812587" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632477847742812584" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632477847742812583" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632477847742812592" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632477847742812589" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632477847742812588" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632341428970781937" level="1" text="InviteCallee">
        <node type="function" name="InviteCallee" id="632119221846373852" path="Metreos.StockTools" />
        <calls>
          <ref actid="632472703757845921" />
        </calls>
      </treenode>
      <treenode type="fun" id="632341428970781942" level="1" text="SendExecute">
        <node type="function" name="SendExecute" id="632120084318084735" path="Metreos.StockTools" />
        <calls>
          <ref actid="632472703757845839" />
          <ref actid="632472703757845928" />
          <ref actid="632472703757845940" />
          <ref actid="632120290972038573" />
          <ref actid="632120356930982886" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_AppServerIP" id="632767222799198673" vid="632116852951182312">
        <Properties type="Metreos.Types.String" initWith="AppServerIP">g_AppServerIP</Properties>
      </treenode>
      <treenode text="g_RecordFileURL" id="632767222799198675" vid="632116852951182314">
        <Properties type="Metreos.Types.String" initWith="RecordFileURL">g_RecordFileURL</Properties>
      </treenode>
      <treenode text="g_SmtpServer" id="632767222799198677" vid="632116852951182316">
        <Properties type="Metreos.Types.String" initWith="SMTP_Server">g_SmtpServer</Properties>
      </treenode>
      <treenode text="g_SmtpUsername" id="632767222799198679" vid="632116852951182318">
        <Properties type="Metreos.Types.String" defaultInitWith=".UNDEFINED." initWith="SMTP_Username">g_SmtpUsername</Properties>
      </treenode>
      <treenode text="g_SmtpPassword" id="632767222799198681" vid="632116852951182320">
        <Properties type="Metreos.Types.String" defaultInitWith=".UNDEFINED." initWith="SMTP_Password">g_SmtpPassword</Properties>
      </treenode>
      <treenode text="g_SmtpFromAddress" id="632767222799198683" vid="632116852951182322">
        <Properties type="Metreos.Types.String" initWith="SMTP_FromAddress">g_SmtpFromAddress</Properties>
      </treenode>
      <treenode text="g_SmtpSubject" id="632767222799198685" vid="632116852951182324">
        <Properties type="Metreos.Types.String" initWith="SMTP_Subject">g_SmtpSubject</Properties>
      </treenode>
      <treenode text="g_SmtpBody" id="632767222799198687" vid="632116852951182326">
        <Properties type="Metreos.Types.String" initWith="SMTP_Body">g_SmtpBody</Properties>
      </treenode>
      <treenode text="g_DialPlan" id="632767222799198689" vid="632116852951182328">
        <Properties type="Metreos.Types.Hashtable" initWith="DialPlan">g_DialPlan</Properties>
      </treenode>
      <treenode text="g_SendLink" id="632767222799198691" vid="632116852951182330">
        <Properties type="Metreos.Types.Bool" initWith="SendRecordedFileAsLink">g_SendLink</Properties>
      </treenode>
      <treenode text="g_RecordExpire" id="632767222799198693" vid="632116852951182332">
        <Properties type="Metreos.Types.Int" initWith="RecordFileExpireTime">g_RecordExpire</Properties>
      </treenode>
      <treenode text="g_ConferenceData" id="632767222799198695" vid="632116852951182334">
        <Properties type="Metreos.Types.ClickToTalk.ConferenceData">g_ConferenceData</Properties>
      </treenode>
      <treenode text="g_ConferenceId" id="632767222799198697" vid="632116852951182336">
        <Properties type="Metreos.Types.String">g_ConferenceId</Properties>
      </treenode>
      <treenode text="g_MsConnections" id="632767222799198699" vid="632116852951182338">
        <Properties type="Metreos.Types.Hashtable">g_MsConnections</Properties>
      </treenode>
      <treenode text="g_RecordConnectionId" id="632767222799198701" vid="632116852951182340">
        <Properties type="Metreos.Types.String">g_RecordConnectionId</Properties>
      </treenode>
      <treenode text="g_NumCalleesContacted" id="632767222799198703" vid="632116852951182342">
        <Properties type="Metreos.Types.Int">g_NumCalleesContacted</Properties>
      </treenode>
      <treenode text="g_callees" id="632767222799198705" vid="632128143823552065">
        <Properties type="Metreos.Types.ClickToTalk.CalleeCollection">g_callees</Properties>
      </treenode>
      <treenode text="g_hostConnectionId" id="632767222799198707" vid="632128183102187780">
        <Properties type="Metreos.Types.String">g_hostConnectionId</Properties>
      </treenode>
      <treenode text="g_hostCallId" id="632767222799198709" vid="632129001672657048">
        <Properties type="Metreos.Types.String">g_hostCallId</Properties>
      </treenode>
      <treenode text="g_NumMakeCallAbort" id="632767222799198711" vid="632129001672657053">
        <Properties type="Metreos.Types.Int">g_NumMakeCallAbort</Properties>
      </treenode>
      <treenode text="g_inCall" id="632767222799198713" vid="632133290852188032">
        <Properties type="Metreos.Types.Bool">g_inCall</Properties>
      </treenode>
      <treenode text="g_CM_Username" id="632767222799198715" vid="632477159022886795">
        <Properties type="String" initWith="CCM_Device_Username">g_CM_Username</Properties>
      </treenode>
      <treenode text="g_CM_Password" id="632767222799198717" vid="632477159022886797">
        <Properties type="String" initWith="CCM_Device_Password">g_CM_Password</Properties>
      </treenode>
      <treenode text="g_DbConferenceId" id="632767222799198719" vid="632536540248685379">
        <Properties type="UInt">g_DbConferenceId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632767222799198918" vid="632767222799198917">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632116075241171831" treenode="632341428970781877" appnode="632116075241171830" handlerfor="632116075241171829">
    <node type="Loop" id="632472703757845920" name="Loop" text="loop (expr)" cx="168" cy="124" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1216" y="64" mx="1300" my="126">
      <linkto id="632472703757845921" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632472703757845930" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">g_callees.Count</Properties>
    </node>
    <node type="Start" id="632116075241171831" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="120">
      <linkto id="632472703757845936" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632129001672656600" text="Answer the host's phone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="95.5293" y="73" />
    <node type="Action" id="632341438297500673" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="232" y="120">
      <linkto id="632536540248685377" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(Hashtable g_MsConnections, string callId, 
string g_hostConnectionId, ref string g_SmtpUsername, ref string g_SmtpPassword)
{
	g_MsConnections.Add(callId, g_hostConnectionId);

	if(g_SmtpUsername == ".UNDEFINED.")
	{
		g_SmtpUsername = String.Empty;
	}

	if(g_SmtpPassword == ".UNDEFINED.")
	{
		g_SmtpPassword = String.Empty;
	}
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632472703757845823" name="GetCallees" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="600" y="120">
      <linkto id="632472703757845826" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632472703757845824" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceID" type="variable">g_DbConferenceId</ap>
        <rd field="ResultData">g_callees</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"Number of contacts to call: " + g_callees.Count</log>
      </Properties>
    </node>
    <node type="Action" id="632472703757845824" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="720" y="120">
      <linkto id="632472703757845828" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632472703757845834" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_ConferenceData.Record</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845826" name="SaveConferenceError" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="533" y="240">
      <linkto id="632472703757845839" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
        <ap name="Error" type="literal">No callees found for conference</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845828" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="656" y="248" mx="716" my="264">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632341428970781933" />
        <item text="OnRecord_Failed" treenode="632341428970781935" />
      </items>
      <linkto id="632472703757845829" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632472703757845830" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CommandTimeout" type="literal">360000</ap>
        <ap name="Expires" type="variable">g_RecordExpire</ap>
        <ap name="ConferenceId" type="variable">g_ConferenceId</ap>
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ConnectionId">g_RecordConnectionId</rd>
        <log condition="default" on="true" level="Warning" type="literal">Unable to record conference</log>
      </Properties>
    </node>
    <node type="Action" id="632472703757845829" name="UpdateConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="720" y="392">
      <linkto id="632472703757845833" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ID" type="variable">g_DbConferenceId</ap>
        <ap name="RecordConnectionId" type="variable">g_RecordConnectionId</ap>
        <ap name="RecordEnded" type="literal">false</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845830" name="UpdateConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="864" y="264">
      <linkto id="632472703757845834" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ID" type="variable">g_DbConferenceId</ap>
        <ap name="RecordEnded" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845832" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="488" y="456" mx="541" my="472">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341428970781913" />
        <item text="OnPlay_Failed" treenode="632341428970781923" />
      </items>
      <linkto id="632472703757845847" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">c2t_error.wav</ap>
        <ap name="ConnectionId" type="csharp">g_MsConnections[callId]</ap>
        <ap name="UserData" type="literal">hostError</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845833" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="992" y="376" mx="1045" my="392">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341428970781913" />
        <item text="OnPlay_Failed" treenode="632341428970781923" />
      </items>
      <linkto id="632472703757845850" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">c2t_inviting_recording.wav</ap>
        <ap name="ConnectionId" type="csharp">g_MsConnections[callId]</ap>
        <ap name="UserData" type="literal">hostWelcome</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845834" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="816" y="104" mx="869" my="120">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341428970781913" />
        <item text="OnPlay_Failed" treenode="632341428970781923" />
      </items>
      <linkto id="632472703757845850" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">c2t_inviting.wav</ap>
        <ap name="ConnectionId" type="csharp">g_MsConnections[callId]</ap>
        <ap name="UserData" type="literal">hostWelcome</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845839" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="496" y="328" mx="536" my="344">
      <items count="1">
        <item text="SendExecute" />
      </items>
      <linkto id="632472703757845832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SendExecute</ap>
      </Properties>
    </node>
    <node type="Comment" id="632472703757845842" text="No callees found for the conference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="456" y="696" />
    <node type="Comment" id="632472703757845843" text="Flag RecordEnded" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="832" y="296" />
    <node type="Comment" id="632472703757845844" text="Flag RecordStart&#xD;&#xA;          and&#xD;&#xA;Save RecordId" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="680" y="448" />
    <node type="Comment" id="632472703757845845" text="The host's phone has been answered by the Application Server" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="696" y="512" />
    <node type="Comment" id="632472703757845846" text="Invite all the rest of the participants to the call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="696" y="544" />
    <node type="Action" id="632472703757845847" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="640">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632472703757845850" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1040" y="120">
      <linkto id="632472703757845920" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute()
{
	System.Threading.Thread.Sleep(4000);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632472703757845910" text="Invite all non-host participants" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="264.777466" y="210" />
    <node type="Action" id="632472703757845921" name="CallFunction" container="632472703757845920" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1264" y="96" mx="1301" my="112">
      <items count="1">
        <item text="InviteCallee" />
      </items>
      <linkto id="632472703757845920" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="HostIP" type="variable">mediaIP</ap>
        <ap name="conferenceId" type="variable">g_ConferenceId</ap>
        <ap name="calleeAddress" type="csharp">g_callees.GetAddress(loopIndex)</ap>
        <ap name="calleeName" type="csharp">g_callees.GetName(loopIndex)</ap>
        <ap name="FunctionName" type="literal">InviteCallee</ap>
      </Properties>
    </node>
    <node type="Comment" id="632472703757845922" text="Invite all non-host participants" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1160" y="192" />
    <node type="Action" id="632472703757845925" name="GetConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="464" y="120">
      <linkto id="632472703757845823" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632472703757845826" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
        <rd field="ResultData">g_ConferenceData</rd>
      </Properties>
    </node>
    <node type="Action" id="632472703757845928" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1528" y="192" mx="1568" my="208">
      <items count="1">
        <item text="SendExecute" />
      </items>
      <linkto id="632472703757845931" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SendExecute</ap>
      </Properties>
    </node>
    <node type="Comment" id="632472703757845929" text="Send errors to the phone if all attempts to call non-host participants fails" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1368" y="280" />
    <node type="Action" id="632472703757845930" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1448" y="120">
      <linkto id="632472703757845928" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632472703757845931" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_NumMakeCallAbort</ap>
        <ap name="Value2" type="csharp">g_callees.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845931" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1696" y="120">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632472703757845936" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="142" y="120">
      <linkto id="632341438297500673" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632536540248685376" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Click-To-Talk</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_hostCallId</rd>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_hostConnectionId</rd>
        <rd field="ConferenceId">g_ConferenceId</rd>
        <rd field="MediaTxIP">mediaIP</rd>
        <rd field="MediaTxPort">mediaPort</rd>
      </Properties>
    </node>
    <node type="Action" id="632472703757845939" name="SaveConferenceError" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="144" y="376">
      <linkto id="632472703757845940" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
        <ap name="Error" type="literal">Unable to fully connect the host to the call.  The call must end.</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845940" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="101.598251" y="472" mx="141" my="488">
      <items count="1">
        <item text="SendExecute" />
      </items>
      <linkto id="632472703757845946" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SendExecute</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845946" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="138" y="600">
      <linkto id="632472703757845947" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632472703757845947" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138" y="704">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632536540248685376" name="GetConferenceId" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="144" y="240">
      <linkto id="632472703757845939" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="HostIP" type="variable">mediaIP</ap>
        <rd field="ConferenceId">g_DbConferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632536540248685377" name="GetConferenceId" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="339" y="121">
      <linkto id="632472703757845925" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632472703757845832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="HostIP" type="variable">mediaIP</ap>
        <rd field="ConferenceId">g_DbConferenceId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632116852951182344" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632477159022886087" name="mediaIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">mediaIP</Properties>
    </node>
    <node type="Variable" id="632477159022886088" name="mediaPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">mediaPort</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="630000000000000002" treenode="632341428970781886" appnode="632116852951182364" handlerfor="632116852951182363">
    <node type="Loop" id="632129001672657042" name="Loop" text="loop (var)" cx="298.191284" cy="158" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="800.8087" y="32" mx="950" my="111">
      <linkto id="632472799207244721" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632538427756179774" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="dictEnum" type="variable">g_MsConnections</Properties>
    </node>
    <node type="Start" id="630000000000000002" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="57">
      <linkto id="632129001672657033" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632120356930982908" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="96" y="376">
      <linkto id="632341438297500669" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632472799207244718" type="Labeled" style="Bezier" ortho="true" label="1" />
      <linkto id="632120356930982911" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_MsConnections.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632120356930982911" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="640" y="368">
      <linkto id="632341438297500665" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632538427756179444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_ConferenceData.Recording</ap>
      </Properties>
    </node>
    <node type="Comment" id="632129001672656583" text="Last member of the conference is gone" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="544" y="760" />
    <node type="Comment" id="632129001672656584" text="This is last event for the application,&#xD;&#xA;unless the conference is being recorded." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="694.380249" y="527" />
    <node type="Action" id="632129001672657030" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="238" y="111">
      <linkto id="632341438297500667" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="csharp">g_MsConnections[callId]</ap>
      </Properties>
    </node>
    <node type="Action" id="632129001672657033" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="96" y="112">
      <linkto id="632129001672657030" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632345120667813210" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_MsConnections[callId]</ap>
        <ap name="Value2" type="variable">g_hostConnectionId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632129001672657034" text="Stop playing audio to the host" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="217" y="66" />
    <node type="Comment" id="632129001672657050" text="If there is one member left in the call, &#xD;&#xA;force a hangup" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="296" y="648" />
    <node type="Comment" id="632129001672657051" text="In the event that the host hangs up first, end all outstanding calls" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="794.5698" y="198" />
    <node type="Action" id="632341438297500665" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="768" y="368">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632341438297500667" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="371" y="111">
      <linkto id="632345120667813208" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute()
{
	System.Threading.Thread.Sleep(500);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632341438297500668" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="640" y="560">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297500669" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="96" y="600">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632345120667813208" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="509" y="111">
      <linkto id="632345685410625659" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(Hashtable g_MsConnections, string callId)
{
	g_MsConnections.Remove(callId);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632345120667813210" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="96" y="240">
      <linkto id="632120356930982908" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(Hashtable g_MsConnections, string callId)
{
	g_MsConnections.Remove(callId);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632345685410625659" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="640" y="112">
      <linkto id="632129001672657042" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632120356930982911" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_MsConnections.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632472799207244718" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="392" y="584">
      <linkto id="632538427756180907" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="csharp">Hack.GetCallId(g_MsConnections)</ap>
      </Properties>
    </node>
    <node type="Action" id="632472799207244721" name="Hangup" container="632129001672657042" class="MaxActionNode" group="" path="Metreos.CallControl" x="953" y="108">
      <linkto id="632129001672657042" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="csharp">loopDictEnum.Key</ap>
      </Properties>
    </node>
    <node type="Label" id="632484722858931872" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="520" y="264">
      <linkto id="632120356930982911" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632484722858932344" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="1296" y="112">
      <linkto id="632484722858932345" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="literal">Host exited.  Ending application</ap>
        <ap name="LogLevel" type="literal">Info</ap>
      </Properties>
    </node>
    <node type="Label" id="632484722858932345" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1400" y="112" />
    <node type="Action" id="632538427756179444" name="DeleteConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="640" y="472">
      <linkto id="632341438297500668" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632538427756179774" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="1192" y="112">
      <linkto id="632484722858932344" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref Hashtable g_MsConnections)
{
	g_MsConnections.Clear();
	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632538427756179775" text="Clear g_msConnections" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1152" y="40" />
    <node type="Action" id="632538427756180907" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="488" y="440">
      <linkto id="632120356930982911" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref Hashtable g_MsConnections)
{
	g_MsConnections.Clear();
	return "";
}
</Properties>
    </node>
    <node type="Comment" id="632538427756180908" text="Clear g_msConnections" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="432" y="408" />
    <node type="Variable" id="632120356930982905" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="CallId" refType="reference">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632120106495874931" treenode="632341428970781909" appnode="632120106495874930" handlerfor="632120106495874929">
    <node type="Start" id="632120106495874931" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="205">
      <linkto id="632127168472969974" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632120290972038569" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="820.1783" y="205">
      <linkto id="632120290972038573" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632341438297502403" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_inCall</ap>
        <ap name="Value2" type="literal">false</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"gCallees: " + g_callees.Count + " gNumCalleesContacted: " + g_NumCalleesContacted</log>
      </Properties>
    </node>
    <node type="Action" id="632120290972038570" name="GetConferenceErrors" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="592" y="205">
      <linkto id="632120290972038571" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632341438297502403" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632120290972038571" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="716" y="205">
      <linkto id="632120290972038569" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632341438297502403" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">errors.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632120290972038573" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="938.552" y="189" mx="978" my="205">
      <items count="1">
        <item text="SendExecute" />
      </items>
      <linkto id="632341438297502403" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SendExecute</ap>
      </Properties>
    </node>
    <node type="Action" id="632127168472969974" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="118" y="205">
      <linkto id="632472799207244724" type="Labeled" style="Bezier" ortho="true" label="hostGone" />
      <linkto id="632477847742812575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_MsConnections.Count == 0 ? "hostGone" : "continue"</ap>
      </Properties>
    </node>
    <node type="Action" id="632128157350202439" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="462" y="95">
      <linkto id="632341438297500683" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnectionId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632129001672656591" text="The host left prematurely.  End the call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="502" />
    <node type="Comment" id="632129001672656595" text="Send accumated errors to the host" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="795.4185" y="140" />
    <node type="Action" id="632133290852188034" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="336" y="205">
      <linkto id="632128157350202439" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632341438297500683" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_callees.Count</ap>
        <ap name="Value2" type="variable">g_NumCalleesContacted</ap>
      </Properties>
    </node>
    <node type="Action" id="632341438297500681" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="116" y="453">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297500683" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="462" y="205">
      <linkto id="632120290972038570" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_inCall</rd>
      </Properties>
    </node>
    <node type="Action" id="632341438297502403" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="752.936768" y="314">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632472799207244724" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="117" y="320">
      <linkto id="632341438297500681" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632477847742812575" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="217.90625" y="205">
      <linkto id="632133290852188034" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(Hashtable g_MsConnections, string callId, string connectionId, int g_NumCalleesContacted)
{
	g_MsConnections.Add(callId, connectionId);
	g_NumCalleesContacted++;

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Variable" id="632120290972038560" name="callee" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="to" refType="reference">callee</Properties>
    </node>
    <node type="Variable" id="632120290972038561" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632120290972038565" name="errors" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.StringCollection" refType="reference">errors</Properties>
    </node>
    <node type="Variable" id="632477847742812577" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632120106495874935" treenode="632341428970781911" appnode="632120106495874934" handlerfor="632120106495874933">
    <node type="Start" id="632120106495874935" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="87">
      <linkto id="632128157350202432" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632120356930982883" name="SaveConferenceError" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="407" y="87">
      <linkto id="632120356930982884" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
        <ap name="Error" type="csharp">String.Format("{0} failed to join the call", userData)</ap>
      </Properties>
    </node>
    <node type="Action" id="632120356930982884" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="532" y="87">
      <linkto id="632120356930982886" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632341438297500688" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_callees.Count</ap>
        <ap name="Value2" type="variable">g_NumCalleesContacted</ap>
        <log condition="exit" on="true" level="Info" type="csharp">"gCallees: " + g_callees.Count + " gNumCalleesContacted: " + g_NumCalleesContacted</log>
      </Properties>
    </node>
    <node type="Action" id="632120356930982886" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="647.758057" y="70" mx="687" my="86">
      <items count="1">
        <item text="SendExecute" />
      </items>
      <linkto id="632133029472344334" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SendExecute</ap>
      </Properties>
    </node>
    <node type="Action" id="632128157350202432" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="124" y="87">
      <linkto id="632128157350202436" type="Labeled" style="Bezier" ortho="true" label="1" />
      <linkto id="632341438297500687" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_callees.Count</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Reason code for the call to fail: " + reason</log>
      </Properties>
    </node>
    <node type="Action" id="632128157350202435" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="360" y="370" mx="413" my="386">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341428970781913" />
        <item text="OnPlay_Failed" treenode="632341428970781923" />
      </items>
      <linkto id="632129001672656637" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">c2t_busy.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnectionId</ap>
        <ap name="UserData" type="literal">hostError</ap>
      </Properties>
    </node>
    <node type="Action" id="632128157350202436" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="123" y="195">
      <linkto id="632341438297500685" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_hostConnectionId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632129001672656596" text="Send accumated errors to the host" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="514" y="32" />
    <node type="Comment" id="632129001672656597" text="In the case that this is a peer-to-peer call, simulate a busy tone if the call is busy" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="55" y="787" />
    <node type="Action" id="632129001672656610" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="122" y="386">
      <linkto id="632128157350202435" type="Labeled" style="Bezier" ortho="true" label="RemoteBusy" />
      <linkto id="632129001672656620" type="Labeled" style="Bezier" ortho="true" label="NoAnswer" />
      <linkto id="632129001672656632" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632129001672656629" type="Labeled" style="Bezier" ortho="true" label="Unreachable" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">reason</ap>
      </Properties>
    </node>
    <node type="Action" id="632129001672656620" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="260" y="449" mx="313" my="465">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341428970781913" />
        <item text="OnPlay_Failed" treenode="632341428970781923" />
      </items>
      <linkto id="632129001672656636" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">c2t_no_answer.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnectionId</ap>
        <ap name="UserData" type="literal">hostError</ap>
      </Properties>
    </node>
    <node type="Action" id="632129001672656629" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="134" y="486" mx="187" my="502">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341428970781913" />
        <item text="OnPlay_Failed" treenode="632341428970781923" />
      </items>
      <linkto id="632129001672656635" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">c2t_unreachable.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnectionId</ap>
        <ap name="UserData" type="literal">hostError</ap>
      </Properties>
    </node>
    <node type="Action" id="632129001672656632" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="14" y="486" mx="67" my="502">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632341428970781913" />
        <item text="OnPlay_Failed" treenode="632341428970781923" />
      </items>
      <linkto id="632129001672656634" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">c2t_error.wav</ap>
        <ap name="ConnectionId" type="variable">g_hostConnectionId</ap>
        <ap name="UserData" type="literal">hostError</ap>
      </Properties>
    </node>
    <node type="Label" id="632129001672656633" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="256" y="180">
      <linkto id="632341438297500687" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632129001672656634" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="63" y="634" />
    <node type="Label" id="632129001672656635" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="183" y="629" />
    <node type="Label" id="632129001672656636" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="309" y="588" />
    <node type="Label" id="632129001672656637" text="a" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="410" y="510.999969" />
    <node type="Action" id="632133029472344334" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="683.624634" y="192">
      <linkto id="632341438297500689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632477082348579887" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">(g_callees.Count &gt; 1)</ap>
      </Properties>
    </node>
    <node type="Comment" id="632133029472344337" text="If all calls have been made, &#xD;&#xA;and if not a two-party call,&#xD;&#xA;then hangup the host" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="737.4186" y="173" />
    <node type="Action" id="632341438297500685" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="123.90625" y="284">
      <linkto id="632129001672656610" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute()
{
	System.Threading.Thread.Sleep(1000);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632341438297500687" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="256" y="87">
      <linkto id="632120356930982883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(Hashtable g_MsConnections, ref int g_NumCalleesContacted, string callId)
{
	g_MsConnections.Remove(callId);
	g_NumCalleesContacted++;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632341438297500688" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="532.1797" y="189">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297500689" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="759.1797" y="305">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632477082348579887" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="626.6504" y="305">
      <linkto id="632341438297500689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_hostCallId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632120290972038575" name="callee" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="from" refType="reference">callee</Properties>
    </node>
    <node type="Variable" id="632120290972038576" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="callId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632126359692500488" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632128157350202431" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">reason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632128058030746774" treenode="632341428970781913" appnode="632128058030746773" handlerfor="632128058030746772">
    <node type="Start" id="632128058030746774" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="88">
      <linkto id="632128949322187820" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632128949322187820" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="151" y="89">
      <linkto id="632341438297500690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632477082348579888" type="Labeled" style="Bezier" ortho="true" label="hostError" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Comment" id="632128949322187834" text="The host was not able to be connected to the conference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="56" y="383" />
    <node type="Action" id="632341438297500690" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="321" y="89">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297500692" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="154" y="341">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632477082348579888" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="152" y="213">
      <linkto id="632341438297500692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="csharp">Hack.GetCallId(g_MsConnections)</ap>
      </Properties>
    </node>
    <node type="Variable" id="632128949322187821" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632128058030746778" treenode="632341428970781923" appnode="632128058030746777" handlerfor="632128058030746776">
    <node type="Start" id="632128058030746778" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632128949322187832" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632128949322187832" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="180" y="147">
      <linkto id="632341438297500693" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632477082348579889" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">String.Compare(userData, "hostError")</ap>
      </Properties>
    </node>
    <node type="Comment" id="632128949322187833" text="The host was not able to be connected to the conference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="112" y="335" />
    <node type="Action" id="632341438297500693" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="195">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632477082348579889" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="180" y="263">
      <linkto id="632341438297500693" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="csharp">Hack.GetCallId(g_MsConnections)</ap>
      </Properties>
    </node>
    <node type="Variable" id="632129001672656607" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="userData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632120356930982892" treenode="632341428970781933" appnode="632120356930982891" handlerfor="632120356930982890">
    <node type="Start" id="632120356930982892" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632120356930982900" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632120356930982900" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="120" y="92">
      <linkto id="632120356930982901" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632538427756179068" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_SendLink</ap>
      </Properties>
    </node>
    <node type="Action" id="632120356930982901" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="252" y="91">
      <linkto id="632127131572091228" type="Labeled" style="Bezier" label="default" />
      <linkto id="632127131572091228" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="To" type="csharp">g_ConferenceData.EmailAddress</ap>
        <ap name="From" type="variable">g_SmtpFromAddress</ap>
        <ap name="MailServer" type="variable">g_SmtpServer</ap>
        <ap name="Username" type="variable">g_SmtpUsername</ap>
        <ap name="Password" type="variable">g_SmtpPassword</ap>
        <ap name="Subject" type="variable">g_SmtpSubject</ap>
        <ap name="Body" type="csharp">String.Format("{0}{1}\n\nNote: Link is only valid for {2} days.", g_RecordFileURL, filename, g_RecordExpire)</ap>
        <log condition="success" on="true" level="Info" type="csharp">String.Format("Recorded conference emailed to host ({0}) successfully", g_ConferenceData.EmailAddress)</log>
        <log condition="default" on="true" level="Error" type="csharp">String.Format("Failure emailing the recording to host ({0})", g_ConferenceData.EmailAddress)</log>
      </Properties>
    </node>
    <node type="Action" id="632120356930982902" name="Send" class="MaxActionNode" group="" path="Metreos.Native.Mail" x="120" y="280">
      <linkto id="632127131572091228" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="To" type="csharp">g_ConferenceData.EmailAddress</ap>
        <ap name="From" type="variable">g_SmtpFromAddress</ap>
        <ap name="MailServer" type="variable">g_SmtpServer</ap>
        <ap name="Username" type="variable">g_SmtpUsername</ap>
        <ap name="Password" type="variable">g_SmtpPassword</ap>
        <ap name="Subject" type="variable">g_SmtpSubject</ap>
        <ap name="Body" type="variable">g_SmtpBody</ap>
        <ap name="AttachmentPaths" type="variable">attachments</ap>
        <log condition="default" on="true" level="Info" type="csharp">String.Format("Recorded conference emailed to host ({0}) successfully", g_ConferenceData.EmailAddress)</log>
      </Properties>
    </node>
    <node type="Action" id="632127131572091228" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="251" y="212">
      <linkto id="632127131572091229" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632538427756179446" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_MsConnections.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632127131572091229" name="UpdateConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="373" y="212">
      <linkto id="632127131572091705" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ID" type="variable">g_DbConferenceId</ap>
        <ap name="RecordEnded" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632127131572091705" name="GetConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="536" y="212">
      <linkto id="632341438297500695" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
        <rd field="ResultData">g_ConferenceData</rd>
      </Properties>
    </node>
    <node type="Comment" id="632129001672656571" text="All callees are fully destroyed" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="176" y="496" />
    <node type="Comment" id="632129001672656603" text="The recording has stopped." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="309.7611" y="78" />
    <node type="Comment" id="632129001672656604" text="If the recording lasted until the last member of the call, &#xD;&#xA;this event will be the final event.&#xD;&#xA;Exit the application in that case." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="308.7611" y="101" />
    <node type="Action" id="632341438297500694" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="248" y="456">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">Recording ended</log>
      </Properties>
    </node>
    <node type="Action" id="632341438297500695" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="679" y="212">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632538427756179068" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="120" y="184">
      <linkto id="632120356930982902" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(System.Collections.Specialized.StringCollection attachments, string g_RecordFileURL, string filename)
{
	attachments.Add(g_RecordFileURL + filename);
	return "";
}
</Properties>
    </node>
    <node type="Action" id="632538427756179446" name="DeleteConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="248" y="344">
      <linkto id="632341438297500694" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632120356930982899" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="filename" refType="reference">filename</Properties>
    </node>
    <node type="Variable" id="632538427756179069" name="attachments" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">attachments</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632120356930982896" treenode="632341428970781935" appnode="632120356930982895" handlerfor="632120356930982894">
    <node type="Start" id="632120356930982896" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632127131572091706" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632127131572091706" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="151" y="82">
      <linkto id="632127131572091707" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632341438297500696" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">g_MsConnections.Count</ap>
      </Properties>
    </node>
    <node type="Action" id="632127131572091707" name="UpdateConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="151" y="198">
      <linkto id="632127131572091708" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ID" type="variable">g_DbConferenceId</ap>
        <ap name="RecordEnded" type="literal">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632127131572091708" name="GetConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="302" y="198">
      <linkto id="632341438297500697" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
      </Properties>
    </node>
    <node type="Comment" id="632129001672656573" text="All callees are fully destroyed" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="244" y="32" />
    <node type="Action" id="632341438297500696" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="287" y="81">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297500697" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="433" y="198">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632477847742812581" treenode="632477847742812582" appnode="632477847742812579" handlerfor="632477847742812578">
    <node type="Start" id="632477847742812581" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632477847742812593" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632477847742812593" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="157" y="98">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632477847742812586" treenode="632477847742812587" appnode="632477847742812584" handlerfor="632477847742812583">
    <node type="Start" id="632477847742812586" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632477847742812594" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632477847742812594" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="138" y="95">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632477847742812591" treenode="632477847742812592" appnode="632477847742812589" handlerfor="632477847742812588">
    <node type="Start" id="632477847742812591" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632477847742812595" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632477847742812595" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="172" y="131">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="InviteCallee" activetab="true" startnode="630000000000000006" treenode="632341428970781937" appnode="632119221846373852" handlerfor="632477847742812588">
    <node type="Start" id="630000000000000006" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="110">
      <linkto id="632120106495874927" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632120106495874927" name="FormatAddress" class="MaxActionNode" group="" path="Metreos.Native.DialPlan" x="140" y="110">
      <linkto id="632120106495874939" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632477082348579895" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="DialedNumber" type="variable">calleeAddress</ap>
        <ap name="DialingRules" type="variable">g_DialPlan</ap>
        <rd field="ResultData">formattedCallee</rd>
      </Properties>
    </node>
    <node type="Action" id="632120106495874939" name="SaveConferenceError" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="140" y="240">
      <linkto id="632341438297500698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
        <ap name="Error" type="csharp">String.Format("Encountered configuration error while applying dial plan to '{0}'", calleeAddress)</ap>
      </Properties>
    </node>
    <node type="Action" id="632120106495874940" name="SaveConferenceError" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="287" y="244">
      <linkto id="632341438297500698" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferenceId" type="variable">g_DbConferenceId</ap>
        <ap name="Error" type="csharp">String.Format("Failed to make outbound call to {0}", calleeAddress)</ap>
      </Properties>
    </node>
    <node type="Comment" id="632129001672656590" text="Unable to call a participant" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="141" y="505" />
    <node type="Comment" id="632129001672656606" text="This method is called for every non-host participant" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="82.76111" y="545" />
    <node type="Comment" id="632129001672657056" text="Take note of # failed attempts to call participants" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="257" y="347" />
    <node type="Action" id="632341438297500698" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="207" y="354">
      <linkto id="632341438297500701" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref int g_NumMakeCallAbort)
{
	g_NumMakeCallAbort++;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632341438297500700" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="418" y="110">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632341438297500701" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="206" y="462">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632477082348579895" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="224" y="94" mx="290" my="110">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632341428970781909" />
        <item text="OnMakeCall_Failed" treenode="632341428970781911" />
        <item text="OnRemoteHangup" treenode="632341428970781886" />
      </items>
      <linkto id="632120106495874940" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632341438297500700" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">formattedCallee</ap>
        <ap name="From" type="csharp">g_ConferenceData.HostDescription</ap>
        <ap name="DisplayName" type="csharp">g_ConferenceData.HostDescription</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_ConferenceId</ap>
        <ap name="UserData" type="variable">calleeName</ap>
        <rd field="CallId">callId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632120106495874918" name="calleeAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="calleeAddress" refType="reference">calleeAddress</Properties>
    </node>
    <node type="Variable" id="632120106495874919" name="calleeName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="calleeName" refType="reference">calleeName</Properties>
    </node>
    <node type="Variable" id="632120106495874921" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632120106495874925" name="formattedCallee" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">formattedCallee</Properties>
    </node>
    <node type="Variable" id="632477159022886089" name="hostIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="HostIP" refType="reference">hostIP</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SendExecute" startnode="632120084318084736" treenode="632341428970781942" appnode="632120084318084735" handlerfor="632477847742812588">
    <node type="Start" id="632120084318084736" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632120106495874913" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632120106495874913" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="110" y="89">
      <linkto id="632120106495874914" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">String.Format("http://{0}:8000/click-to-talk/errors.xml", g_AppServerIP)</ap>
        <rd field="ResultData">executeMsg</rd>
      </Properties>
    </node>
    <node type="Action" id="632120106495874914" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="246" y="89">
      <linkto id="632341438297500709" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="variable">executeMsg</ap>
        <ap name="URL" type="csharp">g_ConferenceData.HostIP</ap>
        <ap name="Username" type="variable">g_CM_Username</ap>
        <ap name="Password" type="variable">g_CM_Password</ap>
        <log condition="success" on="true" level="Info" type="literal">Sent execute</log>
      </Properties>
    </node>
    <node type="Comment" id="632129001672656598" text="Send a Cisco IP Phone Menu to the host's IP phone containing the accumulated errors for this call" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="135" />
    <node type="Action" id="632341438297500709" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="387" y="89">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Variable" id="632120106495874915" name="executeMsg" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeMsg</Properties>
    </node>
  </canvas>
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>