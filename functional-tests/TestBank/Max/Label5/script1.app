<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632146032163593951" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632146012209687651" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632146012209687650" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Max.Label5.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224881663907997" vid="632146012209687659">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" activetab="true" varsy="692" startnode="632146012209687652" treenode="632146032163593951" appnode="632146012209687651" handlerfor="632146012209687650">
    <node type="Start" id="632146012209687652" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="56" y="192">
      <linkto id="632146012209687662" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632146012209687656" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="258" y="192" />
    <node type="Label" id="632146012209687657" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="320" y="194">
      <linkto id="632146032163593947" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632146012209687658" name="Signal" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="510" y="194">
      <linkto id="632224881663908009" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632146012209687662" name="Write" class="MaxActionNode" group="" path="Metreos.Native.Log" x="170" y="191">
      <linkto id="632146012209687656" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Message" type="literal">Stub for first action before label.</ap>
        <ap name="LogLevel" type="literal">Verbose</ap>
      </Properties>
    </node>
    <node type="Label" id="632146032163593947" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="319" y="276" />
    <node type="Label" id="632146032163593948" text="B" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="319" y="339">
      <linkto id="632146032163593963" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632146032163593962" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="435" y="194">
      <linkto id="632146012209687658" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632146032163593963" text="C" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="319" y="410" />
    <node type="Action" id="632224881663908009" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="632" y="194">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
