<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632517209727139127" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632517209727139124" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632517209727139123" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632517209727139152" level="2" text="Metreos.CallControl.RemoteHangup: OnRemoteHangup">
        <node type="function" name="OnRemoteHangup" id="632517209727139149" path="Metreos.StockTools" />
        <node type="event" name="RemoteHangup" id="632517209727139148" path="Metreos.CallControl.RemoteHangup" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632520303397145536" level="2" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632520303397145533" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632520303397145532" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_AnsweredCall" id="632520303397145515" vid="632517209727139136">
        <Properties type="String" initWith="S_AnsweredCall">S_AnsweredCall</Properties>
      </treenode>
      <treenode text="S_Hangup" id="632520303397145517" vid="632517209727139138">
        <Properties type="String" initWith="S_Hangup">S_Hangup</Properties>
      </treenode>
      <treenode text="g_callId" id="632520303397145539" vid="632520303397145538">
        <Properties type="String">g_callId</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632517209727139126" treenode="632517209727139127" appnode="632517209727139124" handlerfor="632517209727139123">
    <node type="Start" id="632517209727139126" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="342">
      <linkto id="632517209727139141" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632517209727139141" name="AnswerCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="307" y="343">
      <linkto id="632517209727139142" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632517209727139143" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
        <rd field="CallId">g_callId</rd>
      </Properties>
    </node>
    <node type="Action" id="632517209727139142" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="488" y="342">
      <linkto id="632517209727139146" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="csharp">true</ap>
        <ap name="signalName" type="variable">S_AnsweredCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632517209727139143" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="309" y="509">
      <linkto id="632517209727139147" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="success" type="csharp">false</ap>
        <ap name="signalName" type="variable">S_AnsweredCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632517209727139146" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="670" y="344">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632517209727139147" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="311" y="626">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632517209727139140" name="from" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="From" refType="reference" name="Metreos.CallControl.IncomingCall">from</Properties>
    </node>
    <node type="Variable" id="632517209727139145" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnRemoteHangup" startnode="632517209727139151" treenode="632517209727139152" appnode="632517209727139149" handlerfor="632517209727139148">
    <node type="Start" id="632517209727139151" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="51" y="361">
      <linkto id="632517209727139153" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632517209727139153" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="280" y="360">
      <linkto id="632517209727139154" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Hangup</ap>
      </Properties>
    </node>
    <node type="Action" id="632517209727139154" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="488" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" activetab="true" startnode="632520303397145535" treenode="632520303397145536" appnode="632520303397145533" handlerfor="632520303397145532">
    <node type="Start" id="632520303397145535" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="222">
      <linkto id="632520303397145537" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632520303397145537" name="Hangup" class="MaxActionNode" group="" path="Metreos.CallControl" x="383" y="220">
      <linkto id="632520303397145540" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">g_callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632520303397145540" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="590" y="221">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
