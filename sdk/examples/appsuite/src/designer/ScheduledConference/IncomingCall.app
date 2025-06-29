<Application name="IncomingCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IncomingCall">
    <outline>
      <treenode type="evh" id="632473584568355442" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632473584568355439" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632473584568355438" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632473589366260141" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632473589366260138" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632473589366260137" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632802943703352249" actid="632473589366260147" />
          <ref id="632802943703352253" actid="632473589366260152" />
          <ref id="632802943703352270" actid="632473589366260225" />
          <ref id="632802943703352277" actid="632473589366260252" />
          <ref id="632802943703352293" actid="632518614741266874" />
          <ref id="632802943703352297" actid="632518614741266884" />
          <ref id="632802943703352302" actid="632518614741266889" />
          <ref id="632802943703352350" actid="632473589366260333" />
          <ref id="632802943703352354" actid="632473589366260337" />
          <ref id="632802943703352399" actid="632473589366260404" />
          <ref id="632802943703352414" actid="632473589366260432" />
          <ref id="632802943703352417" actid="632473589366260435" />
          <ref id="632802943703352439" actid="632473589366260475" />
          <ref id="632802943703352444" actid="632473589366260488" />
          <ref id="632802943703352461" actid="632473589366261054" />
          <ref id="632802943703352514" actid="632802943703352487" />
          <ref id="632802943703352517" actid="632802943703352488" />
          <ref id="632802943703352523" actid="632802943703352492" />
          <ref id="632802943703352527" actid="632802943703352494" />
          <ref id="632802943703352542" actid="632802943703352539" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632473589366260146" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632473589366260143" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632473589366260142" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632802943703352250" actid="632473589366260147" />
          <ref id="632802943703352254" actid="632473589366260152" />
          <ref id="632802943703352271" actid="632473589366260225" />
          <ref id="632802943703352278" actid="632473589366260252" />
          <ref id="632802943703352294" actid="632518614741266874" />
          <ref id="632802943703352298" actid="632518614741266884" />
          <ref id="632802943703352303" actid="632518614741266889" />
          <ref id="632802943703352351" actid="632473589366260333" />
          <ref id="632802943703352355" actid="632473589366260337" />
          <ref id="632802943703352400" actid="632473589366260404" />
          <ref id="632802943703352415" actid="632473589366260432" />
          <ref id="632802943703352418" actid="632473589366260435" />
          <ref id="632802943703352440" actid="632473589366260475" />
          <ref id="632802943703352445" actid="632473589366260488" />
          <ref id="632802943703352462" actid="632473589366261054" />
          <ref id="632802943703352515" actid="632802943703352487" />
          <ref id="632802943703352518" actid="632802943703352488" />
          <ref id="632802943703352524" actid="632802943703352492" />
          <ref id="632802943703352528" actid="632802943703352494" />
          <ref id="632802943703352543" actid="632802943703352539" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632473589366260216" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632473589366260213" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632473589366260212" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632802943703352267" actid="632473589366260222" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632473589366260221" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632473589366260218" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632473589366260217" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632802943703352268" actid="632473589366260222" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632473589366260243" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632473589366260240" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632473589366260239" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="632802943703352274" actid="632473589366260249" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632473589366260248" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632473589366260245" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632473589366260244" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="632802943703352275" actid="632473589366260249" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632473589366260496" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632473589366260493" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632473589366260492" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632473589366261040" level="1" text="Exit">
        <node type="function" name="Exit" id="632473589366261037" path="Metreos.StockTools" />
        <calls>
          <ref actid="632473589366261065" />
          <ref actid="632473589366261067" />
          <ref actid="632473589366261078" />
          <ref actid="632473589366261090" />
          <ref actid="632473589366261098" />
          <ref actid="632473589366261122" />
          <ref actid="632473589366261063" />
          <ref actid="632802943703352555" />
        </calls>
      </treenode>
      <treenode type="fun" id="632802943703352476" level="1" text="EnterConference">
        <node type="function" name="EnterConference" id="632802943703352473" path="Metreos.StockTools" />
        <calls>
          <ref actid="632802943703352472" />
          <ref actid="632802943703352550" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_incomingConnectionId" id="632802943703352200" vid="632273883766250161">
        <Properties type="Metreos.Types.Int">g_incomingConnectionId</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632802943703352202" vid="632274041751875217">
        <Properties type="Metreos.Types.String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632802943703352204" vid="632277635633125682">
        <Properties type="Metreos.Types.Int">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_confNumber" id="632802943703352206" vid="632279180679219022">
        <Properties type="Metreos.Types.Int">g_confNumber</Properties>
      </treenode>
      <treenode text="g_InConference" id="632802943703352208" vid="632279180679219028">
        <Properties type="Metreos.Types.Bool">g_InConference</Properties>
      </treenode>
      <treenode text="g_dbConnected" id="632802943703352210" vid="632279345072304570">
        <Properties type="Metreos.Types.Bool">g_dbConnected</Properties>
      </treenode>
      <treenode text="g_AS_DB_Server" id="632802943703352212" vid="632345833788907099">
        <Properties type="String" defaultInitWith="localhost" initWith="Server">g_AS_DB_Server</Properties>
      </treenode>
      <treenode text="g_AS_DB_Port" id="632802943703352214" vid="632345833788907101">
        <Properties type="UInt" defaultInitWith="3306" initWith="Port">g_AS_DB_Port</Properties>
      </treenode>
      <treenode text="g_AS_DB_Name" id="632802943703352216" vid="632345833788907103">
        <Properties type="String" defaultInitWith="application_suite" initWith="AS_DatabaseName">g_AS_DB_Name</Properties>
      </treenode>
      <treenode text="g_AS_DB_Username" id="632802943703352218" vid="632345833788907105">
        <Properties type="String" defaultInitWith="root" initWith="Username">g_AS_DB_Username</Properties>
      </treenode>
      <treenode text="g_AS_DB_Password" id="632802943703352220" vid="632345833788907107">
        <Properties type="String" defaultInitWith="metreos" initWith="Password">g_AS_DB_Password</Properties>
      </treenode>
      <treenode text="g_mmsId" id="632802943703352222" vid="632345833788907158">
        <Properties type="String">g_mmsId</Properties>
      </treenode>
      <treenode text="g_tolerance" id="632802943703352224" vid="632345963804531766">
        <Properties type="UInt" defaultInitWith="10" initWith="LoginTolerance">g_tolerance</Properties>
      </treenode>
      <treenode text="g_CallRecordsId" id="632802943703352226" vid="632346474164063012">
        <Properties type="UInt">g_CallRecordsId</Properties>
      </treenode>
      <treenode text="g_NumParticipants" id="632802943703352228" vid="632346474164063015">
        <Properties type="UInt" defaultInitWith="0">g_NumParticipants</Properties>
      </treenode>
      <treenode text="g_failureCounter" id="632802943703352230" vid="632387247834486306">
        <Properties type="UInt" defaultInitWith="0">g_failureCounter</Properties>
      </treenode>
      <treenode text="g_hostConfPin" id="632802943703352232" vid="632387456128704345">
        <Properties type="UInt">g_hostConfPin</Properties>
      </treenode>
      <treenode text="g_currentQueueFileId" id="632802943703352234" vid="632519348840747129">
        <Properties type="UInt">g_currentQueueFileId</Properties>
      </treenode>
      <treenode text="g_exiting" id="632802943703352236" vid="632519437692287321">
        <Properties type="String" defaultInitWith="false">g_exiting</Properties>
      </treenode>
      <treenode text="g_TTS" id="632802943703352238" vid="632764614125879565">
        <Properties type="String" initWith="UseTTS">g_TTS</Properties>
      </treenode>
      <treenode text="g_conferenceFailureCounter" id="632802943703352552" vid="632802943703352551">
        <Properties type="UInt" defaultInitWith="0">g_conferenceFailureCounter</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632473584568355441" treenode="632473584568355442" appnode="632473584568355439" handlerfor="632473584568355438">
    <node type="Start" id="632473584568355441" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="200">
      <linkto id="632473589366260108" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366260107" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="264" y="200">
      <linkto id="632473589366260120" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632473589366260136" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">as_dsn</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnIncomingCall: Opening database Connection...</log>
        <log condition="success" on="true" level="Info" type="literal">OnIncomingCall: OpenDatabase successful</log>
        <log condition="failure" on="true" level="Error" type="literal">OnIncomingCall: OpenDatabase: FAILURE OPENING DATABASE!</log>
      </Properties>
    </node>
    <node type="Action" id="632473589366260108" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="144" y="200">
      <linkto id="632473589366260107" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_AS_DB_Name</ap>
        <ap name="Server" type="variable">g_AS_DB_Server</ap>
        <ap name="Port" type="variable">g_AS_DB_Port</ap>
        <ap name="Username" type="variable">g_AS_DB_Username</ap>
        <ap name="Password" type="variable">g_AS_DB_Password</ap>
        <rd field="DSN">as_dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632473589366260119" name="WriteCallRecordStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="520" y="200">
      <linkto id="632473589366260130" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="OriginNumber" type="variable">incomingCallFrom</ap>
        <ap name="DestinationNumber" type="variable">to</ap>
        <rd field="CallRecordsId">g_CallRecordsId</rd>
      </Properties>
    </node>
    <node type="Action" id="632473589366260120" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="392" y="200">
      <linkto id="632473589366260119" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value3" type="variable">localIncomingCallId</ap>
        <rd field="ResultData">g_dbConnected</rd>
        <rd field="ResultData3">g_incomingCallId</rd>
      </Properties>
    </node>
    <node type="Comment" id="632473589366260126" text="if anything fails at this point,&#xD;&#xA;we play error message" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="712" y="512" />
    <node type="Action" id="632473589366260130" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="704" y="200">
      <linkto id="632473589366260131" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366260152" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <rd field="ConnectionId">g_incomingConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632473589366260131" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="704" y="352">
      <linkto id="632473589366260161" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_CallRecordsId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260136" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="264" y="368">
      <linkto id="632473589366260147" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632473589366260150" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260147" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="360" y="352" mx="413" my="368">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260162" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_later.wav</ap>
        <ap name="Prompt3" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_technical_difficulties.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">SetupFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260150" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="264" y="480">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366260152" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="816" y="184" mx="869" my="200">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260160" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632473589366261065" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_enter_conference_pin.wav</ap>
        <ap name="Prompt1" type="literal">sc_hello.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">ProcessDigits</ap>
        <log condition="default" on="true" level="Info" type="literal">OnIncomingCall: 'enter conference pin' greeting failed. Exiting</log>
      </Properties>
    </node>
    <node type="Action" id="632473589366260160" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1032" y="200">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366260161" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="704" y="464">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366260162" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="536" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261065" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="832" y="328" mx="869" my="344">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632473589366261066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261066" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="864" y="472">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632473589366260111" name="incomingCallFrom" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">incomingCallFrom</Properties>
    </node>
    <node type="Variable" id="632473589366260112" name="localIncomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">localIncomingCallId</Properties>
    </node>
    <node type="Variable" id="632473589366260115" name="as_dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">as_dsn</Properties>
    </node>
    <node type="Variable" id="632473589366260116" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="To" refType="reference" name="Metreos.CallControl.IncomingCall">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632473589366260140" treenode="632473589366260141" appnode="632473589366260138" handlerfor="632473589366260137">
    <node type="Start" id="632473589366260140" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632473589366260163" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366260163" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="224" y="368">
      <linkto id="632473589366260222" type="Labeled" style="Bezier" ortho="true" label="ProcessDigits" />
      <linkto id="632473589366260249" type="Labeled" style="Bezier" ortho="true" label="AddToConference" />
      <linkto id="632473589366261072" type="Labeled" style="Bezier" ortho="true" label="SetupFailure" />
      <linkto id="632473589366261073" type="Labeled" style="Bezier" ortho="true" label="MediaControlFailure" />
      <linkto id="632473589366261071" type="Labeled" style="Bezier" ortho="true" label="AuthenticationFailure" />
      <linkto id="632473589366261127" type="Labeled" style="Bezier" ortho="true" label="ConferenceNotReady" />
      <linkto id="632473589366261129" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632518614741266887" type="Labeled" style="Bezier" ortho="true" label="queue" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260222" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="368" y="456" mx="442" my="472">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632473589366260216" />
        <item text="OnGatherDigits_Failed" treenode="632473589366260221" />
      </items>
      <linkto id="632473589366260225" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366261131" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="TermCondMaxTime" type="literal">15000</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260225" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="392" y="624" mx="445" my="640">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260238" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632473589366261130" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_technical_difficulties.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">MediaControlFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260238" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="600" y="640">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366260249" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="384" y="264" mx="444" my="280">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632473589366260243" />
        <item text="OnRecord_Failed" treenode="632473589366260248" />
      </items>
      <linkto id="632473589366260252" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366261132" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="CommandTimeout" type="literal">10000</ap>
        <ap name="Expires" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="TermCondMaxTime" type="literal">10000</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="TermCondSilence" type="literal">10000</ap>
        <ap name="AudioFileSampleRate" type="literal">8</ap>
        <ap name="AudioFileSampleSize" type="literal">16</ap>
        <ap name="AudioFileEncoding" type="literal">pcm</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260252" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="632" y="264" mx="685" my="280">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260255" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632473589366261074" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_technical_difficulties.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">MediaControlFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260255" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="816" y="280">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261067" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="184" y="744" mx="221" my="760">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632473589366261068" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261068" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="216" y="888">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632473589366261071" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="316.4707" y="556" />
    <node type="Label" id="632473589366261072" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="128" y="536" />
    <node type="Label" id="632473589366261073" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="224" y="560" />
    <node type="Label" id="632473589366261074" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="682" y="451" />
    <node type="Label" id="632473589366261075" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="224" y="648">
      <linkto id="632473589366261067" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632473589366261127" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="136" y="632" />
    <node type="Action" id="632473589366261129" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="192">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632473589366261130" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="443" y="799" />
    <node type="Action" id="632473589366261131" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592" y="472">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261132" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="440" y="184">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632518614741266874" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1214" y="685" mx="1267" my="701">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632518614741266879" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632519348840747148" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_caller_entered_conference.wav</ap>
        <ap name="Prompt1" type="variable">nextFilename</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">queue</ap>
      </Properties>
    </node>
    <node type="Action" id="632518614741266879" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1424" y="624">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632518614741266884" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1220" y="521" mx="1273" my="537">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632518614741266879" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632519348840747146" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sc_caller_entered_conference.wav</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">queue</ap>
      </Properties>
    </node>
    <node type="Label" id="632518614741266887" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="552" y="368" />
    <node type="Label" id="632518614741266888" text="J" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="674" y="625">
      <linkto id="632519348840747134" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632518614741266889" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="942" y="727" mx="995" my="743">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632518614741266892" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632519348840747145" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sc_caller_exited_conference.wav</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">queue</ap>
      </Properties>
    </node>
    <node type="Action" id="632518614741266892" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="994" y="882">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632519348840747134" name="RemoveFromQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="848" y="625">
      <linkto id="632519348840747135" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632519437692287625" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="QueueId" type="variable">g_currentQueueFileId</ap>
        <ap name="Pin" type="variable">g_hostConfPin</ap>
        <rd field="NextQueueId">g_currentQueueFileId</rd>
        <rd field="NextType">nextType</rd>
        <rd field="NextFilename">nextFilename</rd>
      </Properties>
    </node>
    <node type="Action" id="632519348840747135" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="992" y="625">
      <linkto id="632518614741266889" type="Labeled" style="Bezier" ortho="true" label="2" />
      <linkto id="632519348840747139" type="Labeled" style="Bezier" ortho="true" label="1" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">nextType</ap>
      </Properties>
    </node>
    <node type="Action" id="632519348840747139" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1139" y="624">
      <linkto id="632518614741266884" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632518614741266874" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">nextFilename == String.Empty || nextFilename == null</ap>
      </Properties>
    </node>
    <node type="Action" id="632519348840747145" name="RemoveFromQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="1117.4707" y="745">
      <linkto id="632519437692287627" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="QueueId" type="variable">g_currentQueueFileId</ap>
        <ap name="Pin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632519348840747146" name="RemoveFromQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="1423.4707" y="538">
      <linkto id="632519437692287631" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="QueueId" type="variable">g_currentQueueFileId</ap>
        <ap name="Pin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632519348840747148" name="RemoveFromQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="1424.4707" y="701">
      <linkto id="632519437692287629" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="QueueId" type="variable">g_currentQueueFileId</ap>
        <ap name="Pin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632519437692287624" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="977" y="420">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632519437692287625" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="847" y="486">
      <linkto id="632519437692287624" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632519437692287626" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exiting</ap>
      </Properties>
    </node>
    <node type="Action" id="632519437692287626" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="972" y="553">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632519437692287627" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1118" y="884">
      <linkto id="632519437692287628" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632518614741266892" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exiting</ap>
      </Properties>
    </node>
    <node type="Action" id="632519437692287628" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1240" y="885">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632519437692287629" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1557.626" y="699">
      <linkto id="632519437692287633" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632518614741266879" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exiting</ap>
      </Properties>
    </node>
    <node type="Action" id="632519437692287631" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1552" y="537">
      <linkto id="632519437692287633" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632518614741266879" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exiting</ap>
      </Properties>
    </node>
    <node type="Action" id="632519437692287633" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1632.37476" y="620">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632473589366260164" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632519348840747136" name="nextType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">nextType</Properties>
    </node>
    <node type="Variable" id="632519348840747137" name="nextFilename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">nextFilename</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632473589366260145" treenode="632473589366260146" appnode="632473589366260143" handlerfor="632473589366260142">
    <node type="Start" id="632473589366260145" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="376">
      <linkto id="632473589366260271" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366260271" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="152" y="376">
      <linkto id="632473589366260273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366261088" type="Labeled" style="Bezier" ortho="true" label="Exit" />
      <linkto id="632473589366261085" type="Labeled" style="Bezier" ortho="true" label="MediaControlFailure" />
      <linkto id="632473589366261086" type="Labeled" style="Bezier" ortho="true" label="SetupFailure" />
      <linkto id="632473589366261076" type="Labeled" style="Bezier" ortho="true" label="AuthenicationFailure" />
      <linkto id="632473589366261128" type="Labeled" style="Bezier" ortho="true" label="ConferenceNotReady" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260273" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="432" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632473589366261076" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="328" y="208" />
    <node type="Action" id="632473589366261078" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="392" y="552" mx="429" my="568">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632473589366261079" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261079" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="432" y="688">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632473589366261080" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="432" y="448">
      <linkto id="632473589366261078" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632473589366261085" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="320" y="511.999969" />
    <node type="Label" id="632473589366261086" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="328" y="280" />
    <node type="Action" id="632473589366261088" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="312" y="616">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632473589366261128" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="320" y="440" />
    <node type="Variable" id="632473589366260274" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632473589366260215" treenode="632473589366260216" appnode="632473589366260213" handlerfor="632473589366260212">
    <node type="Start" id="632473589366260215" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="482">
      <linkto id="632473589366260293" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366260288" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="272" y="482">
      <linkto id="632473589366260300" type="Labeled" style="Vector" label="default" />
      <linkto id="632802943703352472" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="csharp">"OnGatherDigits_Complete: Digits received: " + restOfDigits</log>
	public static string Execute(ref int g_confNumber, string restOfDigits)
	{
		if (restOfDigits == null)
			return IApp.VALUE_FAILURE;
		string digits = restOfDigits.Replace("*",string.Empty);
		digits = digits.Replace("#",string.Empty);
		if (digits.Equals(string.Empty))
			return IApp.VALUE_FAILURE;

		g_confNumber = int.Parse(digits);
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632473589366260293" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="104" y="482">
      <linkto id="632473589366260288" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366260300" type="Labeled" style="Bezier" ortho="true" label="maxtime" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnGatherDigits_Complete: The termination condition was: " + termCond</log>
      </Properties>
    </node>
    <node type="Action" id="632473589366260300" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="443" y="329">
      <linkto id="632473589366260301" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_failureCounter+1</ap>
        <rd field="ResultData">g_failureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632473589366260301" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="441" y="219">
      <linkto id="632473589366260333" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366260337" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_failureCounter</ap>
        <ap name="Value2" type="csharp">(uint)3</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnReceiveDigits_Complete: The value of g_failureCounter is: " + g_failureCounter</log>
      </Properties>
    </node>
    <node type="Action" id="632473589366260333" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="390" y="70" mx="443" my="86">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260336" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632473589366260393" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_enter_conference_pin.wav</ap>
        <ap name="Prompt1" type="literal">sc_conference_pin_invalid.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">ProcessDigits</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260336" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="588" y="85">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366260337" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="540" y="203" mx="593" my="219">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260336" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632473589366260394" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_later.wav</ap>
        <ap name="Prompt3" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_check_conference_pin.wav</ap>
        <ap name="ConferenceId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">AuthenticationFailure</ap>
      </Properties>
    </node>
    <node type="Comment" id="632473589366260340" text="Here, we process the user input. We&#xD;&#xA;remove all *s and #s in from the input,&#xD;&#xA;using CustomCode. If the length of the user input is zero, then &#xD;&#xA;either the user failed to give proper input, or there was another failure&#xD;&#xA;and we take appropriate action." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="208" />
    <node type="Action" id="632473589366260353" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="445" y="625">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632473589366260393" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="305" y="86" />
    <node type="Label" id="632473589366260394" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="591" y="348" />
    <node type="Action" id="632473589366261090" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="760" y="184" mx="797" my="200">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632473589366261091" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261091" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="795" y="334">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632473589366261092" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="791" y="94">
      <linkto id="632473589366261090" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632802943703352472" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="397.636719" y="465" mx="447" my="481">
      <items count="1">
        <item text="EnterConference" />
      </items>
      <linkto id="632473589366260300" type="Labeled" style="Bezier" ortho="true" label="NoConference" />
      <linkto id="632473589366260353" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">EnterConference</ap>
      </Properties>
    </node>
    <node type="Variable" id="632473589366260275" name="restOfDigits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="0" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">restOfDigits</Properties>
    </node>
    <node type="Variable" id="632473589366260279" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632473589366260220" treenode="632473589366260221" appnode="632473589366260218" handlerfor="632473589366260217">
    <node type="Start" id="632473589366260220" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632473589366260404" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366260404" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="184" y="320" mx="237" my="336">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260407" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632473589366261098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_technical_difficulties.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">MediaControlFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260407" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="416" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261098" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="200" y="480" mx="237" my="496">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632473589366261099" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261099" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="232" y="624">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" activetab="true" startnode="632473589366260242" treenode="632473589366260243" appnode="632473589366260240" handlerfor="632473589366260239">
    <node type="Start" id="632473589366260242" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="304">
      <linkto id="632473589366260421" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366260409" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="816" y="304">
      <linkto id="632473589366260432" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366260435" type="Labeled" style="Bezier" ortho="true" label="silence" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260411" name="IncrementParticipantCount" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="304" y="304">
      <linkto id="632473589366260412" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260412" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="448" y="304">
      <linkto id="632519348840747128" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_InConference</rd>
      </Properties>
    </node>
    <node type="Action" id="632473589366260421" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="123" y="304">
      <linkto id="632473589366260411" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632802943703352557" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260431" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="123" y="658">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366260432" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="936" y="192" mx="989" my="208">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260448" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632519348840747131" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_has_entered_conference.wav</ap>
        <ap name="Prompt1" type="variable">filename</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="TermCondMaxTime" type="literal">11000</ap>
        <ap name="UserData" type="literal">queue</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260435" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="936" y="384" mx="989" my="400">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260448" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632519348840747132" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sc_caller_entered_conference.wav</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="TermCondMaxTime" type="literal">11000</ap>
        <ap name="UserData" type="literal">queue</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260448" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1144" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632473589366260449" text="We switch on the terminating condition of the RecordAudio action.&#xD;&#xA;If it terminated because it detected the specified length of silence, &#xD;&#xA;we play a standard prompt, otherwise we play the intro recorder by&#xD;&#xA;by the caller to the conference, using the filename passed into this&#xD;&#xA;event handler, which we assigned to the local variable fileName" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="250.824219" y="76" />
    <node type="Comment" id="632473589366260453" text="Generic announcement" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="928" y="472" />
    <node type="Comment" id="632473589366260455" text="We add the user to the conference" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="240" />
    <node type="Action" id="632518614741266867" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="594" y="432">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632519348840747128" name="AddToQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="593" y="302">
      <linkto id="632518614741266867" type="Labeled" style="Bezier" ortho="true" label="Queued" />
      <linkto id="632473589366260409" type="Labeled" style="Bezier" label="PlayFile" />
      <linkto id="632473589366260409" type="Labeled" style="Bezier" label="Failure" />
      <Properties final="false" type="native" log="On">
        <ap name="Pin" type="variable">g_hostConfPin</ap>
        <ap name="AnnouncementType" type="literal">1</ap>
        <ap name="Filename" type="variable">filename</ap>
        <rd field="QueueId">g_currentQueueFileId</rd>
      </Properties>
    </node>
    <node type="Action" id="632519348840747131" name="RemoveFromQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="1144" y="210">
      <linkto id="632473589366260448" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="QueueId" type="variable">g_currentQueueFileId</ap>
        <ap name="Pin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632519348840747132" name="RemoveFromQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="1144" y="401">
      <linkto id="632473589366260448" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="QueueId" type="variable">g_currentQueueFileId</ap>
        <ap name="Pin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352550" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="76.63672" y="507" mx="126" my="523">
      <items count="1">
        <item text="EnterConference" />
      </items>
      <linkto id="632473589366260431" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">EnterConference</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352557" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="123" y="419">
      <linkto id="632802943703352550" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_conferenceFailureCounter + 1</ap>
        <rd field="ResultData">g_conferenceFailureCounter</rd>
      </Properties>
    </node>
    <node type="Variable" id="632473589366260451" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" refType="reference" name="Metreos.MediaControl.Record_Complete">filename</Properties>
    </node>
    <node type="Variable" id="632473589366260452" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Record_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632473589366260247" treenode="632473589366260248" appnode="632473589366260245" handlerfor="632473589366260244">
    <node type="Start" id="632473589366260247" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632473589366260470" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366260459" name="IncrementParticipantCount" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="464" y="320">
      <linkto id="632473589366260460" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260460" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616" y="320">
      <linkto id="632473589366260488" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_InConference</rd>
      </Properties>
    </node>
    <node type="Action" id="632473589366260470" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="152" y="320">
      <linkto id="632473589366260472" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_conferenceId == 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260472" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="264" y="320">
      <linkto id="632473589366260459" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632473589366260475" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260475" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="216" y="488" mx="269" my="504">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632473589366260487" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_technical_difficulties.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">MediaControlFailure</ap>
      </Properties>
    </node>
    <node type="Label" id="632473589366260486" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="264" y="672" />
    <node type="Action" id="632473589366260487" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="400" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366260488" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="704" y="304" mx="757" my="320">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366260491" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sc_caller_entered_conference.wav</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366260491" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="896" y="320">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261121" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="584" y="752">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261122" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="552" y="608" mx="589" my="624">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632473589366261121" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632473589366261123" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="592" y="511.999969">
      <linkto id="632473589366261122" type="Basic" style="Bezier" ortho="true" />
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632473589366260495" treenode="632473589366260496" appnode="632473589366260493" handlerfor="632473589366260492">
    <node type="Start" id="632473589366260495" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="264">
      <linkto id="632473589366261063" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366261063" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="184" y="248" mx="221" my="264">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632473589366261064" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="isRemoteHangup" type="literal">true</ap>
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261064" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="376" y="264">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="Exit" startnode="632473589366261039" treenode="632473589366261040" appnode="632473589366261037" handlerfor="632473589366260492">
    <node type="Start" id="632473589366261039" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="288">
      <linkto id="632519437692287323" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632473589366261042" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="497" y="288">
      <linkto id="632473589366261045" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632473589366261044" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">g_InConference</ap>
        <log condition="entry" on="false" level="Info" type="literal">PerformHangup: Hangup successfull... removing connections to MMS.</log>
      </Properties>
    </node>
    <node type="Action" id="632473589366261043" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="353" y="288">
      <linkto id="632473589366261042" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_CallRecordsId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.Normal</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261044" name="DecrementParticipantCount" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="497" y="464">
      <linkto id="632473589366261046" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261045" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="833" y="288">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261046" name="IsConferenceEmpty" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="681" y="464">
      <linkto id="632473589366261047" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632519348840747141" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261047" name="DeleteConfRecord" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="833" y="464">
      <linkto id="632473589366261045" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261054" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="770" y="661" mx="823" my="677">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632473589366261055" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632519348840747142" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sc_caller_exited_conference.wav</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">queue</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261055" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="992" y="676">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632473589366261061" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="289" y="480">
      <linkto id="632473589366261043" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632473589366261062" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="224.999832" y="288">
      <linkto id="632473589366261043" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632473589366261061" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isRemoteHangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632519348840747141" name="AddToQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="679.4707" y="677">
      <linkto id="632473589366261054" type="Labeled" style="Bezier" label="PlayFile" />
      <linkto id="632473589366261054" type="Labeled" style="Bezier" label="Failure" />
      <linkto id="632519437692287622" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Pin" type="variable">g_hostConfPin</ap>
        <ap name="AnnouncementType" type="literal">2</ap>
        <rd field="QueueId">g_currentQueueFileId</rd>
      </Properties>
    </node>
    <node type="Action" id="632519348840747142" name="RemoveFromQueue" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="821.4707" y="830">
      <linkto id="632519437692287623" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="QueueId" type="variable">g_currentQueueFileId</ap>
        <ap name="Pin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632519437692287323" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="124" y="288">
      <linkto id="632473589366261062" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_exiting</rd>
      </Properties>
    </node>
    <node type="Action" id="632519437692287622" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="678" y="827">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632519437692287623" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="997.4707" y="829">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632473589366261060" name="isRemoteHangup" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="isRemoteHangup" defaultInitWith="false" refType="reference">isRemoteHangup</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="EnterConference" startnode="632802943703352475" treenode="632802943703352476" appnode="632802943703352473">
    <node type="Start" id="632802943703352475" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="300">
      <linkto id="632802943703352553" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632802943703352477" name="GetConference" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="280" y="303.1667">
      <linkto id="632802943703352478" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632802943703352487" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632802943703352549" type="Labeled" style="Bezier" ortho="true" label="NoConference" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_confNumber</ap>
        <rd field="MmsId">g_mmsId</rd>
        <rd field="MmsConfId">g_conferenceId</rd>
        <rd field="ScheduledTime">scheduledDate</rd>
        <rd field="DurationMinutes">numMinutes</rd>
        <rd field="ScheduledConferenceId">dbConfId</rd>
        <rd field="HostConferencePin">g_hostConfPin</rd>
        <log condition="exit" on="true" level="Info" type="literal">EnterConference:   GetConference Completed</log>
      </Properties>
    </node>
    <node type="Action" id="632802943703352478" name="SchedConfIdIntoCallRecord" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="440" y="303.1667">
      <linkto id="632802943703352479" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ScheduledConferenceId" type="variable">dbConfId</ap>
        <ap name="CallRecordsId" type="variable">g_CallRecordsId</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352479" name="IsConferenceReadyToStart" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="640" y="303.1667">
      <linkto id="632802943703352483" type="Labeled" style="Bezier" ortho="true" label="Yes" />
      <linkto id="632802943703352481" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632802943703352500" type="Labeled" style="Bezier" ortho="true" label="Expired" />
      <linkto id="632802943703352502" type="Labeled" style="Bezier" ortho="true" label="No" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_confNumber</ap>
        <ap name="Tolerance" type="variable">g_tolerance</ap>
        <log condition="default" on="true" level="Info" type="csharp">"EnterConference: The scheduled time for the conference is: " + scheduledDate</log>
      </Properties>
    </node>
    <node type="Action" id="632802943703352480" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="596.75" y="493.6667">
      <linkto id="632802943703352488" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">sc_not_yet_scheduled.wav</ap>
        <rd field="ResultData">filenameToPlay</rd>
      </Properties>
    </node>
    <node type="Action" id="632802943703352481" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="792" y="471.1667">
      <linkto id="632802943703352488" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">sc_technical_difficulties.wav</ap>
        <rd field="ResultData">filenameToPlay</rd>
      </Properties>
    </node>
    <node type="Action" id="632802943703352482" name="UpdateConference" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="1160" y="303.1667">
      <linkto id="632802943703352485" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="ScheduledConferenceId" type="variable">dbConfId</ap>
        <ap name="MmsId" type="variable">g_mmsId</ap>
        <ap name="MmsConfId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352483" name="IsConferenceEmpty" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="824" y="303.1667">
      <linkto id="632802943703352491" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632802943703352539" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
        <rd field="ParticipantCount">numParticipants</rd>
      </Properties>
    </node>
    <node type="Action" id="632802943703352484" name="WriteCallRecordStop" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="992" y="439.1667">
      <linkto id="632802943703352492" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="CallRecordsId" type="variable">g_CallRecordsId</ap>
        <ap name="EndReason" type="csharp">Metreos.ApplicationSuite.Storage.EndReason.InternalError</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352485" name="CreateConfRecord" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="1312" y="303.1667">
      <linkto id="632802943703352486" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352486" name="IncrementParticipantCount" class="MaxActionNode" group="" path="Metreos.Native.ScheduledConference" x="1480" y="303.1667">
      <linkto id="632802943703352494" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencePin" type="variable">g_hostConfPin</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352487" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="231" y="432.1667" mx="284" my="448">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632802943703352490" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632802943703352497" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_later.wav</ap>
        <ap name="Prompt3" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_technical_difficulties.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">DatabaseFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352488" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="592" y="623.1667" mx="645" my="639">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632802943703352489" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632802943703352498" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="variable">filenameToPlay</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">ConferenceNotReady</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352489" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="642" y="769.1667">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632802943703352490" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="281" y="611.1667">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632802943703352491" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="992" y="303.1667">
      <linkto id="632802943703352482" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632802943703352484" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632802943703352492" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="944" y="551.1667" mx="997" my="567">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632802943703352493" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632802943703352499" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="literal">sc_goodbye.wav</ap>
        <ap name="Prompt1" type="literal">sc_technical_difficulties.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">MediaControlFailure</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352493" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1160" y="567.1667">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632802943703352494" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="1576" y="287.1667" mx="1629" my="303">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632802943703352496" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sc_welcome_to_conference.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352495" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1888" y="303.1667">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632802943703352496" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1744" y="303.1667">
      <linkto id="632802943703352495" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_NumParticipants++</ap>
        <ap name="Value2" type="literal">true</ap>
        <ap name="Value3" type="literal">0</ap>
        <rd field="ResultData">g_NumParticipants</rd>
        <rd field="ResultData2">g_InConference</rd>
        <rd field="ResultData3">g_failureCounter</rd>
      </Properties>
    </node>
    <node type="Label" id="632802943703352497" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="387" y="448.1667" />
    <node type="Label" id="632802943703352498" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="768" y="639.1667" />
    <node type="Label" id="632802943703352499" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="994" y="710.1667" />
    <node type="Action" id="632802943703352500" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="488" y="471.1667">
      <linkto id="632802943703352488" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">sc_conference_expired.wav</ap>
        <rd field="ResultData">filenameToPlay</rd>
      </Properties>
    </node>
    <node type="Action" id="632802943703352501" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="685" y="496.9167">
      <linkto id="632802943703352488" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">"The conference is not yet scheduled to start.   Please try your call again within " + g_tolerance + " minutes of your scheduled time."</ap>
        <rd field="ResultData">filenameToPlay</rd>
      </Properties>
    </node>
    <node type="Action" id="632802943703352502" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="640.25" y="434.9167">
      <linkto id="632802943703352501" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632802943703352480" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_TTS</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352539" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="773" y="149" mx="826" my="165">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632473589366260141" />
        <item text="OnPlay_Failed" treenode="632473589366260146" />
      </items>
      <linkto id="632802943703352540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sc_state_name_company.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">AddToConference</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352540" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="822" y="71">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632802943703352549" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="279" y="187">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">NoConference</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352553" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="138" y="301">
      <linkto id="632802943703352477" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632802943703352487" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_conferenceFailureCounter &lt; 2</ap>
      </Properties>
    </node>
    <node type="Label" id="632802943703352554" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1409" y="583">
      <linkto id="632802943703352555" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632802943703352555" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1486.82581" y="567" mx="1524" my="583">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632802943703352556" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632802943703352556" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1650" y="583">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632802943703352538" name="numMinutes" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Int" refType="reference">numMinutes</Properties>
    </node>
    <node type="Variable" id="632802943703352545" name="dbConfId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">dbConfId</Properties>
    </node>
    <node type="Variable" id="632802943703352546" name="scheduledDate" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DateTime" refType="reference">scheduledDate</Properties>
    </node>
    <node type="Variable" id="632802943703352547" name="filenameToPlay" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">filenameToPlay</Properties>
    </node>
    <node type="Variable" id="632802943703352548" name="numParticipants" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" defaultInitWith="0" refType="reference">numParticipants</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>