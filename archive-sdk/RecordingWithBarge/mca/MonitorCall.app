<Application name="MonitorCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="MonitorCall">
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
          <ref id="633078146602994636" actid="632644651432639882" />
          <ref id="633078146602994656" actid="632644651432639926" />
          <ref id="633078146602994664" actid="632644651432639945" />
          <ref id="633078146602994703" actid="632605817629675036" />
          <ref id="633078146602994729" actid="632605881876499391" />
          <ref id="633078146602994732" actid="632605881876499394" />
          <ref id="633078146602994751" actid="632644651432639815" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605770032726854" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632605770032726851" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632605770032726850" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="633078146602994637" actid="632644651432639882" />
          <ref id="633078146602994657" actid="632644651432639926" />
          <ref id="633078146602994665" actid="632644651432639945" />
          <ref id="633078146602994704" actid="632605817629675036" />
          <ref id="633078146602994730" actid="632605881876499391" />
          <ref id="633078146602994733" actid="632605881876499394" />
          <ref id="633078146602994752" actid="632644651432639815" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605817629675007" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632605817629675004" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632605817629675003" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="633078146602994600" actid="632644651432639953" />
          <ref id="633078146602994645" actid="632644651432639893" />
          <ref id="633078146602994711" actid="632605817629675060" />
          <ref id="633078146602994758" actid="632644651432639857" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632605817629675012" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632605817629675009" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632605817629675008" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="633078146602994601" actid="632644651432639953" />
          <ref id="633078146602994646" actid="632644651432639893" />
          <ref id="633078146602994712" actid="632605817629675060" />
          <ref id="633078146602994759" actid="632644651432639857" />
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
      <treenode type="evh" id="632644651432639936" level="2" text="Metreos.MediaControl.Record_Complete: OnRecord_Complete">
        <node type="function" name="OnRecord_Complete" id="632644651432639933" path="Metreos.StockTools" />
        <node type="event" name="Record_Complete" id="632644651432639932" path="Metreos.MediaControl.Record_Complete" />
        <references>
          <ref id="633078146602994661" actid="632644651432639942" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632644651432639941" level="2" text="Metreos.MediaControl.Record_Failed: OnRecord_Failed">
        <node type="function" name="OnRecord_Failed" id="632644651432639938" path="Metreos.StockTools" />
        <node type="event" name="Record_Failed" id="632644651432639937" path="Metreos.MediaControl.Record_Failed" />
        <references>
          <ref id="633078146602994662" actid="632644651432639942" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="fun" id="632605817629675035" level="1" text="RequestAuthentication">
        <node type="function" name="RequestAuthentication" id="632605817629675032" path="Metreos.StockTools" />
        <calls>
          <ref actid="632605817629675050" />
          <ref actid="632644651432639846" />
          <ref actid="632644651432639847" />
          <ref actid="632605881876499380" />
          <ref actid="632605881876499404" />
        </calls>
      </treenode>
      <treenode type="fun" id="632605817629675024" level="1" text="Exit">
        <node type="function" name="Exit" id="632605817629675021" path="Metreos.StockTools" />
        <calls>
          <ref actid="632605817629675049" />
          <ref actid="632605817629675020" />
          <ref actid="632606843862474824" />
          <ref actid="632644651432639843" />
          <ref actid="632606843862474830" />
          <ref actid="632605881876499384" />
          <ref actid="632606225558163532" />
          <ref actid="632646266605191616" />
          <ref actid="632646266605191618" />
          <ref actid="632605881876499401" />
          <ref actid="632644651432639817" />
          <ref actid="632644651432639860" />
        </calls>
      </treenode>
      <treenode type="fun" id="632644651432639810" level="1" text="GetExtensionToMonitor">
        <node type="function" name="GetExtensionToMonitor" id="632644651432639807" path="Metreos.StockTools" />
        <calls>
          <ref actid="632605817629675031" />
          <ref actid="632605817629675063" />
          <ref actid="632644651432639806" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_originalTo" id="633078146602994516" vid="632605881876499427">
        <Properties type="String">g_originalTo</Properties>
      </treenode>
      <treenode text="g_callId" id="633078146602994518" vid="632605770032726814">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="633078146602994520" vid="632605770032726816">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="633078146602994522" vid="632605770032726860">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_displayName" id="633078146602994524" vid="632605770032726858">
        <Properties type="String" initWith="DisplayName">g_displayName</Properties>
      </treenode>
      <treenode text="g_failureThreshold" id="633078146602994526" vid="632605817629675039">
        <Properties type="UInt" defaultInitWith="3">g_failureThreshold</Properties>
      </treenode>
      <treenode text="g_passCode" id="633078146602994528" vid="632605881876499375">
        <Properties type="String" initWith="PassCode">g_passCode</Properties>
      </treenode>
      <treenode text="g_maxFailedLogins" id="633078146602994530" vid="632605881876499389">
        <Properties type="UInt" initWith="LoginFailureThresh">g_maxFailedLogins</Properties>
      </treenode>
      <treenode text="g_failedLogins" id="633078146602994532" vid="632605881876499381">
        <Properties type="UInt" defaultInitWith="0">g_failedLogins</Properties>
      </treenode>
      <treenode text="g_isCallerAnswered" id="633078146602994534" vid="632605817629675054">
        <Properties type="Bool" defaultInitWith="false">g_isCallerAnswered</Properties>
      </treenode>
      <treenode text="g_fromNumber" id="633078146602994536" vid="632609914127331545">
        <Properties type="String">g_fromNumber</Properties>
      </treenode>
      <treenode text="AuthFailedAudio" id="633078146602994538" vid="632611925512542955">
        <Properties type="String" initWith="AuthFailedAudio">AuthFailedAudio</Properties>
      </treenode>
      <treenode text="RequestAuthAudio" id="633078146602994540" vid="632611925512542957">
        <Properties type="String" initWith="RequestAuthAudio">RequestAuthAudio</Properties>
      </treenode>
      <treenode text="GoodByeAudio" id="633078146602994542" vid="632611925512542959">
        <Properties type="String" initWith="GoodByeAudio">GoodByeAudio</Properties>
      </treenode>
      <treenode text="PoundSignAudio" id="633078146602994544" vid="632611925512542965">
        <Properties type="String" initWith="PoundSignAudio">PoundSignAudio</Properties>
      </treenode>
      <treenode text="g_recording" id="633078146602994546" vid="632644651432639897">
        <Properties type="Bool" defaultInitWith="false">g_recording</Properties>
      </treenode>
      <treenode text="g_recordingDigit" id="633078146602994548" vid="632644651432639899">
        <Properties type="String" initWith="RecordKey">g_recordingDigit</Properties>
      </treenode>
      <treenode text="g_recordingConnectionId" id="633078146602994550" vid="632644651432639908">
        <Properties type="String">g_recordingConnectionId</Properties>
      </treenode>
      <treenode text="RecordNotifyAudio" id="633078146602994552" vid="632644651432641740">
        <Properties type="String" initWith="RecordNotifyAudio">RecordNotifyAudio</Properties>
      </treenode>
      <treenode text="ExtensionAudio" id="633078146602994554" vid="632644651432641742">
        <Properties type="String" initWith="ExtensionToMonitorAudio">ExtensionAudio</Properties>
      </treenode>
      <treenode text="NoActiveCallAudio" id="633078146602994556" vid="632644651432641744">
        <Properties type="String" initWith="NoActiveCallAudio">NoActiveCallAudio</Properties>
      </treenode>
      <treenode text="g_exit" id="633078146602994558" vid="632646266605191612">
        <Properties type="Bool" defaultInitWith="false">g_exit</Properties>
      </treenode>
      <treenode text="g_mmsId" id="633078146602994560" vid="632767263316803637">
        <Properties type="UInt">g_mmsId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632605770032726810" treenode="632605770032726811" appnode="632605770032726808" handlerfor="632605770032726807">
    <node type="Start" id="632605770032726810" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="159">
      <linkto id="632605770032726818" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605770032726818" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="364" y="158">
      <linkto id="632605770032726844" type="Labeled" style="Bezier" label="default" />
      <linkto id="632605817629675058" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="DisplayName" type="variable">g_displayName</ap>
        <ap name="WaitForMedia" type="csharp">true</ap>
        <ap name="Conference" type="literal">false</ap>
        <rd field="CallId">g_callId</rd>
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
      <linkto id="632605770032726862" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnIncomingCall: calling Exit function</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675050" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="566.551758" y="143" mx="631" my="159">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632605817629675049" type="Labeled" style="Bezier" label="default" />
      <linkto id="632605770032726862" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnIncomingCall: Invoking RequestAuthentication</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675058" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="496" y="159">
      <linkto id="632605817629675050" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">fromNumber</ap>
        <rd field="ResultData">g_isCallerAnswered</rd>
        <rd field="ResultData2">g_fromNumber</rd>
      </Properties>
    </node>
    <node type="Variable" id="632605770032726812" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
    <node type="Variable" id="632609914127331544" name="fromNumber" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">fromNumber</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632605770032726848" treenode="632605770032726849" appnode="632605770032726846" handlerfor="632605770032726845">
    <node type="Start" id="632605770032726848" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="58" y="252">
      <linkto id="632605817629675016" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605817629675016" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="147" y="252">
      <linkto id="632605817629675017" type="Labeled" style="Bezier" label="auth_req" />
      <linkto id="632606843862474818" type="Labeled" style="Bezier" label="exit" />
      <linkto id="632606843862474819" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639823" type="Labeled" style="Bezier" label="extension" />
      <linkto id="632644651432639949" type="Labeled" style="Bezier" label="record" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnPlay_Complete: UserData is: " + userData</log>
      </Properties>
    </node>
    <node type="Label" id="632605817629675017" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="279" y="251" />
    <node type="Label" id="632605817629675018" text="X" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="388" y="885">
      <linkto id="632605817629675025" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605817629675019" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="890" y="882">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605817629675020" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="681.8259" y="1037" mx="719" my="1053">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632605817629675019" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling Exit function
