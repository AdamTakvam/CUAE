<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632146103040000171" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146103040000169" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146103040000168" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">SMA.SMA138.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224881663907869" vid="632146103040000172">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632146103040000170" treenode="632146103040000171" appnode="632146103040000169" handlerfor="632146103040000168">
    <node type="Start" id="632146103040000170" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632146103040000174" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146103040000174" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="302" y="165">
      <linkto id="632224881663907874" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224881663907874" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="501" y="195">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
