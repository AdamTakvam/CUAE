<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632555867427675818" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632555867427675815" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632555867427675814" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.OutgoingCallControlPanel.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675867" level="2" text="Metreos.Providers.FunctionalTest.Event: OnMakeCall">
        <node type="function" name="OnMakeCall" id="632555867427675864" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675863" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.OutgoingCallControlPanel.script1.E_MakeCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675872" level="2" text="Metreos.Providers.FunctionalTest.Event: OnConsult">
        <node type="function" name="OnConsult" id="632555867427675869" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675868" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.OutgoingCallControlPanel.script1.E_Consult</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675877" level="2" text="Metreos.Providers.FunctionalTest.Event: OnBlind">
        <node type="function" name="OnBlind" id="632555867427675874" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675873" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.OutgoingCallControlPanel.script1.E_Blind</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675882" level="2" text="Metreos.Providers.FunctionalTest.Event: OnPlay">
        <node type="function" name="OnPlay" id="632555867427675879" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675878" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.OutgoingCallControlPanel.script1.E_Play</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675887" level="2" text="Metreos.Providers.FunctionalTest.Event: OnHangup">
        <node type="function" name="OnHangup" id="632555867427675884" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675883" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.OutgoingCallControlPanel.script1.E_Hangup</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675951" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632555867427675948" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632555867427675947" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632909098592656699" actid="632556676095333283" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675956" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632555867427675953" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632555867427675952" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675961" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632555867427675958" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632555867427675957" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675966" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632555867427675963" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632555867427675962" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427676002" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632555867427675999" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632555867427675998" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632909098592656724" actid="632555867427676008" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427676007" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632555867427676004" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632555867427676003" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632909098592656725" actid="632555867427676008" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632556676095333277" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632556676095333274" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632556676095333273" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632909098592656697" actid="632556676095333283" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632556676095333282" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632556676095333279" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632556676095333278" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632909098592656698" actid="632556676095333283" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632842561467849274" level="2" text="Metreos.MediaControl.Play_Complete: OnWaitForMediaPlay_Complete">
        <node type="function" name="OnWaitForMediaPlay_Complete" id="632842561467849269" path="Metreos.StockTools" />
        <node type="event" name="OnWaitForMediaPlay_Complete" id="632842561467849272" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632909098592656760" actid="632842561467849266" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632842561467849280" level="2" text="Metreos.MediaControl.Play_Failed: OnWaitForMediaPlay_Failed">
        <node type="function" name="OnWaitForMediaPlay_Failed" id="632842561467849275" path="Metreos.StockTools" />
        <node type="event" name="OnWaitForMediaPlay_Failed" id="632842561467849278" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632909098592656761" actid="632842561467849266" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_MakeCall" id="632909098592656671" vid="632555867427675827">
        <Properties type="String" initWith="S_MakeCall">S_MakeCall</Properties>
      </treenode>
      <treenode text="S_Consult" id="632909098592656673" vid="632555867427675829">
        <Properties type="String" initWith="S_Consult">S_Consult</Properties>
      </treenode>
      <treenode text="S_Blind" id="632909098592656675" vid="632555867427675831">
        <Properties type="String" initWith="S_Blind">S_Blind</Properties>
      </treenode>
      <treenode text="S_Play" id="632909098592656677" vid="632555867427675833">
        <Properties type="String" initWith="S_Play">S_Play</Properties>
      </treenode>
      <treenode text="S_Hold" id="632909098592656679" vid="632555867427675835">
        <Properties type="String" initWith="S_Hold">S_Hold</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632909098592656681" vid="632555867427675839">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632909098592656683" vid="632555867427675939">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_incomingConnectionId" id="632909098592656685" vid="632555867427675943">
        <Properties type="String">g_incomingConnectionId</Properties>
      </treenode>
      <treenode text="S_Started" id="632909098592656687" vid="632556676095333271">
        <Properties type="String" initWith="S_Started">S_Started</Properties>
      </treenode>
      <treenode text="g_testWaitForMedia" id="632909098592656689" vid="632840004067146846">
        <Properties type="Bool" defaultInitWith="false">g_testWaitForMedia</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632555867427675817" treenode="632555867427675818" appnode="632555867427675815" handlerfor="632555867427675814">
    <node type="Start" id="632555867427675817" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="371">
      <linkto id="632555867427675837" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675837" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="255" y="372">
      <linkto id="632555867427675838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Started</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675838" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="373">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall" activetab="true" startnode="632555867427675866" treenode="632555867427675867" appnode="632555867427675864" handlerfor="632555867427675863">
    <node type="Start" id="632555867427675866" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="368">
      <linkto id="632840023032966757" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675945" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="649" y="458">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555867427675946" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="654" y="261">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632556676095333283" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="217" y="350" mx="283" my="366">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632556676095333277" />
        <item text="OnMakeCall_Failed" treenode="632556676095333282" />
        <item text="OnRemoteHangup" treenode="632555867427675951" />
      </items>
      <linkto id="632556676095333288" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632556676095333289" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="variable">to</ap>
        <ap name="From" type="literal">Test</ap>
        <ap name="WaitForMedia" type="variable">WaitForMediaState</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"DEBUG: MakeCall - WaitForMediaState - " + WaitForMediaState</log>
      </Properties>
    </node>
    <node type="Action" id="632556676095333288" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="464" y="261">
      <linkto id="632555867427675946" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="literal">Make call started successfully</ap>
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_MakeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632556676095333289" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="469" y="461">
      <linkto id="632555867427675945" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="literal">Make call not started successfully</ap>
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MakeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632840023032966757" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140" y="354">
      <linkto id="632556676095333283" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">testWaitForMedia</ap>
        <rd field="ResultData">g_testWaitForMedia</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"DEBUG - Assign: testWaitForMedia " + testWaitForMedia</log>
      </Properties>
    </node>
    <node type="Variable" id="632556676095333287" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632839971543459245" name="WaitForMediaState" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="WaitForMediaState" defaultInitWith="TxRx" refType="reference">WaitForMediaState</Properties>
    </node>
    <node type="Variable" id="632840023032966756" name="testWaitForMedia" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="testWaitForMedia" refType="reference">testWaitForMedia</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnConsult" startnode="632555867427675871" treenode="632555867427675872" appnode="632555867427675869" handlerfor="632555867427675868">
    <node type="Start" id="632555867427675871" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="342">
      <linkto id="632555867427675982" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675982" name="BeginConsultationTransfer" class="MaxActionNode" group="" path="Metreos.CallControl" x="248" y="344">
      <linkto id="632555867427675986" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555867427675988" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="To" type="variable">to</ap>
        <rd field="TransferCallId">xferCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632555867427675985" name="EndConsultationTransfer" class="MaxActionNode" group="" path="Metreos.CallControl" x="550" y="344">
      <linkto id="632555867427675988" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632555867427675987" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="TransferCallId" type="variable">xferCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675986" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="409" y="344">
      <linkto id="632555867427675985" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">3000</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675987" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="667" y="343">
      <linkto id="632555867427675990" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Consult</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675988" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="358" y="532">
      <linkto id="632555867427675990" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Consult</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675990" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="669" y="530">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632555867427675983" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
    <node type="Variable" id="632555867427675984" name="xferCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">xferCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnBlind" startnode="632555867427675876" treenode="632555867427675877" appnode="632555867427675874" handlerfor="632555867427675873">
    <node type="Start" id="632555867427675876" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="360">
      <linkto id="632555867427675996" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675992" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="219.610046" y="490">
      <linkto id="632555867427675997" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_Blind</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675993" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="367.610046" y="362">
      <linkto id="632555867427675997" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_Blind</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675996" name="BlindTransfer" class="MaxActionNode" group="" path="Metreos.CallControl" x="219" y="359">
      <linkto id="632555867427675993" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555867427675992" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="To" type="variable">to</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675997" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="367" y="491">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632555867427675991" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="to" refType="reference">to</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay" startnode="632555867427675881" treenode="632555867427675882" appnode="632555867427675879" handlerfor="632555867427675878">
    <node type="Start" id="632555867427675881" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="346">
      <linkto id="632555867427676008" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427676008" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="254" y="335" mx="307" my="351">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632555867427676002" />
        <item text="OnPlay_Failed" treenode="632555867427676007" />
      </items>
      <linkto id="632555867427676012" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632555867427676013" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <log condition="entry" on="true" level="Verbose" type="csharp">"** DEBUG: g_incomingConnectionId - " + g_incomingConnectionId</log>
      </Properties>
    </node>
    <node type="Action" id="632555867427676012" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="510" y="354">
      <linkto id="632555867427676015" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="message" type="literal">Play started successfully</ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427676013" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="305" y="534">
      <linkto id="632555867427676016" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="message" type="literal">Play did not start successfully</ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427676015" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="619" y="358">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555867427676016" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="307" y="645">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnHangup" startnode="632555867427675886" treenode="632555867427675887" appnode="632555867427675884" handlerfor="632555867427675883">
    <node type="Start" id="632555867427675886" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="350">
      <linkto id="632555867427675980" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675980" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="271" y="346">
      <linkto id="632555867427675981" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675981" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="510" y="352">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632555867427675950" treenode="632555867427675951" appnode="632555867427675948" handlerfor="632555867427675947">
    <node type="Start" id="632555867427675950" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="341">
      <linkto id="632555867427675968" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675968" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="295" y="344">
      <linkto id="632555867427675969" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675969" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="483" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632555867427675967" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632555867427675955" treenode="632555867427675956" appnode="632555867427675953" handlerfor="632555867427675952">
    <node type="Start" id="632555867427675955" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632555867427675970" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675970" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="379" y="265">
      <linkto id="632555867427675971" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="literal">StartTx</ap>
        <ap name="signalName" type="variable">S_Hold</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675971" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="737" y="270">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632555867427675960" treenode="632555867427675961" appnode="632555867427675958" handlerfor="632555867427675957">
    <node type="Start" id="632555867427675960" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="175">
      <linkto id="632555867427675972" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675972" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="255.610046" y="172">
      <linkto id="632555867427675973" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="literal">StopTx</ap>
        <ap name="signalName" type="variable">S_Hold</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675973" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528.610046" y="166">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632555867427675965" treenode="632555867427675966" appnode="632555867427675963" handlerfor="632555867427675962">
    <node type="Start" id="632555867427675965" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="61">
      <linkto id="632555867427675976" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675976" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="255.610046" y="62">
      <linkto id="632555867427675977" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="literal">StartRx</ap>
        <ap name="signalName" type="variable">S_Hold</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675977" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="433.610046" y="60">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632555867427676001" treenode="632555867427676002" appnode="632555867427675999" handlerfor="632555867427675998">
    <node type="Start" id="632555867427676001" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="369">
      <linkto id="632555867427676018" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427676018" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="255.610046" y="368">
      <linkto id="632555867427676022" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="message" type="csharp">"Play completed successfully, term cond is '" + termCond + "'";</ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427676022" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="420" y="364">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632555867427676017" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632555867427676006" treenode="632555867427676007" appnode="632555867427676004" handlerfor="632555867427676003">
    <node type="Start" id="632555867427676006" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="344">
      <linkto id="632555867427676023" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427676023" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="206.610016" y="344">
      <linkto id="632555867427676025" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="message" type="csharp">"Play did not complete successfully, result code is '" + termCond  + "'" </ap>
        <ap name="signalName" type="variable">S_Play</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427676025" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="414" y="342">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632555867427676026" name="termCond" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ResultCode" refType="reference" name="Metreos.MediaControl.Play_Failed">termCond</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632556676095333276" treenode="632556676095333277" appnode="632556676095333274" handlerfor="632556676095333273">
    <node type="Start" id="632556676095333276" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="101">
      <linkto id="632556676095333301" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632556676095333292" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="292" y="327">
      <linkto id="632556676095333293" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="literal">Make Call completed successfully</ap>
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_MakeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632556676095333293" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="552" y="329">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632556676095333301" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="113" y="101">
      <linkto id="632840004067146848" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">connectionId</ap>
        <rd field="ResultData">g_incomingConnectionId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"DEBUG: connectionId value: " + connectionId</log>
      </Properties>
    </node>
    <node type="Action" id="632840004067146848" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="287" y="102">
      <linkto id="632556676095333292" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632842561467849266" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_testWaitForMedia</ap>
      </Properties>
    </node>
    <node type="Action" id="632842561467849266" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="460.5879" y="91" mx="551" my="107">
      <items count="2">
        <item text="OnWaitForMediaPlay_Complete" treenode="632842561467849274" />
        <item text="OnWaitForMediaPlay_Failed" treenode="632842561467849280" />
      </items>
      <linkto id="632556676095333293" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">doorbell.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632556676095333300" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632556676095333281" treenode="632556676095333282" appnode="632556676095333279" handlerfor="632556676095333278">
    <node type="Start" id="632556676095333281" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="374">
      <linkto id="632556676095333294" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632556676095333294" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="226.61" y="379">
      <linkto id="632556676095333299" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="message" type="csharp">"Make Call did not completed successfully, end reason is '" + endreason + "'"</ap>
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_MakeCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632556676095333299" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="463" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632556676095333298" name="endreason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="EndReason" refType="reference" name="Metreos.CallControl.MakeCall_Failed">endreason</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnWaitForMediaPlay_Complete" startnode="632842561467849271" treenode="632842561467849274" appnode="632842561467849269" handlerfor="632842561467849272">
    <node type="Start" id="632842561467849271" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="116">
      <linkto id="632842561467849281" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632842561467849281" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="242" y="117">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnWaitForMediaPlay_Failed" startnode="632842561467849277" treenode="632842561467849280" appnode="632842561467849275" handlerfor="632842561467849278">
    <node type="Start" id="632842561467849277" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="133">
      <linkto id="632842561467849282" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632842561467849282" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="258" y="129">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>