</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675025" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="546" y="884">
      <linkto id="632605817629675027" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632605817629675031" type="Labeled" style="Bezier" label="default" />
      <linkto id="632605817629675063" type="Labeled" style="Bezier" label="eod" />
      <linkto id="632605817629675063" type="Labeled" style="Bezier" label="maxdigits" />
      <linkto id="632605817629675027" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnPlay_Complete: Termination condition: " + termCond</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675027" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="399" y="1054">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605817629675031" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="484.897461" y="1037" mx="552" my="1053">
      <items count="1">
        <item text="GetExtensionToMonitor" />
      </items>
      <linkto id="632605817629675027" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632605817629675020" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">GetExtensionToMonitor</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling GetExtensionToMonitor(Action = play) function
</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675063" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="649.897461" y="867" mx="717" my="883">
      <items count="1">
        <item text="GetExtensionToMonitor" />
      </items>
      <linkto id="632605817629675019" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632605817629675020" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">digits</ap>
        <ap name="FunctionName" type="literal">GetExtensionToMonitor</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling GetExtensionToMonitor(Action = digits) function</log>
      </Properties>
    </node>
    <node type="Label" id="632606843862474818" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="282" y="357" />
    <node type="Action" id="632606843862474819" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="148" y="402">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632606843862474820" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="385" y="596">
      <linkto id="632606843862474822" type="Basic" style="Bezier" />
    </node>
    <node type="Comment" id="632606843862474821" text="termCond&#xD;&#xA;" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="449" y="32" />
    <node type="Action" id="632606843862474822" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="485" y="596">
      <linkto id="632606843862474823" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632606843862474824" type="Labeled" style="Bezier" label="default" />
      <linkto id="632606843862474823" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632606843862474823" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="653" y="595">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606843862474824" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="451.825867" y="706" mx="489" my="722">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632606843862474823" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632644651432639823" text="X" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="278" y="148" />
    <node type="Label" id="632644651432639841" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="410" y="188">
      <linkto id="632644651432639844" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432639842" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="912" y="185">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632644651432639843" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="702.825867" y="338" mx="740" my="354">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632644651432639842" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling Exit function
