<Application name="script1" trigger="Metreos.CallControl.IncomingCall" version="0.8" single="false" grid="true">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632934875050471426" level="1" text="Metreos.CallControl.IncomingCall (trigger): OnIncomingCall">
        <node type="function" name="OnIncomingCall" id="632934875050471423" path="Metreos.StockTools" />
        <node type="event" name="IncomingCall" id="632934875050471422" path="Metreos.CallControl.IncomingCall" trigger="true" />
        <Properties type="triggering">
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnIncomingCall" activetab="true" startnode="632934875050471425" treenode="632934875050471426" appnode="632934875050471423" handlerfor="632934875050471422">
    <node type="Start" id="632934875050471425" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="40" y="416" />
  </canvas>
  <Properties desc="">
  </Properties>
</Application>