<Application name="BROQ_answerCall" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="BROQ_answerCall">
    <outline>
      <treenode type="evh" id="632732619173444322" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632732619173444319" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632732619173444318" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" startnode="632732619173444321" treenode="632732619173444322" appnode="632732619173444319" handlerfor="632732619173444318">
    <node type="Start" id="632732619173444321" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32" />
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>