</log>
      </Properties>
    </node>
    <node type="Action" id="632644651432639844" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="568" y="187">
      <linkto id="632644651432639845" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632644651432639846" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639847" type="Labeled" style="Bezier" label="eod" />
      <linkto id="632644651432639847" type="Labeled" style="Bezier" label="maxdigits" />
      <linkto id="632644651432639845" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnPlay_Complete: Termination condition: " + termCond</log>
      </Properties>
    </node>
    <node type="Action" id="632644651432639845" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="421" y="357">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632644651432639846" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="509.551758" y="340" mx="574" my="356">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632644651432639845" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632644651432639843" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling RequestAuthentication(Action = play) function
</log>
      </Properties>
    </node>
    <node type="Action" id="632644651432639847" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="674.551758" y="170" mx="739" my="186">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632644651432639842" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632644651432639843" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">digits</ap>
        <ap name="FunctionName" type="literal">RequestAuthentication</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnPlay_Complete: Calling RequestAuthentication(Action = digits) function</log>
      </Properties>
    </node>
    <node type="Label" id="632644651432639949" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="145" y="116" />
    <node type="Label" id="632644651432639950" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="369" y="1421">
      <linkto id="632644651432639951" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432639951" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="491" y="1421">
      <linkto id="632644651432639952" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632644651432639953" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639952" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639952" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="490" y="1280">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632644651432639953" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="552" y="1406" mx="626" my="1422">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632605817629675007" />
        <item text="OnGatherDigits_Failed" treenode="632605817629675012" />
      </items>
      <linkto id="632644651432639957" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="variable">g_recordingDigit</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">record</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639957" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="755" y="1422">
      <Properties final="true" type="appControl" log="On">
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
      <linkto id="632606843862474826" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632606225558163508" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="182" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632606843862474826" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="116" y="164">
      <linkto id="632606225558163508" type="Labeled" style="Bezier" label="default" />
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
      <linkto id="632606225558163508" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Variable" id="632606843862474825" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" activetab="true" startnode="632605817629675006" treenode="632605817629675007" appnode="632605817629675004" handlerfor="632605817629675003">
    <node type="Start" id="632605817629675006" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="57" y="244">
      <linkto id="632605881876499372" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605881876499372" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="168" y="244">
      <linkto id="632605881876499374" type="Labeled" style="Bezier" label="auth_req" />
      <linkto id="632644651432639866" type="Labeled" style="Bezier" label="record" />
      <linkto id="632644651432639867" type="Labeled" style="Bezier" label="extension" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">userData</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData is: " + userData
