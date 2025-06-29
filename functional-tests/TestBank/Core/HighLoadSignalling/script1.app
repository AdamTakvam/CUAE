<Application name="script1" trigger="Metreos.Providers.FunctionalTest.TriggerApp" version="0.6" single="false">
  <!--//// app tree ////-->
  <global name="script1">
    <outline>
      <treenode type="evh" id="632145137236406374" level="1" text="TriggerApp (trigger): OnTriggerApp">
        <node type="function" name="OnTriggerApp" id="632145128990625123" path="Metreos.StockTools" />
        <node type="event" name="TriggerApp" id="632145128990625122" path="Metreos.Providers.FunctionalTest.TriggerApp" trigger="true" />
        <Properties type="triggering">
          <ep name="testScriptName" type="literal">Core.HighLoadSignalling.script1</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="S_Simple" id="632224814145469356" vid="632145128990625126">
        <Properties type="Metreos.Types.String" initWith="S_Simple">S_Simple</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnTriggerApp" varsy="667" startnode="632145128990625124" treenode="632145137236406374" appnode="632145128990625123" handlerfor="632145128990625122">
    <node type="Loop" id="632145128990625128" name="Loop" text="loop (var)" cx="297" cy="246" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="198" y="43" mx="346" my="166">
      <linkto id="632145128990625130" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632224814145469362" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="variable">signalAmount</Properties>
    </node>
    <node type="Start" id="632145128990625124" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="95">
      <linkto id="632145128990625128" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Variable" id="632145128990625129" name="signalAmount" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="17" y="685">
      <Properties type="Metreos.Types.Int" initWith="signalAmount" refType="reference">signalAmount</Properties>
    </node>
    <node type="Action" id="632145128990625130" name="Signal" container="632145128990625128" class="MaxActionNode" group="" path="Metreos.Providers.FunctionalTest" x="351" y="151">
      <linkto id="632145128990625128" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="signalName" type="variable">S_Simple</ap>
      </Properties>
    </node>
    <node type="Action" id="632224814145469362" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="627" y="153">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
</Application>
