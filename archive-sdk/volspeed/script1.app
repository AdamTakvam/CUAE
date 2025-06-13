<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632799289548649813" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632799289548649810" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632799289548649809" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632799289548649826" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632799289548649823" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632799289548649822" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632799602599781223" actid="632799289548649832" />
          <ref id="632799602599781232" actid="632799289548649851" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632799289548649831" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632799289548649828" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632799289548649827" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632799602599781224" actid="632799289548649832" />
          <ref id="632799602599781233" actid="632799289548649851" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632799289548649839" level="2" text="Metreos.MediaControl.GatherDigits_Complete: OnGatherDigits_Complete">
        <node type="function" name="OnGatherDigits_Complete" id="632799289548649836" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Complete" id="632799289548649835" path="Metreos.MediaControl.GatherDigits_Complete" />
        <references>
          <ref id="632799602599781226" actid="632799289548649845" />
          <ref id="632799602599781240" actid="632799289548649864" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632799289548649844" level="2" text="Metreos.MediaControl.GatherDigits_Failed: OnGatherDigits_Failed">
        <node type="function" name="OnGatherDigits_Failed" id="632799289548649841" path="Metreos.StockTools" />
        <node type="event" name="GatherDigits_Failed" id="632799289548649840" path="Metreos.MediaControl.GatherDigits_Failed" />
        <references>
          <ref id="632799602599781227" actid="632799289548649845" />
          <ref id="632799602599781241" actid="632799289548649864" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632799289548649859" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632799289548649856" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632799289548649855" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632799602599781215" vid="632799289548649814">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="g_connectionId" id="632799602599781217" vid="632799289548649816">
        <Properties type="String">g_connectionId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632799289548649812" treenode="632799289548649813" appnode="632799289548649810" handlerfor="632799289548649809">
    <node type="Start" id="632799289548649812" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="133">
      <linkto id="632799289548649818" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632799289548649818" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="164" y="133">
      <linkto id="632799289548649820" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632799289548649832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <ap name="WaitForMedia" type="literal">true</ap>
        <rd field="CallId">g_callId</rd>
        <rd field="ConnectionId">g_connectionId</rd>
      </Properties>
    </node>
    <node type="Action" id="632799289548649820" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="164" y="257">
      <linkto id="632799289548649821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649821" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="164" y="385">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632799289548649832" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="306" y="117" mx="359" my="133">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632799289548649826" />
        <item text="OnPlay_Failed" treenode="632799289548649831" />
      </items>
      <linkto id="632799289548649845" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649845" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="481" y="117" mx="555" my="133">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632799289548649839" />
        <item text="OnGatherDigits_Failed" treenode="632799289548649844" />
      </items>
      <linkto id="632799289548649848" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649848" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="744" y="133">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632799289548649819" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632799289548649825" treenode="632799289548649826" appnode="632799289548649823" handlerfor="632799289548649822">
    <node type="Start" id="632799289548649825" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="99">
      <linkto id="632799289548649851" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632799289548649851" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="180" y="82" mx="233" my="98">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632799289548649826" />
        <item text="OnPlay_Failed" treenode="632799289548649831" />
      </items>
      <linkto id="632799289548649854" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Prompt1" type="literal">sample_music.wav</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649854" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="445" y="98">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632799289548649830" treenode="632799289548649831" appnode="632799289548649828" handlerfor="632799289548649827">
    <node type="Start" id="632799289548649830" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="107">
      <linkto id="632799289548649849" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632799289548649849" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="176" y="107">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Complete" activetab="true" startnode="632799289548649838" treenode="632799289548649839" appnode="632799289548649836" handlerfor="632799289548649835">
    <node type="Start" id="632799289548649838" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="43" y="83">
      <linkto id="632799289548649868" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632799289548649863" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="304" y="131">
      <linkto id="632799289548649869" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">relative</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Speed" type="literal">-2</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649864" name="GatherDigits" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="231" y="197" mx="305" my="213">
      <items count="2">
        <item text="OnGatherDigits_Complete" treenode="632799289548649839" />
        <item text="OnGatherDigits_Failed" treenode="632799289548649844" />
      </items>
      <linkto id="632799289548649867" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="TermCondMaxDigits" type="literal">1</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649867" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="300" y="428">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632799289548649868" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="155" y="81">
      <linkto id="632799289548649863" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632799289548649869" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">digits == "1"</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649869" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="469" y="83">
      <linkto id="632799289548649864" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632799289548649870" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">digits == "2"</ap>
      </Properties>
    </node>
    <node type="Action" id="632799289548649870" name="AdjustPlay" class="MaxActionNode" group="" path="Metreos.MediaControl" x="612" y="83">
      <linkto id="632799289548649864" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="AdjustmentType" type="literal">relative</ap>
        <ap name="ConnectionId" type="variable">g_connectionId</ap>
        <ap name="Volume" type="literal">-2</ap>
      </Properties>
    </node>
    <node type="Variable" id="632799289548649861" name="digits" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Digits" refType="reference" name="Metreos.MediaControl.GatherDigits_Complete">digits</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnGatherDigits_Failed" startnode="632799289548649843" treenode="632799289548649844" appnode="632799289548649841" handlerfor="632799289548649840">
    <node type="Start" id="632799289548649843" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="65" y="106">
      <linkto id="632799289548649850" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632799289548649850" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="255" y="106">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632799289548649858" treenode="632799289548649859" appnode="632799289548649856" handlerfor="632799289548649855">
    <node type="Start" id="632799289548649858" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="129">
      <linkto id="632799289548649860" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632799289548649860" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="215" y="129">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>