</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499374" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="309" y="245">
      <linkto id="632605881876499378" type="Labeled" style="Bezier" label="digit" />
      <linkto id="632605881876499403" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632605881876499404" type="Labeled" style="Bezier" label="default" />
      <linkto id="632605881876499403" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: Termination Condition is: " + termCond</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499378" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="508" y="246">
      <linkto id="632605881876499379" type="Labeled" style="Bezier" label="default" />
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
      <linkto id="632605881876499380" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639806" type="Labeled" style="Bezier" label="equal" />
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
      <linkto id="632605881876499383" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632605881876499384" type="Labeled" style="Bezier" label="default" />
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
      <linkto id="632605881876499383" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OnGatherDigits_Complete: Calling Exit function</log>
      </Properties>
    </node>
    <node type="Action" id="632605881876499403" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="308" y="128">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632605881876499404" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="249.551758" y="356" mx="314" my="372">
      <items count="1">
        <item text="RequestAuthentication" />
      </items>
      <linkto id="632605881876499405" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632605881876499407" type="Labeled" style="Bezier" label="default" />
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
      <linkto id="632605881876499384" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632605881876499407" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="311.4707" y="497" />
    <node type="Comment" id="632608290328114535" text="Check if user input matches defined passcode." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="368" y="32" />
    <node type="Action" id="632644651432639806" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="874.177734" y="232" mx="941" my="248">
      <items count="1">
        <item text="GetExtensionToMonitor" />
      </items>
      <linkto id="632605881876499383" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="literal">play</ap>
        <ap name="FunctionName" type="literal">GetExtensionToMonitor</ap>
      </Properties>
    </node>
    <node type="Label" id="632644651432639866" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="167" y="134" />
    <node type="Label" id="632644651432639867" text="X" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="170" y="358" />
    <node type="Label" id="632644651432639868" text="X" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="156" y="779">
      <linkto id="632644651432639877" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432639871" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="636.847046" y="778">
      <linkto id="632644651432639882" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639890" type="Labeled" style="Bezier" label="Success" />
      <Properties language="csharp">
