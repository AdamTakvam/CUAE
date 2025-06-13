<Application name="Administrator" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Administrator">
    <outline>
      <treenode type="evh" id="632527335560196792" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632527335560196789" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632527335560196788" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560196867" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632527335560196864" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632527335560196863" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632735680773453013" actid="632527335560196873" />
          <ref id="632735680773453076" actid="632528071621735656" />
          <ref id="632735680773453085" actid="632528071621735677" />
          <ref id="632735680773453094" actid="632528130761068027" />
          <ref id="632735680773453099" actid="632528130761068041" />
          <ref id="632735680773453102" actid="632528130761068044" />
          <ref id="632735680773453107" actid="632538493910671176" />
          <ref id="632735680773453120" actid="632538493910671444" />
          <ref id="632735680773453123" actid="632538493910671445" />
          <ref id="632735680773453131" actid="632549474022185571" />
          <ref id="632735680773453136" actid="632555905720470068" />
          <ref id="632735680773453144" actid="632555905720470080" />
          <ref id="632735680773453155" actid="632555905720470743" />
          <ref id="632735680773453198" actid="632611348412560566" />
          <ref id="632735680773453201" actid="632611348412560567" />
          <ref id="632735680773453224" actid="632549551328160420" />
          <ref id="632735680773453229" actid="632593879973705376" />
          <ref id="632735680773453232" actid="632593879973705379" />
          <ref id="632735680773453284" actid="632548695475214917" />
          <ref id="632735680773453288" actid="632550346808447331" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560196872" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632527335560196869" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632527335560196868" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632735680773453014" actid="632527335560196873" />
          <ref id="632735680773453077" actid="632528071621735656" />
          <ref id="632735680773453086" actid="632528071621735677" />
          <ref id="632735680773453095" actid="632528130761068027" />
          <ref id="632735680773453100" actid="632528130761068041" />
          <ref id="632735680773453103" actid="632528130761068044" />
          <ref id="632735680773453108" actid="632538493910671176" />
          <ref id="632735680773453121" actid="632538493910671444" />
          <ref id="632735680773453124" actid="632538493910671445" />
          <ref id="632735680773453132" actid="632549474022185571" />
          <ref id="632735680773453137" actid="632555905720470068" />
          <ref id="632735680773453145" actid="632555905720470080" />
          <ref id="632735680773453156" actid="632555905720470743" />
          <ref id="632735680773453199" actid="632611348412560566" />
          <ref id="632735680773453202" actid="632611348412560567" />
          <ref id="632735680773453225" actid="632549551328160420" />
          <ref id="632735680773453230" actid="632593879973705376" />
          <ref id="632735680773453233" actid="632593879973705379" />
          <ref id="632735680773453285" actid="632548695475214917" />
          <ref id="632735680773453289" actid="632550346808447331" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560197530" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632527335560197527" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632527335560197526" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632735680773453035" actid="632527335560197536" />
          <ref id="632735680773453045" actid="632528071621735662" />
          <ref id="632735680773453051" actid="632528071621735685" />
          <ref id="632735680773453057" actid="632538493910671414" />
          <ref id="632735680773453189" actid="632593879973704967" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560197535" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632527335560197532" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632527335560197531" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632735680773453036" actid="632527335560197536" />
          <ref id="632735680773453046" actid="632528071621735662" />
          <ref id="632735680773453052" actid="632528071621735685" />
          <ref id="632735680773453058" actid="632538493910671414" />
          <ref id="632735680773453190" actid="632593879973704967" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560196854" level="2" text="Metreos.Events.RemoteAgent.RequestConferenceResponse: OnRequestConferenceResponse">
        <node type="function" name="OnRequestConferenceResponse" id="632527335560196851" path="Metreos.StockTools" />
        <node type="event" name="RequestConferenceResponse" id="632527335560196850" path="Metreos.Events.RemoteAgent.RequestConferenceResponse" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632527335560196859" level="2" text="Metreos.Events.RemoteAgent.RecordingEventResponse: OnRecordingEventResponse">
        <node type="function" name="OnRecordingEventResponse" id="632527335560196856" path="Metreos.StockTools" />
        <node type="event" name="RecordingEventResponse" id="632527335560196855" path="Metreos.Events.RemoteAgent.RecordingEventResponse" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632538425211820328" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632538425211820325" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632538425211820324" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632735680773453262" actid="632538493910671462" />
          <ref id="632735680773453273" actid="632542558789590852" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632550392566729311" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632550392566729308" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632550392566729307" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555905720469428" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632555905720469425" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632555905720469424" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555905720469433" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632555905720469430" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632555905720469429" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632593879973704241" level="2" text="Metreos.CallControl.GotDigits: OnGotDigits">
        <node type="function" name="OnGotDigits" id="632593879973704238" path="Metreos.StockTools" />
        <node type="event" name="GotDigits" id="632593879973704237" path="Metreos.CallControl.GotDigits" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632593879973704247" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632593879973704244" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632593879973704243" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632538493910671430" level="1" text="MonitorAgent">
        <node type="function" name="MonitorAgent" id="632538493910671427" path="Metreos.StockTools" />
        <calls>
          <ref actid="632538493910671426" />
        </calls>
      </treenode>
      <treenode type="fun" id="632538493910671435" level="1" text="RecordAgent">
        <node type="function" name="RecordAgent" id="632538493910671432" path="Metreos.StockTools" />
        <calls>
          <ref actid="632538493910671431" />
        </calls>
      </treenode>
      <treenode type="fun" id="632538493910671440" level="1" text="NewExtension">
        <node type="function" name="NewExtension" id="632538493910671437" path="Metreos.StockTools" />
        <calls>
          <ref actid="632538493910671436" />
          <ref actid="632594454533619403" />
          <ref actid="632550346808447329" />
          <ref actid="632550346808447328" />
        </calls>
      </treenode>
      <treenode type="fun" id="632555905720470075" level="1" text="Exit">
        <node type="function" name="Exit" id="632555905720470072" path="Metreos.StockTools" />
        <calls>
          <ref actid="632555905720470739" />
          <ref actid="632555905720470760" />
          <ref actid="632555905720470071" />
          <ref actid="632555905720470083" />
          <ref actid="632555905720470084" />
          <ref actid="632555905720470089" />
          <ref actid="632555905720470746" />
          <ref actid="632555905720470757" />
          <ref actid="632555905720470761" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="db_ConnectionName" id="632735680773452931" vid="632527335560197494">
        <Properties type="String" initWith="DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="db_Name" id="632735680773452933" vid="632527335560197496">
        <Properties type="String" initWith="DbName">db_Name</Properties>
      </treenode>
      <treenode text="db_Server" id="632735680773452935" vid="632527335560197498">
        <Properties type="String" initWith="DbServer">db_Server</Properties>
      </treenode>
      <treenode text="db_Port" id="632735680773452937" vid="632527335560197500">
        <Properties type="UInt" initWith="DbPort">db_Port</Properties>
      </treenode>
      <treenode text="db_Username" id="632735680773452939" vid="632527335560197502">
        <Properties type="String" initWith="DbUsername">db_Username</Properties>
      </treenode>
      <treenode text="db_Password" id="632735680773452941" vid="632527335560197504">
        <Properties type="String" initWith="DbPassword">db_Password</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632735680773452943" vid="632527335560197491">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_agentRoutingGuid" id="632735680773452945" vid="632528130761068038">
        <Properties type="String">g_agentRoutingGuid</Properties>
      </treenode>
      <treenode text="g_callId" id="632735680773452947" vid="632527335560196924">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632735680773452949" vid="632527335560196926">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632735680773452951" vid="632527335560196928">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_to" id="632735680773452953" vid="632527335560197517">
        <Properties type="String" defaultInitWith="UNAVAILABLE">g_to</Properties>
      </treenode>
      <treenode text="g_from" id="632735680773452955" vid="632527335560197519">
        <Properties type="String" defaultInitWith="UNAVAILABLE">g_from</Properties>
      </treenode>
      <treenode text="g_callRecordId" id="632735680773452957" vid="632527335560197506">
        <Properties type="UInt" defaultInitWith="0">g_callRecordId</Properties>
      </treenode>
      <treenode text="g_userId" id="632735680773452959" vid="632528071621735692">
        <Properties type="UInt">g_userId</Properties>
      </treenode>
      <treenode text="g_agentDN" id="632735680773452961" vid="632538493910671421">
        <Properties type="String" defaultInitWith="NONE">g_agentDN</Properties>
      </treenode>
      <treenode text="g_accountNumber" id="632735680773452963" vid="632528071621735674">
        <Properties type="String" defaultInitWith="NONE">g_accountNumber</Properties>
      </treenode>
      <treenode text="g_adminLevelValue" id="632735680773452965" vid="632528130761068025">
        <Properties type="UInt" defaultInitWith="5">g_adminLevelValue</Properties>
      </treenode>
      <treenode text="g_outstandingEventsList" id="632735680773452967" vid="632538425211819376">
        <Properties type="ArrayList">g_outstandingEventsList</Properties>
      </treenode>
      <treenode text="g_timerFireDelay" id="632735680773452969" vid="632538425211820333">
        <Properties type="UInt" defaultInitWith="5">g_timerFireDelay</Properties>
      </treenode>
      <treenode text="g_isRecorded" id="632735680773452971" vid="632538425211820337">
        <Properties type="Bool" defaultInitWith="false">g_isRecorded</Properties>
      </treenode>
      <treenode text="ui_monitorKey" id="632735680773452973" vid="632528130761068049">
        <Properties type="String" initWith="MonitorKey">ui_monitorKey</Properties>
      </treenode>
      <treenode text="ui_recordingKey" id="632735680773452975" vid="632528130761068051">
        <Properties type="String" initWith="RecordKey">ui_recordingKey</Properties>
      </treenode>
      <treenode text="ui_selectAnotherExtKey" id="632735680773452977" vid="632528130761068053">
        <Properties type="String" initWith="AnotherExtKey">ui_selectAnotherExtKey</Properties>
      </treenode>
      <treenode text="ui_mainMenuKey" id="632735680773452979" vid="632604822630455060">
        <Properties type="String" initWith="MainMenuKey">ui_mainMenuKey</Properties>
      </treenode>
      <treenode text="g_poundAudio" id="632735680773452981" vid="632527335560196915">
        <Properties type="String" initWith="PressPoundAudio">g_poundAudio</Properties>
      </treenode>
      <treenode text="g_acctNumAudio" id="632735680773452983" vid="632527335560196917">
        <Properties type="String" initWith="EnterAccountNumAudio">g_acctNumAudio</Properties>
      </treenode>
      <treenode text="g_pinNumAudio" id="632735680773452985" vid="632527335560196919">
        <Properties type="String" initWith="EnterPINAudio">g_pinNumAudio</Properties>
      </treenode>
      <treenode text="g_insufficientLevelAudio" id="632735680773452987" vid="632528130761068035">
        <Properties type="String" initWith="InsufficientLevelAudio">g_insufficientLevelAudio</Properties>
      </treenode>
      <treenode text="g_extensionAudio" id="632735680773452989" vid="632528071621735680">
        <Properties type="String" initWith="ExtensionToMonitorAudio">g_extensionAudio</Properties>
      </treenode>
      <treenode text="g_instructionsAudio" id="632735680773452991" vid="632604822630455062">
        <Properties type="String" initWith="InstructionsAudio">g_instructionsAudio</Properties>
      </treenode>
      <treenode text="g_mainMenuRecordingAudio" id="632735680773452993" vid="632528130761068055">
        <Properties type="String" initWith="MainMenuRecordingAudio">g_mainMenuRecordingAudio</Properties>
      </treenode>
      <treenode text="g_callBeingRecordedAudio" id="632735680773452995" vid="632538493910671180">
        <Properties type="String" initWith="CallBeingRecordedAudio">g_callBeingRecordedAudio</Properties>
      </treenode>
      <treenode text="g_unrecognizedInputAudio" id="632735680773452997" vid="632538493910671442">
        <Properties type="String" initWith="UnrecognizedInputAudio">g_unrecognizedInputAudio</Properties>
      </treenode>
      <treenode text="g_mainMenuNotRecordingAudio" id="632735680773452999" vid="632538493910671457">
        <Properties type="String" initWith="MainMenuNotRecordingAudio">g_mainMenuNotRecordingAudio</Properties>
      </treenode>
      <treenode text="g_noActiveCallAudio" id="632735680773453001" vid="632545333827685011">
        <Properties type="String" initWith="NoActiveCallAudio">g_noActiveCallAudio</Properties>
      </treenode>
      <treenode text="g_loginFailedAudio" id="632735680773453003" vid="632549474022185574">
        <Properties type="String" initWith="LoginFailedAudio">g_loginFailedAudio</Properties>
      </treenode>
      <treenode text="g_recordingEnabledAudio" id="632735680773453005" vid="632549551328160729">
        <Properties type="String" initWith="RecordingEnabledAudio">g_recordingEnabledAudio</Properties>
      </treenode>
      <treenode text="g_recordingDisabledAudio" id="632735680773453007" vid="632549551328160731">
        <Properties type="String" initWith="RecordingDisabledAudio">g_recordingDisabledAudio</Properties>
      </treenode>
      <treenode text="g_callEndedAudio" id="632735680773453009" vid="632549697335393899">
        <Properties type="String" initWith="CallEndedAudio">g_callEndedAudio</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632527335560196791" treenode="632527335560196792" appnode="632527335560196789" handlerfor="632527335560196788">
    <node type="Start" id="632527335560196791" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="228">
      <linkto id="632527335560197487" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560196849" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="908" y="226">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632527335560196873" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="566" y="212" mx="619" my="228">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470751" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_acctNumAudio</ap>
        <ap name="UserData" type="literal">account_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632527335560196923" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="404" y="228">
      <linkto id="632527335560196873" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="literal">Remote Agent Admin</ap>
        <ap name="Conference" type="literal">false</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
        <log condition="default" on="true" level="Warning" type="csharp">"OnIncomingCall: Failed to answer incoming call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632527335560197111" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="203.188782" y="229">
      <linkto id="632527335560197112" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470066" type="Labeled" style="Bezier" ortho="true" label="default" />
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
    <node type="Action" id="632527335560197112" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="310.188843" y="229">
      <linkto id="632527335560196923" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">DSN</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="default" on="true" level="Warning" type="literal">OnIncomingCall: failed to open application suite database connection</log>
      </Properties>
    </node>
    <node type="Action" id="632527335560197295" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="407.7246" y="488">
      <linkto id="632527335560197296" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632527335560197297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">from</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632527335560197296" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="407.724548" y="603">
      <linkto id="632527335560197297" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632527335560197297" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="276.724548" y="603">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632527335560197487" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="111.90625" y="228">
      <linkto id="632527335560197111" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string from, ref string g_routingGuid, string routingGuid,  string to, ref string g_to, ref string g_from, ref string g_conferenceId, ref ArrayList g_outstandingEventsList)
{
	if (from == null || from == string.Empty)
		g_from = from = "UNAVAILABLE";

	if (to == null || to == string.Empty)
		g_to = to = "UNAVAILABLE";

	if (g_outstandingEventsList == null)
		g_outstandingEventsList = new ArrayList();

	g_conferenceId = string.Empty;
	g_routingGuid = routingGuid;
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Comment" id="632527335560197521" text="need to specify commandtimeout for media actions" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="390" y="32" />
    <node type="Action" id="632555905720470066" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="406" y="373">
      <linkto id="632527335560197295" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnIncomingCall: hanging up call with callId: " + callId</log>
      </Properties>
    </node>
    <node type="Action" id="632555905720470751" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="755" y="227">
      <linkto id="632527335560196849" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470752" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">g_from</ap>
        <ap name="DestinationNumber" type="variable">g_to</ap>
        <rd field="CallRecordsId">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Action" id="632555905720470752" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="832" y="340">
      <linkto id="632527335560196849" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">g_callRecordId</rd>
      </Properties>
    </node>
    <node type="Comment" id="632585988688887076" text="Initialize variables, check for callerId integrity,&#xD;&#xA;open Application Suite database, and answer the call.&#xD;&#xA;Play announcement to caller, prompting for user account&#xD;&#xA;number. If any of those operations fail, hang up the call, write &#xD;&#xA;call record, and exit. Otherwise, write start of call record, and &#xD;&#xA;wait for Play to complete." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="69" y="100" />
    <node type="Variable" id="632527335560196862" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632527335560196930" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
    <node type="Variable" id="632527335560196931" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632527335560197489" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632527335560197493" name="DSN" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">DSN</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632527335560196866" treenode="632527335560196867" appnode="632527335560196864" handlerfor="632527335560196863">
    <node type="Start" id="632527335560196866" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="276">
      <linkto id="632527335560197525" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197525" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="125" y="276">
      <linkto id="632527335560197539" type="Labeled" style="Bezier" ortho="true" label="account_code" />
      <linkto id="632527335560197543" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632528071621735660" type="Labeled" style="Bezier" ortho="true" label="pin_code" />
      <linkto id="632528071621735683" type="Labeled" style="Bezier" ortho="true" label="extension" />
      <linkto id="632538493910671412" type="Labeled" style="Bezier" ortho="true" label="mainMenu" />
      <linkto id="632555905720470737" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnPlay_Complete: Value of UserData is: " + userData
</log>
      </Properties>
    </node>
    <node type="Action" id="632527335560197536" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="493" y="44" mx="567" my="60">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632527335560197541" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Label" id="632527335560197539" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="126" y="156" />
    <node type="Label" id="632527335560197540" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="462" y="59">
      <linkto id="632527335560197536" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197541" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="703" y="58">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632527335560197542" text="Make sure to handle potential flake&#xD;&#xA;that can be caused by a RemoteHangup&#xD;&#xA;while a media op is in progress, if any." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="43" y="61" />
    <node type="Action" id="632527335560197543" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="428">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632528071621735660" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="262" y="162" />
    <node type="Label" id="632528071621735661" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="456" y="268">
      <linkto id="632528071621735662" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632528071621735662" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="493" y="252" mx="567" my="268">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632528071621735665" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735665" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="694" y="267">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632528071621735683" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="293" y="275" />
    <node type="Label" id="632528071621735684" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="463" y="493">
      <linkto id="632528071621735685" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632528071621735685" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="497" y="477" mx="571" my="493">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632528071621735688" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735688" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="704" y="492">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632538493910671412" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="299" y="363" />
    <node type="Label" id="632538493910671413" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="474" y="704">
      <linkto id="632538493910671414" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671414" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="517" y="687" mx="591" my="703">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632538493910671417" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671417" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="712" y="701">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632555905720470737" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="284" y="468" />
    <node type="Label" id="632555905720470738" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="470" y="924">
      <linkto id="632555905720470739" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720470739" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="537.825867" y="908" mx="575" my="924">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470740" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="705" y="923">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632585988688887077" text="Based on the UserData, different actions may be taken.&#xD;&#xA;Most of these involve receiving digits, while using specific &#xD;&#xA;termination conditions. Play may be used for error handling,&#xD;&#xA;and as such, a branch for the exit path is provided." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="34" y="523" />
    <node type="Variable" id="632527335560197524" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632527335560196871" treenode="632527335560196872" appnode="632527335560196869" handlerfor="632527335560196868">
    <node type="Start" id="632527335560196871" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="86" y="198">
      <linkto id="632555905720470760" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671459" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="328" y="197">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470760" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="161.825851" y="181" mx="199" my="197">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632538493910671459" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632527335560197529" treenode="632527335560197530" appnode="632527335560197527" handlerfor="632527335560197526">
    <node type="Start" id="632527335560197529" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="225">
      <linkto id="632605107870147738" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197547" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="293" y="225">
      <linkto id="632527335560197548" type="Labeled" style="Bezier" ortho="true" label="account_code" />
      <linkto id="632528071621735666" type="Labeled" style="Bezier" ortho="true" label="pin_code" />
      <linkto id="632528071621735689" type="Labeled" style="Bezier" ortho="true" label="extension" />
      <linkto id="632538493910671408" type="Labeled" style="Bezier" ortho="true" label="mainMenu" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Label" id="632527335560197548" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="292" y="114" />
    <node type="Label" id="632527335560197549" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="508.998657" y="135">
      <linkto id="632527335560197550" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560197550" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="589" y="135">
      <linkto id="632528071621735672" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632555905720470067" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632555905720470068" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470067" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632527335560197551" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="211" y="225">
      <linkto id="632527335560197547" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Verbose" type="csharp">"OnGatherDigits_Complete: Value of UserData is: " + userData + ", and the RoutingGuid is: " + g_routingGuid + ", and digits contains: " + digits</log>
public static string Execute(ref string digits)
{
	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);
	digits = digits.Replace("*", string.Empty);	
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632528071621735656" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="736" y="118" mx="789" my="134">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632528071621735659" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470071" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_pinNumAudio</ap>
        <ap name="UserData" type="literal">pin_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735659" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="898" y="133">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632528071621735666" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="415" y="115" />
    <node type="Label" id="632528071621735667" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="509" y="676">
      <linkto id="632528071621735668" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632528071621735668" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="591" y="676">
      <linkto id="632542558789588978" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632555905720470077" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632555905720470078" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470077" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735672" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="680" y="134">
      <linkto id="632528071621735656" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_accountNumber</rd>
      </Properties>
    </node>
    <node type="Action" id="632528071621735676" name="PhoneLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1028" y="676">
      <linkto id="632528130761068022" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632549474022185571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AccountCode" type="csharp">UInt32.Parse(g_accountNumber)</ap>
        <ap name="Pin" type="csharp">UInt32.Parse(digits)</ap>
        <ap name="UserPhoneNumber" type="variable">g_from</ap>
        <rd field="UserId">g_userId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - Validating user credentials"</log>
      </Properties>
    </node>
    <node type="Action" id="632528071621735677" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1645" y="660" mx="1698" my="676">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632528071621735682" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470089" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_extensionAudio</ap>
        <ap name="UserData" type="literal">extension</ap>
      </Properties>
    </node>
    <node type="Action" id="632528071621735682" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1846.01172" y="675">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632528071621735689" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="423" y="224" />
    <node type="Label" id="632528071621735690" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="628" y="1368">
      <linkto id="632528071621735691" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632528071621735691" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="727" y="1368">
      <linkto id="632555905720470741" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632555905720470742" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632555905720470743" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470741" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632528130761068022" name="GetRemoteAgentLevel" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1217" y="676">
      <linkto id="632528130761068024" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="UserLevel">userLevel</rd>
      </Properties>
    </node>
    <node type="Action" id="632528130761068024" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1385.4707" y="676">
      <linkto id="632528071621735677" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632528130761068027" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">userLevel</ap>
        <ap name="Value2" type="variable">g_adminLevelValue</ap>
      </Properties>
    </node>
    <node type="Action" id="632528130761068027" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1337.4707" y="830" mx="1390" my="846">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632528130761068030" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470084" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_insufficientLevelAudio</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632528130761068030" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1519.4707" y="845">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632528130761068037" name="RetrieveAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="1096.4707" y="1369">
      <linkto id="632528130761068041" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632538493910671420" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentDN" type="variable">digits</ap>
        <rd field="RoutingGuid">g_agentRoutingGuid</rd>
        <rd field="IsRecorded">g_isRecorded</rd>
        <log condition="entry" on="false" level="Info" type="literal">retrieve</log>
        <log condition="default" on="true" level="Info" type="csharp">"OnGatherDigits_Complete: the value of 'g_agentRoutingGuid' is: " + ((g_agentRoutingGuid == null) ? "null" : g_agentRoutingGuid)</log>
      </Properties>
    </node>
    <node type="Action" id="632528130761068041" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1046.4707" y="1466" mx="1099" my="1482">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632539304763653388" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_extensionAudio</ap>
        <ap name="Prompt3" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_noActiveCallAudio</ap>
        <ap name="UserData" type="literal">extension</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing no active call message."
</log>
      </Properties>
    </node>
    <node type="Action" id="632528130761068044" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1292.4707" y="1508" mx="1345" my="1524">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470746" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470747" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_instructionsAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_mainMenuNotRecordingAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing main menu message."
</log>
      </Properties>
    </node>
    <node type="Comment" id="632528130761068047" text="Need a way to distinguish that a record does not exist or if there was a problem" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="927.4707" y="1213" />
    <node type="Comment" id="632528130761068048" text="Need to further examine the DB constraints to avoid stale record issues" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="944.4707" y="1190" />
    <node type="Action" id="632538493910671176" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1457" y="1353" mx="1510" my="1369">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632538493910671407" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470746" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuRecordingAudio</ap>
        <ap name="Prompt3" type="variable">g_instructionsAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_callBeingRecordedAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing call being recorded main menu message."</log>
      </Properties>
    </node>
    <node type="Action" id="632538493910671179" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1341" y="1369">
      <linkto id="632538493910671176" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632528130761068044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isRecorded</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671407" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1682.4707" y="1368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632538493910671408" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="424" y="323" />
    <node type="Label" id="632538493910671409" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="495" y="2215">
      <linkto id="632538493910671410" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671410" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="585" y="2215">
      <linkto id="632538493910671419" type="Labeled" style="Bezier" ortho="true" label="maxdigits" />
      <linkto id="632555905720470748" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632538493910671446" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470748" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671419" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="806" y="2213.5">
      <linkto id="632538493910671426" type="Labeled" style="Bezier" ortho="true" label="Monitor" />
      <linkto id="632538493910671431" type="Labeled" style="Bezier" ortho="true" label="Record" />
      <linkto id="632538493910671436" type="Labeled" style="Bezier" ortho="true" label="NewExtension" />
      <linkto id="632538493910671446" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605079654814442" type="Labeled" style="Bezier" ortho="true" label="MainMenu" />
      <Properties language="csharp">
public static string Execute(string ui_monitorKey, string ui_recordingKey, string ui_selectAnotherExtKey, string ui_mainMenuKey, string digits)
{
	if (digits == null)
		return IApp.VALUE_FAILURE;

	if (digits.Equals(ui_monitorKey))
		return "Monitor";
	else if (digits.Equals(ui_recordingKey))
		return "Record";
	else if (digits.Equals(ui_selectAnotherExtKey))
		return "NewExtension";
	else if (digits.Equals(ui_mainMenuKey))
		return "MainMenu";
	else
		return "Unrecognized";
}
</Properties>
    </node>
    <node type="Action" id="632538493910671420" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1204" y="1368.5">
      <linkto id="632538493910671179" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">digits</ap>
        <rd field="ResultData">g_agentDN</rd>
      </Properties>
    </node>
    <node type="Action" id="632538493910671426" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="921.2754" y="2096" mx="961" my="2112">
      <items count="1">
        <item text="MonitorAgent" />
      </items>
      <linkto id="632538493910671456" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">MonitorAgent</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671431" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1053.97559" y="2198" mx="1093" my="2214">
      <items count="1">
        <item text="RecordAgent" />
      </items>
      <linkto id="632538493910671456" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">RecordAgent</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671436" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="764.6094" y="2010" mx="807" my="2026">
      <items count="1">
        <item text="NewExtension" />
      </items>
      <linkto id="632538493910671456" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Reason" type="literal">user</ap>
        <ap name="FunctionName" type="literal">NewExtension</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671444" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="760.4707" y="2644" mx="813" my="2660">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470757" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470758" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuNotRecordingAudio</ap>
        <ap name="Prompt3" type="variable">g_instructionsAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_unrecognizedInputAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671445" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="941" y="2509" mx="994" my="2525">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632538493910671447" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470757" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuRecordingAudio</ap>
        <ap name="Prompt3" type="variable">g_instructionsAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_unrecognizedInputAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671446" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="808" y="2525">
      <linkto id="632538493910671445" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632538493910671444" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isRecorded</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671447" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1145.4707" y="2525">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632538493910671456" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1089" y="2024">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632539304763653388" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="978.9414" y="1482">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632542558789588978" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="740.9414" y="676">
      <linkto id="632528071621735676" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632555905720470080" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(string digits, string g_accountNumber)
{
	// maybe we should do cast here, but then we need to mess around with having two
	// local variables just for account number and pin. Why are those UInts?

	if (g_accountNumber == null || g_accountNumber.Equals(string.Empty) || digits.Equals(string.Empty))
		return IApp.VALUE_FAILURE;

	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632549474022185571" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="981.4707" y="822" mx="1034" my="838">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632549474022185876" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470083" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_acctNumAudio</ap>
        <ap name="Prompt3" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_loginFailedAudio</ap>
        <ap name="UserData" type="literal">account_code</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing invalid login message."</log>
      </Properties>
    </node>
    <node type="Action" id="632549474022185876" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="901" y="839">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470067" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="589" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470068" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="541.4707" y="249" mx="594" my="265">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632555905720470071" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555905720470076" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_acctNumAudio</ap>
        <ap name="UserData" type="literal">account_code</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470071" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="752.825867" y="249" mx="790" my="265">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470086" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470076" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592.4707" y="410">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470077" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="590.4707" y="571">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632555905720470078" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="591.4707" y="788" />
    <node type="Label" id="632555905720470079" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="783.4707" y="36">
      <linkto id="632528071621735656" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720470080" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="693.4707" y="824" mx="746" my="840">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632549474022185876" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555905720470083" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_acctNumAudio</ap>
        <ap name="Prompt3" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_unrecognizedInputAudio</ap>
        <ap name="UserData" type="literal">account_code</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing invalid input message."</log>
      </Properties>
    </node>
    <node type="Action" id="632555905720470083" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="868.2966" y="952" mx="905" my="968">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632549474022185876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470084" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1416.29651" y="974" mx="1453" my="990">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632528130761068030" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470086" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="900.4707" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632555905720470088" text="Exit userdata for play" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1231.4707" y="837" />
    <node type="Action" id="632555905720470089" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1662.29651" y="833" mx="1699" my="849">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632528071621735682" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Comment" id="632555905720470090" text="?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1208.47656" y="714" />
    <node type="Action" id="632555905720470741" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="727" y="1262">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470742" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="841" y="1369">
      <linkto id="632528130761068037" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632555905720470743" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">digits.Equals(string.Empty)</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470743" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="792" y="1466" mx="845" my="1482">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632539304763653388" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_extensionAudio</ap>
        <ap name="Prompt3" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_unrecognizedInputAudio</ap>
        <ap name="UserData" type="literal">extension</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing invalid input message."</log>
      </Properties>
    </node>
    <node type="Action" id="632555905720470746" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1473.82581" y="1508" mx="1511" my="1524">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470747" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470747" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1508" y="1643">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470748" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="585" y="2108">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470757" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="955.8258" y="2644" mx="993" my="2660">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632555905720470758" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470758" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="810" y="2815">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605079654814442" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="897" y="2362">
      <linkto id="632605079654814443" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632605107870147742" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_conferenceId</ap>
        <ap name="Value2" type="csharp">string.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632605079654814443" name="LeaveConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="1073" y="2363">
      <linkto id="632605107870147743" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632605107870147738" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="116" y="225">
      <linkto id="632527335560197551" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <linkto id="632605107870147739" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">userData</ap>
        <ap name="Value2" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632605107870147739" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="211" y="338">
      <linkto id="632527335560197547" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(ref string digits)
{
	if (digits == null)
		digits = string.Empty;
	
	return IApp.VALUE_SUCCESS;
}

</Properties>
    </node>
    <node type="Comment" id="632605107870147740" text="Don't strip * or # when dealing with&#xD;&#xA;main menu." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="115" y="386" />
    <node type="Label" id="632605107870147741" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1340" y="1270">
      <linkto id="632538493910671179" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632605107870147742" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="897" y="2445" />
    <node type="Label" id="632605107870147743" text="M" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1074" y="2449" />
    <node type="Variable" id="632527335560197544" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" defaultInitWith="NONE" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632527335560197545" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632527335560197546" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632528130761068023" name="userLevel" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="" defaultInitWith="0" refType="reference">userLevel</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632527335560197534" treenode="632527335560197535" appnode="632527335560197532" handlerfor="632527335560197531">
    <node type="Start" id="632527335560197534" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="71">
      <linkto id="632555905720470761" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671460" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="294" y="71">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470761" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="119.825851" y="55" mx="157" my="71">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632538493910671460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRequestConferenceResponse" startnode="632527335560196853" treenode="632527335560196854" appnode="632527335560196851" handlerfor="632527335560196850">
    <node type="Start" id="632527335560196853" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="181">
      <linkto id="632544576604905873" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560196860" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1189" y="235">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632544576604905873" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="129" y="180">
      <linkto id="632566955826141689" type="Labeled" style="Bezier" label="StaleEvent" />
      <linkto id="632566955826141689" type="Labeled" style="Bezier" label="default" />
      <linkto id="632594454533619404" type="Labeled" style="Bezier" ortho="true" label="ValidEvent" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="literal">OnRequestConferenceResponse: function entry</log>
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "OnRequestConferenceResponse: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	if (g_outstandingEventsList.Contains(timerId))
	{
		log.Write(TraceLevel.Verbose, "OnRequestConferenceResponse: received valid event with TimerId: " + timerId + ". Removing from Outstanding list.");
		g_outstandingEventsList.Remove(timerId);
		return "ValidEvent";
	}
	else
	{
		log.Write(TraceLevel.Verbose, "OnRequestConferenceResponse: received STALE event with TimerId: " + timerId);				
		return "StaleEvent";
	}
}

</Properties>
    </node>
    <node type="Action" id="632544576604905874" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="804" y="234">
      <linkto id="632594454533619403" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632605079654814441" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ReceiveOnly" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="ConferenceId" type="variable">conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632544576604905875" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="687" y="233">
      <linkto id="632544576604905874" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">responseMessage</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRequestConferenceResponse: response message is: " + responseMessage</log>
      </Properties>
    </node>
    <node type="Action" id="632544576604905876" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="350" y="180">
      <linkto id="632544576604905877" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632544576604905878" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">success</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRequestConferenceResponse: the value of 'success' is: " + success.ToString()</log>
      </Properties>
    </node>
    <node type="Label" id="632544576604905877" text="T" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="472" y="123" />
    <node type="Label" id="632544576604905878" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="473" y="240" />
    <node type="Label" id="632544576604905879" text="T" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="595" y="233">
      <linkto id="632544576604905875" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632545163016516184" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="445.4707" y="628">
      <linkto id="632611348412560563" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632566955826141689" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="375">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632593879973704967" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="959.4707" y="219" mx="1034" my="235">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632527335560197530" />
        <item text="OnGatherDigits_Failed" treenode="632527335560197535" />
      </items>
      <linkto id="632527335560196860" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632594454533619403" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="963.0801" y="435" mx="1005" my="451">
      <items count="1">
        <item text="NewExtension" />
      </items>
      <linkto id="632527335560196860" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">callEnded</ap>
        <ap name="FunctionName" type="literal">NewExtension</ap>
      </Properties>
    </node>
    <node type="Action" id="632594454533619404" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="249" y="181">
      <linkto id="632544576604905876" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"OnRequestConferenceResponse: Removing timer with timerId: " + timerId</log>
      </Properties>
    </node>
    <node type="Action" id="632605079654814441" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="909.4707" y="233">
      <linkto id="632593879973704967" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">conferenceId</ap>
        <rd field="ResultData">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632611348412560563" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="559.145142" y="629">
      <linkto id="632611348412560565" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">responseMessage</ap>
      </Properties>
    </node>
    <node type="Action" id="632611348412560564" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="976.1452" y="763">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632611348412560565" name="RetrieveAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="674.145142" y="630">
      <linkto id="632611348412560568" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentDN" type="variable">g_agentDN</ap>
        <rd field="IsRecorded">g_isRecorded</rd>
      </Properties>
    </node>
    <node type="Action" id="632611348412560566" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="926.1452" y="613" mx="979" my="629">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632611348412560564" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuRecordingAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_callBeingRecordedAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632611348412560567" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="750.1452" y="748" mx="803" my="764">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632611348412560564" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_mainMenuNotRecordingAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632611348412560568" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="798.1452" y="630">
      <linkto id="632611348412560567" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632611348412560566" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isRecorded</ap>
      </Properties>
    </node>
    <node type="Variable" id="632544576604905869" name="success" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="Success" defaultInitWith="false" refType="reference">success</Properties>
    </node>
    <node type="Variable" id="632544576604905870" name="responseMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ResponseMessage" refType="reference">responseMessage</Properties>
    </node>
    <node type="Variable" id="632544576604905871" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" defaultInitWith="0" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632544576604905872" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TimerId" defaultInitWith="0" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecordingEventResponse" startnode="632527335560196858" treenode="632527335560196859" appnode="632527335560196856" handlerfor="632527335560196855">
    <node type="Start" id="632527335560196858" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="217">
      <linkto id="632538425211819379" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632527335560196861" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="153" y="410">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632538425211819367" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="396" y="217">
      <linkto id="632538425211819374" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632538425211819375" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632538425211819368" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="762" y="122">
      <linkto id="632544576604905323" type="Labeled" style="Bezier" ortho="true" label="RECORDING" />
      <linkto id="632544576604905324" type="Labeled" style="Bezier" ortho="true" label="NOT_RECORDING" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">responseMessage</ap>
      </Properties>
    </node>
    <node type="Action" id="632538425211819370" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="650" y="433">
      <linkto id="632593879973705375" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">responseMessage</ap>
      </Properties>
    </node>
    <node type="Label" id="632538425211819372" text="T" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="676" y="120">
      <linkto id="632538425211819368" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632538425211819373" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="577.9999" y="433">
      <linkto id="632538425211819370" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632538425211819374" text="T" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="530" y="125" />
    <node type="Label" id="632538425211819375" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="529" y="217" />
    <node type="Action" id="632538425211819379" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="152" y="217">
      <linkto id="632527335560196861" type="Labeled" style="Bezier" label="StaleEvent" />
      <linkto id="632527335560196861" type="Labeled" style="Bezier" label="default" />
      <linkto id="632594454533620863" type="Labeled" style="Bezier" ortho="true" label="ValidEvent" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="literal">OnRecordingEventResponse: function entry</log>
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "OnRecordingEventResponse: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	if (g_outstandingEventsList.Contains(timerId))
	{
		log.Write(TraceLevel.Verbose, "OnRecordingEventResponse: received valid event with TimerId: " + timerId + ". Removing from Outstanding list.");
		g_outstandingEventsList.Remove(timerId);
		return "ValidEvent";
	}
	else
	{
		log.Write(TraceLevel.Verbose, "OnRecordingEventResponse: received STALE event with TimerId: " + timerId);
		return "StaleEvent";
	}
}
</Properties>
    </node>
    <node type="Comment" id="632538425211819852" text="default path?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="315" />
    <node type="Action" id="632539304763653386" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1069" y="585">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632539304763653387" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1380" y="115">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632544576604905323" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="902.4707" y="32">
      <linkto id="632549551328160420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">g_recordingEnabledAudio</ap>
        <rd field="ResultData">g_isRecorded</rd>
        <rd field="ResultData2">statusPrompt</rd>
      </Properties>
    </node>
    <node type="Action" id="632544576604905324" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="906.4707" y="207">
      <linkto id="632549551328160420" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <ap name="Value2" type="variable">g_recordingDisabledAudio</ap>
        <rd field="ResultData">g_isRecorded</rd>
        <rd field="ResultData2">statusPrompt</rd>
      </Properties>
    </node>
    <node type="Action" id="632549551328160420" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1004.4707" y="99" mx="1057" my="115">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632539304763653387" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">statusPrompt</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Comment" id="632549551328160733" text="what userData to use?" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1284.4707" y="241" />
    <node type="Action" id="632593879973705375" name="RetrieveAgentRecord" class="MaxActionNode" group="" path="Metreos.Native.RemoteAgent" x="767" y="433">
      <linkto id="632593879973705382" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AgentDN" type="variable">g_agentDN</ap>
        <rd field="IsRecorded">g_isRecorded</rd>
      </Properties>
    </node>
    <node type="Action" id="632593879973705376" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1019" y="419" mx="1072" my="435">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632539304763653386" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_mainMenuRecordingAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_callBeingRecordedAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632593879973705379" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="851" y="568" mx="904" my="584">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632539304763653386" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_mainMenuNotRecordingAudio</ap>
        <ap name="UserData" type="literal">mainMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632593879973705382" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="900" y="434">
      <linkto id="632593879973705379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632593879973705376" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isRecorded</ap>
      </Properties>
    </node>
    <node type="Action" id="632594454533620863" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="276" y="217">
      <linkto id="632538425211819367" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnRecordingEventResponse: Removing timer with timerId: " + timerId
