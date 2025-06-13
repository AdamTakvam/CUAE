<Application name="EnrollSpeaker" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="EnrollSpeaker">
    <outline>
      <treenode type="evh" id="632739556158359907" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632739556158359904" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632739556158359903" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744023639228095" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632744023639228092" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632744023639228091" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632899400480156748" actid="632744960684114736" />
          <ref id="632899400480156763" actid="632744834910247472" />
          <ref id="632899400480156792" actid="632744023639228128" />
          <ref id="632899400480156795" actid="632744023639228131" />
          <ref id="632899400480156803" actid="632744723103660020" />
          <ref id="632899400480156884" actid="632744723103660416" />
          <ref id="632899400480156887" actid="632744723103660419" />
          <ref id="632899400480156893" actid="632744999415327118" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744023639228111" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632744023639228108" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632744023639228107" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632899400480156781" actid="632744023639228117" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744023639228116" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632744023639228113" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632744023639228112" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632899400480156782" actid="632744023639228117" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744723103660019" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632744723103660016" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632744723103660015" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632899400480156749" actid="632744960684114736" />
          <ref id="632899400480156764" actid="632744834910247472" />
          <ref id="632899400480156793" actid="632744023639228128" />
          <ref id="632899400480156796" actid="632744023639228131" />
          <ref id="632899400480156804" actid="632744723103660020" />
          <ref id="632899400480156885" actid="632744723103660416" />
          <ref id="632899400480156888" actid="632744723103660419" />
          <ref id="632899400480156894" actid="632744999415327118" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744834910247454" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632744834910247451" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632744834910247450" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632899400480156872" actid="632744834910247460" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744834910247459" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632744834910247456" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632744834910247455" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632899400480156873" actid="632744834910247460" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632745578636398979" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632745578636398976" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632745578636398975" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632745578636398984" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632745578636398981" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632745578636398980" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632745578636398989" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632745578636398986" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632745578636398985" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632745750998260934" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632745750998260931" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632745750998260930" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632744002508073642" level="1" text="RequestAuthentication">
        <node type="function" name="RequestAuthentication" id="632744002508073639" path="Metreos.StockTools" />
        <calls>
          <ref actid="632743199524612757" />
          <ref actid="632745578636398419" />
          <ref actid="632744023639228148" />
          <ref actid="632744023639228153" />
          <ref actid="632744023639228186" />
          <ref actid="632744023639228190" />
        </calls>
      </treenode>
      <treenode type="fun" id="632744714582090194" level="1" text="FindSpeaker">
        <node type="function" name="FindSpeaker" id="632744714582090191" path="Metreos.StockTools" />
        <calls>
          <ref actid="632744714582090190" />
        </calls>
      </treenode>
      <treenode type="fun" id="632744714582090199" level="1" text="DoEnrollSpeaker">
        <node type="function" name="DoEnrollSpeaker" id="632744714582090196" path="Metreos.StockTools" />
        <calls>
          <ref actid="632744960684114726" />
          <ref actid="632744834910247476" />
          <ref actid="632744714582090195" />
        </calls>
      </treenode>
      <treenode type="fun" id="632744719383901091" level="1" text="Exit">
        <node type="function" name="Exit" id="632744719383901088" path="Metreos.StockTools" />
        <calls>
          <ref actid="632743199524612762" />
          <ref actid="632744023639228137" />
          <ref actid="632744023639228150" />
          <ref actid="632744023639228155" />
          <ref actid="632744023639228183" />
          <ref actid="632744023639228191" />
        </calls>
      </treenode>
      <treenode type="fun" id="632751616617656922" level="1" text="SetTrained">
        <node type="function" name="SetTrained" id="632751616617656919" path="Metreos.StockTools" />
        <calls>
          <ref actid="632751616617656918" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_originalTo" id="632899400480156663" vid="632739564151169693">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_callId" id="632899400480156665" vid="632739564151169695">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632899400480156667" vid="632739564151169697">
        <Properties type="String">g_mmsId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632899400480156669" vid="632739564151169699">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_isCallerAnswered" id="632899400480156671" vid="632739564151169705">
        <Properties type="Bool" defaultInitWith="false">g_isCallerAnswered</Properties>
      </treenode>
      <treenode text="g_fromNumber" id="632899400480156673" vid="632739564151169707">
        <Properties type="String">g_fromNumber</Properties>
      </treenode>
      <treenode text="g_loginFailureThreshold" id="632899400480156675" vid="632743199524613254">
        <Properties type="UInt" initWith="LoginFailureThresh">g_loginFailureThreshold</Properties>
      </treenode>
      <treenode text="g_loginFailures" id="632899400480156677" vid="632743248748878037">
        <Properties type="UInt" defaultInitWith="0">g_loginFailures</Properties>
      </treenode>
      <treenode text="g_operationFailureThreshold" id="632899400480156679" vid="632743248748878039">
        <Properties type="UInt" initWith="OperationFailureThresh">g_operationFailureThreshold</Properties>
      </treenode>
      <treenode text="g_operationFailures" id="632899400480156681" vid="632743248748878041">
        <Properties type="UInt" defaultInitWith="0">g_operationFailures</Properties>
      </treenode>
      <treenode text="AuthFailedAudio" id="632899400480156683" vid="632743248748878043">
        <Properties type="String" initWith="AuthFailedAudio">AuthFailedAudio</Properties>
      </treenode>
      <treenode text="RequestAuthAudio" id="632899400480156685" vid="632743248748878045">
        <Properties type="String" initWith="RequestAuthAudio">RequestAuthAudio</Properties>
      </treenode>
      <treenode text="GoodByeAudio" id="632899400480156687" vid="632743248748878047">
        <Properties type="String" initWith="GoodByeAudio">GoodByeAudio</Properties>
      </treenode>
      <treenode text="PoundSignAudio" id="632899400480156689" vid="632743248748878149">
        <Properties type="String" initWith="PoundSignAudio">PoundSignAudio</Properties>
      </treenode>
      <treenode text="PleaseSayAudio" id="632899400480156691" vid="632743248748878151">
        <Properties type="String" initWith="PleaseSayAudio">PleaseSayAudio</Properties>
      </treenode>
      <treenode text="SpeakerNotTrainedAudio" id="632899400480156693" vid="632743248748878153">
        <Properties type="String" initWith="SpeakerNotTrainedAudio">SpeakerNotTrainedAudio</Properties>
      </treenode>
      <treenode text="SpeakerTrainedAudio" id="632899400480156695" vid="632743248748878155">
        <Properties type="String" initWith="SpeakerTrainedAudio">SpeakerTrainedAudio</Properties>
      </treenode>
      <treenode text="TrainingPhraseAudio" id="632899400480156697" vid="632743248748878157">
        <Properties type="String" initWith="TrainingPhraseAudio">TrainingPhraseAudio</Properties>
      </treenode>
      <treenode text="g_DB_Name" id="632899400480156699" vid="632744023639228377">
        <Properties type="String" initWith="DatabaseName">g_DB_Name</Properties>
      </treenode>
      <treenode text="g_DB_Password" id="632899400480156701" vid="632744023639228379">
        <Properties type="String" initWith="Password">g_DB_Password</Properties>
      </treenode>
      <treenode text="g_DB_Username" id="632899400480156703" vid="632744023639228381">
        <Properties type="String" initWith="Username">g_DB_Username</Properties>
      </treenode>
      <treenode text="g_DB_Server" id="632899400480156705" vid="632744023639228383">
        <Properties type="String" initWith="Server">g_DB_Server</Properties>
      </treenode>
      <treenode text="g_DB_Port" id="632899400480156707" vid="632744023639228385">
        <Properties type="String" initWith="Port">g_DB_Port</Properties>
      </treenode>
      <treenode text="g_isTrained" id="632899400480156709" vid="632744834910247234">
        <Properties type="Bool" defaultInitWith="false">g_isTrained</Properties>
      </treenode>
      <treenode text="g_groupName" id="632899400480156711" vid="632744834910247236">
        <Properties type="String">g_groupName</Properties>
      </treenode>
      <treenode text="g_speakerName" id="632899400480156713" vid="632744834910247238">
        <Properties type="String">g_speakerName</Properties>
      </treenode>
      <treenode text="g_voiceSegmentFile" id="632899400480156715" vid="632744834910247467">
        <Properties type="String">g_voiceSegmentFile</Properties>
      </treenode>
      <treenode text="g_vocalPasswordServerUrl" id="632899400480156717" vid="632744960684114727">
        <Properties type="String" defaultInitWith="http://10.89.31.17/VocalPassword/VocalPasswordServer.asmx">g_vocalPasswordServerUrl</Properties>
      </treenode>
      <treenode text="g_subName" id="632899400480156719" vid="632744999415327113">
        <Properties type="String">g_subName</Properties>
      </treenode>
      <treenode text="g_displayName" id="632899400480156721" vid="632745578636398417">
        <Properties type="String" initWith="DisplayName">g_displayName</Properties>
      </treenode>
      <treenode text="g_audioSegmentsPath" id="632899400480156723" vid="632745750998260927">
        <Properties type="String" initWith="AudioSegmentsPath">g_audioSegmentsPath</Properties>
      </treenode>
      <treenode text="g_passcode" id="632899400480156725" vid="632751616617656932">
        <Properties type="String">g_passcode</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632739556158359906" treenode="632739556158359907" appnode="632739556158359904" handlerfor="632739556158359903">
    <node type="Start" id="632739556158359906" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="214">
      <linkto id="632739564151169687" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632739564151169686" text="(1) Verify destination number is valid.&#xD;&#xA;(2) Answer call&#xD;&#xA;(3) Verify user identification&#xD;&#xA;(4) Group and Speaker ID lookup, create Speaker if necessary&#xD;&#xA;(5) If speaker is trained then give caller option to hangup or re-train&#xD;&#xA;(6) Play training phrase and wait for user to repeat&#xD;&#xA;(7) Repeat (5) until speaker is trained&#xD;&#xA;(8) If voice segments are more than 3 but train failed then give user to remove existing voice segments" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="62" y="42" />
    <node type="Action" id="632739564151169687" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="119" y="214">
      <linkto id="632739564151169691" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632739564151169692" type="Labeled" style="Bezier" ortho="true" label="Success" />
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
    <node type="Action" id="632739564151169691" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="121" y="350">
      <linkto id="632739564151169703" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632739564151169692" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="244" y="214">
      <linkto id="632739564151169703" type="Labeled" style="Bezier" ortho="true" label="Fail" />
      <linkto id="632739564151169704" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="variable">g_displayName</ap>
        <ap name="WaitForMedia" type="csharp">true</ap>
        <ap name="Conference" type="literal">false</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632739564151169703" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="245" y="350">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632739564151169704" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="374" y="212">
      <linkto id="632743199524612757" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">fromNumber</ap>
        <rd field="ResultData">g_isCallerAnswered</rd>
        <rd field="ResultData2">g_fromNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632743199524612757" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="462.551758" y="199" mx="527" my="215">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632743199524612767" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632743199524612762" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
      </Properties>
    </node>
    <node type="Action" id="632743199524612762" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="599.8258" y="347" mx="637" my="363">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632743199524612767" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632743199524612767" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="753" y="215">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632739564151169688" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632739564151169689" name="originalTo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="OriginalTo" refType="reference" name="Metreos.CallControl.IncomingCall">originalTo</Properties>
    </node>
    <node type="Variable" id="632739564151169690" name="fromNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">fromNumber</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632744023639228110" treenode="632744023639228111" appnode="632744023639228108" handlerfor="632744023639228107">
    <node type="Start" id="632744023639228110" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="161">
      <linkto id="632744023639228145" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228145" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="136" y="161">
      <linkto id="632744023639228146" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228146" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="246" y="161">
      <linkto id="632744023639228147" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632744023639228148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632744023639228151" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228147" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="246" y="39">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228148" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="184.551758" y="284" mx="249" my="300">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632744023639228150" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632744023639228149" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228149" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="418" y="369">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228150" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="211.825851" y="432" mx="249" my="448">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228149" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228151" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="409" y="161">
      <linkto id="632744714582090190" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string digits, ref string g_passcode, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "OnGatherDigits_Complete: cleaning up received digits string.");

	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);

	g_passcode = digits;

	return IApp.VALUE_SUCCESS;	
}