public static string Execute(DataTable queryResult, ref string g_conferenceId, ref uint g_mmsId)
{
	if (queryResult == null || queryResult.Rows == null || queryResult.Rows.Count == 0)
		return IApp.VALUE_FAILURE;

	DataRow row = queryResult.Rows[0];
	if (Convert.IsDBNull(row["conference_id"]))
		return IApp.VALUE_FAILURE;

	if (Convert.IsDBNull(row["mms_id"]))
		return IApp.VALUE_FAILURE;

	g_conferenceId = Convert.ToString(row["conference_id"]);
	g_mmsId	= Convert.ToUInt32(row["mms_id"]);
	return IApp.VALUE_SUCCESS;		
}
</Properties>
    </node>
    <node type="Action" id="632644651432639875" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="355" y="779">
      <linkto id="632644651432639881" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639886" type="Labeled" style="Bezier" label="Success" />
      <Properties language="csharp">
public static string Execute(ref string digits, LogWriter log)
{
	log.Write(TraceLevel.Verbose, "OnGatherDigits_Complete: cleaning up received digits string.");

	if (digits == null)
		digits = string.Empty;

	digits = digits.Replace("#", string.Empty);
	return digits == string.Empty ? IApp.VALUE_FAILURE : IApp.VALUE_SUCCESS;	
}
</Properties>
    </node>
    <node type="Action" id="632644651432639877" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="242" y="779">
      <linkto id="632644651432639878" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632644651432639875" type="Labeled" style="Bezier" label="digit" />
      <linkto id="632644651432639880" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639878" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639878" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="242" y="658">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632644651432639879" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="935.4795" y="139">
      <linkto id="632644651432639806" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632644651432639880" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="242.479492" y="883" />
    <node type="Label" id="632644651432639881" text="Z" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="355.4795" y="882" />
    <node type="Action" id="632644651432639882" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="587.4795" y="864" mx="640" my="880">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632644651432643523" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">ExtensionAudio</ap>
        <ap name="Prompt3" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">NoActiveCallAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">extension</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"OnGatherDigits_Complete: UserData: " + userData + " - playing no active call message."