</log>
      </Properties>
    </node>
    <node type="Variable" id="632538425211819364" name="success" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="Success" defaultInitWith="false" refType="reference">success</Properties>
    </node>
    <node type="Variable" id="632538425211819365" name="responseMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ResponseMessage" refType="reference">responseMessage</Properties>
    </node>
    <node type="Variable" id="632538425211819366" name="conferenceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConferenceId" refType="reference">conferenceId</Properties>
    </node>
    <node type="Variable" id="632538425211819380" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TimerId" refType="reference">timerId</Properties>
    </node>
    <node type="Variable" id="632549551328160423" name="statusPrompt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">statusPrompt</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632538425211820327" treenode="632538425211820328" appnode="632538425211820325" handlerfor="632538425211820324">
    <node type="Start" id="632538425211820327" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="198">
      <linkto id="632538425211820330" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538425211820330" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="135" y="198">
      <linkto id="632538425211820335" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnTimerFire: Removing timer with ID: " + timerId</log>
      </Properties>
    </node>
    <node type="Action" id="632538425211820335" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="256.90625" y="198">
      <linkto id="632543463644885294" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "OnTimerFire: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	if (g_outstandingEventsList.Contains(timerId))
	{
		log.Write(TraceLevel.Verbose, "OnTimerFire: timer fired for outstanding event with TimerId: " + timerId + ". Removing from Outstanding list.");
		g_outstandingEventsList.Remove(timerId);
		return "Valid";
	}
	else
	{
		log.Write(TraceLevel.Verbose, "OnTimerFire: timer fired for event that is not in outstanding event list. TimerId: " + timerId );			return "Invalid";
	}
}
</Properties>
    </node>
    <node type="Action" id="632539304763653385" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="491" y="198">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632543463644885294" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="364" y="198">
      <linkto id="632539304763653385" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Variable" id="632538425211820329" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerId" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">timerId</Properties>
    </node>
    <node type="Variable" id="632543463644884955" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerUserData" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632550392566729310" treenode="632550392566729311" appnode="632550392566729308" handlerfor="632550392566729307">
    <node type="Start" id="632550392566729310" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="261">
      <linkto id="632555905720470754" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632550392566729312" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="318" y="260">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555905720470754" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="170.360016" y="260">
      <linkto id="632550392566729312" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">Normal</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632555905720469427" treenode="632555905720469428" appnode="632555905720469425" handlerfor="632555905720469424">
    <node type="Start" id="632555905720469427" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632555905720469434" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720469434" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="119" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632555905720469432" treenode="632555905720469433" appnode="632555905720469430" handlerfor="632555905720469429">
    <node type="Start" id="632555905720469432" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632555905720469435" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720469435" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="98" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632593879973704240" treenode="632593879973704241" appnode="632593879973704238" handlerfor="632593879973704237">
    <node type="Start" id="632593879973704240" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="159">
      <linkto id="632593879973704242" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632593879973704242" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="160">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632593879973704246" treenode="632593879973704247" appnode="632593879973704244" handlerfor="632593879973704243">
    <node type="Start" id="632593879973704246" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="107">
      <linkto id="632593879973704963" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632593879973704963" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="110" y="107">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="MonitorAgent" startnode="632538493910671429" treenode="632538493910671430" appnode="632538493910671427" handlerfor="632593879973704243">
    <node type="Start" id="632538493910671429" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="41" y="169">
      <linkto id="632538493910671462" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632538493910671461" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="398" y="169">
      <linkto id="632539304763653382" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632593879973704971" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SenderRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="EventName" type="literal">Metreos.Events.RemoteAgent.RequestConference</ap>
        <ap name="ToGuid" type="variable">g_agentRoutingGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632538493910671462" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="93" y="153" mx="155" my="169">
      <items count="1">
        <item text="OnTimerFire" treenode="632538425211820328" />
      </items>
      <linkto id="632538493910671464" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_timerFireDelay)</ap>
        <ap name="timerUserData" type="literal">Monitor</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">timerId</rd>
        <log condition="exit" on="false" level="Info" type="csharp">"MonitorAgent: Added timer with timerId: " + timerId
