<Application name="Main" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Main">
    <outline>
      <treenode type="evh" id="632572177549892984" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632572177549892981" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632572177549892980" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632572177549892983" treenode="632572177549892984" appnode="632572177549892981" handlerfor="632572177549892980">
    <node type="Start" id="632572177549892983" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632572177549892985" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632572177549892985" name="RejectCall" class="MaxActionNode" group="" path="Metreos.CallControl" x="288" y="142">
      <linkto id="632572177549892986" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="CallId" type="variable">callId</ap>
      </Properties>
    </node>
    <node type="Action" id="632572177549892986" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="520" y="212">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632572177549893055" name="callId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="CallId" refType="reference" name="Metreos.CallControl.IncomingCall">callId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>