</log>
      </Properties>
    </node>
    <node type="Action" id="632644651432639886" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="509.4795" y="779">
      <linkto id="632644651432639887" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639871" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Query" type="csharp">"SELECT * FROM rec_with_barge WHERE line_id='" + digits + "'" + " ORDER BY rec_with_barge_id DESC"</ap>
        <ap name="Name" type="literal">RecordingWithBarge</ap>
        <rd field="ResultSet">queryResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632644651432639887" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="509.4795" y="875">
      <linkto id="632644651432639888" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639888" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="509.4795" y="976">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632644651432639890" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="778.4795" y="777">
      <linkto id="632644651432639892" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639893" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="ReceiveOnly" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Label" id="632644651432639891" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="437.4795" y="874">
      <linkto id="632644651432639887" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632644651432639892" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="780.4795" y="880" />
    <node type="Action" id="632644651432639893" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="846.4795" y="762" mx="921" my="778">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632605817629675007" />
        <item text="OnGatherDigits_Failed" treenode="632605817629675012" />
      </items>
      <linkto id="632644651432639901" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="variable">g_recordingDigit</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">record</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639901" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1055.9502" y="777">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632644651432639902" text="R" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="147" y="1274">
      <linkto id="632644651432639903" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432639903" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="236" y="1274">
      <linkto id="632644651432639904" type="Labeled" style="Bezier" label="userstop" />
      <linkto id="632644651432639906" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639907" type="Labeled" style="Bezier" label="digit" />
      <linkto id="632644651432639904" type="Labeled" style="Bezier" label="autostop" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">termCond</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639904" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="235" y="1144">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632644651432639905" text="Y" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="916" y="697">
      <linkto id="632644651432639893" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632644651432639906" text="Y" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="237" y="1409.5" />
    <node type="Action" id="632644651432639907" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="424" y="1273">
      <linkto id="632644651432639910" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639942" type="Labeled" style="Bezier" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639910" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="424" y="1373">
      <linkto id="632644651432639931" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_recordingConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639926" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="376" y="1531" mx="429" my="1547">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632644651432639929" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">RecordNotifyAudio</ap>
        <ap name="Prompt1" type="variable">RecordNotifyAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">record</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639929" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="1685">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632644651432639931" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="424" y="1459">
      <linkto id="632644651432639926" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <rd field="ResultData">g_recording</rd>
      </Properties>
    </node>
    <node type="Action" id="632644651432639942" name="Record" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="507" y="1257" mx="567" my="1273">
      <items count="2">
        <item text="OnRecord_Complete" treenode="632644651432639936" />
        <item text="OnRecord_Failed" treenode="632644651432639941" />
      </items>
      <linkto id="632644651432639931" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639959" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="AudioFileFormat" type="literal">wav</ap>
        <ap name="Expires" type="literal">0</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="ConnectionId">g_recordingConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632644651432639945" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="784" y="1259" mx="837" my="1275">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632644651432639948" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="variable">RecordNotifyAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">record</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639948" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="959" y="1275">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632644651432639959" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="701" y="1274">
      <linkto id="632644651432639945" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">true</ap>
        <rd field="ResultData">g_recording</rd>
      </Properties>
    </node>
    <node type="Action" id="632644651432643523" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="636.4209" y="1035">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632605817629675078" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632605817629675079" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">termCond</Properties>
    </node>
    <node type="Variable" id="632605881876499377" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632644651432639889" name="queryResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">queryResult</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632605817629675011" treenode="632605817629675012" appnode="632605817629675009" handlerfor="632605817629675008">
    <node type="Start" id="632605817629675011" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="75" y="224">
      <linkto id="632606225558163509" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632606225558163509" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="285" y="224">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632608290328114534" text="Needs error handling." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="297" y="114" />
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632605770032726822" treenode="632605770032726823" appnode="632605770032726820" handlerfor="632605770032726819">
    <node type="Start" id="632605770032726822" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="54" y="275">
      <linkto id="632644651432640803" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632606225558163532" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="347.825867" y="259" mx="385" my="275">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632606225558163533" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">onRemoteHangup: Calling Exit function</log>
      </Properties>
    </node>
    <node type="Action" id="632606225558163533" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="382" y="403">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632644651432640803" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="276">
      <linkto id="632646266605191610" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <ap name="Value2" type="csharp">true</ap>
        <rd field="ResultData">g_isCallerAnswered</rd>
        <rd field="ResultData2">g_exit</rd>
      </Properties>
    </node>
    <node type="Action" id="632646266605191610" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="230" y="276">
      <linkto id="632606225558163532" type="Labeled" style="Bezier" label="false" />
      <linkto id="632646266605191611" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_recording</ap>
      </Properties>
    </node>
    <node type="Action" id="632646266605191611" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="230" y="404">
      <linkto id="632606225558163533" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632606225558163532" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_recordingConnectionId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotDigits" startnode="632605770032726827" treenode="632605770032726828" appnode="632605770032726825" handlerfor="632605770032726824">
    <node type="Start" id="632605770032726827" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="270">
      <linkto id="632605809299934190" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605809299934190" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="103" y="270">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632605770032726832" treenode="632605770032726833" appnode="632605770032726830" handlerfor="632605770032726829">
    <node type="Start" id="632605770032726832" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="45" y="160">
      <linkto id="632605809299934191" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605809299934191" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="104" y="158">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632605770032726837" treenode="632605770032726838" appnode="632605770032726835" handlerfor="632605770032726834">
    <node type="Start" id="632605770032726837" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="81">
      <linkto id="632605809299934188" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605809299934188" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="110" y="81">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632605770032726842" treenode="632605770032726843" appnode="632605770032726840" handlerfor="632605770032726839">
    <node type="Start" id="632605770032726842" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="119">
      <linkto id="632605809299934189" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605809299934189" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="114" y="119">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Complete" startnode="632644651432639935" treenode="632644651432639936" appnode="632644651432639933" handlerfor="632644651432639932">
    <node type="Start" id="632644651432639935" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="167">
      <linkto id="632646266605191608" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432640801" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="400" y="166">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="csharp">"OnRecord_Complete: Recording was saved to file: " + filename</log>
      </Properties>
    </node>
    <node type="Action" id="632646266605191608" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136.048828" y="167">
      <linkto id="632646266605191614" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <rd field="ResultData">g_recording</rd>
      </Properties>
    </node>
    <node type="Action" id="632646266605191614" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="252" y="166">
      <linkto id="632644651432640801" type="Labeled" style="Bezier" label="false" />
      <linkto id="632646266605191616" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632646266605191616" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="218.825836" y="252" mx="256" my="268">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632644651432640801" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Variable" id="632644651432640800" name="filename" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Filename" defaultInitWith="NONE" refType="reference" name="Metreos.MediaControl.Record_Complete">filename</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRecord_Failed" startnode="632644651432639940" treenode="632644651432639941" appnode="632644651432639938" handlerfor="632644651432639937">
    <node type="Start" id="632644651432639940" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="61" y="75">
      <linkto id="632646266605191607" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432640802" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="494" y="79">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632646266605191607" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="177" y="76">
      <linkto id="632646266605191617" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">false</ap>
        <rd field="ResultData">g_recording</rd>
      </Properties>
    </node>
    <node type="Action" id="632646266605191617" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="324.174164" y="78">
      <linkto id="632646266605191618" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432640802" type="Labeled" style="Bezier" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632646266605191618" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="290" y="170" mx="327" my="186">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632644651432640802" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestAuthentication" startnode="632605817629675034" treenode="632605817629675035" appnode="632605817629675032" handlerfor="632644651432639937">
    <node type="Start" id="632605817629675034" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="122" y="251">
      <linkto id="632605817629675041" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605817629675036" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="657" y="78" mx="710" my="94">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632605817629675071" type="Labeled" style="Bezier" label="default" />
      <linkto id="632605817629675076" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">RequestAuthAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">auth_req</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"RequestAuthentication: Playing authentication request to connectionId '" + g_connectionId + "' associated with callId: " + g_callId</log>
        <log condition="default" on="true" level="Warning" type="csharp">"RequestAuthentication: Authentication request Play command FAILED. ConnectionId '" + g_connectionId + "' associated with callId: " + g_callId</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675041" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="206" y="251">
      <linkto id="632605817629675044" type="Labeled" style="Bezier" label="default" />
      <linkto id="632605817629675066" type="Labeled" style="Bezier" label="equal" />
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
      <linkto id="632605817629675041" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">failureCounter + 1</ap>
        <rd field="ResultData">failureCounter</rd>
      </Properties>
    </node>
    <node type="Action" id="632605817629675053" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="243" y="544">
      <linkto id="632605817629675042" type="Labeled" style="Bezier" label="default" />
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
      <linkto id="632605817629675077" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632605817629675075" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">auth_req</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"RequestAuthentication: GatherDigits to obtain authentication code on connectionId '" + g_connectionId + "' associated with callId: " + g_callId</log>
        <log condition="default" on="true" level="Warning" type="csharp">"RequestAuthentication: GatherDigits to obtain authentication code FAILED on connectionId '" + g_connectionId + "' associated with callId: " + g_callId