</log>
      </Properties>
    </node>
    <node type="Action" id="632538493910671464" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="281.90625" y="169">
      <linkto id="632538493910671461" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "MonitorAgent: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	g_outstandingEventsList.Add(timerId);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632539304763653382" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="537" y="168">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632549697335393901" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="533" y="373">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632550346808447329" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="360.609375" y="357" mx="403" my="373">
      <items count="1">
        <item text="NewExtension" />
      </items>
      <linkto id="632549697335393901" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Reason" type="literal">callEnded</ap>
        <ap name="FunctionName" type="literal">NewExtension</ap>
      </Properties>
    </node>
    <node type="Action" id="632593879973704971" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="399" y="260">
      <linkto id="632550346808447329" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632538493910671466" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RecordAgent" startnode="632538493910671434" treenode="632538493910671435" appnode="632538493910671432" handlerfor="632593879973704243">
    <node type="Start" id="632538493910671434" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="50" y="167">
      <linkto id="632542558789590852" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632539304763653383" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="549" y="164">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632542558789590850" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="399" y="167">
      <linkto id="632539304763653383" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632593879973704973" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SenderRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="ActionType" type="variable">actionType</ap>
        <ap name="EventName" type="literal">Metreos.Events.RemoteAgent.RecordingEvent</ap>
        <ap name="ToGuid" type="variable">g_agentRoutingGuid</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"RecordAgent: Sending RecordingEvent(" + actionType + ") to agent script."</log>
      </Properties>
    </node>
    <node type="Action" id="632542558789590852" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="101" y="151" mx="163" my="167">
      <items count="1">
        <item text="OnTimerFire" treenode="632538425211820328" />
      </items>
      <linkto id="632542558789591395" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_timerFireDelay)