</Properties>
    </node>
    <node type="Action" id="632744023639228153" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="479.551758" y="290" mx="544" my="306">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632744023639228156" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632744023639228155" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">auth_fail</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228155" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="508.819977" y="433" mx="546" my="449">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228156" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228156" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="705.994141" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744714582090190" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="504.942383" y="145" mx="543" my="161">
      <items count="1">
        <item text="FindSpeaker" />
      </items>
      <linkto id="632744714582090195" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632744023639228153" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="UserPasscode" type="variable">digits</ap>
        <ap name="FunctionName" type="literal">FindSpeaker</ap>
      </Properties>
    </node>
    <node type="Action" id="632744714582090195" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="658.569336" y="146" mx="708" my="162">
      <items count="1">
        <item text="DoEnrollSpeaker" />
      </items>
      <linkto id="632744023639228156" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FirstTry" type="literal">1</ap>
        <ap name="FunctionName" type="literal">DoEnrollSpeaker</ap>
      </Properties>
    </node>
    <node type="Variable" id="632744023639228161" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632744023639228162" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632744023639228163" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632744723103660018" treenode="632744723103660019" appnode="632744723103660016" handlerfor="632744723103660015">
    <node type="Start" id="632744723103660018" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632751616617656934" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632751616617656934" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" activetab="true" startnode="632744834910247453" treenode="632744834910247454" appnode="632744834910247451" handlerfor="632744834910247450">
    <node type="Start" id="632744834910247453" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="84">
      <linkto id="632745750998260929" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744834910247469" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="250" y="540">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744960684114724" name="EncodeAudioToBase64" class="MaxActionNode" group="" path="Metreos.Native.EnrollSpeaker" x="249" y="83">
      <linkto id="632744960684114726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632745750998260936" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AudioFilePath" type="variable">fileName</ap>
        <rd field="EncodedData">encodedData</rd>
      </Properties>
    </node>
    <node type="Action" id="632744960684114726" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="202.569336" y="219" mx="252" my="235">
      <items count="1">
        <item text="DoEnrollSpeaker" />
      </items>
      <linkto id="632744834910247469" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FirstTry" type="literal">0</ap>
        <ap name="FunctionName" type="literal">DoEnrollSpeaker</ap>
      </Properties>
    </node>
    <node type="Action" id="632744960684114736" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="625" y="524" mx="678" my="540">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744834910247469" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">SpeakerTrainedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">trained</ap>
      </Properties>
    </node>
    <node type="Action" id="632745750998260929" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="125" y="84">
      <linkto id="632744960684114724" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(string g_audioSegmentsPath, ref string fileName)
{
	fileName = System.IO.Path.Combine(g_audioSegmentsPath, fileName);
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632745750998260936" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="395" y="85">
      <linkto id="632899400480156937" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string encodedData, string g_subName, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "Size of encoded data is {0}", encodedData.Length);
	log.Write(TraceLevel.Verbose, "Speaker SubId {0}", g_subName);
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632745750998260937" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="672" y="84">
      <linkto id="632745750998260938" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(WebServices.NativeTypes.VocalPasswordServer.EnrollStatusType enrollResult, LogWriter log)
{
	if (enrollResult.Value == WebServices.Base.VocalPasswordServer.EnrollStatus.Full)
		log.Write(TraceLevel.Verbose, "Enroll Full");
      else if (enrollResult.Value == WebServices.Base.VocalPasswordServer.EnrollStatus.NotReady)
		log.Write(TraceLevel.Verbose, "Enroll Not Ready");
     	else if (enrollResult.Value == WebServices.Base.VocalPasswordServer.EnrollStatus.Ready)
		log.Write(TraceLevel.Verbose, "Enroll Ready");
      else if (enrollResult.Value == WebServices.Base.VocalPasswordServer.EnrollStatus.Trained)
		log.Write(TraceLevel.Verbose, "Enroll Trained");
      else if (enrollResult.Value == WebServices.Base.VocalPasswordServer.EnrollStatus.TrainFailed)
		log.Write(TraceLevel.Verbose, "Enroll Trained Failed");
      else
		log.Write(TraceLevel.Verbose, "Enroll Status Unknown");
	
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632745750998260938" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="673.373657" y="236">
      <linkto id="632744960684114726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632745750998260939" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">enrollResult.Value == WebServices.Base.VocalPasswordServer.EnrollStatus.Trained</ap>
      </Properties>
    </node>
    <node type="Action" id="632745750998260939" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="673" y="331">
      <linkto id="632751616617656918" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_isTrained</rd>
      </Properties>
    </node>
    <node type="Action" id="632751616617656918" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="639.825867" y="412" mx="677" my="428">
      <items count="1">
        <item text="SetTrained" />
      </items>
      <linkto id="632744960684114736" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">SetTrained</ap>
      </Properties>
    </node>
    <node type="Action" id="632899400480156937" name="Enroll" class="MaxActionNode" group="" path="WebServices.NativeActions.VocalPasswordServer" x="529.4707" y="84">
      <linkto id="632745750998260937" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Url" type="variable">g_vocalPasswordServerUrl</ap>
        <ap name="SpeakerId" type="variable">g_speakerName</ap>
        <ap name="SpeakerSubId" type="variable">g_subName</ap>
        <ap name="Audio" type="variable">encodedData</ap>
        <ap name="ConfigSetName" type="literal">Default</ap>
        <ap name="EnrollAttributes" type="csharp">null</ap>
        <rd field="RequestId">requestId</rd>
        <rd field="Result">enrollResult</rd>
      </Properties>
    </node>
    <node type="Variable" id="632744834910247477" name="fileName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" refType="reference" name="Metreos.MediaControl.Record_Complete">fileName</Properties>
    </node>
    <node type="Variable" id="632744960684114725" name="encodedData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">encodedData</Properties>
    </node>
    <node type="Variable" id="632744960684114729" name="enrollResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="WebServices.NativeTypes.VocalPasswordServer.EnrollStatusType" initWith="" defaultInitWith="WebServices.Base.VocalPasswordServer.EnrollStatus.TrainedFailed" refType="reference">enrollResult</Properties>
    </node>
    <node type="Variable" id="632744999415327115" name="requestId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="0" refType="reference">requestId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632744834910247458" treenode="632744834910247459" appnode="632744834910247456" handlerfor="632744834910247455">
    <node type="Start" id="632744834910247458" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="229">
      <linkto id="632744834910247471" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744834910247471" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="138" y="229">
      <linkto id="632744834910247472" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632744834910247476" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_isTrained</ap>
      </Properties>
    </node>
    <node type="Action" id="632744834910247472" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="259" y="104" mx="312" my="120">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744834910247475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">SpeakerTrainedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">trained</ap>
      </Properties>
    </node>
    <node type="Action" id="632744834910247475" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="233">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744834910247476" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="263.569336" y="322" mx="313" my="338">
      <items count="1">
        <item text="DoEnrollSpeaker" />
      </items>
      <linkto id="632744834910247475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">DoEnrollSpeaker</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestAuthentication" startnode="632744002508073641" treenode="632744002508073642" appnode="632744002508073639" handlerfor="632745750998260930">
    <node type="Start" id="632744002508073641" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="160">
      <linkto id="632744023639228082" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228082" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="156" y="160">
      <linkto id="632744023639228085" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632744023639228084" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">operationFailureCounter &lt; g_operationFailureThreshold</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228083" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="156" y="62">
      <linkto id="632744023639228082" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">operationFailureCounter + 1</ap>
        <rd field="ResultData">operationFailureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632744023639228084" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="156" y="262">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228085" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="291" y="160">
      <linkto id="632744023639228087" type="Labeled" style="Bezier" ortho="true" label="play" />
      <linkto id="632744023639228088" type="Labeled" style="Bezier" ortho="true" label="digits" />
      <linkto id="632744023639228089" type="Labeled" style="Bezier" ortho="true" label="auth_fail" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Label" id="632744023639228086" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="32" y="62.0000038">
      <linkto id="632744023639228083" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632744023639228087" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="343" y="60" />
    <node type="Label" id="632744023639228088" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="397" y="160" />
    <node type="Label" id="632744023639228089" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="346" y="279" />
    <node type="Label" id="632744023639228090" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="484" y="59">
      <linkto id="632744723103660020" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632744023639228104" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="697" y="59" />
    <node type="Label" id="632744023639228105" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="582" y="189" />
    <node type="Label" id="632744023639228106" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="487" y="283">
      <linkto id="632744023639228117" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228117" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="512" y="267" mx="586" my="283">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632744023639228111" />
        <item text="OnGatherDigits_Failed" treenode="632744023639228116" />
      </items>
      <linkto id="632744023639228120" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632744023639228121" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">auth_req</ap>
      </Properties>
    </node>
    <node type="Label" id="632744023639228120" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="706" y="283" />
    <node type="Label" id="632744023639228121" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="583" y="412" />
    <node type="Label" id="632744023639228122" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="791" y="59">
      <linkto id="632744023639228123" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228123" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="792" y="158">
      <linkto id="632744023639228124" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">operationFailureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632744023639228124" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="793" y="284">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Label" id="632744023639228125" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="34" y="467">
      <linkto id="632744023639228126" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228126" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="116" y="467">
      <linkto id="632744023639228127" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_loginFailures + 1</ap>
        <rd field="ResultData">g_loginFailures</rd>
      </Properties>
    </node>
    <node type="Action" id="632744023639228127" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="233" y="466">
      <linkto id="632744023639228128" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632744023639228131" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_loginFailures &lt; g_loginFailureThreshold</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228128" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="322" y="450.998535" mx="375" my="467">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744023639228135" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632744023639228134" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">RequestAuthAudio</ap>
        <ap name="Prompt3" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">AuthFailedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">auth_req</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228131" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="183" y="604" mx="236" my="620">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744023639228136" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632744023639228137" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">AuthFailedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632744023639228134" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="489" y="518" />
    <node type="Label" id="632744023639228135" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="488" y="410.996155" />
    <node type="Label" id="632744023639228136" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="367" y="587" />
    <node type="Action" id="632744023639228137" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="332.8258" y="645" mx="370" my="661">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228138" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228138" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="661">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660020" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="533.4707" y="43" mx="586" my="59">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744023639228104" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632744023639228105" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">RequestAuthAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">auth_req</ap>
      </Properties>
    </node>
    <node type="Variable" id="632744023639228139" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Action" refType="reference">action</Properties>
    </node>
    <node type="Variable" id="632744023639228140" name="operationFailureCounter" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" defaultInitWith="0" refType="reference">operationFailureCounter</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="FindSpeaker" startnode="632744714582090193" treenode="632744714582090194" appnode="632744714582090191" handlerfor="632745750998260930">
    <node type="Start" id="632744714582090193" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="72">
      <linkto id="632744723103660207" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744714582090201" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="553" y="424">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660207" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="147" y="71">
      <linkto id="632744723103660208" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632745578636398419" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632744723103660208" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="263" y="72">
      <linkto id="632744723103660210" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632745578636398419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">enrolled_speakers</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660209" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="553" y="72">
      <linkto id="632744723103660212" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632745578636398419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="variable">lookupQuery</ap>
        <ap name="Name" type="literal">enrolled_speakers</ap>
        <rd field="ResultSet">speakerLookupResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632744723103660210" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="403" y="72">
      <linkto id="632744723103660209" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string userPasscode, ref string lookupQuery)
{
	lookupQuery = "select es_enrolled_group_name, es_enrolled_speaker_name, es_enrolled_sub_name, es_trained from enrolled_speakers where es_enrolled_speaker_passcode = '" + userPasscode + "'";
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632744723103660212" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="553" y="175">
      <linkto id="632745578636398419" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632745578636398974" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
public static string Execute(DataTable speakerLookupResult, ref string g_groupName, ref string g_speakerName, ref string g_subName, ref string isTrained, LogWriter log)
{
	g_groupName = "";
	g_speakerName = "";

	if (speakerLookupResult.Rows.Count == 0)
		return "Failure";	

	log.Write(TraceLevel.Verbose, "Speaker lookup returns {0} row", speakerLookupResult.Rows.Count);

	if (speakerLookupResult.Rows[0]["es_enrolled_group_name"] != null &amp;&amp; 
	   speakerLookupResult.Rows[0]["es_enrolled_speaker_name"] != null &amp;&amp; 
         speakerLookupResult.Rows[0]["es_enrolled_sub_name"] != null &amp;&amp; 
         speakerLookupResult.Rows[0]["es_trained"] != null)
	{
		g_groupName = speakerLookupResult.Rows[0]["es_enrolled_group_name"] as string;
		g_speakerName = speakerLookupResult.Rows[0]["es_enrolled_speaker_name"] as string;
		g_subName = speakerLookupResult.Rows[0]["es_enrolled_sub_name"] as string;
		isTrained = speakerLookupResult.Rows[0]["es_trained"] as string;
		return "Success";
	}

	return "Failure";
}
</Properties>
    </node>
    <node type="Action" id="632745578636398419" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="201.551758" y="257" mx="266" my="273">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632745578636398972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">auth_fail</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
      </Properties>
    </node>
    <node type="Action" id="632745578636398972" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="264" y="414">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632745578636398974" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="554" y="294">
      <linkto id="632744714582090201" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">isTrained == "1"</ap>
        <rd field="ResultData">g_isTrained</rd>
      </Properties>
    </node>
    <node type="Variable" id="632744723103660206" name="userPasscode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserPasscode" refType="reference">userPasscode</Properties>
    </node>
    <node type="Variable" id="632744723103660213" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632744723103660214" name="lookupQuery" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">lookupQuery</Properties>
    </node>
    <node type="Variable" id="632744723103660215" name="speakerLookupResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">speakerLookupResult</Properties>
    </node>
    <node type="Variable" id="632745578636398973" name="isTrained" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">isTrained</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SetTrained" startnode="632751616617656921" treenode="632751616617656922" appnode="632751616617656919" handlerfor="632745750998260930">
    <node type="Start" id="632751616617656921" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="50" y="84">
      <linkto id="632751616617656923" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632751616617656923" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="175" y="84">
      <linkto id="632751616617656924" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632751616617656924" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="323" y="84">
      <linkto id="632751616617656925" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">enrolled_speakers</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632751616617656925" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="477" y="84">
      <linkto id="632751616617656926" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string g_passcode, ref string setValueQuery)
{
	setValueQuery = "update enrolled_speakers set es_trained = '1' where es_enrolled_speaker_passcode = '" + g_passcode + "'";

	return "Success";
}

