<Application name="SomeScript" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SomeScript">
    <outline>
      <treenode type="evh" id="632676742491470229" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632676742491470226" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632676742491470225" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
          <ep name="CallId" type="literal">42</ep>
          <ep name="From" type="literal">1</ep>
          <ep name="To" type="literal">5</ep>
          <ep name="OriginalTo" type="literal">3</ep>
          <ep name="DisplayName" type="literal">tim</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632676742491470228" treenode="632676742491470229" appnode="632676742491470226" handlerfor="632676742491470225">
    <node type="Start" id="632676742491470228" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="42" y="89">
      <linkto id="632676742491470238" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632676742491470238" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="130" y="89">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>