</ap>
        <ap name="timerUserData" type="literal">Record</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">timerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632542558789591395" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="287" y="167">
      <linkto id="632542558789590850" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">public static string Execute(bool g_isRecorded, ref string actionType, string timerId, ref ArrayList g_outstandingEventsList)
{
	if (g_isRecorded)
		actionType = "end";
	else
		actionType = "begin";

	if (g_outstandingEventsList == null)
		g_outstandingEventsList = new ArrayList();

	g_outstandingEventsList.Add(timerId);
	return IApp.VALUE_SUCCESS;
}</Properties>
    </node>
    <node type="Action" id="632549697335393905" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="548" y="379">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632550346808447328" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="359.609375" y="364" mx="402" my="380">
      <items count="1">
        <item text="NewExtension" />
      </items>
      <linkto id="632549697335393905" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Reason" type="literal">callEnded</ap>
        <ap name="FunctionName" type="literal">NewExtension</ap>
      </Properties>
    </node>
    <node type="Action" id="632593879973704973" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="399.897766" y="268">
      <linkto id="632550346808447328" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Variable" id="632542558789590854" name="actionType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="end" refType="reference">actionType</Properties>
    </node>
    <node type="Variable" id="632542558789590855" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="NewExtension" startnode="632538493910671439" treenode="632538493910671440" appnode="632538493910671437" handlerfor="632593879973704243">
    <node type="Start" id="632538493910671439" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="91" y="272">
      <linkto id="632548695475214916" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632539304763653384" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="434">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632548695475214916" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="192" y="272">
      <linkto id="632550346808447330" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(ref string g_agentRoutingGuid, ref string g_conferenceId, ArrayList g_outstandingEventsList, ref bool g_isRecorded, ref string g_agentDN)
{
	g_agentRoutingGuid = g_conferenceId = g_agentDN = string.Empty;
	g_isRecorded = false;
	g_outstandingEventsList.Clear();
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632548695475214917" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="250" y="419" mx="303" my="435">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632539304763653384" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_extensionAudio</ap>
        <ap name="UserData" type="literal">extension</ap>
      </Properties>
    </node>
    <node type="Action" id="632550346808447330" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="298" y="273">
      <linkto id="632548695475214917" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632550346808447331" type="Labeled" style="Bezier" ortho="true" label="callEnded" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">reason</ap>
      </Properties>
    </node>
    <node type="Action" id="632550346808447331" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="433" y="257" mx="486" my="273">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632527335560196867" />
        <item text="OnPlay_Failed" treenode="632527335560196872" />
      </items>
      <linkto id="632539304763653384" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">g_extensionAudio</ap>
        <ap name="Prompt3" type="variable">g_poundAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="variable">g_callEndedAudio</ap>
        <ap name="UserData" type="literal">extension</ap>
      </Properties>
    </node>
    <node type="Variable" id="632550346808447327" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Reason" defaultInitWith="user" refType="reference">reason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Exit" startnode="632555905720470074" treenode="632555905720470075" appnode="632555905720470072" handlerfor="632593879973704243">
    <node type="Start" id="632555905720470074" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="95">
      <linkto id="632555905720470750" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555905720470749" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="299" y="97">
      <linkto id="632555905720470753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"Exit: hanging up call with callId: " + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632555905720470750" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="175" y="96">
      <linkto id="632555905720470749" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_callRecordId</ap>
        <ap name="EndReason" type="literal">Normal</ap>
      </Properties>
    </node>
    <node type="Action" id="632555905720470753" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="420" y="98">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>