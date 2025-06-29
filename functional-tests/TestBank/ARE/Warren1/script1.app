<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632520303397145547" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632520303397145544" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632520303397145543" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750758362195657" level="2" text="Metreos.MediaControl.Play_Complete: OnPlay_Complete">
        <node type="function" name="OnPlay_Complete" id="632750758362195654" path="Metreos.StockTools" />
        <node type="event" name="Play_Complete" id="632750758362195653" path="Metreos.MediaControl.Play_Complete" />
        <references>
          <ref id="632751022365024617" actid="632751022365024612" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750758362195662" level="2" text="Metreos.MediaControl.Play_Failed: OnPlay_Failed">
        <node type="function" name="OnPlay_Failed" id="632750758362195659" path="Metreos.StockTools" />
        <node type="event" name="Play_Failed" id="632750758362195658" path="Metreos.MediaControl.Play_Failed" />
        <references>
          <ref id="632751022365024618" actid="632751022365024612" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750758362195671" level="2" text="Metreos.CallControl.MakeCall_Complete: OnMakeCall_Complete">
        <node type="function" name="OnMakeCall_Complete" id="632750758362195668" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Complete" id="632750758362195667" path="Metreos.CallControl.MakeCall_Complete" />
        <references>
          <ref id="632751022365024589" actid="632750758362195682" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750758362195676" level="2" text="Metreos.CallControl.MakeCall_Failed: OnMakeCall_Failed">
        <node type="function" name="OnMakeCall_Failed" id="632750758362195673" path="Metreos.StockTools" />
        <node type="event" name="MakeCall_Failed" id="632750758362195672" path="Metreos.CallControl.MakeCall_Failed" />
        <references>
          <ref id="632751022365024590" actid="632750758362195682" />
        </references>
        <Properties type="asyncCallback">
        </Properties>
      </treenode>
      <treenode type="evh" id="632750758362195681" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632750758362195678" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632750758362195677" path="Metreos.CallControl.RemoteHangup" />
        <references>
          <ref id="632751022365024591" actid="632750758362195682" />
        </references>
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632751022297996675" level="2" text="Metreos.Providers.FunctionalTest.Event: OnAnswerCall">
        <node type="function" name="OnAnswerCall" id="632751022297996674" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632751022297996673" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.Warren1.script1.E_AnswerCall</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_incomingCallId" id="632751022365024560" vid="632750758362195643">
        <Properties type="String">g_incomingCallId</Properties>
      </treenode>
      <treenode text="g_incomingConnId" id="632751022365024562" vid="632750758362195645">
        <Properties type="String">g_incomingConnId</Properties>
      </treenode>
      <treenode text="g_outboundCallId" id="632751022365024564" vid="632750758362195647">
        <Properties type="String">g_outboundCallId</Properties>
      </treenode>
      <treenode text="g_conferenceId" id="632751022365024566" vid="632750758362195649">
        <Properties type="String">g_conferenceId</Properties>
      </treenode>
      <treenode text="g_from" id="632751022365024568" vid="632750758362195651">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="S_HangUp" id="632751022365024570" vid="632750773820039642">
        <Properties type="String" initWith="S_HangUp">S_HangUp</Properties>
      </treenode>
      <treenode text="S_Failure" id="632751022365024572" vid="632750774935997242">
        <Properties type="String" initWith="S_Failure">S_Failure</Properties>
      </treenode>
      <treenode text="S_ReceiveCall" id="632751022365024574" vid="632751022297996672">
        <Properties type="String" initWith="S_ReceiveCall">S_ReceiveCall</Properties>
      </treenode>
      <treenode text="g_to" id="632751022365024622" vid="632751022365024621">
        <Properties type="String">g_to</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632520303397145546" treenode="632520303397145547" appnode="632520303397145544" handlerfor="632520303397145543">
    <node type="Start" id="632520303397145546" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="40">
      <linkto id="632751022365024623" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632751016614600571" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="299" y="61">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632751016614600572" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="183.610016" y="66">
      <linkto id="632751016614600571" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_ReceiveCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632751022365024623" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="89.04883" y="124">
      <linkto id="632751016614600572" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">from</ap>
        <ap name="Value2" type="variable">incomingCallId</ap>
        <rd field="ResultData">g_from</rd>
        <rd field="ResultData2">g_incomingCallId</rd>
      </Properties>
    </node>
    <node type="Variable" id="632750954803710330" name="incomingCallId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">incomingCallId</Properties>
    </node>
    <node type="Variable" id="632750954803710331" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Complete" startnode="632750758362195656" treenode="632750758362195657" appnode="632750758362195654" handlerfor="632750758362195653">
    <node type="Start" id="632750758362195656" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632750758362195682" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750758362195682" name="MakeCall" class="MaxAsyncActionNode" group="" path="Metreos.CallControl" x="168" y="32" mx="234" my="48">
      <items count="3">
        <item text="OnMakeCall_Complete" treenode="632750758362195671" />
        <item text="OnMakeCall_Failed" treenode="632750758362195676" />
        <item text="OnRemoteHangup" treenode="632750758362195681" />
      </items>
      <linkto id="632750758362195686" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="To" type="literal">4426</ap>
        <ap name="From" type="variable">g_from</ap>
        <ap name="Conference" type="literal">true</ap>
        <ap name="ConferenceId" type="variable">g_conferenceId</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="CallId">g_outboundCallId</rd>
      </Properties>
    </node>
    <node type="Action" id="632750758362195686" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="555" y="60">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnPlay_Failed" startnode="632750758362195661" treenode="632750758362195662" appnode="632750758362195659" handlerfor="632750758362195658">
    <node type="Start" id="632750758362195661" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632750758362195687" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750758362195687" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="198" y="50">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Complete" startnode="632750758362195670" treenode="632750758362195671" appnode="632750758362195668" handlerfor="632750758362195667">
    <node type="Start" id="632750758362195670" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632750758362195688" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750758362195688" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="226" y="63">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnMakeCall_Failed" startnode="632750758362195675" treenode="632750758362195676" appnode="632750758362195673" handlerfor="632750758362195672">
    <node type="Start" id="632750758362195675" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632750758362195689" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750758362195689" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="127.352859" y="34">
      <linkto id="632750954803710333" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632750758362195690" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="460.352844" y="163">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632750954803710333" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="281.610016" y="77">
      <linkto id="632750758362195690" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_Failure</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632750758362195680" treenode="632750758362195681" appnode="632750758362195678" handlerfor="632750758362195677">
    <node type="Start" id="632750758362195680" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632750758362195693" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632750758362195693" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="137.41861" y="50">
      <linkto id="632750758362195694" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632750758362195695" type="Labeled" style="Bezier" ortho="true" label="unequal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">callId</ap>
        <ap name="Value2" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632750758362195694" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="267.4186" y="32">
      <linkto id="632750774962337968" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_outboundCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632750758362195695" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="251.41861" y="131">
      <linkto id="632750774962337968" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
      </Properties>
    </node>
    <node type="Action" id="632750774962337968" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="387.610016" y="93">
      <linkto id="632750774962337969" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="signalName" type="variable">S_HangUp</ap>
      </Properties>
    </node>
    <node type="Action" id="632750774962337969" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="530.61" y="94">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632750954803710332" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.RemoteHangup">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnAnswerCall" activetab="true" startnode="632751022297996676" treenode="632751022297996675" appnode="632751022297996674" handlerfor="632751022297996673">
    <node type="Start" id="632751022297996676" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="395">
      <linkto id="632751022365024610" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632751022365024610" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="70.79231" y="68">
      <linkto id="632751022365024611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_incomingCallId</ap>
        <ap name="Conference" type="literal">true</ap>
        <rd field="CallId">g_incomingCallId</rd>
        <rd field="ConnectionId">g_incomingConnId</rd>
        <rd field="ConferenceId">g_conferenceId</rd>
      </Properties>
    </node>
    <node type="Action" id="632751022365024611" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="197.7923" y="250">
      <linkto id="632751022365024612" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">to</ap>
        <rd field="ResultData">g_to</rd>
      </Properties>
    </node>
    <node type="Action" id="632751022365024612" name="Play" class="MaxAsyncActionNode" group="" path="Metreos.MediaControl" x="285" y="233" mx="338" my="249">
      <items count="2">
        <item text="OnPlay_Complete" treenode="632750758362195657" />
        <item text="OnPlay_Failed" treenode="632750758362195662" />
      </items>
      <linkto id="632751022365024613" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="ConnectionId" type="variable">g_incomingConnId</ap>
        <ap name="Prompt1" type="literal">welcome.wav</ap>
        <ap name="UserData" type="literal">none</ap>
      </Properties>
    </node>
    <node type="Action" id="632751022365024613" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="501" y="257">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632751022365024620" name="to" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">to</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>