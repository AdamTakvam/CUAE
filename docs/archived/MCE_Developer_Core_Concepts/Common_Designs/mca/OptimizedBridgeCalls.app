<Application name="OptimizedBridgeCalls" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="OptimizedBridgeCalls">
    <outline>
      <treenode type="evh" id="632722533371894262" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632722533371894259" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632722533371894258" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632722533371894272" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632722533371894269" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632722533371894268" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632724962137272727" actid="632722533371894278" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632722533371894277" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632722533371894274" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632722533371894273" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632724962137272728" actid="632722533371894278" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632724359405628642" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632724359405628639" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632724359405628638" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632724962137272733" actid="632724359405628653" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632724359405628647" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632724359405628644" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632724359405628643" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632724962137272734" actid="632724359405628653" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632724359405628652" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632724359405628649" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632724359405628648" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632724962137272735" actid="632724359405628653" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_incomingConnId" id="632724962137272717" vid="632722533371894265">
        <Properties type="String">g_incomingConnId</Properties>
      </treenode>
      <treenode text="g_incomingCallId" id="632724962137272719" vid="632722533371894284">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_outboundCallId" id="632724962137272721" vid="632724359405628657">
        <Properties type="String">g_outboundCallId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632724962137272723" vid="632724962137272677">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632722533371894261" treenode="632722533371894262" appnode="632722533371894259" handlerfor="632722533371894258">
    <node type="Start" id="632722533371894261" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="369">
      <linkto id="632722533371894264" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632722533371894264" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="129" y="369">
      <linkto id="632722533371894278" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">incomingCallId</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <rd field="ConnectionId">g_incomingConnId</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632722533371894278" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="177" y="352" mx="230" my="368">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632722533371894272" />
        <item text="OnPlay_Failed" treenode="632722533371894277" />
      </items>
      <linkto id="632722533371894281" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="Prompt1" type="literal">Streaming audio is easy</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632722533371894281" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="345" y="369">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632722533371894263" name="incomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">incomingCallId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632722533371894271" treenode="632722533371894272" appnode="632722533371894269" handlerfor="632722533371894268">
    <node type="Start" id="632722533371894271" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="370">
      <linkto id="632724359405628653" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632724359405628653" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="72" y="353" mx="138" my="369">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632724359405628642" />
        <item text="OnMakeCall_Failed" treenode="632724359405628647" />
        <item text="OnRemoteHangup" treenode="632724359405628652" />
      </items>
      <linkto id="632724359405628659" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="literal">5000</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outboundCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632724359405628659" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="291" y="370">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632722533371894276" treenode="632722533371894277" appnode="632722533371894274" handlerfor="632722533371894273">
    <node type="Start" id="632722533371894276" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="381">
      <linkto id="632724347332067677" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632724347332067677" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="108.352859" y="381">
      <linkto id="632724347332067678" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632724347332067678" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="198.352844" y="381">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" activetab="true" startnode="632724359405628641" treenode="632724359405628642" appnode="632724359405628639" handlerfor="632724359405628638">
    <node type="Start" id="632724359405628641" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="375">
      <linkto id="632724962137272680" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632724962137272680" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="375">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632724359405628646" treenode="632724359405628647" appnode="632724359405628644" handlerfor="632724359405628643">
    <node type="Start" id="632724359405628646" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="384">
      <linkto id="632724962137272688" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632724962137272688" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="131" y="384">
      <linkto id="632724962137272689" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632724962137272689" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="228" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632724359405628651" treenode="632724359405628652" appnode="632724359405628649" handlerfor="632724359405628648">
    <node type="Start" id="632724359405628651" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="34" y="384">
      <linkto id="632724962137272683" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632724962137272683" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="137" y="384">
      <linkto id="632724962137272684" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632724962137272685" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632724962137272684" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="272" y="313">
      <linkto id="632724962137272687" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outboundCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632724962137272685" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="271" y="442">
      <linkto id="632724962137272687" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632724962137272687" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="392" y="382">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632724962137272682" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>