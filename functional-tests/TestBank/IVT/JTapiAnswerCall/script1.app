<Application name="script1" trigger="Metreos.Providers.JTapi.JTapiIncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632526159164677065" level="1" text="Metreos.Providers.JTapi.JTapiIncomingCall (trigger): OnJTapiIncomingCall">
        <node type="function" name="OnJTapiIncomingCall" id="632526159164677062" path="Metreos.StockTools" />
        <node type="event" name="JTapiIncomingCall" id="632526159164677061" path="Metreos.Providers.JTapi.JTapiIncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632526159164677097" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632526159164677094" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632526159164677093" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">IVT.JTapiAnswerCall.script1.E_Hangup</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_callId" id="632526221257774412" vid="632526159164677068">
        <Properties type="String">g_callId</Properties>
      </treenode>
      <treenode text="S_AnswerCallComplete" id="632526221257774414" vid="632526159164677087">
        <Properties type="String" initWith="S_AnswerCallComplete">S_AnswerCallComplete</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnJTapiIncomingCall" activetab="true" startnode="632526159164677064" treenode="632526159164677065" appnode="632526159164677062" handlerfor="632526159164677061">
    <node type="Start" id="632526159164677064" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="336">
      <linkto id="632526159164677070" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526159164677066" name="JTapiAnswerCall" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="280" y="336">
      <linkto id="632526159164677071" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632526159164677090" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632526159164677070" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="136" y="336">
      <linkto id="632526159164677066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">callId</ap>
        <rd field="ResultData">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632526159164677071" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="392" y="336">
      <linkto id="632526159164677092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_AnswerCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632526159164677090" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="280" y="448">
      <linkto id="632526159164677092" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_AnswerCallComplete</ap>
      </Properties>
    </node>
    <node type="Action" id="632526159164677092" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="392" y="448">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632526159164677067" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.Providers.JTapi.JTapiIncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632526159164677096" treenode="632526159164677097" appnode="632526159164677094" handlerfor="632526159164677093">
    <node type="Start" id="632526159164677096" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="344">
      <linkto id="632526221257774423" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632526221257774423" name="JTapiHangup" class="MaxActionNode" group="" path="Metreos.Providers.JTapi" x="273" y="341">
      <linkto id="632526221257774424" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632526221257774424" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="487" y="338">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
