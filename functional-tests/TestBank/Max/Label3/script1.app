<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632146032163593914" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146012209687651" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146012209687650" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Max.Label3.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632225746388750168" vid="632146012209687659">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="667" startnode="632146012209687652" treenode="632146032163593914" appnode="632146012209687651" handlerfor="632146012209687650">
    <node type="Start" id="632146012209687652" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="192">
      <linkto id="632146012209687697" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632146012209687657" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="378" y="194">
      <linkto id="632146012209687658" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146012209687658" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="510" y="194">
      <linkto id="632224881663907951" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Label" id="632146012209687697" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="239" y="192" />
    <node type="Variable" id="632146012209687698" name="switchOn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.Int" initWith="switchOn" defaultInitWith="3" refType="reference">switchOn</Properties>
    </node>
    <node type="Action" id="632224881663907951" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="640" y="196">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
