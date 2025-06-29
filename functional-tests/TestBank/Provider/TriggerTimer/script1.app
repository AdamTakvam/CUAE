<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632149760897031378" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632149760897031376" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632149760897031375" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Provider.TriggerTimer.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables />
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" startnode="632149760897031377" treenode="632149760897031378" appnode="632149760897031376" handlerfor="632149760897031375">
    <node type="Start" id="632149760897031377" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="354">
      <linkto id="632472808604236182" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632224881663907691" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="427" y="349">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632472808604236182" name="AddTriggerTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="181" y="351">
      <linkto id="632224881663907691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now</ap>
        <ap name="timerUserData" type="literal">onlyTimer</ap>
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
