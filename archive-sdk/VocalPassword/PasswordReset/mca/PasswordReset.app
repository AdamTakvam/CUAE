<Application name="PasswordReset" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="PasswordReset">
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
          <ref id="632899489952344313" actid="632748409117547747" />
          <ref id="632899489952344324" actid="632748409117547770" />
          <ref id="632899489952344343" actid="632748129023917158" />
          <ref id="632899489952344380" actid="632744023639228128" />
          <ref id="632899489952344383" actid="632744023639228131" />
          <ref id="632899489952344391" actid="632744723103660020" />
          <ref id="632899489952344406" actid="632748129023917154" />
          <ref id="632899489952344415" actid="632748129023916835" />
          <ref id="632899489952344430" actid="632748409117547773" />
          <ref id="632899489952344435" actid="632748409117548123" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744023639228111" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632744023639228108" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632744023639228107" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632899489952344369" actid="632744023639228117" />
          <ref id="632899489952344420" actid="632748409117547730" />
          <ref id="632899489952344425" actid="632748409117547755" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744023639228116" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632744023639228113" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632744023639228112" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632899489952344370" actid="632744023639228117" />
          <ref id="632899489952344421" actid="632748409117547730" />
          <ref id="632899489952344426" actid="632748409117547755" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744723103660019" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632744723103660016" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632744723103660015" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632899489952344314" actid="632748409117547747" />
          <ref id="632899489952344325" actid="632748409117547770" />
          <ref id="632899489952344344" actid="632748129023917158" />
          <ref id="632899489952344381" actid="632744023639228128" />
          <ref id="632899489952344384" actid="632744023639228131" />
          <ref id="632899489952344392" actid="632744723103660020" />
          <ref id="632899489952344407" actid="632748129023917154" />
          <ref id="632899489952344416" actid="632748129023916835" />
          <ref id="632899489952344431" actid="632748409117547773" />
          <ref id="632899489952344436" actid="632748409117548123" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744834910247454" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632744834910247451" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632744834910247450" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632899489952344275" actid="632744834910247460" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632744834910247459" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632744834910247456" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632744834910247455" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632899489952344276" actid="632744834910247460" />
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
          <ref actid="632744023639228186" />
          <ref actid="632744023639228190" />
          <ref actid="632744023639228148" />
          <ref actid="632744023639228153" />
        </calls>
      </treenode>
      <treenode type="fun" id="632744714582090194" level="1" text="FindSpeaker">
        <node type="function" name="FindSpeaker" id="632744714582090191" path="Metreos.StockTools" />
        <calls>
          <ref actid="632744714582090190" />
        </calls>
      </treenode>
      <treenode type="fun" id="632744719383901091" level="1" text="Exit">
        <node type="function" name="Exit" id="632744719383901088" path="Metreos.StockTools" />
        <calls>
          <ref actid="632743199524612762" />
          <ref actid="632744023639228183" />
          <ref actid="632744023639228191" />
          <ref actid="632744023639228150" />
          <ref actid="632744023639228155" />
          <ref actid="632744023639228137" />
        </calls>
      </treenode>
      <treenode type="fun" id="632748129023916834" level="1" text="DoVerifySpeaker">
        <node type="function" name="DoVerifySpeaker" id="632748129023916831" path="Metreos.StockTools" />
        <calls>
          <ref actid="632744714582090195" />
          <ref actid="632744960684114726" />
          <ref actid="632744834910247476" />
        </calls>
      </treenode>
      <treenode type="fun" id="632748409117547728" level="1" text="RequestPassword">
        <node type="function" name="RequestPassword" id="632748409117547725" path="Metreos.StockTools" />
        <calls>
          <ref actid="632748409117547724" />
          <ref actid="632748409117547753" />
        </calls>
      </treenode>
      <treenode type="fun" id="632748409117547769" level="1" text="ResetPassword">
        <node type="function" name="ResetPassword" id="632748409117547766" path="Metreos.StockTools" />
        <calls>
          <ref actid="632748409117547765" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_originalTo" id="632899489952344168" vid="632739564151169693">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_callId" id="632899489952344170" vid="632739564151169695">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632899489952344172" vid="632739564151169697">
        <Properties type="String">g_mmsId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632899489952344174" vid="632739564151169699">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_isCallerAnswered" id="632899489952344176" vid="632739564151169705">
        <Properties type="Bool" defaultInitWith="false">g_isCallerAnswered</Properties>
      </treenode>
      <treenode text="g_fromNumber" id="632899489952344178" vid="632739564151169707">
        <Properties type="String">g_fromNumber</Properties>
      </treenode>
      <treenode text="g_loginFailureThreshold" id="632899489952344180" vid="632743199524613254">
        <Properties type="UInt" initWith="LoginFailureThresh">g_loginFailureThreshold</Properties>
      </treenode>
      <treenode text="g_loginFailures" id="632899489952344182" vid="632743248748878037">
        <Properties type="UInt" defaultInitWith="0">g_loginFailures</Properties>
      </treenode>
      <treenode text="g_operationFailureThreshold" id="632899489952344184" vid="632743248748878039">
        <Properties type="UInt" initWith="OperationFailureThresh">g_operationFailureThreshold</Properties>
      </treenode>
      <treenode text="g_operationFailures" id="632899489952344186" vid="632743248748878041">
        <Properties type="UInt" defaultInitWith="0">g_operationFailures</Properties>
      </treenode>
      <treenode text="AuthFailedAudio" id="632899489952344188" vid="632743248748878043">
        <Properties type="String" initWith="AuthFailedAudio">AuthFailedAudio</Properties>
      </treenode>
      <treenode text="RequestAuthAudio" id="632899489952344190" vid="632743248748878045">
        <Properties type="String" initWith="RequestAuthAudio">RequestAuthAudio</Properties>
      </treenode>
      <treenode text="GoodByeAudio" id="632899489952344192" vid="632743248748878047">
        <Properties type="String" initWith="GoodByeAudio">GoodByeAudio</Properties>
      </treenode>
      <treenode text="PoundSignAudio" id="632899489952344194" vid="632743248748878149">
        <Properties type="String" initWith="PoundSignAudio">PoundSignAudio</Properties>
      </treenode>
      <treenode text="PleaseSayAudio" id="632899489952344196" vid="632743248748878151">
        <Properties type="String" initWith="PleaseSayAudio">PleaseSayAudio</Properties>
      </treenode>
      <treenode text="SpeakerNotTrainedAudio" id="632899489952344198" vid="632743248748878153">
        <Properties type="String" initWith="SpeakerNotTrainedAudio">SpeakerNotTrainedAudio</Properties>
      </treenode>
      <treenode text="g_DB_Name" id="632899489952344200" vid="632744023639228377">
        <Properties type="String" initWith="DatabaseName">g_DB_Name</Properties>
      </treenode>
      <treenode text="g_DB_Password" id="632899489952344202" vid="632744023639228379">
        <Properties type="String" initWith="Password">g_DB_Password</Properties>
      </treenode>
      <treenode text="g_DB_Username" id="632899489952344204" vid="632744023639228381">
        <Properties type="String" initWith="Username">g_DB_Username</Properties>
      </treenode>
      <treenode text="g_DB_Server" id="632899489952344206" vid="632744023639228383">
        <Properties type="String" initWith="Server">g_DB_Server</Properties>
      </treenode>
      <treenode text="g_DB_Port" id="632899489952344208" vid="632744023639228385">
        <Properties type="String" initWith="Port">g_DB_Port</Properties>
      </treenode>
      <treenode text="g_isTrained" id="632899489952344210" vid="632744834910247234">
        <Properties type="Bool" defaultInitWith="false">g_isTrained</Properties>
      </treenode>
      <treenode text="g_groupName" id="632899489952344212" vid="632744834910247236">
        <Properties type="String">g_groupName</Properties>
      </treenode>
      <treenode text="g_speakerName" id="632899489952344214" vid="632744834910247238">
        <Properties type="String">g_speakerName</Properties>
      </treenode>
      <treenode text="g_voiceSegmentFile" id="632899489952344216" vid="632744834910247467">
        <Properties type="String">g_voiceSegmentFile</Properties>
      </treenode>
      <treenode text="g_vocalPasswordServerUrl" id="632899489952344218" vid="632744960684114727">
        <Properties type="String" defaultInitWith="http://10.89.31.17/VocalPassword/VocalPasswordServer.asmx">g_vocalPasswordServerUrl</Properties>
      </treenode>
      <treenode text="g_subName" id="632899489952344220" vid="632744999415327113">
        <Properties type="String">g_subName</Properties>
      </treenode>
      <treenode text="g_displayName" id="632899489952344222" vid="632745578636398417">
        <Properties type="String" initWith="DisplayName">g_displayName</Properties>
      </treenode>
      <treenode text="g_audioSegmentsPath" id="632899489952344224" vid="632745750998260927">
        <Properties type="String" initWith="AudioSegmentsPath">g_audioSegmentsPath</Properties>
      </treenode>
      <treenode text="g_ntUserName" id="632899489952344226" vid="632748129023916853">
        <Properties type="String">g_ntUserName</Properties>
      </treenode>
      <treenode text="g_ntPassword" id="632899489952344228" vid="632748129023916855">
        <Properties type="String">g_ntPassword</Properties>
      </treenode>
      <treenode text="g_newPassword2" id="632899489952344230" vid="632748129023916857">
        <Properties type="String">g_newPassword2</Properties>
      </treenode>
      <treenode text="PasswordResetSuccessAudio" id="632899489952344232" vid="632748129023917149">
        <Properties type="String" initWith="PasswordResetSuccess">PasswordResetSuccessAudio</Properties>
      </treenode>
      <treenode text="PasswordResetFailAudio" id="632899489952344234" vid="632748129023917151">
        <Properties type="String" initWith="PasswordResetFail">PasswordResetFailAudio</Properties>
      </treenode>
      <treenode text="EnterNewPasswordAudio" id="632899489952344236" vid="632748409117547430">
        <Properties type="String" initWith="EnterNewPasswordAudio">EnterNewPasswordAudio</Properties>
      </treenode>
      <treenode text="AgainAudio" id="632899489952344238" vid="632748409117547432">
        <Properties type="String" initWith="AgainAudio">AgainAudio</Properties>
      </treenode>
      <treenode text="g_newPassword1" id="632899489952344240" vid="632748409117547744">
        <Properties type="String">g_newPassword1</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632739556158359906" treenode="632739556158359907" appnode="632739556158359904" handlerfor="632739556158359903">
    <node type="Start" id="632739556158359906" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="214">
      <linkto id="632739564151169687" type="Basic" style="Bezier" ortho="true" />
    </node>
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
    <node type="Action" id="632743199524612767" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="746" y="216">
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
  <canvas type="Function" name="OnPlay_Complete" startnode="632744023639228094" treenode="632744023639228095" appnode="632744023639228092" handlerfor="632744023639228091">
    <node type="Start" id="632744023639228094" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="113">
      <linkto id="632744023639228176" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228176" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="111" y="113">
      <linkto id="632744023639228178" type="Labeled" style="Bezier" ortho="true" label="auth_req" />
      <linkto id="632744023639228179" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <linkto id="632744023639228177" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632744834910247445" type="Labeled" style="Bezier" ortho="true" label="reset_done" />
      <linkto id="632744834910247446" type="Labeled" style="Bezier" ortho="true" label="request_voice" />
      <linkto id="632748129023918298" type="Labeled" style="Bezier" ortho="true" label="not_trained" />
      <linkto id="632748409117547721" type="Labeled" style="Bezier" ortho="true" label="request_new_password" />
      <linkto id="632748409117547750" type="Labeled" style="Bezier" ortho="true" label="password_again" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228177" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="111" y="219">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632744023639228178" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="216" y="32" />
    <node type="Label" id="632744023639228179" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="271" y="62.0000038" />
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
    <node type="Action" id="632744023639228182" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="157">
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
    <node type="Label" id="632744023639228184" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="124" y="363">
      <linkto id="632744023639228185" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228185" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="307" y="363">
      <linkto id="632744023639228186" type="Labeled" style="Bezier" label="eod" />
      <linkto id="632744023639228186" type="Labeled" style="Bezier" label="maxdigits" />
      <linkto id="632744023639228189" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632744023639228190" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228186" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="421.551758" y="349" mx="486" my="365">
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
    <node type="Action" id="632744023639228188" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="656" y="365">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228189" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="212" y="483">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228190" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="343.551758" y="467" mx="408" my="483">
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
    <node type="Action" id="632744023639228191" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="534.8258" y="468" mx="572" my="484">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228188" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632744834910247445" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="307" y="113" />
    <node type="Label" id="632744834910247446" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="300" y="168" />
    <node type="Label" id="632744834910247447" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="74" y="577">
      <linkto id="632744834910247448" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744834910247448" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="192" y="577">
      <linkto id="632744834910247449" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632744834910247460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632744834910247449" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="228" y="702">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744834910247460" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="287" y="561" mx="347" my="577">
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
        <ap name="AudioFileEncoding" type="literal">pcm</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="Filename">g_voiceSegmentFile</rd>
      </Properties>
    </node>
    <node type="Label" id="632748129023918298" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="250" y="250" />
    <node type="Label" id="632748409117547721" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="289" y="222" />
    <node type="Label" id="632748409117547722" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="471" y="578">
      <linkto id="632748409117547723" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748409117547723" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="545" y="578">
      <linkto id="632748409117547724" type="Labeled" style="Bezier" label="eod" />
      <linkto id="632748409117547724" type="Labeled" style="Bezier" label="maxdigits" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547724" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="612.305664" y="561" mx="665" my="577">
      <items count="1">
        <item text="RequestPassword" />
      </items>
      <linkto id="632748409117547729" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FirstTry" type="literal">1</ap>
        <ap name="FunctionName" type="literal">RequestPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547729" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="662" y="695">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632748409117547750" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="193" y="286" />
    <node type="Label" id="632748409117547751" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="400" y="253">
      <linkto id="632748409117547752" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748409117547752" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="482" y="253">
      <linkto id="632748409117547753" type="Labeled" style="Bezier" label="eod" />
      <linkto id="632748409117547753" type="Labeled" style="Bezier" label="maxdigits" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547753" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="573.305664" y="236" mx="626" my="252">
      <items count="1">
        <item text="RequestPassword" />
      </items>
      <linkto id="632748409117547754" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FirstTry" type="literal">0</ap>
        <ap name="FunctionName" type="literal">RequestPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547754" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="741" y="252">
      <Properties final="true" type="appControl" log="On">
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
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632744023639228110" treenode="632744023639228111" appnode="632744023639228108" handlerfor="632744023639228107">
    <node type="Start" id="632744023639228110" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="161">
      <linkto id="632744023639228145" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744023639228145" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="136" y="161">
      <linkto id="632744023639228146" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632748409117547737" type="Labeled" style="Bezier" ortho="true" label="firstPassword" />
      <linkto id="632748409117547758" type="Labeled" style="Bezier" ortho="true" label="secondPassword" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228146" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="298" y="161">
      <linkto id="632744023639228147" type="Labeled" style="Bezier" ortho="true" label="userstop" />
      <linkto id="632744023639228148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632744023639228151" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228147" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="298" y="39">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228148" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="236.553314" y="284" mx="301" my="300">
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
    <node type="Action" id="632744023639228149" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="470" y="369">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744023639228150" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="263.82605" y="432" mx="301" my="448">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228149" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228151" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="461" y="160">
      <linkto id="632744714582090190" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632744023639228153" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="531.551758" y="289" mx="596" my="305">
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
    <node type="Action" id="632744023639228155" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="560.819946" y="433" mx="598" my="449">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632744023639228156" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632744023639228156" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="757.994141" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744714582090190" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="556.9424" y="145" mx="595" my="161">
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
    <node type="Action" id="632744714582090195" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="710.6074" y="146" mx="760" my="162">
      <items count="1">
        <item text="DoVerifySpeaker" />
      </items>
      <linkto id="632744023639228156" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">DoVerifySpeaker</ap>
      </Properties>
    </node>
    <node type="Label" id="632748409117547737" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="136" y="269" />
    <node type="Label" id="632748409117547738" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="46" y="558">
      <linkto id="632748409117547739" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748409117547739" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="165" y="558">
      <linkto id="632748409117547741" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632748409117547740" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547740" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="291" y="556.75">
      <linkto id="632748409117547743" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string digits, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "OnGatherDigits_Complete: cleaning up received digits string.");

	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);
	return IApp.VALUE_SUCCESS;	
}


