<Application name="Listen" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Listen">
    <outline>
      <treenode type="evh" id="632497091239813542" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632497091239813539" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632497091239813538" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="To" type="literal">4331</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497091239813688" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632497091239813685" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632497091239813684" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632497091239815624" actid="632497091239813694" />
          <ref id="632497091239815643" actid="632497091239814342" />
          <ref id="632497091239815646" actid="632497091239814345" />
          <ref id="632497091239815651" actid="632497091239814352" />
          <ref id="632497091239815666" actid="632497091239814657" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632497091239813693" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632497091239813690" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632497091239813689" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632497091239815625" actid="632497091239813694" />
          <ref id="632497091239815644" actid="632497091239814342" />
          <ref id="632497091239815647" actid="632497091239814345" />
          <ref id="632497091239815652" actid="632497091239814352" />
          <ref id="632497091239815667" actid="632497091239814657" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632497091239813705" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632497091239813702" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632497091239813701" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632497091239815631" actid="632497091239813711" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632497091239813710" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632497091239813707" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632497091239813706" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632497091239815632" actid="632497091239813711" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632497091239814435" level="2" text="Metreos.Providers.Http.GotRequest: OnGotRequest">
        <node type="function" name="OnGotRequest" id="632497091239814432" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632497091239814431" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632497091239814667" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632497091239814664" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632497091239814663" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_incomingCallId" id="632497091239815616" vid="632497091239813680">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_incomingConnectionId" id="632497091239815618" vid="632497091239813682">
        <Properties type="String">g_incomingConnectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632497091239813541" treenode="632497091239813542" appnode="632497091239813539" handlerfor="632497091239813538">
    <node type="Start" id="632497091239813541" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="363">
      <linkto id="632497091239813673" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497091239813673" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="235.792313" y="363">
      <linkto id="632497091239813674" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632497091239813694" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <rd field="ConnectionId">g_incomingConnectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632497091239813674" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="235.792313" y="475">
      <linkto id="632497091239813675" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239813675" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="235.7923" y="587">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">Call rejected</log>
      </Properties>
    </node>
    <node type="Action" id="632497091239813694" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="380" y="348" mx="433" my="364">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632497091239813688" />
        <item text="OnPlay_Failed" treenode="632497091239813693" />
      </items>
      <linkto id="632497091239813697" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">rac_enter_extension.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">enter</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239813697" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="621" y="364">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632497091239813679" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" activetab="true" startnode="632497091239813687" treenode="632497091239813688" appnode="632497091239813685" handlerfor="632497091239813684">
    <node type="Start" id="632497091239813687" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="352">
      <linkto id="632497091239813700" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497091239813700" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="219" y="353">
      <linkto id="632497091239813711" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632497091239814661" type="Labeled" style="Bezier" ortho="true" label="end" />
      <linkto id="632497091239815671" type="Labeled" style="Bezier" ortho="true" label="done" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239813711" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="371" y="338" mx="445" my="354">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632497091239813705" />
        <item text="OnGatherDigits_Failed" treenode="632497091239813710" />
      </items>
      <linkto id="632497091239813882" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="TermCondMaxTime" type="literal">10000</ap>
        <ap name="TermCondMaxDigits" type="literal">5</ap>
        <ap name="TermCondDigit" type="literal">#</ap>
        <ap name="UserData" type="literal">enter</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239813882" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="611" y="356">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632497091239814661" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="220" y="494">
      <linkto id="632497091239814662" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814662" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="225" y="606">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632497091239815671" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="412" y="185">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632497091239813698" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">terminationCondition</Properties>
    </node>
    <node type="Variable" id="632497091239813699" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632497091239813692" treenode="632497091239813693" appnode="632497091239813690" handlerfor="632497091239813689">
    <node type="Start" id="632497091239813692" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632497091239814430" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497091239814430" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="241">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632497091239813704" treenode="632497091239813705" appnode="632497091239813702" handlerfor="632497091239813701">
    <node type="Start" id="632497091239813704" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="358">
      <linkto id="632497091239814341" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497091239814341" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="232" y="359">
      <linkto id="632497091239814342" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632497091239814348" type="Labeled" style="Bezier" label="digit" />
      <linkto id="632497091239814348" type="Labeled" style="Bezier" label="maxdigits" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814342" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="368" y="230" mx="421" my="246">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632497091239813688" />
        <item text="OnPlay_Failed" treenode="632497091239813693" />
      </items>
      <linkto id="632497091239814356" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">rac_enter_extension.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">enter</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814345" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="763" y="370" mx="816" my="386">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632497091239813688" />
        <item text="OnPlay_Failed" treenode="632497091239813693" />
      </items>
      <linkto id="632497091239814357" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Prompt1" type="literal">rac_connected.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">done</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814348" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="324" y="482">
      <linkto id="632497091239814351" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="csharp">"select conferenceid from extensiontable where sccpextension='" + digits.Substring(0, digits.Length - 1) + "'"</ap>
        <ap name="Name" type="literal">RemoteAgentConcept</ap>
        <rd field="ResultSet">data</rd>
      </Properties>
    </node>
    <node type="Action" id="632497091239814351" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="453" y="479">
      <linkto id="632497091239814352" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632497091239814359" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">data.Rows.Count == 1</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814352" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="593" y="608" mx="646" my="624">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632497091239813688" />
        <item text="OnPlay_Failed" treenode="632497091239813693" />
      </items>
      <linkto id="632497091239814358" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="Prompt1" type="literal">not_in_use.wav</ap>
        <ap name="Prompt2" type="literal">rac_enter_extension.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">enter</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814355" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="674" y="384">
      <linkto id="632497091239814345" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ReceiveOnly" type="literal">true</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="ConferenceId" type="csharp">data.Rows[0][0]</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814356" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="592" y="246">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632497091239814357" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="978" y="386">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632497091239814358" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="762" y="623">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632497091239814359" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="483" y="385">
      <linkto id="632497091239814355" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Command" type="csharp">"update extensiontable set appInstanceId='" + routingGuid + "'"</ap>
        <ap name="Name" type="literal">RemoteAgentConcept</ap>
      </Properties>
    </node>
    <node type="Variable" id="632497091239814340" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">terminationCondition</Properties>
    </node>
    <node type="Variable" id="632497091239814349" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
    <node type="Variable" id="632497091239814350" name="data" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">data</Properties>
    </node>
    <node type="Variable" id="632497091239814360" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632497091239813709" treenode="632497091239813710" appnode="632497091239813707" handlerfor="632497091239813706">
    <node type="Start" id="632497091239813709" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632497091239814429" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497091239814429" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="524" y="267">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGotRequest" startnode="632497091239814434" treenode="632497091239814435" appnode="632497091239814432" handlerfor="632497091239814431">
    <node type="Start" id="632497091239814434" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="383">
      <linkto id="632497091239814657" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497091239814657" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="228" y="368" mx="281" my="384">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632497091239813688" />
        <item text="OnPlay_Failed" treenode="632497091239813693" />
      </items>
      <linkto id="632497091239814660" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Prompt1" type="literal">rac_agent_convo_end.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">end</ap>
      </Properties>
    </node>
    <node type="Action" id="632497091239814660" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="481" y="386">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632497091239814666" treenode="632497091239814667" appnode="632497091239814664" handlerfor="632497091239814663">
    <node type="Start" id="632497091239814666" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="79" y="388">
      <linkto id="632497091239814669" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632497091239814669" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="408" y="378">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>