</Properties>
    </node>
    <node type="Action" id="632751616617656926" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="477" y="196">
      <linkto id="632751616617656927" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="variable">setValueQuery</ap>
        <ap name="Name" type="literal">enrolled_speakers</ap>
      </Properties>
    </node>
    <node type="Action" id="632751616617656927" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="477" y="323">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632751616617656928" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632751616617656929" name="setValueQuery" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">setValueQuery</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632745578636398988" treenode="632745578636398989" appnode="632745578636398986" handlerfor="632745578636398985">
    <node type="Start" id="632745578636398988" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632745578636398992" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632745578636398992" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DoEnrollSpeaker" startnode="632744714582090198" treenode="632744714582090199" appnode="632744714582090196" handlerfor="632745750998260930">
    <node type="Start" id="632744714582090198" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="107">
      <linkto id="632744999415327117" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744723103660416" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="304" y="184" mx="357" my="200">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744723103660422" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">SpeakerTrainedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">trained</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660419" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="301" y="364" mx="354" my="380">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744723103660422" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">PleaseSayAudio</ap>
        <ap name="Prompt3" type="variable">TrainingPhraseAudio</ap>
        <ap name="Prompt1" type="variable">SpeakerNotTrainedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">train_phrase</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660422" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="543" y="293">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744834910247240" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="289">
      <linkto id="632744723103660416" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632744723103660419" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_isTrained</ap>
      </Properties>
    </node>
    <node type="Action" id="632744999415327117" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="107">
      <linkto id="632744999415327118" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632744834910247240" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">firstTry == "1"</ap>
      </Properties>
    </node>
    <node type="Action" id="632744999415327118" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="493" y="93" mx="546" my="109">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744723103660422" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">TrainingPhraseAudio</ap>
        <ap name="Prompt1" type="variable">PleaseSayAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">train_phrase</ap>
      </Properties>
    </node>
    <node type="Variable" id="632744999415327116" name="firstTry" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="FirstTry" refType="reference">firstTry</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632744023639228094" treenode="632744023639228095" appnode="632744023639228092" handlerfor="632744023639228091">
    <node type="Start" id="632744023639228094" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="113">
      <linkto id="632744023639228176" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228176" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="111" y="113">
      <linkto id="632744023639228178" type="Labeled" style="Bezier" ortho="true" label="auth_req" />
      <linkto id="632744023639228179" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <linkto id="632744023639228177" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632744834910247445" type="Labeled" style="Bezier" ortho="true" label="trained" />
      <linkto id="632744834910247446" type="Labeled" style="Bezier" ortho="true" label="train_phrase" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228177" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="111" y="219">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632744023639228178" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="216" y="32" />
    <node type="Label" id="632744023639228179" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="262" y="64" />
    <node type="Label" id="632744023639228180" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="399" y="111">
      <linkto id="632744023639228181" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228181" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="484" y="111">
      <linkto id="632744023639228183" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632744023639228182" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228182" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="190">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228183" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="584.8258" y="16" mx="622" my="32">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228182" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632744023639228184" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="116" y="282">
      <linkto id="632744023639228185" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228185" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="299" y="282">
      <linkto id="632744023639228186" type="Labeled" style="Bezier" label="eod" />
      <linkto id="632744023639228186" type="Labeled" style="Bezier" label="maxdigits" />
      <linkto id="632744023639228189" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632744023639228190" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228186" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="413.551758" y="268" mx="478" my="284">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632744023639228188" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632744023639228191" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">digits</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228188" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="648" y="284">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228189" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="204" y="402">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228190" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="335.551758" y="386" mx="400" my="402">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632744023639228189" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632744023639228191" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228191" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="526.8258" y="387" mx="564" my="403">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228188" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632744834910247445" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="261" y="127" />
    <node type="Label" id="632744834910247446" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="216" y="177" />
    <node type="Label" id="632744834910247447" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="119" y="495">
      <linkto id="632744834910247448" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744834910247448" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="241" y="495">
      <linkto id="632744834910247449" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632744834910247460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632744834910247449" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="277" y="620">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744834910247460" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="336" y="479" mx="396" my="495">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632744834910247454" />
        <item text="OnRecord_Failed" treenode="632744834910247459" />
      </items>
      <linkto id="632744834910247449" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="CommandTimeout" type="variable">maxRecordTime</ap>
        <ap name="Expires" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondSilence" type="variable">maxSilenceTime</ap>
        <ap name="AudioFileSampleRate" type="literal">8</ap>
        <ap name="AudioFileSampleSize" type="literal">16</ap>
        <ap name="AudioFileEncoding" type="literal">pcm</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="Filename">g_voiceSegmentFile</rd>
      </Properties>
    </node>
    <node type="Variable" id="632744023639228174" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632744023639228175" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632744834910247465" name="maxRecordTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" defaultInitWith="30000" refType="reference">maxRecordTime</Properties>
    </node>
    <node type="Variable" id="632744834910247466" name="maxSilenceTime" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" defaultInitWith="5000" refType="reference">maxSilenceTime</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <canvas type="Function" name="Exit" show="false" startnode="632744719383901090" treenode="632744719383901091" appnode="632744719383901088" handlerfor="632745750998260930">
    <node type="Start" id="632744719383901090" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="129">
      <linkto id="632744723103660203" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744723103660203" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="136" y="129">
      <linkto id="632744723103660204" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632744723103660205" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isCallerAnswered</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660204" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="286" y="240">
      <linkto id="632744723103660205" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660205" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443" y="130">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" show="false" startnode="632745578636398983" treenode="632745578636398984" appnode="632745578636398981" handlerfor="632745578636398980">
    <node type="Start" id="632745578636398983" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632745578636398991" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632745578636398991" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="134" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" show="false" startnode="632744023639228115" treenode="632744023639228116" appnode="632744023639228113" handlerfor="632744023639228112">
    <node type="Start" id="632744023639228115" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="120">
      <linkto id="632744023639228144" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228144" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="158" y="120">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" show="false" startnode="632745750998260933" treenode="632745750998260934" appnode="632745750998260931" handlerfor="632745750998260930">
    <node type="Start" id="632745750998260933" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632745750998260935" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632745750998260935" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="169" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" show="false" startnode="632745578636398978" treenode="632745578636398979" appnode="632745578636398976" handlerfor="632745578636398975">
    <node type="Start" id="632745578636398978" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632745578636398990" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632745578636398990" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>