</log>
      </Properties>
    </node>
    <node type="Action" id="632605817629675066" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="327" y="250">
      <linkto id="632605817629675067" type="Labeled" style="Bezier" label="play" />
      <linkto id="632605817629675068" type="Labeled" style="Bezier" label="digits" />
      <linkto id="632605881876499385" type="Labeled" style="Bezier" label="auth_fail" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Label" id="632605817629675067" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="450" y="124" />
    <node type="Label" id="632605817629675068" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="448" y="250" />
    <node type="Label" id="632605817629675069" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="597" y="94">
      <linkto id="632605817629675036" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632605817629675070" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="206" y="91">
      <linkto id="632605817629675048" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632605817629675071" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="707.4707" y="229" />
    <node type="Label" id="632605817629675072" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="595.4707" y="359">
      <linkto id="632605817629675060" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632605817629675074" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="152.4707" y="544">
      <linkto id="632605817629675053" type="Basic" style="Bezier" />
    </node>
    <node type="Label" id="632605817629675075" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="703.4707" y="506" />
    <node type="Label" id="632605817629675076" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="828.4707" y="93" />
    <node type="Label" id="632605817629675077" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="837.4707" y="358" />
    <node type="Label" id="632605881876499385" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="451" y="392" />
    <node type="Label" id="632605881876499386" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="590" y="661">
      <linkto id="632605881876499387" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605881876499387" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="667" y="661">
      <linkto id="632605881876499388" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="csharp">g_failedLogins + 1</ap>
        <rd field="ResultData">g_failedLogins</rd>
      </Properties>
    </node>
    <node type="Action" id="632605881876499388" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="777" y="661">
      <linkto id="632605881876499391" type="Labeled" style="Bezier" label="equal" />
      <linkto id="632605881876499394" type="Labeled" style="Bezier" label="default" />
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
      <linkto id="632605881876499397" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632605881876499398" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">RequestAuthAudio</ap>
        <ap name="Prompt3" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">AuthFailedAudio</ap>
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
      <linkto id="632605881876499399" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632605881876499401" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">GoodByeAudio</ap>
        <ap name="Prompt1" type="variable">AuthFailedAudio</ap>
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
      <linkto id="632605881876499402" type="Labeled" style="Bezier" label="default" />
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
  <canvas type="Function" name="Exit" startnode="632605817629675023" treenode="632605817629675024" appnode="632605817629675021" handlerfor="632644651432639937">
    <node type="Start" id="632605817629675023" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="205">
      <linkto id="632605817629675059" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632605817629675051" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="227.3529" y="318">
      <linkto id="632606225558163534" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632605817629675059" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="150" y="206">
      <linkto id="632605817629675051" type="Labeled" style="Bezier" label="true" />
      <linkto id="632606225558163534" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_isCallerAnswered</ap>
      </Properties>
    </node>
    <node type="Action" id="632606225558163534" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="320.82605" y="206">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="GetExtensionToMonitor" startnode="632644651432639809" treenode="632644651432639810" appnode="632644651432639807" handlerfor="632644651432639937">
    <node type="Start" id="632644651432639809" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="255">
      <linkto id="632644651432639812" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432639812" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="153" y="255">
      <linkto id="632644651432639813" type="Labeled" style="Bezier" label="play" />
      <linkto id="632644651432639855" type="Labeled" style="Bezier" label="digits" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Label" id="632644651432639813" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="232" y="170" />
    <node type="Label" id="632644651432639814" text="P" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="522" y="75">
      <linkto id="632644651432639815" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432639815" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="620" y="58" mx="673" my="74">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632605770032726849" />
        <item text="OnPlay_Failed" treenode="632605770032726854" />
      </items>
      <linkto id="632644651432639816" type="Labeled" style="Bezier" label="Success" />
      <linkto id="632644651432639817" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt2" type="variable">PoundSignAudio</ap>
        <ap name="Prompt1" type="variable">ExtensionAudio</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="UserData" type="literal">extension</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639816" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="821.0117" y="73">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639817" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="637.2965" y="217" mx="674" my="233">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632644651432639816" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Label" id="632644651432639855" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="232" y="361" />
    <node type="Label" id="632644651432639856" text="D" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="537" y="426">
      <linkto id="632644651432639857" type="Basic" style="Bezier" />
    </node>
    <node type="Action" id="632644651432639857" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="573" y="410" mx="647" my="426">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632605817629675007" />
        <item text="OnGatherDigits_Failed" treenode="632605817629675012" />
      </items>
      <linkto id="632644651432639860" type="Labeled" style="Bezier" label="default" />
      <linkto id="632644651432639865" type="Labeled" style="Bezier" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">extension</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639860" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="612.825867" y="566" mx="650" my="582">
      <items count="1">
        <item text="Exit" />
      </items>
      <linkto id="632644651432639865" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">Exit</ap>
      </Properties>
    </node>
    <node type="Action" id="632644651432639865" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="880.4707" y="426">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Variable" id="632644651432639811" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Action" refType="reference">action</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>
