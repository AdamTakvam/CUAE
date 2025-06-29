<Application name="IncomingCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IncomingCall">
    <outline>
      <treenode type="evh" id="632491670134395881" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632491670134395878" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632491670134395877" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="To" type="literal">4330</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632491670134395892" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632491670134395889" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632491670134395888" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632497091239815723" actid="632491670134395898" />
          <ref id="632497091239815730" actid="632491670134395909" />
          <ref id="632497091239815764" actid="632491670134396028" />
          <ref id="632497091239815779" actid="632491670134396045" />
          <ref id="632497091239815787" actid="632491670134396057" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632491670134395897" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632491670134395894" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632491670134395893" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632497091239815724" actid="632491670134395898" />
          <ref id="632497091239815731" actid="632491670134395909" />
          <ref id="632497091239815765" actid="632491670134396028" />
          <ref id="632497091239815780" actid="632491670134396045" />
          <ref id="632497091239815788" actid="632491670134396057" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632491670134395916" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632491670134395913" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632491670134395912" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632497091239815744" actid="632497025201910723" />
          <ref id="632497091239815782" actid="632491670134396046" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632491670134395921" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632491670134395918" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632491670134395917" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632497091239815745" actid="632497025201910723" />
          <ref id="632497091239815783" actid="632491670134396046" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632491670134395926" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632491670134395923" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632491670134395922" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632497091239815746" actid="632497025201910723" />
          <ref id="632497091239815784" actid="632491670134396046" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632491670134395997" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632491670134395994" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632491670134395993" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632497091239815734" actid="632491670134396003" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632491670134396002" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632491670134395999" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632491670134395998" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632497091239815735" actid="632491670134396003" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_incomingCallId" id="632497091239815707" vid="632491670134395883">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_incomingConnectionId" id="632497091239815709" vid="632491670134395901">
        <Properties type="String">g_incomingConnectionId</Properties>
      </treenode>
      <treenode text="g_confId" id="632497091239815711" vid="632491670134395903">
        <Properties type="String">g_confId</Properties>
      </treenode>
      <treenode text="g_callOut" id="632497091239815713" vid="632491670134395988">
        <Properties type="String" initWith="CallOutNumber">g_callOut</Properties>
      </treenode>
      <treenode text="g_outgoingCallId" id="632497091239815715" vid="632491670134395990">
        <Properties type="String">g_outgoingCallId</Properties>
      </treenode>
      <treenode text="g_outgoingConnectionId" id="632497091239815717" vid="632491670134396010">
        <Properties type="String">g_outgoingConnectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632491670134395880" treenode="632491670134395881" appnode="632491670134395878" handlerfor="632491670134395877">
    <node type="Start" id="632491670134395880" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632491670134395885" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134395885" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="224" y="360">
      <linkto id="632491670134395886" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632491670134395898" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="literal">0</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <rd field="ConnectionId">g_incomingConnectionId</rd>
        <rd field="ConferenceId">g_confId</rd>
      </Properties>
    </node>
    <node type="Action" id="632491670134395886" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="224" y="472">
      <linkto id="632491670134395887" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134395887" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="224" y="584">
      <Properties final="true" type="appControl">
        <log condition="entry" on="true" level="Info" type="literal">Call rejected</log>
      </Properties>
    </node>
    <node type="Action" id="632491670134395898" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="400" y="344" mx="453" my="360">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632491670134395892" />
        <item text="OnPlay_Failed" treenode="632491670134395897" />
      </items>
      <linkto id="632491670134396056" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondDigit" type="literal">1</ap>
        <ap name="Prompt1" type="literal">rac_welcome.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">welcome</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396056" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616" y="360">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632491670134395882" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632491670134395891" treenode="632491670134395892" appnode="632491670134395889" handlerfor="632491670134395888">
    <node type="Start" id="632491670134395891" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632491670134395908" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134395908" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="200" y="360">
      <linkto id="632491670134395909" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632491670134396009" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134395909" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="152" y="504" mx="205" my="520">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632491670134395892" />
        <item text="OnPlay_Failed" treenode="632491670134395897" />
      </items>
      <linkto id="632497025201910720" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Prompt1" type="literal">rac_please_wait.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">wait</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134395992" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="790" y="519">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632491670134396003" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="128" y="56" mx="202" my="72">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632491670134395997" />
        <item text="OnGatherDigits_Failed" treenode="632491670134396002" />
      </items>
      <linkto id="632491670134396006" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="TermCondMaxTime" type="literal">7000</ap>
        <ap name="TermCondDigit" type="literal">1</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396006" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="424" y="72">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632491670134396009" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="200" y="240">
      <linkto id="632491670134396012" type="Labeled" style="Bezier" ortho="true" label="wait" />
      <linkto id="632491670134396035" type="Labeled" style="Bezier" ortho="true" label="exit" />
      <linkto id="632491670134396063" type="Labeled" style="Bezier" ortho="true" label="welcome" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">userData</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396012" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="480" y="240">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632491670134396035" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="352" y="352">
      <linkto id="632491670134396036" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396036" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="512" y="352">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632491670134396063" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="34" y="164">
      <linkto id="632491670134396003" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SleepTime" type="literal">1000</ap>
      </Properties>
    </node>
    <node type="Action" id="632497025201910720" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="442" y="670">
      <linkto id="632497091239814168" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="literal">select callto from extensiontable;</ap>
        <ap name="Name" type="literal">RemoteAgentConcept</ap>
        <rd field="ResultSet">data</rd>
      </Properties>
    </node>
    <node type="Action" id="632497025201910723" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="725" y="657" mx="791" my="673">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632491670134395916" />
        <item text="OnMakeCall_Failed" treenode="632491670134395921" />
        <item text="OnRemoteHangup" treenode="632491670134395926" />
      </items>
      <linkto id="632491670134395992" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="To" type="csharp">data.Rows[0][0]</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outgoingCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632497091239814168" name="ExecuteCommand" class="MaxActionNode" group="" path="Metreos.Native.Database" x="697.2129" y="671">
      <linkto id="632497025201910723" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Command" type="csharp">"update extensiontable set conferenceid='" + g_confId + "'"</ap>
        <ap name="Name" type="literal">RemoteAgentConcept</ap>
      </Properties>
    </node>
    <node type="Variable" id="632491670134395905" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.Play_Complete">terminationCondition</Properties>
    </node>
    <node type="Variable" id="632491670134395906" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference">connectionId</Properties>
    </node>
    <node type="Variable" id="632491670134395907" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="UserData" refType="reference">userData</Properties>
    </node>
    <node type="Variable" id="632497025201910721" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632497025201910722" name="data" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">data</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632491670134395896" treenode="632491670134395897" appnode="632491670134395894" handlerfor="632491670134395893">
    <node type="Start" id="632491670134395896" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="240">
      <linkto id="632491670134396013" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134396013" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="342" y="253">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632491670134395915" treenode="632491670134395916" appnode="632491670134395913" handlerfor="632491670134395912">
    <node type="Start" id="632491670134395915" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="368">
      <linkto id="632491670134396014" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134396014" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="280" y="368">
      <linkto id="632491670134396020" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396020" name="JoinConference" class="MaxActionNode" group="" path="Metreos.MediaControl" x="480" y="368">
      <linkto id="632491670134396025" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">connection</ap>
        <ap name="ConferenceId" type="variable">g_confId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396025" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="640" y="368">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632491670134396023" name="connection" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="ConnectionId" refType="reference" name="Metreos.CallControl.MakeCall_Complete">connection</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632491670134395920" treenode="632491670134395921" appnode="632491670134395918" handlerfor="632491670134395917">
    <node type="Start" id="632491670134395920" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="256">
      <linkto id="632491670134396027" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134396026" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="696" y="264">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632491670134396027" name="StopMediaOperation" class="MaxActionNode" group="" path="Metreos.MediaControl" x="238" y="256">
      <linkto id="632491670134396028" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396028" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="416" y="240" mx="469" my="256">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632491670134395892" />
        <item text="OnPlay_Failed" treenode="632491670134395897" />
      </items>
      <linkto id="632491670134396026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Prompt1" type="literal">rac_unable_connect.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">exit</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" activetab="true" startnode="632491670134395925" treenode="632491670134395926" appnode="632491670134395923" handlerfor="632491670134395922">
    <node type="Start" id="632491670134395925" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="344">
      <linkto id="632491670134396037" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134396037" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="232" y="344">
      <linkto id="632491670134396039" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632491670134396040" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">callId == g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396039" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="416" y="344">
      <linkto id="632491670134396041" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632497091239815584" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">g_outgoingCallId != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396040" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="232" y="464">
      <linkto id="632497091239815584" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396041" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="542" y="342">
      <linkto id="632497091239815584" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="CallId" type="variable">g_outgoingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396042" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="776.878967" y="482">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632497091239814581" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="662" y="481">
      <linkto id="632491670134396042" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <ap name="ToGuid" type="csharp">data.Rows[0][0]</ap>
        <log condition="exit" on="true" level="Info" type="literal">Sent event</log>
      </Properties>
    </node>
    <node type="Action" id="632497091239815584" name="ExecuteQuery" class="MaxActionNode" group="" path="Metreos.Native.Database" x="526.821" y="483">
      <linkto id="632497091239814581" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Query" type="literal">select appinstanceid from extensiontable</ap>
        <ap name="Name" type="literal">RemoteAgentConcept</ap>
        <rd field="ResultSet">data</rd>
      </Properties>
    </node>
    <node type="Variable" id="632491670134396038" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
    <node type="Variable" id="632497091239814579" name="data" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">data</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" startnode="632491670134395996" treenode="632491670134395997" appnode="632491670134395994" handlerfor="632491670134395993">
    <node type="Start" id="632491670134395996" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="344">
      <linkto id="632491670134396044" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134396044" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="232" y="344">
      <linkto id="632491670134396045" type="Labeled" style="Bezier" ortho="true" label="digit" />
      <linkto id="632491670134396057" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SwitchOn" type="variable">terminationCondition</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396045" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="184" y="472" mx="237" my="488">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632491670134395892" />
        <item text="OnPlay_Failed" treenode="632491670134395897" />
      </items>
      <linkto id="632491670134396046" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="Prompt1" type="literal">rac_please_wait.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">wait</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396046" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="408" y="472" mx="474" my="488">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632491670134395916" />
        <item text="OnMakeCall_Failed" treenode="632491670134395921" />
        <item text="OnRemoteHangup" treenode="632491670134395926" />
      </items>
      <linkto id="632491670134396047" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="To" type="variable">g_callOut</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outgoingCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632491670134396047" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="672" y="488">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632491670134396057" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="416" y="328" mx="469" my="344">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632491670134395892" />
        <item text="OnPlay_Failed" treenode="632491670134395897" />
      </items>
      <linkto id="632491670134396061" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="TermCondDigit" type="literal">1</ap>
        <ap name="Prompt1" type="literal">rac_welcome.wav</ap>
        <ap name="ConnectionId" type="variable">g_incomingConnectionId</ap>
        <ap name="UserData" type="literal">welcome</ap>
      </Properties>
    </node>
    <node type="Action" id="632491670134396061" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="672" y="344">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Variable" id="632491670134396043" name="terminationCondition" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TerminationCondition" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">terminationCondition</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632491670134396001" treenode="632491670134396002" appnode="632491670134395999" handlerfor="632491670134395998">
    <node type="Start" id="632491670134396001" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632491670134396062" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491670134396062" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="590" y="280">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>