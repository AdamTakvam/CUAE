<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632555867427675818" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632555867427675815" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632555867427675814" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675867" level="2" text="Metreos.Providers.FunctionalTest.Event: OnIncomingCallResponse">
        <node type="function" name="OnIncomingCallResponse" id="632555867427675864" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675863" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.IncomingCallControlPanel.script1.E_IncomingCall</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675872" level="2" text="Metreos.Providers.FunctionalTest.Event: OnConsult">
        <node type="function" name="OnConsult" id="632555867427675869" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675868" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.IncomingCallControlPanel.script1.E_Consult</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675877" level="2" text="Metreos.Providers.FunctionalTest.Event: OnBlind">
        <node type="function" name="OnBlind" id="632555867427675874" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675873" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.IncomingCallControlPanel.script1.E_Blind</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675882" level="2" text="Metreos.Providers.FunctionalTest.Event: OnPlay">
        <node type="function" name="OnPlay" id="632555867427675879" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675878" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.IncomingCallControlPanel.script1.E_Play</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675887" level="2" text="Metreos.Providers.FunctionalTest.Event: OnHangup">
        <node type="function" name="OnHangup" id="632555867427675884" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632555867427675883" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">Provider.IncomingCallControlPanel.script1.E_Hangup</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427675951" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632555867427675948" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632555867427675947" path="Metreos.CallControl.RemoteHangup" />
        <references />
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
          <ref id="632909098592656898" actid="632555867427676008" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632555867427676007" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632555867427676004" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632555867427676003" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632909098592656899" actid="632555867427676008" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632842524038257100" level="2" text="Metreos.MediaControl.Play_Complete: OnWaitForMediaPlay_Complete">
        <node type="function" name="OnWaitForMediaPlay_Complete" id="632842524038257095" path="Metreos.StockTools" />
        <node type="event" name="OnWaitForMediaPlay_Complete" id="632842524038257098" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632909098592656871" actid="632842524038257092" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632842524038257106" level="2" text="Metreos.MediaControl.Play_Failed: OnWaitForMediaPlay_Failed">
        <node type="function" name="OnWaitForMediaPlay_Failed" id="632842524038257101" path="Metreos.StockTools" />
        <node type="event" name="OnWaitForMediaPlay_Failed" id="632842524038257104" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632909098592656872" actid="632842524038257092" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_IncomingCall" id="632909098592656830" vid="632555867427675827">
        <Properties type="String" initWith="S_IncomingCall">S_IncomingCall</Properties>
      </treenode>
      <treenode text="S_Consult" id="632909098592656832" vid="632555867427675829">
        <Properties type="String" initWith="S_Consult">S_Consult</Properties>
      </treenode>
      <treenode text="S_Blind" id="632909098592656834" vid="632555867427675831">
        <Properties type="String" initWith="S_Blind">S_Blind</Properties>
      </treenode>
      <treenode text="S_Play" id="632909098592656836" vid="632555867427675833">
        <Properties type="String" initWith="S_Play">S_Play</Properties>
      </treenode>
      <treenode text="S_Hold" id="632909098592656838" vid="632555867427675835">
        <Properties type="String" initWith="S_Hold">S_Hold</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632909098592656840" vid="632555867427675839">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632909098592656842" vid="632555867427675939">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_incomingConnectionId" id="632909098592656844" vid="632555867427675943">
        <Properties type="String">g_incomingConnectionId</Properties>
      </treenode>
      <treenode text="g_hairpin" id="632909098592656846" vid="632593531028865709">
        <Properties type="Bool" defaultInitWith="false">g_hairpin</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632909098592656848" vid="632593531028865714">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_testWaitForMedia" id="632909098592656850" vid="632840626488232992">
        <Properties type="Bool" defaultInitWith="false">g_testWaitForMedia</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632555867427675817" treenode="632555867427675818" appnode="632555867427675815" handlerfor="632555867427675814">
    <node type="Start" id="632555867427675817" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="371">
      <linkto id="632593555188014633" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675837" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="234" y="374">
      <linkto id="632555867427675838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_IncomingCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675838" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="373">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632593555188014633" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="132" y="376">
      <linkto id="632555867427675837" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_incomingCallId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632555867427675942" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnIncomingCallResponse" activetab="true" startnode="632555867427675866" treenode="632555867427675867" appnode="632555867427675864" handlerfor="632555867427675863">
    <node type="Start" id="632555867427675866" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="368">
      <linkto id="632593555188014635" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632555867427675936" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="205" y="368">
      <linkto id="632555867427675937" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632555867427675938" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">reject</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675937" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="460" y="267">
      <linkto id="632593531028865712" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="WaitForMedia" type="variable">WaitForMediaState</ap>
        <rd field="ConnectionId">g_incomingConnectionId</rd>
        <rd field="MediaTxIP">txIp</rd>
        <rd field="MediaTxPort">txPort</rd>
        <log condition="entry" on="true" level="Verbose" type="csharp">"DEBUG: AnswerCall - WaitForMediaState - " + WaitForMediaState</log>
      </Properties>
    </node>
    <node type="Action" id="632555867427675938" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="458" y="461">
      <linkto id="632555867427675945" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632555867427675945" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="649" y="458">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632555867427675946" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="967" y="171">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632593531028865712" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="577" y="264">
      <linkto id="632593531028865713" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632840626488232998" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_hairpin</ap>
      </Properties>
    </node>
    <node type="Action" id="632593531028865713" name="CreateConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="579" y="100">
      <linkto id="632593531028865718" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632593531028865718" name="CreateConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="720" y="99">
      <linkto id="632593531028865720" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="MediaTxIP" type="variable">txIp</ap>
        <ap name="MediaTxPort" type="variable">txPort</ap>
        <rd field="ConnectionId">connection</rd>
      </Properties>
    </node>
    <node type="Action" id="632593531028865720" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="849" y="98">
      <linkto id="632555867427675946" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">connection</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
    <node type="Action" id="632593555188014635" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="117" y="365">
      <linkto id="632555867427675936" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">hairpin</ap>
        <rd field="ResultData">g_hairpin</rd>
      </Properties>
    </node>
    <node type="Comment" id="632761261646860581" text=" 2/20/06: WKY| SFT-25 &quot;FTF does not support Hairpin Connections&quot;&#xD;&#xA;This feature is not available and will be initialized as false until&#xD;&#xA; FTF can support it." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="379" y="378" />
    <node type="Action" id="632840626488232994" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="752.735" y="265">
      <linkto id="632555867427675946" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632842524038257092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_testWaitForMedia</ap>
      </Properties>
    </node>
    <node type="Action" id="632840626488232998" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="667.7838" y="265">
      <linkto id="632840626488232994" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">testWaitForMedia</ap>
        <rd field="ResultData">g_testWaitForMedia</rd>
        <log condition="exit" on="true" level="Verbose" type="csharp">"DEBUG Assign testWaitForMedia: " + testWaitForMedia</log>
      </Properties>
    </node>
    <node type="Action" id="632842524038257092" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="785.0586" y="250" mx="875" my="266">
      <items count="2">
        <item text="OnWaitForMediaPlay_Complete" treenode="632842524038257100" />
        <item text="OnWaitForMediaPlay_Failed" treenode="632842524038257106" />
      </items>
      <linkto id="632555867427675946" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">doorbell.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Variable" id="632555867427675935" name="reject" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="reject" refType="reference">reject</Properties>
    </node>
    <node type="Variable" id="632593531028865716" name="txIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">txIp</Properties>
    </node>
    <node type="Variable" id="632593531028865717" name="txPort" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">txPort</Properties>
    </node>
    <node type="Variable" id="632593531028865719" name="connection" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connection</Properties>
    </node>
    <node type="Variable" id="632593555188014632" name="hairpin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="hairpin" refType="reference">hairpin</Properties>
    </node>
    <node type="Variable" id="632840626488232996" name="WaitForMediaState" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="WaitForMediaState" refType="reference">WaitForMediaState</Properties>
    </node>
    <node type="Variable" id="632840626488232997" name="testWaitForMedia" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
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
    <node type="Action" id="632555867427675993" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="399.610046" y="358">
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
    <node type="Action" id="632555867427675997" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="399" y="489">
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
      <linkto id="632593531028865721" type="Basic" style="Bezier" ortho="true" />
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
    <node type="Action" id="632593531028865721" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="144" y="354">
      <linkto id="632555867427675980" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632593531028865722" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_hairpin</ap>
      </Properties>
    </node>
    <node type="Action" id="632593531028865722" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="144" y="498">
      <linkto id="632555867427675980" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_conferenceId</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632555867427675950" treenode="632555867427675951" appnode="632555867427675948" handlerfor="632555867427675947">
    <node type="Start" id="632555867427675950" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="341">
      <linkto id="632593531028865723" type="Basic" style="Bezier" ortho="true" />
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
    <node type="Action" id="632593531028865723" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="167.076172" y="347">
      <linkto id="632593531028865724" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632555867427675968" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_hairpin</ap>
      </Properties>
    </node>
    <node type="Action" id="632593531028865724" name="DeleteConnection" class="MaxActionNode" group="" path="Metreos.MediaControl" x="167.076172" y="491">
      <linkto id="632555867427675968" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_conferenceId</ap>
      </Properties>
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
  <canvas type="Function" name="OnWaitForMediaPlay_Complete" startnode="632842524038257097" treenode="632842524038257100" appnode="632842524038257095" handlerfor="632842524038257098">
    <node type="Start" id="632842524038257097" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="97">
      <linkto id="632842524038257107" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632842524038257107" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="185" y="93">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnWaitForMediaPlay_Failed" startnode="632842524038257103" treenode="632842524038257106" appnode="632842524038257101" handlerfor="632842524038257104">
    <node type="Start" id="632842524038257103" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="53" y="161">
      <linkto id="632842524038257108" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632842524038257108" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="345" y="161">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>