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
      <treenode type="evh" id="632413167845296142" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632413167845296139" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632413167845296138" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632610810100479249" actid="632413167845296148" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632413167845296147" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632413167845296144" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632413167845296143" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632610810100479250" actid="632413167845296148" />
        </references>
        <Properties type="asyncCallback">
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
    </outline>
    <variables>
      <treenode text="g_CallId" id="632610810100479243" vid="632413169871914689">
        <Properties type="String">g_CallId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632364804899375199" treenode="632364804899375200" appnode="632364804899375197" handlerfor="632364804899375196">
    <node type="Start" id="632364804899375199" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="38" y="362">
      <linkto id="632413169871914688" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632364804899375220" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="575" y="561">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632380359791875196" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="378" y="363">
      <linkto id="632364804899375220" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632380359791875199" type="Labeled" style="Bezier" ortho="true" label="never2" />
      <linkto id="632413167845296148" type="Labeled" style="Bezier" ortho="true" label="never" />
      <linkto id="632413169871914685" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="ConnectionId">connectionId</rd>
        <log condition="failure" on="true" level="Error" type="literal">AnswerCall Failed</log>
      </Properties>
    </node>
    <node type="Action" id="632380359791875199" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="738" y="363">
      <linkto id="632364804899375220" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <log condition="failure" on="true" level="Error" type="literal">Hangup failed</log>
      </Properties>
    </node>
    <node type="Action" id="632413167845296148" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="515" y="216" mx="568" my="232">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632413167845296142" />
        <item text="OnPlay_Failed" treenode="632413167845296147" />
      </items>
      <linkto id="632380359791875199" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <linkto id="632413169871914685" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="ConnectionId" type="variable">connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
        <log condition="failure" on="true" level="Warning" type="literal">"Play failed"</log>
      </Properties>
    </node>
    <node type="Action" id="632413169871914685" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="378" y="230">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632413169871914688" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="152" y="363">
      <linkto id="632610810100479270" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_CallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632610810100479270" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="243" y="363">
      <linkto id="632380359791875196" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632610810100479272" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">callId == null || callId == String.Empty || callId == "0"</ap>
      </Properties>
    </node>
    <node type="Action" id="632610810100479271" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="244" y="565">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632610810100479272" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="244" y="460">
      <linkto id="632610810100479271" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">"Invalid call ID: " + callId == null ? "&lt;null&gt;" : callId</ap>
        <ap name="LogLevel" type="literal">Error</ap>
      </Properties>
    </node>
    <node type="Variable" id="632380359791875198" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference">callId</Properties>
    </node>
    <node type="Variable" id="632413169871914684" name="connectionId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">connectionId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632413167845296141" treenode="632413167845296142" appnode="632413167845296139" handlerfor="632413167845296138">
    <node type="Start" id="632413167845296141" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="34">
      <linkto id="632413169871914686" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632413169871914686" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="334" y="130">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632413167845296146" treenode="632413167845296147" appnode="632413167845296144" handlerfor="632413167845296143">
    <node type="Start" id="632413167845296146" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632413169871914691" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632413169871914691" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="151" y="103">
      <linkto id="632413169871914692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_CallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632413169871914692" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="306" y="102">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632465032096586870" treenode="632465032096586871" appnode="632465032096586868" handlerfor="632465032096586867">
    <node type="Start" id="632465032096586870" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="148">
      <linkto id="632465032096586872" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632465032096586872" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="147">
      <Properties final="true" type="appControl" log="On">
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
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>