</Properties>
    </node>
    <node type="Action" id="632748409117547741" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="165" y="657">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632748409117547743" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="558">
      <linkto id="632748409117547747" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_newPassword1</rd>
      </Properties>
    </node>
    <node type="Action" id="632748409117547746" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="701" y="559">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632748409117547747" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="502" y="542" mx="555" my="558">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632748409117547746" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">PoundSignAudio</ap>
        <ap name="Prompt3" type="variable">AgainAudio</ap>
        <ap name="Prompt1" type="variable">EnterNewPasswordAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">password_again</ap>
      </Properties>
    </node>
    <node type="Label" id="632748409117547758" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="135" y="47" />
    <node type="Label" id="632748409117547759" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="40" y="780">
      <linkto id="632748409117547760" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748409117547760" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="155" y="780">
      <linkto id="632748409117547762" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632748409117547761" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547761" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="155" y="887.5">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632748409117547762" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="281.25" y="780">
      <linkto id="632748409117547763" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string digits, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "OnGatherDigits_Complete: cleaning up received digits string.");

	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);
	return IApp.VALUE_SUCCESS;	
}



</Properties>
    </node>
    <node type="Action" id="632748409117547763" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="406.25" y="780">
      <linkto id="632748409117547764" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_newPassword2</rd>
      </Properties>
    </node>
    <node type="Action" id="632748409117547764" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="547.5" y="781.25">
      <linkto id="632748409117547765" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632748409117547770" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_newPassword1</ap>
        <ap name="Value2" type="variable">g_newPassword2</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547765" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="692.2295" y="765" mx="738" my="781">
      <items count="1">
        <item text="ResetPassword" />
      </items>
      <linkto id="632748409117547777" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">ResetPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547770" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="497.5" y="885" mx="550" my="901">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632748409117547777" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">PasswordResetFailAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">1</ap>
        <ap name="UserData" type="literal">reset_done</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547777" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="735" y="901.75">
      <Properties final="true" type="appControl" log="On">
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
      <linkto id="632751772461563229" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632751772461563229" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="159" y="32">
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
    <node type="Action" id="632744960684114726" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="202.607422" y="224" mx="252" my="240">
      <items count="1">
        <item text="DoVerifySpeaker" />
      </items>
      <linkto id="632744834910247469" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">DoVerifySpeaker</ap>
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
    <node type="Action" id="632745750998260936" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="395" y="84">
      <linkto id="632899489952344484" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string encodedData, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "Size of encoded data is {0}", encodedData.Length);
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632745750998260937" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="672" y="84">
      <linkto id="632745750998260938" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(WebServices.NativeTypes.VocalPasswordServer.ResultType verifyResult, LogWriter log)
{
	if (verifyResult.Value.Decision == WebServices.Base.VocalPasswordServer.Decision.Match)
		log.Write(TraceLevel.Verbose, "Voice print matched");
      else if (verifyResult.Value.Decision == WebServices.Base.VocalPasswordServer.Decision.Mismatch)
		log.Write(TraceLevel.Verbose, "Voice print not match");
     	else if (verifyResult.Value.Decision == WebServices.Base.VocalPasswordServer.Decision.Inconclusive)
		log.Write(TraceLevel.Verbose, "Voice print inconclusive");
      else
		log.Write(TraceLevel.Verbose, "Verify result Unknown");
	
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632745750998260938" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="672.373657" y="239">
      <linkto id="632744960684114726" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632748129023917158" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">verifyResult.Value.Decision == WebServices.Base.VocalPasswordServer.Decision.Match</ap>
      </Properties>
    </node>
    <node type="Action" id="632748129023917158" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="624" y="358" mx="677" my="374">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632748129023917161" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">EnterNewPasswordAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">request_new_password</ap>
      </Properties>
    </node>
    <node type="Action" id="632748129023917161" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="673" y="548">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632899489952344484" name="Verify" class="MaxActionNode" group="" path="WebServices.NativeActions.VocalPasswordServer" x="520.4707" y="84">
      <linkto id="632745750998260937" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Url" type="variable">g_vocalPasswordServerUrl</ap>
        <ap name="SpeakerId" type="variable">g_speakerName</ap>
        <ap name="SpeakerSubId" type="variable">g_subName</ap>
        <ap name="Audio" type="variable">encodedData</ap>
        <ap name="ConfigSetName" type="literal">Default</ap>
        <rd field="RequestId">requestId</rd>
        <rd field="Result">verifyResult</rd>
      </Properties>
    </node>
    <node type="Variable" id="632744834910247477" name="fileName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" refType="reference" name="Metreos.MediaControl.Record_Complete">fileName</Properties>
    </node>
    <node type="Variable" id="632744960684114725" name="encodedData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">encodedData</Properties>
    </node>
    <node type="Variable" id="632744960684114729" name="verifyResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="WebServices.NativeTypes.VocalPasswordServer.ResultType" initWith="" refType="reference">verifyResult</Properties>
    </node>
    <node type="Variable" id="632744999415327115" name="requestId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" defaultInitWith="0" refType="reference">requestId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632744834910247458" treenode="632744834910247459" appnode="632744834910247456" handlerfor="632744834910247455">
    <node type="Start" id="632744834910247458" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="229">
      <linkto id="632744834910247476" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632744834910247475" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="233">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632744834910247476" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="251.607422" y="217" mx="301" my="233">
      <items count="1">
        <item text="DoVerifySpeaker" />
      </items>
      <linkto id="632744834910247475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">DoVerifySpeaker</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632745750998260933" treenode="632745750998260934" appnode="632745750998260931" handlerfor="632745750998260930">
    <node type="Start" id="632745750998260933" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632745750998260935" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632745750998260935" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="169" y="32">
      <Properties final="true" type="appControl" log="On">
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
    <node type="Action" id="632744023639228127" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="232" y="467">
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
    <node type="Action" id="632744023639228131" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="182" y="603" mx="235" my="619">
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
    <node type="Action" id="632744714582090201" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="553" y="594">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660207" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="147" y="72">
      <linkto id="632744723103660208" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632745578636398972" type="Labeled" style="Bezier" ortho="true" label="default" />
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
      <linkto id="632745578636398972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">enrolled_speakers</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632744723103660209" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="553" y="72">
      <linkto id="632744723103660212" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632745578636398972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="variable">lookupQuery</ap>
        <ap name="Name" type="literal">enrolled_speakers</ap>
        <rd field="ResultSet">speakerLookupResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632744723103660210" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="405" y="72">
      <linkto id="632744723103660209" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string userPasscode, ref string lookupQuery)
{
	lookupQuery = "select es_enrolled_group_name, es_enrolled_speaker_name, es_enrolled_sub_name, es_trained, es_user_name, es_password from enrolled_speakers where es_enrolled_speaker_passcode = '" + userPasscode + "'";
	return "Success";
}
</Properties>
    </node>
    <node type="Action" id="632744723103660212" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="553" y="187">
      <linkto id="632745578636398974" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632745578636398972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(DataTable speakerLookupResult, ref string g_groupName, ref string g_speakerName, ref string g_subName, ref string isTrained, ref string g_ntUserName, ref string g_ntPassword, LogWriter log)
{
	g_groupName = "";
	g_speakerName = "";
	g_subName = "";
	g_ntUserName = "";
	g_ntPassword = "";

	if (speakerLookupResult.Rows.Count == 0)
		return "Failure";	

	log.Write(TraceLevel.Verbose, "Speaker lookup returns {0} row", speakerLookupResult.Rows.Count);

	if (speakerLookupResult.Rows[0]["es_enrolled_group_name"] != null &amp;&amp; 
	   speakerLookupResult.Rows[0]["es_enrolled_speaker_name"] != null &amp;&amp; 
         speakerLookupResult.Rows[0]["es_enrolled_sub_name"] != null &amp;&amp; 
         speakerLookupResult.Rows[0]["es_trained"] != null &amp;&amp;
	   speakerLookupResult.Rows[0]["es_user_name"] != null &amp;&amp;
	   speakerLookupResult.Rows[0]["es_password"] != null)
	{
		g_groupName = speakerLookupResult.Rows[0]["es_enrolled_group_name"] as string;
		g_speakerName = speakerLookupResult.Rows[0]["es_enrolled_speaker_name"] as string;
		g_subName = speakerLookupResult.Rows[0]["es_enrolled_sub_name"] as string;
		isTrained = speakerLookupResult.Rows[0]["es_trained"] as string;
		g_ntUserName = speakerLookupResult.Rows[0]["es_user_name"] as string;
		g_ntPassword = speakerLookupResult.Rows[0]["es_password"] as string;

	log.Write(TraceLevel.Verbose, "Is trained flag is {0}", isTrained);


		return "Success";
	}

	return "Failure";
}
</Properties>
    </node>
    <node type="Action" id="632745578636398972" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="100" y="415">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632745578636398974" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="554" y="294">
      <linkto id="632748129023917153" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">isTrained == "1"</ap>
        <rd field="ResultData">g_isTrained</rd>
      </Properties>
    </node>
    <node type="Action" id="632748129023917153" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="553" y="420">
      <linkto id="632744714582090201" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632748129023917154" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_isTrained == true</ap>
      </Properties>
    </node>
    <node type="Action" id="632748129023917154" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="376" y="404" mx="429" my="420">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632744714582090201" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">SpeakerNotTrainedAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">not_trained</ap>
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
  <canvas type="Function" name="DoVerifySpeaker" startnode="632748129023916833" treenode="632748129023916834" appnode="632748129023916831" handlerfor="632745750998260930">
    <node type="Start" id="632748129023916833" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="230">
      <linkto id="632748129023916835" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748129023916835" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="188" y="215" mx="241" my="231">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632748129023916837" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">PleaseSayAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">request_voice</ap>
      </Properties>
    </node>
    <node type="Action" id="632748129023916837" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="447" y="231">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestPassword" startnode="632748409117547727" treenode="632748409117547728" appnode="632748409117547725" handlerfor="632745750998260930">
    <node type="Start" id="632748409117547727" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="48" y="97">
      <linkto id="632748409117547734" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748409117547730" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="335" y="79" mx="409" my="95">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632744023639228111" />
        <item text="OnGatherDigits_Failed" treenode="632744023639228116" />
      </items>
      <linkto id="632748409117547735" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">firstPassword</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547734" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="196" y="97">
      <linkto id="632748409117547730" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632748409117547755" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">firstTry == "1"</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547735" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="634" y="97">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632748409117547755" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="125" y="288" mx="199" my="304">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632744023639228111" />
        <item text="OnGatherDigits_Failed" treenode="632744023639228116" />
      </items>
      <linkto id="632748409117547735" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">secondPassword</ap>
      </Properties>
    </node>
    <node type="Variable" id="632748409117547733" name="firstTry" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="FirstTry" refType="reference">firstTry</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ResetPassword" startnode="632748409117547768" treenode="632748409117547769" appnode="632748409117547766" handlerfor="632745750998260930">
    <node type="Start" id="632748409117547768" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="114">
      <linkto id="632748409117548122" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632748409117547773" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="271" y="16" mx="324" my="32">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632748409117547776" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">PasswordResetSuccessAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">1</ap>
        <ap name="UserData" type="literal">reset_done</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117547776" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="478" y="116">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632748409117548122" name="SetPassword" class="MaxActionNode" group="" path="Metreos.SDK.PasswordReset" x="176" y="114">
      <linkto id="632748409117547773" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632748409117548123" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">g_ntUserName</ap>
        <ap name="NewPassword" type="variable">g_newPassword1</ap>
      </Properties>
    </node>
    <node type="Action" id="632748409117548123" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="276" y="159" mx="329" my="175">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632744023639228095" />
        <item text="OnPlay_Failed" treenode="632744723103660019" />
      </items>
      <linkto id="632748409117547776" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">PasswordResetFailAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">reset_done</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Exit" startnode="632744719383901090" treenode="632744719383901091" appnode="632744719383901088" handlerfor="632745750998260930">
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
  <canvas type="Function" name="OnGotDigits" startnode="632745578636398988" treenode="632745578636398989" appnode="632745578636398986" handlerfor="632745578636398985">
    <node type="Start" id="632745578636398988" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632745578636398992" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632745578636398992" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
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