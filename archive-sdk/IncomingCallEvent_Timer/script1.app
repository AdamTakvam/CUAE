<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632364804899375200" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632364804899375197" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632364804899375196" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632465032096586871" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632465032096586868" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632465032096586867" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632465052598863019" level="2" text="Metreos.CallControl.StartTx: OnStartTx">
        <node type="function" name="OnStartTx" id="632465052598863016" path="Metreos.StockTools" />
        <node type="event" name="StartTx" id="632465052598863015" path="Metreos.CallControl.StartTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632465052598863024" level="2" text="Metreos.CallControl.StartRx: OnStartRx">
        <node type="function" name="OnStartRx" id="632465052598863021" path="Metreos.StockTools" />
        <node type="event" name="StartRx" id="632465052598863020" path="Metreos.CallControl.StartRx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632491616152334398" level="2" text="Metreos.CallControl.StopTx: OnStopTx">
        <node type="function" name="OnStopTx" id="632491616152334395" path="Metreos.StockTools" />
        <node type="event" name="StopTx" id="632491616152334394" path="Metreos.CallControl.StopTx" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632603920418183092" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632603920418183089" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632603920418183088" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632604159806026567" actid="632603920418183093" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_CallId" id="632604159806026556" vid="632413169871914689">
        <Properties type="String">g_CallId</Properties>
      </treenode>
      <treenode text="g_TimerId" id="632604159806026558" vid="632603920418183095">
        <Properties type="String">g_TimerId</Properties>
      </treenode>
      <treenode text="g_TimerTime" id="632604159806026560" vid="632603920418183175">
        <Properties type="Int" defaultInitWith="0" initWith="HangupTime">g_TimerTime</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632364804899375199" treenode="632364804899375200" appnode="632364804899375197" handlerfor="632364804899375196">
    <node type="Start" id="632364804899375199" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="178">
      <linkto id="632413169871914688" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632364804899375220" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="492" y="305">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Warning" type="csharp">"OnIncomingCall: Failed to answer call with callId '" + callId + "' Exiting Script."</log>
      </Properties>
    </node>
    <node type="Action" id="632380359791875196" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="491" y="178">
      <linkto id="632364804899375220" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632413169871914685" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="failure" on="true" level="Error" type="literal">AnswerCall Failed</log>
      </Properties>
    </node>
    <node type="Action" id="632413169871914685" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="636" y="178">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632413169871914688" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="149" y="178">
      <linkto id="632603920418183177" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632603920418183093" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="191" y="278" mx="253" my="294">
      <items count="1">
        <item text="OnTimerFire" treenode="632603920418183092" />
      </items>
      <linkto id="632380359791875196" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.Add(new TimeSpan(0, 0, 0, g_TimerTime))</ap>
        <ap name="timerRecurrenceInterval" type="literal">0</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">g_TimerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632603920418183177" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="249" y="178">
      <linkto id="632603920418183093" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632380359791875196" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerTime &gt; 0</ap>
      </Properties>
    </node>
    <node type="Variable" id="632380359791875198" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632413169871914684" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632465032096586870" treenode="632465032096586871" appnode="632465032096586868" handlerfor="632465032096586867">
    <node type="Start" id="632465032096586870" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="148">
      <linkto id="632603920418183294" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632465032096586872" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="410" y="148">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632603920418183097" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="141" y="267">
      <linkto id="632465032096586872" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632603920418183294" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="140" y="148">
      <linkto id="632603920418183097" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632465032096586872" type="Labeled" style="Bezier" ortho="true" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_TimerTime &gt; 0</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartTx" startnode="632465052598863018" treenode="632465052598863019" appnode="632465052598863016" handlerfor="632465052598863015">
    <node type="Start" id="632465052598863018" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632465052598863025" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632465052598863025" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="145" y="64">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStartRx" startnode="632465052598863023" treenode="632465052598863024" appnode="632465052598863021" handlerfor="632465052598863020">
    <node type="Start" id="632465052598863023" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632465052598863026" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632465052598863026" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="180" y="103">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnStopTx" startnode="632491616152334397" treenode="632491616152334398" appnode="632491616152334395" handlerfor="632491616152334394">
    <node type="Start" id="632491616152334397" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="114">
      <linkto id="632491616152334399" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632491616152334399" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="153" y="122">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" activetab="true" startnode="632603920418183091" treenode="632603920418183092" appnode="632603920418183089" handlerfor="632603920418183088">
    <node type="Start" id="632603920418183091" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="179">
      <linkto id="632603920418183296" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632603920418183098" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="275" y="179">
      <linkto id="632603920418183099" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">g_TimerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632603920418183099" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="408" y="179">
      <linkto id="632603920418183100" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632603920418183100" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="532" y="179">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632603920418183296" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="140" y="179">
      <linkto id="632603920418183098" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">0</ap>
        <rd field="ResultData">g_TimerTime</rd>
        <log condition="entry" on="true" level="Warning" type="csharp">"OnTimerFire: Timer fired for call with callId '" + g_CallId + "'"
</log>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>