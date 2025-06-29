<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632488286155223784" level="1" text="Metreos.Providers.FunctionalTest.TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146305770000137" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146305770000136" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">ARE.EventQueuing.script1</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632488286155223785" level="1" text="Metreos.Providers.FunctionalTest.Event: OnEvent">
        <node type="function" name="OnEvent" id="632146305770000143" path="Metreos.StockTools" />
        <node type="event" name="Event" id="632146305770000142" path="Metreos.Providers.FunctionalTest.Event" />
        <references />
        <Properties type="nonTriggering">
          <ep name="uniqueEventParam" type="literal">ARE.EventQueuing.script1.E_QueuedEvent</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Trigger" id="632488286576500729" vid="632146305770000147">
        <Properties type="Metreos.Types.String" initWith="S_Trigger">S_Trigger</Properties>
      </treenode>
      <treenode text="S_QueuedEvent" id="632488286576500731" vid="632146305770000149">
        <Properties type="Metreos.Types.String" initWith="S_QueuedEvent">S_QueuedEvent</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" startnode="632146305770000138" treenode="632488286155223784" appnode="632146305770000137" handlerfor="632146305770000136">
    <node type="Start" id="632146305770000138" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="52" y="198">
      <linkto id="632146305770000170" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146305770000146" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="431" y="196">
      <linkto id="632224796097968929" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632146305770000170" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="178" y="196">
      <linkto id="632488286576500744" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Trigger</ap>
      </Properties>
    </node>
    <node type="Action" id="632224796097968929" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="579" y="194">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632488286576500744" name="Sleep" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="302" y="204">
      <linkto id="632146305770000146" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="SleepTime" type="literal">10000</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnEvent" startnode="632146305770000144" treenode="632488286155223785" appnode="632146305770000143" handlerfor="632146305770000142">
    <node type="Start" id="632146305770000144" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632146305770000151" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146305770000151" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="268" y="158">
      <linkto id="632224796097968928" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_QueuedEvent</ap>
      </Properties>
    </node>
    <node type="Action" id="632224796097968928" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="600" y="156">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>
