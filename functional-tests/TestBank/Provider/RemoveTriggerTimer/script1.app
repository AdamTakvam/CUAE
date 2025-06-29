<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632150277987187685" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632150277987187683" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632150277987187682" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.RemoveTriggerTimer.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632150277987187684" treenode="632150277987187685" appnode="632150277987187683" handlerfor="632150277987187682">
    <node type="Start" id="632150277987187684" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="367">
      <linkto id="632471912136003542" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224881663907498" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="566" y="365">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632471912136003542" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="352" y="368">
      <linkto id="632224881663907498" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now</ap>
        <ap name="timerRecurrenceInterval" type="csharp">System.TimeSpan.FromMilliseconds(5000)</ap>
        <ap name="timerUserData" type="literal">onlyTimer</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
