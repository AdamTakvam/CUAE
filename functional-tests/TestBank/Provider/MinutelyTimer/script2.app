<Application name="script2" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script2">
    <outline>
      <treenode type="evh" id="632150328113281389" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150328113281387" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150328113281386" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.MinutelyTimer.script2</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632150328113281388" treenode="632150328113281389" appnode="632150328113281387" handlerfor="632150328113281386">
    <node type="Start" id="632150328113281388" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632150328113281403" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632150328113281403" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="358" y="328">
      <linkto id="632224855787500336" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerId" type="literal">Minutely_Timer</ap>
      </Properties>
    </node>
    <node type="Action" id="632224855787500336" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="570" y="330">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
